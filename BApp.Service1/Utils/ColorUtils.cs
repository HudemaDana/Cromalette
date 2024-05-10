using System.Drawing;

namespace BApp.Services.Utils
{
    public static class ColorUtils
    {
        /// <summary>
        /// This method generates a given amount of tints based on a given hex
        /// </summary>
        /// <param name="hexColor"> The color hex value,upon which we want to generate the tints</param>
        /// <param name="count">The number of tints we want to generate</param>
        /// <returns>A list of strings, under the form of hex values</returns>
        public static List<string> GenerateTints(this string hexColor, int count)
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

        /// <summary>
        /// This method generates a given amount of shades based on a given hex
        /// </summary>
        /// <param name="hexColor"> The color hex value,upon which we want to generate the shades</param>
        /// <param name="count">The number of shades we want to generate</param>
        /// <returns>A list of strings, under the form of hex values</returns>
        public static List<string> GenerateShades(this string hexColor, int count)
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

