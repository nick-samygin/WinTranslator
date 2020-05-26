using PasswordBoss.ViewModel;
using PasswordBoss.ViewModel.DigitalWallet;
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


namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for DigitalWalletContentPanel.xaml
    /// </summary>
    public partial class DigitalWalletContentPanel : UserControl
    {
        public DigitalWalletContentPanel()
        {
            InitializeComponent();
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(DigitalWalletContentPanel_IsVisibleChanged);
        }

        private void DigitalWalletContentPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if (this.Visibility == Visibility.Visible)
            //{
            //    try
            //    {
            //        if ((this.DataContext as DigitalWalletViewModel).AddNewItemGridVisibility == true)
            //        {
            //            (this.DataContext as DigitalWalletViewModel).AddControlViewModel.InitializeAddressCollection();
            //        }
            //    }
            //    catch { }
            //}
        }

        
    }
}
