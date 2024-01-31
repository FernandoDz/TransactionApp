using System.Text.Json;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class PaymentInfoService : IPaymentInfoService
    {
        private readonly HttpClient httpClient;
        public PaymentInfoService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<PaymentInfo>> GetById(int id)
        {
            var httpResponse = await httpClient.GetStringAsync($"PaymentInfo/Get/{id}");
            var response = JsonSerializer.Deserialize<PaymentInfoResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response.Response;


        }
    }
}
