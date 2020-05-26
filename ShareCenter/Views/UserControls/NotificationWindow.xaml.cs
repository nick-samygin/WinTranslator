using System.Windows;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow
    {
        public NotificationWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = new bool?(false);
            Close();
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            DialogResult = new bool?(true);
            Close();
        }
    }
}
