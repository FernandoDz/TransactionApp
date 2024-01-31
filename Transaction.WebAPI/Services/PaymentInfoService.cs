using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class PaymentInfoService
    {

        private readonly string _connectionString;
        private readonly ILogger<PaymentInfoService> _logger;

        public PaymentInfoService(IConfiguration configuration ,ILogger<PaymentInfoService> logger)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;
        }

        public List<PaymentInfo> GetTransactionPaymentInfoByClientId(int ClientId)
        {
            List<PaymentInfo> paymentInfoList = new List<PaymentInfo>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("GetTransactionPaymentInfoByClientId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                        cmd.Parameters["@ClientId"].Value = ClientId;

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                paymentInfoList.Add(new PaymentInfo
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving payment information for a client. Method: GetTransactionPaymentInfoByClientId");
            }

            return paymentInfoList;
        }
    }
}
