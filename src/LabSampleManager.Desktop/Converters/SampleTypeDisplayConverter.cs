using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LabSampleManager.Desktop.Models;
using LabSampleManager.Desktop.Properties;

namespace LabSampleManager.Desktop.Converters;

public sealed class SampleTypeDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            SampleType.Blood => Resources.SampleTypeBlood,
            SampleType.Urine => Resources.SampleTypeUrine,
            _ => DependencyProperty.UnsetValue
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => Binding.DoNothing;
}
