using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLevelController : ControllerBase
    {
        private readonly IUserLevelService _userLevelService;

        public UserLevelController(IUserLevelService userLevelService)
        {
            _userLevelService = userLevelService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLevel(int userId)
        {
            var userLevel = _userLevelService.GetUserLevel(userId);
            return Ok(userLevel);
        }

        [HttpPost("signup/{userId}")]
        public async Task<IActionResult> CreateUserLevelOnUserSignUp(int userId)
        {
            await _userLevelService.CreateUserLevelOnUserSignUp(userId);
            return Ok();
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUserLevelOnUserDelete(int userId)
        {
            await _userLevelService.DeleteUserLevelOnUserDelete(userId);
            return Ok();
        }

        [HttpPost("colorsave")]
        public async Task<IActionResult> UpdateUserLevelOnColorSave(UserColor userColor)
        {
            await _userLevelService.UpdateUserLevelOnColorSave(userColor);
            return Ok();
        }
    }
}
