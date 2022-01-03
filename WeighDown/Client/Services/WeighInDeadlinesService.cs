using System.Net.Http.Json;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Services
{
    public class WeighInDeadlinesService
    {
        private readonly HttpClient _client;

        public WeighInDeadlinesService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<WeighInDeadline>> GetWeighInDeadlinesByCompetition(int id)
        {
            return await _client.GetFromJsonAsync<List<WeighInDeadline>>($"weighInDeadlines/competition/{id}");
        }

        public async Task<WeighInDeadline> GetWeighInDeadline(int id)
        {
            return await _client.GetFromJsonAsync<WeighInDeadline>($"weighInDeadlines/{id}");
        }

        public async Task PutWeighInDeadline(int id, WeighInDeadline weighInDeadline)
        {
            var response = await _client.PutAsJsonAsync($"weighInDeadlines/{id}", weighInDeadline);
            response.EnsureSuccessStatusCode();
        }

        public async Task<WeighInDeadline> PostWeighInDeadline(WeighInDeadline weighInDeadline)
        {
            var response = await _client.PostAsJsonAsync("weighInDeadlines", weighInDeadline);
            return await response.Content.ReadFromJsonAsync<WeighInDeadline>();
        }

        public async Task DeleteWeighInDeadline(int id)
        {
            var response = await _client.DeleteAsync($"weighInDeadlines/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteWeighInDeadlineRangeByCompetition(int id)
        {
            var response = await _client.DeleteAsync($"weighInDeadlines/competition/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
