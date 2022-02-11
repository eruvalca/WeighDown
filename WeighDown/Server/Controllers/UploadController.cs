using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace WeighDown.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _azureConnectionString;

        public UploadController(IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _azureConnectionString = configuration["AzureBlobConnectionString"];
            }
            else
            {
                _azureConnectionString = configuration.GetValue<string>("AzureBlobConnectionString");
            }
        }

        [HttpPost("weightlog")]
        public async Task<IActionResult> UploadWeightLog()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files[0];

                if (file.Length > 0)
                {
                    var container = new BlobContainerClient(_azureConnectionString, "upload-container");
                    var createResponse = await container.CreateIfNotExistsAsync();

                    if (createResponse is not null && createResponse.GetRawResponse().Status == 201)
                    {
                        await container.SetAccessPolicyAsync(PublicAccessType.Blob);
                    }

                    var fileName = file.FileName.Split(".")[0];
                    var fileExt = "." + file.FileName.Split(".")[1];
                    var newFileName = fileName + "_weightlog_" + DateTime.UtcNow.ToString("MM-dd-yyyy_HH-mm-tt") + fileExt;

                    var blob = container.GetBlobClient(newFileName);
                    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                    using (var memoryStream = new MemoryStream())
                    {
                        using (Image image = Image.Load(file.OpenReadStream()))
                        {
                            if (image.Width > 600)
                            {
                                image.Mutate(x => x.Resize(600, 0));
                            }

                            image.SaveAsJpeg(memoryStream);
                        }

                        using (var fileStream = memoryStream)
                        {
                            fileStream.Position = 0;
                            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                        }
                    }

                    return Ok(blob.Uri.ToString());
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
