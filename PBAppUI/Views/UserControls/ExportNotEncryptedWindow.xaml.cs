using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ExportNotEncryptedWindow.xaml
    /// </summary>
    public partial class ExportNotEncryptedWindow : Window
    {
        public ExportNotEncryptedWindow(MenuViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            var parent = System.Windows.Application.Current.MainWindow as Window;
            Owner = parent;
            Top = parent.Top;
            Left = parent.Left;
            Height = parent.ActualHeight;
            Width = parent.ActualWidth;
            WindowStartupLocation = parent.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
