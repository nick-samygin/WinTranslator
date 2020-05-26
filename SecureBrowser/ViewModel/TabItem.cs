using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using PasswordBoss.PBAnalytics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading;

namespace PasswordBoss.ViewModel
{
    public class TabItem : ViewModelBase
    {
        private const string NewTabMessage = "NewTab";

        public SecureBrowserViewModel Model { get; set; }
        public ObservableCollection<TabItem> TabItemCollection { get; set; }

        private WebBrowserEx _webBrowser;
        public WebBrowserEx WebBrowser
        {
            get { return _webBrowser; }
            set { _webBrowser = value; RaisePropertyChanged("WebBrowserInitiated"); }
        } //NOTE: There is no support for MVVM for WinForms controls, so we need to create some ugly hacks

        public RelayCommand RemoveSelectedTabCommand { get; set; }
        public RelayCommand AddNewTabCommand { get; set; }
        public RelayCommand ShowFavoriteListCommand { get; set; }
        public RelayCommand HideFavoriteListCommand { get; set; }
        public RelayCommand SelectedFavoriteEditCommand { get; set; }
        public RelayCommand SelectedFavoriteHideEditDialogCommand { get; set; }
        public RelayCommand SelectedFavoriteSaveCommand { get; set; }
        public RelayCommand SelectedFavoriteDeleteCommand { get; set; }
        public RelayCommand ShowOrHideFavoriteListCommand { get; set; }
        public RelayCommand ShowOrHideHomepageCommand { get; set; }
        public RelayCommand RefreshBrowserCommand { get; set; }
        public RelayCommand PrintCommand { get; set; }

        
        public string TabId { get; set; }
        public bool IsNewTabTemplate { get; set; }

        private Visibility _IsWebBrowserContainerVisible = Visibility.Collapsed;
        public Visibility IsWebBrowserContainerVisible 
        {
            get { return _IsWebBrowserContainerVisible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    IsFavoriteListContainerVisible = Visibility.Collapsed;
                    IsHomepageContainerVisible = Visibility.Collapsed;
                }
                _IsWebBrowserContainerVisible = value;
                RaisePropertyChanged("IsWebBrowserContainerVisible");
            }
        }
        private Visibility _IsFavoriteListContainerVisible = Visibility.Collapsed;
        public Visibility IsFavoriteListContainerVisible 
        {
            get { return _IsFavoriteListContainerVisible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    IsWebBrowserContainerVisible = Visibility.Collapsed;
                    IsHomepageContainerVisible = Visibility.Collapsed;
                }
                _IsFavoriteListContainerVisible = value;
                RaisePropertyChanged("IsFavoriteListContainerVisible");
            }
        }
        
        private Visibility _IsHomepageContainerVisible = Visibility.Collapsed;
        public Visibility IsHomepageContainerVisible
        {
            get { return _IsHomepageContainerVisible; }
            set
            {
                if (value == Visibility.Visible)
                {
                    IsWebBrowserContainerVisible = Visibility.Collapsed;
                    IsFavoriteListContainerVisible = Visibility.Collapsed;
                }
                _IsHomepageContainerVisible = value;
                RaisePropertyChanged("IsHomepageContainerVisible");
            }
        }

        private Visibility _IsHomepageContainerFavoriteListVisible = Visibility.Visible;
        public Visibility IsHomepageContainerFavoriteListVisible
        {
            get { return _IsHomepageContainerFavoriteListVisible; }
            set
            {
                _IsHomepageContainerFavoriteListVisible = value;
                RaisePropertyChanged("IsHomepageContainerFavoriteListVisible");
            }
        }

        private Visibility _IsHomepageContainerRecentlyUsedListVisible = Visibility.Collapsed;
        public Visibility IsHomepageContainerRecentlyUsedListVisible
        {
            get { return _IsHomepageContainerRecentlyUsedListVisible; }
            set
            {
                _IsHomepageContainerRecentlyUsedListVisible = value;
                RaisePropertyChanged("IsHomepageContainerRecentlyUsedListVisible");
            }
        }

        private Visibility _IsHomepageContainerMostVisitedListVisible = Visibility.Collapsed;
        public Visibility IsHomepageContainerMostVisitedListVisible
        {
            get { return _IsHomepageContainerMostVisitedListVisible; }
            set
            {
                _IsHomepageContainerMostVisitedListVisible = value;
                RaisePropertyChanged("IsHomepageContainerMostVisitedListVisible");
            }
        }

        public void HideAllHompageContainerItems()
        {
            IsHomepageContainerMostVisitedListVisible = Visibility.Collapsed;
            IsHomepageContainerFavoriteListVisible = Visibility.Collapsed;
            IsHomepageContainerRecentlyUsedListVisible = Visibility.Collapsed;
        }
        
        private Visibility _CloseButtonVisibility = Visibility.Collapsed;
        public Visibility CloseButtonVisibility 
        {
            get { return _CloseButtonVisibility; }
            set
            {
                _CloseButtonVisibility = value;
                RaisePropertyChanged("CloseButtonVisibility");
                if(value == Visibility.Collapsed)
                {
                    NewTabButtonVisibility = true;
                }
                else if(value == Visibility.Visible)
                {
                    NewTabButtonVisibility = false;
                }
            }
        }

        private Visibility _SelectedFavoriteEditVisibility = Visibility.Collapsed;
        public Visibility SelectedFavoriteEditVisibility 
        {
            get { return _SelectedFavoriteEditVisibility; }
            set
            {
                _SelectedFavoriteEditVisibility = value;
                RaisePropertyChanged("SelectedFavoriteEditVisibility");
            }
        }

        private string _SelectedFavoriteName = "";
        public string SelectedFavoriteName 
        {
            get { return _SelectedFavoriteName; }
            set
            {
                _SelectedFavoriteName = value;
                RaisePropertyChanged("SelectedFavoriteName");
            }
        }

        private string _SelectedFavoriteUrl = "";
        public string SelectedFavoriteUrl 
        {
            get { return _SelectedFavoriteUrl; }
            set
            {
                _SelectedFavoriteUrl = value;
                RaisePropertyChanged("SelectedFavoriteUrl");
            }
        }

        private Favorite _SelectedFavorite = null;
        public Favorite SelectedFavorite
        {
            get { return _SelectedFavorite; }
            set
            {
                _SelectedFavorite = value;
                RaisePropertyChanged("SelectedFavorite");
            }
        }

        private bool _Navigating = false;
        public bool Navigating
        {
            get { return _Navigating; }
            set
            {
                _Navigating = value;
                RaisePropertyChanged("Navigating");
            }
        }


        private bool _NewTabButtonVisibility = true;
        public bool NewTabButtonVisibility
        {
            get { return _NewTabButtonVisibility; }
            set
            {
                _NewTabButtonVisibility = value;
                RaisePropertyChanged("NewTabButtonVisibility");
            }
        }

        private bool _CanGoForward = false;
        public bool CanGoForward
        {
            get { return _CanGoForward; }
            set
            {
                _CanGoForward = value;
                RaisePropertyChanged("CanGoForward");
            }
        }

        private bool _CanGoBack = false;
        public bool CanGoBack
        {
            get { return _CanGoBack; }
            set
            {
                _CanGoBack = value;
                RaisePropertyChanged("CanGoBack");
            }
        }


        private IResolver resolver;

        public TabItem(SecureBrowserViewModel model, IResolver resolver)
        {
            this.resolver = resolver;
            SearchBar = new TabItemSearchBar(this, resolver);
            RemoveSelectedTabCommand = new RelayCommand(RemoveSelectedTabClick);
            AddNewTabCommand = new RelayCommand(AddNewTabClick);
            ShowFavoriteListCommand = new RelayCommand(ShowFavoriteListClick);
            HideFavoriteListCommand = new RelayCommand(HideFavoriteListClick);
            SelectedFavoriteEditCommand = new RelayCommand(SelectedFavoriteEditClick);
            SelectedFavoriteHideEditDialogCommand = new RelayCommand(SelectedFavoriteHideEditDialogClick);
            ShowOrHideFavoriteListCommand = new RelayCommand(ShowOrHideFavoriteListClick);
            ShowOrHideHomepageCommand = new RelayCommand(ShowOrHideHomepageClick);
            SelectedFavoriteSaveCommand = new RelayCommand(SelectedFavoriteSaveClick);
            SelectedFavoriteDeleteCommand = new RelayCommand(SelectedFavoriteDeleteClick);
            RefreshBrowserCommand = new RelayCommand(RefreshBrowserClick);
            PrintCommand = new RelayCommand(PrintClick);

            Model = model;
            TabItemCollection = model.TabItemCollection;
            
            CloseButtonVisibility = Visibility.Collapsed;
            //TODO: Check here should we show homepage or redirect to search

            IsHomepageContainerVisible = Visibility.Visible;

            TabId = Guid.NewGuid().ToString();
        }
        
        public void RefreshBrowserClick(object obj)
        {
            if (!string.IsNullOrEmpty(SearchBar.Address))
            {
                WebBrowser.Navigate(SearchBar.Address);
            }
            //.Refresh(System.Windows.Forms.WebBrowserRefreshOption.Completely);
        }

        public void PrintClick(object obj)
        {
            WebBrowser.ShowPrintDialog();
        }
        public void AddPressedClick(object obj)
        {
            //Debugger.Launch();
        }
        

        public void ShowOrHideHomepageClick(object obj)
        {
            //if (IsHomepageContainerVisible != Visibility.Visible)
            //{
            //    IsHomepageContainerVisible = Visibility.Visible;
            //}
            //else
            //{
            //    var startPage = this.Model.PBData.GetPrivateSetting(DefaultProperties.Settings_StartPageUrl);
            //    if (!string.IsNullOrWhiteSpace(startPage))
            //    {
            //        SearchBar.Address = startPage;
            //        SearchBar.Navigate();
            //    }
            //    else
            //    {
            //        IsHomepageContainerVisible = Visibility.Visible;
            //    }

            //}
            var startPage = this.Model.PBData.GetPrivateSetting(DefaultProperties.Settings_StartPageUrl);
            if (!string.IsNullOrWhiteSpace(startPage))
            {
                SearchBar.Address = startPage;
                SearchBar.Navigate(); 
            }
            else
            {
                this.WebBrowser.Navigate("about:blank");
                
                SearchBar.Address = null;
                SearchBar.Title = (string)System.Windows.Application.Current.FindResource(NewTabMessage);
                SearchBar.SiteImage = null;
                IsHomepageContainerVisible = Visibility.Visible;
            }

        }

        

        public void ShowOrHideFavoriteListClick(object obj)
        {
            if(IsFavoriteListContainerVisible == Visibility.Visible)
            {
                HideFavoriteListClick(obj);
            }
            else
            {
                ShowFavoriteListClick(obj);
        }
        }

        private void HideFavoriteListClick(object obj)
        {
            //TODO: check if is address bar populated, show that, else show homepage
            IsWebBrowserContainerVisible = Visibility.Visible;
        }


        private void SelectedFavoriteSaveClick(object obj)
        {
            if(SelectedFavorite != null)
            {
                SelectedFavorite.Name = SelectedFavoriteName;
                SelectedFavorite.Url = SelectedFavoriteUrl;
                Model.PBData.UpdateFavorite(SelectedFavorite);
                SelectedFavoriteHideEditDialogClick(obj);
                this.Model.LoadSecureBrowserFavoriteList();
            }
            
        }

        private void SelectedFavoriteDeleteClick(object obj)
        {
            if(SelectedFavorite != null)
            {
                Model.PBData.RemoveFromFavorites(SelectedFavorite);
                SelectedFavoriteHideEditDialogClick(obj);
                this.Model.LoadSecureBrowserFavoriteList();
            }
            
        }
        

        private void SelectedFavoriteEditClick(object obj)
        {
            if (obj != null)
            {
                var favorite = Model.PBData.GetFavorites().Where(x => x.Url == obj.ToString()).FirstOrDefault();
               // Model.PBData.AddToFavorites()
                if(favorite != null)
                {
                    SelectedFavoriteEditVisibility = Visibility.Visible;
                    SelectedFavorite = favorite;
                    SelectedFavoriteName = favorite.Name;
                    SelectedFavoriteUrl = favorite.Url;
                }
            }
        }

        private void SelectedFavoriteHideEditDialogClick(object obj)
        {
            SelectedFavoriteEditVisibility = Visibility.Collapsed;
            SelectedFavorite = null;
            SelectedFavoriteName = null;
            SelectedFavoriteUrl = null;
        }

        private void ShowFavoriteListClick(object obj)
        {
            IsFavoriteListContainerVisible = Visibility.Visible;
        }
        public void RemoveSelectedTabClick(object obj)
        {
           // Debugger.Launch();
           // WebBrowser.Navigate("about:blank");
            if (WebBrowser != null)
            {
                WebBrowser.Dispose();
            }
            
            GC.Collect();
            TabItemCollection.Remove(this);
            
            if(TabItemCollection.Where(x=>x.CloseButtonVisibility== Visibility.Visible).Count() == 0)
            {
                AddNewTabClick(this);
                Model.SelectedIndex = 0;
                ShowAddNewTabIfNeeded();
            }
            else
            {
                Model.SelectedIndex = Model.TabItemCollection.Count - 2;
            }

            ShowAddNewTabIfNeeded();

        }

        private const int MAX_TAB_COUNT = 3;

        public void ShowAddNewTabIfNeeded()
        {
            int count = TabItemCollection.Where(x => x.NewTabButtonVisibility == true).Count();
            if (count == 0)
            {
                if (TabItemCollection.Count < MAX_TAB_COUNT)
                {
                    TabItem item = new TabItem(Model, resolver);
                    TabItemCollection.Add(item);
                }
            }

            if (TabItemCollection.Count > MAX_TAB_COUNT)
            {
                var tab = TabItemCollection.FirstOrDefault(x => x.NewTabButtonVisibility);
                TabItemCollection.Remove(tab);
            }
        }

        public void AddNewTabForUrl(string url, bool focusOnNewTab, Action<TabItem> callback = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                int _tabElements = TabItemCollection.Count;

                TabItem newTab = new TabItem(Model, resolver);
                newTab.SearchBar.Address = url;

                newTab.CloseButtonVisibility = Visibility.Visible;

                int count = TabItemCollection.Where(x => x.CloseButtonVisibility == Visibility.Visible).Count();

                if (count > 0)
                {
                    if (count >= MAX_TAB_COUNT)
                    {
                        var tab = TabItemCollection.LastOrDefault(x => x.CloseButtonVisibility == Visibility.Visible);
                        tab.SearchBar.Address = url;
                        tab.SearchBar.Navigate();
                    }
                    else
                    {
                        TabItemCollection.Insert(count, newTab);
                    }
                    
                }
                else
                {
                    TabItemCollection.Insert(0, newTab);
                }

                if (focusOnNewTab)
                {
                    if (TabItemCollection.Count > 1)
                    {
                        bool isNewButtonVisible = TabItemCollection.Any(x => x.NewTabButtonVisibility);
                        if(isNewButtonVisible)
                        {
                            Model.SelectedIndex = TabItemCollection.Count - 2;
                        }
                        else
                        {
                            Model.SelectedIndex = TabItemCollection.Count - 1;
                        }
                         
                    }
                    else
                    {
                        Model.SelectedIndex = 0;
                    }
                }

                if (callback != null)
                    callback(newTab);
            }

            ShowAddNewTabIfNeeded();
        }

        public void AddNewTabClick(object obj)
        {
            TabItem newTab = new TabItem(Model, resolver);
            newTab.SearchBar.Title = (string)System.Windows.Application.Current.FindResource(NewTabMessage);
            var startPage = this.Model.PBData.GetPrivateSetting(DefaultProperties.Settings_StartPageUrl);
            if (!string.IsNullOrWhiteSpace(startPage))
            {
                newTab.SearchBar.Address = startPage;
            }
            newTab.CloseButtonVisibility = Visibility.Visible;

            int count = TabItemCollection.Where(x => x.CloseButtonVisibility == Visibility.Visible).Count();

            if(count > 0)
            {
                if (!(TabItemCollection.Count > MAX_TAB_COUNT))
                {
                    TabItemCollection.Insert(count, newTab);
                }
            }
            else
            {
                TabItemCollection.Insert(0,newTab);
            }
            
            if(!(obj is TabItem))
            {
                Model.SelectedIndex = Model.TabItemCollection.Count - 2;
            }
            if(TabItemCollection.Count == 2)
            {
                Model.SelectedIndex = 0;
            }
           
            ShowAddNewTabIfNeeded();
        }

        public void AddNewTabEmpty(object obj)
        {
            if(TabItemCollection.Where(x=>x.NewTabButtonVisibility == true).Count()== 0)
            {
                TabItem newTab = new TabItem(Model, resolver);
                newTab.SearchBar.Title = (string)System.Windows.Application.Current.FindResource(NewTabMessage);
                HideAllHompageContainerItems();
                NewTabButtonVisibility = true;
                TabItemCollection.Add(newTab);
            }
            ShowAddNewTabIfNeeded();
        }
        

        public int Index { get; set; }

        public TabItemSearchBar SearchBar { get; set; }
    }

    public class TabItemSearchBar : ViewModelBase
    {
        //private IAnalytics<SecureBrowserAction> analytics;

        public delegate void OnNavigateRequiredHandler(TabItemSearchBar model);
        public event OnNavigateRequiredHandler OnNavigateRequired;

        public RelayCommand ShowAddressVerificationWarningCommand { get; set; }
        public RelayCommand AddToFavoriteListCommand { get; set; }
        public RelayCommand RemoveFromFavoriteListCommand { get; set; }
        public RelayCommand SecureSearchCommand { get; set; }
        
        public void Navigate()
        {
            if(OnNavigateRequired != null)
            {
                TabItem.ShowAddNewTabIfNeeded();
                //Address = url;
                OnNavigateRequired(this);
            }
        }
 
        public TabItem TabItem { get; set; }
        /* string title = Address.Replace("https://", "");
                    title = Address.Replace("http://", "");

                    return title.Substring(0, 20); */
        public TabItemSearchBar(TabItem tabItem, IResolver resolver)
        {
            TabItem = tabItem;
            _IsFocusedAddressBar = true;
            _AddressVerificationByDNSVisibility = Visibility.Collapsed;
            ShowAddressVerificationWarningCommand = new RelayCommand(ShowAddressVerificationWarningClick);
            AddToFavoriteListCommand = new RelayCommand(AddToFavoriteListClick);
            RemoveFromFavoriteListCommand = new RelayCommand(RemoveFromFavoriteListClick);
            SecureSearchCommand = new RelayCommand(SecureSearchClick);
            _IsInFavoriteListVisibility = Visibility.Collapsed;
            _IsNotInFavoriteListVisibility = Visibility.Collapsed;
            var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            //analytics = inAppAnalyitics.Get<Events.UsedSecureBrowser, SecureBrowserAction>();
        }

        public void ShowAddressVerificationWarningClick(object obj)
        {
            MessageBox.Show(string.Format(@"We couldn't verify address by secure DNS.
                            Try navigating to site directly by IP resolved by secure DNS: {0}", IPBySecureDNS));
        }

        public void AddToFavoriteListClick(object obj)
        {
            //analytics.Log(SecureBrowserAction.SavedFavorite);

            Favorite favorite = new Favorite();
            favorite.Url = Address;
            favorite.Name = _TitleFull;
            favorite.Active = true;
            favorite.Order = 1;
            this.TabItem.Model.PBData.AddToFavorites(favorite);
            SetFavoriteIconVisibility(true);
            this.TabItem.Model.LoadSecureBrowserFavoriteList();
        }

        public void RemoveFromFavoriteListClick(object obj)
        {
            //analytics.Log(SecureBrowserAction.DeletedFavorite);

            var favorite = this.TabItem.Model.PBData.GetFavorites().Where(x => x.Url == Address).FirstOrDefault();
            if(favorite != null)
            {
                this.TabItem.Model.PBData.RemoveFromFavorites(favorite);
            }
            this.TabItem.Model.LoadSecureBrowserFavoriteList();
            SetFavoriteIconVisibility(false);
        }

        public void SecureSearchClick(object obj)
        {
            
            if (!string.IsNullOrWhiteSpace(SecureSearchQuery))
            {
                var installationUUID = TabItem.Model.PBData.InstallationUUID;
                if (installationUUID != null)
                {
                    var searchProviderTemplate = TabItem.Model.PBData.GetPrivateSetting(DefaultProperties.Settings_SearchProviderUrl);
                    if (searchProviderTemplate != null)
                    {
                        //https://www.google.com/?gws_rd=ssl#q=[query]&uuid=[uuid]
                        searchProviderTemplate = searchProviderTemplate.Replace("[query]", SecureSearchQuery).Replace("[uuid]", installationUUID);
                        Address = searchProviderTemplate;
                        Navigate();
                        //analytics.Log(SecureBrowserAction.SearchBar);
                    }
                }
            }
        }
        

        public void SetFavoriteIconVisibility(bool isVisible)
        {
            if(isVisible)
            {
                IsNotInFavoriteListVisibility = Visibility.Collapsed;
                IsInFavoriteListVisibility = Visibility.Visible;
            }
            else
            {
                IsNotInFavoriteListVisibility = Visibility.Visible;
                IsInFavoriteListVisibility = Visibility.Collapsed;
            }
        }
        private bool _IsFocusedAddressBar;
        public bool IsFocusedAddressBar 
        {
            get { return _IsFocusedAddressBar; }
            set
            {
                _IsFocusedAddressBar = value;
                RaisePropertyChanged("IsFocusedAddressBar");
            }
        }

        private Visibility _AddressVerificationByDNSVisibility;
        public Visibility AddressVerificationByDNSVisibility
        {
            get { return _AddressVerificationByDNSVisibility; }
            set
            {
                if(_AddressVerificationByDNSVisibility != value)
                {
                    _AddressVerificationByDNSVisibility = value;
                    RaisePropertyChanged("AddressVerificationByDNSVisibility");
                }
            }
        }

        private Visibility _IsInFavoriteListVisibility;
        public Visibility IsInFavoriteListVisibility
        {
            get { return _IsInFavoriteListVisibility; }
            set
            {
                if (_IsInFavoriteListVisibility != value)
                {
                    _IsInFavoriteListVisibility = value;
                    RaisePropertyChanged("IsInFavoriteListVisibility");
                }
            }
        }

        private Visibility _IsNotInFavoriteListVisibility;
        public Visibility IsNotInFavoriteListVisibility
        {
            get { return _IsNotInFavoriteListVisibility; }
            set
            {
                if (_IsNotInFavoriteListVisibility != value)
                {
                    _IsNotInFavoriteListVisibility = value;
                    RaisePropertyChanged("IsNotInFavoriteListVisibility");
                }
            }
        }


        
        public string IPBySecureDNS { get; set; }

        private string _Title;
        private string _TitleFull;
        public string Title 
        {
            get { return _Title; }
            set
            {
                if(value != null && value.Length > 25)
                {
                    _Title = value.Substring(0, 25);
                }
                else
                {
                    _Title = value;
                }
                _TitleFull = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if(value == null || value == "about:blank") 
                {
                    TabItem.CloseButtonVisibility = Visibility.Visible;
                    TabItem.NewTabButtonVisibility = false;
                    SetFavoriteIconVisibility(false);
                    IsSecureBrowsing = Visibility.Hidden;
                    _Address = null;
                    RaisePropertyChanged("Address");
                }
                else if(_Address != value)
                {
                    //analytics.Log(SecureBrowserAction.TypedUrl);

                    _Address = value;
                    RaisePropertyChanged("Address");
                    //Title = Address;
                    TabItem.CloseButtonVisibility = Visibility.Visible;
                    TabItem.NewTabButtonVisibility = false;
                    TabItem.IsWebBrowserContainerVisible = Visibility.Visible;

                    //var pbWebApi =  resolver.GetInstanceOf<IPBWebAPI>();
                    //pbWebApi.GetFaviconAsync(Url).DownloadDataCompleted += FaviconDownloadCompleted;
                    this.TabItem.Model.PBWebAPI.GetFaviconAsync(Address).DownloadDataCompleted += TabItemSearchBar_DownloadDataCompleted;
                    var emptyTabCount = this.TabItem.TabItemCollection.Where(x => string.IsNullOrEmpty(x.SearchBar.Address)).Count();

                    if (this.TabItem.TabItemCollection.Count > 0 && emptyTabCount == 0)
                    {
                        //TabItem.AddNewTabEmpty(this);
                    }
                    if(_Address.ToLower().StartsWith("https://"))
                    {
                        IsSecureBrowsing = Visibility.Visible;
                    }
                    else
                    {
                        IsSecureBrowsing = Visibility.Hidden;
                    }
                    var favoriteList = this.TabItem.Model.PBData.GetFavorites();
                    if(favoriteList.Select(x=>x.Url).Contains(Address))
                    {
                        SetFavoriteIconVisibility(true);
                    }
                    else
                    {
                        SetFavoriteIconVisibility(false);
                    }
                }
                
            }
        }

        void TabItemSearchBar_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(e.Result);
                image.EndInit();
                SiteImage = image as ImageSource;
            }
            catch
            {
                SiteImage = null;
            }
        }

        private ImageSource _siteImage;
        public ImageSource SiteImage
        {
            get
            {
                return _siteImage;
            }
            set
            {
                _siteImage = value;
                RaisePropertyChanged("SiteImage");
            }
        }

        private Visibility _IsSecureBrowsing;
        public Visibility IsSecureBrowsing
        {
            get { return _IsSecureBrowsing; }
            set
            {
                _IsSecureBrowsing = value;
                RaisePropertyChanged("IsSecureBrowsing");
            }
        }

        private string _IsAddedToFavorites;
        public string IsAddedToFavorites
        {
            get { return _IsAddedToFavorites; }
            set
            {
                _IsAddedToFavorites = value;
                RaisePropertyChanged("IsAddedToFavorites");
            }
        }

        private string _SecureSearchQuery;
        public string SecureSearchQuery
        {
            get { return _SecureSearchQuery; }
            set
            {
                _SecureSearchQuery = value;
                RaisePropertyChanged("SecureSearchQuery");
            }
        }

        
    }
}
