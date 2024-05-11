namespace BApp.Services.Interfaces
{
    public interface IColorService
    {
        List<string> GenerateTints(string hexColor, int count);
        List<string> GenerateShades(string hexColor, int count);
    }
}
