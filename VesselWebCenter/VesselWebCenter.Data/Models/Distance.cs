using System.ComponentModel.DataAnnotations;

namespace VesselWebCenter.Data.Models
{
    public class Distance
    {
        [Key]
        public int Id { get; set; }
        public int? VesselId { get; set; }
        public string? VesselName { get; set; }
        public double? VesselDistance { get; set; }

    }
}
