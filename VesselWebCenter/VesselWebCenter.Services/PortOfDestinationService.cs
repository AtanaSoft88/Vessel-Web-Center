using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{
    public class PortOfDestinationService : IPortOfDestinationService
    {
        private readonly IRepository repo;

        public PortOfDestinationService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task<IEnumerable<VesselAssignViewModel>> GetAllAvaliableForVoyage()
        {
            var vessels = await repo.AllReadonly<Vessel>().Where(x => x.CrewMembers.Count() >= 15).Select(x => new VesselAssignViewModel
            {
                VesselId = x.Id,
                VesselName = x.Name,
                VesselType = x.VesselType.ToString(),
                LatitudeLastPort = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.Latitude).First(),
                LongitudeLastPort = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.Longitude).First(),
                LastPortName = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.PortName).First(),
            }).ToListAsync();
            return vessels;
        }

        public async Task<DestinationViewModel> GetDestinationPorts(string vesselId)
        {
            var vslId = int.Parse(vesselId.Split(" ")[0]);            
            
            var destinationListItems = repo.AllReadonly<DestinationPort>().Select(x => new SelectListItem 
            { 
                Text = $"Port: {x.PortName} Lat: {x.Latitude} Long: {x.Longitude} Country: {x.Country} Locode: {x.UNLocode}",
                Value = x.Id.ToString(),
            });
            var vessel = await repo.AllReadonly<Vessel>().Include(x=>x.PortsOfCall).Where(x=>x.Id==vslId).FirstOrDefaultAsync();
            var latLP = vessel.PortsOfCall.Select(x=>x.Latitude).Last();
            var lonLP = vessel.PortsOfCall.Select(x=>x.Longitude).Last();

            var destPorts = await repo.AllReadonly<DestinationPort>().Include(x=>x.Vessels).ThenInclude(x=>x.PortsOfCall).Select(x => new DestinationViewModel
            {     
                DestinationId = x.Id,
                VesselName = vessel.Name,                
                VesselType = vessel.VesselType.ToString(),
                LastPortLatitude = latLP,
                LastPortLongitude = lonLP,
                Country = x.Country,
                UNLocode = x.UNLocode,
                DestinationPortLatitude = x.Latitude,
                DestinationPortLongitude = x.Longitude,
                DestinationPorts = destinationListItems.ToList(),
                
            }).FirstOrDefaultAsync();
            
            return destPorts;
        }

        public async Task<IEnumerable<string>> GetCoordinates(string parameters)
        {
            var indexDestPortName = parameters.IndexOf(" ");
            var index = parameters.IndexOf("Lat: ");
            var resultValue = parameters.Substring(index, parameters.Length - index)
                                   .Replace("Lat: ", "")
                                   .Replace(" N Long: ", " ")
                                   .Replace(" E ", " ")
                                   .Replace(" N ", " ")                                   
                                   .Replace(" Country: "," ")
                                   .Replace(" Locode: ", " ");
            var portName = parameters.Substring(indexDestPortName + 1, parameters.Length - (parameters.Length - index + 7));
            var destPortLat = resultValue.Split(" ")[0];
            var destPortLong = resultValue.Split(" ")[1];
            var lastPortLat = resultValue.Split(" ")[4]; 
            var lastPortLong = resultValue.Split(" ")[5];
            var destCountry = resultValue.Split(" ")[2];
            var destUNLocode = resultValue.Split(" ")[3];
            return new List<string>() { portName, destPortLat, destPortLong, lastPortLat, lastPortLong, destCountry, destUNLocode };
        }
    }
}
