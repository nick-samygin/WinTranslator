using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MasterPasswordDialog.xaml
    /// </summary>
    public partial class MasterPasswordDialog : Window, INotifyPropertyChanged
    {
        private PasswordBox passwordBoxControl;
        private TextBox textBoxControl;
        private ToggleButton showHideImage;

        private bool _pinVisibility;

        public bool PwdVisibility
        {
            get { return !_pinVisibility; }
        }

        public bool PinVisibility
        {
            get { return _pinVisibility; }
        }

        public bool UpdatePwdTextVisibility
        {
            set
            {
                if (value)
                {
                    tbEnterMasterPassword.Visibility = System.Windows.Visibility.Collapsed;
                    tbUpdateMasterPassword.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    tbEnterMasterPassword.Visibility = System.Windows.Visibility.Visible;
                    tbUpdateMasterPassword.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public bool SkipPwdValidation { get; set; }

        public string Pwd { get { return passwordBoxControl.Password; } }

        private int? _Pin;
        public int? Pin
        {
            get { return _Pin; }
            set
            {
                _Pin = value;

                string dots = string.Empty;

                if (value.HasValue)
                {
                    for (int i = 0; i < value.ToString().Length; i++)
                    {
                        dots += "*";
                    }
                }

                PinPwdDots = dots;

                RaisePropertyChanged("Pin");
            }
        }

        private string _pinPwdDots;

        public string PinPwdDots
        {
            get { return _pinPwdDots; }
            set
            {
                _pinPwdDots = value;
                RaisePropertyChanged("PinPwdDots");
            }
        }


        public bool AlwaysAllow
        {
            get
            {
                //return CbAlwaysAllow.IsChecked.Value;
                return false;
            }
        }
        IPBData pbData = null;

        public MasterPasswordDialog(IPBData pbData)
        {
            this.pbData = pbData;
            this._pinVisibility = pbData.PinEnabled();
            this.DataContext = this;
//            this.Owner = System.Windows.Application.Current.MainWindow;
            if(passwordBoxControl != null)
            {
                passwordBoxControl.Focus();
            }
            InitializeComponent();
        }

        public void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if(SkipPwdValidation)
            {
                this.DialogResult = true;
                return;
            }

            if(passwordBoxControl != null)
            {
                if(pbData.CheckMasterPassword(passwordBoxControl.Password))
                {
                    this.DialogResult = true;
                }
                else
                {
                    ErrorTextBox.Text = System.Windows.Application.Current.FindResource("MasterPasswordErrorMessage") as string;
                }
                //if (pbData.AuthenticateUser(pbData.ActiveUser, PwdBox.Password, out exist))
                //{
                //    this.DialogResult = true;
                //}
            
            }

            //Old code
            //var pass = PwdBox.Password;
            //if(pbData.PinEnabled())
            //{
            //    pass = pbData.GetMasterPwdFromPin(Pin.ToString());
            //}

            //Pin = null;

            //if (pbData.CheckMasterPassword(pass))
            //{
            //    this.DialogResult = true;
            //}
            //else
            //{
            //    //TODO 
            //}
            //if (pbData.AuthenticateUser(pbData.ActiveUser, PwdBox.Password, out exist))
            //    this.DialogResult = true;
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }
        private void GlobalPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(passwordBoxControl != null && textBoxControl != null)
            {
                textBoxControl.Text = passwordBoxControl.Password;
                ErrorTextBox.Text = string.Empty;
                if(textBoxControl.Text != string.Empty)
                {
                    showHideImage.Visibility = Visibility.Visible;
                }
                else
                {
                    showHideImage.Visibility = Visibility.Hidden;
                }
            }
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordContentControl.ApplyTemplate();
            passwordBoxControl = PasswordContentControl.Template.FindName("GlobalPasswordTextBox", PasswordContentControl) as PasswordBox;
            textBoxControl = PasswordContentControl.Template.FindName("ShowTextBox", PasswordContentControl) as TextBox;
            showHideImage = PasswordContentControl.Template.FindName("ShowPasswordCharsCheckBox", PasswordContentControl) as ToggleButton;
            if (passwordBoxControl != null)
            {
                passwordBoxControl.Focus();
            }
        }
    }
}
