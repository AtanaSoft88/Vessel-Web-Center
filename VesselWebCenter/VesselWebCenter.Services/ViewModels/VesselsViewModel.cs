using VesselWebCenter.Data.Enums;
using VesselWebCenter.Data.Models;

namespace VesselWebCenter.Services.ViewModels
{
    public class VesselsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string  CallSign { get; set; } = null!;

        public VesselType VesselType { get; set; }

        public int LOA { get; set; }
        public int Breadth { get; set; }

        public bool VesselAvailableForVoyage { get; set; } = false;
        public List<PortOfCall>? PortsOfCall { get; set; }
    }
}
