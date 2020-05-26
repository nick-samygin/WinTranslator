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
    /// Interaction logic for TopPanelTextBox.xaml
    /// </summary>
    public partial class TopPanelTextBox : UserControl
    {
        public TopPanelTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitelProperty =
        DependencyProperty.Register(
        "Titel",
        typeof(string),
        typeof(TopPanelTextBox), null);

        public string Titel
        {
            get { return (string)GetValue(TitelProperty); }
            set { SetValue(TitelProperty, value); }
        }

    }
}
