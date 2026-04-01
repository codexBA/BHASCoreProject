using BHASCore.Data.Business.Entities;
using Microsoft.EntityFrameworkCore;
//

namespace BHASCore.Data.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BusinessDbContext _db;

        public EmployeeService(BusinessDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Daj listu svih zaposlenih. 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAll()
        {
            var employees = await _db.Employees.ToListAsync();
            return employees;
        }

        /// <summary>
        /// Vraca listu zaposlenih za dati odjel  
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<Employee>> GetByDepartment(int departmentId)
        {
            var employees = await _db.Employees.Where(e => e.DepartmentID == departmentId).ToListAsync();
            return employees;
        }
    }
}
