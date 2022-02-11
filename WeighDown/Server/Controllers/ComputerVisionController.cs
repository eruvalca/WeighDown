using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeighDown.Server.Services;

namespace WeighDown.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ComputerVisionController : ControllerBase
    {
        private readonly ComputerVisionService _computerVisionService;

        public ComputerVisionController(ComputerVisionService computerVisionService)
        {
            _computerVisionService = computerVisionService;
        }

        [HttpGet("weightlog/{url}")]
        public async Task<ActionResult<List<string>>> GetWeightLogImageData(string url)
        {
            return await _computerVisionService.ReadFileUrl(url);
        }
    }
}
