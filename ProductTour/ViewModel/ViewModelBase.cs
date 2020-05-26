using System.ComponentModel;
using System.Windows.Media;

namespace PasswordBoss.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        #region common global properties

        private bool _showHideImageVisibility = false;
        public bool ShowHideImageVisibility
        {
            get { return _showHideImageVisibility; }
            set
            {
                _showHideImageVisibility = value;
                RaisePropertyChanged("ShowHideImageVisibility");
            }
        }

        private bool _globalPasswordTextBoxVisibility = true;
        public bool GlobalPasswordTextBoxVisibility
        {
            get { return _globalPasswordTextBoxVisibility; }
            set
            {
                _globalPasswordTextBoxVisibility = value;
                RaisePropertyChanged("GlobalPasswordTextBoxVisibility");
            }
        }

        private bool _showTextBoxVisibility;
        public bool ShowTextBoxVisibility
        {
            get { return _showTextBoxVisibility; }
            set
            {
                _showTextBoxVisibility = value;
                RaisePropertyChanged("ShowTextBoxVisibility");
            }
        }

        private ImageSource _showHideImageIcon;
        public ImageSource ShowHideImageIcon
        {
            get { return _showHideImageIcon; }
            set
            {
                if (Equals(_showHideImageIcon, value)) return;
                _showHideImageIcon = value;
                RaisePropertyChanged("ShowHideImageIcon");
            }
        }

        #endregion
    }
}