using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.NotificationViewModels;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    /// <summary>
    /// Represents reimplemented view model for share center
    /// </summary>
    class NewShareCenterViewModel : ViewModelBase
    {
        #region fields
        private static readonly ILogger Logger = PasswordBoss.Logger.GetLogger(typeof(NewShareCenterViewModel));
        private readonly IPBData _pbData;

        private int _selectedIndexTabControl;
        private List<SharedByMeViewModel> _pendingSharesByMe;
        private List<SharedByMeViewModel> _currentSharesByMe;
        private List<SharedWithMeViewModel> _pendingSharesWithMe;
        private List<SharedWithMeViewModel> _currentSharesWithMe; 
        private bool _isTileView;
        private BaseNototificationViewModel _notificationVm;
        private bool _isNotificationVisible;
        private readonly List<AddSecureSubItem> _subItemsComponentTree;

        private RelayCommand _changeShareCommand;
        private RelayCommand _cancelSharingCommand;
        #endregion

        #region properties

        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get { return _subItemsComponentTree; }
        }
        public RelayCommand CancelSharingCommand
        {
            get { return _cancelSharingCommand; }
            set
            {
                _cancelSharingCommand = value;
                RaisePropertyChanged("CancelSharingCommand");
            }
        }

        public RelayCommand ChangeShareCommand
        {
            get { return _changeShareCommand; }
            set
            {
                _changeShareCommand = value;
                RaisePropertyChanged("ChangeShareCommand");
            }
        }

        public bool IsNotificationVisible
        {
            get { return _isNotificationVisible; }
            set
            {
                _isNotificationVisible = value;
                RaisePropertyChanged("IsNotificationVisible");
            }
        }

        public BaseNototificationViewModel NotificationVm
        {
            get { return _notificationVm; }
            set
            {
                _notificationVm = value;
                RaisePropertyChanged("NotificationVm");
            }
        }

        public bool IsAnyCurrentSharesWithMe
        {
            get { return _currentSharesWithMe.Any(); }
        }

        public int CurrentSharesWithMeCount
        {
            get { return _currentSharesWithMe.Count; }
        }

        /// <summary>
        /// Flag indicates whether there is at least one current share.
        /// </summary>
        public bool IsAnyCurrentShares
        {
            get { return _currentSharesByMe.Any(); }
        }

        public int CurrentSharesCount
        {
            get { return CurrentSharesByMe.Count; }
        } 
        /// <summary>
        /// List of current shares.
        /// </summary>
        public List<SharedByMeViewModel> CurrentSharesByMe
        {
            get { return _currentSharesByMe; }
            set
            {
                _currentSharesByMe = value;
                RaisePropertyChanged("CurrentSharesByMe");
                RaisePropertyChanged("IsAnyPendingShares");
                RaisePropertyChanged("CurrentSharesCount");
            }
        }
        /// <summary>
        /// List of current shares with me.
        /// </summary>
        public List<SharedWithMeViewModel> CurrentSharesWithMe
        {
            get { return _currentSharesWithMe; }
            set
            {
                _currentSharesWithMe = value;
                RaisePropertyChanged("CurrentSharesWithMe");
                RaisePropertyChanged("IsAnyCurrentSharesWithMe");
                RaisePropertyChanged("CurrentSharesWithMeCount");
            }
        }


        public bool IsTileView
        {
            get { return _isTileView; }
            set
            {
                _isTileView = value;
                RaisePropertyChanged("IsTileView");
            }
        }
        public DataTemplateSelector CurrentDataTemplateSelector { get; set; }

        public int PendingSharesCount
        {
            get { return PendingSharesByMe.Count; }
        } 
        /// <summary>
        /// List of pending shares.
        /// </summary>
        public List<SharedByMeViewModel> PendingSharesByMe
        {
            get { return _pendingSharesByMe; }
            set
            {
                _pendingSharesByMe = value;
                RaisePropertyChanged("PendingSharesByMe");
                RaisePropertyChanged("IsAnyPendingShares");
                RaisePropertyChanged("PendingSharesCount");
            }
        }

        public int PendingSharesWithMeCount
        {
            get { return PendingSharesWithMe.Count; }
        } 
        /// <summary>
        /// List of pending shares with me.
        /// </summary>
        public List<SharedWithMeViewModel> PendingSharesWithMe
        {
            get { return _pendingSharesWithMe; }
            set
            {
                _pendingSharesWithMe = value;
                RaisePropertyChanged("PendingSharesWithMe");
                RaisePropertyChanged("IsAnyPendingSharesWithMe");
                RaisePropertyChanged("PendingSharesWithMeCount");
            }
        }

        /// <summary>
        /// Flag indicates whether there is at least one pending share.
        /// </summary>
        public bool IsAnyPendingShares
        {
            get { return _pendingSharesByMe.Any(); }
        }

        /// <summary>
        /// Flag indicates whether there is at least one pending share.
        /// </summary>
        public bool IsAnyPendingSharesWithMe
        {
            get { return _pendingSharesWithMe.Any(); }
        } 

        /// <summary>
        /// Index of current selected tab.
        /// </summary>
        public int SelectedIndexTabControl
        {
            get { return _selectedIndexTabControl; }
            set
            {
                _selectedIndexTabControl = value;
                RaisePropertyChanged("SelectedIndexTabControl");
            }
        }
        #endregion

        #region ctrs
        public NewShareCenterViewModel(IResolver resolver, IEnumerable<AddSecureSubItem> addItems)
        {
            _subItemsComponentTree = addItems.ToList();
            _pbData = resolver.GetInstanceOf<IPBData>();
            PendingSharesByMe = new List<SharedByMeViewModel>();
            CurrentSharesByMe = new List<SharedByMeViewModel>();
            PendingSharesWithMe = new List<SharedWithMeViewModel>();
            CurrentSharesWithMe = new List<SharedWithMeViewModel>();

            ChangeShareCommand = new RelayCommand(OnChangeShareCommand);
            CancelSharingCommand = new RelayCommand(OnCancelSharingCommand);
            NotificationVm = new BaseNototificationViewModel();
            LoadList();
        }
        #endregion

        #region methods
        private void OnCancelSharingCommand(object o)
        {
            NotificationVm = new CancelSharingViewModel();
            IsNotificationVisible = true;
        }

        private void OnChangeShareCommand(object o)
        {
            NotificationVm = new ChangeShareViewModel();
            IsNotificationVisible = true;
        }

        /// <summary>
        /// Load dummy shares from db.
        /// </summary>
        private void LoadList()
        {
            if (_pbData.Locked)
                return;

            var items = GetViewItems().ToList();
            
            // Create pending shares
            if (items.Any())
            {
                Test1(items, true);
                Test2(items, true);
                Test3(items, true);

                Test1(items, false);
                Test2(items, false);
                Test3(items, false);
            }
        }
        
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
                            Logger.Error("GetSortedViewItems: site data is null");
                            continue;
                        }
                        var item = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == site.Type);
                        if (item != null)
                        {
                            var secureItemVm = Activator.CreateInstance(item.CreateItemType, site, item.BackgoundColor, item.Icon) as SecureItemViewModel;
                            passVaultItems.Add(secureItemVm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)Application.Current.FindResource("GeneralErrorText"));
                Logger.Error(ex.Message);
            }
            watch.Stop();
            Logger.Info("items got: {0}, executed in: {1} ms", passVaultItems.Count, watch.ElapsedMilliseconds);
            return passVaultItems;
        }

        /// <summary>
        /// Use this method for add current shares.
        /// </summary>
        /// <param name="item"></param>
        private void AddCurrentByMeItem(SharedByMeViewModel item)
        {
            CurrentSharesByMe.Add(item);
            RaisePropertyChanged("CurrentSharesByMe");
            RaisePropertyChanged("CurrentSharesCount");
            RaisePropertyChanged("IsAnyCurrentShares");
        }

        /// <summary>
        /// Use this method for add pending shares.
        /// </summary>
        /// <param name="item"></param>
        private void AddPendingByMeItem(SharedByMeViewModel item)
        {
            PendingSharesByMe.Add(item);
            RaisePropertyChanged("PendingSharesByMe");
            RaisePropertyChanged("IsAnyPendingShares");
            RaisePropertyChanged("PendingSharesCount");
        }

        private void AddPendingWithMeItem(SharedWithMeViewModel item)
        {
            PendingSharesWithMe.Add(item);
            RaisePropertyChanged("PendingSharesWithMe");
            RaisePropertyChanged("IsAnyPendingSharesWithMe");
            RaisePropertyChanged("PendingSharesWithMeCount");
        }

        private void AddCurrentWithMeItem(SharedWithMeViewModel item)
        {
            CurrentSharesWithMe.Add(item);
            RaisePropertyChanged("CurrentSharesWithMe");
            RaisePropertyChanged("CurrentSharesCount");
            RaisePropertyChanged("IsAnyCurrentSharesWithMe");
        }

        private void Test1(IEnumerable<SecureItemViewModel> items, bool isPending)
        {
            if(items == null)
                return;

            var shareWith = new List<SharedWithViewModel>
                {
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Accepted),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Declined),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Pending)
                };

            var lst = items.Count() > 2
                ? items.ToList().GetRange(0, 2)
                : items.ToList();
            var pendingShareWithoutFolder = new SharedByMeViewModel(Guid.NewGuid().ToString(), "Pending share name", true, lst, shareWith, isPending);
            //pending shared with me
            var sharedSubItems = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "First sub-item", isPending, lst, false,
                new DateTime(2016, 3, 23), SharedWithMeItemState.Pending);
            var sharedSubItems2 = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "Second sub-item", isPending, lst, false,
                new DateTime(2016, 5, 22), SharedWithMeItemState.Waiting);
            var pendingShareWithMe = new SharedWithMeViewModel(Guid.NewGuid().ToString(), "sender-email-address.com", isPending, 
                new List<object> { sharedSubItems, sharedSubItems2}, false);

            if (isPending)
            {
                AddPendingByMeItem(pendingShareWithoutFolder);
                AddPendingWithMeItem(pendingShareWithMe);
            }
            else
            {
                AddCurrentByMeItem(pendingShareWithoutFolder);
                AddCurrentWithMeItem(pendingShareWithMe);
            }
        }

        private void Test2(IEnumerable<object> items, bool isPending)
        {
            if (items == null)
                return;

            var shareWith = new List<SharedWithViewModel>
                {
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Accepted),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Declined),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Pending)
                };

            var lst = items.Count() > 2
                ? items.ToList().GetRange(0, 2)
                : items.ToList();
            
            var pendingFolder = new ShareFolderViewModel("1", "First folder", null, true);
            foreach (var subItem in lst)
                pendingFolder.AddSubItem(subItem);
            var pendingShareWithFolder = new SharedByMeViewModel(Guid.NewGuid().ToString(), "Share with folder", true, lst, shareWith, isPending);
            pendingShareWithFolder.AddItem(pendingFolder);
            //shared with me
            var sharedSubItems = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "First sub-item", isPending, lst, false,
                new DateTime(2016, 3, 23), SharedWithMeItemState.Pending);
            var sharedSubItems2 = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "Second sub-item", isPending, lst, false,
                new DateTime(2016, 5, 22), SharedWithMeItemState.Waiting);
            var shareWithMe = new SharedWithMeViewModel(Guid.NewGuid().ToString(), "sender-email-address.com", isPending,
                new List<object> { sharedSubItems, sharedSubItems2 }, false);

            if (isPending)
            {
                AddPendingByMeItem(pendingShareWithFolder);
            }
            else
            {
                AddCurrentByMeItem(pendingShareWithFolder);
                AddCurrentWithMeItem(shareWithMe);
            }
        }

        private void Test3(IEnumerable<object> items, bool isPending)
        {
            if (items == null)
                return;

            var shareWith = new List<SharedWithViewModel>
                {
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Accepted),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Declined),
                    new SharedWithViewModel("user@emailcompany.com", ShareWithStatus.Pending)
                };

            var lst = items.Count() > 2
                ? items.ToList().GetRange(0, 2)
                : items.ToList();

            var pendingFolder = new ShareFolderViewModel("1", "First folder", null, true);
            var subFolder = new ShareFolderViewModel("2", "Sub-folder", null, true);
            foreach (var subItem in lst)
                subFolder.AddSubItem(subItem);
            pendingFolder.AddSubItem(subFolder);
            foreach (var subItem in lst)
                pendingFolder.AddSubItem(subItem);
            var pendingShareWithSubFolder = new SharedByMeViewModel(Guid.NewGuid().ToString(), "Share with sub-folder", true, lst, shareWith, true);
            pendingShareWithSubFolder.AddItem(pendingFolder);

            //shared with me
            var sharedSubItems = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "First sub-item", isPending, lst, false,
                new DateTime(2016, 3, 23), SharedWithMeItemState.Pending);
            sharedSubItems.AddItem(subFolder);
            var sharedSubItems2 = new SharedWithMeItemShareViewModel(Guid.NewGuid().ToString(), "Second sub-item", isPending, lst, false,
                new DateTime(2016, 5, 22), SharedWithMeItemState.Waiting);
            var shareWithMe = new SharedWithMeViewModel(Guid.NewGuid().ToString(), "sender-email-address.com", isPending,
                new List<object> { sharedSubItems, sharedSubItems2 }, false);

            if (isPending)
            {
                AddPendingByMeItem(pendingShareWithSubFolder);
            }
            else
            {
                AddCurrentByMeItem(pendingShareWithSubFolder);
                AddCurrentWithMeItem(shareWithMe);
            }
        }
        #endregion
    }
}
