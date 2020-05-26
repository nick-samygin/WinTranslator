using System;
using System.Collections.Generic;
using System.Globalization;
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
using PasswordBoss.ViewModel;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            InitializeComponent();
        }

        //Hack - to refresh Menu - callback into MenuViewModel 
         private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var vm = (MenuViewModel)this.DataContext;
            vm.RefreshBrowserMenu(null);
        }
    }
}
