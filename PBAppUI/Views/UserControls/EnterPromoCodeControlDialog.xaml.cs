using System.Windows;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EnterPromoCodeControlDialog.xaml
    /// </summary>
    public partial class EnterPromoCodeControlDialog : Window
    {
        public EnterPromoCodeControlDialog(Window owner)
        {
            this.Owner = owner;
            this.Height = owner.ActualHeight;
            this.Width = owner.ActualWidth;
            this.Left = owner.Left;
            this.Top = owner.Top;
            this.WindowStartupLocation = owner.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
            InitializeComponent();
        }

        private void pConfirmationValue_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            pConfirmationValue.Focus();
        }
    }
}
