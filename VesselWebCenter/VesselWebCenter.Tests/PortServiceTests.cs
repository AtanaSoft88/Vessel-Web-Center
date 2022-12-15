using MockQueryable.Moq;
using VesselWebCenter.Tests.DataPopulation;
namespace VesselWebCenter.Tests
{
    public class PortServiceTests : DataPopulator
    {
        private IRepository repo;
        private IPortService service;        

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_Most_Visited_Ports()
        {            
            var mostVisitedPortName = "Varna";
            var mostVisitedPortVesselsCount = 4;
            var leastVisited = 1;
            var mockRepo = new Mock<IRepository>();
            var portsOfCall = PortOfCallPopulator(new List<PortOfCall>());
            mockRepo.Setup(x => x.AllReadonly<PortOfCall>()).Returns(portsOfCall.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortService(repo);
            var result = await service.GetMostVisitedPorts();
            Assert.That(result.Count() == 15);
            Assert.That(result.First().PortName == mostVisitedPortName);           
            Assert.That(result.First().TotalVesselsVisited == mostVisitedPortVesselsCount);           
            Assert.That(result.Last().TotalVesselsVisited == leastVisited);
        }

        [Test]
        public async Task Get_10_Most_Visited_Ports()
        {
            var mostVisitedPortName = "Varna";
            var mostVisitedPortVesselsCount = 4;
            var leastVisited = 1;
            var mockRepo = new Mock<IRepository>();
            var portsOfCall = PortOfCallPopulator(new List<PortOfCall>());
            mockRepo.Setup(x => x.AllReadonly<PortOfCall>()).Returns(portsOfCall.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortService(repo);
            var result = await service.Get10MostVisitedPorts();
            Assert.That(result.Count() == 10);
            Assert.That(result.First().PortName == mostVisitedPortName);
            Assert.That(result.First().TotalVesselsVisited == mostVisitedPortVesselsCount);
            Assert.That(result.Last().TotalVesselsVisited == leastVisited);
        }

            [TearDown]
        public void TearDown()
        {

        }
    }
}
