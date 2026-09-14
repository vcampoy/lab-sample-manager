using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LabSampleManager.Desktop.Controls;
using LabSampleManager.Desktop.Models;
using LabSampleManager.Desktop.Properties;
using LabSampleManager.Desktop.Validation;
using LabSampleManager.Desktop.ViewModels;
using Xunit;

namespace LabSampleManager.Desktop.Tests;

public sealed class FieldControlsTests
{
    private static readonly SampleType[] ExpectedSampleTypes = [SampleType.Blood, SampleType.Urine];

    [StaFact]
    public void textBox_should_exposeFieldDefaults_and_indicatorModes()
    {
        var control = new TextBoxControl();
        Assert.Equal(string.Empty, control.Title);
        Assert.Equal(string.Empty, control.SubTitle);
        Assert.Equal(FieldIndicatorMode.Never, control.FieldIndicatorMode);
        Assert.True(control.IsValid);
        Assert.False(control.ShowFieldIndicator);
        ApplyTemplate(control);
        control.FieldIndicatorMode = FieldIndicatorMode.Always;
        Assert.True(control.ShowFieldIndicator);
        control.FieldIndicatorMode = FieldIndicatorMode.WhenInvalid;
        control.IsValid = false;
        Assert.True(control.ShowFieldIndicator);
    }

    [StaFact]
    public void dropDown_should_beNonEditable_and_supportSampleOptions()
    {
        var control = new DropDownControl { ItemsSource = new[] { Resources.SampleTypeBlood, Resources.SampleTypeUrine } };
        Assert.False(control.IsEditable);
        Assert.Equal(FieldIndicatorMode.Never, control.FieldIndicatorMode);
        control.FieldIndicatorMode = FieldIndicatorMode.Always;
        ApplyTemplate(control);
        Assert.Equal(2, control.Items.Count);
        Assert.Equal(Resources.SampleTypeBlood, control.Items[0]);
        Assert.Equal(Resources.SampleTypeUrine, control.Items[1]);
        Assert.True(control.ShowFieldIndicator);
    }

    [Fact]
    public void registerSampleViewModel_should_exposeTypedSampleTypes_when_created()
    {
        var viewModel = new RegisterSampleViewModel();

        Assert.Equal(ExpectedSampleTypes, viewModel.SampleTypes);
        Assert.Equal(SampleType.Blood, viewModel.SelectedSampleType);
    }

    [Fact]
    public void selectedSampleType_should_raisePropertyChanged_when_value_changes()
    {
        var viewModel = new RegisterSampleViewModel();
        var changedProperty = string.Empty;
        viewModel.PropertyChanged += (_, args) => changedProperty = args.PropertyName ?? string.Empty;

        viewModel.SelectedSampleType = SampleType.Urine;

        Assert.Equal(SampleType.Urine, viewModel.SelectedSampleType);
        Assert.Equal(nameof(RegisterSampleViewModel.SelectedSampleType), changedProperty);
    }

    [StaFact]
    public void dropDown_should_resolvePopupPlacementTarget_whenTemplateIsApplied()
    {
        var control = new DropDownControl();
        var window = new Window { Content = control, Width = 320, Height = 120 };
        window.Show();
        try
        {
            control.UpdateLayout();

            var border = Assert.IsType<Border>(control.Template!.FindName("PART_DropDownBorder", control));
            var popup = Assert.IsType<Popup>(control.Template.FindName("PART_Popup", control));

            Assert.Same(border, popup.PlacementTarget);

            control.IsDropDownOpen = true;
            Assert.True(popup.IsOpen);
            control.IsDropDownOpen = false;
        }
        finally
        {
            window.Close();
        }
    }

    [StaFact]
    public void textBox_should_useCompactSurface_and_separateTitleSurfaceSubtitleRows()
    {
        var control = new TextBoxControl { Title = "Barcode", SubTitle = "Enter the barcode" };
        ApplyTemplate(control);

        var fieldGrid = Assert.IsType<Grid>(control.Template!.FindName("PART_FieldGrid", control));
        var title = Assert.IsType<TextBlock>(control.Template.FindName("PART_Title", control));
        var surface = Assert.IsType<Border>(control.Template.FindName("PART_Surface", control));
        var subtitle = Assert.IsType<TextBlock>(control.Template.FindName("PART_SubTitle", control));

        Assert.Equal(3, fieldGrid.RowDefinitions.Count);
        Assert.Equal(0, control.MinHeight);
        Assert.Equal(38, surface.Height);
        Assert.True(Grid.GetRow(title) < Grid.GetRow(surface));
        Assert.True(Grid.GetRow(surface) < Grid.GetRow(subtitle));
    }

    [StaFact]
    public void textBox_should_collapseEmptySubtitle_without_reservingLayoutSpace()
    {
        var control = new TextBoxControl { Title = "Barcode" };
        ApplyTemplate(control);

        var subtitle = Assert.IsType<TextBlock>(control.Template!.FindName("PART_SubTitle", control));

        Assert.Equal(Visibility.Collapsed, subtitle.Visibility);

        control.SubTitle = "Enter the barcode";
        Assert.Equal(Visibility.Visible, subtitle.Visibility);

        control.SubTitle = null!;
        Assert.Equal(Visibility.Collapsed, subtitle.Visibility);
    }

    [StaFact]
    public void dropDown_should_useCompactSurface_and_separateTitleSurfaceSubtitleRows()
    {
        var control = new DropDownControl { Title = "Sample type", SubTitle = "Select the sample type" };
        ApplyTemplate(control);

        var fieldGrid = Assert.IsType<Grid>(control.Template!.FindName("PART_FieldGrid", control));
        var title = Assert.IsType<TextBlock>(control.Template.FindName("PART_Title", control));
        var surface = Assert.IsType<Border>(control.Template.FindName("PART_DropDownBorder", control));
        var subtitle = Assert.IsType<TextBlock>(control.Template.FindName("PART_SubTitle", control));

        Assert.Equal(3, fieldGrid.RowDefinitions.Count);
        Assert.Equal(0, control.MinHeight);
        Assert.Equal(38, surface.Height);
        Assert.True(Grid.GetRow(title) < Grid.GetRow(surface));
        Assert.True(Grid.GetRow(surface) < Grid.GetRow(subtitle));
    }

    [StaFact]
    public void dropDown_should_forwardSelectionTemplate_toSelectedValuePresenter_when_templateIsApplied()
    {
        var itemTemplate = new DataTemplate();
        var control = new DropDownControl
        {
            ItemsSource = ExpectedSampleTypes,
            ItemTemplate = itemTemplate,
            SelectedItem = SampleType.Blood
        };
        ApplyTemplate(control);

        var toggleButton = Assert.IsType<ToggleButton>(control.Template!.FindName("PART_ToggleButton", control));
        var contentPresenter = Assert.IsType<ContentPresenter>(toggleButton.Content);

        Assert.Same(itemTemplate, contentPresenter.ContentTemplate);
    }

    [StaFact]
    public void radioButton_should_supportPriorityValues_and_groupSelection()
    {
        var normal = new RadioButtonControl { Content = Resources.PriorityNormal, GroupName = "Priority" };
        var urgent = new RadioButtonControl { Content = Resources.PriorityUrgent, GroupName = "Priority" };
        var stat = new RadioButtonControl { Content = Resources.PriorityStat, GroupName = "Priority" };
        normal.IsChecked = true;
        Assert.True(normal.IsChecked);
        Assert.False(urgent.IsChecked);
        Assert.False(stat.IsChecked);
        Assert.Equal("Normal", normal.Content);
        Assert.Equal("Urgent", urgent.Content);
        Assert.Equal("STAT", stat.Content);
        ApplyTemplate(normal);
        Assert.NotNull(normal.Template!.FindName("PART_OuterCircle", normal));
    }

    [StaFact]
    public void dateTimePicker_should_preserveDateAndTime_when_parts_change()
    {
        var initial = new DateTime(2026, 9, 13, 14, 35, 0);
        var control = new DateTimePickerControl { SelectedDateTime = initial, FieldIndicatorMode = FieldIndicatorMode.Always };
        ApplyTemplate(control);
        var date = Assert.IsType<DatePicker>(control.Template!.FindName("PART_DatePicker", control));
        var time = Assert.IsType<TextBox>(control.Template.FindName("PART_TimeTextBox", control));
        Assert.Equal(initial.Date, date.SelectedDate);
        Assert.Equal(initial.ToString("t", CultureInfo.CurrentCulture), time.Text);
        date.SelectedDate = initial.Date.AddDays(1);
        Assert.Equal(initial.TimeOfDay, control.SelectedDateTime!.Value.TimeOfDay);
        Assert.True(control.ShowFieldIndicator);
        date.SelectedDate = null;
        Assert.Null(control.SelectedDateTime);
    }

    [StaFact]
    public void textArea_should_limitText_and_updatePlaceholderCounter()
    {
        var control = new TextAreaControl { Placeholder = Resources.NotesPlaceholder, MaxLength = 5 };
        ApplyTemplate(control);
        Assert.Equal(0, control.CharacterCount);
        Assert.Equal("0/5", control.CharacterCountText);
        var placeholder = Assert.IsType<TextBlock>(control.Template!.FindName("PART_Placeholder", control));
        Assert.Equal(Visibility.Visible, placeholder.Visibility);
        control.Text = "abcdef";
        Assert.Equal("abcde", control.Text);
        Assert.Equal(5, control.CharacterCount);
        Assert.Equal("5/5", control.CharacterCountText);
        control.Text = "ab";
        Assert.Equal("2/5", control.CharacterCountText);
    }

    [StaFact]
    public void registerSampleView_should_wireFields_withoutScanner_and_useValidatorSeam()
    {
        var validator = new TestValidator();
        var viewModel = new RegisterSampleViewModel(validator);
        var view = new Views.RegisterSampleView(viewModel);
        var container = Assert.IsType<ContainerControl>(view.FindName("SampleInformationContainer"));
        var form = Assert.IsType<Grid>(container.Content);
        Assert.DoesNotContain(form.Children.Cast<UIElement>(), child => child is Button);
        Assert.IsType<TextBoxControl>(view.FindName("BarcodeTextBox"));
        Assert.IsType<TextBoxControl>(view.FindName("PatientCodeTextBox"));
        var dropdown = Assert.IsType<DropDownControl>(view.FindName("SampleTypeDropDown"));
        Assert.Equal(2, viewModel.SampleTypes.Count);
        Assert.Equal(SamplePriority.Normal, viewModel.SelectedPriority);
        Assert.Equal(DateTime.Today, viewModel.ReceivedDateTime!.Value.Date);
        viewModel.Barcode = "ok";
        Assert.True(viewModel.IsBarcodeValid);
        Assert.Equal(1, validator.BarcodeCalls);
    }

    [StaFact]
    public void sampleTypeDropDown_should_updateTypedSelection_when_item_changes()
    {
        var viewModel = new RegisterSampleViewModel();
        var view = new Views.RegisterSampleView(viewModel);
        var window = new Window { Content = view, Width = 640, Height = 480 };
        window.Show();
        try
        {
            view.UpdateLayout();
            var dropdown = Assert.IsType<DropDownControl>(view.FindName("SampleTypeDropDown"));
            Assert.Equal(SampleType.Blood, dropdown.SelectedItem);

            dropdown.SelectedItem = SampleType.Urine;

            Assert.Equal(SampleType.Urine, viewModel.SelectedSampleType);
            Assert.NotNull(dropdown.ItemTemplate);
        }
        finally
        {
            window.Close();
        }
    }

    private static void ApplyTemplate(Control control)
    {
        if (Application.Current is null) _ = new Application();
        if (Application.Current!.Resources[typeof(ContainerControl)] is null)
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/LabSampleManager.Desktop;component/Themes/Generic.xaml", UriKind.Relative) });
        }
        control.Style = Application.Current.Resources[control.GetType()] as Style;
        control.ApplyTemplate();
    }

    private sealed class TestValidator : ISampleRegistrationValidator
    {
        public int BarcodeCalls { get; private set; }
        public bool IsBarcodeValid(string? barcode) { BarcodeCalls++; return barcode == "ok"; }
        public bool IsPatientCodeValid(string? patientCode) => patientCode == "patient";
    }
}



