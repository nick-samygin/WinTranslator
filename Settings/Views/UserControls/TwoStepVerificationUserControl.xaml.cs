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

namespace Settings.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TwoStepVerificationUserControl.xaml
    /// </summary>
    public partial class TwoStepVerificationUserControl : Window
    {
        public TwoStepVerificationUserControl()
        {
            InitializeComponent();
        }

        private void pConfirmationValueWatermark_GotFocus(object sender, RoutedEventArgs e)
        {
            pConfirmationValueWatermark.Visibility = System.Windows.Visibility.Collapsed;
            pConfirmationValue.Visibility = System.Windows.Visibility.Visible;
            pConfirmationValue.Focus();
        }

        private void pConfirmationValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.pConfirmationValue.Text))
            {
                pConfirmationValue.Visibility = System.Windows.Visibility.Collapsed;
                pConfirmationValueWatermark.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
