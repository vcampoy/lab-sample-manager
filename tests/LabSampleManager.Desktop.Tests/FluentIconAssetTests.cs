using System.Windows;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class FluentIconAssetTests
{
    private static readonly string[] RequiredIconUris =
    [
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Beaker/ic_fluent_beaker_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Home/ic_fluent_home_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Home/ic_fluent_home_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/DocumentText/ic_fluent_document_text_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/DocumentText/ic_fluent_document_text_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/AddCircle/ic_fluent_add_circle_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/AddCircle/ic_fluent_add_circle_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Settings/ic_fluent_settings_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Settings/ic_fluent_settings_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/CheckmarkCircle/ic_fluent_checkmark_circle_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/CheckmarkCircle/ic_fluent_checkmark_circle_24_filled.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Options/ic_fluent_options_24_regular.png",
        "/LabSampleManager.Desktop;component/Assets/Icons/Fluent/Options/ic_fluent_options_24_filled.png"
    ];

    [StaFact]
    public void menuIconResources_should_be_embedded_in_desktop_assembly()
    {
        foreach (var iconUri in RequiredIconUris)
        {
            Assert.NotNull(Application.GetResourceStream(new Uri(iconUri, UriKind.Relative)));
        }
    }
}
