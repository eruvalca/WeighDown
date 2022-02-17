using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeighDown.Server.Services;
using WeighDown.Shared;

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

        [HttpPost]
        public async Task<ActionResult<List<string>>> GetWeightLogImageData(ComputerVisionDTO cpuVisionDto)
        {
            return await _computerVisionService.ReadFileUrl(cpuVisionDto.Url);
        }
    }
}
