using AutoMapper;
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
        private readonly IMapper _mapper;
        public CreditCardController(IConfiguration configuration, CreditCardService creditCardService, IMapper mapper)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _creditCardService = creditCardService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<CreditCard> cards = _creditCardService.GetAllCards();
            var mappedCards = _mapper.Map<List<CreditCard>>(cards);

            if (mappedCards.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = mappedCards });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron tarjetas", response = mappedCards });
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
