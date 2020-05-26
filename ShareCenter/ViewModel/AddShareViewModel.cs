using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    class AddedShareFolder : ViewModelBase
    {
        #region fields
        private bool _isSelected;
        private string _folderId;
        private List<AddedShareFolder> _childFolders;
        private AddedShareFolder _parentFolder;
        private string _name;
        private bool _isExpanded;
        #endregion

        #region properties

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
        public string FolderId
        {
            get { return _folderId; }
            set
            {
                _folderId = value;
                RaisePropertyChanged("FolderId");
            }
        }

        public AddedShareFolder ParentFolder
        {
            get { return _parentFolder; }
            set
            {
                _parentFolder = value;
                RaisePropertyChanged("ParentFolder");
            }
        }

        public List<AddedShareFolder> ChildFolders
        {
            get { return _childFolders; }
            set
            {
                _childFolders = value;
                RaisePropertyChanged("ChildFolders");
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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (!value && ParentFolder != null && ParentFolder.IsSelected)
                    return;
                _isSelected = value;
                if (_isSelected)
                {
                    IsExpanded = true;
                    SelectChildFolders(this);
                }
                RaisePropertyChanged("IsSelected");
            }
        }
        #endregion

        public AddedShareFolder(string folderId, string folderName)
        {
            FolderId = folderId;
            ChildFolders = new List<AddedShareFolder>();
            Name = folderName;
        }

        #region methods
        private void SelectChildFolders(AddedShareFolder parentFolder)
        {
            foreach (var childFolder in parentFolder.ChildFolders)
            {
                childFolder.IsSelected = true;
                childFolder.IsExpanded = true;
                SelectChildFolders(childFolder);
            }
        }

        public void AddChildFolder(AddedShareFolder childFolder)
        {
            ChildFolders.Add(childFolder);
            RaisePropertyChanged("ChildFolders");
        }
        #endregion
    }

    class AddShareViewModel : ViewModelBase
    {
        #region fields
        private static readonly ILogger Logger = PasswordBoss.Logger.GetLogger(typeof(AddShareViewModel));
        private readonly IPBData _pbData;
        private List<object> _allItems;
        private List<object> _displayedItems;
        private ObservableCollection<object> _currentTypeSelectedItems;
        private readonly List<AddSecureSubItem> _subItemsComponentTree;
        private bool _isIndividualItems;
        private AddShareItemType _currentItemType;
        private RelayCommand _unselectItemCommand;
        private string _searchedText;
        private List<object> _savedSelectedItems;
        private ObservableCollection<object> _selectedItems;
        public SearchViewModel _searchViewModel;
        private RelayCommand _nextCommand;
        private readonly IResolver _resolver;
        private bool _mayGoNext;
        private SharedByMeViewModel _resultedShare;
        private bool _isValid = true;
        #endregion

        #region properties
        public SharedByMeViewModel ResultedShare
        {
            get { return _resultedShare; }
            set
            {
                _resultedShare = value;
                RaisePropertyChanged("ResultedShare");
            }
        }

        public RelayCommand NextCommand
        {
            get { return _nextCommand; }
            set
            {
                _nextCommand = value;
                RaisePropertyChanged("NextCommand");
            }
        }

        public bool MayGoNext
        {
            get { return _mayGoNext; }
            set
            {
                _mayGoNext = value;
                RaisePropertyChanged("MayGoNext");
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                RaisePropertyChanged("IsValid");
            }
        }

        public SearchViewModel SearchViewModel
        {
            get { return _searchViewModel; }
            set
            {
                _searchViewModel = value;
                RaisePropertyChanged("SearchViewModel");
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                RaisePropertyChanged("SelectedItems");
            }
        }

        public RelayCommand UnselectItemCommand
        {
            get { return _unselectItemCommand; }
            set
            {
                _unselectItemCommand = value;
                RaisePropertyChanged("UnselectItemCommand");
            }
        }

        public AddShareItemType CurrentItemType
        {
            get { return _currentItemType; }
            set
            {
                _currentItemType = value;
                RaisePropertyChanged("CurrentItemType");
            }
        }

        public bool IsIndividualItems
        {
            get { return _isIndividualItems; }
            set
            {
                _isIndividualItems = value;
                RefreshDispalyedList();
                SelectedItems.Clear();
                IsValid = true;
                RaisePropertyChanged("IsIndividualItems");
            }
        }

        public List<object> DisplayedItems
        {
            get { return _displayedItems; }
            set
            {
                _displayedItems = value;
                RaisePropertyChanged("DisplayedItems");
            }
        }

        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get { return _subItemsComponentTree; }
        }

        public List<object> AllItems
        {
            get { return _allItems; }
            set
            {
                _allItems = value;
                RaisePropertyChanged("AllItems");
            }
        }
        #endregion

        public AddShareViewModel(IResolver resolver, IEnumerable<AddSecureSubItem> addItems)
        {
            _resolver = resolver;
            _pbData = resolver.GetInstanceOf<IPBData>();
            _subItemsComponentTree = addItems.ToList();
            InitializeSecurityItems();
            SearchViewModel = new SearchViewModel(resolver, null);
            
            SelectedItems = new ObservableCollection<object>();
            SelectedItems.CollectionChanged += (o, e) =>
            {
                if (!IsValid)
                    IsValid = SelectedItems.Any();
            };
            CurrentItemType = AddShareItemType.Passwords;
            IsIndividualItems = true;
            
            UnselectItemCommand = new RelayCommand(OnUnselectItem);
            NextCommand = new RelayCommand(OnNextCommandHandler);
            MayGoNext = false;
        }

        public AddShareViewModel(AddShareViewModel cpy, IResolver resolver, IEnumerable<AddSecureSubItem> addItems)
            : this(resolver, addItems)
        {
            SearchViewModel = cpy.SearchViewModel;
            SelectedItems = cpy.SelectedItems;
            CurrentItemType = cpy.CurrentItemType;
            IsIndividualItems = cpy.IsIndividualItems;
        }

        #region methods
        private void OnNextCommandHandler(object o)
        {
            MayGoNext = SelectedItems.Any();
            IsValid = MayGoNext;
            if (!MayGoNext)
                return;
            var newShareView = new NewShareView
            {
                DataContext = new NewShareViewModel(_resolver, this, _subItemsComponentTree)
            };
            newShareView.ShowDialog();
        }

        private void OnUnselectItem(object o)
        {
            if (o is AddedShareFolder)
            {
                var folder = o as AddedShareFolder;
                if (folder.ParentFolder != null && folder.ParentFolder.IsSelected)
                    return;
            }
            SelectedItems.Remove(o);
        }

        private void RefreshDispalyedList()
        {
            if (IsIndividualItems)
                DisplayedItems = AllItems.Where(item => item is SecureItemViewModel).ToList();
            else
                DisplayedItems = AllItems.Where(item => item is AddedShareFolder).ToList();
        }

        private void InitializeSecurityItems()
        {
            AllItems = new List<object>();
            // passwords
            var passSecureItems = _pbData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
            CreateSecureItems(passSecureItems);
            // DW
            var walletSecureItems = _pbData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet);
            CreateSecureItems(walletSecureItems);
            // personal infos
            var personalInfoSecureItems = _pbData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo);
            CreateSecureItems(personalInfoSecureItems);
            // folder
            var folders = _pbData.GetFoldersBySecureItemType();
            CreateFolderViewModels(folders);
        }

        private void CreateFolderViewModels(IEnumerable<Folder> folders)
        {
            var lstFolders = folders as Folder[] ?? folders.ToArray();
            var foldersVm = lstFolders.Select(folder => new AddedShareFolder(folder.Id, folder.Name)).ToList();
            for (int i=0; i < lstFolders.Count(); i++)
            {
                var folderVm = foldersVm[i];
                var folder = lstFolders[i];
                if (!string.IsNullOrEmpty(folder.ParentId))
                {
                    var parentVm = foldersVm.First(parent => parent.FolderId == folder.ParentId);
                    folderVm.ParentFolder = parentVm;
                    parentVm.AddChildFolder(folderVm);
                }
            }

            foldersVm.RemoveAll(child => child.ParentFolder != null);
            foldersVm.ForEach(folder => AllItems.Add(folder));
            if (foldersVm.Any())
                RaisePropertyChanged("AllItems");
        }

        private void CreateSecureItems(IEnumerable<SecureItem> sites)
        {
            var secureItems = sites as SecureItem[] ?? sites.ToArray();
            foreach (var site in secureItems)
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
                    AllItems.Add(secureItemVm);
                }
            }
            if(secureItems.Any())
                RaisePropertyChanged("AllItems");
        }
        #endregion
    }
}
