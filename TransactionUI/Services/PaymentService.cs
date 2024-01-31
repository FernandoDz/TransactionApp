using System.Text.Json;
using System.Text;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class PaymentService:IPaymentService
    {
        private readonly HttpClient httpClient;
        public PaymentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<bool> CreatePayment(Payment payment)
        {
            var purchaseJson = JsonSerializer.Serialize(payment);
            var content = new StringContent(purchaseJson, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync("Payment/Create", content);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Payment>> GetById(int id)
        {
            {
                var httpResponse = await httpClient.GetStringAsync($"Payment/Get/{id}");
                var response = JsonSerializer.Deserialize<PaymentResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return response.Response;

            }
        }
    }
}
