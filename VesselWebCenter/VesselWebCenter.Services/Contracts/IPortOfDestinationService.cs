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
        Task<IEnumerable<VesselAssignViewModel>> GetAllAvaliableForVoyage();
        Task<IEnumerable<string>> GetCoordinates(string parameters);
        Task<DestinationViewModel> GetDestinationPorts(string vesselId);
    }
}
