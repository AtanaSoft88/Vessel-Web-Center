using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VesselWebCenter.Data.Constants;

namespace VesselWebCenter.Controllers
{
    [Authorize(Roles = RoleConstants.USER_OWNER)]
    public class ManningCompanyController : BaseController
    {
        public async Task<IActionResult> MostVisitedPorts()
        {
            return View();
        }
    }
}
