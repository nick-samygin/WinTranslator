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
    /// Interaction logic for PersonalInfoMenuButton.xaml
    /// </summary>

    public partial class PersonalInfoMenuButton : UserControl
        {
            private bool selected;
            public PersonalInfoMenuButton()
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
                    btnPersonalInfo.IsChecked = value ? true : false;                   
                    if (selected)
                    {                       
                        var dictionary = new Dictionary<string, object> { { "ShowOrHide", false } };
                        ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowMenuExpander", dictionary);
                    }                   
                }
            }
        }
}
