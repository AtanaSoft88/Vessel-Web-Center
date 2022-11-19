using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IAccountSupportService accountSupportService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole<Guid>> _roleManager,
            IAccountSupportService accountSupportService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = _roleManager;
            this.accountSupportService = accountSupportService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(registerModel);
            }

            var user = new AppUser()
            {
                Email = registerModel.Email,
                EmailConfirmed = true,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.Email,
                // to be deleted in a real project , no password in db allowed! Only hashed passwords.
                //PasswordPreserved = registerModel.Password,
            };
            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                //Add claim , use the First name if it is null - get the email
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("first_name", user.FirstName ?? user.Email));
                //-------------------------------------------------------------------------------------------------------------------
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.ADMINISTRATOR));
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.MANAGER));
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.USER_OWNER));
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.REGULAR_USER));
                }
                await signInManager.SignInAsync(user, isPersistent: false);
                if (userManager.Users.Count() == 1)
                {
                    await userManager.AddToRolesAsync(user, new string[] 
                    {   RoleConstants.ADMINISTRATOR,
                        RoleConstants.MANAGER,
                        RoleConstants.USER_OWNER 
                    });
                }                             
                return RedirectToAction("Index", "Home");
            }

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return View(registerModel);

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var loginModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
            };
            return View(loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel) //djx7000@abv.bg - User Nakata / Nasko8807@
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(loginModel);
            }
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && user.IsDeleted == false) // && user.IsDeleted == false
            {
                // "isPersistent:false" - here we need this true if we already implemented "Remmember me" on login
                var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (loginModel.ReturnUrl != null)
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid Login");
            return View(loginModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }        
        public IActionResult MessageOnDeleteUser()
        {
            return this.View();
        }

        //[AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }      

        [HttpGet]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> DeleteUserAccount()
        {
            var model = new AccountDeleteViewModel();
            model.Users = await accountSupportService.GetAllUsers();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "myFullPermissionPolicy")]        
        public async Task<IActionResult> DeleteUserAccount(AccountDeleteViewModel account)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(account);
            }

            if (User.IsInRole(RoleConstants.ADMINISTRATOR) && User.IsInRole(RoleConstants.MANAGER) && User?.Identity?.Name == account.EmailAddress)
            {
                return RedirectToAction("MessageOnDeleteUser", "Account");
            }
            var currentUserToDelete = userManager.Users.FirstOrDefault(x => x.Email == account.EmailAddress);
            if ((!userManager.Users.Any(x => x.Email == account.EmailAddress)) || currentUserToDelete.IsDeleted == true)
            {
                ModelState.AddModelError("", "There is no such email address available!");
                TempData["delEmail"] = "unavailable";
                return RedirectToAction("MessageOnDeleteUser", "Account");
            }
            await accountSupportService.DeleteUserAccount(account);
            TempData["delUser"] = userManager.Users.Where(x => x.Email == account.EmailAddress).FirstOrDefault()?.FirstName;
            TempData["delEmail"] = userManager.Users.Where(x => x.Email == account.EmailAddress).FirstOrDefault()?.Email;
            return RedirectToAction("MessageOnDeleteUser", "Account");
        }

        [HttpGet]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> ManageUserRoles()
        {
            var model = new AccountAddRolesViewModel();
            if (await roleManager.Roles.AnyAsync())
            {
                model.RolesAvailable = await roleManager.Roles.Select(x => x.Name).ToListAsync();
                model.RolesAvailable.Add("Reset Account Roles");
            }  
           
            model.UserIds = await accountSupportService.GetAllUsersId();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "myFullPermissionPolicy")]        
        public async Task<IActionResult> ManageUserRoles(AccountAddRolesViewModel model, string userId, string roleName)
        {
            if (model.UserId == Guid.Empty || userId.Contains("Select") || roleName.Contains("Select")) 
            {                
                TempData["select_from_menu"] = RoleConstants.SELECT_RELEVANT_ROLE_AND_ACCOUNT;
                return this.RedirectToAction(nameof(ManageUserRoles));

            }
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            await accountSupportService.ManageRoles(model);
            if (model.IsRoleAddWithSuccess)
            {
                TempData["role_add_successfully"] = RoleConstants.SUCCESSFUL_ROLE_ADD;
                return this.RedirectToAction(nameof(ManageUserRoles));
            }
            if (model.IsAlreadyInRole)
            {
                TempData["already_in_role"] = RoleConstants.ROLE_ALREADY_EXISTS;
                return RedirectToAction(nameof(ManageUserRoles));
            }
            if (model.AreRolesReset)
            {
                TempData["roles_reset"] = RoleConstants.RESET_ROLES;
                return RedirectToAction(nameof(ManageUserRoles));
            }
            return RedirectToAction(nameof(ManageUserRoles));
        }
    }
}
