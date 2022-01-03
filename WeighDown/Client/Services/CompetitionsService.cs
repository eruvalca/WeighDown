using System.Net.Http.Json;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Services
{
    public class CompetitionsService
    {
        private readonly HttpClient _client;

        public CompetitionsService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Competition>> GetCompetitions()
        {
            return await _client.GetFromJsonAsync<List<Competition>>("competitions");
        }

        public async Task<Competition> GetCompetition(int id)
        {
            return await _client.GetFromJsonAsync<Competition>($"competitions/{id}");
        }

        public async Task PutCompetition(int id, Competition competition)
        {
            var response = await _client.PutAsJsonAsync($"competitions/{id}", competition);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Competition> PostCompetition(Competition competition)
        {
            var response = await _client.PostAsJsonAsync("competitions", competition);
            return await response.Content.ReadFromJsonAsync<Competition>();
        }

        public async Task DeleteCompetition(int id)
        {
            var response = await _client.DeleteAsync($"competitions/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
