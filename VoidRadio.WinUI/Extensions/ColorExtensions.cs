using Windows.UI;
using Windows.UI.ViewManagement;

namespace VoidRadio.WinUI.Extensions;

public static class ColorExtensions
{
    public static Color GetColor(this string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return new UISettings().GetColorValue(UIColorType.Accent);
        }

        try
        {
            var a = Convert.ToByte(color.Substring(1, 2), 16);
            var r = Convert.ToByte(color.Substring(3, 2), 16);
            var g = Convert.ToByte(color.Substring(5, 2), 16);
            var b = Convert.ToByte(color.Substring(7, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }
        catch
        {
            return new UISettings().GetColorValue(UIColorType.Accent);
        }
    }

    public static string GetString(this Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static Color GetTint(this Color color)
    {
        var a = color.A;
        var r = (byte)Math.Clamp(color.R + ((255 - color.R) * 0.10), 0, 255);
        var g = (byte)Math.Clamp(color.G + ((255 - color.G) * 0.10), 0, 255);
        var b = (byte)Math.Clamp(color.B + ((255 - color.B) * 0.10), 0, 255);

        return Color.FromArgb(a, r, g, b);
    }

    public static Color GetShade(this Color color)
    {
        var a = color.A;
        var r = (byte)Math.Clamp(color.R * (1 - 0.10), 0, 255);
        var g = (byte)Math.Clamp(color.G * (1 - 0.10), 0, 255);
        var b = (byte)Math.Clamp(color.B * (1 - 0.10), 0, 255);

        return Color.FromArgb(a, r, g, b);
    }
}