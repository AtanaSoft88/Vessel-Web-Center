using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
    public interface IAccountSupportService
    {
        Task DeleteAccountAsync(AccountDeleteViewModel account);         
        Task<IEnumerable<SelectListItem>> GetAllUsers();
        Task<IEnumerable<SelectListItem>> GetAllDeletedUsers();
        Task GetUserAccountRecovered(AccountRecoverViewModel account);
        Task ManageRoles(AccountAddRolesViewModel model);
        Task<IEnumerable<SelectListItem>> GetAllUsersId();
    }
}
