using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Constants;

namespace VesselWebCenter.Services.ViewModels
{
    public class CrewMemberViewModel
    {
        [Required]
        [StringLength(21, MinimumLength = 2)]
        [RegularExpression(@"\b[A-Z]{1}[a-z]{1,20}\b", ErrorMessage = GlobalConstants.NAME_FIELD_REQUIREMENTS)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(21, MinimumLength = 2)]
        [RegularExpression(@"\b[A-Z]{1}[a-z]{1,20}\b", ErrorMessage = GlobalConstants.NAME_FIELD_REQUIREMENTS)]
        public string LastName { get; set; } = null!;

        [Required]
        [Range(18, 80)]
        public int Age { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [RegularExpression(@"\b[^\d\W]+\b", ErrorMessage = GlobalConstants.TEXT_FIELD_REQUIREMENTS)]
        public string Nationality { get; set; } = null!;

    }
}
