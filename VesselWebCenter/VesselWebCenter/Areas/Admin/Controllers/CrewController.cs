﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Areas.Admin.Controllers
{   
    public class CrewController : BaseController
    {
        private readonly ICrewService service;
        private readonly INotyfService notyf;
        private readonly ILogger<CrewController> logger;

        public CrewController(ICrewService service,
                              INotyfService _notyf,
                              ILogger<CrewController> _logger)
        {
            this.service = service;
            notyf = _notyf;
            logger = _logger;
        }

        [HttpGet]        
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

        [HttpGet]       
        public IActionResult AddCrewMemberAsUnassigned()
        {
            var crewMember = new CrewMemberViewModel();

            return this.View(crewMember);

        }
        
        [HttpPost]
        public async Task<IActionResult> AddCrewMemberAsUnassigned(CrewMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);

            }

            try
            {
               var isCrewAdd = await service.AddCrewMemberToDataBase(model);
                if (isCrewAdd)
                {
                    notyf.Success($"{model.FirstName} {model.LastName} has been registered as a crew member with success by Admin!");
                }
                else
                {
                    notyf.Warning($"{model.FirstName} {model.LastName} already exists and could not be registered!");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(AddCrewMemberAsUnassigned), ex);
                throw new ApplicationException("Crew member could not be add to db",ex);
            }            
            
            return RedirectToAction(nameof(AddCrewMemberAsUnassigned));

        }

        [HttpGet]        
        public async Task<IActionResult> AssignCrewToVessel()
        {
            var model = new CrewMembersDropDownViewModel
            {
                CrewMembers = await service.GetAllAvailableCrewMembers()
            };
            return this.View(model);
        }


        [HttpPost]       
        public async Task<IActionResult> AssignCrewToVessel(CrewMembersDropDownViewModel model, int Id)
        {
            model.VesselId = Id;
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            await service.AddCrewMemberToVessel(model);
            notyf.Success($"Admin has just add a crew member to this vessel!");
            return RedirectToAction("ChooseAVessel","Vessel", new { Id});

        }

        [HttpGet]        
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
        public async Task<IActionResult> RemoveCrewFromVessel(CrewMembersDropDownViewModel model, int vslId)
        {
            model.VesselId = vslId;            
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            await service.RemovedCrewMemberFromVessel(model);
            notyf.Warning($"Admin has removed a crew member from this vessel!");
            return RedirectToAction("ChooseAVessel", "Vessel", new { model.VesselId });

        }        
    }
}
