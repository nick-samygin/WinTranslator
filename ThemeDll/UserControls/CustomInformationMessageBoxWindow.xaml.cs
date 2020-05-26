using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PasswordBoss.UserControls
{
    /// <summary>
    /// Interaction logic for CustomInformationMessageBoxWindow.xaml
    /// </summary>
    public partial class CustomInformationMessageBoxWindow : Window
    {
        public CustomInformationMessageBoxWindow(string message)
        {
            InitializeComponent();
            var parent = System.Windows.Application.Current.MainWindow as Window;
            Owner = parent;
            Top = parent.Top;
            Left = parent.Left;
            Height = parent.ActualHeight;
            Width = parent.ActualWidth;
            WindowStartupLocation = parent.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
            if (!string.IsNullOrEmpty(message))
            {
                DialogMessageTextBlock.Text = message;
            }
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
