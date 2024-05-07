using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BApp.Pages
{
    public partial class ColorDetector: ComponentBase
    {
        private string? _imageDataUrl { get; set; }

        private async Task PreviewImage(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.GetMultipleFiles().FirstOrDefault();
                if (file != null)
                {
                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream().CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using var image = Image.Load(memoryStream);

                    // Resize the image to 50x50 pixels
                    image.Mutate(x => x.Resize(250, 250));

                    using var outputStream = new MemoryStream();
                    await image.SaveAsync(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                    var buffer = outputStream.ToArray();

                    _imageDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error processing image: {ex.Message}");
            }
        }
    }
}
