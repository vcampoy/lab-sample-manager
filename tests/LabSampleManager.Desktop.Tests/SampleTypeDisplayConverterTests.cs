using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LabSampleManager.Desktop.Converters;
using LabSampleManager.Desktop.Models;
using LabSampleManager.Desktop.Properties;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class SampleTypeDisplayConverterTests
{
    private static readonly SampleType[] SupportedSampleTypes = [SampleType.Blood, SampleType.Urine];

    [Fact]
    public void convert_should_returnLocalizedText_when_sampleTypeIsSupported()
    {
        var converter = new SampleTypeDisplayConverter();

        foreach (var sampleType in SupportedSampleTypes)
        {
            var expected = sampleType == SampleType.Blood ? Resources.SampleTypeBlood : Resources.SampleTypeUrine;

            Assert.Equal(expected, converter.Convert(sampleType, typeof(string), null, CultureInfo.InvariantCulture));
        }
    }

    [Fact]
    public void convert_should_returnUnsetValue_when_valueIsNotSampleType()
    {
        var converter = new SampleTypeDisplayConverter();

        Assert.Same(DependencyProperty.UnsetValue, converter.Convert("Blood", typeof(string), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void convertBack_should_not_updateSource_when_displayTextChanges()
    {
        var converter = new SampleTypeDisplayConverter();

        Assert.Same(Binding.DoNothing, converter.ConvertBack(Resources.SampleTypeBlood, typeof(SampleType), null, CultureInfo.InvariantCulture));
    }
}
