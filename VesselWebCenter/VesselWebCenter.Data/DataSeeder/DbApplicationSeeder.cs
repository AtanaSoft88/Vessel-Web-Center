using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Data.DataSeeder
{
    public class DbApplicationSeeder
    {
        public async Task SeedDataBaseAsync(VesselAppDbContext context)
        {
            if (context.ManningCompanies.Count() == 0)
            {
                string jsonString = File.ReadAllText("bin\\Debug\\net6.0\\DataSeeder\\DataImportSets\\companyNoId.json");
                var companiesJsonInput = JsonConvert.DeserializeObject<ManningCompany[]>(jsonString);
                foreach (var compJson in companiesJsonInput)
                {
                    var company = new ManningCompany()
                    {
                        Name = compJson.Name,
                        Country = compJson.Country,
                    };
                    context.ManningCompanies.Add(company);
                }
                await context.SaveChangesAsync();
            }
            if (context.Vessels.Count() == 0)
            {
                string jsonString = File.ReadAllText("bin\\Debug\\net6.0\\DataSeeder\\DataImportSets\\vesselsNoId.json");
                var vesselsJsonInput = JsonConvert.DeserializeObject<Vessel[]>(jsonString);
                foreach (var vsl in vesselsJsonInput)
                {
                    //Two ways to relate and find appropriate vessel for a specific company
                    var companyId1 = await context.ManningCompanies.FindAsync(vsl.ManningCompanyId);
                    //var companyId2 = await context.ManningCompanies.FirstOrDefaultAsync(x => x.Id == vsl.ManningCompanyId);
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
                        context.Vessels.Add(vessel);
                    }
                }
                await context.SaveChangesAsync();
            }
            if (context.PortsOfCall.Count() == 0)
            {
                string jsonString = File.ReadAllText("bin\\Debug\\net6.0\\DataSeeder\\DataImportSets\\PortsOfCall.json");
                var portsJsonInput = JsonConvert.DeserializeObject<PortOfCall[]>(jsonString);
                foreach (var port in portsJsonInput)
                {
                    var portOfCall = new PortOfCall()
                    {
                        Latitude = port.Latitude,
                        Longitude = port.Longitude,
                        Country = port.Country,
                        PortName = port.PortName,
                        UNLocode = port.UNLocode,
                    };
                    var randomNumberVessels = new Random().Next(1, 14);

                    var vesselMinId = 1;
                    var vesselMaxId = context.Vessels.Count() + 1;

                    for (int i = 0; i < randomNumberVessels; i++)
                    {
                        var vesselRandomId = new Random().Next(vesselMinId, vesselMaxId);
                        var vesselVisitedPort = await context.Vessels.FindAsync(vesselRandomId);
                        if (vesselVisitedPort != null)
                        {
                            portOfCall.Vessels.Add(vesselVisitedPort);
                            await context.PortsOfCall.AddAsync(portOfCall);

                            if (vesselVisitedPort.PortsOfCall.Contains(portOfCall))
                            {
                                vesselVisitedPort.PortsOfCall.Add(portOfCall);
                                context.Update<Vessel>(vesselVisitedPort);

                            }

                        }
                    }
                    await context.PortsOfCall.AddAsync(portOfCall);


                }
                await context.SaveChangesAsync();
            }
            if (context.CrewMembers.Count() == 0)
            {
                string jsonString = File.ReadAllText("bin\\Debug\\net6.0\\DataSeeder\\DataImportSets\\CrewMembers.json");
                var crewMembersJsonInput = JsonConvert.DeserializeObject<CrewMember[]>(jsonString);
                foreach (var member in crewMembersJsonInput)
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
                    context.CrewMembers.Add(crewMember);
                }
                await context.SaveChangesAsync();

            }
        }
    }    
}

