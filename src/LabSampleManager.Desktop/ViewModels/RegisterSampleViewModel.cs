using System.ComponentModel;
using System.Runtime.CompilerServices;
using LabSampleManager.Desktop.Validation;
using LabSampleManager.Desktop.Properties;

namespace LabSampleManager.Desktop.ViewModels;

public enum SamplePriority { Normal, Urgent, STAT }

public sealed class RegisterSampleViewModel : INotifyPropertyChanged
{
    private readonly ISampleRegistrationValidator _validator;
    private string _barcode = string.Empty, _patientCode = string.Empty, _notes = string.Empty;
    private string? _selectedSampleType;
    private SamplePriority _selectedPriority = SamplePriority.Normal;
    private DateTime? _receivedDateTime;
    public RegisterSampleViewModel(ISampleRegistrationValidator? validator = null)
    { _validator = validator ?? new BasicSampleRegistrationValidator(); _receivedDateTime = DateTime.Now; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public IReadOnlyList<string> SampleTypes { get; } = [Resources.SampleTypeBlood, Resources.SampleTypeUrine];
    public string Barcode { get => _barcode; set { if (_barcode == value) return; _barcode = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsBarcodeValid)); } }
    public string PatientCode { get => _patientCode; set { if (_patientCode == value) return; _patientCode = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsPatientCodeValid)); } }
    public string? SelectedSampleType { get => _selectedSampleType; set { if (_selectedSampleType == value) return; _selectedSampleType = value; OnPropertyChanged(); } }
    public SamplePriority SelectedPriority { get => _selectedPriority; set { if (_selectedPriority == value) return; _selectedPriority = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNormalPriority)); OnPropertyChanged(nameof(IsUrgentPriority)); OnPropertyChanged(nameof(IsStatPriority)); } }
    public bool IsNormalPriority { get => SelectedPriority == SamplePriority.Normal; set { if (value) SelectedPriority = SamplePriority.Normal; } }
    public bool IsUrgentPriority { get => SelectedPriority == SamplePriority.Urgent; set { if (value) SelectedPriority = SamplePriority.Urgent; } }
    public bool IsStatPriority { get => SelectedPriority == SamplePriority.STAT; set { if (value) SelectedPriority = SamplePriority.STAT; } }
    public DateTime? ReceivedDateTime { get => _receivedDateTime; set { if (_receivedDateTime == value) return; _receivedDateTime = value; OnPropertyChanged(); } }
    public string Notes { get => _notes; set { if (_notes == value) return; _notes = value; OnPropertyChanged(); } }
    public bool IsBarcodeValid => _validator.IsBarcodeValid(Barcode);
    public bool IsPatientCodeValid => _validator.IsPatientCodeValid(PatientCode);
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


