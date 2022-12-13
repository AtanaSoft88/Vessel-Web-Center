
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using NuGet.Packaging;
using VesselWebCenter.Data.Models;
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
                    Age = 18
                },
                new T
                {
                    Id = 2,
                    FirstName = "Martin",
                    LastName = "Kamenov",
                    Nationality = "Bgn",
                    Age = 19
                },
                new T
                {
                    Id = 3,
                    FirstName = "Nikola",
                    LastName = "Cenov",
                    Nationality = "Bgn",
                    Age = 20

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

            Assert.That(result.Count(), Is.EqualTo(3));
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
            bool canAdd = false;

            var modelDublicate = new CrewMemberViewModel()
            {
                FirstName = "Nikola",
                LastName = "Cenov",
                Nationality = "Bgn",
                Age = 20
            };
            canAdd = await service.AddCrewMemberToDataBase(modelDublicate);
            Assert.IsFalse(canAdd);

        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
