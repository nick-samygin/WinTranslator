using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
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
    /// Interaction logic for UpdateData.xaml
    /// </summary>
    [Export]
    public partial class UpdateData : Page
    {
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver;
        [ImportingConstructor]
        public UpdateData([Import(typeof(IResolver))]IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            DataContext = new UpdateDataViewModel(resolver);
            InitializeComponent();
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

/// <summary>
/// navigation to next page
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
      

        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var navService = NavigationService.GetNavigationService(this);
            //if (navService != null)
            //    navService.Navigate(new Uri(VerificationRequiredViewPath, UriKind.RelativeOrAbsolute));
        }
    }
}
