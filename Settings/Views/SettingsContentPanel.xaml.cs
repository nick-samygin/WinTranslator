using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Settings.Views
{
    /// <summary>
    /// Interaction logic for SettingsContentPanel.xaml
    /// </summary>
    public partial class SettingsContentPanel :UserControl
    {
        public SettingsContentPanel(IResolver resolver)
        {
            InitializeComponent();
        }

        private void accountSettingsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            accountSettingsTab.Focusable = true;
            accountSettingsTab.Focus();
        }

        private void accountSettingsTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (accountSettingsTab.Visibility == System.Windows.Visibility.Visible)
            {
                accountSettingsTab.Focusable = true;
                accountSettingsTab.Focus();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cnt = (ComboBox)sender;
            cnt.Focus();

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                Keyboard.Focus(cnt);
            }));
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            accountsetting_grid.Focusable = true;
            accountsetting_grid.Focus();
        }
    }
}
