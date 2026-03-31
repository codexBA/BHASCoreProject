using BHASCore.Data.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHASCoreProject.Controllers
{
    public class DepartmentController : Controller
    { 
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Konstruktor koji prima IDepartmentService kao zavisnost (dependency) i inicijalizuje privatno 
        /// polje _departmentService koje se koristi za pristup funkcionalnostima vezanim za odeljenja (Department) 
        /// u okviru ovog kontrolera.
        /// </summary>
        /// <param name="departmentService"></param>
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var odjeli = await _departmentService.GetAll();
            return View("Svi", odjeli);
        }
    }
}
