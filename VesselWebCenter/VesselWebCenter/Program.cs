using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.DataSeeder.Contracts;
using VesselWebCenter.Data.DataSeeder;
using VesselWebCenter.Data.DataSeeder.DataSeedingServices;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services;
using VesselWebCenter.Services.Contracts;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VesselAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase=false; 
    options.Password.RequireUppercase=false; 
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
   
})  .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<VesselAppDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("myFullPermissionPolicy", policy =>
    policy.RequireAssertion(context =>
                            context.User.IsInRole(RoleConstants.ADMINISTRATOR) &&                            
                            context.User.IsInRole(RoleConstants.USER_OWNER)));
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository, Repository>(); 
builder.Services.AddScoped<IVesselDataService, VesselDataService>(); 
builder.Services.AddScoped<ICrewService, CrewService>(); 
builder.Services.AddScoped<IPortService, PortService>(); 
builder.Services.AddScoped<IAccountSupportService, AccountSupportService>();
builder.Services.AddScoped<ISeederService, SeederService>();
builder.Services.AddScoped<IPortOfDestinationService, PortOfDestinationService>();
builder.Services.AddScoped<IManningCompanyService, ManningCompanyService>();
builder.Services.AddNotyf(config =>  // notify toast msgs
{ 
    config.DurationInSeconds = 10; config
    .IsDismissable = true; config
    .Position = NotyfPosition
    .TopCenter; 
});
builder.Services.AddResponseCaching();
var app = builder.Build(); 

using (var serviceScope = app.Services.CreateScope())
{
    IRepository repository = serviceScope.ServiceProvider.GetRequiredService<IRepository>();
    ISeederService seederService = serviceScope.ServiceProvider.GetRequiredService<ISeederService>();     
    await new DbApplicationSeeder().SeedDataBaseAsync(repository, seederService);
}

if (app.Environment.IsDevelopment())
{    
    app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?errorCode={0}");    
    app.UseDeveloperExceptionPage();    
}
else
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseNotyf(); // notify toast msgs
app.MapRazorPages();
app.UseResponseCaching();
app.Run();

