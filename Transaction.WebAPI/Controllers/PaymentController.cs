using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly string CadenaSQL;
        public PaymentController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");

        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Payment objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_RegisterPayment", conexion);
                    cmd.Parameters.AddWithValue("ClientId", objeto.ClientId);
                    cmd.Parameters.AddWithValue("CardNumber", objeto.CardNumber);
                    cmd.Parameters.AddWithValue("PaymentDate", objeto.PaymentDate);
                    cmd.Parameters.AddWithValue("Amount", objeto.Amount);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
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
            List<Payment> list = new List<Payment>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_GetPaymentsByClientId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                    cmd.Parameters["@ClientId"].Value = ClientId;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Payment
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                CardNumber = rd["CardNumber"].ToString(),
                                PaymentDate = Convert.ToDateTime(rd["PaymentDate"]),
                                Amount = Convert.ToDecimal(rd["Amount"])


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
