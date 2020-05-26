using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for ShareCenterMenuButton.xaml
    /// </summary>
    public partial class ShareCenterMenuButton : UserControl
    {
        private bool selected;
        public ShareCenterMenuButton()
        {
            selected = false;
            InitializeComponent();
        }

        public event Action<object, RoutedEventArgs> Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null) Click(sender, e);
        }

       
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                btnShareCenter.IsChecked = value ? true : false;
                if(selected)
                {
                    var dictionary = new Dictionary<string, object> { { "ShowOrHide", false } };
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowMenuExpander", dictionary);
                }
                
            }
        }
    }
}
