using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using PasswordBoss.UserControls;
using System.Collections.ObjectModel;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using PasswordBoss;
using System.Diagnostics;
using SecureItemsCommon.ViewModels;
using SecureItemsCommon.Helpers;
using System.Windows.Media;

namespace SecureItemsCommon
{

    public class SecureItemsHolderViewModel : ViewModelBase, ISecureHolder
    {
        
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecureItemsHolderViewModel));
        //private IAnalytics<SecureItemsHolderViewModel> inAppAnalyitics;


        private bool _recommendedsiteFlag;

        public event EventHandler DataUpdated;
        public event EventHandler FolderListUpdated;
        public event EventHandler AddSecureItem;
        
        #region Relay commands

        public RelayCommand SortBySelectionChangedCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }

        public RelayCommand EditItemCommand { get; set; }
        public RelayCommand MoveToCommand { get; set; }
        public RelayCommand ShareItemCommand { get; set; }
        public RelayCommand ChangeFolderItemCommand { get; set; }
        public RelayCommand SetAutoLoginCommand { get; set; }
        public RelayCommand OpenWebsiteCommand { get; set; }

        public RelayCommand AddNewFolderCommand { get; set; }

        public RelayCommand DeleteSelectedItemsCommand { get; set; }
        public RelayCommand ConfirmedDeleteSelectedItemsCommand { get; set; }
        public RelayCommand CanceledDeleteSelectedItemsCommand { get; set; }

        public RelayCommand AddSecureItemCommand { get; set; }


        #endregion

        #region Object Declararion
        //private PasswordVaultHelper _passwordVaultHelper;

        private IResolver resolver = null;
        private IPBData pbData = null;
        private IPBExtSecureBrowserBridge _pbExtSecureBrowserBridge;


        #endregion

        #region Properties

        public string SecureItemType { get; set; }


        private List<AddSecureSubItem> _subItemsComponentTree;

        public List<AddSecureSubItem> SubItemsComponentTree { get { return _subItemsComponentTree; } }


        public DataTemplateSelector CurrentDataTemplateSelector { get; set; }

        public List<ISecureItemVM> secureItemList;

        public List<ISecureItemVM> SecureItemList
        {
            get { return secureItemList; }
            set
            {
                secureItemList = value;
                RaisePropertyChanged("SecureItemList");
            }
        }


        private List<FolderView> allHierarchyFolderList = new List<FolderView>();
        private List<FolderView> allHierarchyTypeList = new List<FolderView>();

        private ObservableCollection<FolderView> hierarchyFolderList;
        public ObservableCollection<FolderView> HierarchyFolderList
        {
            get { return hierarchyFolderList; }
            set
            {
                hierarchyFolderList = value;
                RaisePropertyChanged("HierarchyFolderList");
            }
        }


        public List<string> SortByItems
        {
            get; set;
        }


        /// sort by combobox selected index property
        private int _sortBySelectedIndex;

        public int SortBySelectedIndex
        {
            get { return _sortBySelectedIndex; }
            set
            {

                if (Equals(_sortBySelectedIndex, value)) return;

                _sortBySelectedIndex = value;
                UpdateTreeView();
                RaisePropertyChanged("SortBySelectedIndex");
            }
        }

      



        private bool _DeleteSelectedItemsMessageBoxVisibility;
        public bool DeleteSelectedItemsMessageBoxVisibility
        {
            get
            {
                return _DeleteSelectedItemsMessageBoxVisibility;
            }
            set
            {
                _DeleteSelectedItemsMessageBoxVisibility = value;
                RaisePropertyChanged("DeleteSelectedItemsMessageBoxVisibility");
            }
        }

        private string _DeleteSelectedItemsMessageText;
        public string DeleteSelectedItemsMessageText
        {
            get
            {
                return _DeleteSelectedItemsMessageText;
            }
            set
            {
                _DeleteSelectedItemsMessageText = value;
                RaisePropertyChanged("DeleteSelectedItemsMessageText");
            }
        }

        private bool _showLandingScreen;
        public bool ShowLandingScreen
        {
            get
            {
                return _showLandingScreen;
            }
            set
            {
                _showLandingScreen = value;
                RaisePropertyChanged("ShowLandingScreen");
            }
        }

        private bool _ExpandAll;
        public bool ExpandAll
        {
            get
            {
                return _ExpandAll;
            }
            set
            {
                if (_ExpandAll != value)
                {
                    _ExpandAll = value;
                    ChangesExpandAllState(HierarchyFolderList, _ExpandAll);
                    RaisePropertyChanged("ExpandAll");
                }
            }
        }


        private bool _isTileView;

        public bool IsTileView
        {
            get { return _isTileView; }
            set
            {
                _isTileView = value;
                RaisePropertyChanged("IsTileView");
            }
        }

        private List<IContextAction> actions = new List<IContextAction>();
        public List<IContextAction> Actions
        {
            get
            {
                return actions;
            }
            set
            {
                actions = value;
                RaisePropertyChanged("Actions");
            }
        }

        public List<IContextAction> GetActions(IEnumerable<ISecureItemVM> selectedItems)
        {
            if (selectedItems == null || !selectedItems.Any())
                return null;

            Actions.FirstOrDefault(x => x.IsFolderList).SubItems = FoldersListContext;

            if (!selectedItems.Any(x => x.SubType == SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login))
                Actions.FirstOrDefault().Visibility = Visibility.Collapsed;
            else
                Actions.FirstOrDefault().Visibility = Visibility.Visible;

            if (selectedItems.Count() > 1)
                Actions[1].Visibility = Visibility.Collapsed;
            else
                Actions[1].Visibility = Visibility.Visible;

            return Actions;
        }



        private List<IContextAction> _folderActions = new List<IContextAction>();
        public List<IContextAction> FolderActions
        {
            get
            {
                return _folderActions;
            }
            set
            {
                _folderActions = value;
                RaisePropertyChanged("FolderActions");
            }
        }

        public List<IContextAction> GetFolderActions(FolderView view)
        {
            if (view == null)
                return null;

            FolderActions.FirstOrDefault(x => x.IsFolderList).SubItems = FoldersListContext;

            if (string.IsNullOrEmpty(view.uuid))
                FolderActions.FirstOrDefault().Visibility = Visibility.Collapsed;
            else
                FolderActions.FirstOrDefault().Visibility = Visibility.Visible;


            return FolderActions;
        }

        public List<IContextAction> FoldersListContext
        {
            get
            {
                var list = new List<IContextAction>();
                foreach (var folder in pbData.GetFoldersBySecureItemType())
                    list.Add(new SubContextAction() { Name = folder.Name, Action = ChangeFolderItemCommand, ActionParameter = folder.Id });
                list.Insert(0, new SubContextAction() { Name = string.Empty, Action = ChangeFolderItemCommand, ActionParameter = string.Empty });
                return list;
            }

        }


        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public SecureItemsHolderViewModel(IResolver resolver, List<AddSecureSubItem> addItems, string type)
        {
            this.resolver = resolver;

            if (_pbExtSecureBrowserBridge == null) _pbExtSecureBrowserBridge = resolver.GetInstanceOf<IPBExtSecureBrowserBridge>();

            pbData = resolver.GetInstanceOf<IPBData>();


            InitializeCommands();
            //TODO check if there is any passwords, if not _recommendedsiteFlag set true
            _recommendedsiteFlag = false;
            SecureItemList = new List<ISecureItemVM>();
            _subItemsComponentTree = addItems;
            SecureItemType = type;

            ShowLandingScreen = pbData.GetSecureItemCountByType(SecureItemType) == 0 ? true : false;
            RefreshData();

        }



        void ShareImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            //    try
            //    {
            //        SecureItem item = pbData.GetSecureItemById(e.ItemId);
            //        //We can't share what is shared with us
            //        if (!item.Readonly)
            //        {
            //            ShareItem(item);
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        logger.Error(ex.Message);
            //    }
        }


        private ObservableCollection<FolderView> GetFoldersHierarchyCollection()
        {
            var tempList = new ObservableCollection<FolderView>();
            foreach (var item in pbData.GetFoldersBySecureItemType())
            {
                tempList.Add(new FolderView()
                {
                    uuid = item.Id,
                    parentId = item.ParentId,
                    FolderName = item.Name

                });

            }

            var itemsToRemove = new List<FolderView>();

            for (int i = 0; i < tempList.Count; i++)
            {
                if (!string.IsNullOrEmpty(tempList[i].parentId))
                {
                    var parent = tempList.FirstOrDefault(x => x.uuid == tempList[i].parentId);
                    if (parent != null)
                    {
                        parent.ChildList.Add(tempList[i]);
                        itemsToRemove.Add(tempList[i]);
                    }

                }

            }
            foreach (var item in itemsToRemove)
                tempList.Remove(item);

            return tempList;
        }


        void OpenBrowser_Clicked(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            var secureItem = sender as SecureItemViewModel;
            if (secureItem != null)
            {
                _pbExtSecureBrowserBridge.OneClickLogin(secureItem.Id, false);
            }
        }

        private void FavoriteImage_Clicked(object sender, EventArgs e)
        {
            //IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            //if (!featureChecker.IsEnabled(DefaultProperties.Features_PasswordValt_ManageFavorites))
            //{
            //    return;
            //}

            //Code changes due to favorite grid and listView differences

            if (sender != null && sender is SecureItemViewModel)
            {
                var secureItem = (SecureItemViewModel)sender;
                bool favorite = secureItem.Favorite;
                if (pbData.UpdateSecureItemFavorite(secureItem.Id, !favorite))
                    secureItem.Favorite = !favorite;

            }

            RefreshData();
        }



        public void ShareItem(SecureItem item)
        {
            //if (item != null)
            //{
            //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            //    {
            //        _addControlViewModel.SecureItem = item;
            //        _addControlViewModel.SelectedIndexTabControl = 1;
            //        AddNewItemVisibility = true;
            //        Storyboard sb = passwordVaultContentPanel.FindResource("sbOpenEditNewItem") as Storyboard;
            //        var uc = passwordVaultContentPanel.FindName("AddEditControl") as UserControl;
            //        Storyboard.SetTarget(sb, uc);
            //        sb.Begin();
            //    }));


            //}
        }

        void DeleteImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            DeleteSelectedItemsClick(new List<ISecureItemVM> { sender as ISecureItemVM });
        }



        public void RefreshData()
        {

            RefreshList(this, new RoutedEventArgs());
        }

        public void ChangesExpandAllState(IEnumerable<object> folders, bool expandState)
        {

            foreach (var item in folders)
            {
                var folder = item as FolderView;
                if (folder != null && folder.ChildList.Any())
                {
                    folder.IsExpanded = expandState;
                    ChangesExpandAllState(folder.ChildList, expandState);
                }
            }

        }


        public void ChangeValuesForDatabase()
        {
            forceReloadFromDatabase = true;
        }

        private DateTime lastCheckDate = DateTime.MinValue;
        private int lastSortIndex = 0;
        private bool forceReloadFromDatabase = false;
        private string lastAccount = "";


        private void RefreshList(object sender, RoutedEventArgs e)
        {
            if (pbData.Locked) return;
            ShowLandingScreen = pbData.GetSecureItemCountByType(SecureItemType) == 0 ? true : false;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (forceReloadFromDatabase || SecureItemList == null || SecureItemList.Count == 0
                || lastSortIndex != SortBySelectedIndex || _recommendedsiteFlag || pbData.IsSecureItemOrSiteChanged(lastCheckDate)
                || lastAccount != pbData.ActiveUser) //TODO: We should check are there any changes in DB and if there are only then reload
            {
                List<SecureItemViewModel> items = GetViewItems(_recommendedsiteFlag);
                SecureItemList.Clear();
                items.ForEach(x =>
                {
                    x.FavoritesIcon_Clicked += FavoriteImage_Clicked;
                    x.OpenInBrowser_Clicked += OpenBrowser_Clicked;
                    x.SharingIcon_Clicked += ShareImage_Clicked;
                    x.DeletingIcon_Clicked += DeleteImage_Clicked;
                    x.Edit_Clicked += Edit_Clicked;
                    x.AddNewFolder_Clicked += AddNewFolder_Clicked;
                });

                SecureItemList = new List<ISecureItemVM>(items);
                RebuildFolderTreeView(items);


                lastCheckDate = DateTime.Now;
                lastSortIndex = SortBySelectedIndex;
                lastAccount = pbData.ActiveUser;
            }

            UpdateTreeView();
            forceReloadFromDatabase = false;
            watch.Stop();
            logger.Debug("SecureItemsHolderViewModel.RefreshList: Elapsed time {0}", watch.ElapsedMilliseconds);

        }

        private void RebuildFolderTreeView(List<SecureItemViewModel> items)
        {
            var oldTree = allHierarchyFolderList;
            allHierarchyFolderList = new List<FolderView>();
            foreach (var item in pbData.GetFoldersBySecureItemType())
            {
                var previosFolderState = FindTreeView(oldTree, item.Id);
                allHierarchyFolderList.Add(new FolderView(item.Id, item.ParentId, item.Name, previosFolderState == null ? true : previosFolderState.IsExpanded));

            }

            var previosRootFolderState = oldTree.FirstOrDefault(x => x.uuid == string.Empty);
            var rootFolder = new FolderView(string.Empty, string.Empty, "No Folder", previosRootFolderState == null ? true : previosRootFolderState.IsExpanded);

            allHierarchyFolderList.Add(rootFolder);

            foreach (var item in SortSecureItems(items))
            {
                if (item.Folder != null)
                {
                    var folder = allHierarchyFolderList.FirstOrDefault(x => x.uuid == item.Folder.Id);
                    if (folder != null)
                    {
                        folder.AddSecureItem(item);
                        continue;
                    }
                }

                rootFolder.AddSecureItem(item);

            }


            var itemsToRemove = new List<FolderView>();

            for (int i = 0; i < allHierarchyFolderList.Count; i++)
            {
                if (!string.IsNullOrEmpty(allHierarchyFolderList[i].parentId))
                {
                    var parent = allHierarchyFolderList.FirstOrDefault(x => x.uuid == allHierarchyFolderList[i].parentId);
                    if (parent != null && parent.uuid!= allHierarchyFolderList[i].uuid)
                    {
                        parent.ChildList.Add(allHierarchyFolderList[i]);
                        itemsToRemove.Add(allHierarchyFolderList[i]);
                    }

                }

            }
            foreach (var item in itemsToRemove)
                allHierarchyFolderList.Remove(item);
        }

        private void RebuildByTypeTreeView(List<ISecureItemVM> items)
        {
            allHierarchyTypeList = new List<FolderView>();
            foreach (var type in items.GroupBy(x => x.SubType))
            {
                var typeView = new FolderView()
                {
                    FolderName = type.Key,
                    IsExpanded = true

                };
                foreach (var item in type)
                    typeView.AddSecureItem(item);

                allHierarchyTypeList.Add(typeView);


            }
        }

        private void AddNewFolder_Clicked(object sender, EventArgs e)
        {
            var folder = ServiceLocator.Get<IFolderService>().AddFolder();
            if (!string.IsNullOrEmpty(folder))
            {
                (sender as SecureItemViewModel).FoldersList = pbData.GetFoldersBySecureItemType();
                (sender as SecureItemViewModel).Folder = (sender as SecureItemViewModel).FoldersList.FirstOrDefault(x => x.UUID == folder);
                if (FolderListUpdated != null)
                    FolderListUpdated(null, null);
            };
        }

        private void InitializeCommands()
        {
            SortByItems = new List<string>()
                {
                    Application.Current.FindResource("AZSort").ToString(),
                    Application.Current.FindResource("ZASort").ToString(),
                    Application.Current.FindResource("MenuCreatedDate").ToString(),
                    Application.Current.FindResource("Favorites").ToString(),
                    Application.Current.FindResource("MenuLastModified").ToString(),
                    Application.Current.FindResource("LastUsedAndroidSort").ToString(),
                    Application.Current.FindResource("ItemFieldType").ToString()
                };
            SortBySelectedIndex = 0;

            DeleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItemsClick);
            ConfirmedDeleteSelectedItemsCommand = new RelayCommand(ConfirmedDeleteSelectedItemsClick);
            CanceledDeleteSelectedItemsCommand = new RelayCommand(CanceledDeleteSelectedItemsClick);
            ChangeFolderItemCommand = new RelayCommand(ChangeFolderClick);
            EditItemCommand = new RelayCommand(EditItemClick);
            SetAutoLoginCommand = new RelayCommand(SetAutoLoginClick);

            AddSecureItemCommand = new RelayCommand(AddSecureItemClick);

            InitFolderActions();

        }

        private void InitFolderActions()
        {
            Actions = new List<IContextAction>()
                    {
                        new ContextAction()
                        {
                            Action = OpenWebsiteCommand,
                            Name = Application.Current.FindResource("MenuOpenWebsite") as string,
                            Icon = Application.Current.Resources["menuOpenSiteGrey"] as ImageSource,
                            IconHover = Application.Current.Resources["menuOpenSiteGreen"] as ImageSource
                        },
                         new ContextAction()
                            {
                                Action =EditItemCommand,
                                Name =Application.Current.FindResource("Edit") as string,
                                Icon =Application.Current.Resources["menuGearGrey"] as ImageSource,
                                IconHover =Application.Current.Resources["menuGearGreen"] as ImageSource
                           },
                          new ContextAction()
                        {
                                        IsFolderList=true,
                                        Name =Application.Current.FindResource("MenuMoveTo") as string,

                         }, new ContextAction()
                        {
                                        Action =ShareItemCommand,
                                        Name =Application.Current.FindResource("Share") as string,
                                        Icon =Application.Current.Resources["menuPeopleGrey"] as ImageSource,
                                        IconHover =Application.Current.Resources["menuPeopleGreen"] as ImageSource
                         },
                            new ContextAction()
                            {
                                Action =DeleteSelectedItemsCommand,
                                Name =Application.Current.FindResource("Delete") as string,
                                Icon =Application.Current.Resources["menuTrashGrey"] as ImageSource,
                                IconHover =Application.Current.Resources["menuTrashGreen"] as ImageSource
                            }
                    };

            FolderActions = new List<IContextAction>()
                    {
                            new ContextAction()
                            {
                                Action =EditItemCommand,
                                Name =Application.Current.FindResource("Edit") as string,
                                Icon =Application.Current.Resources["menuPencilGrey"] as ImageSource,
                                IconHover =Application.Current.Resources["menuPencilGreen"] as ImageSource
                            },
                            new ContextAction()
                            {
                                Name =Application.Current.FindResource("MenuMoveTo") as string,
                                IsFolderList=true
                            },
                            new ContextAction()
                            {
                                Action =ShareItemCommand,
                                Name =Application.Current.FindResource("Share") as string,
                                Icon =Application.Current.Resources["menuPeopleGrey"] as ImageSource,
                                IconHover =Application.Current.Resources["menuPeopleGreen"] as ImageSource
                            },
                            new ContextAction()
                            {
                                Action =DeleteSelectedItemsCommand,
                                Name =Application.Current.FindResource("Delete") as string,
                                Icon =Application.Current.Resources["menuTrashGrey"] as ImageSource,
                                IconHover =Application.Current.Resources["menuTrashGreen"] as ImageSource
                            },
                            new ContextAction()
                            {
                                Name = Application.Current.FindResource("MenuSetAutoLogin") as string,
                                SubItems=new List<IContextAction>()
                                {
                                    new SubContextAction()
                                    {
                                        Action = SetAutoLoginCommand,
                                        ActionParameter=true.ToString(),
                                        Name =Application.Current.FindResource("On") as string
                                    },
                                    new SubContextAction()
                                    {
                                        Action = SetAutoLoginCommand,
                                        ActionParameter=false.ToString(),
                                        Name =Application.Current.FindResource("Off") as string
                                    },
                                }
                            },
                            new ContextAction()
                            {
                                Action = OpenWebsiteCommand,
                                Name = Application.Current.FindResource("MenuOpenWebsite") as string,
                                Icon = Application.Current.Resources["menuOpenSiteGrey"] as ImageSource,
                                IconHover = Application.Current.Resources["menuOpenSiteGreen"] as ImageSource
                            }


                    };
        }



        private IEnumerable<ISecureItemVM> selectedItems;
        public void DeleteSelectedItemsClick(object parameter)
        {
            if (parameter == null)
                return;
            if (parameter is FolderView)
            {
                DeleteFolder(parameter as FolderView);
                return;
            }

            if (parameter is SecureItemsView)
                selectedItems = new List<ISecureItemVM>((parameter as SecureItemsView).SelectedItems);

            if (parameter is IEnumerable<ISecureItemVM>)
                selectedItems = parameter as IEnumerable<ISecureItemVM>;

            if (selectedItems != null && selectedItems.Count() > 0)
            {
                if (selectedItems.Count() > 1)
                {
                    DeleteSelectedItemsMessageText = (string)Application.Current.FindResource("DeleteSecureItemsMessageText");
                }
                else
                {
                    DeleteSelectedItemsMessageText = (string)Application.Current.FindResource("DeleteSecureItemBody");
                }
                DeleteSelectedItemsMessageBoxVisibility = true;
            }

        }

        private void DeleteFolder(FolderView folder)
        {
            pbData.DeleteFolder(folder.uuid);
        }

      

        private void CanceledDeleteSelectedItemsClick(object parameter)
        {
            DeleteSelectedItemsMessageBoxVisibility = false;
        }

        private void ConfirmedDeleteSelectedItemsClick(object parameter)
        {
            IEnumerable<string> selectedIds = selectedItems.Select(x => x.Id);
            if (selectedIds == null)
                return;
            Task.Factory.StartNew(() =>
            {
                bool logged = false;

                Parallel.ForEach(selectedIds, (id) =>
                {
                    var selectedItem = pbData.GetSecureItemById(id);


                    if (selectedItem != null)
                    {
                        SecureItem secureItem = null;
                        selectedItem.Active = false;
                        if ((secureItem = pbData.AddOrUpdateSecureItem(selectedItem)) == null)
                        {
                            DeleteSelectedItemsMessageBoxVisibility = false;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show((string)Application.Current.FindResource("GeneralErrorText"));
                            }));
                        }
                        else
                        {
                            ShareCommon shareCommon = new ShareCommon(resolver);
                            shareCommon.UpdateShares(secureItem);
                        }

                        if (!logged)
                        {
                            logged = true;
                            //  inAppAnalyitics.Log(new PasswordVaultItem(SecureItemAction.Deleted, ApplicationSource.MainUI));

                        }
                    }
                });

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    DeletionFinished();
                }));
            });
        }

        private void DeletionFinished()
        {
            DeleteSelectedItemsMessageBoxVisibility = false;
            IEnumerable<string> selectedIds = selectedItems.Select(x => x.Id);
            foreach (var item in selectedItems)
            {
                if (item.Folder != null)
                {
                    var folder = FindTreeViewWithDefault(allHierarchyFolderList, item.Folder.Id);

                    if (folder != null)
                    {
                        folder.RemoveSecureItem(item);
                        ShowLandingScreen = pbData.GetSecureItemCountByType(SecureItemType) == 0 ? true : false;
                    }
                }

            }
            if (DataUpdated != null)
                DataUpdated(null, null);

        }

        public void MoveSecureItemToFolder(IEnumerable<object> items, string folderId)
        {
            if (items == null)
                return;
            var folder = pbData.GetFolderById(folderId);
            var temp = new List<object>(items);
            bool IsSearch;
            if (folder != null)
            {
                foreach (var it in temp)
                {
                    var item = it as SecureItemViewModel;
                    if (item != null)
                    {

                       var secureItemVM = GetFullSecureItem(item.Id);
                       if (secureItemVM != null)
                           item = secureItemVM;


                        FolderView oldFolder = null;
                        if(item.Folder!=null)
                            oldFolder= FindTreeViewWithDefault(allHierarchyFolderList, item.Folder.Id);
                        FolderView folderView = FindTreeViewWithDefault(allHierarchyFolderList, folderId);

                        item.Folder = folder;
                        if (SaveItem(item.CreateSecureItem()))
                        {

                            if (oldFolder != null)
                                oldFolder.RemoveSecureItem(item);
                            if (folderView != null)
                                folderView.AddSecureItem(item);
                        }

                    }
                }
                // RefreshData();
                // folderView.IsExpanded = true;
            }
        }
        
        public void AddNewItem(ISecureItemVM secureItem)
        {
            if (secureItem == null)
                return;
            SetCountriesValue(secureItem);

            AddSecureItemWindow addWindow = new AddSecureItemWindow(CurrentDataTemplateSelector) { Title = secureItem.ItemTitel };
            addWindow.DataContext = secureItem;
            addWindow.Closing += (o, args) =>
            {
                if (addWindow.DialogResult == null)
                    args.Cancel = true;

                if (addWindow.DialogResult.HasValue && addWindow.DialogResult.Value)
                    args.Cancel = !secureItem.Validate();
            };
            bool? dialogResult = addWindow.ShowDialog();
            if (dialogResult.Value)
            {
                if (SaveItem(secureItem.CreateSecureItem()))
                    RefreshData();
            }

        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            var secureItem = sender as ISecureItemVM;
            if (secureItem == null)
                return;

            secureItem.FoldersList = pbData.GetFoldersBySecureItemType();
            SetCountriesValue(secureItem, false);

            AddSecureItemWindow addWindow = new AddSecureItemWindow(CurrentDataTemplateSelector, true) { Title = secureItem.Name };
            addWindow.DataContext = secureItem;
            addWindow.Closing += (o, args) =>
            {
                if (addWindow.DialogResult == null)
                    args.Cancel = true;

                if (addWindow.DialogResult.HasValue && addWindow.DialogResult.Value)
                    args.Cancel = !secureItem.Validate();
            };
            bool? dialogResult = addWindow.ShowDialog();
            if (dialogResult.Value)
            {
                if (SaveItem(secureItem.CreateSecureItem()))
                {
                    RefreshData();
                    if (DataUpdated != null)
                        DataUpdated(null, null);
                }
            }
        }

        private void EditItemClick(object param)
        {
            if (param is ISecureItemVM)
            {
                Edit_Clicked((ISecureItemVM)param, null);
            }

            if (param is SecureItemsView)
            {
                var secureItemView = param as SecureItemsView;
                if (secureItemView.SelectedItems != null && secureItemView.SelectedItems.Count == 1)
                    Edit_Clicked(secureItemView.SelectedItems.FirstOrDefault(), null);
            }

            if (param is FolderView)
            {
                var folder = param as FolderView;
                if (ServiceLocator.Get<IFolderService>().UpdateFolder(pbData.GetFolderById(folder.uuid)))
                {
                    if (FolderListUpdated != null)
                        FolderListUpdated(null, null);
                    ChangeValuesForDatabase();
                    RefreshData();
                };
            }

        }

        private void ChangeFolderClick(object param)
        {
            var ob = param as Tuple<object, string>;
            if (ob != null)
            {
                var parentFolderId = ob.Item2;

                if (ob.Item1 is SecureItemsView)
                {
                    MoveSecureItemToFolder((ob.Item1 as SecureItemsView).SelectedItems, parentFolderId);
                    return;
                }


                if (ob.Item1 is FolderView)
                {
                    var folderView = ob.Item1 as FolderView;
                    var folder = pbData.GetFolderById(folderView.uuid);
                    if (folder.Id == parentFolderId || folder.ParentId == parentFolderId)
                        return;
                    folder.ParentId = parentFolderId == null ? string.Empty : parentFolderId;
                    if (pbData.UpdateFolder(folder))
                    {
                        if (FolderListUpdated != null)
                            FolderListUpdated(null, null);
                        ChangeValuesForDatabase();
                        RefreshData();
                    }
                }

            }
        }

        private void SetAutoLoginClick(object param)
        {
            var ob = param as Tuple<object, string>;
            if (ob != null)
            {
                var setAutoLogin = bool.Parse(ob.Item2);

                if (ob.Item1 is FolderView)
                {
                    var list = (ob.Item1 as FolderView).GetAllSecureItems();
                    foreach (var site in list.Where(x => x.SubType == SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login))
                    {
                        site.Autologin = setAutoLogin;
                        SaveItem(site.CreateSecureItem());
                    }
                    //list.All(x=>x)
                }


            }
        }

        private void AddSecureItemClick(object param)
        {
            if (AddSecureItem != null)
                AddSecureItem(null, null);
        }

        private void SetCountriesValue(ISecureItemVM secureItem, bool setDefaulCountry = true)
        {
            SecureItemWithCountryViewModel countrySecureItem = secureItem as SecureItemWithCountryViewModel;
            if (countrySecureItem != null)
            {
                countrySecureItem.Countries = new ObservableCollection<Country>(pbData.GetCountryList());
                if (setDefaulCountry)
                    SetDefaultCountry(countrySecureItem);
            }

        }

        private void SetDefaultCountry(SecureItemWithCountryViewModel item)
        {
            string country = pbData.GetPrivateSetting(DefaultProperties.Settings_Country);
            if (String.IsNullOrEmpty(country))
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Country, DefaultProperties.Settings_DefaultCountryCode);
                country = DefaultProperties.Settings_DefaultCountryCode;
            }
            if (item != null && item.Countries != null)
                item.SelectedCountry = item.Countries.FirstOrDefault(x => x.Code == country);
        }

        private bool SaveItem(SecureItem secureItem)
        {
            try
            {

                //if (urlChanged && Url != null && _common.IsUrlValid(Url, uriKind: UriKind.Absolute))
                //{
                //     var siteId = pbData.GetSiteIdByUriFullSearch(new Uri(Url));
                //     secureItem.SiteData.Id = siteId;
                //}

                //bool updateSiteName = false;
                //if (string.IsNullOrEmpty(SiteName))
                //{
                //    updateSiteName = true;
                //}

                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Error while saving item");
                    }));
                    return false;
                }
                return true;

                //else
                //{
                //    //update shares

                //    ShareCommon shareCommon = new ShareCommon(resolver);
                //    shareCommon.UpdateShares(secureItem);

                //    if (SecureItem == null)
                //    {
                //        var pw = inAppAnalyitics.Get<Events.PasswordVault, PasswordVaultItem>();
                //        pw.Log(new PasswordVaultItem(SecureItemAction.Added, ApplicationSource.MainUI));
                //    }

                //    Task.Factory.StartNew(() =>
                //    {
                //        sync.RegisterSites();
                //        if (updateSiteName)
                //        {
                //            secureItem.Name = pbData.GetSiteById(secureItem.SiteData.Id).FriendlyName;
                //        }
                //        pbData.AddOrUpdateSecureItem(secureItem);
                //    });
                //}

            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show((string)Application.Current.FindResource("GeneralErrorText"));
                }));
                logger.Error(ex.Message);
            }
            return false;
        }



        public void UpdateTreeView()
        {
            var selectedFolder = FindTreeView(allHierarchyFolderList, ServiceLocator.Get<IFolderService>().SelectedFolderId);
            SecureItemList.All(x => x.ShowLastModifiedDate = _sortBySelectedIndex == 4);


            if (_sortBySelectedIndex != 6)
            {
                if (selectedFolder != null)
                    HierarchyFolderList = new ObservableCollection<FolderView>() { selectedFolder };
                else
                    HierarchyFolderList = new ObservableCollection<FolderView>(allHierarchyFolderList);

                SortTreeItemsCollection(HierarchyFolderList);
                HierarchyFolderList = new ObservableCollection<FolderView>(HierarchyFolderList.OrderBy(x => x, new CompareCommonViews()));

            }
            else
            {

                RebuildByTypeTreeView(selectedFolder == null ? SecureItemList : selectedFolder.GetAllSecureItems());
                HierarchyFolderList = new ObservableCollection<FolderView>(allHierarchyTypeList);
            }
        }


        private FolderView FindTreeViewWithDefault(IEnumerable<FolderView> folders, string targetFolderId)
        {
            foreach (var item in folders)
            {
                var folder = item as FolderView;
                if (folder != null)
                {
                    if (folder.uuid == targetFolderId)
                        return folder;
                    if (folder.ChildList.Any())
                    {
                        var child = FindTreeView(folder.ChildList, targetFolderId);
                        if (child != null)
                            return child;
                    }
                }

            }
            return folders.FirstOrDefault(x => x.uuid == string.Empty);
        }


        private FolderView FindTreeView(IEnumerable<object> folders, string targetFolderId)
        {
            foreach (var item in folders)
            {
                var folder = item as FolderView;
                if (folder != null)
                {
                    if (folder.uuid == targetFolderId)
                        return folder;
                    if (folder.ChildList.Any())
                    {
                        var child = FindTreeView(folder.ChildList, targetFolderId);
                        if (child != null)
                            return child;
                    }
                }

            }
            return null;
        }


        private void SortTreeItemsCollection(IEnumerable<CommonView> folders)
        {

            foreach (var item in folders)
            {
                var folder = item as FolderView;
                if (folder != null && folder.ChildList.Any())
                {
                    folder.ChildList = new ObservableCollection<CommonView>(folder.ChildList.OrderBy(x => x, new CompareCommonViews()));
                    folder.SecureItemsView.SecureList = new ObservableCollection<ISecureItemVM>(SortSecureItems(folder.SecureItemsView.SecureList));
                    SortTreeItemsCollection(folder.ChildList);
                }
            }

        }

        private IEnumerable<ISecureItemVM> SortSecureItems(IEnumerable<ISecureItemVM> list)
        {
            if (list == null)
                return null;

            switch (SortBySelectedIndex)
            {
                case 0:
                    return list.OrderBy(x => x.Name);
                case 1:
                    return list.OrderByDescending(x => x.Name);
                case 2:
                    return list.OrderByDescending(x => x.CreatedDate);
                case 3:
                    return list.OrderByDescending(x => x.Favorite);
                case 4:
                    return list.OrderByDescending(x => x.LastModifiedDate);
                case 5:
                    return list.OrderByDescending(x => x.LastAccess);

            }
            return list.OrderBy(x => x.Name);

        }

        public List<SecureItemViewModel> GetViewItems(bool recommendedsiteFlag)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IPBData pbData = resolver.GetInstanceOf<IPBData>();
            IPBWebAPI pbWebApi = resolver.GetInstanceOf<IPBWebAPI>();
            List<SecureItem> sites;
            List<SecureItemViewModel> passVaultItems = new List<SecureItemViewModel>();
            try
            {

                if ((sites = pbData.GetSecureItemsByItemType(SecureItemType)) != null)
                {
                    foreach (var site in sites)
                    {
                        if (site.Data == null)
                        {
                            logger.Error("GetSortedViewItems: site data is null");
                            continue;
                        }
                        var item = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == site.Type);
                        if (item != null)
                        {
                            var secureItemVM = Activator.CreateInstance(item.CreateItemType, site, item.BackgoundColor, item.Icon) as SecureItemViewModel;

                            passVaultItems.Add(secureItemVM);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            watch.Stop();
            logger.Info("SecureItemsHolderViewModel.GetViewItems> items got: {0}, executed in: {1} ms", passVaultItems.Count, watch.ElapsedMilliseconds);
            return passVaultItems;
        }

        public ISecureItemVM CreateItemForSearch(SecureItemSearchResult searchItem)
        {
            var item = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == searchItem.Type);
            if (item != null)
            {
                var secureItemVM = Activator.CreateInstance(item.CreateItemType, searchItem, item.BackgoundColor, item.Icon) as SecureItemViewModel;
                if (secureItemVM != null)
                {
                    secureItemVM.Edit_Clicked += EdiSearchItemClicked;
                    secureItemVM.DeletingIcon_Clicked += DeleteImage_Clicked;
                    return secureItemVM;
                }
            }
            return null;
        }

        public void EdiSearchItemClicked(object sender, EventArgs e)
        {
            var searchSecureItem = (sender as ISecureItemVM);
            if (searchSecureItem != null)
            {
                var secureItemVM = GetFullSecureItem(searchSecureItem.Id);
                if (secureItemVM != null)
                    Edit_Clicked(secureItemVM, null);
                
            }
        }

        private SecureItemViewModel GetFullSecureItem(string Id)
        {
            var secureItem = pbData.GetSecureItemById(Id);
            var item = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == secureItem.Type);
            if (item != null)
                return Activator.CreateInstance(item.CreateItemType, secureItem, item.BackgoundColor, item.Icon) as SecureItemViewModel;

            return null;

        }
    }

}
