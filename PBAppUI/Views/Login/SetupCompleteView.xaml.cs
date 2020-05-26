
using PasswordBoss.ViewModel.Account;

namespace PasswordBoss.Views.Login
{
    public partial class SetupCompleteView
    {
        public SetupCompleteView(SetupCompleteViewModel dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
			dataContext.OnDialogCloseRequired += (o, e) => this.Close();
        }
    }
}