using BApp.Domain.DTOs;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserColorsController : ControllerBase
    {
        private readonly IUserColorService _userColorService;

        public UserColorsController(IUserColorService userColorService)
        {
            _userColorService = userColorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserColor>>> GetAllUserColors()
        {
            var userColors = await _userColorService.GetAllUserColors();
            return Ok(userColors);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserColor>>> GetUserColorsByUserId(int userId)
        {
            var userColors = await _userColorService.GetUserColorsByUserId(userId);
            return Ok(userColors);
        }

        [HttpGet("{hexValue}/{userId}")]
        public async Task<ActionResult<UserColor>> GetUserColorByHexValueAndUserId(string hexValue, int userId)
        {
            var userColor = await _userColorService.GetUserColorByHexValueAndUserId(hexValue, userId);
            if (userColor == null)
                return NotFound();
            return Ok(userColor);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserColor(UserColorDTO userColor)
        {
            await _userColorService.AddUserColor(userColor);
            return Ok();
        }
    }

}
