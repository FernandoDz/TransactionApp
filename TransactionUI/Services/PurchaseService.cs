using System.Text;
using System.Text.Json;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly HttpClient httpClient;
        public PurchaseService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> CreatePurchase(Purchase purchase)
        {
            var purchaseJson = JsonSerializer.Serialize(purchase);
            var content = new StringContent(purchaseJson, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync("Purchase/Create", content);

            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Purchase>> GetById(int id)
        {
            var httpResponse = await httpClient.GetStringAsync($"Purchase/Get/{id}");
            var response = JsonSerializer.Deserialize<PurchaseResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response.Response;

        }
    }
}
