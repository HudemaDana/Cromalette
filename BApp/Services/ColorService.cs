using System.Net.Http.Json;
using BApp.Services.Interfaces;

namespace BApp.Services
{
    public partial class ColorService : IColorService
    {
        private readonly HttpClient _httpClient;

        public ColorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GenerateTints(string hexColor, int count)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/Colors/tints?hexColor={hexColor}&count={count}");
        }

        public async Task<List<string>> GenerateShades(string hexColor, int count)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/Colors/shades?hexColor={hexColor}&count={count}");
        }

    }
}
