using System.Net.Http.Json;
using BApp.Services.Interfaces;

namespace BApp.Services
{
    public class ColorService : IColorService
    {
        private readonly HttpClient _httpClient;

        public ColorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GenerateTints(string hexColor, int count)
        {
            var hexValue = "%23" + hexColor.Substring(1);
            return await _httpClient.GetFromJsonAsync<List<string>>($"colors/tints?hexColor={hexValue}&count={count}");
        }

        public async Task<List<string>> GenerateShades(string hexColor, int count)
        {
            var hexValue = "%23" + hexColor.Substring(1);
            return await _httpClient.GetFromJsonAsync<List<string>>($"colors/shades?hexColor={hexValue}&count={count}");
        }

    }
}
