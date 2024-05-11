using BApp.Services.Interfaces;
using System.Drawing;

namespace BApp.Services
{
    public class ColorService : IColorService
    {
        public List<string> GenerateTints(string hexColor, int count)
        {
            Color baseColor = ColorTranslator.FromHtml(hexColor);
            List<string> tints = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                double tintFactor = 1.0 - i * 0.1;
                Color tint = MixColors(Color.White, baseColor, tintFactor);
                tints.Add(ColorToHex(tint));
            }

            return tints;
        }

        public List<string> GenerateShades(string hexColor, int count)
        {
            Color baseColor = ColorTranslator.FromHtml(hexColor);
            List<string> shades = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                double shadeFactor = 1.0 - i * 0.1;
                Color shade = MixColors(Color.Black, baseColor, shadeFactor);
                shades.Add(ColorToHex(shade));
            }

            return shades;
        }

        private static Color MixColors(Color color1, Color color2, double ratio)
        {
            int r = (int)(color1.R * ratio + color2.R * (1 - ratio));
            int g = (int)(color1.G * ratio + color2.G * (1 - ratio));
            int b = (int)(color1.B * ratio + color2.B * (1 - ratio));
            return Color.FromArgb(r, g, b);
        }

        private static string ColorToHex(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
