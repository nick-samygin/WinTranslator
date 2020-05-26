using PasswordBoss.Helpers;
using System.Windows;
using System.Windows.Input;

namespace PasswordBoss.Views.ApplicationUpdates
{
    /// <summary>
    /// Interaction logic for CheckForUpdatesWindow.xaml
    /// </summary>
    public partial class CheckForUpdatesWindow : Window
    {
        public CheckForUpdatesWindow()
        {
            InitializeComponent();
            CloseCommand = new RelayCommand(CloseWindow);
            DataContext = this;         
        }
        public ICommand CloseCommand { get; set; }

        private void CloseWindow(object o)
        {
            Window holder = o as Window;
            if (holder != null)
            {
                holder.Close();
            }
        }
    }
}
