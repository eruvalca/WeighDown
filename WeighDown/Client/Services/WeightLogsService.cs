using System.Net.Http.Json;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Services
{
    public class WeightLogsService
    {
        private readonly HttpClient _client;

        public WeightLogsService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<WeightLog>> GetWeightLogs()
        {
            return await _client.GetFromJsonAsync<List<WeightLog>>("weightlogs");
        }

        public async Task<WeightLog> GetWeightLog(int id)
        {
            return await _client.GetFromJsonAsync<WeightLog>($"weightlogs/{id}");
        }

        public async Task PutWeightLog(int id, WeightLog weightLog)
        {
            var response = await _client.PutAsJsonAsync($"weightlogs/{id}", weightLog);
            response.EnsureSuccessStatusCode();
        }

        public async Task<WeightLog> PostWeightLog(WeightLog weightLog)
        {
            var response = await _client.PostAsJsonAsync("weightlogs", weightLog);
            return await response.Content.ReadFromJsonAsync<WeightLog>();
        }

        public async Task DeleteWeightLog(int id)
        {
            var response = await _client.DeleteAsync($"weightlogs/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
