
using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class CreditCardService
    {
        private readonly string _connectionString;
        private readonly ILogger<CreditCardService> _logger;
        public CreditCardService(IConfiguration configuration, ILogger<CreditCardService> logger)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;

        }

        public List<CreditCard> GetAllCards()
        {
            List<CreditCard> cards = new List<CreditCard>();

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all credit cards. Method: GetAllCards");

            }

            return cards;
        }

        public List<CreditCard> SearchCreditCardByNumber(string cardNumber)
        {
            List<CreditCard> cards = new List<CreditCard>();

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while searching credit cards by number '{cardNumber}'. Method: SearchCreditCardByNumber");
            }

            return cards;
        }
    }
}
