using BApp.Services.Interfaces;
using System.Drawing;

namespace BApp.Services
{
    public class ColorService : IColorService
    {
        public async Task<List<string>> GenerateTints(string hexColor, int count)
        {
            Color baseColor = Color.FromHex(hexColor);
            List<string> tints = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                double tintFactor = i * (1.0 / count);
                Color tint = MixColors(Color.White, baseColor, tintFactor);
                tints.Add(tint.ToHex());
            }

            return tints;
        }

        public async Task<List<string>> GenerateShades(string hexColor, int count)
        {
            Color baseColor = Color.FromHex(hexColor);
            List<string> shades = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                double shadeFactor = i * (1.0 / count);
                Color shade = MixColors(Color.Black, baseColor, shadeFactor);
                shades.Add(shade.ToHex());
            }

            return shades;
        }

        public async Task<List<string>> GenerateTones(string hexColor, int count)
        {
            Color baseColor = Color.FromHex(hexColor);
            List<string> tones = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                double toneFactor = i * (1.0 / count);
                Color gray = Color.FromArgb(128, 128, 128);
                Color tone = MixColors(gray, baseColor, toneFactor);
                tones.Add(tone.ToHex());
            }

            return tones;
        }

        public async Task<List<string>> GeneratePalette(string hex, int rule)
        {
            Color baseColor = Color.FromHex(hex);
            List<string> palette = new List<string> { hex };

            switch (rule)
            {
                case 1: // Complementary Colors
                    palette.Add(GetComplementaryColor(baseColor));
                    break;
                case 2: // Analogous Colors
                    palette.AddRange(GetAnalogousColors(baseColor));
                    break;
                case 3: // Triadic Colors
                    palette.AddRange(GetTriadicColors(baseColor));
                    break;
                case 4: // Tetradic Colors
                    palette.AddRange(GetTetradicColors(baseColor));
                    break;
                default:
                    throw new ArgumentException("Invalid rule. Must be between 1 and 4.");
            }


            return palette;
        }

        private static string GetComplementaryColor(Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B).ToHex();
        }

        private static List<string> GetAnalogousColors(Color color)
        {
            return new List<string>
        {
            ShiftHue(color, 30).ToHex(),
            ShiftHue(color, -30).ToHex()
        };
        }

        private static List<string> GetTriadicColors(Color color)
        {
            return new List<string>
        {
            ShiftHue(color, 120).ToHex(),
            ShiftHue(color, -120).ToHex()
        };
        }

        private static List<string> GetTetradicColors(Color color)
        {
            return new List<string>
        {
            ShiftHue(color, 90).ToHex(),
            ShiftHue(color, 180).ToHex(),
            ShiftHue(color, -90).ToHex()
        };
        }

        private static Color ShiftHue(Color color, double degree)
        {
            var hsv = RgbToHsv(color);
            hsv.H = (hsv.H + degree) % 360;
            if (hsv.H < 0) hsv.H += 360;
            return HsvToRgb(hsv);
        }

        private static (double H, double S, double V) RgbToHsv(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            double h = 0;
            if (delta != 0)
            {
                if (max == r) h = (g - b) / delta % 6;
                else if (max == g) h = (b - r) / delta + 2;
                else if (max == b) h = (r - g) / delta + 4;
            }
            h *= 60;
            if (h < 0) h += 360;

            double s = max == 0 ? 0 : delta / max;
            double v = max;

            return (h, s, v);
        }

        private static Color HsvToRgb((double H, double S, double V) hsv)
        {
            double c = hsv.V * hsv.S;
            double x = c * (1 - Math.Abs((hsv.H / 60) % 2 - 1));
            double m = hsv.V - c;

            double r, g, b;
            if (hsv.H >= 0 && hsv.H < 60) (r, g, b) = (c, x, 0);
            else if (hsv.H >= 60 && hsv.H < 120) (r, g, b) = (x, c, 0);
            else if (hsv.H >= 120 && hsv.H < 180) (r, g, b) = (0, c, x);
            else if (hsv.H >= 180 && hsv.H < 240) (r, g, b) = (0, x, c);
            else if (hsv.H >= 240 && hsv.H < 300) (r, g, b) = (x, 0, c);
            else (r, g, b) = (c, 0, x);

            return new Color((int)((r + m) * 255), (int)((g + m) * 255), (int)((b + m) * 255));
        }

        private static Color MixColors(Color color1, Color color2, double ratio)
        {
            int r = (int)(color1.R * ratio + color2.R * (1 - ratio));
            int g = (int)(color1.G * ratio + color2.G * (1 - ratio));
            int b = (int)(color1.B * ratio + color2.B * (1 - ratio));
            return Color.FromArgb(r, g, b);
        }
    }
}
