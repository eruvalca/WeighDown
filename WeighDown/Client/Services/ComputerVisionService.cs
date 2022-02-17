using System.Net.Http.Json;
using System.Web;
using WeighDown.Shared;

namespace WeighDown.Client.Services
{
    public class ComputerVisionService
    {
        private readonly HttpClient _client;

        public ComputerVisionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<string>> PostWeightLogImageData(ComputerVisionDTO cpuVisionDto)
        {
            cpuVisionDto.Url = HttpUtility.UrlEncode(cpuVisionDto.Url);
            var response = await _client.PostAsJsonAsync("computervision", cpuVisionDto);
            return await response.Content.ReadFromJsonAsync<List<string>>();
        }
    }
}
