using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;

using System.Data;
using System.Data.SqlClient;
using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

namespace Transaction.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly string CadenaSQL;
        private readonly StatementService _statementService;

        public StatementController(IConfiguration configuration, StatementService statementService)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
            _statementService = statementService;
        }

        [HttpGet]
        [Route("List")]
        public IActionResult Lista()
        {
            List<Statement> statements = _statementService.GetAllStatements();

            if (statements.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = statements });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron estados de cuenta", response = statements });
            }
        }

        [HttpGet]
        [Route("Get/{ClientId:int}")]
        public IActionResult GetById(int ClientId)
        {
            List<Statement> statements = _statementService.GetStatementsByClientId(ClientId);

            if (statements.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = statements });
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, new { mensaje = "No se encontraron estados de cuenta para el cliente proporcionado", response = statements });
            }
        }
    }
}
