﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeighDown.Server.Services;
using WeighDown.Shared;

namespace WeighDown.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _azureConnectionString;
        private readonly ComputerVisionService _computerVisionService;

        public UploadController(IConfiguration configuration, ComputerVisionService computerVisionService)
        {
            _azureConnectionString = configuration["AzureBlobConnectionString"];
            _computerVisionService = computerVisionService;
        }

        [HttpPost("weightlog/vision")]
        public async Task<ActionResult<ImageVisionDTO>> UploadWeightLogAndVision()
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
                    var fileExt = ".jpeg";
                    var newFileName = fileName + "_weightlog_" + DateTime.UtcNow.ToString("MM-dd-yyyy_HH-mm-tt") + fileExt;

                    var blob = container.GetBlobClient(newFileName);
                    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var image = new MagickImage(file.OpenReadStream()))
                        {
                            image.Format = MagickFormat.Jpeg;
                            image.Write(memoryStream);
                        }

                        using var fileStream = memoryStream;
                        fileStream.Position = 0;
                        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                    }

                    //var reads = await _computerVisionService.ReadFileUrl(blob.Uri.ToString());

                    return Ok(new ImageVisionDTO { Uri = blob.Uri.ToString(), Reads = new List<string>() });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                    var fileExt = ".jpeg";
                    var newFileName = fileName + "_weightlog_" + DateTime.UtcNow.ToString("MM-dd-yyyy_HH-mm-tt") + fileExt;

                    var blob = container.GetBlobClient(newFileName);
                    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var image = new MagickImage(file.OpenReadStream()))
                        {
                            if (image.Width > 600)
                            {
                                double ratio = 600.00 / image.Width;
                                Percentage percentage = new(ratio * 100.00);

                                var size = new MagickGeometry(percentage, percentage)
                                {
                                    IgnoreAspectRatio = false
                                };

                                image.Resize(size);
                            }

                            image.Format = MagickFormat.Jpeg;
                            image.Write(memoryStream);
                        }

                        using var fileStream = memoryStream;
                        fileStream.Position = 0;
                        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
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
