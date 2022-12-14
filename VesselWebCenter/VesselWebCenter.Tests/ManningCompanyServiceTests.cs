using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    public class ManningCompanyServiceTests
    {
        private IRepository repo;
        private IManningCompanyService service;
        private List<T> CompanyPopulator<T>(List<T> model)
            where T : ManningCompany, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    Name = "Company1",
                    Country = "Bgn",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 12,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=4,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                new T
                {
                    Id = 2,
                    Name = "Company2",
                    Country = "Ng",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 9,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=4,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                new T
                {
                    Id = 3,
                    Name = "BestCompanyEver",
                    Country = "Bg",
                    Vessels = new List<Vessel>()
                    {
                        new Vessel()
                        {
                            Id = 1,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=3,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        },
                        new Vessel()
                        {
                            Id = 2,
                            Name="Seven Ocenas",
                            LengthOverall=260,
                            BreadthMax = 20,
                            CallSign ="xxxx",
                            IsLaden = true,
                            ManningCompanyId=3,
                            VesselImageUrl="",
                            VesselType=0,
                            CargoTypeOnBoard="",
                        }
                    }
                },
                 new T
                 {
                    Id = 4,
                    Name = "Company4",
                    Country = "Dn",
                    Vessels = new List<Vessel>() 
                    {
                        new Vessel() 
                        { Id = 5,
                            Name = "Seven Ocenas",
                            LengthOverall = 260,
                            BreadthMax = 20,
                            CallSign = "xxxx",
                            IsLaden = true,
                            ManningCompanyId = 4,
                            VesselImageUrl = "", 
                            VesselType = 0, 
                            CargoTypeOnBoard = "" 
                    }   }
                 }
            });
            return model;


        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task Get_All_Manning_Companies()
        {
            var repoMock = new Mock<IRepository>();
            var companies = CompanyPopulator<ManningCompany>(new List<ManningCompany>());
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
