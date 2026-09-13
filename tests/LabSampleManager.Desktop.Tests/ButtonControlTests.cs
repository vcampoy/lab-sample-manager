using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LabSampleManager.Desktop.Controls;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class ButtonControlTests
{
    private static readonly (ButtonType Type, Color Background, Color Foreground)[] VariantBrushes =
    [
        (ButtonType.Main, Color.FromRgb(0x17, 0x68, 0xD7), Colors.White),
        (ButtonType.Secondary, Colors.White, Color.FromRgb(0x0B, 0x63, 0xB6)),
        (ButtonType.Cancel, Color.FromRgb(0xFE, 0xF4, 0xF6), Color.FromRgb(0xD7, 0x00, 0x15)),
        (ButtonType.View, Color.FromRgb(0xED, 0xF6, 0xFE), Color.FromRgb(0x0B, 0x63, 0xB6))
    ];

    [StaFact]
    public void dependencyProperties_should_haveExpectedDefaults_when_control_is_created()
    {
        var control = new ButtonControl();

        Assert.Equal(ButtonType.Main, control.ButtonType);
        Assert.Equal(ButtonIconPosition.Left, control.IconPosition);
        Assert.Null(control.IconSource);
        Assert.Equal(string.Empty, control.Title);
        Assert.Equal(new CornerRadius(6), control.CornerRadius);
        Assert.Equal(20, control.IconSize);
        Assert.Equal(8, control.IconSpacing);
        Assert.True(control.TintIcon);
        Assert.Equal(typeof(ButtonControl), ButtonControl.ButtonTypeProperty.OwnerType);
        Assert.Equal(typeof(ButtonType), ButtonControl.ButtonTypeProperty.PropertyType);
        Assert.Equal(ButtonType.Main, ButtonControl.ButtonTypeProperty.DefaultMetadata.DefaultValue);
        Assert.Equal(typeof(ButtonIconPosition), ButtonControl.IconPositionProperty.PropertyType);
        Assert.Equal(ButtonIconPosition.Left, ButtonControl.IconPositionProperty.DefaultMetadata.DefaultValue);
        Assert.Equal(typeof(ImageSource), ButtonControl.IconSourceProperty.PropertyType);
        Assert.Null(ButtonControl.IconSourceProperty.DefaultMetadata.DefaultValue);
        Assert.Equal(typeof(string), ButtonControl.TitleProperty.PropertyType);
        Assert.Equal(string.Empty, ButtonControl.TitleProperty.DefaultMetadata.DefaultValue);
    }

    [StaFact]
    public void dependencyProperties_should_preserveAssignedValues_when_values_are_set()
    {
        var icon = new BitmapImage();
        var control = new ButtonControl
        {
            ButtonType = ButtonType.View,
            IconPosition = ButtonIconPosition.Right,
            IconSource = icon,
            Title = "View Details",
            CornerRadius = new CornerRadius(10),
            IconSize = 24,
            IconSpacing = 12,
            TintIcon = false
        };

        Assert.Equal(ButtonType.View, control.ButtonType);
        Assert.Equal(ButtonIconPosition.Right, control.IconPosition);
        Assert.Same(icon, control.IconSource);
        Assert.Equal("View Details", control.Title);
        Assert.Equal(new CornerRadius(10), control.CornerRadius);
        Assert.Equal(24, control.IconSize);
        Assert.Equal(12, control.IconSpacing);
        Assert.False(control.TintIcon);
    }

    [StaFact]
    public void template_should_applyExpectedBrushes_when_buttonType_changes()
    {
        foreach (var variant in VariantBrushes)
        {
            var control = new ButtonControl { ButtonType = variant.Type, Title = "Action" };

            ApplyLayout(control, 180, 40);

            var border = Assert.IsType<Border>(control.Template!.FindName("PART_ButtonBorder", control));

            Assert.Equal(variant.Background, Assert.IsType<SolidColorBrush>(border.Background).Color);
            Assert.Equal(variant.Foreground, Assert.IsType<SolidColorBrush>(control.Foreground).Color);
        }
    }

    [StaFact]
    public void template_should_positionAndTintIcon_when_iconIsProvided()
    {
        var icon = new BitmapImage();
        var control = new ButtonControl
        {
            IconPosition = ButtonIconPosition.Right,
            IconSource = icon,
            Title = "View Details"
        };

        ApplyLayout(control, 180, 40);

        var leftIcon = Assert.IsType<Rectangle>(control.Template!.FindName("PART_LeftIcon", control));
        var rightIcon = Assert.IsType<Rectangle>(control.Template.FindName("PART_RightIcon", control));
        var mask = Assert.IsType<ImageBrush>(rightIcon.OpacityMask);

        Assert.Equal(Visibility.Collapsed, leftIcon.Visibility);
        Assert.Equal(Visibility.Visible, rightIcon.Visibility);
        Assert.Same(icon, mask.ImageSource);
    }

    [StaFact]
    public void template_should_bindGeometryProperties_when_values_are_customized()
    {
        var control = new ButtonControl
        {
            CornerRadius = new CornerRadius(10),
            IconSize = 24,
            IconSpacing = 12,
            IconSource = new BitmapImage(),
            Title = "Action"
        };

        ApplyLayout(control, 220, 48);

        var border = Assert.IsType<Border>(control.Template!.FindName("PART_ButtonBorder", control));
        var leftIcon = Assert.IsType<Rectangle>(control.Template.FindName("PART_LeftIcon", control));

        Assert.Equal(new CornerRadius(10), border.CornerRadius);
        Assert.Equal(24, leftIcon.Width);
        Assert.Equal(24, leftIcon.Height);
        Assert.Equal(new Thickness(0, 0, 12, 0), leftIcon.Margin);
    }

    [StaFact]
    public void style_should_useOverriddenPaletteResource_when_resourceIsProvidedByApplication()
    {
        var application = EnsureApplication();
        const string resourceKey = "Button.Main.Background";
        var previous = application.Resources[resourceKey];
        var replacement = new SolidColorBrush(Color.FromRgb(0x01, 0x02, 0x03));

        try
        {
            application.Resources[resourceKey] = replacement;
            var control = new ButtonControl { Title = "Custom" };

            ApplyLayout(control, 180, 40);

            Assert.Same(replacement, control.Background);
        }
        finally
        {
            application.Resources[resourceKey] = previous;
        }
    }

    [StaFact]
    public void template_should_collapseIcons_when_iconIsNull()
    {
        var control = new ButtonControl { Title = "Cancel" };

        ApplyLayout(control, 180, 40);

        var leftIcon = Assert.IsType<Rectangle>(control.Template!.FindName("PART_LeftIcon", control));
        var rightIcon = Assert.IsType<Rectangle>(control.Template.FindName("PART_RightIcon", control));
        var title = Assert.IsType<TextBlock>(control.Template.FindName("PART_Title", control));

        Assert.Equal(Visibility.Collapsed, leftIcon.Visibility);
        Assert.Equal(Visibility.Collapsed, rightIcon.Visibility);
        Assert.Equal("Cancel", title.Text);
    }

    [StaFact]
    public void template_should_showOnlyOriginalIconAtConfiguredPosition_when_tintIsDisabled()
    {
        var icon = new BitmapImage();
        var control = new ButtonControl
        {
            IconPosition = ButtonIconPosition.Left,
            IconSource = icon,
            TintIcon = false,
            Title = "Action"
        };

        ApplyLayout(control, 180, 40);

        var leftTinted = Assert.IsType<Rectangle>(control.Template!.FindName("PART_LeftIcon", control));
        var rightTinted = Assert.IsType<Rectangle>(control.Template.FindName("PART_RightIcon", control));
        var leftOriginal = Assert.IsType<Image>(control.Template.FindName("PART_LeftOriginalIcon", control));
        var rightOriginal = Assert.IsType<Image>(control.Template.FindName("PART_RightOriginalIcon", control));

        Assert.Equal(Visibility.Collapsed, leftTinted.Visibility);
        Assert.Equal(Visibility.Collapsed, rightTinted.Visibility);
        Assert.Equal(Visibility.Visible, leftOriginal.Visibility);
        Assert.Equal(Visibility.Collapsed, rightOriginal.Visibility);
    }

    [StaFact]
    public void dependencyProperties_should_rejectInvalidGeometry_when_values_are_set()
    {
        var control = new ButtonControl();

        Assert.Throws<ArgumentException>(() => control.IconSize = double.NaN);
        Assert.Throws<ArgumentException>(() => control.IconSpacing = -1);
        Assert.Throws<ArgumentException>(() => control.CornerRadius = new CornerRadius(-1));
    }

    [StaFact]
    public void click_should_beRaisedThroughStandardButtonEvent_when_eventIsSubscribed()
    {
        var control = new ButtonControl();
        var clicks = 0;
        control.Click += (_, _) => clicks++;

        control.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

        Assert.Equal(1, clicks);
    }

    [StaFact]
    public void defaultStyle_should_be_available_when_application_is_created()
    {
        var control = new ButtonControl();

        ApplyLayout(control, 180, 40);

        Assert.NotNull(control.Template);
        Assert.NotNull(control.Template!.FindName("PART_ButtonBorder", control));
    }

    private static void ApplyLayout(ButtonControl control, double width, double height)
    {
        var application = EnsureApplication();

        if (application.Resources[typeof(ButtonControl)] is null)
        {
            application.Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    Source = new Uri(
                        "/LabSampleManager.Desktop;component/Themes/Generic.xaml",
                        UriKind.Relative)
                });
        }

        control.Style ??= (Style)application.Resources[typeof(ButtonControl)];
        control.Measure(new Size(width, height));
        control.Arrange(new Rect(0, 0, width, height));
        control.UpdateLayout();
    }

    private static Application EnsureApplication()
    {
        if (Application.Current is null)
        {
            _ = new Application();
        }

        return Application.Current!;
    }
}
