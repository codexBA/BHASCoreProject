using BHASCore.Data.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BHASCore.Web.Controllers.API
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesApiController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() // dobavlja listu svih zaposlenih
        {
            var employees = await _employeeService.GetAll();
            return Ok(employees);
        }

        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetByDepartment(departmentId);
            return Ok(employees);
        }
    }
}
