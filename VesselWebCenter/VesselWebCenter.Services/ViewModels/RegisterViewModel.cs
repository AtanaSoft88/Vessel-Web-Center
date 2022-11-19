using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Constants;

namespace VesselWebCenter.Services.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]        
        [Compare(nameof(ConfirmPassword))]        
        [DataType(DataType.Password)]
        [StringLength(32,MinimumLength =5)]
        public string Password { get; set; } = null!;

        [Required]        
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = null!;

       // public string PreservedPass => this.Password; // Only for test - delete it before exam !!!

        [Required]
        [StringLength(21,MinimumLength = 2)]
        [RegularExpression(@"\b[A-Z]{1}[a-z]{1,20}\b", ErrorMessage = GlobalConstants.NAME_FIELD_REQUIREMENTS)]
        public string FirstName { get; set; } = null!;


        [Required]
        [StringLength(21, MinimumLength = 2)]
        [RegularExpression(@"\b[A-Z]{1}[a-z]{1,20}\b", ErrorMessage = GlobalConstants.NAME_FIELD_REQUIREMENTS)]
        public string LastName { get; set; } = null!;
    }
}
