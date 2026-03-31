using BHASCore.Data.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
//
using Microsoft.EntityFrameworkCore;

namespace BHASCore.Data.Business.Services
{
    /// <summary>
    /// Manipulisanje department podacima - logika poslovanja vezana za odeljenja (Department) 
    /// - CRUD operacije, validacija, poslovna pravila, itd.
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly BusinessDbContext _db;

        public DepartmentService(BusinessDbContext db)
        {
            _db = db;
        }

        public async Task<List<Department>> GetAll()
        {
            var departments = await _db.Departments
                                    //.Include(x=>x.Employees)
                                    .ToListAsync();
            return departments;
        }
    }
}
