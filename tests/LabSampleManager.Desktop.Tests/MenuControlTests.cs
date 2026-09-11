using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using LabSampleManager.Desktop.Controls;
using LabSampleManager.Desktop.Views;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class MenuControlTests
{
    private static readonly MenuDestination[] Destinations =
    [
        MenuDestination.Dashboard,
        MenuDestination.Samples,
        MenuDestination.RegisterSample,
        MenuDestination.Processing,
        MenuDestination.Validation,
        MenuDestination.Settings
    ];

    [StaFact]
    public void selectedDestination_should_default_toDashboard_when_control_is_created()
    {
        var control = new MenuControl();

        Assert.Equal(MenuDestination.Dashboard, control.SelectedDestination);
    }

    [StaFact]
    public void menuItems_should_updateSelectedDestination_when_each_item_is_clicked()
    {
        var control = new MenuControl();

        foreach (var destination in Destinations)
        {
            var item = (RadioButton)control.FindName(destination.ToString());

            item.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            Assert.Equal(destination, control.SelectedDestination);
            Assert.True(item.IsChecked);
        }
    }

    [StaFact]
    public void navigationRequested_should_raiseExactlyOnce_when_menuItem_is_clicked()
    {
        var control = new MenuControl();
        var requestedDestinations = new List<MenuDestination>();
        control.NavigationRequested += (_, args) => requestedDestinations.Add(args.Destination);
        var item = (RadioButton)control.FindName(nameof(MenuDestination.Samples));

        item.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

        Assert.Equal([MenuDestination.Samples], requestedDestinations);
    }

    [StaFact]
    public void menuItems_should_keepOnlyOneSelected_when_destination_changes()
    {
        var control = new MenuControl();

        ((RadioButton)control.FindName(nameof(MenuDestination.Samples))).RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

        var selectedCount = Destinations
            .Select(destination => (RadioButton)control.FindName(destination.ToString()))
            .Count(item => item.IsChecked == true);

        Assert.Equal(1, selectedCount);
    }

    [StaFact]
    public void selectedDestination_should_syncCheckedItem_when_value_is_set_externally()
    {
        var control = new MenuControl
        {
            SelectedDestination = MenuDestination.Validation
        };

        Assert.True(((RadioButton)control.FindName(nameof(MenuDestination.Validation))).IsChecked);
        Assert.False(((RadioButton)control.FindName(nameof(MenuDestination.Dashboard))).IsChecked);
    }

    [StaFact]
    public void icons_should_loadFromFluentResources_andKeepAccessibleTitles_when_control_is_created()
    {
        var control = new MenuControl();

        var headerImage = (Image)control.FindName("HeaderPlaceholder");
        var logoImage = (Image)control.FindName("RocheLogoPlaceholder");

        Assert.NotNull(headerImage.Source);
        Assert.Contains("Beaker/ic_fluent_beaker_24_filled.png", headerImage.Source.ToString());
        Assert.Equal("icon-lab-sample-manager", headerImage.ToolTip);
        Assert.Equal("icon-lab-sample-manager", AutomationProperties.GetName(headerImage));
        Assert.NotNull(logoImage.Source);
        Assert.Contains("Assets/Images/roche-logo-blue.png", logoImage.Source.ToString());
        Assert.Equal("logo-roche", logoImage.ToolTip);
        Assert.Equal("logo-roche", AutomationProperties.GetName(logoImage));
    }

    [StaFact]
    public void contentHost_should_loadDashboardView_when_mainWindow_is_created()
    {
        var window = new MainWindow();

        var contentHost = (ContentControl)window.FindName("ContentHost");

        Assert.IsType<DashboardView>(contentHost.Content);
    }

    [StaFact]
    public void contentHost_should_keepDashboardView_when_nonDashboard_destination_is_requested()
    {
        var window = new MainWindow();
        var contentHost = (ContentControl)window.FindName("ContentHost");
        var menu = (MenuControl)((Grid)window.Content).Children[0];

        ((RadioButton)menu.FindName(nameof(MenuDestination.Samples))).RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

        Assert.IsType<DashboardView>(contentHost.Content);
    }

}
