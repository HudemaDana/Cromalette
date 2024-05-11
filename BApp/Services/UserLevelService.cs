using BApp.Domain.Models;
using BApp.Services.Interfaces;
using System.Net.Http.Json;

namespace BApp.Services
{
    public partial class UserLevelService : IUserLevelService
    {
        private readonly HttpClient _httpClient;

        public UserLevelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateUserLevelOnUserSignUp(int userId)
        {
            await _httpClient.PostAsync($"api/UserLevel/signup/{userId}", null);
        }

        public async Task DeleteUserLevelOnUserDelete(int userId)
        {
            await _httpClient.DeleteAsync($"api/UserLevel/delete/{userId}");
        }

        public async Task UpdateUserLevelOnColorSave(UserColor userColor)
        {
            await _httpClient.PostAsJsonAsync("api/UserLevel/colorsave", userColor);
        }
    }
}
