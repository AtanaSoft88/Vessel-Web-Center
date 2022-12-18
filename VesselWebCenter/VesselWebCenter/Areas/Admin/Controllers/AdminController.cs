using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VesselWebCenter.Controllers;
using VesselWebCenter.Areas.Admin.Constants;
namespace VesselWebCenter.Areas.Admin.Controllers
{	
	public class AdminController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}

	}
}
