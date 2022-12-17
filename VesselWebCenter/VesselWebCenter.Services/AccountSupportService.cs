using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services
{
    public class AccountSupportService : IAccountSupportService
    {        
        private readonly IRepository repo;
        private readonly UserManager<AppUser> userManager;
        

        public AccountSupportService(IRepository repo, UserManager<AppUser> userManager)
        {            
            this.repo = repo;
            this.userManager = userManager;            
        }
        public async Task DeleteAccountAsync(AccountDeleteViewModel account)
        {            
            var userToDelete = await repo.All<AppUser>(x => x.Email == account.EmailAddress).FirstOrDefaultAsync();
           
            if (userToDelete != null)
            {
                userToDelete.IsDeleted = true;
                userToDelete.DeletedOn = DateTime.UtcNow;
                repo.Update(userToDelete);
                await repo.SaveChangesAsync();

            }                        
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUsers()
        {
            var users = await repo.All<AppUser>().Where(u=>u.IsDeleted==false).Select(x=>new SelectListItem 
            { 
                Text= $"💇‍♂️‍[{x.FirstName} {x.LastName}] | ✉:{x.Email}",
                Value= x.Email,
            }).ToListAsync();
            return users;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUsersId()
        {            
            var users = await userManager.Users.Select(x => new SelectListItem
            {
                Text = $"💇‍♂️‍[{x.FirstName} {x.LastName}] | ✉:{x.Email}",
                Value = x.Id.ToString(),
            }).ToListAsync();
            
            return users;
        }

        public async Task ManageRoles(AccountAddRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId.ToString());            
            if (user == null) 
            {                 
                return; 
            }
            if (await userManager.IsInRoleAsync(user, model.RoleName) == true)
            {
                model.IsAlreadyInRole = true;
                return;
            }
            if (model.RoleName.Contains("Reset"))
            {
                var rolesToReset = await userManager.GetRolesAsync(user);
                var resultReset = await userManager.RemoveFromRolesAsync(user, rolesToReset);
                if (resultReset.Succeeded)
                {
                    model.AreRolesReset = true;
                    return;
                }
            }
            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                model.IsRoleAddWithSuccess = true;
                return;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetAllDeletedUsers()
        {
            var users = await repo.All<AppUser>().Where(u => u.IsDeleted == true).Select(x => new SelectListItem
            {
                Text = $"💇‍♂️‍[{x.FirstName} {x.LastName}] | ✉:{x.Email}",
                Value = x.Email,
            }).ToListAsync();
            return users;
        }

        public async Task GetUserAccountRecovered(AccountRecoverViewModel account)
        {
            var userToRecover = await repo.All<AppUser>(x => x.Email == account.EmailAddress).FirstOrDefaultAsync();

            if (userToRecover != null)
            {
                userToRecover.IsDeleted = false;
                userToRecover.DeletedOn = null;
                repo.Update(userToRecover);
                await repo.SaveChangesAsync();

            }
        }
    }
}
