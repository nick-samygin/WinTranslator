using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.Helpers.TemplateSelectors
{
    public class DWSecureItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _bankAccountTemplate;

        public DataTemplate BankAccountTemplate
        {
            get
            {
                return _bankAccountTemplate;
            }
            set
            {
                _bankAccountTemplate = value;
            }
        }


        private DataTemplate _creditCardTemplate;

        public DataTemplate CreditCardTemplate
        {
            get
            {
                return _creditCardTemplate;
            }
            set
            {
                _creditCardTemplate = value;
            }
        }


      
        public DWSecureItemTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
           
            if (item is BankAccountItemViewModel)
                return BankAccountTemplate;

            if (item is CreditCardItemViewModel)
                return CreditCardTemplate;


            return BankAccountTemplate;

        }
    }
}
