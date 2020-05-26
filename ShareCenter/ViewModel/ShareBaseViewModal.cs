using System.Collections.Generic;
using System.Linq;

namespace PasswordBoss.ViewModel
{
    class ShareBaseViewModal : ViewModelBase
    {
        #region properties
        private string _shareId;
        /// <summary>
        /// Share may has Id.
        /// </summary>
        public string ShareId
        {
            get { return _shareId; }
            set
            {
                _shareId = value;
                RaisePropertyChanged("ShareId");
            }
        }

        private string _shareName;
        /// <summary>
        /// Friendly share name.
        /// </summary>
        public string ShareName
        {
            get { return _shareName; }
            set
            {
                _shareName = value;
                RaisePropertyChanged("ShareName");
            }
        }

        private bool _isPending;
        /// <summary>
        /// Is this share pending.
        /// </summary>
        public bool IsPending
        {
            get { return _isPending; }
            set
            {
                _isPending = value;
                RaisePropertyChanged("IsPending");
            }
        }

        private List<object> _items;
        /// <summary>
        /// Reference to subitems. It maybe ShareFolderViewModel, or ISecureItemVM
        /// </summary>
        public List<object> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
                RaisePropertyChanged("Count");
            }
        }

        private bool _isExpanded;
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

        /// <summary>
        /// Total count of inner items.
        /// </summary>
        public int Count
        {
            get
            {
                if (Items != null)
                    return Items.Count;
                return 0;
            }
        }
        #endregion

        public ShareBaseViewModal(string id, string name, bool isPending, IEnumerable<object> items, bool isExpanded)
        {
            ShareId = id;
            ShareName = name;
            IsPending = isPending;
            Items = items != null ? items.ToList() : new List<object>();
            IsExpanded = isExpanded;
        }

        #region methods
        public void AddItem(object item)
        {
            Items.Add(item);
            RaisePropertyChanged("Items");
        }
        #endregion
    }
}
