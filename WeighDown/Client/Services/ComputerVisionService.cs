using System.Net.Http.Json;
using System.Web;

namespace WeighDown.Client.Services
{
    public class ComputerVisionService
    {
        private readonly HttpClient _client;

        public ComputerVisionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<string>> GetWeightLogImageData(string url)
        {
            url = HttpUtility.UrlEncode(url);
            return await _client.GetFromJsonAsync<List<string>>($"computervision/weightlog/{url}");
        }
    }
}
