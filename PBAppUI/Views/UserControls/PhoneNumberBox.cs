using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordBoss.Views.UserControls
{
    public class PhoneNumberBox : TextBox
    {
        #region Constructors
        /*
         * The default constructor
         */
        public PhoneNumberBox()
        {
            TextChanged += new TextChangedEventHandler(OnTextChanged);
            KeyDown += new KeyEventHandler(OnKeyDown);
        }
        #endregion

        #region Properties
        new public String Text
        {
            get { return base.Text; }
            set
            {
                base.Text = LeaveOnlyNumbers(value);
            }
        }

        #endregion

        #region Functions
        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAllowedBesideNumbersKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.OemPlus || inKey == Key.Add;
        }

        private string LeaveOnlyNumbers(String inString)
        {
            String tmp = inString;
            var cArray = inString.ToArray();
            for (int i = 0; i < cArray.Length; i++ )
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(cArray[i].ToString(), "^[0-9]*$") && cArray[i] != '+')
                {
                    tmp = tmp.Replace(cArray[i].ToString(), "");
                }
            }
               
            return tmp;
        }
        #endregion

        #region Event Functions
        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsAllowedBesideNumbersKey(e.Key);
        }

        protected void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //base.Text = LeaveOnlyNumbers(Text);
        }
        #endregion
    }
}
