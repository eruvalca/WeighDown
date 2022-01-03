using System.Net.Http.Json;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Services
{
    public class ContestantsService
    {
        private readonly HttpClient _client;

        public ContestantsService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Contestant>> GetContestants()
        {
            return await _client.GetFromJsonAsync<List<Contestant>>("contestants");
        }

        public async Task<Contestant> GetContestant(string userId)
        {
            return await _client.GetFromJsonAsync<Contestant>($"contestants/{userId}");
        }

        public async Task PutContestant(int id, Contestant contestant)
        {
            var response = await _client.PutAsJsonAsync($"contestants/{id}", contestant);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Contestant> PostContestant(Contestant contestant)
        {
            var response = await _client.PostAsJsonAsync("contestants", contestant);
            return await response.Content.ReadFromJsonAsync<Contestant>();
        }

        public async Task DeleteContestant(int id)
        {
            var response = await _client.DeleteAsync($"contestants/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
