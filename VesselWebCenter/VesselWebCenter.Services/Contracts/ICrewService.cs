using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
    public interface ICrewService
    {
        Task<bool> AddCrewMemberToDataBase(CrewMemberViewModel model);
        Task AddCrewMemberToVessel(CrewMembersDropDownViewModel model);
        Task RemovedCrewMemberFromVessel(CrewMembersDropDownViewModel model);
        Task<IEnumerable<SelectListItem>> GetAllAvailableCrewMembers();
        Task<IEnumerable<SelectListItem>> GetAllCrewMembers(int vesselId);
        Task<IQueryable<CrewAllViewModel>> GetAll();
    }
}
