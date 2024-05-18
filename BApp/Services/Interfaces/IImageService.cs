using Microsoft.AspNetCore.Components.Forms;

namespace BApp.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> GetPaletteColors(IBrowserFile file);

        Task<List<string>> GetColors(IBrowserFile file);
    }
}
