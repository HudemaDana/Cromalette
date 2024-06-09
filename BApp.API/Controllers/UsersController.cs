﻿using BApp.Domain.DTOs;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] SignUpDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Password = userDto.Password,
                    UserLevel = null,

                };
                await _userService.CreateUser(user);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] User user)
        {
            try
            {
                await _userService.UpdateUser(id, user);
                return Ok();
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> AuthentificateUser([FromBody] LoginRequest userDto)
        {
            try
            {
                var user = new User
                {
                    Username = "",
                    Email = userDto.Email,
                    FirstName = "",
                    LastName = "",
                    Password = userDto.Password,
                    UserLevel = null,
                    UserColors = null

                };

                var token = await _userService.AuthentificateUser(user);
                return Ok(token);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

    }
}
