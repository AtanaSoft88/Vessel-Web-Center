using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{
    public class VesselController : BaseController
    {
        private readonly IVesselDataService service;

        public VesselController(IVesselDataService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllVessels()
        {
            IEnumerable<VesselsViewModel> vessels = await service.GetAll();
            if (vessels.Count()>0)
            {
                return View(vessels);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [AllowAnonymous]        
        public async Task<IActionResult> ChooseAVessel(int id, int vesselId)
        {
            if (id==0)
            {
                id = vesselId;
            }
            var model = await service.GetChoosenVessel(id);
            if (model != null)
            {
                return this.View(model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
