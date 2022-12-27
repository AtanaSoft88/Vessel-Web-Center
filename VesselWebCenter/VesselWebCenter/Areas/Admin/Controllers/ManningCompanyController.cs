using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Services.Contracts;

namespace VesselWebCenter.Areas.Admin.Controllers
{    
    public class ManningCompanyController : BaseController
    {
        private readonly IManningCompanyService service;

        public ManningCompanyController(IManningCompanyService _service)
        {
            this.service = _service;
        }

        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetAllManningCompanies()
        {
            var companies = await service.GetAllCompanies();
            return View(companies);
        }

        [HttpPost]
        public async Task<IActionResult> GetManningCompaniesVessels(int compId, int idComp)
        {
            if (compId == 0)
            {
                compId = idComp;
            }
            var vessels = await service.GetVessels(compId);
            return View(vessels);
        }
    }
}
