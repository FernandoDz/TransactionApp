using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;

using System.Data;
using System.Data.SqlClient;
using Transaction.WebAPI.Models;
namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly string CadenaSQL;

        public StatementController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");

        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<Statement> statements = new List<Statement>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_GetAllStatements", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            statements.Add(new Statement
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                CardNumber = rd["CardNumber"].ToString(),
                                CurrentBalance = Convert.ToDecimal(rd["CurrentBalance"]),
                                CreditLimit = Convert.ToDecimal(rd["CreditLimit"]),
                                BonifiableInterest = Convert.ToDecimal(rd["BonifiableInterest"]),
                                AvailableBalance = Convert.ToDecimal(rd["AvailableBalance"])
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = statements });
            }

            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = statements });
            }
        }
        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<Statement> list = new List<Statement>();

            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("usp_GetStatementsByClientId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                    cmd.Parameters["@ClientId"].Value = ClientId;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new Statement
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                ClientId = Convert.ToInt32(rd["ClientId"]),
                                CardNumber = rd["CardNumber"].ToString(),
                                CurrentBalance = Convert.ToDecimal(rd["CurrentBalance"]),
                                CreditLimit = Convert.ToDecimal(rd["CreditLimit"]),
                                BonifiableInterest = Convert.ToDecimal(rd["BonifiableInterest"]),
                                AvailableBalance = Convert.ToDecimal(rd["AvailableBalance"])
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
