using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Models;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVesselDataService service;
        private readonly UserManager<AppUser> userManager;

        public HomeController(ILogger<HomeController> logger, IVesselDataService _service, UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.service = _service;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 30)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await service.AllEmptyVesselsAsHomePage();            
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            return View(models);
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