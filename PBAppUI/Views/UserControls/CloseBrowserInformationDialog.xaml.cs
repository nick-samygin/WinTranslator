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
    /// Interaction logic for CloseBrowserInformationDialog.xaml
    /// </summary>
    public partial class CloseBrowserInformationDialog : Window
    {
        public CloseBrowserInformationDialog(string header, string bodyText)
        {
            InitializeComponent();
            HeaderText.Text = header;
            DialogMessageText.Text = bodyText;
            this.Topmost = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
   
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
