using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using LabSampleManager.Desktop.Controls;
using LabSampleManager.Desktop.Views;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class LocalizationTests
{
    private static readonly (string Key, string Value)[] ResourceCases =
    [
        ("ApplicationName", "Lab Sample Manager"),
        ("UserDisplayName", "Dr. Bruce Banner"),
        ("UserRole", "Laboratory Scientist"),
        ("UserInitials", "BB"),
        ("MenuDashboard", "Dashboard"),
        ("MenuSamples", "Samples"),
        ("MenuRegisterSample", "Register Sample"),
        ("MenuProcessing", "Processing"),
        ("MenuValidation", "Validation"),
        ("FooterTagline", "Doing now what patients needs next"),
        ("ApplicationIconAccessibleName", "icon-lab-sample-manager"),
        ("DashboardIconAccessibleName", "icon-dashboard"),
        ("SamplesIconAccessibleName", "icon-samples"),
        ("RegisterSampleIconAccessibleName", "icon-register-sample"),
        ("ProcessingIconAccessibleName", "icon-processing"),
        ("ValidationIconAccessibleName", "icon-validation"),
        ("RocheLogoAccessibleName", "logo-roche"),
        ("DashboardTitle", "Dashboard"),
        ("DashboardSubtitle", "Laboratory Overview"),
        ("SamplesTitle", "Samples"),
        ("SamplesSubtitle", "Search and manage laboratory samples"),
        ("RegisterSampleTitle", "Register Sample"),
        ("RegisterSampleSubtitle", "Create a new laboratory sample request"),
        ("ProcessingMonitorTitle", "Processing Monitor"),
        ("ProcessingMonitorSubtitle", "Track analyzer workloads and sample execution"),
        ("ResultsValidationTitle", "Results Validation"),
        ("ResultsValidationSubtitle", "Validate and review completed results"),
        ("UnsupportedMenuDestination", "Unsupported menu destination.")
    ];

    private static readonly (Func<UserControl> CreateView, string TitleKey, string SubtitleKey)[] ViewCases =
    [
        (() => new DashboardView(), "DashboardTitle", "DashboardSubtitle"),
        (() => new SamplesView(), "SamplesTitle", "SamplesSubtitle"),
        (() => new RegisterSampleView(), "RegisterSampleTitle", "RegisterSampleSubtitle"),
        (() => new ProcessingMonitor(), "ProcessingMonitorTitle", "ProcessingMonitorSubtitle"),
        (() => new ResultsValidationView(), "ResultsValidationTitle", "ResultsValidationSubtitle")
    ];

    private static readonly string[] LocalizationXamlFiles =
    [
        "src/LabSampleManager.Desktop/Controls/HeaderControl.xaml",
        "src/LabSampleManager.Desktop/Controls/MenuControl.xaml",
        "src/LabSampleManager.Desktop/Views/MainWindow.xaml",
        "src/LabSampleManager.Desktop/Views/DashboardView.xaml",
        "src/LabSampleManager.Desktop/Views/SamplesView.xaml",
        "src/LabSampleManager.Desktop/Views/RegisterSampleView.xaml",
        "src/LabSampleManager.Desktop/Views/ProcessingMonitorView.xaml",
        "src/LabSampleManager.Desktop/Views/ResultsValidationView.xaml"
    ];

    private static readonly string[] TranslatableAttributeNames =
    [
        "Text",
        "Content",
        "Title",
        "SubTitle",
        "ToolTip"
    ];

    private static readonly ResourceManager ResourceManager =
        new("LabSampleManager.Desktop.Properties.Resources", typeof(App).Assembly);

    [StaFact]
    public void resources_should_returnExpectedEnglishValues_when_neutralCultureIsUsed()
    {
        foreach (var (key, expectedValue) in ResourceCases)
        {
            Assert.Equal(expectedValue, ResourceManager.GetString(key, CultureInfo.InvariantCulture));
        }
    }

    [Fact]
    public void xaml_should_referenceResources_when_propertyContainsTranslatableText()
    {
        var repositoryRoot = FindRepositoryRoot();

        foreach (var relativePath in LocalizationXamlFiles)
        {
            AssertXamlFileUsesResources(Path.Combine(repositoryRoot, relativePath));
        }
    }

    [StaFact]
    public void views_should_displayResourceValues_when_controlsAreCreated()
    {
        foreach (var (createView, titleKey, subtitleKey) in ViewCases)
        {
            var header = (HeaderControl)((Grid)createView().Content).Children[0];

            Assert.Equal(GetResource(titleKey), header.Title);
            Assert.Equal(GetResource(subtitleKey), header.SubTitle);
        }
    }

    private static string GetResource(string key) =>
        ResourceManager.GetString(key, CultureInfo.InvariantCulture)!;

    private static void AssertXamlFileUsesResources(string path)
    {
        var document = XDocument.Load(path);
        var literalAttributes = document.Descendants()
            .Attributes()
            .Where(IsTranslatableAttribute)
            .Where(attribute => !attribute.Value.StartsWith("{", StringComparison.Ordinal));

        foreach (var attribute in literalAttributes)
        {
            Assert.StartsWith("{x:Static resources:Resources.", attribute.Value);
            Assert.EndsWith("}", attribute.Value);
        }
    }

    private static bool IsTranslatableAttribute(XAttribute attribute) =>
        TranslatableAttributeNames.Contains(attribute.Name.LocalName, StringComparer.Ordinal) ||
        (attribute.Name.LocalName == "Name" &&
         attribute.Name.NamespaceName == "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "LabSampleManager.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new DirectoryNotFoundException("Repository root was not found.");
    }
}
