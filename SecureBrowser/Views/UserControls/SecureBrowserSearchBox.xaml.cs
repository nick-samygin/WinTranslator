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
    /// Interaction logic for SecureBrowserSearchBox.xaml
    /// </summary>
    public partial class SecureBrowserSearchBox : UserControl
    {
        public SecureBrowserSearchBox()
        {
            InitializeComponent();
            txtAddressBar.LostFocus += txtAddressBar_LostFocus;
            //this.DataContext = ""
        }

        void txtAddressBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtAddressBar.SelectAll();
        }
        void txtAddressBar_LostFocus(object sender, RoutedEventArgs e)
        {
            //var model = this.DataContext as TabItemSearchBar;
            //if (model != null && model.Address != txtAddressBar.Text)
            //{
                //model.Address = txtAddressBar.Text;
                //model.Navigate();
            //}
        }

        private void txtAddressBar_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void txtAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
               || e.Key == Key.Tab)
            {
                var model = this.DataContext as TabItemSearchBar;
                
                if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.Enter))
                {
                    if (model != null)
                    {
                        string address = "";
                        if (!txtAddressBar.Text.StartsWith("www.", StringComparison.CurrentCultureIgnoreCase))
                        {
                            address = "www." + txtAddressBar.Text;
                        }
                        if (!txtAddressBar.Text.EndsWith(".com", StringComparison.CurrentCultureIgnoreCase))
                        {
                            address = address + ".com";
                        }
                        model.Address = address;
                        model.Navigate();
                    }
                }
                else if(e.Key == Key.Enter)
                {
                    if (model != null)
                    {
                        bool openSearchUrl = false;
                        var search = txtAddressBar.Text.Trim();
                        if (search.Contains(' '))
                        {
                            openSearchUrl = true;
                        }
                        else if (!search.Contains("://") && !search.Contains("."))
                        {
                            openSearchUrl = true;
                        }


                        if(openSearchUrl)
                        {
                            model.SecureSearchQuery = search;
                            model.SecureSearchClick(null);
                        }
                        else
                        {
                            if (model != null)
                            {
                                if(!string.IsNullOrWhiteSpace(search))
                                {
                                    model.Address = search;
                                    model.Navigate();
                                }
                                
                            }
                        }

                    }
                }
                else
                {
                    if (model != null)
                    {
                        model.Address = txtAddressBar.Text;
                        model.Navigate();
                    }
                }
            }
            
        }
        
    }
}
