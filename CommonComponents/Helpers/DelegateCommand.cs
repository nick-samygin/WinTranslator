using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PasswordBoss.Helpers
{
    /// <summary>
    /// delegate command class to delegate command functions
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private Action _executeMethod;
        /// <summary>
        /// delegate command to assign command function
        /// </summary>
        /// <param name="executeMethod"></param>
        public DelegateCommand(Action executeMethod)
        {
            _executeMethod = executeMethod;
        }
        /// <summary>
        /// check function execution
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// executes the function
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _executeMethod.Invoke();
        }
    }
}
