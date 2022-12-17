using MockQueryable.Moq;
using VesselWebCenter.Tests.DataPopulation;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    public class VesselDataServiceTests : DataPopulator
    {
        private IRepository repo;
        private IVesselDataService service;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_All_Vessels()
        {
            var fakeDb = DataBaseMock.Instance;
            await fakeDb.AddRangeAsync(VesselPopulator(new List<Vessel>()));
            await fakeDb.SaveChangesAsync();
            repo = new Repository(fakeDb);
            service = new VesselDataService(repo);
            var serviceResult = service.GetAll();
            Assert.That(serviceResult.Count(), Is.EqualTo(5));
            fakeDb.Vessels.RemoveRange(fakeDb.Vessels);
            await fakeDb.SaveChangesAsync();
            if (!fakeDb.Vessels.Any())
            {
                serviceResult = service.GetAll();
                Assert.That(serviceResult, Is.Empty);
            }
        }

        [Test]
        public async Task Get_The_Chosen_Vessel()
        {
            int chosenVesselId = 5;
            string expectedVesselName = "Maveric";
            var mockRepo = new Mock<IRepository>();
            var vessels = VesselPopulator(new List<Vessel>());
            mockRepo.Setup(r => r.AllReadonly<Vessel>())
            .Returns(vessels.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new VesselDataService(repo);
            var resultVessel = await service.GetChoosenVessel(chosenVesselId);
            Assert.That(resultVessel.Name, Is.EqualTo(expectedVesselName));
        }

        [Test]
        public async Task Get_All_Vessels_As_HomePage()
        {
            var vesselNameWithoutCargo = "Lena";
            var mockRepo = new Mock<IRepository>();
            var allVessels = VesselPopulator(new List<Vessel>());
            mockRepo.Setup(r => r.AllReadonly<Vessel>())
                .Returns(allVessels.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new VesselDataService(repo);
            var serviceResult = await service.AllEmptyVesselsAsHomePage();
            Assert.That(serviceResult.Count(), Is.EqualTo(1));
            Assert.That(allVessels.Count(), Is.EqualTo(5));
            Assert.That(serviceResult.Select(x => x.Name).Any(), Is.EqualTo(true));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
