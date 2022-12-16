using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService notyf;
        public PortOfDestinationController(IPortOfDestinationService _service, INotyfService _notyf)
        {
            this.service = _service;
            this.notyf = _notyf;
        }
        
        [HttpGet]
        public async Task<IActionResult> AssignVesselForVoyage()
        {
            var model = await service.GetAllAvailableForVoyage();
            if (model.Count()!=0)
            {
                model.Select(x => x.IsValueAvailable=true);
            }   
            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> AssignVesselForVoyage(string vesselParams)
        {
            var model = await service.GetDestinationPort(vesselParams);
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
               var model = await service.GetDestinationPort(vesselParams);
                return View(model);
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Error occurred on attempt to pass 'vesselParameters' and return correctly filled model", ex);
            }
        }
                
        [HttpPost]
        public async Task<IActionResult> ChooseDestinationForCurrentVessel(string value, double spd, int vslId)
        {
            try
            {
                var extractedCoordinates = await service.GetCoordinates(value,vslId);
                if (extractedCoordinates == null) 
                {                    
                    notyf.Warning("Voyage could not be processed! Please select Destination port to be different from Last port!");
                    return RedirectToAction(nameof(AssignVesselForVoyage), "PortOfDestination");
                }
                
                return RedirectToAction(nameof(ProcessVoyageDetails), new { extractedCoordinates, spd, vslId });
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error occurred on attempt to set last port and destination port parameters",ex);
               
            }        
                        
        }
        
        [HttpGet]
        public async Task<IActionResult> ProcessVoyageDetails(IEnumerable<string> extractedCoordinates,double spd,int vslId)
        {
            var model = await service.GetDataForCalculation(extractedCoordinates,spd, vslId);            
            return View(model);

        }
        
        [HttpPost]
        public async Task<IActionResult> SetVesselNewDestination(int vslId, int destinationId, double distance) 
        {
            await service.AddDestinationToVessel(vslId, destinationId, distance);
            
            notyf.Success($"Voyage succeeded! Distance of {distance.ToString("f2")} has been travelled.");
            notyf.Information("Vessel new port location has been set!",11);
            return RedirectToAction(nameof(AssignVesselForVoyage), "PortOfDestination");
        }
    }
}
