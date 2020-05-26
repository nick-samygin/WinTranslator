using System.Collections.Generic;
using System.Linq;
using PasswordBoss.Helpers;
using SecureItemsCommon.ViewModels;

namespace Emergency.ViewModel
{
    class EmergencyContactViewModel : ViewModelBase
    {
        #region fields
        private string _email;
        private AccessPeriodType _accessPeriodType;
        private int _periodTime;
        private AccessType _accessType;
        private List<EmergencyGroupViewModel> _items;
        private RelayCommand _changeCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _approveDenyCommand;
        private bool _isPending;
        private int _count;
        #endregion

        #region properties
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public AccessPeriodType AccessPeriodType
        {
            get { return _accessPeriodType; }
            set
            {
                _accessPeriodType = value;
                RaisePropertyChanged("AccessPeriodType");
            }
        }

        public int PeriodTime
        {
            get { return _periodTime; }
            set
            {
                _periodTime = value;
                RaisePropertyChanged("PeriodTime");
            }
        }

        public AccessType AccessType
        {
            get { return _accessType; }
            set
            {
                _accessType = value;
                RaisePropertyChanged("AccessType");
            }
        }

        public List<EmergencyGroupViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RecalcCount();
                RaisePropertyChanged("Items");
            }
        }
        
        public RelayCommand ChangeCommand
        {
            get { return _changeCommand; }
            set
            {
                _changeCommand = value;
                RaisePropertyChanged("ChangeCommand");
            }
        }

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand; }
            set
            {
                _deleteCommand = value;
                RaisePropertyChanged("DeleteCommand");
            }
        }

        public RelayCommand ApproveDenyCommand
        {
            get { return _approveDenyCommand; }
            set
            {
                _approveDenyCommand = value;
                RaisePropertyChanged("ApproveDenyCommand");
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                RaisePropertyChanged("Count");
            }
        }

        public bool IsPending
        {
            get { return _isPending; }
            set
            {
                _isPending = value;
                RaisePropertyChanged("IsPending");
            }
        }
        #endregion

        public EmergencyContactViewModel(string email, AccessPeriodType accessPeriodType, int periodTime, AccessType accessType,
            IEnumerable<SecureItemViewModel> items)
        {
            Email = email;
            AccessPeriodType = accessPeriodType;
            PeriodTime = periodTime;
            AccessType = accessType;
            Items = new List<EmergencyGroupViewModel>();
            if (items != null)
            {
                var subItems = items.ToList();
                foreach (var subItem in subItems)
                    AddSubItem(subItem);
            }
            ChangeCommand = new RelayCommand(OnChangeCommandHandler);
            DeleteCommand = new RelayCommand(OnDeleteCommandHandler);
            ApproveDenyCommand = new RelayCommand(OnApproveDenyCommandHandler);
        }

        #region commands handlers
        private void OnApproveDenyCommandHandler(object o)
        {
        }

        private void OnDeleteCommandHandler(object o)
        {
        }

        private void OnChangeCommandHandler(object o)
        {
        }
        #endregion

        #region methods
        public void AddSubItem(SecureItemViewModel subItem)
        {
            if (subItem is WebsiteSecureItemViewModel)
            {
                var passwordGroup = Items.FirstOrDefault(c => c.Type == EmergencyCategoryType.Password);
                if (passwordGroup == null)
                {
                    passwordGroup = new EmergencyGroupViewModel(EmergencyCategoryType.Password);
                    Items.Add(passwordGroup);
                }
                passwordGroup.AddSubItems(subItem);
            }
            RecalcCount();
            RaisePropertyChanged("Items");
        }

        private void RecalcCount()
        {
            var count = 0;
            foreach (var item in _items)
                count += item.Count;
            Count = count;
        }
        #endregion
    }
}
