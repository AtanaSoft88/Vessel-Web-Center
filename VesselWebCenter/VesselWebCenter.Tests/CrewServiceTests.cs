using MockQueryable.Moq;
using VesselWebCenter.Services.ViewModels;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    [TestFixture]
    public class CrewServiceTests
    {
        private IRepository repo;
        private ICrewService service;
        private List<T> CrewPopulator<T>(List<T> model)
            where T : CrewMember, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    FirstName = "Pesho",
                    LastName = "Goshev",
                    Nationality = "Bgn",
                    Age = 18,
                    IsPartOfACrew = true,
                    VesselId = 5
                },
                new T
                {
                    Id = 2,
                    FirstName = "Stoycho" ,
                    LastName = "Marinov",
                    Age = 34,
                    Nationality = "Bg",
                    IsPartOfACrew = false,
                    VesselId = null

                },
                new T
                {
                    Id = 3,
                    FirstName = "Nikola",
                    LastName = "Cenov",
                    Nationality = "Bgn",
                    Age = 20,
                    IsPartOfACrew = false,
                    VesselId = null
                },
                 new T
                 {
                    Id = 4,
                    FirstName = "Gencho" ,
                    LastName = "Marinov",
                    Age = 54,
                    Nationality = "Bg",
                    IsPartOfACrew = true,                    
                    VesselId = 5
                 },
                  new T
                  {
                    Id = 5,
                    FirstName = "Nikola",
                    LastName = "Cenov",
                    Nationality = "Bgn",
                    Age = 20,
                    IsPartOfACrew = true,
                    VesselId = 5

                  }
            });
            return model;


        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Crew_Members_Get_All()
        {
            using var fakeDatabase = DataBaseMock.Instance;
            await fakeDatabase.AddRangeAsync(CrewPopulator(new List<CrewMember>()));

            await fakeDatabase.SaveChangesAsync();
            repo = new Repository(fakeDatabase);
            service = new CrewService(repo);
            var result = await service.GetAll();

            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task Crew_Members_Count_In_Db_Increased_After_Add()
        {
            using var fakeDatabase = DataBaseMock.Instance;
            List<CrewMember> dbModelList = CrewPopulator<CrewMember>(new List<CrewMember>());
            await fakeDatabase.AddRangeAsync(dbModelList);
            await fakeDatabase.SaveChangesAsync();
            var initialCountCrewMembers = fakeDatabase.CrewMembers.Count();
            var repo = new Repository(fakeDatabase);
            service = new CrewService(repo);

            var model = new CrewMemberViewModel()
            {
                FirstName = "Misho",
                LastName = "Enchev",
                Nationality = "Bgn",
                Age = 19,
            };
            var addToDb = await service.AddCrewMemberToDataBase(model);
            var membersCountAfterSuccessfulAdd = fakeDatabase.CrewMembers.Count();
            Assert.That(initialCountCrewMembers, Is.Not.EqualTo(membersCountAfterSuccessfulAdd));
        }

        [Test]
        public async Task Can_Add_CrewMember_To_DataBase()
        {
            using var fakeDatabase = DataBaseMock.Instance;
            List<CrewMember> dbModelList = CrewPopulator<CrewMember>(new List<CrewMember>());
            await fakeDatabase.AddRangeAsync(dbModelList);
            await fakeDatabase.SaveChangesAsync();
            var repo = new Repository(fakeDatabase);
            service = new CrewService(repo);
            var model = new CrewMemberViewModel()
            {
                FirstName = "Misho",
                LastName = "Enchev",
                Nationality = "Bgn",
                Age = 19,
            };
            bool canAdd = false;
            canAdd = await service.AddCrewMemberToDataBase(model);
            Assert.IsTrue(canAdd);

        }

        [Test]
        public async Task Can_Not_Add_CrewMember_To_DataBase()
        {
            using var fakeDatabase = DataBaseMock.Instance;
            List<CrewMember> dbModelList = CrewPopulator<CrewMember>(new List<CrewMember>());
            await fakeDatabase.AddRangeAsync(dbModelList);
            await fakeDatabase.SaveChangesAsync();

            var repo = new Repository(fakeDatabase);
            service = new CrewService(repo);           

            var modelDublicate = new CrewMemberViewModel()
            {
                FirstName = "Nikola",
                LastName = "Cenov",
                Nationality = "Bgn",
                Age = 20
            };
            bool canAdd = true;
            canAdd = await service.AddCrewMemberToDataBase(modelDublicate);
            Assert.IsFalse(canAdd);

        }

        [Test]
        public async Task Get_ALL_Crew_Members_Not_Part_Of_A_Crew()
        {
            var mockRepo = new Mock<IRepository>();
            var crewAvailable = CrewPopulator(new List<CrewMember>());
            mockRepo.Setup(r => r.AllReadonly<CrewMember>())
            .Returns(crewAvailable.AsQueryable().BuildMock());
            repo = mockRepo.Object;

            service = new CrewService(repo);
            var result = await service.GetAllAvailableCrewMembers();
            Assert.That(result.Any(x => x.Value.Contains("Stoycho") && result.Any(x => x.Text == "2")));
            Assert.That(result.Count().Equals(2));
        }

        [Test]
        public async Task Get_ALL_Crew_Members_Which_Are_Part_Of_A_Specific_Vessel()
        {
            int specificVesselId = 5;
            var mockRepo = new Mock<IRepository>();
            var crewAvailable = CrewPopulator(new List<CrewMember>());
            mockRepo.Setup(r => r.AllReadonly<CrewMember>())
            .Returns(crewAvailable.AsQueryable().BuildMock());
            repo = mockRepo.Object;

            service = new CrewService(repo);
            var result = await service.GetAllCrewMembers(specificVesselId);
            Assert.That(result.Any(x => x.Text.Contains("1") && result.Any(x => x.Text == "4") && result.Any(x => x.Text == "5")));
            Assert.That(result.Count().Equals(3));
        }

        [Test]
        public async Task Add_Crew_Member_To_A_Vessel()
        {
            var chosenVesselId = 5;
            var chosenMemberId = 3;
            var chosenMemberName = "Nikola";
            using var fakeDatabase = DataBaseMock.Instance;
            List<CrewMember> dbCrewModelList = CrewPopulator<CrewMember>(new List<CrewMember>());
            var vessels = new List<Vessel>()
            {  
                new Vessel() 
                { 
                    Id=1, Name = "Vessel1",CallSign="",BreadthMax=30,LengthOverall=150,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>() 
                },
                new Vessel() 
                { 
                    Id=5, Name = "Vessel2",CallSign="",BreadthMax=30,LengthOverall=160,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>()
                },
                new Vessel() 
                { 
                    Id=3, Name = "Vessel3",CallSign="",BreadthMax=30,LengthOverall=170,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>()
                },
                
            };
            await fakeDatabase.AddRangeAsync(dbCrewModelList);
            await fakeDatabase.AddRangeAsync(vessels);
            await fakeDatabase.SaveChangesAsync();
            var vesselToAddCrew = fakeDatabase.Vessels.FirstOrDefault(x => x.Id == chosenVesselId);
            var initialCountCrewMembersOnBoard = vesselToAddCrew?.CrewMembers.Count() ?? 0;
            var repo = new Repository(fakeDatabase);
            service = new CrewService(repo);

            CrewMembersDropDownViewModel model = new CrewMembersDropDownViewModel()
            {
                memberId = chosenMemberId,
                VesselId = chosenVesselId
            };
            await service.AddCrewMemberToVessel(model);
            var crewMembersAfterAddition = vesselToAddCrew?.CrewMembers.Count() ?? 0; 
            Assert.That(initialCountCrewMembersOnBoard, Is.LessThan(crewMembersAfterAddition));
            Assert.That(vesselToAddCrew.CrewMembers.Any(x=>x.Id==chosenMemberId && x.FirstName== chosenMemberName));
        }

        [Test]
        public async Task Remove_Crew_Member_From_A_Vessel()
        {
            var chosenVesselId = 5;
            var chosenMemberId = 1;
            var chosenMemberName = "Pesho";
            using var fakeDatabase = DataBaseMock.Instance;
            List<CrewMember> dbCrewModelList = CrewPopulator<CrewMember>(new List<CrewMember>());
            var vessels = new List<Vessel>()
            {
                new Vessel()
                {
                    Id=1, Name = "Vessel1",CallSign="",BreadthMax=30,LengthOverall=150,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>()
                },
                new Vessel()
                {
                    Id=5, Name = "Vessel2",CallSign="",BreadthMax=30,LengthOverall=160,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>()
                },
                new Vessel()
                {
                    Id=3, Name = "Vessel3",CallSign="",BreadthMax=30,LengthOverall=170,VesselImageUrl="",VesselType=0,CrewMembers=new List<CrewMember>()
                },

            };
            await fakeDatabase.AddRangeAsync(dbCrewModelList);
            await fakeDatabase.AddRangeAsync(vessels);
            await fakeDatabase.SaveChangesAsync();
            var vesselToRemoveCrewFrom = fakeDatabase.Vessels.FirstOrDefault(x => x.Id == chosenVesselId);
            var initialCountCrewMembersOnBoard = vesselToRemoveCrewFrom?.CrewMembers.Count() ?? 0;
            var repo = new Repository(fakeDatabase);
            service = new CrewService(repo);

            CrewMembersDropDownViewModel model = new CrewMembersDropDownViewModel()
            {
                memberId = chosenMemberId,
                VesselId = chosenVesselId
            };
            await service.RemovedCrewMemberFromVessel(model);
            var crewMembersAfterRemoval = vesselToRemoveCrewFrom?.CrewMembers.Count() ?? 0;
            Assert.That(initialCountCrewMembersOnBoard, Is.GreaterThan(crewMembersAfterRemoval));
            Assert.That(vesselToRemoveCrewFrom.CrewMembers.Any(x => x.Id == chosenMemberId && x.FirstName == chosenMemberName), Is.False);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
