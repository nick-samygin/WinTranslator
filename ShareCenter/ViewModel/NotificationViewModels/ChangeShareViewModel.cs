using System.Windows;

namespace PasswordBoss.ViewModel.NotificationViewModels
{
    public class ChangeShareViewModel : BaseNototificationViewModel
    {
        private string _isPasswordVisible;
        public string IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                _isPasswordVisible = value;
                RaisePropertyChanged("IsPasswordVisible");
            }
        }

        public ChangeShareViewModel()
        {
            Caption = Application.Current.Resources["ChangeShare"] as string;
            OkCaption = Application.Current.Resources["OK"] as string;
            CancelCaption = Application.Current.Resources["Cancel"] as string;
        }

        public ChangeShareViewModel(ChangeShareViewModel cpyObject) : base(cpyObject)
        {
            IsPasswordVisible = cpyObject.IsPasswordVisible;
        }

        public override BaseNototificationViewModel GetCopy()
        {
            return new ChangeShareViewModel(this);
        }
    }
}
