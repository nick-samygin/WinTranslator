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

namespace PasswordBoss.UserControls
{
    /// <summary>
    /// Interaction logic for RequiredTextBox.xaml
    /// </summary>
    public partial class RequiredTextBox : UserControl
    {
        public static readonly DependencyProperty TextProperty =
         DependencyProperty.Register("Text", typeof(string), typeof(RequiredTextBox), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty IsValidProperty =
         DependencyProperty.Register("IsValid", typeof(bool), typeof(RequiredTextBox), new PropertyMetadata(true));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }

        public RequiredTextBox()
        {
            InitializeComponent();
        }
    }
}
