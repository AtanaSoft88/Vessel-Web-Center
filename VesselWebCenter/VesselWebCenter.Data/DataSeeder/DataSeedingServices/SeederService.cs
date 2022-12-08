using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.DataSeeder.Contracts;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;

namespace VesselWebCenter.Data.DataSeeder.DataSeedingServices
{
    public class SeederService : ISeederService
    {
        private readonly IRepository repo;

        public SeederService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task SeedManningCompanies(ManningCompany[] companies)
        {
            foreach (var compJson in companies)
            {
                var company = new ManningCompany()
                {
                    Name = compJson.Name,
                    Country = compJson.Country,
                };
                await repo.AddAsync<ManningCompany>(company);
            }
            await repo.SaveChangesAsync();
        }
        public async Task SeedVessels(Vessel[] vessels)
        {
            foreach (var vsl in vessels)
            {
                var companyId1 = await repo.GetByIdAsync<ManningCompany>(vsl.ManningCompanyId);

                if (companyId1 != null)
                {
                    var vessel = new Vessel()
                    {
                        Name = vsl.Name,
                        CallSign = vsl.CallSign,
                        BreadthMax = vsl.BreadthMax,
                        IsLaden = vsl.IsLaden,
                        LengthOverall = vsl.LengthOverall,
                        VesselType = vsl.VesselType,
                        CargoTypeOnBoard = vsl.CargoTypeOnBoard,
                        VesselImageUrl = vsl.VesselImageUrl,
                        ManningCompanyId = companyId1.Id,
                    };
                    await repo.AddAsync(vessel);
                }
            }
            await repo.SaveChangesAsync();
        }

        public async Task SeedPortsOfCall(PortOfCall[] ports)
        {
            var allVesselsCount = await repo.AllReadonly<Vessel>().CountAsync();
            for (int i = 1; i <= allVesselsCount; i++)
            {
                var vessel = await repo.GetByIdAsync<Vessel>(i);
                if (vessel!=null)
                {
                    var first = 0;
                    var last = ports.Length;
                    var randomPortIndex = new Random().Next(first, last);
                    vessel.PortsOfCall.Add(ports[randomPortIndex]);
                }
            }            
            await repo.SaveChangesAsync();
        }

        public async Task SeedCrewMembers(CrewMember[] crewMembers)
        {
            foreach (var member in crewMembers)
            {
                var crewMember = new CrewMember()
                {
                    Nationality = member.Nationality,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Age = member.Age,
                    IsPartOfACrew = member.IsPartOfACrew,
                    VesselId = member.VesselId,
                };
                await repo.AddAsync(crewMember);
            }
            await repo.SaveChangesAsync();
        }

        public async Task SeedDestinationPorts(DestinationPort[] destinationPorts)
        {
            foreach (var dp in destinationPorts)
            {
                var port = new DestinationPort()
                {
                    PortName = dp.PortName,
                    Country = dp.Country,
                    Latitude = dp.Latitude,
                    Longitude = dp.Longitude,
                    UNLocode = dp.UNLocode,
                };
                await repo.AddAsync(port);
            }
            await repo.SaveChangesAsync();
        }
    }
}