using System.Windows;
using System.Windows.Controls;
using LabSampleManager.Desktop.Controls;
using LabSampleManager.Desktop.Properties;
using LabSampleManager.Desktop.Views;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class RegisterSampleViewTests
{
    [StaFact]
    public void layout_should_haveHeaderBodyAndFooterRows_when_view_is_created()
    {
        var view = new RegisterSampleView();
        var root = Assert.IsType<Grid>(view.Content);

        Assert.Equal(3, root.RowDefinitions.Count);
        Assert.Equal(GridUnitType.Auto, root.RowDefinitions[0].Height.GridUnitType);
        Assert.Equal(GridUnitType.Star, root.RowDefinitions[1].Height.GridUnitType);
        Assert.Equal(GridUnitType.Auto, root.RowDefinitions[2].Height.GridUnitType);
        Assert.IsType<HeaderControl>(root.FindName("RegisterSampleHeader"));
        Assert.IsType<Grid>(root.FindName("RegisterSampleBody"));
        Assert.IsType<Grid>(root.FindName("RegisterSampleFooter"));
    }

    [StaFact]
    public void body_should_useFlexibleLeftAndFixedRightColumns_when_view_is_created()
    {
        var view = new RegisterSampleView();
        var body = Assert.IsType<Grid>(view.FindName("RegisterSampleBody"));
        var left = Assert.IsType<Grid>(view.FindName("RegisterSampleLeft"));

        Assert.Equal(2, body.ColumnDefinitions.Count);
        Assert.Equal(GridUnitType.Star, body.ColumnDefinitions[0].Width.GridUnitType);
        Assert.Equal(340, body.ColumnDefinitions[1].Width.Value);
        Assert.Equal(2, left.RowDefinitions.Count);
        Assert.Equal(GridUnitType.Star, left.RowDefinitions[0].Height.GridUnitType);
        Assert.Equal(GridUnitType.Auto, left.RowDefinitions[1].Height.GridUnitType);
    }

    [StaFact]
    public void containers_should_displayRequestedMetadata_when_view_is_created()
    {
        var view = new RegisterSampleView();
        var sampleInformation = Assert.IsType<ContainerControl>(view.FindName("SampleInformationContainer"));
        var requestedTests = Assert.IsType<ContainerControl>(view.FindName("RequestedTestsContainer"));
        var registrationRules = Assert.IsType<ContainerControl>(view.FindName("RegistrationRulesContainer"));

        Assert.Equal(Resources.SampleInformationTitle, sampleInformation.Title);
        Assert.Equal(Resources.SampleInformationSubtitle, sampleInformation.SubTitle);
        Assert.Null(sampleInformation.IconSource);

        Assert.Equal(Resources.RequestedTestsTitle, requestedTests.Title);
        Assert.Equal(Resources.RequestedTestsSubtitle, requestedTests.SubTitle);
        Assert.Contains("TaskList/ic_fluent_task_list_24_regular_ltr.png", requestedTests.IconSource!.ToString());

        Assert.Equal(Resources.RegistrationRulesTitle, registrationRules.Title);
        Assert.Equal(Resources.RegistrationRulesSubtitle, registrationRules.SubTitle);
        Assert.Contains("Info/ic_fluent_info_24_regular.png", registrationRules.IconSource!.ToString());
    }

    [StaFact]
    public void footer_should_showRightAlignedInertActions_when_view_is_created()
    {
        var view = new RegisterSampleView();
        var footer = Assert.IsType<Grid>(view.FindName("RegisterSampleFooter"));
        var actions = Assert.IsType<Grid>(view.FindName("RegisterSampleActions"));
        var buttonGroup = Assert.IsType<Grid>(view.FindName("RegisterSampleButtonGroup"));
        var cancel = Assert.IsType<ButtonControl>(view.FindName("CancelButton"));
        var register = Assert.IsType<ButtonControl>(view.FindName("RegisterSampleButton"));

        Assert.Same(actions, footer.Children[0]);
        Assert.Equal(HorizontalAlignment.Stretch, actions.HorizontalAlignment);
        Assert.Equal(2, actions.ColumnDefinitions.Count);
        Assert.Equal(GridUnitType.Star, actions.ColumnDefinitions[0].Width.GridUnitType);
        Assert.Equal(340, actions.ColumnDefinitions[1].Width.Value);
        Assert.Same(buttonGroup, actions.Children[0]);
        Assert.Equal(1, Grid.GetColumn(buttonGroup));
        Assert.Equal(new Thickness(20, 0, 0, 0), buttonGroup.Margin);
        Assert.Equal(3, buttonGroup.ColumnDefinitions.Count);
        Assert.Equal(GridUnitType.Star, buttonGroup.ColumnDefinitions[0].Width.GridUnitType);
        Assert.Equal(12, buttonGroup.ColumnDefinitions[1].Width.Value);
        Assert.Equal(GridUnitType.Star, buttonGroup.ColumnDefinitions[2].Width.GridUnitType);
        Assert.Equal(0, Grid.GetColumn(cancel));
        Assert.Equal(2, Grid.GetColumn(register));
        Assert.Equal(HorizontalAlignment.Stretch, cancel.HorizontalAlignment);
        Assert.Equal(HorizontalAlignment.Stretch, register.HorizontalAlignment);
        Assert.Equal(Resources.CancelAction, cancel.Title);
        Assert.Equal(Resources.RegisterSampleAction, register.Title);
        Assert.Equal(ButtonType.Secondary, cancel.ButtonType);
        Assert.Equal(ButtonType.Main, register.ButtonType);
        Assert.Null(cancel.IconSource);
        Assert.Contains("AddCircle/ic_fluent_add_circle_24_regular.png", register.IconSource!.ToString());
        Assert.Null(cancel.Command);
        Assert.Null(register.Command);
    }
}
