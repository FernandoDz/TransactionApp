using System.Text.Json;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class PurchaseInfoService : IPurchaseInfoService
    {
        private readonly HttpClient httpClient;
        public PurchaseInfoService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<PurchaseInfo>> GetById(int id)
        {
            var httpResponse = await httpClient.GetStringAsync($"PurchaseInfo/Get/{id}");
            var response = JsonSerializer.Deserialize<PurchaseInfoResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response.Response;
        }
    }
}
