using Microsoft.UI.Xaml;

using Windows.UI;

namespace VoidRadio.WinUI.Contracts;

public interface IThemeService
{
    Color Accent { get; }
    ElementTheme Theme { get; }
    void Initialize();
    void Reload();
    void Update(Color accent, ElementTheme theme);
    void Update(Color accent, string? themeString);
    void Update(string? accentString, ElementTheme theme);
    void Update(string? accentString, string? themeString);
}