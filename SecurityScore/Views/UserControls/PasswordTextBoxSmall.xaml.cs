using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for PasswordTextBoxSmall.xaml
    /// </summary>
    public partial class PasswordTextBoxSmall : UserControl
    {

        public static readonly DependencyProperty PasswordProperty =
           DependencyProperty.Register("Password", typeof(string), typeof(PasswordTextBoxSmall), new PropertyMetadata(onPasswordCallback));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        static void onPasswordCallback(
        DependencyObject dobj,
        DependencyPropertyChangedEventArgs args)
        {
            var control = dobj as PasswordTextBoxSmall;
            if (control != null)
            {
                // control.txtPasswordbox.Password = args.NewValue.ToString();
                if (control.Password.Length > 0)
                    control.ImgShowHide.Visibility = Visibility.Visible;
                else
                    control.ImgShowHide.Visibility = Visibility.Hidden;

                control.UpdatePasswordMeter(control.Password);
            }
        }

        public PasswordTextBoxSmall()
        {
            InitializeComponent();
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword = !ShowPassword;
        }
        private bool _showPassword;
        public bool ShowPassword
        {
            get { return _showPassword; }
            set
            {
                if (_showPassword != value)
                {
                    _showPassword = value;
                    if (_showPassword)
                        ShowPasswordComand();
                    else
                        HidePasswordComand();
                }
            }
        }
        private void ShowPasswordComand()
        {
            ImgShowHide.Source = Application.Current.Resources["imgLoginHideEye"] as ImageSource;
            txtVisiblePasswordbox.Visibility = Visibility.Visible;
            txtPasswordbox.Visibility = Visibility.Hidden;
            txtVisiblePasswordbox.Text = txtPasswordbox.Password;
        }

        private void HidePasswordComand()
        {
            ImgShowHide.Source = Application.Current.Resources["imgLoginShowEye"] as ImageSource;
            txtVisiblePasswordbox.Visibility = Visibility.Hidden;
            txtPasswordbox.Visibility = Visibility.Visible;
            txtPasswordbox.Focus();
        }


        void UpdatePasswordMeter(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                progressBar.Value = 0;
                return;
            }

            PasswordScanner scanner = new PasswordScanner();
            PasswordBoss.PasswordScanner.Strength s = (PasswordBoss.PasswordScanner.Strength)scanner.scanPassword(password);
            switch (s)
            {
                case (PasswordBoss.PasswordScanner.Strength.VERYWEAK):
                    progressBar.Value = 10;
                    progressBar.Foreground = Application.Current.Resources["passwordWeakColor"] as Brush;
                    break;
                case (PasswordBoss.PasswordScanner.Strength.WEAK):
                    progressBar.Value = 30;
                    progressBar.Foreground = Application.Current.Resources["passwordNormalColor"] as Brush;
                    break;
                case (PasswordBoss.PasswordScanner.Strength.GOOD):
                    progressBar.Value = 50;
                    progressBar.Foreground = Application.Current.Resources["passwordMediumColor"] as Brush;
                    break;
                case (PasswordBoss.PasswordScanner.Strength.STRONG):
                    progressBar.Value = 75;
                    progressBar.Foreground = Application.Current.Resources["passwordStrongColor"] as Brush;
                    break;
                case (PasswordBoss.PasswordScanner.Strength.VERY_STRONG):
                    progressBar.Value = 100;
                    progressBar.Foreground = Application.Current.Resources["passwordVeryStrongColor"] as Brush;
                    break;
                default:
                    progressBar.Value = 0;
                    break;
            }
        }
    }
}
