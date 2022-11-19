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
            int n = 0;
            var mostVisitedPorts = await service.GetMostVisitedPorts(n);

            return View(mostVisitedPorts);

        }

        [AllowAnonymous]        
        [HttpPost]
        public async Task<IActionResult> MostVisitedPorts([FromForm]int n)
        {
            var mostVisitedPorts = await service.GetMostVisitedPorts(n);

            return View(mostVisitedPorts);

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TopTenVisitedPortsHighCharts()
        {
            int n = 10; 
            return Json(await service.GetMostVisitedPorts(n));

        }        
        
    }
}
