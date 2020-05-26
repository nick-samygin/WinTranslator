using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using PasswordBoss;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using SecureItemsCommon.ViewModels;

namespace Emergency.ViewModel
{
    class EmergencyViewModel : ViewModelBase
    {
        #region fields
        private IResolver _resolver;
        private List<EmergencyContactViewModel> _myContacts;
        private List<EmergencyContactViewModel> _trustedContacts;
        private bool _isAnyMyPendingContacts;
        private bool _isAnyMyCurrentAccessContacts;
        private bool _isAnyMyEmergencyContacts;
        private RelayCommand _setupEmergencyCommand;
        private readonly IPBData _pbData;
        private bool _isAnyPendingTrustedContacts;
        private bool _isAnyAcceptedTrustedContacts;
        #endregion

        #region properties
        public bool IsAnyAcceptedTrustedContacts
        {
            get { return _isAnyAcceptedTrustedContacts; }
            set
            {
                _isAnyAcceptedTrustedContacts = value;
                RaisePropertyChanged("IsAnyAcceptedTrustedContacts");
            }
        }

        public bool IsAnyPendingTrustedContacts
        {
            get { return _isAnyPendingTrustedContacts; }
            set
            {
                _isAnyPendingTrustedContacts = value;
                RaisePropertyChanged("IsAnyPendingTrustedContacts");
            }
        }

        public List<EmergencyContactViewModel> TrustedContacts
        {
            get { return _trustedContacts; }
            set
            {
                _trustedContacts = value;
                IsAnyPendingTrustedContacts = _trustedContacts.Any(c => c.IsPending);
                IsAnyAcceptedTrustedContacts = _trustedContacts.Any(c => !c.IsPending);
                RaisePropertyChanged("TrustedContacts");
            }
        }

        public List<EmergencyContactViewModel> MyContacts
        {
            get { return _myContacts; }
            set
            {
                _myContacts = value;
                IsAnyMyCurrentAccessContacts = _myContacts.Any(c => c.AccessPeriodType == AccessPeriodType.FullAccess);
                IsAnyMyPendingContacts = _myContacts.Any(c => c.IsPending);
                IsAnyMyEmergencyContacts = _myContacts.Any(c => c.AccessPeriodType != AccessPeriodType.FullAccess && !c.IsPending);
                RaisePropertyChanged("MyContacts");
            }
        }

        public bool IsAnyMyPendingContacts
        {
            get { return _isAnyMyPendingContacts; }
            set
            {
                _isAnyMyPendingContacts = value;
                RaisePropertyChanged("IsAnyMyPendingContacts");
            }
        }

        public bool IsAnyMyCurrentAccessContacts
        {
            get { return _isAnyMyCurrentAccessContacts; }
            set
            {
                _isAnyMyCurrentAccessContacts = value;
                RaisePropertyChanged("IsAnyMyCurrentAccessContacts");
            }
        }

        public bool IsAnyMyEmergencyContacts
        {
            get { return _isAnyMyEmergencyContacts; }
            set
            {
                _isAnyMyEmergencyContacts = value;
                RaisePropertyChanged("IsAnyMyEmergencyContacts");
            }
        }

        public RelayCommand SetupEmergencyCommand
        {
            get { return _setupEmergencyCommand; }
            set
            {
                _setupEmergencyCommand = value;
                RaisePropertyChanged("SetupEmergencyCommand");
            }
        }
        #endregion

        public EmergencyViewModel(IResolver resolver)
        {
            _resolver = resolver;
            _pbData = resolver.GetInstanceOf<IPBData>();
            MyContacts = new List<EmergencyContactViewModel>();
            TrustedContacts = new List<EmergencyContactViewModel>();
            SetupEmergencyCommand = new RelayCommand(OnSetupEmergencyCommandHandler);
            InitExampleContacts();
        }

        #region commands handlers
        private void OnSetupEmergencyCommandHandler(object o)
        {
        }
        #endregion

        #region methods
        public IEnumerable<SecureItemViewModel> GetViewItems()
        {
            var watch = new Stopwatch();
            watch.Start();
            var passVaultItems = new List<SecureItemViewModel>();
            try
            {
                List<SecureItem> sites;
                if ((sites = _pbData.GetSecureItemsByItemType("PV")) != null)
                {
                    foreach (var site in sites)
                    {
                        if (site.Data == null)
                        {
                            continue;
                        }
                        if (site.Type == "Website")
                        {
                            var webSite = new WebsiteSecureItemViewModel(site, 
                                new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                                Application.Current.Resources["addWebsite"] as ImageSource);
                            passVaultItems.Add(webSite);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)Application.Current.FindResource("GeneralErrorText"));
            }
            watch.Stop();
            return passVaultItems;
        }

        public void AddMyContract(EmergencyContactViewModel contact)
        {
            MyContacts.Add(contact);
            IsAnyMyCurrentAccessContacts = MyContacts.Any(c => c.AccessPeriodType == AccessPeriodType.FullAccess);
            IsAnyMyPendingContacts = MyContacts.Any(c => c.IsPending);
            IsAnyMyEmergencyContacts = MyContacts.Any(c => c.AccessPeriodType != AccessPeriodType.FullAccess && !c.IsPending);
            RaisePropertyChanged("MyContacts");
        }

        public void AddTrustedContact(EmergencyContactViewModel contact)
        {
            TrustedContacts.Add(contact);
            IsAnyPendingTrustedContacts = TrustedContacts.Any(c => c.IsPending);
            IsAnyAcceptedTrustedContacts = TrustedContacts.Any(c => !c.IsPending);
            RaisePropertyChanged("TrustedContacts");
        }

        private void InitExampleContacts()
        {
            var items = GetViewItems().Take(2);
            InitializeMyContacts(items);
            InitializeTrustedContacts(items);
        }

        private void InitializeTrustedContacts(IEnumerable<SecureItemViewModel> items)
        {
            var subItems = items.ToList();
            var contact1 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.WaitingPeriod, 24,
                AccessType.ToSomeItems, null)
            {
                IsPending = true
            };
            AddTrustedContact(contact1);

            var contact2 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.WaitingPeriod, 24,
                AccessType.Declined, null);
            AddTrustedContact(contact2);


            var contact4 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.AccessWillBeGranted, 134,
                AccessType.AccessRequested, null);
            AddTrustedContact(contact4);

            var contact3 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.FullAccess, 0,
                AccessType.AllItems, subItems);
            AddTrustedContact(contact3);
        }

        private void InitializeMyContacts(IEnumerable<SecureItemViewModel> items)
        {
            var contact1 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.WaitingPeriod, 24,
                AccessType.ToSomeItems, items)
            {
                IsPending = true
            };
            AddMyContract(contact1);

            var contact2 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.WaitingPeriod, 24,
                AccessType.Declined, null)
            {
                IsPending = true
            };
            AddMyContract(contact2);

            var contact3 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.FullAccess, 0,
                AccessType.AllItems, null);
            AddMyContract(contact3);

            var contact4 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.AccessWillBeGranted, 134,
                AccessType.AccessRequested, null);
            AddMyContract(contact4);

            var contract5 = new EmergencyContactViewModel("steve.steve@myemail.com", AccessPeriodType.WaitingPeriod, 24,
                AccessType.AllItems, null);
            AddMyContract(contract5);
        }

        #endregion
    }
}
