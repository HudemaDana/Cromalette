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

        [HttpGet("tones")]
        public async Task<ActionResult<List<string>>> GenerateTonesAsync([FromQuery] string hexColor, [FromQuery] int count)
        {
            var tones = await _colorService.GenerateTones(hexColor, count);
            return Ok(tones);
        }

        [HttpGet("palette")]
        public async Task<ActionResult<List<string>>> GeneratePalette([FromQuery] string hexColor, [FromQuery] int ruleNr)
        {
            var palette = await _colorService.GeneratePalette(hexColor, ruleNr);
            return Ok(palette);
        }
    }
}
