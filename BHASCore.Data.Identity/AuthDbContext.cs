using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace BHASCore.Data.Identity
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options)
        : IdentityDbContext(options)
    {
    }

    /// <summary>
    /// Ova klasa se koristi za inicijalizaciju baze podataka sa početnim podacima, 
    /// kao što su korisnici i role.
    /// </summary>
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // ubacit cemo nekoliko rola i korisnika
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // ovo su role koje dodajemo
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                // provjeravamo postoji li rola, ako ne postoji kreiramo je
                if (!await roleManager.RoleExistsAsync(role))
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }

            var adminUser = new Microsoft.AspNetCore.Identity.IdentityUser
            {
                UserName = "admin@bhas.ba",
                Email = "admin@bhas.ba",
                EmailConfirmed = true
            };
            
            // provjeravamo da li user postoji u bazi
            var userFound = await userManager.FindByEmailAsync(adminUser.Email);
            if (userFound == null) // ako user ne postoji => kreiramo i dodajemo u rolu
            {
                var password = "admin123";
                var userCreated = await userManager.CreateAsync(adminUser, password);
                if (userCreated.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
