using PasswordBoss.ViewModel;
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
    /// Interaction logic for SearchBoxWithMagnifier.xaml
    /// </summary>
    public partial class SearchBoxWithMagnifier : UserControl
    {
        public SearchBoxWithMagnifier()
        {
            InitializeComponent();
        }

        private void txtSearchBoxWatermarked_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearchBoxWatermarked.Visibility = System.Windows.Visibility.Collapsed;
            txtSearchBox.Visibility = System.Windows.Visibility.Visible;
            txtSearchBox.Focus();
        }

        private void txtSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSearchBox.Text))
            {
                txtSearchBox.Visibility = System.Windows.Visibility.Collapsed;
                txtSearchBoxWatermarked.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var model = this.DataContext as TabItemSearchBar;
                if (model != null)
                {
                    model.SecureSearchClick(null);
                }
            }
        }
    }
}
