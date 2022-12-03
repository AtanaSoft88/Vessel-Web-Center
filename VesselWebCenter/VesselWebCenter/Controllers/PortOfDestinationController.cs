using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{
    [Authorize(Roles = RoleConstants.USER_OWNER)]
    public class PortOfDestinationController : BaseController
    {
        private readonly IPortOfDestinationService service;

        public PortOfDestinationController(IPortOfDestinationService _service)
        {
            this.service = _service;
        }
        
        [HttpGet]
        public async Task<IActionResult> AssignVesselForVoyage()
        {
            var model = await service.GetAllAvailableForVoyage();

            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> AssignVesselForVoyage(string vesselParams)
        {
            var model = await service.GetDestinationPorts(vesselParams);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(ChooseDestinationForCurrentVessel), new { vesselParams });
        }
                
        [HttpGet]
        public async Task<IActionResult> ChooseDestinationForCurrentVessel(string vesselParams)
        {
            try
            {
                var model = await service.GetDestinationPorts(vesselParams);
                return View(model);
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error occurred on attempt to pass 'vesselParameters' and return correctly filled model", ex);
            }
        }
                
        [HttpPost]
        public async Task<IActionResult> ChooseDestinationForCurrentVessel(string value, int spd, int vslId)
        {
            try
            {
                var extractedCoordinates = await service.GetCoordinates(value);                                
                return RedirectToAction(nameof(ProcessVoyageDetails), new { extractedCoordinates, spd, vslId });
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error occurred on attempt to set last port and destination port parameters",ex);
               
            }        
                        
        }
        
        [HttpGet]
        public async Task<IActionResult> ProcessVoyageDetails(IEnumerable<string> extractedCoordinates,int spd,int vslId)
        {
            var model = await service.GetDataForCalculation(extractedCoordinates,spd, vslId);
            
            return View(model);

        }
        
        [HttpPost]
        public async Task<IActionResult> SetVesselNewDestination(int vslId, int destinationId) 
        { 
            return View();
        }
    }
}
