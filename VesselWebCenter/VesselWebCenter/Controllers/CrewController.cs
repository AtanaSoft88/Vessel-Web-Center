using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{   
    public class CrewController : BaseController
    {
        private readonly ICrewService service;

        public CrewController(ICrewService service)
        {
            this.service = service;
        }
                
        [HttpGet]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public IActionResult AddCrewMemberAsUnassigned()
        {
            var crewMember = new CrewMemberViewModel();

            return this.View(crewMember);

        }
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        [HttpPost]
        public async Task<IActionResult> AddCrewMemberAsUnassigned(CrewMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);

            }
            await service.AddCrewMemberToDataBase(model);
            return RedirectToAction(nameof(AddCrewMemberAsUnassigned));

        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public async Task<IActionResult> AssignCrewToVessel()
        {
            var model = new CrewMembersDropDownViewModel
            {
                CrewMembers = await service.GetAllAvailableCrewMembers()
            };
            return this.View(model);
        }


        [HttpPost]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public async Task<IActionResult> AssignCrewToVessel(CrewMembersDropDownViewModel model, int Id)
        {
            model.VesselId = Id;
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            await service.GetCrewMember(model);
            return RedirectToAction("ChooseAVessel","Vessel", new { Id});

        }
    }
}
