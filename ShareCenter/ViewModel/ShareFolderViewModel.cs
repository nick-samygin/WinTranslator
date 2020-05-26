using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    /// <summary>
    /// Represents share folder
    /// </summary>
    internal class ShareFolderViewModel : ViewModelBase
    {
        #region properties
        private string _folderId;
        /// <summary>
        /// Folder id.
        /// </summary>
        public string FolderId
        {
            get { return _folderId; }
            set
            {
                _folderId = value;
                RaisePropertyChanged("FolderId");
            }
        }

        private string _name;
        /// <summary>
        /// Folder name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _rootFolder;
        /// <summary>
        /// Reference to root folder.
        /// </summary>
        public string RootFolder
        {
            get { return _rootFolder; }
            set
            {
                _rootFolder = value;
                RaisePropertyChanged("RootFolder");
            }
        }

        private List<object> _subItems;
        /// <summary>
        /// Content of the folder. It may has ISecureItemVM or ShareFolderViewModel entities.
        /// </summary>
        public List<object> SubItems
        {
            get { return _subItems; }
            set
            {
                _subItems = value;
                RaisePropertyChanged("SubItems");
                RaisePropertyChanged("Count");
            }
        }

        /// <summary>
        /// Total count of inner items.
        /// </summary>
        public int Count
        {
            get { return RecalcCount(SubItems); }
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
        #endregion

        public ShareFolderViewModel(string id, string name, IEnumerable<object> items, bool isExpanded,
            string rootFolder = null)
        {
            FolderId = id;
            Name = name;
            InitializeSubItems(items);
            RootFolder = rootFolder;
            IsExpanded = isExpanded;
        }

        #region methods
        private void InitializeSubItems(IEnumerable<object> items)
        {
            SubItems = new List<object>();

            if (items != null)
            {
                var subItems = items.ToList();
                foreach (var subItem in subItems)
                    AddSubItem(subItem);
            }

            RaisePropertyChanged("SubItems");
        }

        public void AddSubItem(object subItem)
        {
            //it's sub-item
            if (subItem is ISecureItemVM)
                InsertSecureItem(subItem as SecureItemViewModel);
            //it's sub-folder
            else
                SubItems.Add(subItem);

            RaisePropertyChanged("SubItems");
        }

        private void InsertSecureItem(SecureItemViewModel item)
        {
            if (item == null)
                throw new ArgumentException("Folder's sub-item can't be null");

            if (item is SecureItemWithPasswordViewModel || item is SSHKeySecureItemViewModel)
            {
                var passwordCategory = SubItems.OfType<FolderCategoryViewModel>().FirstOrDefault(c => c.Type == FolderCategoryType.Password);
                if (passwordCategory == null)
                {
                    passwordCategory = new FolderCategoryViewModel(FolderCategoryType.Password);
                    SubItems.Add(passwordCategory);
                }
                passwordCategory.AddItemToCategory(item);
            }
            else if (item is BankAccountItemViewModel || item is CreditCardItemViewModel)
            {
                var dwCategory = SubItems.OfType<FolderCategoryViewModel>().FirstOrDefault(c => c.Type == FolderCategoryType.DigitalWallet);
                if (dwCategory == null)
                {
                    dwCategory = new FolderCategoryViewModel(FolderCategoryType.DigitalWallet);
                    SubItems.Add(dwCategory);
                }
                dwCategory.AddItemToCategory(item);
            }
            else if (item is SecureItemWithCountryViewModel || item is CompanySecureItemViewModel ||
                     item is EmailSecureItemViewModel || item is NameSecureItemViewModel)
            {
                var personalInfoCategory = SubItems.OfType<FolderCategoryViewModel>().FirstOrDefault(c => c.Type == FolderCategoryType.PersonalInfo);
                if (personalInfoCategory == null)
                {
                    personalInfoCategory = new FolderCategoryViewModel(FolderCategoryType.PersonalInfo);
                    SubItems.Add(personalInfoCategory);
                }
                personalInfoCategory.AddItemToCategory(item);
            }
            else 
                throw new ArgumentException("Unknown item type");

            SubItems.Sort(Comparison);
            RaisePropertyChanged("SubItems");
        }

        private int Comparison(object o, object o1)
        {
            if (o is FolderCategoryViewModel && o1 is ShareFolderViewModel)
                return -1;
            if (o is ShareFolderViewModel && o1 is SecureItemViewModel)
                return 1;

            return 0;
        }

        private int RecalcCount(IEnumerable<object> subItems)
        {
            var count = 0;
            foreach (var subItem in subItems)
            {
                if (subItem is FolderCategoryViewModel)
                    count += (subItem as FolderCategoryViewModel).Count;
                else if(subItem is ShareFolderViewModel)
                    count += RecalcCount((subItem as ShareFolderViewModel).SubItems);
            }

            return count;
        }
        #endregion
    }
}
