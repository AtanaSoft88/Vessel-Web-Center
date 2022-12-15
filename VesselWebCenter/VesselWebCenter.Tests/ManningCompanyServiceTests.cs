using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using VesselWebCenter.Tests.DataPopulation;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    public class ManningCompanyServiceTests : DataPopulator
    {
        private IRepository repo;
        private IManningCompanyService service;        

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_All_Manning_Companies()
        {
            var repoMock = new Mock<IRepository>();
            var companies = CompanyPopulator(new List<ManningCompany>());
            repoMock.Setup(x => x.AllReadonly<ManningCompany>())
                .Returns(companies.AsQueryable().BuildMock());
            repo = repoMock.Object;
            service = new ManningCompanyService(repo);
            var resultService = await service.GetAllCompanies();
            Assert.That(resultService.Count(), Is.EqualTo(companies.Count()));
        }

        [Test]
        public async Task Get_All_Vessels_Part_Of_A_Given_Company()
        { 
            var givenCompanyId = 3;
            var companyName = "BestCompanyEver";
            var fakeDb = DataBaseMock.Instance;
            await fakeDb.AddRangeAsync(CompanyPopulator(new List<ManningCompany>()));
            await fakeDb.SaveChangesAsync();
            repo = new Repository(fakeDb);
            service = new ManningCompanyService(repo);
            var serviceResult =await service.GetVessels(givenCompanyId);
            Assert.That(serviceResult.Count(), Is.EqualTo(2));
            Assert.That(serviceResult.Select(x=>x.CompanyName).Contains(companyName));
            Assert.IsTrue(serviceResult.All(x=>x.ManningCompanyId==givenCompanyId));
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
