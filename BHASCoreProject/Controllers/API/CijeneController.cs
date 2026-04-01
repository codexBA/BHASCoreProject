using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BHASCore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CijeneController : ControllerBase
    {
        IPricingService _pricingService;

        /// <summary>
        /// Radimo injektovanje servisa za cijene u konstruktoru kontrolera - da bi mogli koristiti logiku vezanu za cijene u akcijama kontrolera
        /// </summary>
        /// <param name="pricingService"></param>
        public CijeneController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpGet("narudzba")]
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

        [Route("/api/formatirana-cijena")] // ovo je alternativni način definiranja rute - bez obzira na naziv kontrolera, ruta će biti api/formatirana-cijena
        //[HttpGet("FormatiranaCijena")] // api/cijene/formatiranacijena?cijena=1.5
        public IActionResult GetFormattedPrice([FromQuery] double cijena)
        {
            var formatiranaCijena = _pricingService.GetFormattedPrice(cijena);
            var rezultat = new
            {
                poruka = "evo formatirane cijene",
                cijena = formatiranaCijena
            };
            return Ok(rezultat);
        }


        /// <summary>
        /// Jednostavna provjera da li je poslan ispravan API ključ u headeru zahtjeva - ako nije, 
        /// vraćamo 401 Unauthorized
        /// </summary>
        /// <param name="zahtjev"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpPost("naruci")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Naruci([FromBody] Narudzba zahtjev, [FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            if (apiKey == null || apiKey != "NasTajniKljuc")
            { 
                return Unauthorized(); // ako nema api ključa ili je pogrešan, vraćamo 401 Unauthorized
            }

            if (zahtjev.Proizvodi.Any(x => x.Cijena <= 0))
                return BadRequest(); // ako je cijena nekog proizvoda negativna ili nula, vraćamo 400 Bad Request - ne možemo naručiti proizvod koji nema cijenu

            double ukupnaCijena = 0;
            foreach (var proizvod in zahtjev.Proizvodi)
            {
                ukupnaCijena += proizvod.Cijena * proizvod.Kolicina;
            }

            var formatiranaCijena = _pricingService.GetFormattedPrice(ukupnaCijena);

            Rezultat rez = new Rezultat
            {
                Poruka = "Narudžba kompletirana",
                Total = formatiranaCijena,
                Naruceno = zahtjev
            };

            return Ok(rez);

            // alternativno, mogli smo i ovako vratiti rezultat - bez definiranja posebne klase Rezultat, nego direktno u anonimnom tipu
            // return Created("", new { poruka = "narudzba uspjesna", ukupnaCijena = ukupnaCijena });
        }

        public class Rezultat
        {
            public string Poruka { get; set; }
            public string Total { get; set; }

            public Narudzba Naruceno { get; set; }
        }

        /// <summary>
        /// Klasa koja predstavlja narudžbu - sadrži informacije o kupcu, konobaru i listu proizvoda koje je kupac naručio
        /// </summary>
        public class Narudzba
        {
            public string Kupac { get; set; } // to smo mi
            public string Konobar { get; set; } // Aco
            public List<Proizvod> Proizvodi { get; set; } // Narandza, Limunada, Kafa
        }

        /// <summary>
        /// Jednostavna klasa koja predstavlja proizvod - sadrži naziv i cijenu proizvoda
        /// </summary>
        public class Proizvod
        {
            public string Naziv { get; set; }
            public double Cijena { get; set; }

            public int Kolicina { get; set; } = 1;
        }
    }
}
