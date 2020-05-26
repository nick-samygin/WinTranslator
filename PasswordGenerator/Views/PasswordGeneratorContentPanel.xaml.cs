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
using PasswordBoss;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for PasswordGeneratorContentPanel.xaml
    /// </summary>
    public partial class PasswordGeneratorContentPanel : UserControl
    {
        private IInAppAnalytics inAppAnalyitics;
        private IResolver resolver;

        public enum Strength { VERYWEAK, WEAK, MEDIUM, STRONG, VERY_STRONG }

        public PasswordGeneratorContentPanel(IResolver resolver)
        {
            this.resolver = resolver;
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            InitializeComponent();
            this.DataContext = new PasswordGeneratorViewModel(resolver);
        }

    }
}
