using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PasswordVaultItems.xaml
    /// </summary>
    public partial class PersonalInfoItems : UserControl
    {
        public PersonalInfoItems()
        {
            InitializeComponent();
            //listView.View = (ViewBase)(listView.TryFindResource("SecureItemIconView"));
            //listView.View = (ViewBase)(listView.TryFindResource("SecureItemGridView"));
            //listView.Style = (Style)(listView.TryFindResource("SecureItemListViewWrapStyle"));
            //listView.ItemTemplate = (DataTemplate)(listView.TryFindResource("SecureItemIconViewTemplate"));

            
            //this.AddHandler(RadioButton.CheckedEvent, (RoutedEventHandler)delegate(object sender, RoutedEventArgs args)
            //{
            //    string view = (((RadioButton)args.OriginalSource).Content as string);
            //    listView.View = (ViewBase)(listView.TryFindResource(view));
            //    listView.ItemTemplate = (DataTemplate)(listView.TryFindResource(view+"Template"));
            //});
        }
    }
}
