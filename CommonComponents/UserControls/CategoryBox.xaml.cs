using PasswordBoss.DTO;
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

namespace PasswordBoss
{
    /// <summary>
    /// Interaction logic for CategoryBox.xaml
    /// </summary>
    public partial class CategoryBox : Window
    {
        public string NewCategory { get; set; }
        public bool UseSecureBrowser { get; set; }

        public CategoryBox()
        {
            SetMainWindowAsParent();
            InitializeComponent();
        }

        public CategoryBox(Folder category)
        {
            SetMainWindowAsParent();
            InitializeComponent();
            txtGroup.Text = category.Name;
            //cbUseSecureBrowser.IsChecked = category.UseSecureBrowser;
        }

        private void SetMainWindowAsParent()
        {
            Window parentWindow = null;

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType().Name == "MainWindow")
                {
                    parentWindow = window;
                }
            }

            this.Owner = parentWindow;
        }
       
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtGroup.Text))
            {
                NewCategory = txtGroup.Text.Trim();
                this.DialogResult = true;
                //if (cbUseSecureBrowser.IsChecked.HasValue)
                //{
                //    UseSecureBrowser = cbUseSecureBrowser.IsChecked.Value;
                //}
                //else
                //    UseSecureBrowser = false;
                this.Close();
            }
            else
            {
                //todo message error empty 
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            txtGroup.Text = String.Empty;
            this.Close();
        }

        private void txtGroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtGroup.Text))
            {
                ButtonOk.IsEnabled = true;
            }
            else
            {
                ButtonOk.IsEnabled = false;
            }
        }
    }
}
