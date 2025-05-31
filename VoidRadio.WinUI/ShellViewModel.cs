using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Windowing;

namespace VoidRadio.WinUI;

public partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsPinned { get; set; } = false;

    partial void OnIsPinnedChanged(bool value)
    {
        var window = App.GetService<ShellWindow>();
        var appWindow = window.AppWindow;

        if (appWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsAlwaysOnTop = IsPinned;
        }
    }
}