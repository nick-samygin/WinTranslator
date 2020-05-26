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
    /// Interaction logic for SetPINScreen.xaml
    /// </summary>

    [Export]
    public partial class SetPINScreen : Page
    {
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver;
        
        [ImportingConstructor]
        public SetPINScreen([Import(typeof(IResolver))]IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            DataContext = new SetPINScreenViewModel(resolver);
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
       
        /// <summary>
        /// not allow to copy paste cut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void HandleCanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    if (e.Command == ApplicationCommands.Cut ||
        //                     e.Command == ApplicationCommands.Copy ||
        //                     e.Command == ApplicationCommands.Paste)
        //    {
        //        e.CanExecute = false;
        //        e.Handled = true;
        //    }
        //}

        //code to allow number only input in textbox       
        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
