using BApp.Domain.DTOs;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using System.Data;
using System.Net.Http.Json;
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

        public async Task<User> GetUserById(int id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"users/{id}");
        }

        public async Task UpdateUser(int id, User user)
        {
            var updatedUser = JsonSerializer.Serialize(user);
            var response = await _httpClient.PutAsync($"users/{id}", new StringContent(updatedUser, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
