using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Constants;

namespace VesselWebCenter.Services.ViewModels
{
    public class AccountRecoverViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.CHOOSE_ACCOUNT_TO_RECOVER)]
        public string EmailAddress { get; set; } = null!;
        public bool isDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public IEnumerable<SelectListItem> Users = new List<SelectListItem>();
    }
}
