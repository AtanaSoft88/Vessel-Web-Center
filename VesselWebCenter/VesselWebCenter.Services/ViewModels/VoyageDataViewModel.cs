namespace VesselWebCenter.Services.ViewModels
{
    public class VoyageDataViewModel
    {
        public int VesselId { get; set; }        
        public int DestinationId { get; set; }        
        public string DestPortName { get; set; }
        public string LastPortLat { get; set; }
        public string LastPortLong { get; set; }
        public string LastPortName { get; set; }
        public string LastPortCountry { get; set; }
        public string DestPortLat { get; set; }
        public string DestPortLong { get; set; }
        public string Country { get; set; }
        public string UNLocode { get; set; }
        public int ExpectedSpeed { get; set; }
        public double CalculatedDistance { get; set; }
        public double CalculatedTime => this.CalculatedDistance / (this.ExpectedSpeed*1.0);
        public DateTime DepartureTime => DateTime.UtcNow;
        public DateTime? ETA => DateTime.UtcNow.AddHours(this.CalculatedTime);


    }
}
