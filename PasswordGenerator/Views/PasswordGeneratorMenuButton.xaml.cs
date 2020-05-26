using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for PasswordGeneratorMenuButton.xaml
    /// </summary>
    public partial class PasswordGeneratorMenuButton : UserControl
    {
        private bool selected;
        public PasswordGeneratorMenuButton()
        {
            selected = false;
            InitializeComponent();
        }

        public event Action<object, RoutedEventArgs> Click;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Click != null) Click(sender, e);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if(selected) return;
            imgPasswordGenerator.Source = (ImageSource)Application.Current.FindResource("imgPasswordGeneratorIcon");
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if(selected) return;
            imgPasswordGenerator.Source = (ImageSource)Application.Current.FindResource("imgPasswordGeneratorIconHover");
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                btnPasswordGenerator.IsChecked = value ? true : false;
                imgPasswordGenerator.Source = (ImageSource)Application.Current.FindResource((value ? "imgPasswordGeneratorIconHover" : "imgPasswordGeneratorIcon"));
                if (selected)
                {
                    var dictionary = new Dictionary<string, object> { { "ShowOrHide", false } };
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowMenuExpander", dictionary);
                }
            }
        }

    }
}
