using System.ComponentModel.DataAnnotations;

namespace VesselWebCenter.Data.Models
{
    public class ManningCompany
    {
        public ManningCompany()
        {
            Vessels = new HashSet<Vessel>();
        }
        [Key]        
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(60)]
        public string Country { get; set; } = null!;
        public virtual ICollection<Vessel> Vessels { get; set; }  //CompanyVessels       

    }
}
