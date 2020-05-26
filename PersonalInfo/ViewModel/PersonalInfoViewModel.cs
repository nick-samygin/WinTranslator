using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Helpers;
using PasswordBoss.Model.PersonalInfo;
using PasswordBoss.DTO;
using System.Windows.Media.Animation;
using PasswordBoss.Views;
using System.Windows.Threading;
using System.Windows.Data;
using PasswordBoss.Views.UserControls;
using System.Threading.Tasks;
using PasswordBoss.PBAnalytics;
using PasswordBoss.UserControls;
using System.Collections.ObjectModel;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    internal class PersonalInfoViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PersonalInfoViewModel));
        private bool _recommendedsiteFlag;
        private IAnalytics<PersonalInfoItem> inAppAnalyitics;

        # region Object Declararion

        private int viewFlag = 0;
        private bool recommendedFlag;

        //References to WrapPanel and StackPanel of DigitalWallet ContentPanel.
        private StackPanel stackPanel;
        private WrapPanel wrapPanel;
        private PersonalInfoContentPanel personalInfoContentPanel;

        private PersonalInfoHelper _personalInfoHelper;
        private DelegateCommand _delegate;
        //Class for creating PersonalInfo UI elements
        private PluginUIElement _personalInfoElements;
        //List of UI elements
        private List<DefaultView> _items;
        private List<DefaultView> _gridItems;
        private List<DefaultView> _recommendedPersonalInfoItems;
        Common _common = new Common();
        private IResolver resolver = null;
        private IPBData pbData = null;

        # endregion

        public PersonalInfoViewModel(WrapPanel gridWrappanel, StackPanel listStackpanel, PersonalInfoContentPanel panel,IResolver resolver)
        {
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.PersonalInfo, PersonalInfoItem>();
            _personalInfoHelper = new PersonalInfoHelper(resolver);
            _personalInfoAddNewItemViewModel = new PersonalInfoAddNewItemViewModel(resolver, panel.PersonalInfoAddControl, panel);
            _personalInfoAddNewItemViewModel.RefreshList += RefreshList;

            //_recommendedPersonalInfoItems = new List<DefaultView>
            //{
            //    new DefaultView() { Id = "1", Image = System.Windows.Application.Current.FindResource("1").ToString(), Name = System.Windows.Application.Current.FindResource("Names").ToString() , Category = System.Windows.Application.Current.FindResource("Names").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "2", Image = System.Windows.Application.Current.FindResource("2").ToString(), Name = System.Windows.Application.Current.FindResource("Address").ToString() , Category = System.Windows.Application.Current.FindResource("Address").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "3", Image = System.Windows.Application.Current.FindResource("3").ToString(), Name = System.Windows.Application.Current.FindResource("PhoneNumbers").ToString() , Category = System.Windows.Application.Current.FindResource("PhoneNumbers").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "4", Image = System.Windows.Application.Current.FindResource("4").ToString(), Name = System.Windows.Application.Current.FindResource("Company").ToString() , Category = System.Windows.Application.Current.FindResource("Company").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "5", Image = System.Windows.Application.Current.FindResource("5").ToString(), Name = System.Windows.Application.Current.FindResource("Email").ToString() , Category = System.Windows.Application.Current.FindResource("Email").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "6", Image = System.Windows.Application.Current.FindResource("6").ToString(), Name = System.Windows.Application.Current.FindResource("DriverLicense").ToString() , Category = System.Windows.Application.Current.FindResource("DriverLicense").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "7", Image = System.Windows.Application.Current.FindResource("7").ToString(), Name = System.Windows.Application.Current.FindResource("Passport").ToString() , Category = System.Windows.Application.Current.FindResource("Passport").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "8", Image = System.Windows.Application.Current.FindResource("8").ToString(), Name = System.Windows.Application.Current.FindResource("MemberIDs").ToString() , Category = System.Windows.Application.Current.FindResource("MemberIDs").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "9", Image = System.Windows.Application.Current.FindResource("9").ToString(), Name = System.Windows.Application.Current.FindResource("SocialSecurity").ToString() , Category = System.Windows.Application.Current.FindResource("SocialSecurity").ToString() , Favorite = false, Username = null, LastAccess = null },
            //    new DefaultView() { Id = "10", Image = System.Windows.Application.Current.FindResource("10").ToString(), Name = System.Windows.Application.Current.FindResource("SecureNotes").ToString() , Category = System.Windows.Application.Current.FindResource("SecureNotes").ToString() , Favorite = false, Username = null, LastAccess = null },
            //};

            recommendedFlag = false;

            InitializeCommands();

            this.stackPanel = listStackpanel;
            this.wrapPanel = gridWrappanel;
            personalInfoContentPanel = panel;

            SecureItemList = new List<DefaultView>();
            AddRecommendedItemHeaderVisibility = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_DigitalWallet) == 0 ? true : false;

            ApplyVisibilityState();
            ApplyImageState(0);
            //DisplayData(wrapPanel, stackPanel);
            RefreshData();
        }
        public void RefreshData()
        {
            RefreshList(this, new RoutedEventArgs());
        }

        private void RefreshList(object sender, RoutedEventArgs e)
        {
            if (pbData.Locked) return;

            _recommendedsiteFlag = pbData.GetSecureItemCountByType(DefaultProperties.SecurityItemType_PersonalInfo) == 0 ? true : false;
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

            List<DefaultView> items = _personalInfoHelper.GetSortedViewItems(SelectedSortIndex, _recommendedsiteFlag);
            SecureItemList.Clear();
            items.ForEach(x =>
            {
                //SecureItemList.Add(x);
                x.GearButton_Clicked += SettingImage_Clicked;
                x.FavoritesIcon_Clicked += FavoriteImage_Clicked;
                x.OpenInBrowser_Clicked += _personalInfoElements_OpenBrowser_Clicked;
                x.SharingIcon_Clicked += ShareImage_Clicked;
            });

            SecureItemList = items;

            switch (viewFlag)
            {
                case 0:
                    {
                        if (SelectedSortIndex == 0) // group by categories
                        {
                            // SetupGridViewProperties(_gridWrappanel, _listStackpanel);
                            //personalInfoContentPanel.PersonalInfoItemsContainer.listView.View = (ViewBase)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemIconView"));
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.View = (ViewBase)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemGridView"));
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.Style = (Style)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemListViewGroupedWrapStyle"));
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.ItemTemplate = (DataTemplate)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoIconViewTemplate"));
                        }
                        else
                        {
                            // SetupGridViewProperties(_gridWrappanel, _listStackpanel);
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.View = (ViewBase)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemIconView"));
                            //personalInfoContentPanel.PersonalInfoItemsContainer.listView.View = (ViewBase)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemGridView"));
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.Style = (Style)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemListViewWrapStyle"));
                            personalInfoContentPanel.PersonalInfoItemsContainer.listView.ItemTemplate = (DataTemplate)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoIconViewTemplate"));
                        }

                        break;
                    }
                case 1:
                    {

                        personalInfoContentPanel.PersonalInfoItemsContainer.listView.Style = (Style)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemListViewStackStyle"));
                        personalInfoContentPanel.PersonalInfoItemsContainer.listView.View = (ViewBase)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource("SecureItemGridView"));
                        personalInfoContentPanel.PersonalInfoItemsContainer.listView.ItemTemplate = (DataTemplate)(personalInfoContentPanel.PersonalInfoItemsContainer.listView.TryFindResource(preppendedTemplateName + "DigitalWalletPersonalInfoSecureItemListViewTemplate"));
                        break;
                    }
                default:
                    break;
            }

            if (SelectedSortIndex == 0)
            {
                if (personalInfoContentPanel.PersonalInfoItemsContainer.listView.GroupStyle.Count == 0)
                {
                    Style _listViewGroupStyle = (Style)System.Windows.Application.Current.FindResource("ListViewGroupStyle");
                    GroupStyle _newGroupStyle = new GroupStyle();
                    _newGroupStyle.ContainerStyle = _listViewGroupStyle;
                    personalInfoContentPanel.PersonalInfoItemsContainer.listView.GroupStyle.Add(_newGroupStyle);
                }
            }
            else
            {
                personalInfoContentPanel.PersonalInfoItemsContainer.listView.GroupStyle.Clear();
            }

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (true) //TODO: this should check shall we go to db again
                    {

                        CollectionView myView = (CollectionView)CollectionViewSource.GetDefaultView(personalInfoContentPanel.PersonalInfoItemsContainer.listView.ItemsSource);
                        if (SelectedSortIndex == 0)
                        {
                            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Category");
                            myView.GroupDescriptions.Clear();
                            myView.GroupDescriptions.Add(groupDescription);
                            //SecureItemListGroupStyle
                            //if (passwordVaultContentPanel.PasswordVaultItemsContainer.listView.GroupStyle.Count == 0)
                            //{
                            //    GroupStyle g = new GroupStyle();
                            //    g.ContainerStyle = (Style)(passwordVaultContentPanel.PasswordVaultItemsContainer.listView.TryFindResource("SecureItemListViewStackStyle"));
                            //    passwordVaultContentPanel.PasswordVaultItemsContainer.listView.GroupStyle.Add(g);
                            //}

                        }
                        else
                        {
                            if (myView != null && myView.GroupDescriptions.Count != 0)
                            {
                                myView.GroupDescriptions.Clear();
                            }

                            //passwordVaultContentPanel.PasswordVaultItemsContainer.listView.GroupStyle.Clear();
                        }
                        //if (SecureItemList.Count == 0)
                        //{


                        //}

                    }

                    personalInfoContentPanel.PersonalInfoItemsContainer.listView.Visibility = Visibility.Visible;
                }));
            });
           
        }

        #region Relay commands
        public RelayCommand AddNewItemCommand { get; set; }
        public RelayCommand GridViewCommand { get; set; }
        public RelayCommand ListViewCommand { get; set; }
        public RelayCommand PersonalInfoAddItemCommand { get; set; }
        public RelayCommand SortBySelectionChangedCommand { get; set; }


        public RelayCommand ItemsGridGotFocusCommand { get; set; }
        public RelayCommand OpenBrowserCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }

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

        public List<DefaultView> secureItemList;

        public List<DefaultView> SecureItemList
        {
            get { return secureItemList; }
            set
            {
                secureItemList = value;
                RaisePropertyChanged("SecureItemList");
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

        private PersonalInfoAddNewItemViewModel _personalInfoAddNewItemViewModel;
        public PersonalInfoAddNewItemViewModel PersonalInfoAddNewItemViewModel
        {
            get { return _personalInfoAddNewItemViewModel; }
            private set { _personalInfoAddNewItemViewModel = value; RaisePropertyChanged("PersonalInfoAddNewItemViewModel"); }
        }

        private int _selectedSortIndex = 1;

        public int SelectedSortIndex
        {
            get { return _selectedSortIndex; }
            set
            {

                if (Equals(_selectedSortIndex, value)) return;
                _selectedSortIndex = value;
                RaisePropertyChanged("SelectedSortIndex");
                sortItems();
            }
        }

        private bool _personalInfoAddControlVisibility;
        public bool PersonalInfoAddControlVisibility
        {
            get { return _personalInfoAddControlVisibility; }
            set
            {
                _personalInfoAddControlVisibility = value;
                RaisePropertyChanged("PersonalInfoAddControlVisibility");
            }
        }

        private bool _personalInfoAddNewItemVisibility;
        public bool PersonalInfoAddNewItemVisibility
        {
            get { return _personalInfoAddNewItemVisibility; }
            set
            {

                if (!value && _personalInfoAddNewItemViewModel != null)
                {
                    if (_personalInfoAddNewItemViewModel.HasModelChanged())
                    {
                        if (_personalInfoAddNewItemViewModel.IsValid)
                            _personalInfoAddNewItemViewModel.SettingsChangeDialogVisibility = true;
                        else
                        {
                            _personalInfoAddNewItemViewModel.IsValidErrorMessageVisible = true;
                            _personalInfoAddNewItemViewModel.SettingsChangeInvalidDialogVisibility = true;
                        }
                    }
                }
                if (!_personalInfoAddNewItemViewModel.SettingsChangeInvalidDialogVisibility)
                {
                    _personalInfoAddNewItemVisibility = value;
                    RaisePropertyChanged("PersonalInfoAddNewItemVisibility");
                }


            }
        }

        private bool _addNewItemGridVisibility;
        public bool AddNewItemGridVisibility
        {
            get { return _addNewItemGridVisibility; }
            set
            {
                _addNewItemGridVisibility = value;
                RaisePropertyChanged("AddNewItemGridVisibility");
                if(!value && PersonalInfoAddNewItemViewModel != null)
                    if (PersonalInfoAddNewItemViewModel.HasModelChanged())
                    {
                        PersonalInfoAddNewItemViewModel.SettingsChangeDialogVisibility = true;
                    }
            }
        }



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
            AddNewItemCommand = new RelayCommand(AddNewItemClick);
            GridViewCommand = new RelayCommand(EnableGridView);
            ListViewCommand = new RelayCommand(EnableListView);
            PersonalInfoAddItemCommand = new RelayCommand(PersonalInfoAddItemClick);
            SortBySelectionChangedCommand = new RelayCommand(SortBySelectionChanged);

             ItemsGridGotFocusCommand = new RelayCommand(ItemsGridGotFocus);
             DeleteItemCommand = new RelayCommand(DeleteItemClick);
             OpenBrowserCommand = new RelayCommand(OpenBrowserClick);
             
             DeleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItemsClick);
             ConfirmedDeleteSelectedItemsCommand = new AsyncRelayCommand<LoadingWindow>(ConfirmedDeleteSelectedItemsClick, beforeExecute: (obj) => ConfirmedDeleteSelectedBefore(obj), completed: (obj) => ConfirmedDeleteSelectedItemsCompleted(obj));
             CanceledDeleteSelectedItemsCommand = new RelayCommand(CanceledDeleteSelectedItemsClick);
        }

        private void DeleteSelectedItemsClick(object parameter)
        {
            var selected = personalInfoContentPanel.PersonalInfoItemsContainer.listView.SelectedItems.Cast<DefaultView>();
            if (selected != null && selected.Count() > 0)
            {
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

        List<string> selectedIds;
        private void ConfirmedDeleteSelectedBefore(object parameter)
        {
            var selected = personalInfoContentPanel.PersonalInfoItemsContainer.listView.SelectedItems.Cast<DefaultView>();
            selectedIds = new List<string>();
            foreach (var sel in selected)
                selectedIds.Add(sel.Id);
        }

        private void ConfirmedDeleteSelectedItemsClick(object parameter)
        {
            if (selectedIds.Count > 0)
            {
                var itm = selectedIds.FirstOrDefault();
                var firstItem = pbData.GetSecureItemById(itm);

                if (firstItem != null)
                {
                    inAppAnalyitics.Log(new PersonalInfoItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DefaultProperties.GetPIEventTypeBySecureItemType(firstItem.Type)));
                }
            }

            Parallel.ForEach(selectedIds, x =>
            {
                var selectedItem = pbData.GetSecureItemById(x);
                if (selectedItem != null)
                {
                    SecureItem secureItem = null;
                    selectedItem.Active = false;
                    if ((secureItem = pbData.AddOrUpdateSecureItem(selectedItem)) == null)
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                           {
                               DeleteSelectedItemsMessageBoxVisibility = false;
                               MessageBox.Show("Error while saving item");
                               
                           }));
                        
                    }
                    else
                    {

                        ShareCommon shareCommon = new ShareCommon(resolver);
                        shareCommon.UpdateShares(secureItem);
                    }
                }
            });

        }

        private void ConfirmedDeleteSelectedItemsCompleted(object parameter)
        {
            DeleteSelectedItemsMessageBoxVisibility = false;
            personalInfoContentPanel.PersonalInfoItemsContainer.listView.SelectedItems.Clear();
            RefreshData();

        }

        public void DeleteItemClick(object parameter)
        {
            string _id = string.Empty;
            if (parameter != null)
            {
                _id = parameter as string;
                SecureItem si = pbData.GetSecureItemById(_id);
                _personalInfoAddNewItemViewModel.SecureItem = si;
                _personalInfoAddNewItemViewModel.DeleteItem();

            }
        }

        public void OpenBrowserClick(object parameter)
        {
            
        }

        public void ItemsGridGotFocus(object o)
        {
            if (_personalInfoAddNewItemViewModel.HasModelChanged())
            {
                if (_personalInfoAddNewItemViewModel.IsValid)
                {
                    AddNewItemGridVisibility = false;
                    PersonalInfoAddNewItemVisibility = false;
                }
            }
            else
            {
                if(AddNewItemGridVisibility || PersonalInfoAddNewItemVisibility)
                {
                    AddNewItemGridVisibility = false;
                    PersonalInfoAddNewItemVisibility = false;
                }
            }
            //if (PersonalInfoAddNewItemViewModel.HasModelChanged())
            //{
            //    PersonalInfoAddNewItemViewModel.SettingsChangeDialogVisibility = true;
            //}
        }

        /// <summary>
        /// Change view to list view after selecting on dash board List icon
        /// </summary>
        /// <param name="sender"></param>
        private void EnableListView(object sender)
        {
            AddNewItemGridVisibility = false;
            PersonalInfoAddNewItemVisibility = false;

            viewFlag = 1;
            RefreshData();
            ApplyImageState(1);
        }

        /// <summary>
        /// Change view to grid view after selecting on dash board grid icon
        /// </summary>
        /// <param name="sender"></param>
        private void EnableGridView(object sender)
        {
            AddNewItemGridVisibility = false;
            PersonalInfoAddNewItemVisibility = false;

            viewFlag = 0;
            RefreshData();
            ApplyImageState(0);
        }


        private void DisplayData(WrapPanel gridWrappanel, StackPanel listStackpanel)
        {
            SetupGridViewProperties(gridWrappanel, listStackpanel);
            ApplyImageState(0);
        }



        /// <summary>
        /// used to enable disble list & grid pannel according to  _listGridFlag
        /// </summary>
        /// <param name="listViewStackpanel"></param>
        /// <param name="gridViewWrapPanel"></param>
        private void SetupListViewProperties(StackPanel listViewStackpanel, WrapPanel gridViewWrapPanel)
        {
            gridViewWrapPanel.Children.Clear();
            gridViewWrapPanel.Visibility = Visibility.Collapsed;

            listViewStackpanel.Children.Clear();
            listViewStackpanel.Visibility = Visibility.Visible;
        }



        /// <summary>
        /// used to enable disble list & grid pannel according to  _listGridFlag
        /// </summary>
        private void SetupGridViewProperties(WrapPanel gridViewWrapPanel, StackPanel listViewStackpanel)
        {
            listViewStackpanel.Children.Clear();
            listViewStackpanel.Visibility = Visibility.Collapsed;

            gridViewWrapPanel.Children.Clear();
            gridViewWrapPanel.Visibility = Visibility.Visible;
        }

        public void addItemsToPanels(WrapPanel gridPanel, StackPanel listViewPanel)
        {
            //vedo - async
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        recommendedFlag = false;
                        List<SecureItem> items = pbData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PersonalInfo);
                        if(_personalInfoElements == null)
                        {
                            _personalInfoElements = new PluginUIElement();
                            _personalInfoElements.SettingImage_Clicked += SettingImage_Clicked;
                            _personalInfoElements.ShareImage_Clicked += ShareImage_Clicked;
                            _personalInfoElements.FavoriteImage_Clicked += FavoriteImage_Clicked;
                            _personalInfoElements.OpenBrowser_Clicked += _personalInfoElements_OpenBrowser_Clicked;
                        }
                        _items = new List<DefaultView>();
                        _gridItems = new List<DefaultView>();
                        if(items != null)
                        {
                            if (items.Count == 0)
                            {
                                _items = new List<DefaultView>(_recommendedPersonalInfoItems);
                                _gridItems = new List<DefaultView>(_recommendedPersonalInfoItems);
                                recommendedFlag = true;
                            }
                            string text2 = String.Empty;
                            string text1 = String.Empty;
                            foreach(var item in items)
                            {
                                text2 = String.Empty;
                                text1 = item.Name;
                                switch(item.Type)
                                {
                                    case DefaultProperties.SecurityItemSubType_PI_Address:
                                        text2 = item.Data.address1;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_PI_Company:
                                        text2 = String.Empty;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_SN_DriverLicense:
                                        text2 = item.Data.driverLicenceNumber;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_PI_Email:
                                        text2 = item.Data.email;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_SN_MemberIDs:
                                        text2 = item.Data.memberID;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_PI_Names:
                                        text2 = String.Empty;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_SN_Passport:
                                        text2 = item.Data.passportNumber;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_PI_PhoneNumber:
                                        text2 = item.Data.phoneNumber;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_PI_SecureNotes:
                                        text2 = String.Empty;
                                        break;
                                    case DefaultProperties.SecurityItemSubType_SN_SocialSecurity:
                                        text2 = String.Empty;
                                        break;
                                    default:
                                        text2 = String.Empty;
                                        break;
                                }
                                _gridItems.Add(new DefaultView() { Id = item.Id, Name = text1, Image = GetItemImage(item.Type), Category = item.Folder != null ? item.Folder.Name : "Other", Favorite = item.Favorite, Username = text2, LastAccess = item.LastAccess, shared = item.Share });
                                _items.Add(new DefaultView() { Id = item.Id, Name = item.Name, Image = GetItemImage(item.Type), Category = item.Folder != null ? item.Folder.Name : "Other", Favorite = item.Favorite, Username = text2, LastAccess = item.LastAccess, shared = item.Share });
                            }
                            sortItems();
                        }

                        if(gridPanel != null)
                            gridPanel.Children.Add(_personalInfoElements.makeNewDigitalWalletItemsForGrid(_gridItems, false, SelectedSortIndex, recommendedFlag));
                        if(listViewPanel != null)
                            listViewPanel.Children.Add(_personalInfoElements.makeNewRecommendedItemsForList(_items, false, SelectedSortIndex, recommendedFlag));
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                        logger.Error(ex.Message);
                    }

                }));
            });
        }

        void _personalInfoElements_OpenBrowser_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (_recommendedsiteFlag)
                {
                    if (e.ItemId != null)
                    {
                        ApplyPersonalInfoItemVisibility(e.ItemId);
                    }
                }
                else
                {
                    if (e.ItemId != null)
                    {
                        _personalInfoAddNewItemViewModel.SecureItem = pbData.GetSecureItemById(e.ItemId);
                    }
                }

                AddNewItemGridVisibility = true;
                PersonalInfoAddNewItemVisibility = false;
                PersonalInfoAddControlVisibility = true;

                Storyboard sb = personalInfoContentPanel.FindResource("sbOpenNewItem") as Storyboard;
                var uc = personalInfoContentPanel.FindName("PersonalInfoAddControl") as UserControl;
                Storyboard.SetTarget(sb, uc);
                sb.Begin();
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
            if(item != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                {
                _personalInfoAddNewItemViewModel.SecureItem = item;
                AddNewItemGridVisibility = true;
                PersonalInfoAddNewItemVisibility = false;
                PersonalInfoAddControlVisibility = true;

                Storyboard sb = personalInfoContentPanel.FindResource("sbOpenNewItem") as Storyboard;
                var uc = personalInfoContentPanel.FindName("PersonalInfoAddControl") as UserControl;
                Storyboard.SetTarget(sb, uc);
                sb.Begin();
                }));
            }
        }
        public void ShareItem(SecureItem item)
        {
            if (item != null)
            {

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    _personalInfoAddNewItemViewModel.SecureItem = item;
                    _personalInfoAddNewItemViewModel.SelectedIndexTabControl = 1;
                    AddNewItemGridVisibility = true;
                    PersonalInfoAddNewItemVisibility = false;
                    PersonalInfoAddControlVisibility = true;

                    Storyboard sb = personalInfoContentPanel.FindResource("sbOpenNewItem") as Storyboard;
                    var uc = personalInfoContentPanel.FindName("PersonalInfoAddControl") as UserControl;
                    Storyboard.SetTarget(sb, uc);
                    sb.Begin();
                }));
                
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
            SecureItem item = pbData.GetSecureItemById(e.ItemId);
            EditItem(item);
        }

        private string GetItemImage(string itemType)
        {

            if (String.IsNullOrEmpty(itemType)) return "";
            switch (itemType)
            {
                case DefaultProperties.SecurityItemSubType_PI_Names:
                    return System.Windows.Application.Current.FindResource("1").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Address:
                    return System.Windows.Application.Current.FindResource("2").ToString();
                case DefaultProperties.SecurityItemSubType_PI_PhoneNumber:
                    return System.Windows.Application.Current.FindResource("3").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Company:
                    return System.Windows.Application.Current.FindResource("4").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Email:
                    return System.Windows.Application.Current.FindResource("5").ToString();
                case DefaultProperties.SecurityItemSubType_SN_DriverLicense:
                    return System.Windows.Application.Current.FindResource("6").ToString();
                case DefaultProperties.SecurityItemSubType_SN_Passport:
                    return System.Windows.Application.Current.FindResource("7").ToString();
                case DefaultProperties.SecurityItemSubType_SN_MemberIDs:
                    return System.Windows.Application.Current.FindResource("8").ToString();
                case DefaultProperties.SecurityItemSubType_SN_SocialSecurity:
                    return System.Windows.Application.Current.FindResource("9").ToString();
                case DefaultProperties.SecurityItemSubType_PI_SecureNotes:
                    return System.Windows.Application.Current.FindResource("10").ToString();
                default:
                    break;
            }
            return "";
        }


        private void SortBySelectionChanged(object sender)
        {
            AddNewItemGridVisibility = false;
            PersonalInfoAddNewItemVisibility = false;

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


        private void AddNewItemClick(object obj)
        {
            _personalInfoAddNewItemViewModel.SecureItem = null;
            PersonalInfoAddControlVisibility = false;
            AddNewItemGridVisibility = true;
            if (PersonalInfoAddNewItemVisibility == false)
            {
                PersonalInfoAddNewItemVisibility = true;
            }
            else
            {
                PersonalInfoAddNewItemVisibility = false;
            }
        }

        public void AddNewItem(ISecureItemVM secureItem)
        {

            if (secureItem is SecureItemWithCountryViewModel)
            {
                if (secureItem is SecureItemWithCountryViewModel)
                {
                    ((SecureItemWithCountryViewModel)secureItem).Countries = new ObservableCollection<Country>(pbData.GetCountryList());

                    string country = pbData.GetPrivateSetting(DefaultProperties.Settings_Country);
                    if (String.IsNullOrEmpty(country))
                    {
                        pbData.ChangePrivateSetting(DefaultProperties.Settings_Country, DefaultProperties.Settings_DefaultCountryCode);
                        country = DefaultProperties.Settings_DefaultCountryCode;
                    }
                    ((SecureItemWithCountryViewModel)secureItem).SelectedCountry = ((SecureItemWithCountryViewModel)secureItem).Countries.FirstOrDefault(x => x.Code == country);
                }
            }

            AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["PISecureItemTemplateSelector"] as DataTemplateSelector) { Title = secureItem.ItemTitel };
            addWindow.DataContext = secureItem;
            bool? dialogResult = addWindow.ShowDialog();
            if (dialogResult.Value)
            {

            }

        }


        private void PersonalInfoAddItemClick(object parameter)
        {
            _personalInfoAddNewItemViewModel.SecureItem = null;
           
            AddNewItemGridVisibility = true;
            PersonalInfoAddNewItemVisibility = false;
            PersonalInfoAddControlVisibility = true;
            ApplyPersonalInfoItemVisibility(parameter);
            _personalInfoAddNewItemViewModel.NewItemImage = null;
        }

        private void ApplyImageState(int viewType)
        {
            switch (viewType)
            {
                case 0:
                    GridViewIcon = _personalInfoHelper.ReturnImageHover(1);
                    ListViewIcon = _personalInfoHelper.ReturnImage(2);
                    break;
                case 1:
                    GridViewIcon = _personalInfoHelper.ReturnImage(1);
                    ListViewIcon = _personalInfoHelper.ReturnImageHover(2);
                    break;
            }
        }

        internal void ApplyPersonalInfoItemVisibility(object number)
        {
            _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.HideVisibility);
            _delegate.Execute(number);
            _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.DefaultView);
            _delegate.Execute(number);
            switch ((string)number)
            {
                case "1":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setNameVisibility);
                    _delegate.Execute(number);
                    break;
                case "2":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setAddressVisibility);
                    _delegate.Execute(number);
                    break;
                case "3":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setPhoneVisibility);
                    _delegate.Execute(number);
                    break;
                case "4":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setCompanyVisibility);
                    _delegate.Execute(number);
                    break;
                case "5":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setEmailVisibility);
                    _delegate.Execute(number);
                    break;
                case "6":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setLicenseVisibility);
                    _delegate.Execute(number);
                    break;
                case "7":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setPassportVisibility);
                    _delegate.Execute(number);
                    break;
                case "8":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setMemberVisibility);
                    _delegate.Execute(number);
                    break;
                case "9":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setSocialSecurityVisibility);
                    _delegate.Execute(number);
                    break;
                case "10":
                    _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.setSecureNotesVisibility);
                    _delegate.Execute(number);
                    break;
            }

            _delegate = new DelegateCommand(PersonalInfoAddNewItemViewModel.Clear);
            _delegate.Execute(number);
        }

        private void ApplyVisibilityState()
        {
            ItemsGridVisibility = true;
            GridViewTileWrapPanelVisibility = true;
            ListViewStackPanelVisibility = false;
            PersonalInfoAddNewItemVisibility = false;
            AddNewItemGridVisibility = false;
            PersonalInfoAddControlVisibility = false;
        }

        #endregion
    }
}
