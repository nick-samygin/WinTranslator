using System.Windows;
using PasswordBoss.UserControls;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for NewShareView.xaml
    /// </summary>
    public partial class NewShareView
    {
       public NewShareView()
        {
            InitializeComponent();
            CancelButtonContent = Application.Current.Resources["Back"].ToString();
            OkButtonContent = Application.Current.Resources["ShareItem"].ToString();
        }
    }
}
