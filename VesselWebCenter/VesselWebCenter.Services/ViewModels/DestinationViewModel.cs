using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Services.ViewModels
{
	public class DestinationViewModel
	{
        public int VesselId { get; set; }
        public int DestinationId { get; set; }
        public string VesselImage { get; set; } = null!;
        public string VesselName { get; set; } = null!;
        public string VesselType { get; set; } = null!;
        public string LastPortCountry { get; set; } = null!;
        public string LastPortName { get; set; } = null!;
        public string LastPortLatitude { get; set; } = null!;
        public string LastPortLongitude { get; set; } = null!;
        public string DestinationPortLatitude { get; set; } = null!;
        public string DestinationPortLongitude { get; set; } = null!;
        public string DestinationPortCountry { get; set; } = null!;
        public string UNLocode { get; set; } = null!;
        [Required]
        [Range(1,18,ErrorMessage ="Vessel's speed must be between 1 and 18 knots!")]
        public double ExpectedSpeed { get; set; }
        public IEnumerable<SelectListItem> DestinationPorts { get; set; } = new List<SelectListItem>();
    }
}
