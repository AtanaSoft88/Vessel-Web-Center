using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VesselWebCenter.Services.ViewModels
{
    public class CrewMembersDropDownViewModel
    {  
        [Required(ErrorMessage ="Selecting item is required")]
        public int memberId { get; set; }
        [Required]
        public int VesselId { get; set; }
        public IEnumerable<SelectListItem> CrewMembers = new List<SelectListItem>();
    }
}
