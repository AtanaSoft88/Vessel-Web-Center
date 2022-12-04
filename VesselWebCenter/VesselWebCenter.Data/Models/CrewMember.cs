using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace VesselWebCenter.Data.Models
{
    public class CrewMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(21)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(21)]
        public string LastName { get; set; } = null!;

        [Required]
        [Range(18,80)]
        public int Age { get; set; }
       
        [Required]
        [StringLength(30)]
        public string Nationality { get; set; } = null!;
        public bool IsPartOfACrew { get; set; }
        public DateTime? DateHired { get; set; }

        [ForeignKey(nameof(Vessel))]
        public int? VesselId { get; set; }
        public Vessel Vessel { get; set; } = null!;

    }
}
