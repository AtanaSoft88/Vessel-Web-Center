using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
    public interface IAccountSupportService
    {
        Task DeleteUserAccount(AccountDeleteViewModel account);       
        Task<IEnumerable<SelectListItem>> GetAllUsers();        
        Task ManageRoles(AccountAddRolesViewModel model);
        Task<IEnumerable<SelectListItem>> GetAllUsersId();
    }
}
