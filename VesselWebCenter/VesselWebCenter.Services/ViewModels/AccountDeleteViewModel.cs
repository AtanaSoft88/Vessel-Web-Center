using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Constants.CustomAttributes;

namespace VesselWebCenter.Services.ViewModels
{
    public class AccountDeleteViewModel
    {        
        [Required]
        [EmailAddress(ErrorMessage = GlobalConstants.CHOOSE_ACCOUNT_TO_DELETE)]        
        public string EmailAddress { get; set; } = null!;
        public bool isDeleted { get; set; } = false;        
        public DateTime? DeletedOn { get; set; }
        public IEnumerable<SelectListItem> Users = new List<SelectListItem>();

    }
}
