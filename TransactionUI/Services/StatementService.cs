using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using TransactionUI.Models;

namespace TransactionUI.Services
{
    public class StatementService:IStatementService
    {
        private readonly HttpClient httpClient;
        public StatementService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        async Task<IEnumerable<Statement>> IStatementService.GetById(int id)
        {
            var httpResponse = await httpClient.GetStringAsync($"Statement/Get/{id}");
            var response = JsonSerializer.Deserialize<StatementResponse>(httpResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response.Response;
        }
    }
}
