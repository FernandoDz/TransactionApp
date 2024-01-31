using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentInfoController : Controller
    {
        private readonly string CadenaSQL;
        private readonly PaymentInfoService _paymentInfoService;

        public PaymentInfoController(IConfiguration configuration, PaymentInfoService paymentInfoService)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _paymentInfoService = paymentInfoService;
        }

        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<PaymentInfo> paymentInfoList = _paymentInfoService.GetTransactionPaymentInfoByClientId(ClientId);

            if (paymentInfoList.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = paymentInfoList });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron detalles de pagos para el cliente proporcionado", response = paymentInfoList });
            }
        }
    }

}


