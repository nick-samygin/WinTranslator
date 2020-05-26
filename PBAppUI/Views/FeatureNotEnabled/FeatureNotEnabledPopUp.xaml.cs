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

namespace PasswordBoss.Views.FeatureNotEnabled
{
    /// <summary>
    /// Interaction logic for FeatureNotEnabledPopUp.xaml
    /// </summary>
    public partial class FeatureNotEnabledPopUp : Window
    {
        public FeatureNotEnabledPopUp(MainWindow parent)
        {
            this.Owner = parent;
            InitializeComponent();
        }

        private void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            Close();

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("BuySB", null);
                ///((MainWindow)Owner).BtnUpgrade.Focus();
            });
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
