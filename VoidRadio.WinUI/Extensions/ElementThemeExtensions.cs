using Microsoft.UI.Xaml;

namespace VoidRadio.WinUI.Extensions;

public static class ElementThemeExtensions
{
    public static ElementTheme GetElementTheme(this string? elementTheme)
    {
        return elementTheme switch
        {
            "Light" => ElementTheme.Light,
            "Dark" => ElementTheme.Dark,
            _ => ElementTheme.Default
        };
    }

    public static string GetString(this ElementTheme elementTheme)
    {
        return elementTheme switch
        {
            ElementTheme.Light => "Light",
            ElementTheme.Dark => "Dark",
            _ => "System"
        };
    }

    public static ElementTheme GetInverse(this ElementTheme elementTheme)
    {
        return elementTheme switch
        {
            ElementTheme.Light => ElementTheme.Dark,
            ElementTheme.Dark => ElementTheme.Light,
            _ => Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Dark : ElementTheme.Light
        };
    }

    public static bool IsLight(this ElementTheme elementTheme)
    {
        return elementTheme switch
        {
            ElementTheme.Light => true,
            ElementTheme.Dark => false,
            _ => Application.Current.RequestedTheme == ApplicationTheme.Light
        };
    }
}