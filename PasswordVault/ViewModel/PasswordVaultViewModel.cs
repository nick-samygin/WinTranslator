using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PasswordBoss.Views.UserControls;
using System.Windows.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PasswordBoss.PBAnalytics;
using System.Windows.Data;
using System.Threading.Tasks;
using PasswordBoss.UserControls;
using System.Collections;
using SecureItemsCommon.Helpers;

namespace PasswordBoss.ViewModel
{
    public class PasswordVaultViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PasswordVaultViewModel));
        /// <summary>
        /// Constant variables
        /// </summary>

        private int viewFlag = 0;

        private IAnalytics<PasswordVaultItem> inAppAnalyitics;

        /// <summary>
        /// for checking any data is present in item table related to password valut if not then this flag set to false.
        /// by default it is false
        /// </summary>
        private bool _recommendedsiteFlag;
        //public ObservableCollection<DefaultView> SecureItemList { get; set; }



        #region Relay commands

        public RelayCommand SortBySelectionChangedCommand { get; set; }
        public RelayCommand ItemsGridGotFocusCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand AddNewFolderCommand { get; set; }

        public RelayCommand DeleteSelectedItemsCommand { get; set; }
        public RelayCommand ConfirmedDeleteSelectedItemsCommand { get; set; }
        public RelayCommand CanceledDeleteSelectedItemsCommand { get; set; }


        #endregion

        #region Object Declararion
        private PasswordVaultHelper _passwordVaultHelper;
        Common _common = new Common();
        private IResolver resolver = null;
        private IPBData pbData = null;
        private IPBExtSecureBrowserBridge _pbExtSecureBrowserBridge;


        # endregion

        #region Properties

        // View model property for AddControl User control
        private AddControlViewModel _addControlViewModel;// = new AddControlViewModel();

        public AddControlViewModel AddNewItemViewModel
        {
            get { return _addControlViewModel; }
            private set
            {
                _addControlViewModel = value;
                RaisePropertyChanged("AddNewItemViewModel");
            }
        }

        public bool _showMenuTreeView;

        public bool ShowMenuTreeView
        {
            get { return _showMenuTreeView; }
            set
            {
                _showMenuTreeView = value;
                RaisePropertyChanged("ShowMenuTreeView");
            }
        }

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
                RaisePropertyChanged("IsLastModifiedState");
            }
        }

        public bool IsLastModifiedState
        {
            get
            {
                return _sortBySelectedIndex == 4;
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

        private bool _AddRecommendedItemHeaderVisibility;
        public bool AddRecommendedItemHeaderVisibility
        {
            get
            {
                return _AddRecommendedItemHeaderVisibility;
            }
            set
            {
                _AddRecommendedItemHeaderVisibility = value;
                RaisePropertyChanged("AddRecommendedItemHeaderVisibility");
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

        private ObservableCollection<ContextAction> actions=new ObservableCollection<ContextAction>();
        public ObservableCollection<ContextAction> Actions
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

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public PasswordVaultViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            _pbExtSecureBrowserBridge = resolver.GetInstanceOf<IPBExtSecureBrowserBridge>();
            pbData = resolver.GetInstanceOf<IPBData>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.PasswordVault, PasswordVaultItem>();

            _passwordVaultHelper = new PasswordVaultHelper(resolver);
            _addControlViewModel = new AddControlViewModel(resolver);

            _addControlViewModel.RefreshList += RefreshList;


            InitializeCommands();
            //TODO check if there is any passwords, if not _recommendedsiteFlag set true
            _recommendedsiteFlag = false;
            SecureItemList = new List<ISecureItemVM>();


            AddRecommendedItemHeaderVisibility = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_PasswordVault) == 0 ? true : false;
            RefreshData();

        }

        void ShareImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            try
            {
                SecureItem item = pbData.GetSecureItemById(e.ItemId);
                //We can't share what is shared with us
                if (!item.Readonly)
                {
                    ShareItem(item);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
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


        void OpenBrowser_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (_recommendedsiteFlag)
                {
                    var site = pbData.GetSiteById(e.ItemId);

                    if (site != null)
                    {
                        AddNewItemViewModel.SecureItem = null;
                        AddNewItemViewModel.DefaultView(url: site.Uri, categoryId: site.FolderId, siteName: site.FriendlyName);
                        // AddEditItemsVisibility = true;


                        //Storyboard sb = passwordVaultContentPanel.FindResource("sbOpenEditNewItem") as Storyboard;
                        //var uc = passwordVaultContentPanel.FindName("AddEditControl") as UserControl;
                        //Storyboard.SetTarget(sb, uc);
                        //sb.Begin();

                    }
                }
                else
                {
                    var item = pbData.GetSecureItemById(e.ItemId);
                    if (item != null)
                    {
                        if (item.Data != null && item.Data.use_secure_browser)
                        {
                            resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.InAppBrowser, bool>().Log(true);
                        }
                        if (_pbExtSecureBrowserBridge != null)
                        _pbExtSecureBrowserBridge.OneClickLogin(item.Id, false);
                    }

                }
            }));
        }

        private void FavoriteImage_Clicked(object sender, EventArgs e)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_PasswordValt_ManageFavorites))
            {
                return;
            }

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
            _recommendedsiteFlag = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_PasswordVault) == 0 ? true : false;
            string preppendedTemplateName = "";
            if (_recommendedsiteFlag)
            {
                preppendedTemplateName = "Recommended";
                AddRecommendedItemHeaderVisibility = true;
            }
            else
            {
                AddRecommendedItemHeaderVisibility = false;
            }
            if (forceReloadFromDatabase || SecureItemList == null || SecureItemList.Count == 0
                || lastSortIndex != SortBySelectedIndex || _recommendedsiteFlag || pbData.IsSecureItemOrSiteChanged(lastCheckDate)
                || lastAccount != pbData.ActiveUser) //TODO: We should check are there any changes in DB and if there are only then reload
            {
                List<SecureItemViewModel> items = _passwordVaultHelper.GetViewItems(_recommendedsiteFlag);
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

                SecureItemList =new List<ISecureItemVM>(items);
                RebuildFolderTreeView(items);
                UpdateTreeView();

               
                lastCheckDate = DateTime.Now;
                lastSortIndex = SortBySelectedIndex;
                lastAccount = pbData.ActiveUser;
            }


            forceReloadFromDatabase = false;

                        }

        private void RebuildFolderTreeView(List<SecureItemViewModel> items)
                        {
            var oldTree = allHierarchyFolderList;
            allHierarchyFolderList = new List<FolderView>();
            foreach (var item in pbData.GetFoldersBySecureItemType())
            {
                var previosFolderState = FindTreeView(oldTree, item.Id);
                allHierarchyFolderList.Add(new FolderView(item.Id, item.ParentId, item.Name, previosFolderState == null ? true : previosFolderState.IsExpanded) );

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
                    if (parent != null)
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
                    FolderName = type.Key,IsExpanded=true

                };
                foreach (var item in type)
                    typeView.AddSecureItem(item);

                allHierarchyTypeList.Add(typeView);


                    }
        }

        private void AddNewFolder_Clicked(object sender, EventArgs e)
        {
            if (ServiceLocator.Get<IFolderService>().AddFolder())
        {
                (sender as SecureItemViewModel).FoldersList = pbData.GetFoldersBySecureItemType();
            };
        }

        private void InitializeCommands()
        {
            SortByItems = new List<string>()
            {
                Application.Current.FindResource("AZSort").ToString(),
                Application.Current.FindResource("ZASort").ToString(),
                "Created date","Favorites","Last modified",
                Application.Current.FindResource("LastUsedAndroidSort").ToString(),
                Application.Current.FindResource("ItemFieldType").ToString()
            };
            SortBySelectedIndex = 0;

            //DeleteItemCommand = new RelayCommand(DeleteItemClick);
            DeleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItemsClick);
            ConfirmedDeleteSelectedItemsCommand = new RelayCommand(ConfirmedDeleteSelectedItemsClick);
            CanceledDeleteSelectedItemsCommand = new RelayCommand(CanceledDeleteSelectedItemsClick);

        }


        private IEnumerable<ISecureItemVM> selectedItems;
        private void DeleteSelectedItemsClick(object parameter)
        {
            if (parameter == null)
                return;

            var selected = parameter as IEnumerable<ISecureItemVM>;
            if (selected != null && selected.Count() > 0)
            {
                selectedItems = selected;
                if (selected.Count() > 1)
                {
                    DeleteSelectedItemsMessageText = (string)System.Windows.Application.Current.FindResource("DeleteSecureItemsMessageText");
                }
                else
                {
                    DeleteSelectedItemsMessageText = (string)System.Windows.Application.Current.FindResource("DeleteSecureItemBody");
                }
                DeleteSelectedItemsMessageBoxVisibility = true;
            }

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
                            inAppAnalyitics.Log(new PasswordVaultItem(SecureItemAction.Deleted, ApplicationSource.MainUI));

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
                var folder = FindTreeViewWithDefault(allHierarchyFolderList, item.Folder.Id);
                if (folder != null && folder.SecureItemsView != null && folder.SecureItemsView.SecureList.Contains(item))
                    folder.SecureItemsView.SecureList.Remove(item);
                //else
                //{
                //    var rootFolder = FindTreeView(allHierarchyFolderList, string.Empty);
                //    if (rootFolder != null && rootFolder.SecureList != null && rootFolder.SecureList.Contains(item))
                //        rootFolder.SecureList.Remove(item);
                //}

        }
        }

        public void MoveSecureItemToFolder(IEnumerable<object> items, string folderId)
        {
            if (items == null )
                return;
            var folder = pbData.GetFolderById(folderId);
            var temp = new List<object>(items);
            if(folder!=null )
            {
                foreach (var it in temp)
                {
                    var item = it as SecureItemViewModel;
                    if (item!=null)
                    {
                        var oldFolder = FindTreeViewWithDefault(allHierarchyFolderList, item.Folder.Id);
                        var folderView = FindTreeViewWithDefault(allHierarchyFolderList, folderId);

                        item.Folder = folder;
                        if (SaveItem(item.CreateSecureItem()))
        {
                            if(oldFolder.SecureItemsView.SecureList.Contains(item))
                                oldFolder.SecureItemsView.SecureList.Remove(item);
                            if(folderView!=null)
                                folderView.SecureItemsView.SecureList.Add(item);
                        }

                    }
                }
               // RefreshData();
               // folderView.IsExpanded = true;
            }
        }

        public void AddNewItem(ISecureItemVM secureItem)
        {

            AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["PVSecureItemTemplateSelector"] as DataTemplateSelector) { Title = secureItem.ItemTitel};
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
                if(SaveItem(secureItem.CreateSecureItem()))
                    RefreshData();
            }

        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            var secureItem = sender as ISecureItemVM;
            if (secureItem == null)
                return;
            secureItem.FoldersList = pbData.GetFoldersBySecureItemType();
            AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["PVSecureItemTemplateSelector"] as DataTemplateSelector, true) { Title = secureItem.Name };
            addWindow.DataContext = secureItem;
            addWindow.Closing += (o, args) =>
            {
                if (addWindow.DialogResult == null)
                    args.Cancel = true;

                if(addWindow.DialogResult.HasValue && addWindow.DialogResult.Value)
                    args.Cancel=!secureItem.Validate();
            };
            bool? dialogResult = addWindow.ShowDialog();
            if (dialogResult.Value)
        {
                if(SaveItem(secureItem.CreateSecureItem()))
            RefreshData();
        }
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


        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {
                return _passwordVaultHelper.SubItemsComponentTree;  
            }
        }

        public void UpdateTreeView()
        {
            var selectedFolder = FindTreeView(allHierarchyFolderList, ServiceLocator.Get<IFolderService>().SelectedFolderId);
            if (_sortBySelectedIndex != 6)
        {
                if (selectedFolder!=null)
                    HierarchyFolderList = new ObservableCollection<FolderView>() { selectedFolder };
                else
                    HierarchyFolderList = new ObservableCollection<FolderView>(allHierarchyFolderList);

                SortTreeItemsCollection(HierarchyFolderList);
        }
            else
        {

                RebuildByTypeTreeView(selectedFolder == null? SecureItemList : selectedFolder.GetAllSecureItems());
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
            return folders.FirstOrDefault(x=>x.uuid==string.Empty);
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


        private void SortTreeItemsCollection(IEnumerable<object> folders)
        {
            foreach (var item in folders)
            {
                var folder = item as FolderView;
                if (folder != null && folder.ChildList.Any())
                {
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
    }
}
