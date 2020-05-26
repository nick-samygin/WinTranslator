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

namespace PasswordBoss.UserControls
{
    /// <summary>
    /// Interaction logic for AddSecureItemWindow.xaml
    /// </summary>
    public partial class AddSecureItemWindow : CustomChildWindow
    {
               
        public AddSecureItemWindow()
        {
            InitializeComponent();

        }


        public AddSecureItemWindow(DataTemplateSelector contentTemplateSelector,bool IsEdit=false)
        {
            InitializeComponent();
            if (IsEdit)
            {
                Style = Application.Current.Resources["RightSideWindowStyle"] as Style;
                topDetailsGrid.Visibility = Visibility.Visible;

                Grid.SetRow(colorPicker, 2);
                Grid.SetColumn(colorPicker, 0);
            }
            else
                ScrollWidth = 545;
            contentControl.ContentTemplateSelector = contentTemplateSelector;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            notesTxt.BringIntoView();
            notesTxt.Focus();
            notesTxt.Select(0, 0);
            Keyboard.Focus(notesTxt);
        }
    }
}
