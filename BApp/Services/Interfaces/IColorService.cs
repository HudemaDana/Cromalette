namespace BApp.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<string>> GenerateTints(string hexColor, int count);

        Task<List<string>> GenerateShades(string hexColor, int count);
    }
}
