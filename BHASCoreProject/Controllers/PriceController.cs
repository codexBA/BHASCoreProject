using BHASCore.Data.Identity;
using BHASCore.Web.Servis;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace BHASCore.Web.Controllers
{
    public class PriceController : Controller
    {
       IPricingService pricingService;
       
        public PriceController(IPricingService pc)
        {
            pricingService = pc;          
        }

        public IActionResult Index()
        {  
            ViewBag.Cijena = pricingService.GetFormattedPrice(2.5);
            //
            return View();
            // return View("View-NoviNaziv"); // ako zelimo da koristimo drugi View umjesto defaultng koji prati naziv metode/action-a/akcije
        }
    }

    public interface IPricingService
    {
        string GetFormattedPrice(double price);
    }

    public class PricingServiceBAM : IPricingService
    {
        public string GetFormattedPrice(double price)
        {
            return $"{price:N2} KM"; // za price = 2.5 => vratit ce "2.50 KM"
        }
    }

    public class PricingServiceEUR : IPricingService
    {
        public string GetFormattedPrice(double price)
        {
            return $"{price/1.955:N2} EUR"; // za price = 2.5 => vratit ce "1.28 EUR"
        }
    }
}
