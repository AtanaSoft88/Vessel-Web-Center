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
        Task AddCrewMemberToDataBase(CrewMemberViewModel model);
        Task GetCrewMember(CrewMembersDropDownViewModel model);
        Task<IEnumerable<SelectListItem>> GetAllAvailableCrewMembers();
        Task<IEnumerable<CrewAllViewModel>> GetAll();
    }
}
