using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : Controller
    {
        private readonly string CadenaSQL;

        public PurchaseController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");

        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Purchase objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_RegisterPurchase", conexion);
                    cmd.Parameters.AddWithValue("ClientId", objeto.ClientId);
                    cmd.Parameters.AddWithValue("CardNumber", objeto.CardNumber);
                    cmd.Parameters.AddWithValue("PurchaseDate", objeto.PurchaseDate);
                    cmd.Parameters.AddWithValue("Description", objeto.Description);
                    cmd.Parameters.AddWithValue("Amount", objeto.Amount);
                    cmd.Parameters.AddWithValue("AuthorizationNumber", objeto.AuthorizationNumber);
                    cmd.Parameters.AddWithValue("Month", objeto.Month);
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
            List<Purchase> list = new List<Purchase>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_GetPurchaseByClientId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                    cmd.Parameters["@ClientId"].Value = ClientId;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Purchase
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                PurchaseDate = (rd["PurchaseDate"].ToString()),
                                Description = rd["Description"].ToString(),
                                Amount = Convert.ToDecimal(rd["Amount"]),
                                AuthorizationNumber = rd["AuthorizationNumber"].ToString(),
                                Month = rd["Month"].ToString()

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
