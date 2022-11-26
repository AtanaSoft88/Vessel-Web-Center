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
            foreach (var port in ports)
            {
                var portOfCall = new PortOfCall()
                {
                    Latitude = port.Latitude,
                    Longitude = port.Longitude,
                    Country = port.Country,
                    PortName = port.PortName,
                    UNLocode = port.UNLocode,
                };
                var randomNumberVessels = new Random().Next(1, 15);

                var vesselMinId = 1;
                var vesselMaxId = repo.AllReadonly<Vessel>().Count() + 1;

                for (int i = 0; i < randomNumberVessels; i++)
                {
                    var vesselRandomId = new Random().Next(vesselMinId, vesselMaxId);
                    var vesselVisitedPort = await repo.GetByIdAsync<Vessel>(vesselRandomId);
                    if (vesselVisitedPort != null)
                    {
                        portOfCall.Vessels.Add(vesselVisitedPort);
                        await repo.AddAsync(portOfCall);

                        if (!vesselVisitedPort.PortsOfCall.Contains(portOfCall))
                        {
                            vesselVisitedPort.PortsOfCall.Add(portOfCall);
                            repo.Update<Vessel>(vesselVisitedPort);

                        }

                    }
                }
                await repo.AddAsync(portOfCall);
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
                    Latitude =dp.Latitude,
                    Longitude=dp.Longitude,
                    UNLocode =dp.UNLocode,                   
                };
               await repo.AddAsync(port);
            }
            await repo.SaveChangesAsync();
        }
    }
}