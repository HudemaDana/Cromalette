using BApp.DataAccess.Data;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(
            ApplicationDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public User GetUserById(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user != null)
                return user;
            else
            {
                throw new Exception("The user was not found");
            }
        }

        public async Task CreateUser(User user)
        {
            var isUserWithExistingUniqueData = _dbContext.Users.Any(u => u.Email == user.Email || u.Username == user.Username);
            if (isUserWithExistingUniqueData)
            {
                throw new Exception("Already exists a user with given email/username.");
            }
            else
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int Id)
        {
            var user = _dbContext.Users.Find(Id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cannot delete! The user was not found.");
            }
        }

        public async Task UpdateUser(int id, User user)
        {
            var userToUpdate = _dbContext.Users.Find(id);
            if (userToUpdate != null)
            {
                userToUpdate = user;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cannot update! The user was not found.");
            }
        }

        public async Task<string> AuthentificateUser(User user)
        {
            if (user != null && user.Email != null && user.Password != null)
            {
                var isFirstAuthentification = _dbContext.UserLevels.Any(ul => ul.UserId == user.Id);
                if (isFirstAuthentification)
                {
                    var userLevel = new UserLevel
                    {
                        UserId = user.Id,
                        LevelId = 1,
                        CurrentXP = 0
                    };
                    await _dbContext.UserLevels.AddAsync(userLevel);
                    await _dbContext.SaveChangesAsync();
                }
                var userData = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                if (userData != null)
                {
                    var token = GenerateToken(userData);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new Exception("Invalid credentials!");
                }
            }
            else
            {
                throw new Exception("No credentials recieved!");
            }
        }

        private JwtSecurityToken GenerateToken(User user)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", user.Username),
                        new Claim("Email", user.Email),
                        new Claim("Password", user.Password)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return token;
        }
    }
}
