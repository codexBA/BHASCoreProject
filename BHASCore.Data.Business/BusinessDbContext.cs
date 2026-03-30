using Microsoft.EntityFrameworkCore;

namespace BHASCore.Data.Business
{
    public class BusinessDbContext(DbContextOptions<BusinessDbContext> options)
       : DbContext(options)
    {

    }
}
