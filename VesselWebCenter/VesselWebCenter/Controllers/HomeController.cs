using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VesselWebCenter.Models;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVesselDataService service;

        public HomeController(ILogger<HomeController> logger, IVesselDataService _service)
        {
            _logger = logger;
            this.service = _service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await service.AllAsHomePage();

            return View(models);
        }        

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult StatusCodeError(int errorCode)
        {
            return this.View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}