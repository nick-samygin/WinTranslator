using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SecureItemsCommon.Helpers
{ 

    public class MenuItemStyleSelector: StyleSelector
    {
        public Style SubItemStyle{ get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ISubContextAction)
                return SubItemStyle;

            return null;
        }
    }
}
