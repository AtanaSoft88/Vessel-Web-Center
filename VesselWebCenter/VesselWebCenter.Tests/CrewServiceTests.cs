using Moq;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services;
using VesselWebCenter.Services.Contracts;

namespace VesselWebCenter.Tests
{
    [TestFixture]
    public class CrewServiceTests
    {
        private IRepository repo;
        private ICrewService service;
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task Crew_Members_Get_All()
        {
            IQueryable<CrewMember> members = new List<CrewMember>()
            { 
                new CrewMember {Id=1 },  
                new CrewMember {Id=2 },  
                new CrewMember {Id=3 },  
                new CrewMember {Id=4 }, 
            }.AsQueryable();

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(c => c.AllReadonly<CrewMember>())
                .Returns(members);
            
            repo = repoMock.Object;
            service = new CrewService(repo);

            var crewMembers = await service.GetAll();

            Assert.That(members.Count(), Is.EqualTo(4));
        }

        [TearDown]
        public void TearDown() 
        {

        }
    }
}
