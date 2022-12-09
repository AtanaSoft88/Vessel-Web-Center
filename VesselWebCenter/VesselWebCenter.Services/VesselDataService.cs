using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;


namespace VesselWebCenter.Services
{
    public class VesselDataService : IVesselDataService
    {
        private readonly IRepository repo;       

        public VesselDataService(IRepository repo)
        {
            this.repo = repo;            
        }        
        public async Task<SingleVesselViewModel> GetChoosenVessel(int idVessel)
        {
            return await repo.AllReadonly<Vessel>().Include(x=>x.CrewMembers).Where(x=>x.Id==idVessel).Select(x => new SingleVesselViewModel
            {
                Id = x.Id,                
                Name = x.Name,
                CallSign = x.CallSign,
                IsLaden = x.IsLaden,
                Breadth = x.BreadthMax,
                LOA = x.LengthOverall,
                VesselType = x.VesselType,
                VesselImageUrl = x.VesselImageUrl,
                CargoTypeOnBoard = x.CargoTypeOnBoard ?? "Unavailable",
                CrewMembersOnBoard = x.CrewMembers.Count(),
                ManningCompanyName = x.ManningCompany.Name,
                Distance = x.Distances
                .Sum(x=>x.VesselDistance) == 0 ?
                "𝕟𝐼𝕒" : double.Parse(x.Distances
                .Sum(x => x.VesselDistance.Value)
                .ToString("f2"))+ " nm",
                
                PortsOfCall = x.PortsOfCall.ToList(),                
            }).FirstAsync();     
                  
        }

        public IQueryable<VesselsViewModel> GetAll()
        {
            var allVessels = repo.AllReadonly<Vessel>().Select(x => new VesselsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CallSign = x.CallSign,
                Breadth = x.BreadthMax,
                LOA = x.LengthOverall,
                VesselType = x.VesselType,
                VesselAvailableForVoyage = x.CrewMembers.Count()>=15? true : false,
                CrewMembersCount = x.CrewMembers.Count()==0?"n/a": x.CrewMembers.Count().ToString(),
                PortsOfCall = x.PortsOfCall.ToList(),
            });
            return allVessels.AsNoTracking();
             
        }

        public async Task<IEnumerable<VesselsHomeViewModel>> AllAsHomePage()
        {
            return await repo.AllReadonly<Vessel>().Where(x=>x.IsLaden==false).Select(v => new VesselsHomeViewModel
            {
                IdVessel = v.Id,
                ImageUrl = v.VesselImageUrl,
                Name = v.Name,
            }).ToListAsync();             
        }
    }
}
