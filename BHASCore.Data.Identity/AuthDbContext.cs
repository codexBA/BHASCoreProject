using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BHASCore.Data.Identity
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) 
        : IdentityDbContext(options)
    {
    }
}
