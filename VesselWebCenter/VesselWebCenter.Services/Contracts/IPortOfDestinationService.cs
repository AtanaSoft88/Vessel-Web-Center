using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
	public interface IPortOfDestinationService
	{
        Task<IEnumerable<VesselAssignViewModel>> GetAllAvailableForVoyage();
        Task<IEnumerable<string>> GetCoordinates(string parameters,int vslId);
        Task<DestinationViewModel> GetDestinationPorts(string vesselId);
        Task<VoyageDataViewModel> GetDataForCalculation(IEnumerable<string> extractedCoordinates, double spd, int VesselId);
        Task AddDestinationToVessel( int vesselId, int destinationId, double distance);
    }
}
