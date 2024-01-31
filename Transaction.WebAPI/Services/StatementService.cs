using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class StatementService
    {
        private readonly string _connectionString;
        private readonly ILogger<StatementService> _logger;
        public StatementService(IConfiguration configuration, ILogger<StatementService> logger)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;
        }

        public List<Statement> GetAllStatements()
        {
            List<Statement> statements = new List<Statement>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("usp_GetAllStatements", connection))
                    {
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving statements for a client. Method: GetAllStatements");
            }

            return statements;
        }

        public List<Statement> GetStatementsByClientId(int ClientId)
        {
            List<Statement> statements = new List<Statement>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("usp_GetStatementsByClientId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                        cmd.Parameters["@ClientId"].Value = ClientId;

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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving statements for a client. Method: GetStatementsByClientId");
            }

            return statements;
        }
    }
}
