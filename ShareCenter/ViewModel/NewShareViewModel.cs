using System;
using System.Collections.Generic;
using System.Linq;
using PasswordBoss.Helpers;
using PasswordBoss.Views.UserControls;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    class NewShareViewModel : ViewModelBase
    {
        #region fields
        private AddShareViewModel _previousPageViewModel;
        private string _shareName;
        private string _email;
        private string _message;
        private bool _isPasswordVisible;
        private RelayCommand _backCommand;
        private RelayCommand _shareCommand;
        private readonly List<AddSecureSubItem> _addItems;
        private readonly IResolver _resolver;
        private bool _isEmailValid = true;
        private bool _isShareNameValid = true;
        private bool _mayClose;
        private readonly IPBData _pbData;
        #endregion

        #region properties
        public bool MayClose
        {
            get { return _mayClose; }
            set
            {
                _mayClose = value;
                RaisePropertyChanged("MayClose");
            }
        }

        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set
            {
                _isEmailValid = value;
                RaisePropertyChanged("IsEmailValid");
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                IsEmailValid = new Common().IsEmailValid(Email);
                RaisePropertyChanged("Email");
            }
        }

        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
                RaisePropertyChanged("BackCommand");
            }
        }

        public RelayCommand ShareCommand
        {
            get { return _shareCommand; }
            set
            {
                _shareCommand = value;
                RaisePropertyChanged("ShareCommand");
            }
        }

        public AddShareViewModel PreviousPageViewModel
        {
            get { return _previousPageViewModel; }
            set
            {
                _previousPageViewModel = value;
                RaisePropertyChanged("PreviousPageViewModel");
            }
        }

        public string ShareName
        {
            get { return _shareName; }
            set
            {
                _shareName = value;
                IsShareNameValid = !string.IsNullOrEmpty(_shareName);
                RaisePropertyChanged("ShareName");
            }
        }

        public bool IsShareNameValid
        {
            get { return _isShareNameValid; }
            set
            {
                _isShareNameValid = value;
                RaisePropertyChanged("IsShareNameValid");
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

        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                _isPasswordVisible = value;
                RaisePropertyChanged("IsPasswordVisible");
            }
        }
        #endregion

        public NewShareViewModel(IResolver resolver, AddShareViewModel previousPageViewModel, IEnumerable<AddSecureSubItem> addItems)
        {
            _resolver = resolver;
            PreviousPageViewModel = previousPageViewModel;
            _pbData = resolver.GetInstanceOf<IPBData>();
            _addItems = addItems.ToList();
            BackCommand = new RelayCommand(OnBackCommandHandler);
            ShareCommand = new RelayCommand(OnShareCommandHandler);
        }

        private void OnShareCommandHandler(object o)
        {
            IsEmailValid = new Common().IsEmailValid(Email);
            IsShareNameValid = !string.IsNullOrEmpty(_shareName);
            if (IsEmailValid && IsShareNameValid)
            {
                var sharedWithVm = new SharedWithViewModel(Email, ShareWithStatus.Pending);
                if (PreviousPageViewModel.IsIndividualItems)
                    PreviousPageViewModel.ResultedShare = new SharedByMeViewModel(Guid.NewGuid().ToString(), ShareName, true, PreviousPageViewModel.SelectedItems,
                        new List<SharedWithViewModel> { sharedWithVm }, true);
                else
                {
                    var shares = new List<ShareFolderViewModel>();
                    foreach (var selectedFolder in PreviousPageViewModel.SelectedItems.OfType<AddedShareFolder>())
                    {
                        var folder = CreateShareFromFolder(selectedFolder.FolderId, selectedFolder.Name, 
                            selectedFolder.ParentFolder == null ? string.Empty : selectedFolder.ParentFolder.FolderId);
                        if (selectedFolder.ParentFolder != null)
                        {
                            var parentShare = shares.FirstOrDefault(s => s.FolderId == selectedFolder.ParentFolder.FolderId);
                            parentShare.AddSubItem(folder);
                        }
                        shares.Add(folder);
                    }

                    shares.RemoveAll(child => !string.IsNullOrEmpty(child.RootFolder));
                    PreviousPageViewModel.ResultedShare = new SharedByMeViewModel(Guid.NewGuid().ToString(), ShareName, true, shares,
                        new List<SharedWithViewModel> {sharedWithVm}, true);
                }
                MayClose = true;
            }
        }

        private ShareFolderViewModel CreateShareFromFolder(string folderId, string folderName, string rootFolderId)
        {
            var items = _pbData.GetSecureItemsByFolderId(folderId);
            var folder = new ShareFolderViewModel(folderId, folderName, null, false, rootFolderId);
            foreach (var secureItem in items)
            {
                var item = _addItems.FirstOrDefault(x => x.ItemType == secureItem.Type);
                var secureItemVm = Activator.CreateInstance(item.CreateItemType, secureItem, item.BackgoundColor, item.Icon) as SecureItemViewModel;
                folder.AddSubItem(secureItemVm);
            }

            return folder;
        }

        private void OnBackCommandHandler(object o)
        {
            MayClose = true;
            var addShareView = new AddShareWindow()
            {
                DataContext = new AddShareViewModel(PreviousPageViewModel, _resolver, _addItems)
            };
            addShareView.ShowDialog();
        }
    }
}
