using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
    public interface IPortService
    {        
        Task<IEnumerable<MostVisitedPortsViewModel>> GetMostVisitedPorts();        
        Task<IEnumerable<MostVisitedPortsViewModel>> Get10MostVisitedPorts();        
        
    }
}
