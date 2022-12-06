using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models;
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

        [HttpGet]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public async Task<IActionResult> RemoveCrewFromVessel(int id)
        {
            var model = new CrewMembersDropDownViewModel
            {
                CrewMembers = await service.GetAllCrewMembers(id),
                VesselId = id,                 
            };
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public async Task<IActionResult> RemoveCrewFromVessel(CrewMembersDropDownViewModel model, int vslId)
        {
            model.VesselId = vslId;            
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            await service.GetCrewMemberRemoved(model);
            return RedirectToAction("ChooseAVessel", "Vessel", new { model.VesselId });

        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.USER_OWNER)]
        public async Task<IActionResult> GetAllCrewMembers(int pageNumber = 1)
        {
            IQueryable<CrewAllViewModel>? crewMembers = await service.GetAll();
            if (await crewMembers.CountAsync() > 0 && crewMembers != null)
            {
                var model = await PagingList<CrewAllViewModel>.CreatePagesAsync(crewMembers, pageNumber, 15);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
