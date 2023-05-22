using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});
builder.Services.AddRazorPages();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => { options.LoginPath = "/login"; });

await CreateRoles();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages(); // Map Razor Pages endpoints
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


async Task CreateRoles()
{
    var RoleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
    var UserManager = builder.Services.BuildServiceProvider().GetRequiredService<UserManager<User>>();

    IdentityResult roleResult;
    /**
     * 
        Admin,
        Gestionnaire,
        Employee***/

    var roleCheck = await RoleManager.RoleExistsAsync("Admin");
    var roleCheck1 = await RoleManager.RoleExistsAsync("Gestionnaire");
    var roleCheck2 = await RoleManager.RoleExistsAsync("Employee");


    if (!roleCheck)
    {
        roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));

    }
    if (!roleCheck1)
    {
        roleResult = await RoleManager.CreateAsync(new IdentityRole("Gestionnaire"));

    }
    if (!roleCheck2)
    {

        roleResult = await RoleManager.CreateAsync(new IdentityRole("Employee"));
    }


    var checkuser = await UserManager.FindByEmailAsync("admin@admin.com");
    if (checkuser == null)
    {
        var usr = new User
        {
            Nom = "admin",
            preNom = "admin",
            UserName = "admin@admin.com",
            Email = "admin@admin.com"
        };
        var chkUser = await UserManager.CreateAsync(usr, "karim123");
        if (chkUser.Succeeded)
        {
            var result1 = await UserManager.AddToRoleAsync(usr, "Admin");
        }
    }
}