using Microsoft.AspNetCore.Mvc;
using VesselWebCenter.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace VesselWebCenter.Controllers
{
	public class PortController : BaseController
	{
		private readonly IPortService service;

		public PortController(IPortService _service)
		{
			this.service = _service;
		}

        [AllowAnonymous]
        [HttpGet]        
        public async Task<IActionResult> MostVisitedPorts()
        {            
            var mostVisitedPorts = await service.GetMostVisitedPorts();

            return View(mostVisitedPorts);
        }        

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TopTenVisitedPortsHighCharts()
        {             
            return Json(await service.GetMost10VisitedPorts());

        }        
        
    }
}
