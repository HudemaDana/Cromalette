using BApp.Domain.DTOs;
using BApp.Services.Interfaces;
using System.Text;
using WBizTrip.Client.Authentication;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateUser(SignUpDTO signUpModel)
        {
            var newSignUpForUser = JsonSerializer.Serialize(signUpModel);
            await _httpClient.PostAsync("Users/create", new StringContent(newSignUpForUser, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> LoginUser(LoginRequest userDto)
        {
            var newSignUpForUser = JsonSerializer.Serialize(userDto);
            var result = await _httpClient.PostAsync("users/login", new StringContent(newSignUpForUser, Encoding.UTF8, "application/json"));
            return result;
        }
    }
}
