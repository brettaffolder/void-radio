using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using VoidRadio.WinUI.Contracts;

namespace VoidRadio.WinUI;

public partial class ShellViewModel(
    IThemeService theme) : ObservableObject
{
    private readonly IThemeService _theme = theme;

    [ObservableProperty]
    public partial bool IsPinned { get; set; } = false;

    [ObservableProperty]
    public partial bool IsDark { get; set; } = true;

    [ObservableProperty]
    public partial string ThemeIcon { get; set; } = "\uE706";

    public void Setup()
    {
        IsPinned = false;
        IsDark = _theme.Theme == ElementTheme.Dark || _theme.Theme == ElementTheme.Default && Application.Current.RequestedTheme == ApplicationTheme.Dark;
        ThemeIcon = IsDark ? "\uE708" : "\uE706";
    }

    partial void OnIsPinnedChanged(bool value)
    {
        var window = App.GetService<ShellWindow>();
        var appWindow = window.AppWindow;

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsAlwaysOnTop = IsPinned;
        }
    }

    partial void OnIsDarkChanged(bool value)
    {
        var theme = IsDark ? "Dark" : "Light";
        _theme.Update(_theme.Accent, theme);

        ThemeIcon = IsDark ? "\uE708" : "\uE706";
    }
}