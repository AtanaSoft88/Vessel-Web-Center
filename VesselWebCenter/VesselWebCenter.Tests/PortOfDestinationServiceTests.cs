using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using VesselWebCenter.Data.Models;
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
            var vessels = VesselPopulator(new List<Vessel>());
            var initialVesselsCount = vessels.Count();
            mockRepo.Setup(x => x.AllReadonly<Vessel>()).Returns(vessels.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);
            var result = await service.GetAllAvailableForVoyage();
            var vesselsCoveredVoyageRequirements = result.Count();
            Assert.That(vesselsCoveredVoyageRequirements, Is.LessThan(initialVesselsCount));
            Assert.That(result.Count() == 4);
        }

        [Test]
        [TestCase("1 25.30 N 35.40 E")]
        [TestCase("2 25.30 N 35.40 E")]
        [TestCase("3 25.30 N 35.40 E")]
        [TestCase("4 25.30 N 35.40 E")]
        [TestCase("5 25.30 N 35.40 E")]
        [TestCase("15 25.30 N 35.40 E")]
        public async Task Get_The_Chosen_Destination_For_A_Given_Vessel(string testParams)
        {
            var currentVesselId = int.Parse(testParams.Split(" ").ToArray().First());
            var mockRepo = new Mock<IRepository>();
            var destinations = PortOfDestinationPopulator(new List<DestinationPort>());
            var vessels = VesselPopulator(new List<Vessel>());
            mockRepo.Setup(x => x.AllReadonly<DestinationPort>()).Returns(destinations.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AllReadonly<Vessel>()).Returns(vessels.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);
            var result = await service.GetDestinationPort(testParams);
            if (currentVesselId > vessels.Count())
            {
                Assert.That(result, Is.Null);
            }
            else
            {
                Assert.That(result, Is.Not.Null);
            }

        }

        [Test]
        [TestCase(15), Order(1)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]

        public async Task Get_The_Appropriate_Parameters_For_Carying_Out_A_Voyage(int vesselId)
        {
            var mockRepo = new Mock<IRepository>();
            List<Vessel>? vessels = VesselPopulator(new List<Vessel>());
            var testedLat = "emptyLat";
            var testedPortName = "emptyPortName";
            var testedUnlocode = "emptyUnlocode";
            var testedLong = "emptyLong";
            if (vessels.Select(x => x.Id).Contains(vesselId))
            {
                testedLat = vessels.First(x => x.Id == vesselId).PortsOfCall.Select(x => x.Latitude).Last();
                testedPortName = vessels.First(x => x.Id == vesselId).PortsOfCall.Select(x => x.PortName).Last();
                testedUnlocode = vessels.First(x => x.Id == vesselId).PortsOfCall.Select(x => x.UNLocode).Last();
                testedLong = vessels.First(x => x.Id == vesselId).PortsOfCall.Select(x => x.Longitude).Last();
            }
            mockRepo.Setup(x => x.AllReadonly<Vessel>()).Returns(vessels.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);
            var inputParams = $"Port: {testedPortName} Lat: {testedLat} N Long: {testedLong} E Country: Bulgaria Locode: {testedUnlocode} Lat: 43.11 N Long: 27.55 E ";
            var result = await service.GetCoordinates(inputParams, vesselId);
            if (vesselId == 2 || vesselId > vessels.Count())
            {
                Assert.That(result, Is.Null);
            }
            else
            {                
                Assert.That(result.Count() == 7);
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.TypeOf(typeof(List<string>)));
            }



        }
        [Test]
        [TestCase(15, 18)]
        [TestCase(1, 5.8)]
        [TestCase(2, 7)]
        [TestCase(3, 6.6)]
        [TestCase(4, 1)]
        [TestCase(5, 18)]

        public async Task Prepare_Coordinates_For_The_Voyage(int vesselId, double spd)
        {
            var parameters = new List<string> { "Sevastopol", "44.35", "33.30", "41.01", "28.59", "Bulgaria", "UASVP" };
            var destinations = PortOfDestinationPopulator(new List<DestinationPort>());
            List<Vessel>? vessels = VesselPopulator(new List<Vessel>());
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.AllReadonly<Vessel>()).Returns(vessels.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.AllReadonly<DestinationPort>()).Returns(destinations.AsQueryable().BuildMock());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);
            var result = await service.GetDataForCalculation(parameters, spd, vesselId);
            if (vesselId > vessels.Count())
            {
                Assert.That(result, Is.Null);
            }
            else
            {
                Assert.IsNotNull(result);
                Assert.That(result.LastPortName, Is.Not.EqualTo(result.DestPortName));
                Assert.That(result.CalculatedDistance, Is.GreaterThan(0));
            }
        }

        [Test]
        [TestCase(1, 2, 300)]
        [TestCase(2, 4, 500)]
        [TestCase(3, 8, 400)]
        [TestCase(4, 14, 100)]
        [TestCase(5, 15, 1000)]
        public async Task Set_Destination_To_Vessel_Should_Pass_Properly(int vesselId, int destinationId, double distanceSailed)
        {
            double? initialTotalDistance = 0.0;
            int? initialPortsCount = 0;
            var destinations = PortOfDestinationPopulator(new List<DestinationPort>());
            List<Vessel>? vessels = VesselPopulator(new List<Vessel>());
            var currentVessel = vessels.FirstOrDefault(x => x.Id == vesselId);
            if (currentVessel!=null)
            {
               initialTotalDistance = currentVessel.Distances.Sum(x=>x.VesselDistance);
               initialPortsCount = currentVessel.PortsOfCall.Count();
            }
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.All<Vessel>()).Returns(vessels.AsQueryable().BuildMock());
            mockRepo.Setup(x => x.GetByIdAsync<DestinationPort>(destinationId)).Returns(destinations.BuildMock().FirstOrDefaultAsync());
            repo = mockRepo.Object;
            service = new PortOfDestinationService(repo);
            await service.AddDestinationToVessel(vesselId, destinationId, distanceSailed);
            var finalTotalDistance = currentVessel.Distances.Sum(x => x.VesselDistance);
            var finalPortsCount = currentVessel.PortsOfCall.Count();
            Assert.That(finalTotalDistance, Is.GreaterThan(initialTotalDistance));
            Assert.That(finalPortsCount, Is.GreaterThan(initialPortsCount));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}


