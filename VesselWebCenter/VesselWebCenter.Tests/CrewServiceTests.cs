
using Microsoft.Extensions.Logging;

namespace VesselWebCenter.Tests
{
    [TestFixture]
    public class CrewServiceTests
    {
        private IRepository repo;
        private ICrewService service;
        private ILogger<CrewMember> logger;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Crew_Members_Get_All()
        {
            IQueryable<CrewMember> members = new List<CrewMember>()
            {
                new CrewMember {Id=1 , FirstName = "Joro" },
                new CrewMember {Id=2 , FirstName = "Misho" },
                new CrewMember {Id=3 , FirstName = "Kiril"},
                new CrewMember {Id=4 , FirstName = "Gena"},
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
