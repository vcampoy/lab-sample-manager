using System.Windows;
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
            ContentHost.Content = new DashboardView();
        }

        private void OnNavigationRequested(object sender, NavigationRequestedEventArgs args)
        {
            if (args.Destination == MenuDestination.Dashboard)
            {
                ContentHost.Content = new DashboardView();
            }
        }
    }
}
