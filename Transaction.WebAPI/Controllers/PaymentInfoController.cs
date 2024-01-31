using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentInfoController : Controller
    {
        private readonly string CadenaSQL;
        public PaymentInfoController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<PaymentInfo> list = new List<PaymentInfo>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("GetTransactionPaymentInfoByClientId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                    cmd.Parameters["@ClientId"].Value = ClientId;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new PaymentInfo
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                CardNumber = rd["CardNumber"].ToString(),
                                Date = Convert.ToDateTime(rd["Date"]),
                                Description = rd["Description"].ToString(),
                                Payment = Convert.ToDecimal(rd["Payment"])
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = list });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = list });
            }
        }

    }
}

