using System.Windows;
using System.Windows.Media;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MainSearchBox.xaml
    /// </summary>
    public partial class MainSearchBox
    {
        public MainSearchBox()
        {
            InitializeComponent();
            BorderBrush = Application.Current.Resources["LightGrayTextForegroundColor"] as Brush;
        }
    }
}
