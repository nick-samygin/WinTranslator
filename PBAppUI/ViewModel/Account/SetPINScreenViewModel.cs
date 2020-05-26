using PasswordBoss.Helpers;
using PasswordBoss.Views.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PasswordBoss.ViewModel.Account
{
    [Export]
    public class SetPINScreenViewModel : ViewModelBase
    {
        /// <summary>
        /// defining commands for UI elements
        /// </summary>
        public RelayCommand SetPinButtonCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }


        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver = null;
        public SetPINScreenViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            SetPinButtonCommand = new RelayCommand(SetPinButtonClick);
            CloseCommand = new RelayCommand(CloseWindow);
        }
        /// <summary>
        /// For Closing login window
        /// </summary>
        private void CloseWindow(object sender)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            _systemTray.WindowClose(window);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SetPinButtonClick(object obj)
        {
            SetupComplete setumComplete = resolver.GetInstanceOf<SetupComplete>();
            Navigator.NavigationService.Navigate(setumComplete);
        }
    }
}
