using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Services.ViewModels
{
	public class VesselAssignViewModel
	{		
		public int VesselId { get; set; }
		public string VesselName { get; set; }
		public string VesselType { get; set; }
		public string LastPortName { get; set; }
		public string LatitudeLastPort { get; set; }
		public string LongitudeLastPort { get; set; }	

	}
}
