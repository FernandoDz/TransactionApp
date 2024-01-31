using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class PurchaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(IConfiguration configuration, ILogger<PurchaseService> logger)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;
        }

        public void RegisterPurchase(Purchase purchase)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("sp_RegisterPurchase", connection))
                    {
                        cmd.Parameters.AddWithValue("ClientId", purchase.ClientId);
                        cmd.Parameters.AddWithValue("CardNumber", purchase.CardNumber);
                        cmd.Parameters.AddWithValue("PurchaseDate", purchase.PurchaseDate);
                        cmd.Parameters.AddWithValue("Description", purchase.Description);
                        cmd.Parameters.AddWithValue("Amount", purchase.Amount);
                        cmd.Parameters.AddWithValue("AuthorizationNumber", purchase.AuthorizationNumber);
                        cmd.Parameters.AddWithValue("Month", purchase.Month);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a purchase. Method: RegisterPurchase");
            }
        }

        public List<Purchase> GetPurchasesByClientId(int ClientId)
        {
            List<Purchase> purchases = new List<Purchase>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("usp_GetPurchaseByClientId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                        cmd.Parameters["@ClientId"].Value = ClientId;

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                purchases.Add(new Purchase
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving purchases for a client. Method: GetPurchasesByClientId");
            }

            return purchases;
        }
    }
}
