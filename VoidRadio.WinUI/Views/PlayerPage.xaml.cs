using Microsoft.UI.Xaml.Controls;

using VoidRadio.WinUI.ViewModels;

namespace VoidRadio.WinUI.Views;

public sealed partial class PlayerPage : Page
{
    public PlayerViewModel ViewModel { get; }

    public PlayerPage()
    {
        InitializeComponent();

        ViewModel = App.GetService<PlayerViewModel>();
        ViewModel.ScrollViewer = XScrollViewer;
        ViewModel.ItemsRepeater = XItemsRepeater;
    }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.Animated_GotItem(sender, e);
    }

    private void Button_GotFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.Animated_GotItem(sender, e);
    }
}