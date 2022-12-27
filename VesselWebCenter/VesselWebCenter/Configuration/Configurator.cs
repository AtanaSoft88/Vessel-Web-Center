using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.Models.Accounts;

namespace VesselWebCenter.Configuration
{
	public static class Configurator
	{
		public static async Task ConfigureAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager) 
		{
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.ADMINISTRATOR));
                await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.USER_OWNER));
                await roleManager.CreateAsync(new IdentityRole<Guid>(RoleConstants.REGULAR_USER));
            }
            var user = new AppUser
            {
                FirstName = "Mr_Admin",
                LastName = "Mr_Admin",
                Email = "admin@abv.bg",
                UserName = "admin@abv.bg",
                PhoneNumber = "0894650000",

            };
            if (await userManager.Users.AnyAsync(x => x.Email == "admin@abv.bg") == false)
            {
                var result = await userManager.CreateAsync(user, "admin@admin");
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("first_name", user.FirstName ?? user.Email));
                    var roles = await userManager.GetRolesAsync(user);
                    if (!roles.Any())
                    {
                        await userManager.AddToRolesAsync(user, new string[] { RoleConstants.ADMINISTRATOR, RoleConstants.USER_OWNER });
                    }
                }
            }
        }
	}
}
