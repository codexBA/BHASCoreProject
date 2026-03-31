using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BHASCore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CijeneController : ControllerBase
    {
        public IActionResult Get()
        {   
            return Ok(
                new
                {
                    artikli = new[]
                    {    
                        new { naziv = "Narandza", cijena = 1.5 },
                        new { naziv = "Limunada", cijena = 2.0 },
                        new { naziv = "Kafa", cijena = 2.0 }
                    },
                    konobar = "Aco"
                }
            );
        }
    }
} 
