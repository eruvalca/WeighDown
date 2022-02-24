using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeighDown.Shared;

namespace WeighDown.Client.Services
{
    public class UploadService
    {
        private readonly HttpClient _client;

        public UploadService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ImageVisionDTO> UploadWeightLogAndVision(IBrowserFile file)
        {
            using var ms = file.OpenReadStream(file.Size);
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(ms, Convert.ToInt32(file.Size)), "image", file.Name);

            var response = await _client.PostAsync("upload/weightlog/vision", content);
            return await response.Content.ReadFromJsonAsync<ImageVisionDTO>();
        }

        public async Task<string> UploadWeightLogImage(IBrowserFile file)
        {
            using var ms = file.OpenReadStream(file.Size);
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(ms, Convert.ToInt32(file.Size)), "image", file.Name);

            var response = await _client.PostAsync("upload/weightlog", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
