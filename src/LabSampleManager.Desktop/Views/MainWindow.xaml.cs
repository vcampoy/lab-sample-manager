using System.Windows;
using System.Windows.Controls;
using LabSampleManager.Desktop.Controls;
using LabSampleManager.Desktop.Views;

namespace LabSampleManager.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentHost.Content = CreateView(MenuDestination.Dashboard);
        }

        private void OnNavigationRequested(object sender, NavigationRequestedEventArgs args)
        {
            ContentHost.Content = CreateView(args.Destination);
        }

        private static UserControl CreateView(MenuDestination destination) =>
            destination switch
            {
                MenuDestination.Dashboard => new DashboardView(),
                MenuDestination.Samples => new SamplesView(),
                MenuDestination.RegisterSample => new RegisterSampleView(),
                MenuDestination.Processing => new ProcessingMonitor(),
                MenuDestination.Validation => new ResultsValidationView(),
                _ => throw new ArgumentOutOfRangeException(nameof(destination), destination, "Unsupported menu destination.")
            };
    }
}
