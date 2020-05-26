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
    /// Interaction logic for ForgotMasterPassword.xaml
    /// </summary>
    public partial class ForgotMasterPassword : Window
    {
        SystemTray tray = new SystemTray();
        public ForgotMasterPassword()
        {
            InitializeComponent();
        }
        
        private void MainGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = tray.CurrentWindow("ForgotMasterPasswordWindow");
            if (window != null)
            {
                window.DragMove();
            }
           
        }
    }
}
