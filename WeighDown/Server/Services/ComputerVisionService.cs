using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Web;

namespace WeighDown.Server.Services
{
    public class ComputerVisionService
    {
        private readonly string _subKey;
        private readonly string _endpoint;
        private readonly ComputerVisionClient _client;

        public ComputerVisionService(IConfiguration configuration)
        {
            _subKey = configuration["AzureComputerVisionSubKey"];
            _endpoint = configuration["AzureComputerVisionEndpoint"];

            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subKey))
            {
                Endpoint = _endpoint
            };
        }

        public async Task<List<string>> ReadFileUrl(string url)
        {
            var returnResult = new List<string>();

            url = HttpUtility.UrlDecode(url);

            var textHeaders = await _client.ReadAsync(url);
            string operationLocation = textHeaders.OperationLocation;
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await _client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            var textUrlFileResults = results.AnalyzeResult.ReadResults;

            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    returnResult.Add(line.Text);
                }
            }

            return returnResult;
        }
    }
}
