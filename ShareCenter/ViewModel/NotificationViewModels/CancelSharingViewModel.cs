using System.Windows;

namespace PasswordBoss.ViewModel.NotificationViewModels
{
    class CancelSharingViewModel : BaseNototificationViewModel
    {
        private string _question;
        public string Question
        {
            get { return _question;}
            set
            {
                _question = value;
                RaisePropertyChanged("Question");
            }
        }

        public CancelSharingViewModel()
        {
            Caption = Application.Current.Resources["CancelSharing"] as string;
            CancelCaption = Application.Current.Resources["No"] as string;
            OkCaption = Application.Current.Resources["Yes"] as string;
            Question = Application.Current.Resources["CancelSharingQuestion"] as string;
        }

        public override BaseNototificationViewModel GetCopy()
        {
            return new CancelSharingViewModel();
        }
    }
}
