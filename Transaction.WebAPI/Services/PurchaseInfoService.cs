using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class PurchaseInfoService
    {
        private readonly string _connectionString;
        private readonly ILogger<PurchaseInfoService> _logger;

        public PurchaseInfoService(IConfiguration configuration, ILogger<PurchaseInfoService> logger)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;
        }

        public List<PurchaseInfo> GetTransactionInfoByClientId(int ClientId)
        {
            List<PurchaseInfo> purchaseInfoList = new List<PurchaseInfo>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("GetTransactionInfoByClientId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                        cmd.Parameters["@ClientId"].Value = ClientId;

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                purchaseInfoList.Add(new PurchaseInfo
                                {
                                    Id = Convert.ToInt32(rd["Id"]),
                                    ClientId = Convert.ToInt32(rd["ClientId"]),
                                    CardNumber = rd["CardNumber"].ToString(),
                                    AuthorizationNumber = rd["AuthorizationNumber"].ToString(),
                                    Date = Convert.ToDateTime(rd["Date"]),
                                    Description = rd["Description"].ToString(),
                                    Charge = Convert.ToDecimal(rd["Charge"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving purchases for a client. Method: GetTransactionInfoByClientId");
            }

            return purchaseInfoList;
        }
    }
}
