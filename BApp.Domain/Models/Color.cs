public class Color
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }

    public Color(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static Color FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
        {
            throw new ArgumentException("Hex color cannot be null or empty.");
        }

        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length != 6)
        {
            throw new ArgumentException("Hex color must be 6 digits long.");
        }

        int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r, g, b);
    }

    public static Color FromArgb(int r, int g, int b)
    {
        return new Color(r, g, b);
    }

    public string ToHex()
    {
        return $"#{R:X2}{G:X2}{B:X2}";
    }

    public static Color White => new Color(255, 255, 255);
    public static Color Black => new Color(0, 0, 0);
}
