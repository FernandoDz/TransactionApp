using Microsoft.Data.SqlClient;
using System.Data;
using Transaction.WebAPI.Models;

namespace Transaction.WebAPI.Services
{
    public class PaymentService
    {
        private readonly string _connectionString;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IConfiguration configuration , ILogger<PaymentService> logger )
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
            _logger = logger;

        }

        public void RegisterPayment(Payment payment)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("sp_RegisterPayment", connection))
                    {
                        cmd.Parameters.AddWithValue("ClientId", payment.ClientId);
                        cmd.Parameters.AddWithValue("CardNumber", payment.CardNumber);
                        cmd.Parameters.AddWithValue("PaymentDate", payment.PaymentDate);
                        cmd.Parameters.AddWithValue("Amount", payment.Amount);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a payment. Method: RegisterPayment");

            }
        }

        public List<Payment> GetPaymentsByClientId(int ClientId)
        {
            List<Payment> payments = new List<Payment>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("usp_GetPaymentsByClientId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int));
                        cmd.Parameters["@ClientId"].Value = ClientId;

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                payments.Add(new Payment
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving payments for a client. Method: GetPaymentsByClientId");
            }

            return payments;
        }
    }
}
