using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace SecureNotes.Views
{
    /// <summary>
    /// Interaction logic for SecureNotesMenuButton.xaml
    /// </summary>
    public partial class SecureNotesMenuButton : UserControl
    {

        private bool selected;
        public SecureNotesMenuButton()
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
                btnSecureNotes.IsChecked = value ? true : false;
                if (selected)
                {
                    var dictionary = new Dictionary<string, object> { { "ShowOrHide", false } };
                    ((IAppCommand)Application.Current).ExecuteCommand("ShowMenuExpander", dictionary);
                }
            }
        }
    }
}
