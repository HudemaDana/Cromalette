using BApp.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BApp.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        public ImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPaletteColors(IBrowserFile file)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10); // Max 10 MB
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            var response = await _httpClient.PostAsync("upload", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var plotResponse = JsonSerializer.Deserialize<string>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return plotResponse;

            }

            return null;
        }

        public async Task<List<string>> GetColors(IBrowserFile file)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10); // Max 10 MB
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            var response = await _httpClient.PostAsync("colors", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(jsonResponse);
        }
    }
}
