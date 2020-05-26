using System.Windows.Input;
using PasswordBoss.Helpers;
using System.Windows.Controls;
using PasswordBoss.ViewModel.Account;
using System.ComponentModel.Composition;
using System.Windows;

namespace PasswordBoss.Views.Login
{
    /// <summary>
    /// Interaction logic for LoginLanguage.xaml
    /// </summary>

    //[Export]
    public partial class LoginLanguage
    {
   //     SystemTray _systemTray = new SystemTray();

        private IPBData pbData = null;
        private IResolver resolver;
        private IPBWebAPI webAPI = null;
        SystemTray _systemTray = new SystemTray();

        [ImportingConstructor]
        public LoginLanguage([Import(typeof(IResolver))]IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.webAPI = resolver.GetInstanceOf<IPBWebAPI>();
            InitializeComponent();
            DataContext = new LoginLanguageViewModel(SelectLanguageComboBox, this.resolver);
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
        /// Used for enable the dropdown on enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectLanguageComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ((ComboBox)sender).IsDropDownOpen = true;
            }
        }

        public void ComboBoxItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
                return; 
           
            e.Handled = true;
        }
    }
}
