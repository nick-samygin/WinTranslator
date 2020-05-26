using PasswordBoss;
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

namespace Identities.Views
{
    /// <summary>
    /// Interaction logic for IdentitiesMenuButton.xaml
    /// </summary>
    public partial class IdentitiesMenuButton : UserControl
    {
        private bool selected;

        public IdentitiesMenuButton()
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
                btnIdentities.IsChecked = value ? true : false;
                if (selected)
                {
                    var dictionary = new Dictionary<string, object> { { "ShowOrHide", false } };
                    ((IAppCommand)Application.Current).ExecuteCommand("ShowMenuExpander", dictionary);
                }
            }
        }
    }
}
