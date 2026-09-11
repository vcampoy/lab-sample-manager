using System.Windows;
using System.Windows.Controls;

namespace LabSampleManager.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(HeaderControl),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.Register(
                nameof(SubTitle),
                typeof(string),
                typeof(HeaderControl),
                new PropertyMetadata(string.Empty));

        public HeaderControl()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string SubTitle
        {
            get => (string)GetValue(SubTitleProperty);
            set => SetValue(SubTitleProperty, value);
        }
    }
}
