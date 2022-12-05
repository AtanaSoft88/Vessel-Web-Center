namespace VesselWebCenter.Services.ViewModels
{
	public class CrewAllViewModel
	{
        public int CrewMemberId { get; set; }
        public int? VesselId { get; set; }
        public string? VesselName { get; set; }
        public string? ManningCompanyName { get; set; }
        public string FirstName { get; set; } = null!;       
        public string LastName { get; set; } = null!;        
        public int Age { get; set; }        
        public string Nationality { get; set; } = null!;
        public bool HiredToVessel { get; set; }
        public string? DateHired { get; set; }
    }
}
