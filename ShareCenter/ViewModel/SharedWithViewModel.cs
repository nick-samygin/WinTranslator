using System;
using PasswordBoss.Helpers;

namespace PasswordBoss.ViewModel
{
    /// <summary>
    /// Represents every 'shared with' entry.
    /// </summary>
    internal class SharedWithViewModel : ViewModelBase
    {
        #region properties
        private string _shareWithName;
        /// <summary>
        /// Name with whom user to share data.
        /// </summary>
        public string ShareWithName
        {
            get { return _shareWithName; }
            set
            {
                _shareWithName = value;
                RaisePropertyChanged("ShareWithName");
            }
        }

        private ShareWithStatus _status;
        /// <summary>
        /// Status of pending share invitations.
        /// </summary>
        public ShareWithStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        private bool _isExpanded = true;
        /// <summary>
        /// Is this folder expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
        #endregion

        #region commands
        /// <summary>
        /// Remove that user from the share.
        /// </summary>
        public RelayCommand ShareCenterCancelCommand { get; set; }
        /// <summary>
        /// Resend the share invitation.
        /// </summary>
        public RelayCommand ShareCenterResendCommand { get; set; }
        #endregion

        public SharedWithViewModel()
        {
            ShareCenterCancelCommand = new RelayCommand(OnShareCenterCancelCommandHandler);
            ShareCenterResendCommand = new RelayCommand(OnShareCenterResendCommandHandler);
        }

        public SharedWithViewModel(string name, ShareWithStatus status) : this()
        {
            ShareWithName = name;
            Status = status;
        }
        
        #region command's handlers
        private void OnShareCenterCancelCommandHandler(object o)
        {
            throw new NotImplementedException();
        }

        private void OnShareCenterResendCommandHandler(object o)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
