using System.Windows.Controls;
using LabSampleManager.Desktop.ViewModels;

namespace LabSampleManager.Desktop.Views;

public partial class RegisterSampleView : UserControl
{
    public RegisterSampleView() : this(new RegisterSampleViewModel()) { }
    public RegisterSampleView(RegisterSampleViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
