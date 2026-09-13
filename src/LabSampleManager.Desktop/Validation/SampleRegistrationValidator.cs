namespace LabSampleManager.Desktop.Validation;

public interface ISampleRegistrationValidator
{
    bool IsBarcodeValid(string? barcode);
    bool IsPatientCodeValid(string? patientCode);
}

/// <summary>Temporary non-empty validation seam. The domain has not specified barcode or patient-code formats yet.</summary>
public sealed class BasicSampleRegistrationValidator : ISampleRegistrationValidator
{
    public bool IsBarcodeValid(string? barcode) => !string.IsNullOrWhiteSpace(barcode);
    public bool IsPatientCodeValid(string? patientCode) => !string.IsNullOrWhiteSpace(patientCode);
}
