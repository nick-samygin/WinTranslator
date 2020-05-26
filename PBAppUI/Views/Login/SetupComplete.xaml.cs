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
    /// Interaction logic for SetupComplete.xaml
    /// </summary>
   // [Export]
    public partial class SetupComplete : Page
    {
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver;

      //  [ImportingConstructor]
        public SetupComplete(IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            DataContext = new SetupCompleteViewModel(resolver);
            InitializeComponent();
        }
        /// <summary>
        /// will Drag window on mouse left button down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            window.DragMove();
        }
    }
}
