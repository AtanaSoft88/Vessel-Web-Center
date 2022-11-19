using System.ComponentModel.DataAnnotations;

namespace VesselWebCenter.Data.Models
{
    public class PortOfCall
    {
        public PortOfCall()
        {
            Vessels = new HashSet<Vessel>();
        }

        [Key]        
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string PortName { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Latitude { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Longitude { get; set; } = null!;

        [Required]
        [StringLength(80)]
        public string Country { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string UNLocode { get; set; } = null!;
        public virtual ICollection<Vessel> Vessels { get; set; }  // Collection of Vessels sailed to a port       


    }
}
