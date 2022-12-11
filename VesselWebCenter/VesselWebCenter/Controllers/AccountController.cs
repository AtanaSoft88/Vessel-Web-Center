using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Services.Contracts;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Controllers
{
    /// <summary>
    /// Controller responsible for Account management
    /// </summary>
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IAccountSupportService accountSupportService;

        /// <summary>
        /// Configuring UserManager,SignInManager and RoleManager for each specific User.
        /// </summary>
        /// <param name="_userManager"></param>
        /// <param name="_signInManager"></param>
        /// <param name="_roleManager"></param>
        /// <param name="_accountSupportService"></param>
        public AccountController(
            UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            RoleManager<IdentityRole<Guid>> _roleManager,
            IAccountSupportService _accountSupportService)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleManager = _roleManager;
            this.accountSupportService = _accountSupportService;
        }

        /// <summary>
        /// Registers Users
        /// </summary>
        /// <returns>Registeration form</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {            
            var model = new RegisterViewModel();
            return View(model);
        }

        /// <summary>
        /// Registers Users,Initializes all available Roles, 
        /// Gives First User specific roles such as:ADMINISTRATOR,USER-OWNER,MANAGER
        /// Adds Claims to each registered user.
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns>Registration form</returns>
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
            };
            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {                
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("first_name", user.FirstName ?? user.Email));
                
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.ADMINISTRATOR));                   
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.USER_OWNER));
                    await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.REGULAR_USER));
                }
                await signInManager.SignInAsync(user, isPersistent: false);
                if (userManager.Users.Count() == 1)
                {
                    await userManager.AddToRolesAsync(user, new string[]
                    {   RoleConstants.ADMINISTRATOR,                        
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

        /// <summary>
        /// Login all registered and authenticated users
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>Login form to be filled up</returns>
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

        /// <summary>
        /// Login all registered and authenticated users
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>Signing in a User</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(loginModel);
            }
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && user.IsDeleted == false)
            {
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

        /// <summary>
        /// Logging out a User
        /// </summary>
        /// <returns>User Signed out</returns>
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Configure message upon implementing logic for user deletion
        /// </summary>
        /// <returns>msg</returns>
        public IActionResult UserMessages()
        {
            return this.View();
        }

        /// <summary>
        /// Configure message on not persmission granted
        /// </summary>
        /// <returns>msg</returns>
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Deletion of an User Account
        /// </summary>
        /// <returns>All User Accounts which can be deleted</returns>
        [HttpGet]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> DeleteUserAccount()
        {
            var model = new AccountDeleteViewModel();
            model.Users = await accountSupportService.GetAllUsers();
            return View(model);
        }

        /// <summary>
        /// Actual deletion of an User Account - marked as Deleted ( not phisically deleted )
        /// </summary>
        /// <param name="account"></param>
        /// <returns>All available User's Accounts(Those not marked as Deleted)</returns>
        [HttpPost]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> DeleteUserAccount(AccountDeleteViewModel account)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(account);
            }

            if (User.IsInRole(RoleConstants.ADMINISTRATOR) 
                && User.IsInRole(RoleConstants.USER_OWNER)                                                           
                && User?.Identity?.Name == account.EmailAddress)
            {                
                return RedirectToAction(nameof(UserMessages), "Account");
            }
            var currentUserToDelete = userManager.Users.FirstOrDefault(x => x.Email == account.EmailAddress);
            if ((!userManager.Users.Any(x => x.Email == account.EmailAddress)) || currentUserToDelete.IsDeleted == true)
            {
                ModelState.AddModelError("", "There is no such email address available!");
                TempData["delEmail"] = "unavailable";
                return RedirectToAction("UserMessages", "Account");
            }
            await accountSupportService.DeleteUserAccount(account);
            TempData["delUser"] = userManager.Users.Where(x => x.Email == account.EmailAddress).FirstOrDefault()?.FirstName;
            TempData["delEmail"] = userManager.Users.Where(x => x.Email == account.EmailAddress).FirstOrDefault()?.Email;
            return RedirectToAction(nameof(UserMessages), "Account");
        }
        
        /// <summary>
        /// Recovering of an User Account
        /// </summary>
        /// <returns>All User Accounts which can be Recovered</returns>
        [HttpGet]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> RecoverUserAccount()
        {
            var model = new AccountRecoverViewModel();
            model.Users = await accountSupportService.GetAllDeletedUsers();
            return View(model);
        }

        /// <summary>
        /// Recovering of an User Account
        /// </summary>
        /// <returns>Message describing the operation result</returns>
        [HttpPost]
        [Authorize(Policy = "myFullPermissionPolicy")]
        public async Task<IActionResult> RecoverUserAccount(AccountRecoverViewModel account)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(account);
            }
            await accountSupportService.GetUserAccountRecovered(account);
            TempData["recoverMsg"] = "recover";
            return RedirectToAction(nameof(UserMessages), "Account");            
        }   

            /// <summary>
            /// Configuring User Roles or Resetting existing ones
            /// </summary>
            /// <returns>List of Users and their Roles</returns>
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

        /// <summary>
        /// Actual role assigned to chosen User Accounts.Configuring User Roles or Resetting existing ones.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns>Obtaining roles for given Users</returns>
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
