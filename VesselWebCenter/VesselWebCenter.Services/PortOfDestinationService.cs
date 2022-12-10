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
        public async Task<IEnumerable<VesselAssignViewModel>> GetAllAvailableForVoyage()
        {
            var vessels = await repo.AllReadonly<Vessel>().Where(x => x.CrewMembers.Count() >= 15).Select(x => new VesselAssignViewModel
            {
                VesselId = x.Id,
                VesselName = x.Name,
                VesselType = x.VesselType.ToString(),
                LatitudeLastPort = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.Latitude).First(),
                LongitudeLastPort = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.Longitude).First(),
                LastPortName = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.PortName).First(),
                LastPortCountry = x.PortsOfCall.OrderByDescending(x => x.Id).Select(x => x.Country).First(),
                IsValueAvailable = true
            }).ToListAsync();
            return vessels;
        }

        public async Task<DestinationViewModel> GetDestinationPorts(string vesselParams)
        {
            var vslId = int.Parse(vesselParams.Split(" ")[0]);

            var destinationListItems = repo.AllReadonly<DestinationPort>().Select(x => new SelectListItem
            {
                Text = $"Port: {x.PortName} Lat: {x.Latitude} Long: {x.Longitude} Country: {x.Country} Locode: {x.UNLocode}",
                Value = x.Id.ToString(),
            });
            var vessel = await repo.AllReadonly<Vessel>().Include(x => x.PortsOfCall).Where(x => x.Id == vslId).FirstOrDefaultAsync();
            var latLP = vessel.PortsOfCall.Select(x => x.Latitude).Last();
            var lonLP = vessel.PortsOfCall.Select(x => x.Longitude).Last();
            var lastPortName = vessel.PortsOfCall.Select(x => x.PortName).Last();
            var countryLP = vessel.PortsOfCall.Select(x => x.Country).Last();

            var modelResult = await repo.AllReadonly<DestinationPort>().Include(x => x.Vessels).ThenInclude(x => x.PortsOfCall).Select(x => new DestinationViewModel
            {
                VesselId = vslId,
                DestinationId = x.Id,
                VesselImage = vessel.VesselImageUrl,
                VesselName = vessel.Name,
                VesselType = vessel.VesselType.ToString(),
                LastPortLatitude = latLP,
                LastPortLongitude = lonLP,
                LastPortName = lastPortName,
                LastPortCountry = countryLP,
                DestinationPortCountry = x.Country,
                UNLocode = x.UNLocode,
                DestinationPortLatitude = x.Latitude,
                DestinationPortLongitude = x.Longitude,
                DestinationPorts = destinationListItems.ToList(),

            }).FirstOrDefaultAsync();

            return modelResult;
        }

        public async Task<IEnumerable<string>> GetCoordinates(string parameters, int vslId)
        {
            var indexDestPortName = parameters.IndexOf(" ");
            var index = parameters.IndexOf("Lat: ");
            var resultValue = parameters.Substring(index, parameters.Length - index)
                                   .Replace("Lat: ", "")
                                   .Replace(" N Long: ", " ")
                                   .Replace(" E ", " ")
                                   .Replace(" W ", " ")
                                   .Replace(" N ", " ")
                                   .Replace(" Country: ", " ")
                                   .Replace(" Locode: ", " ");
            var portName = parameters.Substring(indexDestPortName + 1, parameters.Length - (parameters.Length - index + 7));
            var destPortLat = resultValue.Split(" ")[0];
            var destPortLong = resultValue.Split(" ")[1];
            var lastPortLat = resultValue.Split(" ")[4];
            var lastPortLong = resultValue.Split(" ")[5];
            var destCountry = resultValue.Split(" ")[2];
            var destUNLocode = resultValue.Split(" ")[3];

            var vessel = await repo.AllReadonly<Vessel>().Include(x => x.PortsOfCall).Where(x => x.Id == vslId).FirstOrDefaultAsync();
            var lastPortName = vessel.PortsOfCall.Select(x => x.PortName).Last();
            var lastPortUnlocode = vessel.PortsOfCall.Select(x => x.UNLocode).Last();
            if (lastPortName == portName && lastPortUnlocode == destUNLocode && lastPortLat == destPortLat)
            {
                return null;
            }
            return new List<string>() { portName, destPortLat, destPortLong, lastPortLat, lastPortLong, destCountry, destUNLocode };
        }

        public async Task<VoyageDataViewModel> GetDataForCalculation(IEnumerable<string> extractedCoordinates, double spd, int vslId)
        {
            var portName = extractedCoordinates.ToList()[0];
            var destPortLat = extractedCoordinates.ToList()[1];
            var destPortLong = extractedCoordinates.ToList()[2];
            var lastPortLat = extractedCoordinates.ToList()[3];
            var lastPortLong = extractedCoordinates.ToList()[4];
            var destCountry = extractedCoordinates.ToList()[5];
            var destUNLocode = extractedCoordinates.ToList()[6];

            var destinationId = await repo.AllReadonly<DestinationPort>().Where(x => x.PortName == portName).Select(x => x.Id).FirstOrDefaultAsync();
            var currentVessel = await repo.AllReadonly<Vessel>().Include(x => x.PortsOfCall).Where(x => x.Id == vslId).FirstOrDefaultAsync();
            var lastPortName = currentVessel.PortsOfCall.Select(x => x.PortName).Last();
            var lastPortCountry = currentVessel.PortsOfCall.Select(x => x.Country).Last();
            var model = new VoyageDataViewModel()
            {
                DestinationId = destinationId,
                VesselId = vslId,
                LastPortLat = lastPortLat,
                LastPortLong = lastPortLong,
                LastPortName = lastPortName,
                LastPortCountry = lastPortCountry,
                DestPortLat = destPortLat,
                DestPortLong = destPortLong,
                DestPortName = portName,
                Country = destCountry,
                UNLocode = destUNLocode,
                ExpectedSpeed = spd,
                CalculatedDistance = GetDistanceBetweenPorts(double.Parse(lastPortLat), double.Parse(lastPortLong),
                                                             double.Parse(destPortLat), double.Parse(destPortLong)),
            };
            return model;
        }

        private double GetDistanceBetweenPorts(double lastPortLat, double lastPortLong, double destPortLat, double destPortLong)
        {
            var baseRad = Math.PI * lastPortLat / 180.0;
            var targetRad = Math.PI * destPortLat / 180.0;
            var theta = lastPortLong - destPortLong;
            var thetaRad = Math.PI * theta / 180.0;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180.0 / Math.PI;
            dist = dist * 60.0 * 1.1515;
            return dist;
        }

        public async Task AddDestinationToVessel(int vesselId, int destinationId, double distanceSailed)
        {
            var vessel = await repo.All<Vessel>()
                .Include(x => x.PortsOfCall)
                .Include(y=>y.Distances)
                .FirstOrDefaultAsync(x => x.Id == vesselId);

            var destinationPort = await repo.GetByIdAsync<DestinationPort>(destinationId);
            var portOfCall = new PortOfCall()
            {
                Country = destinationPort.Country,
                UNLocode = destinationPort.UNLocode,
                Latitude = destinationPort.Latitude,
                Longitude = destinationPort.Longitude,
                PortName = destinationPort.PortName,
            };            
            if (vessel.PortsOfCall.Last().UNLocode != portOfCall.UNLocode)
            {
                var distance = new Distance() 
                {
                    VesselName=vessel.Name,
                    VesselDistance = distanceSailed,
                    VesselId=vessel.Id,
                };
                vessel.DestinationPortId = destinationId;
                vessel.PortsOfCall.Add(portOfCall);
                vessel.Distances.Add(distance);
                await repo.SaveChangesAsync();
            }
        }
    }
}
