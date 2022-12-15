using MockQueryable.Moq;
using VesselWebCenter.Tests.DataPopulation;

namespace VesselWebCenter.Tests
{
    public class PortOfDestinationServiceTests : DataPopulator
    {
        private IRepository repo;
        private IPortOfDestinationService service;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_All_Available_Vessels_With_More_Than_15_Crew_On_Board()
        {            
            var mockRepo = new Mock<IRepository>();
            var portsOfCall = VesselPopulator(new List<Vessel>());
            var initialVesselsCount = portsOfCall.Count();
            mockRepo.Setup(x => x.AllReadonly<Vessel>()).Returns(portsOfCall.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);            
            var result = await service.GetAllAvailableForVoyage();
            var vesselsCoveredVoyageRequirements = result.Count();
            Assert.That(vesselsCoveredVoyageRequirements,Is.LessThan(initialVesselsCount));
            Assert.That(result.Count() == 4);
        }

        [Test]
        public async Task Get_10_Most_Visited_Ports()
        {
            
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
