using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LabSampleManager.Desktop.Controls;

public enum FieldIndicatorMode { Never, Always, WhenInvalid }

internal static class FieldIndicatorState
{
    public static bool ShouldShow(FieldIndicatorMode mode, bool isValid, bool isEnabled) =>
        mode == FieldIndicatorMode.Always ? isEnabled : mode == FieldIndicatorMode.WhenInvalid && !isValid;
}

public sealed class TextBoxControl : TextBox
{
    static TextBoxControl() { DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxControl), new FrameworkPropertyMetadata(typeof(TextBoxControl))); IsEnabledProperty.OverrideMetadata(typeof(TextBoxControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged)); }
    public static readonly DependencyProperty TitleProperty = RegisterString(nameof(Title));
    public static readonly DependencyProperty SubTitleProperty = RegisterString(nameof(SubTitle));
    public static readonly DependencyProperty FieldIndicatorModeProperty = DependencyProperty.Register(nameof(FieldIndicatorMode), typeof(FieldIndicatorMode), typeof(TextBoxControl), new FrameworkPropertyMetadata(FieldIndicatorMode.Never, OnIndicatorStateChanged));
    public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(TextBoxControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged));
    public static readonly DependencyProperty ShowFieldIndicatorProperty = DependencyProperty.Register(nameof(ShowFieldIndicator), typeof(bool), typeof(TextBoxControl), new FrameworkPropertyMetadata(false));
    private static DependencyProperty RegisterString(string name) => DependencyProperty.Register(name, typeof(string), typeof(TextBoxControl), new FrameworkPropertyMetadata(string.Empty));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string SubTitle { get => (string)GetValue(SubTitleProperty); set => SetValue(SubTitleProperty, value); }
    public FieldIndicatorMode FieldIndicatorMode { get => (FieldIndicatorMode)GetValue(FieldIndicatorModeProperty); set => SetValue(FieldIndicatorModeProperty, value); }
    public bool IsValid { get => (bool)GetValue(IsValidProperty); set => SetValue(IsValidProperty, value); }
    public bool ShowFieldIndicator => (bool)GetValue(ShowFieldIndicatorProperty);
    private static void OnIndicatorStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextBoxControl)d).UpdateIndicatorState();
    private void UpdateIndicatorState() => SetValue(ShowFieldIndicatorProperty, FieldIndicatorState.ShouldShow(FieldIndicatorMode, IsValid, IsEnabled));
}

public sealed class DropDownControl : ComboBox
{
    static DropDownControl() { DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownControl), new FrameworkPropertyMetadata(typeof(DropDownControl))); IsEditableProperty.OverrideMetadata(typeof(DropDownControl), new FrameworkPropertyMetadata(false)); IsEnabledProperty.OverrideMetadata(typeof(DropDownControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged)); }
    public static readonly DependencyProperty TitleProperty = RegisterString(nameof(Title));
    public static readonly DependencyProperty SubTitleProperty = RegisterString(nameof(SubTitle));
    public static readonly DependencyProperty FieldIndicatorModeProperty = DependencyProperty.Register(nameof(FieldIndicatorMode), typeof(FieldIndicatorMode), typeof(DropDownControl), new FrameworkPropertyMetadata(FieldIndicatorMode.Never, OnIndicatorStateChanged));
    public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(DropDownControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged));
    public static readonly DependencyProperty ShowFieldIndicatorProperty = DependencyProperty.Register(nameof(ShowFieldIndicator), typeof(bool), typeof(DropDownControl), new FrameworkPropertyMetadata(false));
    private static DependencyProperty RegisterString(string name) => DependencyProperty.Register(name, typeof(string), typeof(DropDownControl), new FrameworkPropertyMetadata(string.Empty));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string SubTitle { get => (string)GetValue(SubTitleProperty); set => SetValue(SubTitleProperty, value); }
    public FieldIndicatorMode FieldIndicatorMode { get => (FieldIndicatorMode)GetValue(FieldIndicatorModeProperty); set => SetValue(FieldIndicatorModeProperty, value); }
    public bool IsValid { get => (bool)GetValue(IsValidProperty); set => SetValue(IsValidProperty, value); }
    public bool ShowFieldIndicator => (bool)GetValue(ShowFieldIndicatorProperty);
    private static void OnIndicatorStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((DropDownControl)d).UpdateIndicatorState();
    private void UpdateIndicatorState() => SetValue(ShowFieldIndicatorProperty, FieldIndicatorState.ShouldShow(FieldIndicatorMode, IsValid, IsEnabled));
}

public sealed class RadioButtonControl : RadioButton
{
    static RadioButtonControl() { DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonControl), new FrameworkPropertyMetadata(typeof(RadioButtonControl))); }
}

public sealed class DateTimePickerControl : Control
{
    static DateTimePickerControl() { DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePickerControl), new FrameworkPropertyMetadata(typeof(DateTimePickerControl))); IsEnabledProperty.OverrideMetadata(typeof(DateTimePickerControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged)); }
    public static readonly DependencyProperty TitleProperty = RegisterString(nameof(Title));
    public static readonly DependencyProperty SubTitleProperty = RegisterString(nameof(SubTitle));
    public static readonly DependencyProperty FieldIndicatorModeProperty = DependencyProperty.Register(nameof(FieldIndicatorMode), typeof(FieldIndicatorMode), typeof(DateTimePickerControl), new FrameworkPropertyMetadata(FieldIndicatorMode.Never, OnIndicatorStateChanged));
    public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(DateTimePickerControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged));
    public static readonly DependencyProperty ShowFieldIndicatorProperty = DependencyProperty.Register(nameof(ShowFieldIndicator), typeof(bool), typeof(DateTimePickerControl), new FrameworkPropertyMetadata(false));
    public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(nameof(SelectedDateTime), typeof(DateTime?), typeof(DateTimePickerControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateTimeChanged));
    public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedDateTimeChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(DateTimePickerControl));
    private DatePicker? _datePicker; private TextBox? _timeTextBox;
    private static DependencyProperty RegisterString(string name) => DependencyProperty.Register(name, typeof(string), typeof(DateTimePickerControl), new FrameworkPropertyMetadata(string.Empty));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string SubTitle { get => (string)GetValue(SubTitleProperty); set => SetValue(SubTitleProperty, value); }
    public FieldIndicatorMode FieldIndicatorMode { get => (FieldIndicatorMode)GetValue(FieldIndicatorModeProperty); set => SetValue(FieldIndicatorModeProperty, value); }
    public bool IsValid { get => (bool)GetValue(IsValidProperty); set => SetValue(IsValidProperty, value); }
    public bool ShowFieldIndicator => (bool)GetValue(ShowFieldIndicatorProperty);
    public DateTime? SelectedDateTime { get => (DateTime?)GetValue(SelectedDateTimeProperty); set => SetValue(SelectedDateTimeProperty, value); }
    public event RoutedPropertyChangedEventHandler<DateTime?> SelectedDateTimeChanged { add => AddHandler(SelectedDateTimeChangedEvent, value); remove => RemoveHandler(SelectedDateTimeChangedEvent, value); }
    private static void OnIndicatorStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((DateTimePickerControl)d).UpdateIndicatorState();
    private void UpdateIndicatorState() => SetValue(ShowFieldIndicatorProperty, FieldIndicatorState.ShouldShow(FieldIndicatorMode, IsValid, IsEnabled));
    public override void OnApplyTemplate()
    {
        if (_datePicker is not null) _datePicker.SelectedDateChanged -= OnDateChanged;
        if (_timeTextBox is not null) { _timeTextBox.LostFocus -= OnTimeLostFocus; _timeTextBox.KeyDown -= OnTimeKeyDown; }
        base.OnApplyTemplate();
        _datePicker = GetTemplateChild("PART_DatePicker") as DatePicker;
        _timeTextBox = GetTemplateChild("PART_TimeTextBox") as TextBox;
        if (_datePicker is not null) _datePicker.SelectedDateChanged += OnDateChanged;
        if (_timeTextBox is not null) { _timeTextBox.LostFocus += OnTimeLostFocus; _timeTextBox.KeyDown += OnTimeKeyDown; }
        UpdateParts();
    }
    private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (DateTimePickerControl)d; control.UpdateParts();
        control.RaiseEvent(new RoutedPropertyChangedEventArgs<DateTime?>((DateTime?)e.OldValue, (DateTime?)e.NewValue, SelectedDateTimeChangedEvent));
    }
    private void UpdateParts() { if (_datePicker is not null) _datePicker.SelectedDate = SelectedDateTime?.Date; if (_timeTextBox is not null) _timeTextBox.Text = SelectedDateTime?.ToString("t", System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty; }
    private void OnDateChanged(object? sender, SelectionChangedEventArgs e) { if (_datePicker?.SelectedDate is DateTime date) SetDate(date); else if (SelectedDateTime is not null) SelectedDateTime = null; }
    private void SetDate(DateTime date) { var old = SelectedDateTime; SelectedDateTime = date.Date + (old?.TimeOfDay ?? TimeSpan.Zero); }
    private void OnTimeLostFocus(object sender, RoutedEventArgs e) => CommitTime();
    private void OnTimeKeyDown(object sender, System.Windows.Input.KeyEventArgs e) { if (e.Key == System.Windows.Input.Key.Enter) { CommitTime(); e.Handled = true; } }
    private void CommitTime()
    {
        if (_timeTextBox is null || !DateTime.TryParse(_timeTextBox.Text, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out var parsed)) return;
        var date = SelectedDateTime?.Date ?? DateTime.Today; SelectedDateTime = date + parsed.TimeOfDay;
    }
}

public sealed class TextAreaControl : TextBox
{
    private static readonly DependencyPropertyKey CharacterCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CharacterCount), typeof(int), typeof(TextAreaControl), new FrameworkPropertyMetadata(0));
    private static readonly DependencyPropertyKey CharacterCountTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CharacterCountText), typeof(string), typeof(TextAreaControl), new FrameworkPropertyMetadata("0/0"));
    static TextAreaControl() { DefaultStyleKeyProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(typeof(TextAreaControl))); IsEnabledProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged)); AcceptsReturnProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(true)); TextWrappingProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(TextWrapping.Wrap)); VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto)); MaxLengthProperty.OverrideMetadata(typeof(TextAreaControl), new FrameworkPropertyMetadata(0, OnMaxLengthChanged)); }
    public static readonly DependencyProperty TitleProperty = RegisterString(nameof(Title));
    public static readonly DependencyProperty SubTitleProperty = RegisterString(nameof(SubTitle));
    public static readonly DependencyProperty PlaceholderProperty = RegisterString(nameof(Placeholder));
    public static readonly DependencyProperty FieldIndicatorModeProperty = DependencyProperty.Register(nameof(FieldIndicatorMode), typeof(FieldIndicatorMode), typeof(TextAreaControl), new FrameworkPropertyMetadata(FieldIndicatorMode.Never, OnIndicatorStateChanged));
    public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(TextAreaControl), new FrameworkPropertyMetadata(true, OnIndicatorStateChanged));
    public static readonly DependencyProperty ShowFieldIndicatorProperty = DependencyProperty.Register(nameof(ShowFieldIndicator), typeof(bool), typeof(TextAreaControl), new FrameworkPropertyMetadata(false));
    public static readonly DependencyProperty CharacterCountProperty = CharacterCountPropertyKey.DependencyProperty;
    public static readonly DependencyProperty CharacterCountTextProperty = CharacterCountTextPropertyKey.DependencyProperty;
    private static DependencyProperty RegisterString(string name) => DependencyProperty.Register(name, typeof(string), typeof(TextAreaControl), new FrameworkPropertyMetadata(string.Empty));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string SubTitle { get => (string)GetValue(SubTitleProperty); set => SetValue(SubTitleProperty, value); }
    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
    public FieldIndicatorMode FieldIndicatorMode { get => (FieldIndicatorMode)GetValue(FieldIndicatorModeProperty); set => SetValue(FieldIndicatorModeProperty, value); }
    public bool IsValid { get => (bool)GetValue(IsValidProperty); set => SetValue(IsValidProperty, value); }
    public bool ShowFieldIndicator => (bool)GetValue(ShowFieldIndicatorProperty);
    public int CharacterCount => (int)GetValue(CharacterCountProperty);
    public string CharacterCountText => (string)GetValue(CharacterCountTextProperty);
    private static void OnIndicatorStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextAreaControl)d).UpdateIndicatorState();
    private void UpdateIndicatorState() => SetValue(ShowFieldIndicatorProperty, FieldIndicatorState.ShouldShow(FieldIndicatorMode, IsValid, IsEnabled));
    private static void OnMaxLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((TextAreaControl)d).UpdateCharacterCount();
    private void UpdateCharacterCount() { var max = MaxLength > 0 ? MaxLength : 0; SetValue(CharacterCountPropertyKey, max > 0 ? Math.Min(Text.Length, max) : Text.Length); SetValue(CharacterCountTextPropertyKey, $"{CharacterCount}/{max}"); }
    protected override void OnTextChanged(TextChangedEventArgs e) { base.OnTextChanged(e); var max = MaxLength > 0 ? MaxLength : int.MaxValue; if (Text.Length > max) Text = Text[..max]; UpdateCharacterCount(); }
}


