using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace PasswordBoss.Views.Login
{
    /// <summary>
    /// Interaction logic for VerificationRequired.xaml
    /// </summary>
    
    public partial class VerificationRequired : Page
    {
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver;

        public VerificationRequired(IResolver resolver, string email, string password)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            DataContext = new VerificationRequiredViewModel(resolver, email, password);
            InitializeComponent();
            InitMessageBox();
        }

        private void DragGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            window.DragMove();
        }

        public void InitMessageBox()
        {
            
            //Window parentWindow = Window.GetWindow(this);
            //var childElements = ((Panel)parentWindow.Content).Children;
            
            CustomMessageBox deletePopup = new CustomMessageBox();
            deletePopup.Name = "MsgBoxDialogControl";
            deletePopup.CustomMessageBoxType = CustomMessageBox.CustomMessageBoxTypeEnum.Info;
            deletePopup.DataContext = this.DataContext;
            deletePopup.SetBinding(CustomMessageBox.MessageBoxOkCommandProperty, new Binding("MessageBoxInfoConfirmButtonCommand"));
            deletePopup.SetBinding(CustomMessageBox.MessageBoxVisibilityProperty, new Binding("MessageBoxInfoVisibility"));
            deletePopup.SetBinding(CustomMessageBox.MessageBoxTextProperty, new Binding("MessageBoxInfoText"));

            deletePopup.MessageBoxOkButtonText = Application.Current.FindResource("OK").ToString();
            deletePopup.MessageBoxHeaderText = Application.Current.FindResource("MessageBoxErrorTitle").ToString() ;


            VerificationMasterGrid.Children.Add(deletePopup);


        }
    }
}
