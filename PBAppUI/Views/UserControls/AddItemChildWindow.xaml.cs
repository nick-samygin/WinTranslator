using PasswordBoss.UserControls;
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
using System.Windows.Shapes;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddItemChildWindow.xaml
    /// </summary>
    /// 
  
    public partial class AddItemChildWindow : CustomChildWindow
    {
        private AddItem _selectedItemType;
        public AddItem SelectedItemType
        {
            get
            {
                return _selectedItemType;
            }
            set
            {
                _selectedItemType = value;
                InitSelectedItemType();
            }
        }

        private AddSecureSubItem _selectedSubItemType;
        public AddSecureSubItem SelectedSubItemType
        {
            get
            {
                return _selectedSubItemType;
            }
            set
            {
                _selectedSubItemType = value;
            }
        }


        public IEnumerable<AddItem> ItemsList { get; set; }


        public AddItemChildWindow()
        {
            InitializeComponent();

        }

        public AddItemChildWindow(IEnumerable<AddItem> itemsList, AddItem selectedScreenType)
        {
            InitializeComponent();

            ShowButtonsPanel = Visibility.Collapsed;

            ItemsList = itemsList;
            SelectedItemType = selectedScreenType;      
        }

        private void InitSelectedItemType()
        {

            if (SelectedItemType != null && SelectedItemType is AddSecureItem)
                ShowChildTypesList(); 
            else
                ShowTypesList();

        }


        private void ShowTypesList()
        {
            txtTitel.Text = "What type of item do you " + Environment.NewLine + "want to add?";
            txtSubTitel.Visibility = Visibility.Collapsed;
            ShowTitelBorder = Visibility.Collapsed;
            typesList.ItemsSource = ItemsList;
            typesList.SelectedItem = null;
            typesList.Visibility = Visibility;
            childTypesList.Visibility = Visibility.Collapsed;
        }

        private void ShowChildTypesList()
        {
            var addSecureItem = SelectedItemType as AddSecureItem;
            if (addSecureItem == null)
                return; 

            childTypesList.ItemsPanel = addSecureItem.SubTypesList.Count> 9 ? this.Resources["FourColumnsItems"] as ItemsPanelTemplate : this.Resources["ThreeColumnsItems"] as ItemsPanelTemplate;

            txtTitel.Text = addSecureItem.AddTitel;
            txtSubTitel.Text = addSecureItem.AddSubTitel;

            txtSubTitel.Visibility = Visibility.Visible;
            ShowTitelBorder = Visibility.Visible;
            childTypesList.ItemsSource = addSecureItem.SubTypesList;
            childTypesList.SelectedItem = null;
            typesList.Visibility = Visibility.Collapsed;
            childTypesList.Visibility = Visibility.Visible;
        }

        private void typesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typesList.SelectedItem != null)
            {
                var item = typesList.SelectedItem as AddItem;
                if (item != null)
                {
                    SelectedItemType = item;
                    if (!(SelectedItemType is AddSecureItem))
                    {
                        DialogResult = true;
                        Close();
                    }
                }
                
            }
        }

        private void childTypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (childTypesList.SelectedItem == null)
                return;

            var item = childTypesList.SelectedItem as AddSecureSubItem;
            if (item != null)
            {
                SelectedSubItemType = item;

                if (string.IsNullOrEmpty(item.ItemType))
                    SelectedItemType = null;
                else
                {
                    DialogResult = true;
                    Close();
                }
            }
        }
    }
}