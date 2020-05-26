using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for Button2.xaml
    /// </summary>
    public partial class PasswordVaultMenuButton : UserControl
    {
        private bool selected;

        public PasswordVaultMenuButton()
        {
            selected = false;
            InitializeComponent();
           
        }

        
        public event Action<object, RoutedEventArgs> Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if(Click != null) Click(sender, e);
        }
       
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;

                btnPasswordVault.IsChecked = value ? true : false;                           
            }
        }



        
    }
}
