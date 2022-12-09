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
        public async Task<IEnumerable<MostVisitedPortsViewModel>> GetMostVisitedPorts()
        {            
            return await repo.AllReadonly<PortOfCall>().Select(p => new MostVisitedPortsViewModel
            {
                PortName = p.PortName,
                CountryName = p.Country,
                TotalVesselsVisited = p.Vessels.Count(),
            }).OrderByDescending(vc=>vc.TotalVesselsVisited).ToListAsync();
        }

        public async Task<IEnumerable<MostVisitedPortsViewModel>> GetMost10VisitedPorts()
        {
            var allPorts = await repo.All<PortOfCall>().Include(v => v.Vessels).Select(p => new MostVisitedPortsViewModel
            {
                PortName = p.PortName,
                CountryName = p.Country,
                TotalVesselsVisited = p.Vessels.Count(),
            }).OrderByDescending(vc => vc.TotalVesselsVisited).ToListAsync();
            var portNames = allPorts.Select(x=>x.PortName).ToList();
            
            var portInfo = new Dictionary<string, int>();
            foreach (var port in allPorts)
            {
                if (!portInfo.ContainsKey(port.PortName))
                {
                    portInfo.Add(port.PortName, port.TotalVesselsVisited);
                }
                else
                {
                    portInfo[port.PortName]++;
                }
            }
            allPorts = allPorts.DistinctBy(x=>x.PortName).ToList();
            for (int i = 0; i < allPorts.Count(); i++)
            {
                if ( portInfo.Keys.ElementAt(i) == allPorts[i].PortName)
                {
                    allPorts[i].TotalVesselsVisited = portInfo[allPorts[i].PortName];                   
                }                
            }
            
            return allPorts.OrderByDescending(x => x.TotalVesselsVisited).Take(10).ToList();
        }
    }
}
