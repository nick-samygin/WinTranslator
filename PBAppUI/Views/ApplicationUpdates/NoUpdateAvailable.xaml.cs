using PasswordBoss.Helpers;
using System.Windows;
using System.Windows.Input;

namespace PasswordBoss.Views.ApplicationUpdates
{
    /// <summary>
    /// Interaction logic for NoUpdateAvailable.xaml
    /// </summary>
    public partial class NoUpdateAvailable : Window
    {
        public NoUpdateAvailable()
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
