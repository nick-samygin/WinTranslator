using PasswordBoss.Helpers;
using PasswordBoss.Views.UserControls;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel
{
    public class ViewModelPasswordBox : ViewModelBase
    {
        private Common _commonObj = new Common();

        #region password box common events

        /// <summary>
        /// applying default style to password box user control
        /// </summary>
        /// <param name="passwordBox"></param>
        public void ApplyDefaultStyle(PasswordTextBox passwordBox, string passwordStyle, string iconStyle)
        {
            passwordBox.GlobalPasswordTextBox.Style = Application.Current.Resources[passwordStyle] as Style;
            passwordBox.ShowHideImage.Style = Application.Current.Resources[iconStyle] as Style;
        }

        /// <summary>
        /// for handling events for control
        /// </summary>
        /// <param name="passwordTextBox"></param>
        public void AssignPasswordBoxEvents(PasswordTextBox element, string style, string loginShowEye, string loginHideEye, string source)
        {
            element.GlobalPasswordTextBox.PasswordChanged += (sender, e) => PasswordTextBoxPasswordChanged(element.GlobalPasswordTextBox, style);
            element.ShowTextBox.TextChanged += (sender, e) => ShowTextBoxTextChanged(element.ShowTextBox, element.GlobalPasswordTextBox);
            element.ShowPasswordCharsCheckBox.Checked += (sender, e) => ToggledCheckedClick(element.GlobalPasswordTextBox, element.ShowTextBox, loginShowEye);
            element.ShowPasswordCharsCheckBox.Unchecked += (sender, e) => ToggledUncheckedClick(element.GlobalPasswordTextBox, element.ShowTextBox, loginHideEye);
        }

        /// <summary>
        /// for handling events for control
        /// </summary>
        /// <param name="passwordTextBox"></param>
        public void AssignPasswordBoxEvents(PasswordTextBox element, string style, Button nextBtn, string loginShowEye, string loginHideEye, string source, string masterPassword)
        {
            element.GlobalPasswordTextBox.PasswordChanged += (sender, e) => PasswordTextBoxPasswordChanged(element, style, nextBtn, masterPassword);
            element.ShowTextBox.TextChanged += (sender, e) => ShowTextBoxTextChanged(element.ShowTextBox, element.GlobalPasswordTextBox);
            element.ShowPasswordCharsCheckBox.Checked += (sender, e) => ToggledCheckedClick(element.GlobalPasswordTextBox, element.ShowTextBox, loginShowEye);
            element.ShowPasswordCharsCheckBox.Unchecked += (sender, e) => ToggledUncheckedClick(element.GlobalPasswordTextBox, element.ShowTextBox, loginHideEye);
        }

        /// <summary>
        /// setting textbox text to password box
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="passwordBox"></param>
        public void ShowTextBoxTextChanged(TextBox textBox, PasswordBox passwordBox)
        {
            passwordBox.Password = textBox.Text;
        }

        /// <summary>
        /// password change event for changing place holder text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PasswordTextBoxPasswordChanged(PasswordBox sender, string style)
        {
            _commonObj.ElementTextChanged(sender, DefaultProperties.PasswordBoxOnFocusStyle, style);
            ShowHideEyeImage(sender);
        }

        /// <summary>
        /// password change event for changing place holder text
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        public void PasswordTextBoxPasswordChanged(PasswordTextBox element, string style, Button nextBtn, string masterPassword)
        {
            _commonObj.ElementTextChanged(element, DefaultProperties.PasswordBoxOnFocusStyle, style);
            ShowHideEyeImage(element.GlobalPasswordTextBox);

            string confirmPassword = element.GlobalPasswordTextBox.Password;
            if (masterPassword == confirmPassword)
            {
                nextBtn.IsEnabled = true;
            }
            else
            {
                nextBtn.IsEnabled = false;
            }
        }

        /// <summary>
        /// hide password on by disabling textbox visibility
        /// </summary>
        /// <param name="passwordBox"></param>
        /// <param name="textBox"></param>
        public void ToggledUncheckedClick(PasswordBox passwordBox, TextBox textBox, string loginHideEye)
        {
            ShowHideImageIcon = _commonObj.ReturnImageIcon(loginHideEye);
            GlobalPasswordTextBoxVisibility = true;
            ShowTextBoxVisibility = false;
            passwordBox.Password = textBox.Text;
            SetSelection(passwordBox, textBox.Text.Length, 0);
            passwordBox.Focus();
        }

        /// <summary>
        ///  show password on by enabling textbox visibility
        /// </summary>
        /// <param name="passwordBox"></param>
        /// <param name="textBox"></param>
        public void ToggledCheckedClick(PasswordBox passwordBox, TextBox textBox, string loginShowEye)
        {
            ShowHideImageIcon = _commonObj.ReturnImageIcon(loginShowEye);
            GlobalPasswordTextBoxVisibility = false;
            ShowTextBoxVisibility = true;
            textBox.Focus();
            textBox.Text = passwordBox.Password;
            textBox.CaretIndex = textBox.Text.Length;
        }

        //Method to set cursor index on password box control
        private void SetSelection(PasswordBox passwordBox, int start, int length)
        {
            passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
        }

        //Method that shows/hides image as text is entered
        public void ShowHideEyeImage(object element)
        {
            var masterPasswordBox = element as PasswordBox;
            if (masterPasswordBox != null && masterPasswordBox.Password.ToString().Length > 0)
            {
                if (ShowHideImageVisibility != true)
                {
                    ShowHideImageVisibility = true;
                }
            }
            else
            {
                ShowHideImageVisibility = false;
            }
        }

        #endregion
    }
}