using BHASCore.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region -- Identity i Entity Framework Core konfiguracija --

// Add services to the container.
var identityConnString = builder.Configuration.GetConnectionString("IdentityConnection") 
    ?? throw new InvalidOperationException("Connection string 'IdentityConnection' not found.");

// registracija konteksta baze podataka za Identity
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(identityConnString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ASP.NET Core Identity konfiguracija
builder.Services.AddDefaultIdentity<IdentityUser>
    (options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuthDbContext>();

#endregion -- Identity i Entity Framework Core konfiguracija --

//---------------------------------------------
builder.Services.AddControllersWithViews();

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
