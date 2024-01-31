using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseInfoController : Controller
    {
        private readonly string CadenaSQL;
        private readonly PurchaseInfoService _purchaseInfoService;

        public PurchaseInfoController(IConfiguration configuration, PurchaseInfoService purchaseInfoService)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _purchaseInfoService = purchaseInfoService;
        }

        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<PurchaseInfo> purchaseInfoList = _purchaseInfoService.GetTransactionInfoByClientId(ClientId);

            if (purchaseInfoList.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = purchaseInfoList });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron detalles de compras para el cliente proporcionado", response = purchaseInfoList });
            }
        }
    }
}
