using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
//using System.Drawing;
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
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.Views.Login
{
    /// <summary>
    /// Interaction logic for ConfirmMasterPassword1.xaml
    /// </summary>
    
    public partial class ConfirmMasterPassword : Page
    {
      /// <summary>
      /// objectcreation
      /// </summary>
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver;
        private PasswordBox GlobalPasswordTextBox;
        IInAppAnalytics inAppAnalyitics;
       
        public ConfirmMasterPassword(IResolver resolver, string email, string password)
        {
            this.resolver = resolver;
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            InitializeComponent();
            DataContext = new ConfirmMasterPasswordViewModel(email, password,  resolver);            
        }
        /// <summary>
        /// to drag window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            window.DragMove();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordContentControl.ApplyTemplate();
            GlobalPasswordTextBox = PasswordContentControl.Template.FindName("GlobalPasswordTextBox", PasswordContentControl) as PasswordBox;
        }
       
    }
}
