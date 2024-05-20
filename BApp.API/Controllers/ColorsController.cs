using BApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet("tints")]
        public async Task<ActionResult<List<string>>> GenerateTints([FromQuery] string hexColor, [FromQuery] int count)
        {
            var tints = await _colorService.GenerateTints(hexColor, count);
            return Ok(tints);
        }

        [HttpGet("shades")]
        public async Task<ActionResult<List<string>>> GenerateShadesAsync([FromQuery] string hexColor, [FromQuery] int count)
        {
            var shades = await _colorService.GenerateShades(hexColor, count);
            return Ok(shades);
        }
    }
}
