using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LabSampleManager.Desktop.Controls;

public enum MenuDestination
{
    Dashboard,
    Samples,
    RegisterSample,
    Processing,
    Validation
}

public sealed class NavigationRequestedEventArgs(MenuDestination destination) : RoutedEventArgs
{
    public MenuDestination Destination { get; } = destination;
}

public partial class MenuControl : UserControl
{
    private readonly RadioButton[] _menuItems;

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.RegisterAttached(
            "IconSource",
            typeof(ImageSource),
            typeof(MenuControl),
            new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty SelectedIconSourceProperty =
        DependencyProperty.RegisterAttached(
            "SelectedIconSource",
            typeof(ImageSource),
            typeof(MenuControl),
            new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty SelectedDestinationProperty =
        DependencyProperty.Register(
            nameof(SelectedDestination),
            typeof(MenuDestination),
            typeof(MenuControl),
            new FrameworkPropertyMetadata(MenuDestination.Dashboard, OnSelectedDestinationChanged));

    public MenuControl()
    {
        InitializeComponent();
        _menuItems = [Dashboard, Samples, RegisterSample, Processing, Validation];
    }

    public MenuDestination SelectedDestination
    {
        get => (MenuDestination)GetValue(SelectedDestinationProperty);
        set => SetValue(SelectedDestinationProperty, value);
    }

    public event EventHandler<NavigationRequestedEventArgs>? NavigationRequested;

    public static void SetIconSource(DependencyObject element, ImageSource? value) =>
        element.SetValue(IconSourceProperty, value);

    public static ImageSource? GetIconSource(DependencyObject element) =>
        (ImageSource?)element.GetValue(IconSourceProperty);

    public static void SetSelectedIconSource(DependencyObject element, ImageSource? value) =>
        element.SetValue(SelectedIconSourceProperty, value);

    public static ImageSource? GetSelectedIconSource(DependencyObject element) =>
        (ImageSource?)element.GetValue(SelectedIconSourceProperty);

    private static void OnSelectedDestinationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        if (dependencyObject is MenuControl control)
        {
            control.UpdateSelectedMenuItem((MenuDestination)args.NewValue);
        }
    }

    private void OnMenuItemClick(object sender, RoutedEventArgs args)
    {
        if (sender is not RadioButton menuItem || !TryGetDestination(menuItem.Name, out var destination))
        {
            return;
        }

        SelectedDestination = destination;
        NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(destination));
    }

    private void UpdateSelectedMenuItem(MenuDestination destination)
    {
        foreach (var menuItem in _menuItems)
        {
            menuItem.IsChecked = TryGetDestination(menuItem.Name, out var itemDestination) && itemDestination == destination;
        }
    }

    private static bool TryGetDestination(string name, out MenuDestination destination)
    {
        return Enum.TryParse(name, ignoreCase: false, out destination);
    }
}
