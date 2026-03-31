using BHASCore.Data.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;


namespace BHASCore.Data.Business
{
    public static class SeedBusinessData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<BusinessDbContext>>();
            
            using var dbContext = new BusinessDbContext(options);
            // provjeravamo da li već postoje podaci u tabeli Departments
            var departmentsExist = await dbContext.Departments.AnyAsync();
            if (departmentsExist)// ako postoje podaci, ne radimo ništa i izlazimo iz metode
            {
                return;
            }
            //
            var odjelStatistika = new Department
            {
                DepartmentName = "Statistika stanovništva",
                DepartmentCode = "STAT-STAN",
                Budget = 1000000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                // DepartmentID = 1 - auto-incremented by the database
            };

            var odjelEkonomskaStatistika = new Department
            {
                DepartmentName = "Ekonomska statistika",
                DepartmentCode = "STAT-EKO",
                Budget = 5000000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };

            var odjelInformatika = new Department
            {
                DepartmentName = "Informatika - IT",
                DepartmentCode = "IT",
                Budget = 7000000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };

            // dodajemo odjele u bazu podataka
            await dbContext.Departments.AddRangeAsync(odjelStatistika, odjelEkonomskaStatistika, odjelInformatika);
            await dbContext.SaveChangesAsync(); // snimamo promjene u bazu podataka
            //
            var emp1 = new Employee
            {
                FirstName = "Ivan",
                LastName = "Ivić",
                Email = "ivan@bhas.ba",
                JMBG = "1234567890123",
                Position = "Statistički analitičar",
                Salary = 3000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DepartmentID = odjelStatistika.DepartmentID // veza sa odjelom statistike
            };

            var emp2 = new Employee
            {
                FirstName = "Emir",
                LastName = "Emirović",
                Email = "emir@bhas.ba",
                JMBG = "1234567890124",
                Position = "Direktor",
                Salary = 5000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DepartmentID = odjelEkonomskaStatistika.DepartmentID // veza sa odjelom ekon.statistike
            };

            var emp3 = new Employee
            {
                FirstName = "Damir",
                LastName = "Damirović",
                Email = "damir@bhas.ba",
                JMBG = "1234567890125",
                Position = "Menadžer",
                Salary = 3000m,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DepartmentID = odjelInformatika.DepartmentID // veza sa odjelom informatike
            };

            // dodajemo zaposlenike u bazu podataka
            await dbContext.Employees.AddRangeAsync(emp1, emp2, emp3);
            await dbContext.SaveChangesAsync(); // snimamo promjene u bazu podataka
        }
    }
}