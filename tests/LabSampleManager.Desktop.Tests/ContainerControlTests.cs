using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LabSampleManager.Desktop.Controls;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class ContainerControlTests
{
    [StaFact]
    public void dependencyProperties_should_haveExpectedDefaults_when_control_is_created()
    {
        var control = new ContainerControl();

        Assert.Equal(string.Empty, control.Title);
        Assert.Equal(string.Empty, control.SubTitle);
        Assert.Null(control.IconSource);
        Assert.True(double.IsNaN(control.Width));
        Assert.True(double.IsNaN(control.Height));
        Assert.Equal(typeof(ContainerControl), ContainerControl.TitleProperty.OwnerType);
        Assert.Equal(typeof(string), ContainerControl.TitleProperty.PropertyType);
        Assert.Equal(string.Empty, ContainerControl.TitleProperty.DefaultMetadata.DefaultValue);
        Assert.Equal(typeof(ContainerControl), ContainerControl.SubTitleProperty.OwnerType);
        Assert.Equal(typeof(string), ContainerControl.SubTitleProperty.PropertyType);
        Assert.Equal(string.Empty, ContainerControl.SubTitleProperty.DefaultMetadata.DefaultValue);
        Assert.Equal(typeof(ContainerControl), ContainerControl.IconSourceProperty.OwnerType);
        Assert.Equal(typeof(ImageSource), ContainerControl.IconSourceProperty.PropertyType);
        Assert.Null(ContainerControl.IconSourceProperty.DefaultMetadata.DefaultValue);
    }

    [StaFact]
    public void dependencyProperties_should_preserveAssignedValues_when_values_are_set()
    {
        var icon = new BitmapImage();
        var control = new ContainerControl
        {
            Title = "Sample Information",
            SubTitle = "Enter the basic details for the new sample.",
            IconSource = icon
        };

        Assert.Equal("Sample Information", control.Title);
        Assert.Equal("Enter the basic details for the new sample.", control.SubTitle);
        Assert.Same(icon, control.IconSource);
    }

    [StaFact]
    public void contentPresenter_should_hostArbitraryContent_when_content_is_assigned()
    {
        var content = new Grid();
        var control = new ContainerControl { Content = content };

        ApplyLayout(control, 640, 480);

        var presenter = Assert.IsType<ContentPresenter>(
            control.Template!.FindName("PART_ContentPresenter", control));

        Assert.Same(content, presenter.Content);
    }

    [StaFact]
    public void template_should_matchReferenceSurface_when_control_is_created()
    {
        var control = new ContainerControl
        {
            Title = "Sample Information",
            SubTitle = "Enter the basic details for the new sample.",
            IconSource = new BitmapImage()
        };

        ApplyLayout(control, 640, 480);

        var border = Assert.IsType<Border>(control.Template!.FindName("PART_ContainerBorder", control));
        var icon = Assert.IsType<Image>(control.Template.FindName("PART_Icon", control));
        var title = Assert.IsType<TextBlock>(control.Template.FindName("PART_Title", control));
        var subtitle = Assert.IsType<TextBlock>(control.Template.FindName("PART_SubTitle", control));

        Assert.Equal(Colors.White, Assert.IsType<SolidColorBrush>(border.Background).Color);
        Assert.Equal(Color.FromRgb(0xDC, 0xE8, 0xF5), Assert.IsType<SolidColorBrush>(border.BorderBrush).Color);
        Assert.Equal(new Thickness(1), border.BorderThickness);
        Assert.Equal(new CornerRadius(8), border.CornerRadius);
        Assert.Equal(30, icon.Width);
        Assert.Equal(30, icon.Height);
        Assert.Equal("Sample Information", title.Text);
        Assert.Equal("Enter the basic details for the new sample.", subtitle.Text);
    }

    [StaFact]
    public void control_should_stretchToParent_when_parentProvidesSpace()
    {
        var control = new ContainerControl();
        var parent = new Grid { Width = 800, Height = 500 };
        parent.Children.Add(control);

        ApplyLayout(parent, 800, 500);

        Assert.Equal(800, control.ActualWidth);
        Assert.Equal(500, control.ActualHeight);
        Assert.Equal(HorizontalAlignment.Stretch, control.HorizontalAlignment);
        Assert.Equal(VerticalAlignment.Stretch, control.VerticalAlignment);
        Assert.Equal(HorizontalAlignment.Stretch, control.HorizontalContentAlignment);
        Assert.Equal(VerticalAlignment.Stretch, control.VerticalContentAlignment);
    }

    [StaFact]
    public void icon_should_be_collapsed_when_source_is_not_provided()
    {
        var control = new ContainerControl();

        ApplyLayout(control, 640, 480);

        var icon = Assert.IsType<Image>(control.Template!.FindName("PART_Icon", control));

        Assert.Equal(Visibility.Collapsed, icon.Visibility);
    }

    [StaFact]
    public void defaultStyle_should_be_available_when_application_is_created()
    {
        var control = new ContainerControl();

        if (Application.Current is null)
        {
            var application = new LabSampleManager.Desktop.App();
            application.InitializeComponent();
        }

        var style = Assert.IsType<Style>(Application.Current!.Resources[typeof(ContainerControl)]);

        control.Style = style;
        Assert.True(control.ApplyTemplate());
        Assert.NotNull(control.Template);
    }

    private static void ApplyLayout(FrameworkElement element, double width, double height)
    {
        var application = EnsureApplication();

        if (element is ContainerControl control)
        {
            ApplyContainerStyle(control, application);
        }

        element.Measure(new Size(width, height));
        element.Arrange(new Rect(0, 0, width, height));
        element.UpdateLayout();
    }

    private static Application EnsureApplication()
    {
        if (Application.Current is null)
        {
            _ = new Application();
        }

        return Application.Current!;
    }

    private static void ApplyContainerStyle(ContainerControl control, Application application)
    {
        if (application.Resources[typeof(ContainerControl)] is null)
        {
            application.Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    Source = new Uri(
                        "/LabSampleManager.Desktop;component/Themes/Generic.xaml",
                        UriKind.Relative)
                });
        }

        if (control.Style is null)
        {
            control.Style = (Style)application.Resources[typeof(ContainerControl)];
        }
    }
}
