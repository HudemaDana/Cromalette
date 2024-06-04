using BApp.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IdentityModel.Tokens.Jwt;

namespace BApp.Pages
{
    public partial class ColorDetector : ComponentBase
    {
        private string? _imageDataUrl { get; set; }

        private string selectedColor { get; set; } = String.Empty;

        private List<string> colors { get; set; } = new List<string>();

        private int _userId { get; set; }

        private string plotUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var token = await LocalStorage.GetItemAsync<string>("token");

            if (token != null)
            {
                await authStateProvider.GetAuthenticationStateAsync();

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                _userId = int.Parse(jwt.Claims.First(c => c.Type == "Id").Value);
            }
        }
        private void ShowColorExtension(string color)
        {
            selectedColor = color;
        }

        private async Task SaveColorAsync()
        {
            if (_userId > 0)
            {
                try
                {
                    var userColor = new UserColorDTO
                    {
                        UserId = _userId,
                        ColorHexValue = selectedColor
                    };

                    await UserColorService.AddUserColor(userColor);

                    var userLevel = await UserLevelService.GetUserLevel(_userId);
                    UserLevelState.SetUserLevel(userLevel.LevelId, userLevel.CurrentXP);

                    NavigationManager.NavigateTo("/counter");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString(), "Error while saving a new color");
                }
            }
            else
            {
                // Handle the case where the user ID is not found
            }


        }
        private async Task PreviewImage(InputFileChangeEventArgs e)
        {
            try
            {
                selectedColor = string.Empty;

                var file = e.GetMultipleFiles().FirstOrDefault();
                if (file != null)
                {
                    colors = await ImageService.GetColors(file);
                    plotUrl = await ImageService.GetPaletteColors(file);


                    // Read the image data into a buffer asynchronously
                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10).CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Load and resize the image using SixLabors.ImageSharp
                    using var image = Image.Load(memoryStream);

                    // Resize the image to a smaller size for preview (e.g., 250x250 pixels)
                    image.Mutate(x => x.Resize(250, 250));

                    // Encode the image to base64 string for display in the browser
                    await using var outputStream = new MemoryStream();
                    await image.SaveAsync(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                    var buffer = outputStream.ToArray();
                    _imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";

                    // Notify Blazor to update the UI
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log it, show error message, etc.)
                Console.WriteLine(ex.Message);
            }
        }


    }
}
