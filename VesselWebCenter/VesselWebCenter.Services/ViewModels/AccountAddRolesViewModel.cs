using Microsoft.AspNetCore.Mvc.Rendering;

namespace VesselWebCenter.Services.ViewModels
{
    public class AccountAddRolesViewModel
    {       
        
        public Guid UserId { get; set; }           
        public string RoleName { get; set; } = null!;        
        public bool IsRoleAddWithSuccess { get; set; } = false;        
        public bool IsAlreadyInRole { get; set; } = false;        
        public bool AreRolesReset { get; set; } = false;        
        public List<string> RolesAvailable { get; set; } = new List<string>();
        
        public IEnumerable<SelectListItem> UserIds = new List<SelectListItem>();

    }
}
