using BApp.Domain.DTOs;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using System.Net.Http.Json;

namespace BApp.Services
{
    public class UserColorService: IUserColorService
    {
        private readonly HttpClient _httpClient;

        public UserColorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserColor>> GetAllUserColors()
        {
            return await _httpClient.GetFromJsonAsync<List<UserColor>>("UserColors");
        }

        public async Task<List<UserColor>> GetUserColorsByUserId(int userId)
        {
            return await _httpClient.GetFromJsonAsync<List<UserColor>>($"UserColors/user/{userId}");
        }

        public async Task<UserColor> GetUserColorByHexValueAndUserId(string hexValue, int userId)
        {
            return await _httpClient.GetFromJsonAsync<UserColor>($"UserColors/{hexValue}/{userId}");
        }

        public async Task AddUserColor(UserColorDTO userColor)
        {
            await _httpClient.PostAsJsonAsync("UserColors", userColor);
        }
    }
}
