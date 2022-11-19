using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VesselWebCenter.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{		
		public string UserFirstName 
		{
			get 
			{
				string firstName = User?.Identity?.Name ?? string.Empty;
				if (User!=null && User.HasClaim(c=>c.Type=="first_name"))
				{
					firstName = User?.Claims.FirstOrDefault(c => c.Type == "first_name")?.Value ?? firstName;
				}
				return firstName;
			}
        }

		public override void OnActionExecuted(ActionExecutedContext context)
		{
            ViewBag.UserFirstName = UserFirstName;			
            base.OnActionExecuted(context);
		}
	}
}
