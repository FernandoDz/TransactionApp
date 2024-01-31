using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly string CadenaSQL;
        private readonly PurchaseService _purchaseService;
        private readonly PurchaseValidator _purchaseValidator;

        public PurchaseController(IConfiguration configuration, PurchaseService purchaseService, PurchaseValidator purchaseValidator)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _purchaseService = purchaseService;
            _purchaseValidator = purchaseValidator;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Purchase purchase)
        {
            ValidationResult validationResult = _purchaseValidator.Validate(purchase);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                _purchaseService.RegisterPurchase(purchase);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<Purchase> purchases = _purchaseService.GetPurchasesByClientId(ClientId);

            if (purchases.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = purchases });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron compras para el cliente proporcionado", response = purchases });
            }
        }
    }
}
