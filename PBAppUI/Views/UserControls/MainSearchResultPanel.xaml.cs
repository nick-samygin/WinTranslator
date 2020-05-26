using PasswordBoss.ViewModel.Search;
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
using Telerik.Windows.Controls;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MainSearchResultPanel.xaml
    /// </summary>
    public partial class MainSearchResultPanel : UserControl
    {
        public MainSearchResultPanel()
        {
            InitializeComponent();
        }

        void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            DependencyObject obj = e.OriginalSource as DependencyObject;
            var listBoxItem = GetDependencyObjectFromVisualTree(obj, typeof(ListBoxItem)) as ListBoxItem;
            if (listBoxItem == null)
                return;

            RadContextMenu menu = new RadContextMenu();
            menu.ItemContainerStyleSelector = this.Resources["MenuItemStyleSelector"] as StyleSelector;
            var secureItemsView = listBoxItem.DataContext as ISecureItemVM;
            var viewModel = DataContext as SearchViewModel;
            if (secureItemsView != null && viewModel!=null)
            {
                
                listBoxItem.IsSelected = true;

                if (viewModel.SelectedItems != null && viewModel.SelectedItems.Any())
                   menu.ItemsSource = viewModel.GetActions();

            }
            if (menu.ItemsSource != null)
                RadContextMenu.SetContextMenu(listBoxItem, menu);

        }

        private static DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            var parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

    }
}
