using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Data.DataSeeder.Contracts
{
    public interface ISeederService
    {
        Task SeedManningCompanies(ManningCompany[] companies);
        Task SeedVessels(Vessel[] vessels);
        Task SeedPortsOfCall(PortOfCall[] ports);
        Task SeedCrewMembers(CrewMember[] crewMembers);
        Task SeedDestinationPorts(DestinationPort[] destinationPorts);
    }
}
