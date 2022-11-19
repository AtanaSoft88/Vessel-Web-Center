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
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string CallSign { get; set; } = null!; // Позивна 9HAYH9 - 6 letters/digits mixed Uppercase

        public bool IsLaden { get; set; }  // Loaded or under Ballast

        [Required]        
        public int LengthOverall { get; set; } // LOA

        [Required]        
        public int BreadthMax { get; set; } // LBP  

        [Column(TypeName = "nvarchar(35)")]  // This Attribute converts an Enum into nvarchar - STRING in DataBase,instead of Int32
        [Required]
        public VesselType VesselType { get; set; }
        public string? CargoTypeOnBoard { get; set; } // Can be null if under Ballast or VesselType = Tug

        [Required]
        [StringLength(400)]
        public string VesselImageUrl { get; set; } = null!;

        [ForeignKey(nameof(ManningCompany))]
        public int ManningCompanyId { get; set; } 
        public ManningCompany ManningCompany { get; set; } = null!;

        [ForeignKey(nameof(DestinationPort))]
        public int? DestinationPortId { get; set; }
        public DestinationPort? DestinationPort { get; set; }

        public virtual ICollection<CrewMember> CrewMembers { get; set; }

        public virtual ICollection<PortOfCall> PortsOfCall { get; set; }
    }
}
