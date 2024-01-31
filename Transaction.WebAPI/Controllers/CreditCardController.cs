using Microsoft.AspNetCore.Mvc;
using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : Controller
    {
        private readonly CreditCardService _creditCardService;
        private readonly string CadenaSQL;
        public CreditCardController(IConfiguration configuration, CreditCardService creditCardService)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _creditCardService = creditCardService;
        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<CreditCard> cards = _creditCardService.GetAllCards();

            if (cards.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cards });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron tarjetas", response = cards });
            }
        }

        [HttpPost("Search")]
        public IActionResult Search(string cardNumber)
        {
            List<CreditCard> cards = _creditCardService.SearchCreditCardByNumber(cardNumber);

            if (cards.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cards });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron tarjetas con el número proporcionado", response = cards });
            }
        }
    }
}
