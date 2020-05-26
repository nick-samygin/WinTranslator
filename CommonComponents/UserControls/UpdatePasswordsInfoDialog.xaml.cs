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
using PasswordBoss.ViewModel;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UpdatePasswordsInfoDialog.xaml
    /// </summary>
    public partial class UpdatePasswordsInfoDialog : Window
    {
        public UpdatePasswordsInfoDialog(Window owner)
        {
            this.Owner = owner;
            this.Height = owner.ActualHeight;
            this.Width = owner.ActualWidth;
            this.Left = owner.Left;
            this.Top = owner.Top;
            this.WindowStartupLocation = owner.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
