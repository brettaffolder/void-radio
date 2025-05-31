using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

using VoidRadio.WinUI.Contracts;
using VoidRadio.WinUI.Extensions;
using VoidRadio.WinUI.Services;

using Windows.Win32;
using Windows.Win32.Foundation;

using WinRT.Interop;

namespace VoidRadio.WinUI;

public partial class App : Application
{
    public static DispatcherQueue DispatcherQueue { get; } = DispatcherQueue.GetForCurrentThread();

    public IHost AppHost { get; }

    public App()
    {
        InitializeComponent();

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IThemeService, ThemeService>();

                services.AddSingleton<ShellViewModel>();

                services.AddSingleton<ShellWindow>();
            })
            .Build();
    }

    public static T GetService<T>() where T : class => 
        (T)((App)Current).AppHost.Services.GetRequiredService(typeof(T));

    protected override async void OnLaunched(LaunchActivatedEventArgs e)
    {
        var args = AppInstance.GetCurrent().GetActivatedEventArgs();
        var instance = AppInstance.FindOrRegisterForKey("hpi");

        if (!instance.IsCurrent)
        {
            await instance.RedirectActivationToAsync(args);
            Process.GetCurrentProcess().Kill();

            return;
        }

        var window = GetService<ShellWindow>();
        var appWindow = window.AppWindow;

        appWindow.Resize(new Windows.Graphics.SizeInt32
        {
            Height = 300,
            Width = 600
        });

        var position = appWindow.Position;
        var display = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

        position.X = (display.WorkArea.Width - appWindow.Size.Width);
        position.Y = (display.WorkArea.Height - appWindow.Size.Height);

        appWindow.Title = "Void Radio";
        appWindow.SetIcon("Assets/Logos/void.ico");
        appWindow.Move(position);

        GetService<IThemeService>().Initialize();

        window.Activate();

        GetService<IThemeService>().Reload();

        await Task.Delay(50);

        switch (args.Kind)
        {
            case ExtendedActivationKind.Protocol:
                HandleProtocolActivation(args);
                break;

            case ExtendedActivationKind.File:
                HandleFileActivation(args);
                break;

            default:
                break;
        }

        instance.Activated += OnActivated;
    }

    private static async void OnActivated(object? s, AppActivationArguments e)
    {
        await DispatcherQueue.EnqueueAsync(() =>
        {
            switch (e.Kind)
            {
                case ExtendedActivationKind.Protocol:
                    HandleProtocolActivation(e);
                    break;

                case ExtendedActivationKind.File:
                    HandleFileActivation(e);
                    break;

                default:
                    break;
            }

            var window = GetService<ShellWindow>();
            var appWindow = window.AppWindow;
            var hWnd = WindowNative.GetWindowHandle(window);
            _ = PInvoke.SetForegroundWindow((HWND)hWnd);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                var isIconic = PInvoke.IsIconic((HWND)hWnd);
                if (isIconic)
                {
                    presenter.Restore();
                }
            }
        });
    }

    private static void HandleProtocolActivation(AppActivationArguments e)
    {

    }

    private static void HandleFileActivation(AppActivationArguments e)
    {

    }
}