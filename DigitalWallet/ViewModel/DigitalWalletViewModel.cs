using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Helpers;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using PasswordBoss.Model.DigitalWallet;
using PasswordBoss.DTO;
using System.Windows.Media.Animation;
using PasswordBoss.Views;
using System.Windows.Threading;
using System.Windows.Data;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views.UserControls;
using System.Collections.ObjectModel;
using PasswordBoss.UserControls;

namespace PasswordBoss.ViewModel.DigitalWallet
{
    public class DigitalWalletViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(DigitalWalletViewModel));
        private bool _recommendedsiteFlag;
        private IAnalytics<DigitalWalletItem> inAppAnalyitics;

        
        # region Object Declararion

        private int viewFlag = 0;
        private bool recommendedFlag;

        //References to WrapPanel and StackPanel of DigitalWallet ContentPanel.


        private DigitalWalletHelper _digitalWalletHelper;
        private DelegateCommand _delegate;
        //Class for creating DigitalWallet UI elements
        private PluginUIElement _digitalWalletElements;
        //List of UI elements
        private List<DefaultView> _items;
        private List<DefaultView> _gridItems;
        private List<DefaultView> _recommendedDigitalWalletItems;
        private IResolver resolver = null;
        Common _common = new Common();
        private IPBData pbData = null;
        # endregion

        public DigitalWalletViewModel( IResolver resolver)
        {
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.DigitalWallet, DigitalWalletItem>();
            _digitalWalletHelper = new DigitalWalletHelper(resolver);
          //  _addControlViewModel = new AddControlViewModel(resolver, panel.DigitalWalletAddEditControl, panel);
          //  _addControlViewModel.RefreshList += RefreshList;

            recommendedFlag = false;

            InitializeCommands();



            SecureItemList = new List<ISecureItemVM>();
            AddRecommendedItemHeaderVisibility = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_DigitalWallet) == 0 ? true : false;

            RefreshData();
        }

        public void RefreshData()
        {
            RefreshList(this, new RoutedEventArgs());
        }

        //private void RefreshList(object sender, RoutedEventArgs e)
        //{
        //    if (pbData.Locked) return;

        //    _recommendedsiteFlag = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_DigitalWallet) == 0 ? true : false;
        //    string preppendedTemplateName = "";
        //    if (_recommendedsiteFlag)
        //    {
        //        preppendedTemplateName = "Recommended";
        //        AddRecommendedItemHeaderVisibility = true;
        //    }
        //    else
        //    {
        //        AddRecommendedItemHeaderVisibility = false;
        //    }

        //    List<DefaultView> items = _digitalWalletHelper.GetSortedViewItems(SelectedSortIndex, _recommendedsiteFlag);
        //    SecureItemList.Clear();
        //    items.ForEach(x =>
        //    {
        //        //SecureItemList.Add(x);
        //        x.GearButton_Clicked += SettingImage_Clicked;
        //        x.FavoritesIcon_Clicked += FavoriteImage_Clicked;
        //        x.OpenInBrowser_Clicked += OpenBrowser_Clicked;
        //        x.SharingIcon_Clicked += ShareImage_Clicked;
        //    });

        //    SecureItemList = items;

        //    //switch (viewFlag)
        //    //{
        //    //    case 0:
        //    //        {
        //    //            if (SelectedSortIndex == 0) // group by categories
        //    //            {
        //    //                // SetupGridViewProperties(_gridWrappanel, _listStackpanel);
        //    //                //digitalWalletContentPanel.DigitalWalletItemsContainer.listView.View = (ViewBase)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemIconView"));
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.View = (ViewBase)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemGridView"));
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.Style = (Style)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemListViewGroupedWrapStyle"));
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.ItemTemplate = (DataTemplate)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoIconViewTemplate"));
        //    //            }
        //    //            else
        //    //            {
        //    //                // SetupGridViewProperties(_gridWrappanel, _listStackpanel);
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.View = (ViewBase)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemIconView"));
        //    //                //digitalWalletContentPanel.DigitalWalletItemsContainer.listView.View = (ViewBase)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemGridView"));
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.Style = (Style)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemListViewWrapStyle"));
        //    //                digitalWalletContentPanel.DigitalWalletItemsContainer.listView.ItemTemplate = (DataTemplate)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoIconViewTemplate"));
        //    //            }

        //    //            break;
        //    //        }
        //    //    case 1:
        //    //        {

        //    //            digitalWalletContentPanel.DigitalWalletItemsContainer.listView.Style = (Style)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemListViewStackStyle"));
        //    //            digitalWalletContentPanel.DigitalWalletItemsContainer.listView.View = (ViewBase)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource("SecureItemGridView"));
        //    //            digitalWalletContentPanel.DigitalWalletItemsContainer.listView.ItemTemplate = (DataTemplate)(digitalWalletContentPanel.DigitalWalletItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoSecureItemListViewTemplate"));
        //    //            break;
        //    //        }
        //    //    default:
        //    //        break;
        //    //}

        //    //if (SelectedSortIndex == 0)
        //    //{
        //    //    if (digitalWalletContentPanel.DigitalWalletItemsContainer.listView.GroupStyle.Count == 0)
        //    //    {
        //    //        Style _listViewGroupStyle = (Style)System.Windows.Application.Current.FindResource("ListViewGroupStyle");
        //    //        GroupStyle _newGroupStyle = new GroupStyle();
        //    //        _newGroupStyle.ContainerStyle = _listViewGroupStyle;
        //    //        digitalWalletContentPanel.DigitalWalletItemsContainer.listView.GroupStyle.Add(_newGroupStyle);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    digitalWalletContentPanel.DigitalWalletItemsContainer.listView.GroupStyle.Clear();
        //    //}

        //    System.Threading.Tasks.Task.Factory.StartNew(() =>
        //    {

        //        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        //        {
        //            if (true) //TODO: this should check shall we go to db again
        //            {



        //            }

        //          //  digitalWalletContentPanel.DigitalWalletItemsContainer.listView.Visibility = Visibility.Visible;
        //        }));
        //    });

        //}

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
            _recommendedsiteFlag = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_DigitalWallet) == 0 ? true : false;
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
                List<SecureItemViewModel> items = _digitalWalletHelper.GetViewItems(_recommendedsiteFlag);
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
                UpdateTreeView();

               

                lastCheckDate = DateTime.Now;
                lastSortIndex = SortBySelectedIndex;
                lastAccount = pbData.ActiveUser;
            }


            forceReloadFromDatabase = false;

        }

        private void AddNewFolder_Clicked(object sender, EventArgs e)
            {
            if (ServiceLocator.Get<IFolderService>().AddFolder())
                    {
                (sender as SecureItemViewModel).FoldersList = pbData.GetFoldersBySecureItemType();
            };
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

        void DeleteImage_Clicked(object sender, SecureItemRoutedEventArgs e)
                    {
            DeleteSelectedItemsClick(new List<ISecureItemVM> { sender as ISecureItemVM });
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
                    FolderName = type.Key,
                    IsExpanded = true

                };
                foreach (var item in type)
                    typeView.AddSecureItem(item);

                allHierarchyTypeList.Add(typeView);


            }
        }

        public void UpdateTreeView()
                        {
            var selectedFolder = FindTreeView(allHierarchyFolderList, ServiceLocator.Get<IFolderService>().SelectedFolderId);
            if (_sortBySelectedIndex != 6)
            {
                if (selectedFolder != null)
                    HierarchyFolderList = new ObservableCollection<FolderView>() { selectedFolder };
                else
                    HierarchyFolderList = new ObservableCollection<FolderView>(allHierarchyFolderList);

                SortTreeItemsCollection(HierarchyFolderList);
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


        #region Relay commands
        // public RelayCommand ItemSettingsCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }

        public RelayCommand DigitalWalletAddItemCommand { get; set; }
        public RelayCommand SortBySelectionChangedCommand { get; set; }
        public RelayCommand GearButtonCommand { get; set; }

        public RelayCommand ItemsGridGotFocusCommand { get; set; }

        public RelayCommand DeleteSelectedItemsCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> ConfirmedDeleteSelectedItemsCommand { get; set; }
        public RelayCommand CanceledDeleteSelectedItemsCommand { get; set; }

        #endregion



        #region Properties
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

        private ImageSource _gridViewIcon;

        public ImageSource GridViewIcon
        {
            get { return _gridViewIcon; }
            set
            {
                if (Equals(_gridViewIcon, value)) return;
                _gridViewIcon = value;
                RaisePropertyChanged("GridViewIcon");
            }
        }

        private ImageSource _listViewIcon;

        public ImageSource ListViewIcon
        {
            get { return _listViewIcon; }
            set
            {
                if (Equals(_listViewIcon, value)) return;
                _listViewIcon = value;
                RaisePropertyChanged("ListViewIcon");
            }
        }

        //private AddControlViewModel _addControlViewModel;
        //public AddControlViewModel AddControlViewModel
        //{
        //    get { return _addControlViewModel; }
        //    private set { _addControlViewModel = value; RaisePropertyChanged("AddControlViewModel"); }
        //}

        private int _selectedSortIndex = 1;

        public int SelectedSortIndex
        {
            get { return _selectedSortIndex; }
            set
            {
                if (Equals(_selectedSortIndex, value)) return;
                _selectedSortIndex = value;
            }
        }

        //private bool _digitalWalletAddControlVisibility;
        //public bool DigitalWalletAddControlVisibility
        //{
        //    get { return _digitalWalletAddControlVisibility; }
        //    set
        //    {

        //        if (!value && _addControlViewModel != null)
        //        {
        //            if (_addControlViewModel.CreditCardVisibility && _addControlViewModel.HasModelChanged())
        //            {
        //                if (_addControlViewModel.IsValid)
        //                    _addControlViewModel.CreditCardSettingsChangeDialogVisibility = true;
        //                else
        //                {
        //                    _addControlViewModel.IsValidErrorMessageVisible = true;
        //                    _addControlViewModel.SettingsChangeInvalidDialogVisibility = true;
        //                }
        //            }
        //            if (_addControlViewModel.BankAccountVisibility && _addControlViewModel.HasModelChanged())
        //            {
        //                if (_addControlViewModel.IsValid)
        //                    _addControlViewModel.BankAccountSettingsChangeDialogVisibility = true;
        //                else
        //                {
        //                    _addControlViewModel.IsValidErrorMessageVisible = true;
        //                    _addControlViewModel.SettingsChangeInvalidDialogVisibility = true;
        //                }
        //            }
        //        }
        //        if (!_addControlViewModel.SettingsChangeInvalidDialogVisibility)
        //        {
        //            _digitalWalletAddControlVisibility = value;
        //            RaisePropertyChanged("DigitalWalletAddControlVisibility");
        //        }



        //    }
        //}

        private bool _digitalWalletaddNewItemVisibility;
        public bool DigitalWalletAddNewItemVisibility
        {
            get { return _digitalWalletaddNewItemVisibility; }
            set
            {
                _digitalWalletaddNewItemVisibility = value;
                RaisePropertyChanged("DigitalWalletAddNewItemVisibility");
            }
        }

        //private bool _addNewItemGridVisibility;
        //public bool AddNewItemGridVisibility
        //{
        //    get { return _addNewItemGridVisibility; }
        //    set
        //    {
        //        _addNewItemGridVisibility = value;
        //        RaisePropertyChanged("AddNewItemGridVisibility");
        //        if(!value && _addControlViewModel != null)
        //        {
        //            if (_addControlViewModel.CreditCardVisibility && _addControlViewModel.HasModelChanged())
        //            {
        //                _addControlViewModel.CreditCardSettingsChangeDialogVisibility = true;
        //            }

        //            if (_addControlViewModel.BankAccountVisibility && _addControlViewModel.HasModelChanged())
        //            {
        //                _addControlViewModel.BankAccountSettingsChangeDialogVisibility = true;
        //            }
        //        }
        //    }
        //}

        private bool _itemsGridVisibility;
        public bool ItemsGridVisibility
        {
            get { return _itemsGridVisibility; }
            set
            {
                _itemsGridVisibility = value;
                RaisePropertyChanged("ItemsGridVisibility");
            }
        }


        private bool _gridViewTileWrapPanelVisibility;

        public bool GridViewTileWrapPanelVisibility
        {
            get { return _gridViewTileWrapPanelVisibility; }
            set
            {
                _gridViewTileWrapPanelVisibility = value;
                RaisePropertyChanged("GridViewTileWrapPanelVisibility");
            }
        }

        private bool _listViewStackPanelVisibility;

        public bool ListViewStackPanelVisibility
        {
            get { return _listViewStackPanelVisibility; }
            set
            {
                _listViewStackPanelVisibility = value;
                RaisePropertyChanged("ListViewStackPanelVisibility");
            }
        }

        #endregion

        #region otherMethods

        private void InitializeCommands()
        {
            SortBySelectionChangedCommand = new RelayCommand(SortBySelectionChanged);
            ItemsGridGotFocusCommand = new RelayCommand(ItemsGridGotFocus);
            DeleteItemCommand = new RelayCommand(DeleteItemClick);

            DeleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItemsClick);
            ConfirmedDeleteSelectedItemsCommand = new AsyncRelayCommand<LoadingWindow>(ConfirmedDeleteSelectedItemsClick, beforeExecute: (obj) => ConfirmedDeleteSelectedBefore(obj), completed: (obj) => ConfirmedDeleteSelectedItemsCompleted(obj));
            CanceledDeleteSelectedItemsCommand = new RelayCommand(CanceledDeleteSelectedItemsClick);
        }

        private void DeleteSelectedItemsClick(object parameter)
        {
            //var selected = digitalWalletContentPanel.DigitalWalletItemsContainer.listView.SelectedItems.Cast<DefaultView>();
            //if (selected != null && selected.Count() > 0)
            //{
            //    if (selected.Count() > 1)
            //    {
            //        DeleteSelectedItemsMessageText = (string)System.Windows.Application.Current.FindResource("DeleteSecureItemsMessageText");
            //    }
            //    else
            //    {
            //        DeleteSelectedItemsMessageText = (string)System.Windows.Application.Current.FindResource("DeleteSecureItemBody");
            //    }
            //    DeleteSelectedItemsMessageBoxVisibility = true;
            //}
            
        }
        private void CanceledDeleteSelectedItemsClick(object parameter)
        {
            DeleteSelectedItemsMessageBoxVisibility = false;
        }

        List<string> selectedIds;
        private void ConfirmedDeleteSelectedBefore(object parameter)
        {
            //var selected = digitalWalletContentPanel.DigitalWalletItemsContainer.listView.SelectedItems.Cast<DefaultView>();
            //selectedIds = new List<string>();
            //foreach (var sel in selected)
            //    selectedIds.Add(sel.Id);
        }
        private void ConfirmedDeleteSelectedItemsClick(object parameter)
        {
            bool logged = false;

            foreach (var id in selectedIds)
            {
                var selectedItem = pbData.GetSecureItemById(id);
                if (selectedItem != null)
                {
                    SecureItem secureItem = null;
                    selectedItem.Active = false;
                    if ((secureItem = pbData.AddOrUpdateSecureItem(selectedItem)) == null)
                    {
                        DeleteSelectedItemsMessageBoxVisibility = false;
                        MessageBox.Show("Error while saving item");
                    }
                    else
                    {

                        ShareCommon shareCommon = new ShareCommon(resolver);
                        shareCommon.UpdateShares(secureItem);
                    }

                    if (!logged)
                    {
                        logged = true;
                        if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_Paypal)
                        {
                            inAppAnalyitics.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.PayPal));
                        }

                        if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_CreditCard)
                        {
                            inAppAnalyitics.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.CreditCard));
                        }

                        if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_Bank)
                        {
                            inAppAnalyitics.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.BankAccount));
                        }
                    }
                }
            }
        }

        private void ConfirmedDeleteSelectedItemsCompleted(object parameter)
        {
            DeleteSelectedItemsMessageBoxVisibility = false;
           // digitalWalletContentPanel.DigitalWalletItemsContainer.listView.SelectedItems.Clear();
            RefreshData();

        }

        public void DeleteItemClick(object parameter)
        {
            string _id = string.Empty;
            if (parameter != null)
            {
                _id = parameter as string;
                SecureItem si = pbData.GetSecureItemById(_id);
                //AddControlViewModel.SecureItem = si;
                //AddControlViewModel.DeleteItem();
            }
        }

        public void ItemsGridGotFocus(object o)
        {
            //if (AddControlViewModel.HasBankAccountModelChanged() || AddControlViewModel.HasCreditCardModelChanged())
            //if(AddControlViewModel.HasModelChanged())
            //{
            //    if (AddControlViewModel.IsValid)
            //    {
            //        AddNewItemGridVisibility = false;
            //        DigitalWalletAddNewItemVisibility = false;
            //    }
            //}
            //else
            //{
            //    if(AddNewItemGridVisibility || DigitalWalletAddNewItemVisibility)
            //    {
            //        AddNewItemGridVisibility = false;
            //        DigitalWalletAddNewItemVisibility = false;
            //    }
            //}
            //if (_addControlViewModel.CreditCardVisibility && _addControlViewModel.HasCreditCardModelChanged())
            //{
            //    _addControlViewModel.CreditCardSettingsChangeDialogVisibility = true;
            //}

            //if (_addControlViewModel.BankAccountVisibility && _addControlViewModel.HasBankAccountModelChanged())
            //{
            //    _addControlViewModel.BankAccountSettingsChangeDialogVisibility = true;
            //}
        }




        private void OpenBrowser_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
            if (_recommendedsiteFlag)
            {
                if (e.ItemId != null)
                {
                    //_addControlViewModel.SecureItem = null;
                   // ApplyDigitalWalletItemVisibility(e.ItemId);
                }
            }
            else
            {
                //if (e.ItemId != null)
                //{
                //    AddControlViewModel.SecureItem = pbData.GetSecureItemById(e.ItemId);
                //}

            }

            //AddNewItemGridVisibility = true;
            //DigitalWalletAddNewItemVisibility = false;
            //DigitalWalletAddControlVisibility = true;

            ////Storyboard sb = digitalWalletContentPanel.FindResource("sbOpenEditNewItem") as Storyboard;
            //var uc = digitalWalletContentPanel.FindName("DigitalWalletAddEditControl") as UserControl;
            //Storyboard.SetTarget(sb, uc);
            //sb.Begin();
            }));
        }

        private void FavoriteImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_PasswordValt_ManageFavorites))
            {
                return;
            }

            //Code changes due to favorite grid and listView differences
            //GridView Favorite click
            if (sender != null)
            {
                var favButton = sender as Button;
                if (favButton.DataContext is DefaultView)
                {
                    var dc = favButton.DataContext as DefaultView;
                    if (dc.Favorite)
                    {
                        pbData.UpdateSecureItemFavorite(dc.Id, false);
                        favButton.Content = PluginUIElement.GetFavoriteImage(false);
                    }
                    else
                    {
                        pbData.UpdateSecureItemFavorite(dc.Id, true);
                        favButton.Content = PluginUIElement.GetFavoriteImage(true);
                    }
                }
            }
            //ListView Favorite click
            else
            {
                bool favorite = false;
                if (e.Data != null)
                    favorite = e.Data.Favorite;
                else
                {
                    SecureItem item = pbData.GetSecureItemById(e.ItemId);
                    favorite = item.Favorite;
                }

                if (favorite)
                {
                    pbData.UpdateSecureItemFavorite(e.ItemId, false);
                }
                else
                {
                    pbData.UpdateSecureItemFavorite(e.ItemId, true);
                }
            }
            if (SelectedSortIndex == 4)
                RefreshData();
        }

        public void EditItem(SecureItem item)
        {
            //if (item != null)
            //{
            //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            //    {
            //    _addControlViewModel.SecureItem = item;
            //    AddNewItemGridVisibility = true;
            //    DigitalWalletAddNewItemVisibility = false;
            //    DigitalWalletAddControlVisibility = true;

            //    //Storyboard sb = digitalWalletContentPanel.FindResource("sbOpenEditNewItem") as Storyboard;
            //    //var uc = digitalWalletContentPanel.FindName("DigitalWalletAddEditControl") as UserControl;
            //    //Storyboard.SetTarget(sb, uc);
            //    //sb.Begin();
            //    }));
            //}
        }
        public void ShareItem(SecureItem item)
        {
            if (item != null)
            {
                //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                //{
                //    _addControlViewModel.SecureItem = item;
                //    _addControlViewModel.SelectedIndexTabControl = 1;
                //    AddNewItemGridVisibility = true;
                //    DigitalWalletAddNewItemVisibility = false;
                //    DigitalWalletAddControlVisibility = true;

                //    //Storyboard sb = digitalWalletContentPanel.FindResource("sbOpenEditNewItem") as Storyboard;
                //    //var uc = digitalWalletContentPanel.FindName("DigitalWalletAddEditControl") as UserControl;
                //    //Storyboard.SetTarget(sb, uc);
                //    //sb.Begin();
                //}));
            }
        }
        void ShareImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            try
            {
                SecureItem item = pbData.GetSecureItemById(e.ItemId);
                //We can't share what is shared with us
                if(!item.Readonly)
                {
                    ShareItem(item);
                }
                

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void SettingImage_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            //todo read data from database
            try
            {
                SecureItem item = pbData.GetSecureItemById(e.ItemId);
                EditItem(item);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
            
        }


        private string GetItemImage(string itemType)
        {
            if (String.IsNullOrEmpty(itemType)) return "";
            switch (itemType)
            {
                case DefaultProperties.SecurityItemSubType_DW_Paypal:
                    return System.Windows.Application.Current.FindResource("23").ToString();
                case DefaultProperties.SecurityItemSubType_DW_Bank:
                    return System.Windows.Application.Current.FindResource("22").ToString();
                case DefaultProperties.SecurityItemSubType_DW_CreditCard:
                    return System.Windows.Application.Current.FindResource("21").ToString();
                default:
                    break;
            }
            return "";
        }

        private void SortBySelectionChanged(object sender)
        {
           // AddNewItemGridVisibility = false;
            DigitalWalletAddNewItemVisibility = false;
            
            RefreshData();

        }


        private void sortItems()
        {
            switch (SelectedSortIndex)
            {
                case 0:
                    _items = _items.OrderBy(x => x.Category).ToList();
                    _gridItems = _gridItems.OrderBy(x => x.Category).ToList();
                    break;
                case 1:
                    _items = _items.OrderBy(x => x.Name).ToList();
                    _gridItems = _gridItems.OrderBy(x => x.Name).ToList();
                    break;
                case 2:
                    _items = _items.OrderByDescending(x => x.Name).ToList();
                    _gridItems = _gridItems.OrderByDescending(x => x.Name).ToList();
                    break;
                case 3:
                    _items = _items.OrderByDescending(x => x.LastAccess).ToList();
                    _gridItems = _gridItems.OrderByDescending(x => x.LastAccess).ToList();
                    break;
                case 4: 
                    _items = _items.OrderByDescending(x => x.Favorite).ToList();
                    _gridItems = _gridItems.OrderByDescending(x => x.Favorite).ToList();
                    break;
                default:
                    break;
            }
        }

        public void MoveSecureItemToFolder(IEnumerable<object> items, string folderId)
        {
            if (items == null)
                return;
            var folder = pbData.GetFolderById(folderId);
            var temp = new List<object>(items);
            if (folder != null)
            {
                foreach (var it in temp)
                {
                    var item = it as SecureItemViewModel;
                    if (item != null)
                    {
                        var oldFolder = FindTreeViewWithDefault(allHierarchyFolderList, item.Folder.Id);
                        var folderView = FindTreeViewWithDefault(allHierarchyFolderList, folderId);

                        item.Folder = folder;
                        if (SaveItem(item.CreateSecureItem()))
                        {
                            if (oldFolder.SecureItemsView.SecureList.Contains(item))
                                oldFolder.SecureItemsView.SecureList.Remove(item);
                            if (folderView != null)
                                folderView.SecureItemsView.SecureList.Add(item);
                        }

                    }
                }
                // RefreshData();
                // folderView.IsExpanded = true;
            }
        }



        //private void ItemSettingsClick(object obj)
        //{
        //    string itemId = obj.ToString();
        //    DigitalWalletAddItemClick("1");

        //    MessageBox.Show(itemId);
        //}


        public void AddNewItem(ISecureItemVM secureItem)
        {
          
            AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["DWSecureItemTemplateSelector"] as DataTemplateSelector) { Title = secureItem.ItemTitel };
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
            AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["DWSecureItemTemplateSelector"] as DataTemplateSelector, true) { Title = secureItem.Name };
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


        private bool SaveItem(SecureItem secureItem)
        {
            try
        {

                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
        {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                        MessageBox.Show("Error while saving item");
                    }));
                    return false;
        }
                return true;

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


        #endregion

        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {
                return _digitalWalletHelper.SubItemsComponentTree;
            }
        }

    }
}
