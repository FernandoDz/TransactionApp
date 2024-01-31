using System.Text.Json;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class CreditCardService:ICreditCardService
    {
        private readonly HttpClient httpClient;
        public CreditCardService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<CreditCard>> GetAllCreditCards()
        {
            var httpResponse = await httpClient.GetStringAsync("CreditCard/List");
            var response = JsonSerializer.Deserialize<CreditCardResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response.Response;
        }

    }
}
