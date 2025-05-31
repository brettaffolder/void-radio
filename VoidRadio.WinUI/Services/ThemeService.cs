using Microsoft.UI;
using Microsoft.UI.Xaml;

using VoidRadio.WinUI.Contracts;
using VoidRadio.WinUI.Extensions;
using VoidRadio.WinUI.Helpers;

using Windows.UI;
using Windows.UI.ViewManagement;

namespace VoidRadio.WinUI.Services;

public class ThemeService : IThemeService
{
    private Color _accent = new UISettings().GetColorValue(UIColorType.Accent);
    public Color Accent => _accent;

    private ElementTheme _theme = ElementTheme.Default;
    public ElementTheme Theme => _theme;

    public void Initialize()
    {
        var accentSetting = SettingHelper.Read<string>("accent");
        var themeSetting = SettingHelper.Read<string>("theme");

        _accent = accentSetting.GetColor();
        _theme = themeSetting.GetElementTheme();

        UpdateInternal();
    }

    public void Reload()
    {
        UpdateInternal();
    }

    public void Update(Color accent, ElementTheme theme)
    {
        _accent = accent;
        _theme = theme;

        UpdateInternal();
    }

    public void Update(Color accent, string? themeString)
    {
        _accent = accent;
        _theme = themeString.GetElementTheme();

        UpdateInternal();
    }

    public void Update(string? accentString, ElementTheme theme)
    {
        _accent = accentString.GetColor();
        _theme = theme;

        UpdateInternal();
    }

    public void Update(string? accentString, string? themeString)
    {
        _accent = accentString.GetColor();
        _theme = themeString.GetElementTheme();

        UpdateInternal();
    }

    private void UpdateInternal()
    {
        var accentLight1 = _accent.GetTint();
        var accentLight2 = accentLight1.GetTint();
        var accentLight3 = accentLight2.GetTint();
        var accentDark1 = _accent.GetShade();
        var accentDark2 = accentDark1.GetShade();
        var accentDark3 = accentDark2.GetShade();

        Application.Current.Resources["SystemAccentColor"] = _accent;
        Application.Current.Resources["SystemAccentColorLight1"] = accentLight1;
        Application.Current.Resources["SystemAccentColorLight2"] = accentLight2;
        Application.Current.Resources["SystemAccentColorLight3"] = accentLight3;
        Application.Current.Resources["SystemAccentColorDark1"] = accentDark1;
        Application.Current.Resources["SystemAccentColorDark2"] = accentDark2;
        Application.Current.Resources["SystemAccentColorDark3"] = accentDark3;

        var window = App.GetService<ShellWindow>();

        if (window.Content is FrameworkElement element)
        {
            element.RequestedTheme = _theme.GetInverse();
            element.RequestedTheme = _theme;
        }

        var titleBar = window.AppWindow.TitleBar;
        var isLight = _theme.IsLight();
        var lightGray = Color.FromArgb(50, 50, 50, 50);
        var darkGray = Color.FromArgb(50, 200, 200, 200);

        titleBar.BackgroundColor = Colors.Transparent;
        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        titleBar.InactiveBackgroundColor = Colors.Transparent;

        titleBar.ButtonHoverBackgroundColor = isLight ? lightGray : darkGray;
        titleBar.ButtonPressedBackgroundColor = isLight ? lightGray : darkGray;

        titleBar.ForegroundColor = isLight ? Colors.Black : Colors.White;
        titleBar.ButtonForegroundColor = isLight ? Colors.Black : Colors.White;

        titleBar.ButtonHoverForegroundColor = Colors.Gray;
        titleBar.ButtonInactiveForegroundColor = Colors.Gray;
        titleBar.ButtonPressedForegroundColor = Colors.Gray;
        titleBar.InactiveForegroundColor = Colors.Gray;

        var accentString = _accent.GetString();
        var themeString = _theme.GetString();

        SettingHelper.Write("accent", accentString);
        SettingHelper.Write("theme", themeString);
    }
}