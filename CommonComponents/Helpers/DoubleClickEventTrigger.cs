using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PasswordBoss.Helpers
{
    public class DoubleClickEventTrigger : EventTrigger
    {
        protected override void OnEvent(EventArgs eventArgs)
        {
            var e = eventArgs as MouseButtonEventArgs;
            if (e == null)
            {
                return;
            }
            if (e.ClickCount == 2)
            {
                base.OnEvent(eventArgs);
            }
        }
    }
}
