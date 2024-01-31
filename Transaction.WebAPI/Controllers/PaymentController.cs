using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly string CadenaSQL;
        private readonly PaymentService _paymentService;
        private readonly PaymentValidator _paymentValidator;

        public PaymentController(IConfiguration configuration, PaymentService paymentService , PaymentValidator paymentValidator)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _paymentService = paymentService;
            _paymentValidator = paymentValidator;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Payment payment)
        {
            ValidationResult validationResult = _paymentValidator.Validate(payment);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                _paymentService.RegisterPayment(payment);
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
            List<Payment> payments = _paymentService.GetPaymentsByClientId(ClientId);

            if (payments.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = payments });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron pagos para el cliente proporcionado", response = payments });
            }
        }
    }
}
