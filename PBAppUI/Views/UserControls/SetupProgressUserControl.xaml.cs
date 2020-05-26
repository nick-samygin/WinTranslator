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
using PasswordBoss.ViewModel;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SetupProgressUserControl.xaml
    /// </summary>
    public partial class SetupProgressUserControl : UserControl
    {
        public SetupProgressUserControl()
        {
            InitializeComponent();
            this.DataContext = new SetupProgressViewModel();
        }
    }
}
