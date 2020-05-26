namespace PasswordBoss.ViewModel.NotificationViewModels
{
    public class BaseNototificationViewModel : ViewModelBase
    {
        #region fields
        private string _caption;
        private string _okCaption;
        private string _cancelCaption;
        #endregion

        #region properties
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                RaisePropertyChanged("Caption");
            }
        }

        public string OkCaption
        {
            get { return _okCaption; }
            set
            {
                _okCaption = value;
                RaisePropertyChanged("OkCaption");
            }
        }

        public string CancelCaption
        {
            get { return _cancelCaption; }
            set
            {
                _cancelCaption = value;
                RaisePropertyChanged("CancelCaption");
            }
        }
        #endregion

        public BaseNototificationViewModel()
        { }

        public BaseNototificationViewModel(BaseNototificationViewModel cpyObject)
        {
            Caption = cpyObject.Caption;
            OkCaption = cpyObject.OkCaption;
            CancelCaption = cpyObject.CancelCaption;
        }

        public virtual BaseNototificationViewModel GetCopy()
        {
            return new BaseNototificationViewModel(this);
        }
    }
}
