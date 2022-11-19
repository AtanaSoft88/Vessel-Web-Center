using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VesselWebCenter.Data;
using VesselWebCenter.Data.Constants;
using VesselWebCenter.Data.DataSeeder;
using VesselWebCenter.Data.Models.Accounts;
using VesselWebCenter.Data.Repositories;
using VesselWebCenter.Services;
using VesselWebCenter.Services.Contracts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<VesselAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//We created User as a model named AppUser.
builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Set it to false at first!(was true)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase=false; // change it for security
    options.Password.RequireUppercase=false; // change it for security
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
   
})  .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<VesselAppDbContext>();

// Make Settings so that, Cookies to redirect to our created custom Login page , not the MVC default page
builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Account/Login";      
    options.AccessDeniedPath = "/Account/AccessDenied";
});

//In case we want to add some Plices ( multiple Roles to justify one action)
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("myFullPermissionPolicy", policy =>
    policy.RequireAssertion(context =>
                            context.User.IsInRole(RoleConstants.ADMINISTRATOR) &&
                            context.User.IsInRole(RoleConstants.MANAGER) &&
                            context.User.IsInRole(RoleConstants.USER_OWNER)));
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository, Repository>(); 
builder.Services.AddScoped<IVesselDataService, VesselDataService>(); 
builder.Services.AddScoped<ICrewService, CrewService>(); 
builder.Services.AddScoped<IPortService, PortService>(); 
builder.Services.AddScoped<IAccountSupportService, AccountSupportService>();

var app = builder.Build();

//Create-vam skoup ,vzemam instanciq na konteksta i puskam Seeder-class, na kojto podavam Context-a za da si seed-na dannite,
//poneje pri rabota na prilojenieto seeder-a ne mi e nujen - za tova ne go registriram v services , a go despose-vam sled seed-a.
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<VesselAppDbContext>();    
    await new DbApplicationSeeder().SeedDataBaseAsync(dbContext);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    //app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?errorCode={0}");    
    app.UseDeveloperExceptionPage();
    //app.UseDatabaseErrorPage();
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

