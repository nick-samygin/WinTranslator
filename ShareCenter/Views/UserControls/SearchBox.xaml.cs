using System.Windows;
using System.Windows.Media;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox
    {
        public SearchBox()
        {
            InitializeComponent();
            BorderBrush = Application.Current.Resources["LightGrayTextForegroundColor"] as Brush;
        }
    }
}
