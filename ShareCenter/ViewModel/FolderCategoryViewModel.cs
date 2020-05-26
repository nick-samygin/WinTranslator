using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PasswordBoss.ViewModel
{
    class FolderCategoryViewModel : ViewModelBase
    {
        #region fields
        private string _categoryName;
        private List<ISecureItemVM> _items;
        private FolderCategoryType _type;
        #endregion

        #region properties
        public FolderCategoryType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                RaisePropertyChanged("CategoryName");
            }
        }

        public List<ISecureItemVM> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
                RaisePropertyChanged("Count");
            }
        }

        public int Count
        {
            get { return _items.Count; }
        } 
        #endregion

        public FolderCategoryViewModel(FolderCategoryType type)
        {
            Type = type;
            switch (type)
            {
                case FolderCategoryType.Password:
                    CategoryName = Application.Current.Resources["NavPasswords"].ToString();
                break;
                case FolderCategoryType.DigitalWallet:
                    CategoryName = Application.Current.Resources["NavDigitalWallet"].ToString();
                break;
                case FolderCategoryType.PersonalInfo:
                    CategoryName = Application.Current.Resources["NavPersonalInfo"].ToString();
                break;
                case FolderCategoryType.SecureNotes:
                    CategoryName = Application.Current.Resources["NavSecureNotes"].ToString();
                break;
            }
            Items = new List<ISecureItemVM>();
        }

        public FolderCategoryViewModel(string categoryName, IEnumerable<ISecureItemVM> items)
        {
            CategoryName = categoryName;
            Items = items.ToList();
        }

        public void AddItemToCategory(ISecureItemVM item)
        {
            Items.Add(item);
            RaisePropertyChanged("Items");
            RaisePropertyChanged("Count");
        }
    }
}
