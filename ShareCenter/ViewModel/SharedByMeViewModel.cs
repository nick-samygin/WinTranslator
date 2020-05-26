using System.Collections.Generic;
using System.Linq;

namespace PasswordBoss.ViewModel
{
    /// <summary>
    /// Represents each share entry in 'Shared by me' view.
    /// </summary>
    internal class SharedByMeViewModel : ShareBaseViewModal
    {
        #region fields
        private List<SharedWithViewModel> _sharesList;
        private string _message;
        private bool _isPasswordVisible;
        #endregion

        #region properties

        private bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                _isPasswordVisible = value;
                RaisePropertyChanged("IsPasswordVisible");
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// List of users with whom data will be shared.
        /// </summary>
        public List<SharedWithViewModel> SharesList
        {
            get { return _sharesList; }
            set
            {
                _sharesList = value;
                RaisePropertyChanged("SharesList");
            }
        }
        #endregion

        #region ctrs
        public SharedByMeViewModel(string id, string name, bool isPending, IEnumerable<object> items,
            IEnumerable<SharedWithViewModel> shares, bool isExpanded)
            :base(id, name, isPending, items, isExpanded)
        {
            SharesList = shares != null ? shares.ToList() : new List<SharedWithViewModel>();
        }
        #endregion
    }
}
