using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository repo;

        public AdminService(IRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task<InformationViewModel> ShowGeneralInfo()
        {
            var model = new InformationViewModel()
            {
                TotalCrewMembersHired = await repo.AllReadonly<CrewMember>().CountAsync(x=>x.IsPartOfACrew==true),
                TotalFreeCrewMembers = await repo.AllReadonly<CrewMember>().CountAsync(),
                TotalVessels = await repo.AllReadonly<Vessel>().CountAsync(),
                VesselsReadyForVoyageCount = await repo.AllReadonly<Vessel>().Where(c=>c.CrewMembers.Count()>=15).CountAsync(),
            };
            return model;
        }
    }
}
