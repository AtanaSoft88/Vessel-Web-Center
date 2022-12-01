using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Services.ViewModels
{
	public class DestinationViewModel
	{
        public int DestinationId { get; set; }
        public string VesselName { get; set; }
        public string VesselType { get; set; }
        public string LastPortLatitude { get; set; }
        public string LastPortLongitude { get; set; }
        public string DestinationPortLatitude { get; set; }
        public string DestinationPortLongitude { get; set; }
        public string Country { get; set; }
        public string UNLocode { get; set; }
        public IEnumerable<SelectListItem> DestinationPorts { get; set; } = new List<SelectListItem>();
    }
}
