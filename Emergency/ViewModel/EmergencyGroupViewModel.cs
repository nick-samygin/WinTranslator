using System;
using System.Collections.Generic;
using System.Windows;
using SecureItemsCommon.ViewModels;

namespace Emergency.ViewModel
{
    class EmergencyGroupViewModel : ViewModelBase
    {
        #region fields
        private string _name;
        private List<SecureItemViewModel> _items;
        private EmergencyCategoryType _type;
        private int _count;
        #endregion

        #region properties
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                RaisePropertyChanged("Count");
            }
        }

        public EmergencyCategoryType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public List<SecureItemViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                Count = _items.Count;
                RaisePropertyChanged("Items");
            }
        }
        #endregion

        public EmergencyGroupViewModel(EmergencyCategoryType type)
        {
            Type = type;
            switch (Type)
            {
                case EmergencyCategoryType.Password:
                    Name = Application.Current.Resources["NavPasswords"].ToString();
                    break;
                case EmergencyCategoryType.DigitalWallet:
                    Name = Application.Current.Resources["NavDigitalWallet"].ToString();
                    break;
                case EmergencyCategoryType.PersonalInfo:
                    Name = Application.Current.Resources["NavPersonalInfo"].ToString();
                    break;
                case EmergencyCategoryType.SecureNotes:
                    Name = Application.Current.Resources["NavSecureNotes"].ToString();
                    break;
            }
            Items = new List<SecureItemViewModel>();
        }

        public void AddSubItems(SecureItemViewModel subItem)
        {
            Items.Add(subItem);
            Count = Items.Count;
            RaisePropertyChanged("Items");
        }
    }
}
