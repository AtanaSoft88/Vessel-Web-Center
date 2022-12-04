namespace VesselWebCenter.Services.ViewModels
{
    public class VoyageDataViewModel
    {
        public int VesselId { get; set; }        
        public int DestinationId { get; set; }
        public string DestPortName { get; set; } = null!;
        public string LastPortLat { get; set; } = null!;
        public string LastPortLong { get; set; } = null!;
        public string LastPortName { get; set; } = null!;
        public string LastPortCountry { get; set; } = null!;
        public string DestPortLat { get; set; } = null!;
        public string DestPortLong { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string UNLocode { get; set; } = null!;
        public int ExpectedSpeed { get; set; }
        public double CalculatedDistance { get; set; }
        public double CalculatedTime => this.CalculatedDistance / (this.ExpectedSpeed*1.0);
        public DateTime DepartureTime => DateTime.UtcNow;
        public DateTime? ETA => DateTime.UtcNow.AddHours(this.CalculatedTime);


    }
}
