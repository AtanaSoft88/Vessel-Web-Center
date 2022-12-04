using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{
    public class PortService : IPortService
    {
        private readonly IRepository repo;

        public PortService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task<IEnumerable<MostVisitedPortsViewModel>> GetMostVisitedPorts(int n)
        {            
            return await repo.AllReadonly<PortOfCall>().Select(p => new MostVisitedPortsViewModel
            {
                PortName = p.PortName,
                CountryName = p.Country,
                TotalVesselsVisited = p.Vessels.Count(),
            }).OrderByDescending(vc=>vc.TotalVesselsVisited).Take(n).ToListAsync();
        }        
    }
}
