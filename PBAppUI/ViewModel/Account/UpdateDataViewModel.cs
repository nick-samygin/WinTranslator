using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel.Account
{
    [Export]
    public class UpdateDataViewModel:ViewModelBase
    {
        /// <summary>
        /// defining commands for UI elements
        /// </summary>  
        public RelayCommand NavigateCommand { get; set; }
        
        SystemTray _systemTray = new SystemTray();

        private IPBData pbData = null;
        private IResolver resolver = null;
        public UpdateDataViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            NavigateCommand = new RelayCommand(Navigate);
        }
       /// <summary>
       /// for now text changing and navigating to enterpin screen 
       /// </summary>
       /// <param name="obj"></param>
        private void Navigate(object obj)
        {
            var updateTextBlock = obj as TextBlock;
            //updateTextBlock.Text = System.Windows.Application.Current.FindResource("DecryptingData");
            //else
            //    _account.NavigateToScreen(DefaultProperties.EnterPinViewPath);
        }
        
        /// <summary>
        /// For Closing login window
        /// </summary>
        private void CloseWindow(object sender)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            _systemTray.WindowClose(window);
        }
    }
}
