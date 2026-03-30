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
    (
        options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        }
    )
    .AddRoles<IdentityRole>()// ako zelimo koristiti role (admin, user, editor...)
    .AddEntityFrameworkStores<AuthDbContext>();// da se koristi AuthDbContext kao skladište za korisničke podatke i uloge

#endregion -- Identity i Entity Framework Core konfiguracija --

//---------------------------------------------
builder.Services.AddControllersWithViews();

var app = builder.Build(); // ovim se pokrece aplikacija
// add-migration naziv_migracije -Context AuthDbContext
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var authDbContext = services.GetService<AuthDbContext>();// dobvljamo servis
    //
    authDbContext!.Database.Migrate(); // pokrecemo migraciju baze podataka - kreiramo bazu i tablice ako ne postoje
    // nakon krerianja baze - pokrenut cemo Seed 
}

// nakon pokretanja aplikacije pokrecemo migraciju / kreiranje baze


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
