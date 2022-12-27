using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Areas.Admin.Controllers
{
    /// <summary>
    /// Vessel Controller responsible for Vessels arrangement
    /// </summary>
    public class VesselController : BaseController
    {
        private readonly IVesselDataService service;

        /// <summary>
        /// Controller's constructor with DI needed services
        /// </summary>
        /// <param name="service"></param>
        public VesselController(IVesselDataService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Getting all Vessels and paginated with possible movement to Next and Previous pages
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns>All Vessels Paginated</returns>
       
        [ResponseCache(Duration = 30 , Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public async Task<IActionResult> GetAllVessels(int pageNumber = 1)
        {
            IQueryable<VesselsViewModel>? vessels = service.GetAll();
            if (await vessels.CountAsync() > 0 && vessels != null)
            {
                var model = await PagingList<VesselsViewModel>.CreatePagesAsync(vessels, pageNumber, 7);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Get chosen Vessel by Id for further manipulation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vesselId"></param>
        /// <returns>Vessel chosen by User</returns>
        
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
