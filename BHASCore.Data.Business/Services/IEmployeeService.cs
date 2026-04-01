using BHASCore.Data.Business.Entities;
//

namespace BHASCore.Data.Business.Services
{
    /// <summary>
    /// Manipulisanje employee podacima - logika poslovanja vezana za zaposlene (Employee) 
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Vraca listu svih zaposlenih, bez obzira na odjel kojem pripadaju
        /// </summary>
        /// <returns></returns>
        Task<List<Employee>> GetAll();

        /// <summary>
        /// Vraca listu zaposlenih koji pripadaju odjelu sa zadanim ID-jem 
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        Task<List<Employee>> GetByDepartment(int departmentId);
    }
}
