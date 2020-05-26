using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Helpers;
using System.Collections.ObjectModel;
using PasswordBoss.DTO;
using System.Diagnostics;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.ViewModel
{
    public class SecureBrowserViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecureBrowserViewModel));
        public IPBData PBData { get; set; }
        public IPBWebAPI PBWebAPI { get; set; }

        public ObservableCollection<TabItem> TabItemCollection { get; set; }

        public ObservableCollection<FavoriteListItemViewModel> FavoriteList { get; set; }
        public ObservableCollection<HomepageItem> HomepageItemList { get; set; }

        //public PageableViewModel<SecureItem> HomepageFavoriteModel { get; set; }
        //public ObservableCollection<SecureItem> HomepageFavoriteItems { get; set; }

        //public PageableViewModel<SecureItem> HomepageRecentlyUsedModel { get; set; }
        //public ObservableCollection<SecureItem> HomepageRecentlyUsedItems { get; set; }

        //public PageableViewModel<SecureItem> HomepageMostUsedModel { get; set; }
        //public ObservableCollection<SecureItem> HomepageMostUsedItems { get; set; }
        private bool _IsFavoriteListEmpty = false;
        public bool IsFavoriteListEmpty
        {
            get { return _IsFavoriteListEmpty; }
            set
            {
                _IsFavoriteListEmpty = value;
                RaisePropertyChanged("IsFavoriteListEmpty");
            }
        }


        private PortManager _portManager;


        #region Relay commands
        public RelayCommand SecureBrowserCommand { get; set; }
        public RelayCommand ShowOrHideFavoriteListCommand { get; set; }
        public RelayCommand SecureBrowserFavouriteCommand { get; set; }
        public RelayCommand FavoriteListDeleteCommand { get; set; }
        public RelayCommand SecureBrowserFavouriteListCloseButtonCommand { get; set; }
        public RelayCommand SecureBrowserPreviewCommand { get; set; }
        public RelayCommand RemoveSelectedTabCommand { get; set; }
        public RelayCommand RemoveFromFavoriteListCommand { get; set; }
        public RelayCommand OpenInCurrentTabCommand { get; set; }
        public RelayCommand ItemSettingsCommand { get; set; }
        public RelayCommand ItemFavoriteCommand { get; set; }
        public RelayCommand ItemOpenHomepageItemCommand { get; set; }


        public RelayCommand AddPressedCommand { get; set; }

        #endregion

        public void LoadSecureBrowserFavoriteList()
        {
            FavoriteList.Clear();

            var favoriteList = PBData.GetFavorites();

			if (favoriteList == null)
				return;

            foreach (var favorite in favoriteList)
            {
                FavoriteListItemViewModel item = new FavoriteListItemViewModel(this);
                item.Favorite = favorite;
                item.SiteImage = null; //force loading of favicon
                FavoriteList.Add(item);
            }

            if(favoriteList.Count == 0)
            {
                IsFavoriteListEmpty = true;
            }
            else
            {
                IsFavoriteListEmpty = false;
            }
        }

        public void LoadRecommendedSites()
        {
            HomepageItems.Clear();
            var recommendedSites = PBData.GetRecommendedSites();
            if (recommendedSites != null)
            {
                foreach (var site in recommendedSites)
                {
                    HomepageItems.Add(new SecureBrowserItem(site));
                }
            }

            HomepageModel = new PageableViewModel<SecureItem>(HomepageItems);
        }

        public void LoadFavoriteItems()
        {

            HomepageItems.Clear();
            var favoriteSecureItemsList = PBData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault).Where(x=>x.Favorite).ToList();

            if (favoriteSecureItemsList.Count == 0)
            {
                LoadRecentlyUsedItems();
            }
            foreach (var favoriteItem in favoriteSecureItemsList)
            {
                HomepageItems.Add(new SecureBrowserItem(favoriteItem));
                if (HomepageItems.Count() == 24) break;
            }
            HomepageModel = new PageableViewModel<SecureItem>(HomepageItems);
        }

        public void LoadMostUsedItems()
        {
            HomepageItems.Clear();

            var mostUsedItemList = PBData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault, SecureItemOrderEnum.AccessCount);
            foreach (var item in mostUsedItemList)
            {
                HomepageItems.Add(new SecureBrowserItem(item));
                if (HomepageItems.Count() == 24) break;
            }
            HomepageModel = new PageableViewModel<SecureItem>(HomepageItems);
        }

        public void LoadRecentlyUsedItems()
        {
            HomepageItems.Clear();

            var recentlyUsedItemList = PBData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault, SecureItemOrderEnum.LastAccess);

            foreach (var item in recentlyUsedItemList)
            {
                HomepageItems.Add(new SecureBrowserItem(item));
                if (HomepageItems.Count() == 24) break;
            }
            HomepageModel = new PageableViewModel<SecureItem>(HomepageItems);
        }



        public void LoadHomepageItemList()
        {
            HomepageItemList.Clear();
            if (PBData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault) != 0)
            {
                HomepageItem favoriteSites = new HomepageItem(this);
                favoriteSites.Name = System.Windows.Application.Current.FindResource("SecureBrowserFavoriteSites") as string;
                favoriteSites.Key = HomepageItem.FAVORITE_ITEM;

                HomepageItem recentlyUsed = new HomepageItem(this);
                recentlyUsed.Name = System.Windows.Application.Current.FindResource("SecureBrowserRecentlyUsed") as string;
                recentlyUsed.Key = HomepageItem.RECENTLY_USED_ITEM;

                HomepageItem mostUsed = new HomepageItem(this);
                mostUsed.Name = System.Windows.Application.Current.FindResource("SecureBrowserMostUsed") as string;
                mostUsed.Key = HomepageItem.MOST_USED_ITEM;

                HomepageItemList.Add(favoriteSites);
                HomepageItemList.Add(recentlyUsed);
                HomepageItemList.Add(mostUsed);

                var lastViewKey = PBData.GetPrivateSetting(HomepageItem.SB_LAST_VIEW_KEY);
                if (lastViewKey == null)
                {
                    SelectedHomepageItem = recentlyUsed;
                }
                else
                {
                    SelectedHomepageItem = HomepageItemList.Where(x=>x.Key == lastViewKey).FirstOrDefault();
                    if (SelectedHomepageItem == null)
                    {
                        SelectedHomepageItem = recentlyUsed;
                    }
                }
            }
           

            //LoadFavoriteItems();


            //var favoriteSecureItemsList = PBData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PasswordVault);
            //HomepageFavoriteItems.Clear();

            //foreach(var favoriteItem in favoriteSecureItemsList)
            //{
            //    HomepageFavoriteItems.Add(new SecureBrowserItem(favoriteItem));
            //}
            //HomepageFavoriteModel.CreatePaging();

            //var recentlyUsedItemList = PBData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PasswordVault, SecureItemOrderEnum.LastAccess);
            //HomepageRecentlyUsedItems.Clear();

            //foreach (var item in recentlyUsedItemList)
            //{
            //    HomepageRecentlyUsedItems.Add(new SecureBrowserItem(item));
            //}
            //HomepageRecentlyUsedModel.CreatePaging();

            //var mostUsedItemList = PBData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PasswordVault, SecureItemOrderEnum.AccessCount);
            //HomepageMostUsedItems.Clear();

            //foreach (var item in mostUsedItemList)
            //{
            //    HomepageMostUsedItems.Add(new SecureBrowserItem(item));
            //}
            //HomepageMostUsedModel.CreatePaging();

            
        }

        private IResolver resolver;

        public SecureBrowserViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            PBData = resolver.GetInstanceOf<IPBData>();
            PBWebAPI = resolver.GetInstanceOf<IPBWebAPI>();
            FavoriteList = new ObservableCollection<FavoriteListItemViewModel>();
            HomepageItemList = new ObservableCollection<HomepageItem>();

            HomepageItems = new ObservableCollection<SecureItem>();
            HomepageModel = new PageableViewModel<SecureItem>(HomepageItems);




            //HomepageFavoriteItems = new ObservableCollection<SecureItem>();
            //HomepageFavoriteModel = new PageableViewModel<SecureItem>(HomepageFavoriteItems);

            //HomepageRecentlyUsedItems = new ObservableCollection<SecureItem>();
            //HomepageRecentlyUsedModel = new PageableViewModel<SecureItem>(HomepageRecentlyUsedItems);

            //HomepageMostUsedItems = new ObservableCollection<SecureItem>();
            //HomepageMostUsedModel = new PageableViewModel<SecureItem>(HomepageMostUsedItems);
            TabItemCollection = new ObservableCollection<TabItem>();
            //->
            LoadSecureBrowserFavoriteList();

            InitialHomePageItemsLoad();
            
            _portManager = new PortManager(TabItemCollection, resolver);

            TabItem defaultItem = new TabItem(this, resolver);
            defaultItem.HideAllHompageContainerItems();
            defaultItem.NewTabButtonVisibility = true;


            //var startPage = PBData.GetPrivateSetting(DefaultProperties.Settings_StartPageUrl);
            //if(!string.IsNullOrWhiteSpace(startPage))
            //{
            //    //vedo - async
            //    System.Threading.Tasks.Task.Factory.StartNew(() =>
            //    {
            //        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            //        {
            //            defaultItem.SearchBar.Address = startPage;
            //        }));
            //    });
            //}
            //defaultItem.SearchBar.Address = "http://www.linkedin.com";
            TabItemCollection.Add(defaultItem);

            //<--

            RemoveFromFavoriteListCommand = new RelayCommand(RemoveFromFavoriteListClick);
            OpenInCurrentTabCommand = new RelayCommand(OpenInCurrentTabClick);
            SecureBrowserfavoriteListVisibility = false;

            InitializeCommands();

            /*
            LoadFavoriteList();
            LoadHomepageItemList();

            RemoveFromFavoriteListCommand = new RelayCommand(RemoveFromFavoriteListClick);
            OpenInCurrentTabCommand = new RelayCommand(OpenInCurrentTabClick);

            TabItemCollection = new ObservableCollection<TabItem>();
            _portManager = new PortManager(TabItemCollection, resolver);

            TabItem defaultItem = new TabItem(this, resolver);

            var startPage = PBData.GetPrivateSetting(DefaultProperties.Settings_StartPageUrl);
            if(!string.IsNullOrWhiteSpace(startPage))
            {
                defaultItem.SearchBar.Address = startPage;
            }
            //defaultItem.SearchBar.Address = "http://www.linkedin.com";
            TabItemCollection.Add(defaultItem);

            //TabItem ln = new TabItem(TabItemCollection);
            //ln.SearchBar.Address = "link";
            //TabItemCollection.Add(ln);
            //SelectedTabItem = defaultItem;

            //TabItem newTab = new TabItem(this);
            //TabItemCollection.Add(newTab);
            SecureBrowserfavoriteListVisibility = false;

            InitializeCommands();
            */

            _pbExtSecureBrowserBridge = resolver.GetInstanceOf<IPBExtSecureBrowserBridge>();
        }

        public void LoadStartPage()
        {
            if(TabItemCollection.Count == 1 && TabItemCollection[0].SearchBar.Address == null)
            {
                this.SelectedTabItem.AddNewTabClick(this);
            }
        }

        public void InitialHomePageItemsLoad()
        {
            if (PBData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault) != 0)
            {
                 LoadFavoriteItems();
            }
            else
            {
                LoadRecommendedSites();
            }
        }

        #region otherMethods

        public void OpenInCurrentTabClick(object obj)
        {
            if (obj != null)
            {
                var favorite = PBData.GetFavorites().Where(x => x.Url == obj.ToString()).FirstOrDefault();
                if (favorite != null && this.SelectedTabItem != null)
                {
                    this.SelectedTabItem.ShowOrHideFavoriteListClick(null);
                    this.SelectedTabItem.WebBrowser.Navigate(favorite.Url);
                }
                else if (favorite != null)
                {

                    TabItem defaultItem = new TabItem(this, resolver);
                    defaultItem.SearchBar.Address = favorite.Url;
                    TabItemCollection.Add(defaultItem);
                    
                }
            }
            else
            {
                this.SelectedTabItem.ShowOrHideFavoriteListClick(null);
            }
        }
        public void RemoveFromFavoriteListClick(object obj)
        {
            if (obj != null)
            {
                var favorite = PBData.GetFavorites().Where(x => x.Url == obj.ToString()).FirstOrDefault();
                if (favorite != null)
                {
                    this.PBData.RemoveFromFavorites(favorite);
                    var tabItem = TabItemCollection.Where(x => x.SearchBar.Address == favorite.Url).FirstOrDefault();
                    if (tabItem != null)
                    {
                        tabItem.SearchBar.SetFavoriteIconVisibility(false);
                    }
                }
                this.LoadSecureBrowserFavoriteList();
            }
        }

        private void InitializeCommands()
        {
            //SecureBrowserCommand = new RelayCommand(SecureBrowserClick);
            SecureBrowserFavouriteCommand = new RelayCommand(SecureBrowserFavouriteImageClick);
            FavoriteListDeleteCommand = new RelayCommand(FavoriteListDeleteClick);
            SecureBrowserFavouriteListCloseButtonCommand = new RelayCommand(SecureBrowserFavouriteListCloseButtonClick);
            SecureBrowserPreviewCommand = new RelayCommand(SecureBrowserPreviewClick);
            RemoveSelectedTabCommand = new RelayCommand(RemoveSelectedTabClick);
            ShowOrHideFavoriteListCommand = new RelayCommand(ShowOrHideFavoriteListClick);
            ItemSettingsCommand = new RelayCommand(ItemSettingsClick);
            ItemFavoriteCommand = new RelayCommand(ItemFavoriteClick);
            ItemOpenHomepageItemCommand = new RelayCommand(ItemOpenHomepageItemClick);
            AddPressedCommand = new RelayCommand(AddPressedClick);
        }

        #endregion


        #region Properties

        private ObservableCollection<SecureItem> _HomepageItems;
        public ObservableCollection<SecureItem> HomepageItems
        {
            get { return _HomepageItems; }
            set
            {
                _HomepageItems = value;
                RaisePropertyChanged("HomepageItems");
            }
        }

        private PageableViewModel<SecureItem> _HomepageModel;
        public PageableViewModel<SecureItem> HomepageModel
        {
            get { return _HomepageModel; }
            set
            {
                _HomepageModel = value;
                RaisePropertyChanged("HomepageModel");
            }
        }

        private TabItem _SelectedTabItem;
        public TabItem SelectedTabItem
        {
            get { return _SelectedTabItem; }
            set
            {
                _SelectedTabItem = value;
                RaisePropertyChanged("SelectedTabItem");
                if (value != null && SelectedHomepageItem != null)
                {
                    SelectedHomepageItem.OpenClick(null);
                }
            }
        }

        private HomepageItem _SelectedHomepageItem;
        public HomepageItem SelectedHomepageItem
        {
            get { return _SelectedHomepageItem; }
            set
            {
                _SelectedHomepageItem = value;
                RaisePropertyChanged("SelectedHomepageItem");
                if (value != null)
                {
                    SelectedHomepageItem.OpenClick(null);
                }
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        private bool _secureBrowserfavoriteListVisibility;
        public bool SecureBrowserfavoriteListVisibility
        {
            get { return _secureBrowserfavoriteListVisibility; }
            set
            {
                _secureBrowserfavoriteListVisibility = value;
                RaisePropertyChanged("SecureBrowserfavoriteListVisibility");
            }
        }

        private bool _deleteUrlGridVisibility;
        private IPBExtSecureBrowserBridge _pbExtSecureBrowserBridge;

        public bool DeleteUrlGridVisibility
        {
            get { return _deleteUrlGridVisibility; }
            set
            {
                _deleteUrlGridVisibility = value;
                RaisePropertyChanged("DeleteUrlGridVisibility");
            }
        }



        //public int IsPreviousButtonVisible
        //{
        //    get { return _isPreviousButtonVisible; }
        //    set
        //    {
        //        _isPreviousButtonVisible = value;
        //        RaisePropertyChanged("IsPreviousButtonVisible");
        //    }
        //}

        #endregion

        ///// <summary>
        /////  Secure Browser command function,
        /////  apply default properties to UI
        ///// </summary>
        ///// <param name="obj"></param>
        //private void SecureBrowserClick(object obj)
        //{
        //    SecureBrowserVisibility = true;
        //}


        private void ItemSettingsClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var id = obj as string;
                    var dictionary = new Dictionary<string, object> { { "id", id } };
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowSecureItemEditor", dictionary);
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        //To - Do Favorite click
        private void ItemFavoriteClick(object obj)
        {
            try
            {
                //SecureBrowserItem s = (SecureBrowserItem)HomepageItems.FirstOrDefault(x => x.Id == _itemId);
                if (obj != null)
                {
                    string _itemId = obj as string;
                    foreach (SecureBrowserItem s in HomepageItems)
                    {
                        if (s.Id == _itemId)
                        {
                            if (s != null)
                            {
                                if (s.ItemFavorite)
                                {
                                    PBData.UpdateSecureItemFavorite(s.Id, false);
                                    s.ItemFavorite = false;
                                }
                                else
                                {
                                    PBData.UpdateSecureItemFavorite(s.Id, true);
                                    s.ItemFavorite = true;
                                }
                            }
                        }
                    }
                    

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public enum OLECMDID
        {
            // ...
            OLECMDID_OPTICAL_ZOOM = 63,
            OLECMDID_OPTICAL_GETZOOMRANGE = 64,
            // ...
        }

        public enum OLECMDEXECOPT
        {
            // ...
            OLECMDEXECOPT_DONTPROMPTUSER,
            // ...
        }

        public enum OLECMDF
        {
            // ...
            OLECMDF_SUPPORTED = 1
        }


        public void AddPressedClick(object obj)
        {
            //if(SelectedTabItem != null)
            //{
            //    if(!SelectedTabItem.WebBrowser.Focused)
            //    {
            //       // Debugger.Launch();
            //       // object pvaIn = (int)50;
            //       // (SelectedTabItem.WebBrowser.ActiveXInstance as dynamic).ExecWB(OLECMDID.OLECMDID_OPTICAL_ZOOM,
            //       //OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
            //       //ref pvaIn, IntPtr.Zero);
            //    }
            //}
           
        }

        private void ItemOpenHomepageItemClick(object obj)
        {
            try
            {
                //SecureBrowserItem s = (SecureBrowserItem)HomepageItems.FirstOrDefault(x => x.Id == _itemId);
                if (obj != null)
                {
                    string _itemId = obj as string;
                    foreach (SecureBrowserItem s in HomepageItems)
                    {
                        if (s.Id == _itemId)
                        {
                            if (s != null)
                            {
                                if (this.SelectedTabItem != null)
                                {
                                    string uri = null;
                                    SecureItem item = PBData.GetSecureItemById(_itemId);
                                    if (item != null)
                                    {
                                        uri = item.Site.Uri;
                                    }
                                    else
                                    {
                                        Site siteItem = PBData.GetSiteById(_itemId);
                                        uri = siteItem.Uri;
                                    }
                                    
                                    if(uri != null)
                                    {
                                        this.SelectedTabItem.ShowOrHideHomepageClick(null);

                                        resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.InAppBrowser, bool>().Log(true);

                                        if(item != null)
                                        {
                                            _pbExtSecureBrowserBridge.OneClickLogin(_itemId, true, TabItemCollection.ToList().FindIndex(p => p.TabId == SelectedTabItem.TabId));
                                        }
                                        else
                                        {
                                            this.SelectedTabItem.WebBrowser.Navigate(uri);
                                        } 
                                    }
                                }
                                else
                                {
                                    SecureItem item = PBData.GetSecureItemById(_itemId);
                                    if (item != null)
                                    {
                                        TabItem defaultItem = new TabItem(this, resolver);
                                        String siteUri = item.Site.Uri;
                                        defaultItem.SearchBar.Address = siteUri;
                                        TabItemCollection.Add(defaultItem);
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        private void SecureBrowserFavouriteImageClick(object obj)
        {
            SecureBrowserfavoriteListVisibility = true;
        }

        private void FavoriteListDeleteClick(object obj)
        {
            DeleteUrlGridVisibility = true;
        }

        private void SecureBrowserFavouriteListCloseButtonClick(object obj)
        {
            DeleteUrlGridVisibility = false;
        }

        private void SecureBrowserPreviewClick(object obj)
        {
            SecureBrowserfavoriteListVisibility = false;
        }

        private void RemoveSelectedTabClick(object obj)
        {
            if (SelectedTabItem != null)
            {
                SelectedTabItem.WebBrowser.Dispose();
            }
        }

        private void ShowOrHideFavoriteListClick(object obj)
        {
            if (SelectedTabItem != null)
            {
                SelectedTabItem.ShowOrHideFavoriteListClick(obj);
            }
        }



        public class SecureBrowserItem : SecureItem, INotifyPropertyChanged
        {
            #region INotifyPropertyChanged implementation

            public event PropertyChangedEventHandler PropertyChanged;

            internal void RaisePropertyChanged(string prop)
            {
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
            }

            #endregion

            public SecureBrowserItem(Site site)
                : base()
            {
                if(site != null)
                {
                    ItemFavoriteIsVisible = false;
                    ItemSettingsIsVisible = false;
                    //ImagePath = null;
                    ImagePath = _common.GetImagePathForSite(site.UUID);
                }

                this.ItemId = site.Id;
                this.UUID = site.UUID;
                this.Id = site.Id;

            }
            Common _common = new Common();
            public SecureBrowserItem(SecureItem site) : base()
            {
                if (site != null)
                {
                    ImagePath = null;
                    

                    if (site.Site != null)
                    {
                        ImagePath = _common.GetImagePathForSite(site.Site.UUID);
                    }
                    else
                    {
                        ImagePath = _common.GetImagePathForSite(null);
                    }

                    this.ItemId = site.Id;
                    this.UUID = site.UUID;
                    this.ItemFavorite = site.Favorite;
                    this.Id = site.Id;
                    this.Favorite = site.Favorite;
                }

            }

            private bool _itemFavorite;
            public bool ItemFavorite
            {
                get { return _itemFavorite; }
                set
                {
                    _itemFavorite = value;
                    RaisePropertyChanged("ItemFavorite");
                }
            }

            private string _imagePath;
            public string ImagePath
            {
                get { return _imagePath; }
                protected set
                {
                    _imagePath = value;
                    RaisePropertyChanged("ImagePath");
                }
            }

            private string _itemId;
            public string ItemId
            {
                get { return _itemId; }
                protected set
                {
                    _itemId = value;
                    RaisePropertyChanged("ItemId");
                }
            }

            //ItemSettingsIsVisible
            //ItemFavoriteIsVisible
            private bool _ItemSettingsIsVisible = true;
            public bool ItemSettingsIsVisible
            {
                get { return _ItemSettingsIsVisible; }
                set
                {
                    _ItemSettingsIsVisible = value;
                    RaisePropertyChanged("ItemSettingsIsVisible");
                }
            }

            private bool _ItemFavoriteIsVisible = true;
            public bool ItemFavoriteIsVisible
            {
                get { return _ItemFavoriteIsVisible; }
                set
                {
                    _ItemFavoriteIsVisible = value;
                    RaisePropertyChanged("ItemFavoriteIsVisible");
                }
            }
        }
        
    }

}
