using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AddShareWindow.xaml
    /// </summary>
    public partial class AddShareWindow
    {
        public AddShareWindow()
        {
            InitializeComponent();
            var cancelButton = GetTemplateChild("btnCancel") as Button;
            if (cancelButton != null)
                cancelButton.Content = Application.Current.Resources["Cancel"].ToString();

            OkButtonContent = Application.Current.Resources["Next"].ToString();
        }
    }
}
