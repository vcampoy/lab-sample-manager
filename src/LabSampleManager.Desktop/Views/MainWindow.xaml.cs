using System.Windows;
using System.Windows.Controls;
using LabSampleManager.Desktop.Controls;
using LocalizationResources = LabSampleManager.Desktop.Properties.Resources;
using LabSampleManager.Desktop.Views;
using LabSampleManager.Desktop.ViewModels;

namespace LabSampleManager.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RegisterSampleViewModel _registerSampleViewModel = new();
        public MainWindow()
        {
            InitializeComponent();
            ContentHost.Content = CreateView(MenuDestination.Dashboard);
        }

        private void OnNavigationRequested(object sender, NavigationRequestedEventArgs args)
        {
            ContentHost.Content = CreateView(args.Destination);
        }

        private UserControl CreateView(MenuDestination destination) =>
            destination switch
            {
                MenuDestination.Dashboard => new DashboardView(),
                MenuDestination.Samples => new SamplesView(),
                MenuDestination.RegisterSample => new RegisterSampleView(_registerSampleViewModel),
                MenuDestination.Processing => new ProcessingMonitor(),
                MenuDestination.Validation => new ResultsValidationView(),
                _ => throw new ArgumentOutOfRangeException(nameof(destination), destination, LocalizationResources.UnsupportedMenuDestination)
            };
    }
}
