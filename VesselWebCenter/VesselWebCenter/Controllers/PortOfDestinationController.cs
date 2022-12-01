using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{
    public class PortOfDestinationController : BaseController
    {
        private readonly IPortOfDestinationService service;

        public PortOfDestinationController(IPortOfDestinationService _service)
        {
            this.service = _service;
        }

        [Authorize(Roles =RoleConstants.USER_OWNER)]
        [HttpGet]
        public async Task<IActionResult> AssignVesselForVoyage()
        {
            var model = await service.GetAllAvaliableForVoyage();

            return View(model);
        }

        [Authorize(Roles = RoleConstants.USER_OWNER)]
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

        [Authorize(Roles = RoleConstants.USER_OWNER)]
        [HttpGet]
        public async Task<IActionResult> ChooseDestinationForCurrentVessel(string vesselParams)
        {
            var model = await service.GetDestinationPorts(vesselParams);
            return View(model);
        }


        [Authorize(Roles = RoleConstants.USER_OWNER)]
        [HttpPost]
        public async Task<IActionResult> CollectVoyageInformation(string value)
        {
            var extractedCoordinates = await service.GetCoordinates(value);
            var portName = extractedCoordinates.ToList()[0];
            var destPortLat = extractedCoordinates.ToList()[1];
            var destPortLong = extractedCoordinates.ToList()[2];
            var lastPortLat = extractedCoordinates.ToList()[3];
            var lastPortLong = extractedCoordinates.ToList()[4];
            var destCountry = extractedCoordinates.ToList()[5];
            var destUNLocode = extractedCoordinates.ToList()[6];
           
            return RedirectToAction(nameof(ProcessVoyageDetails), new {portName, destPortLat, destPortLong, lastPortLat, lastPortLong, destCountry, destUNLocode });
        }

        [Authorize(Roles = RoleConstants.USER_OWNER)]
        [HttpGet]
        public async Task<IActionResult> ProcessVoyageDetails(string portName, string destPortLat, string destPortLong,
            string lastPortLat, string lastPortLong, string destCountry, string destUNLocode)
        {
            var model = new VoyageDataViewModel()
            {
                DestPortName = portName,
                LastPortLat = lastPortLat,
                LastPortLong = lastPortLong,
                DestPortLat = destPortLat,
                DestPortLong = destPortLong,
                Country = destCountry,
                UNLocode = destUNLocode
            };
            return View(model);

        }
    }
}
