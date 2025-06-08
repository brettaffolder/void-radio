using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;

namespace VoidRadio.WinUI.ViewModels;

public partial class PlayerViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ScrollViewer? ScrollViewer { get; set; } = null;

    [ObservableProperty]
    public partial ItemsRepeater? ItemsRepeater { get; set; } = null;

    [ObservableProperty]
    public partial ObservableCollection<int> Songs { get; set; } = new ObservableCollection<int>(Enumerable.Range(1, 450));

    private double _animatedButtonHeight;
    private Thickness _animatedButtonMargin;

    public void Animated_GotItem(object sender, RoutedEventArgs e)
    {
        var item = sender as FrameworkElement;
        item?.StartBringIntoView(new BringIntoViewOptions()
        {
            VerticalAlignmentRatio = 0.5,
            AnimationDesired = true,
        });
    }

    public void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        var item = ElementCompositionPreview.GetElementVisual(args.Element);
        var svVisual = ElementCompositionPreview.GetElementVisual(ScrollViewer);
        var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(ScrollViewer);

        var scaleExpresion = scrollProperties.Compositor.CreateExpressionAnimation();
        scaleExpresion.SetReferenceParameter("svVisual", svVisual);
        scaleExpresion.SetReferenceParameter("scrollProperties", scrollProperties);
        scaleExpresion.SetReferenceParameter("item", item);

        scaleExpresion.Expression = "1 - abs((svVisual.Size.Y/2 - scrollProperties.Translation.Y) - (item.Offset.Y + item.Size.Y / 2))*(.25 / (svVisual.Size.Y / 2))";

        item.StartAnimation("Scale.X", scaleExpresion);
        item.StartAnimation("Scale.Y", scaleExpresion);

        var centerPointExpression = scrollProperties.Compositor.CreateExpressionAnimation();
        centerPointExpression.SetReferenceParameter("item", item);
        centerPointExpression.Expression = "Vector3(item.Size.X/2, item.Size.Y/2, 0)";

        item.StartAnimation("CenterPoint", centerPointExpression);
    }

    public double CenterPointOfViewportInExtent()
    {
        return ScrollViewer!.VerticalOffset + ScrollViewer!.ViewportHeight / 2;
    }

    public int GetSelectedIndexFromViewport()
    {
        int selectedItemIndex = (int)Math.Floor(CenterPointOfViewportInExtent() / ((double)_animatedButtonMargin.Top + _animatedButtonHeight));
        selectedItemIndex %= ItemsRepeater!.ItemsSourceView.Count;
        return selectedItemIndex;
    }

    public object GetSelectedItemFromViewport()
    {
        var selectedIndex = GetSelectedIndexFromViewport();
        var selectedElement = ItemsRepeater!.TryGetElement(selectedIndex) as Button;

        return selectedElement!;
    }
}