using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LabSampleManager.Desktop.Controls;

public enum ButtonType
{
    Main,
    Secondary,
    Cancel,
    View
}

public enum ButtonIconPosition
{
    Left,
    Right
}

public sealed class ButtonControl : Button
{
    static ButtonControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(typeof(ButtonControl)));
        HorizontalContentAlignmentProperty.OverrideMetadata(
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(HorizontalAlignment.Center));
        VerticalContentAlignmentProperty.OverrideMetadata(
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(VerticalAlignment.Center));
    }

    public static readonly DependencyProperty ButtonTypeProperty =
        DependencyProperty.Register(
            nameof(ButtonType),
            typeof(ButtonType),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(
                global::LabSampleManager.Desktop.Controls.ButtonType.Main));

    public static readonly DependencyProperty IconPositionProperty =
        DependencyProperty.Register(
            nameof(IconPosition),
            typeof(ButtonIconPosition),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(ButtonIconPosition.Left));

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register(
            nameof(IconSource),
            typeof(ImageSource),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(new global::System.Windows.CornerRadius(6)),
            IsValidCornerRadius);

    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(20d),
            IsValidNonNegativeFiniteDouble);

    public static readonly DependencyProperty IconSpacingProperty =
        DependencyProperty.Register(
            nameof(IconSpacing),
            typeof(double),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(8d),
            IsValidNonNegativeFiniteDouble);

    public static readonly DependencyProperty TintIconProperty =
        DependencyProperty.Register(
            nameof(TintIcon),
            typeof(bool),
            typeof(ButtonControl),
            new FrameworkPropertyMetadata(true));

    public ButtonType ButtonType
    {
        get => (ButtonType)GetValue(ButtonTypeProperty);
        set => SetValue(ButtonTypeProperty, value);
    }

    public ButtonIconPosition IconPosition
    {
        get => (ButtonIconPosition)GetValue(IconPositionProperty);
        set => SetValue(IconPositionProperty, value);
    }

    public ImageSource? IconSource
    {
        get => (ImageSource?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public double IconSpacing
    {
        get => (double)GetValue(IconSpacingProperty);
        set => SetValue(IconSpacingProperty, value);
    }

    public bool TintIcon
    {
        get => (bool)GetValue(TintIconProperty);
        set => SetValue(TintIconProperty, value);
    }

    private static bool IsValidNonNegativeFiniteDouble(object value) =>
        value is double number && !double.IsNaN(number) && !double.IsInfinity(number) && number >= 0;

    private static bool IsValidCornerRadius(object value)
    {
        if (value is not CornerRadius radius)
        {
            return false;
        }

        return IsValidNonNegativeFiniteDouble(radius.TopLeft)
            && IsValidNonNegativeFiniteDouble(radius.TopRight)
            && IsValidNonNegativeFiniteDouble(radius.BottomRight)
            && IsValidNonNegativeFiniteDouble(radius.BottomLeft);
    }
}

public sealed class IconSpacingConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var spacing = values.Length > 0 && values[0] is double number ? number : 0;
        return string.Equals(parameter as string, "Right", StringComparison.Ordinal)
            ? new Thickness(spacing, 0, 0, 0)
            : new Thickness(0, 0, spacing, 0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        [Binding.DoNothing];
}
