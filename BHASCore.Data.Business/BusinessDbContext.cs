using BHASCore.Data.Business.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BHASCore.Data.Business
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                 .HasIndex(e => e.JMBG)
                 .IsUnique(); // osigurava jedinstvenost JMBG-a

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique(); // osigurava jedinstvenost Email-a
        }


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}