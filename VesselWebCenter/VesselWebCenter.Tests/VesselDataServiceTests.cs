using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VesselWebCenter.Data.Enums;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Services.ViewModels;
using VesselWebCenter.Tests.Mocks;

namespace VesselWebCenter.Tests
{
    public class VesselDataServiceTests
    {
        private IRepository repo;
        private IVesselDataService service;

        private List<T> VesselPopulator<T>(List<T> model)
           where T : Vessel, new()
        {
            model.AddRange(new List<T>
            {
                new T
                {
                    Id = 1,
                    Name="Verila",
                    LengthOverall=160,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                new T
                {
                    Id = 2,
                    Name="Lena",
                    LengthOverall=190,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = false,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard=null,
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                new T
                {
                    Id = 3,
                    Name="Persian Miracle",
                    LengthOverall=220,
                    BreadthMax = 30,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                },
                 new T
                 {
                    Id = 4,
                    Name="Seven Ocenas",
                    LengthOverall=260,
                    BreadthMax = 20,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                 },
                  new T
                  {
                    Id = 5,
                    Name="Maveric",
                    LengthOverall=190,
                    BreadthMax = 20,
                    CallSign ="xxxx",
                    IsLaden = true,
                    ManningCompanyId=1,
                    VesselImageUrl="",
                    VesselType=0,
                    CargoTypeOnBoard="",                    
                    ManningCompany=new ManningCompany(){Name="NBM",Country="Bg" }
                  }                 

            });
            return model;


        }

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
            var serviceResult =await service.AllEmptyVesselsAsHomePage();
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
