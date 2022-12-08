using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselWebCenter.Data.Enums;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Services.ViewModels
{
	public class SingleVesselViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; } = null!;
        public string CallSign { get; set; } = null!;
        public bool IsLaden { get; set; }
        public VesselType VesselType { get; set; }        
        public int LOA { get; set; }
        public int Breadth { get; set; }
        public string VesselImageUrl { get; set; } = null!;
        public string? CargoTypeOnBoard { get; set; } = null!;  // Can be null if under Ballast or VesselType = Tug
        public string ManningCompanyName { get; set; } = null!;
        public int? CrewMembersOnBoard { get; set; }
        public string? Distance { get; set; }
        public List<PortOfCall>? PortsOfCall { get; set; }
    }        
}
