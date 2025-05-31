using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using Windows.Graphics;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.HiDpi;

namespace VoidRadio.WinUI;

public sealed partial class ShellWindow : Window
{
    public ShellViewModel ViewModel { get; }

    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;

        AppWindow.Changed += OnAppWindowChanged;
        AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;
        AppWindow.TitleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;

        XTitleBar.Loaded += (_, _) => SetDragRegion();
        XTitleBar.SizeChanged += (_, _) => SetDragRegion();
    }

    private void OnAppWindowChanged(AppWindow s, AppWindowChangedEventArgs e)
    {
        if (!e.DidPresenterChange)
        {
            return;
        }

        switch (s.Presenter.Kind)
        {
            case AppWindowPresenterKind.CompactOverlay:
                XTitleBar.Visibility = Visibility.Collapsed;
                s.TitleBar.ResetToDefault();
                break;

            case AppWindowPresenterKind.FullScreen:
                XTitleBar.Visibility = Visibility.Collapsed;
                s.TitleBar.ExtendsContentIntoTitleBar = true;
                break;

            case AppWindowPresenterKind.Overlapped:
                XTitleBar.Visibility = Visibility.Visible;
                s.TitleBar.ExtendsContentIntoTitleBar = true;
                SetDragRegion();
                break;

            default:
                s.TitleBar.ResetToDefault();
                break;
        }
    }

    private void SetDragRegion()
    {
        var scale = GetScale();

        try
        {
            XLeftPaddingColumn.Width = new GridLength(AppWindow.TitleBar.LeftInset / scale);
            XRightPaddingColumn.Width = new GridLength(AppWindow.TitleBar.RightInset / scale);
        }
        catch { }

        var rects = new RectInt32[]
        {
            new RectInt32
            {
                X = (int)((XLeftPaddingColumn.ActualWidth + XIconColumn.ActualWidth) * scale),
                Y = 0,
                Height = (int)(XTitleBar.ActualHeight * scale),
                Width = (int)((XTitleColumn.ActualWidth + XDragColumn.ActualWidth) * scale)
            }
        };

        var source = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
        source.ClearRegionRects(NonClientRegionKind.Caption);
        source.SetRegionRects(NonClientRegionKind.Caption, rects);
    }

    private double GetScale()
    {
        var display = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
        var monitor = Win32Interop.GetMonitorFromDisplayId(display.DisplayId);

        _ = PInvoke.GetDpiForMonitor((HMONITOR)monitor, MONITOR_DPI_TYPE.MDT_DEFAULT, out var dpiX, out var _);

        var scale = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);

        return scale / 100;
    }
}