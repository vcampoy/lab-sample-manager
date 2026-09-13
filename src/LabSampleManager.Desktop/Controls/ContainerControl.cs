using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LabSampleManager.Desktop.Controls;

public sealed class ContainerControl : ContentControl
{
    static ContainerControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(typeof(ContainerControl)));
        HorizontalContentAlignmentProperty.OverrideMetadata(
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
        VerticalContentAlignmentProperty.OverrideMetadata(
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty SubTitleProperty =
        DependencyProperty.Register(
            nameof(SubTitle),
            typeof(string),
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register(
            nameof(IconSource),
            typeof(ImageSource),
            typeof(ContainerControl),
            new FrameworkPropertyMetadata(null));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string SubTitle
    {
        get => (string)GetValue(SubTitleProperty);
        set => SetValue(SubTitleProperty, value);
    }

    public ImageSource? IconSource
    {
        get => (ImageSource?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
}
