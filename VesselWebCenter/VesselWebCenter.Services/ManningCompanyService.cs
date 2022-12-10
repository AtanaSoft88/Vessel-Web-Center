using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{    
    public class ManningCompanyService : IManningCompanyService
	{
        private readonly IRepository repo;       

        public ManningCompanyService(IRepository _repo)
        {
            this.repo = _repo;            
        }
        public async Task<IEnumerable<ManningCompanyViewModel>> GetAllCompanies()
		{                  
            return await repo.AllReadonly<ManningCompany>().Include(x => x.Vessels).Select( mc => new ManningCompanyViewModel()
            {
                ManningCompanyId = mc.Id,
                VesselsCount = mc.Vessels.Count(),
                CompanyCountry = mc.Country,
                CompanyName = mc.Name,                

            }).ToListAsync();

        }

        public async Task<IEnumerable<ManningCompaniesVesselsViewModel>> GetVessels(int compId)
        {
            return await repo.AllReadonly<Vessel>().Where(c=>c.ManningCompanyId==compId).Select(v => new ManningCompaniesVesselsViewModel 
            { 
                CompanyName = v.ManningCompany.Name,
                VesselName = v.Name,
                VesselImage = v.VesselImageUrl,
                Distance = v.Distances.Sum(x=>x.VesselDistance) ?? 0
            }).ToListAsync();
        }
    }
}
