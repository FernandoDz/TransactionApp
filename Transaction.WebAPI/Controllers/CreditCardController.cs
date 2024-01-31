using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : Controller
    {
        private readonly string CadenaSQL;
        public CreditCardController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<CreditCard> cards = new List<CreditCard>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_GetAllCards", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            cards.Add(new CreditCard
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                Number = rd["Number"].ToString(),
                                HolderFirstName = rd["HolderFirstName"].ToString(),
                                HolderLastName = rd["HolderLastName"].ToString(),
                                Top_Aux = rd["Top_Aux"] != DBNull.Value ? Convert.ToInt32(rd["Top_Aux"]) : 0  

                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cards });
            }

            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = cards });
            }
        }

        [HttpPost("Search")]

        public IActionResult Search(string cardNumber)
        {
            List<CreditCard> cards = new List<CreditCard>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_SearchCreditCardByNumber", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CardNumber", cardNumber);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            cards.Add(new CreditCard
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                Number = rd["Number"].ToString(),
                                HolderFirstName = rd["HolderFirstName"].ToString(),
                                HolderLastName = rd["HolderLastName"].ToString(),
                                Top_Aux = rd["Top_Aux"] != DBNull.Value ? Convert.ToInt32(rd["Top_Aux"]) : 0
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cards });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = cards });
            }
        }
    }
}
