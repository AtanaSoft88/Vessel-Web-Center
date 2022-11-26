using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.DataSeeder.Contracts;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;

namespace VesselWebCenter.Data.DataSeeder
{
    public class DbApplicationSeeder
    {
        public async Task SeedDataBaseAsync(IRepository repository, ISeederService seederService)
        {
            if (await repository.AllReadonly<ManningCompany>().CountAsync() == 0)
            {
                string jsonString = File.ReadAllText(GlobalConstants.COMPANIES_SEEDING);
                var companiesJsonInput = JsonConvert.DeserializeObject<ManningCompany[]>(jsonString);
                try
                {
                    await seederService.SeedManningCompanies(companiesJsonInput);
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("ManningCompanies could not be saved properly into Data Base", ex);
                }
            }
            if (await repository.AllReadonly<Vessel>().CountAsync() == 0)
            {
                string jsonString = File.ReadAllText(GlobalConstants.VESSELS_SEEDING);
                var vesselsJsonInput = JsonConvert.DeserializeObject<Vessel[]>(jsonString);
                try
                {
                    await seederService.SeedVessels(vesselsJsonInput);
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Vessels could not be saved properly into Data Base", ex);
                }

            }
            if (await repository.AllReadonly<PortOfCall>().CountAsync() == 0)
            {
                string jsonString = File.ReadAllText(GlobalConstants.PORTS_OF_CALL_SEEDING);
                var portsJsonInput = JsonConvert.DeserializeObject<PortOfCall[]>(jsonString);

                try
                {
                    await seederService.SeedPortsOfCall(portsJsonInput);
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("PortsOfCall could not be saved properly into Data Base", ex);
                }

            }
            if (await repository.AllReadonly<CrewMember>().CountAsync() == 0)
            {
                string jsonString = File.ReadAllText(GlobalConstants.CREW_MEMBERS_SEEDING);
                var crewMembersJsonInput = JsonConvert.DeserializeObject<CrewMember[]>(jsonString);

                try
                {
                    await seederService.SeedCrewMembers(crewMembersJsonInput);
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("CrewMembers could not be saved properly into Data Base", ex);
                }

            }
            if (await repository.AllReadonly<DestinationPort>().CountAsync() == 0)
            {
                string jsonString = File.ReadAllText(GlobalConstants.DESTINATION_PORTS_SEEDING);
                var destinationsJsonInput = JsonConvert.DeserializeObject<DestinationPort[]>(jsonString);

                try
                {
                    await seederService.SeedDestinationPorts(destinationsJsonInput);
                }
                catch (Exception ex)
                {

                    throw new ApplicationException("DestinationPorts could not be saved properly into Data Base", ex);
                }

            }
        }
    }
}

