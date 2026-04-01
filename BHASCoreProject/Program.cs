using BHASCore.Data.Business;
using BHASCore.Data.Business.Services;
using BHASCore.Data.Identity;
using BHASCore.Web.Controllers;
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


// business db context
var businessConnString = builder.Configuration.GetConnectionString("BusinessConnection")
    ?? throw new InvalidOperationException("Connection string 'BusinessConnection' not found.");

builder.Services.AddDbContext<BusinessDbContext>(options =>
    options.UseSqlServer(businessConnString));
// ---------------


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

#region -- DI - ovdje radimo registraciju servisa --

#region -- pricing servis - logika vezana za prikaz cijena u različitim valutama --
// dodavanje u container servisa - kako bi se mogao koristiti bilo gdje u aplikaciji (npr. u controllerima) - Dependency Injection
//builder.Services.AddScoped<IPricingService, PricingServiceEUR>(); // registracija servisa za cijene - da se moze koristiti u controllerima
builder.Services.AddScoped<IPricingService>(sp =>
{
    // logika za odabir servisa - npr. na osnovu konfiguracije ili nekog uvjeta
    var configService = sp.GetService<IConfiguration>();

    var currency = configService["AppSettings:Valuta"]; // u appsettings.json
    if (currency == "EUR")
    {
        return new PricingServiceEUR();
    }
    else
    {
        return new PricingServiceBAM();
    }
}); // registracija servisa za cijene - da se moze koristiti u controllerima

// builder.Services.AddScoped<IPricingService, PricingServiceBAM>(); // registracija servisa za cijene - da se moze koristiti u controllerima

#endregion -- pricing servis - logika vezana za prikaz cijena u različitim valutama --

builder.Services.AddScoped<IDepartmentService, DepartmentService>(); // registracija servisa za department - da se moze koristiti u controllerima
builder.Services.AddScoped<IEmployeeService, EmployeeService>(); // registracija servisa za employee - da se moze koristiti u controllerima

#endregion -- DI - ovdje radimo registraciju servisa --

var app = builder.Build(); // ovim se pokrece aplikacija

// add-migration naziv_migracije -Context AuthDbContext
using (var scope = app.Services.CreateScope())
{
    // AUTH / IDENTITY - DB - CONTEXT
    var services = scope.ServiceProvider;
    var authDbContext = services.GetService<AuthDbContext>();// dobvljamo servis
    //
    authDbContext!.Database.Migrate(); // pokrecemo migraciju baze podataka - kreiramo bazu i tablice ako ne postoje
    // nakon krerianja baze - pokrenut cemo Seed 
    await SeedData.Initialize(services);
    //---------------------------------------
    // BUSINESS - DB - CONTEXT
    var businessDbContext = services.GetService<BusinessDbContext>();// dobvljamo servis
    businessDbContext.Database.Migrate(); // pokrecemo migraciju baze podataka - kreiramo bazu i tablice ako ne postoje

    await SeedBusinessData.Initialize(services); // pokrecemo Seed za business bazu podataka - dodajemo pocetne podatke
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
