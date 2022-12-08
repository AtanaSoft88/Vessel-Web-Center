using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Enums;

namespace VesselWebCenter.Data.Models
{
    public class Vessel
    {
        public Vessel()
        {
            PortsOfCall = new HashSet<PortOfCall>();
            CrewMembers = new HashSet<CrewMember>();
            Distances = new HashSet<Distance>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string CallSign { get; set; } = null!; 

        public bool IsLaden { get; set; }  

        [Required]        
        public int LengthOverall { get; set; } 

        [Required]        
        public int BreadthMax { get; set; } 

        [Column(TypeName = "nvarchar(35)")]  
        [Required]
        public VesselType VesselType { get; set; }
        public string? CargoTypeOnBoard { get; set; } 

        [Required]
        [StringLength(400)]
        public string VesselImageUrl { get; set; } = null!;

        [ForeignKey(nameof(ManningCompany))]
        public int ManningCompanyId { get; set; } 
        public ManningCompany ManningCompany { get; set; } = null!;

        [ForeignKey(nameof(DestinationPort))]
        public int? DestinationPortId { get; set; }
        public DestinationPort? DestinationPort { get; set; }

        public virtual ICollection<Distance> Distances { get; set; }

        public virtual ICollection<CrewMember> CrewMembers { get; set; }

        public virtual ICollection<PortOfCall> PortsOfCall { get; set; }
    }
}
