using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Services;
using PasswordBoss.UserControls;
using PasswordBoss.ViewModel;
using PasswordBoss.ViewModel.AccountSettings;
using PasswordBoss.ViewModel.Search;
using PasswordBoss.Views.ApplicationSync;
using PasswordBoss.Views.InAppAdvertising;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PasswordBoss.Views
{
    public partial class MainWindow
    {
        //private MasterPwdBox masterDialog;

        private readonly IEnumerable<IUIComponent> components;
        private readonly IEnumerable<IUISubComponent> subComponents;
        public readonly IEnumerable<IDialog> dialogs;

        private AccountSettings.AccountSettings accountSettingsUserControl;
        private readonly WindowResizer resizer;
        private string selectedUIComponentID;
        private IPBData pbData = null;
        private IResolver resolver;
        private IPBSync sync;
        private IPBWebAPI webApi;
        public SearchViewModel searchViewModel = null;
        public FoldersTreeViewModel folderTreeViewModel = null;
        private SyncImagesHelper syncImagesHelper = null;
        //private int dialogOrder;
        public bool StopShowingDialogs = false;
        public event Action<string> OnMenuItemSelected;
        private Queue<IDialog> showOnStartup;
        private IPBExtSecureBrowserBridge _pbExtSecureBrowserBridge;

        //private IMessagingDialog dialog;

        //private static DateTime? LastSyncMessageShown = null;

        private static readonly ILogger logger = Logger.GetLogger(typeof(MainWindow));

        private readonly PBApp application;

        [ImportingConstructor]
        public MainWindow([Import(typeof(IResolver))] IResolver resolver)//[ImportMany] IEnumerable<IUIComponent> components, [Import(typeof(IPBData))]IPBData pbData)
        {
            Application.Current.MainWindow = this;
            application = (PBApp)Application.Current;
            //masterDialog = new MasterPwdBox();
            this.resolver = resolver;

     
            this.components = resolver.GetAllInstancesOf<IUIComponent>();
            if (this.components == null)
                this.components = new List<IUIComponent>();

            this.subComponents = resolver.GetAllInstancesOf<IUISubComponent>();
            if (this.subComponents == null)
            {
                this.subComponents = new List<IUISubComponent>();
            }

            this.dialogs = resolver.GetAllInstancesOf<IDialog>();
            if (this.dialogs == null)
                this.dialogs = new List<IDialog>();

            this.pbData = resolver.GetInstanceOf<IPBData>();
            ServiceLocator.Register<IFolderService>(new FolderService(pbData));

            this.webApi = resolver.GetInstanceOf<IPBWebAPI>();
            this.accountSettingsUserControl = new AccountSettings.AccountSettings(resolver) { Visibility = System.Windows.Visibility.Collapsed };
            this.accountSettingsUserControl.Loaded += accountSettingsUserControl_Loaded;
            this.searchViewModel = new SearchViewModel(resolver, SecureHolderCollection);
            searchViewModel.ChangeVisibility += (o, e) =>
            {
                topGrid.Visibility = searchViewModel.IsOpen ? Visibility.Collapsed : Visibility.Visible;
                var component = components.FirstOrDefault(x => x.ID == selectedUIComponentID);
                if(component!=null)
                    component.Selected = !searchViewModel.IsOpen;
             };

            folderTreeViewModel = new FoldersTreeViewModel(resolver);
            folderTreeViewModel.SelectedFolder_Changed += (o, e) => 
            {
                if (selectedUIComponentID != null)
                {
                    var component=components.FirstOrDefault(x => x.ID == selectedUIComponentID);
                    if (component!=null && component.ViewModel is ISecureHolder)
                    {
                        ((ISecureHolder)component.ViewModel).UpdateTreeView();
                    }
                }

            };
            folderTreeViewModel.FolderList_Changed += (o, e) => 
            {
                Reload();
            };
            folderTreeViewModel.ItemsToFolder_Moved += (o, e) => 
            {
                if (selectedUIComponentID != null)
                {
                    foreach (var item in e.Items.GroupBy(x=>(x as ISecureItemVM).Type))
                    {
                        var holder = SecureHolderCollection.FirstOrDefault(x => x.SecureItemType == item.Key);
                        if (holder != null)
                        {
                            holder.MoveSecureItemToFolder(item, e.FolderId);
                        }
                    }                       
                }
            };

            foreach (var holder in SecureHolderCollection)
            {
                holder.FolderListUpdated += (o, e) => { folderTreeViewModel.UpdateFolderTree(); };
                holder.DataUpdated += (o, e) => { searchViewModel.Update(); };
                holder.AddSecureItem += (o, e) => { addBtn_Click(null, null); };
            }

         
            syncImagesHelper = new SyncImagesHelper(pbData, webApi);
            //this.pbData.OnLoginToProfile(pbData_LoginToProfile);
            this.pbData.OnProfileLock += ProfileLock;
            this.pbData.OnUserLoggedIn += pbData_OnUserLoggedIn;

            Title = this.pbData.ActiveUser;

            resizer = new WindowResizer(this);
            InitializeComponent();

            LoadMenu();
            //LoadDialog(dialogOrder);
            Closing += OnMainClosing;
            sync = resolver.GetInstanceOf<IPBSync>();
            if (sync != null)
            {
                sync.OnSyncFinished += sync_OnSyncFinished;
                sync.OnSyncDeviceDeleted += sync_OnSyncDeviceDeleted;
                //sync.OnSyncSuccess += sync_OnSyncSuccess;
            }

            var advertControl = new MainWindowAdvertising(resolver);
            inAppAdvertisingGrid.Children.Add(advertControl);
            Grid.SetColumn(advertControl, 1);

            ucMainSearchBox.DataContext = searchViewModel;
            mainSearchResultPanel.DataContext = searchViewModel;
            selectFolderControl.DataContext = folderTreeViewModel;
            
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            this.ShowInTaskbar = true;
            LoadForShowOnStartup();
            if (_pbExtSecureBrowserBridge == null) _pbExtSecureBrowserBridge = resolver.GetInstanceOf<IPBExtSecureBrowserBridge>();
            this.Activated += MainWindow_Activated;
            SyncAsync();
            var test = this.FindName("menuUserControl") as MenuUserControl;

			this.DataContext = new MainWindowViewModel();
        }

		

        public void LoadForShowOnStartup()
        {
            if (showOnStartup != null && showOnStartup.Count() > 0)
            {
                showOnStartup.Clear();
            }

            showOnStartup = new Queue<IDialog>(dialogs.Where(x => x.ShowOnStartup == true).OrderBy(x => x.OrderNumber));
            currentDialog = null;
        }

        void MainWindow_Activated(object sender, EventArgs e)
        {
            Application.Current.MainWindow = this;
            DialogShowOnStartup();
        }

        private IDialog currentDialog = null;
        private bool showExtMsg = true;

        private void DialogShowOnStartup()
        {
            if (StopShowingDialogs) return;
            if (showOnStartup.Count > 0)
            {
                if (currentDialog == null)
                {
                    currentDialog = showOnStartup.Dequeue();

                    if (!(currentDialog is IProductTour))
                    {
                        currentDialog.DialogClosed += dialog_DialogClosed;
                        currentDialog.Show(this);
                    }
                    else
                    {
                        currentDialog = null;
                    }
                }
            }
            else if (showExtMsg)
            {
                try
                {
                    _pbExtSecureBrowserBridge.ShowChromeMessage();
                    showExtMsg = false;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        void pbData_OnUserLoggedIn(object o)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                var vm = accountSettingsUserControl.DataContext as AccountSettingsViewModel;
                if (vm != null)
                {
                    vm.RefreshData();
                }
                showExtMsg = true;

                var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == "SecureBrowser").SingleOrDefault();
                if (c != null)
                {
                    var result = c.ExecuteCommand("ReloadSecureBrowser", null);
                }

                foreach (var sc in subComponents)
                {
                    sc.NotifySubComponent("RefreshStats");
                }

                AccountSettingsClose();
                OpenDefaultMenuItem();
            }));
            SyncAsync();
            LoadForShowOnStartup();
        }

        private void SyncAsync()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                sync.Sync();
            });
        }

        public void UpdateAlertNotificationCount()
        {
            var dc = (AlertButtonViewModel)alertButton.DataContext;
            if (dc != null)
            {
                dc.UpdateAlertNotificationCount();
            }
        }

        public void UpdateAlertMessagesCount()
        {
            var dc = (SecurityNotificationViewModel)alertMessagesButton.DataContext;
            if (dc != null)
            {
                dc.UpdateAlertMessagesCount();
            }
        }

        void sync_OnSyncFinished(bool status)
        {
            logger.Debug("SyncFinished");
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                syncImagesHelper.SyncImages();

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Reload();
                }));
            });

        }

        void sync_OnSyncDeviceDeleted()
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                SystemTray.Logout();
                if (sync != null)
                {
                    sync.SyncNoUser();
                }
            }));
        }

        public void sync_OnSyncSuccess()
        {
            //logger.Debug("SyncSuccess");
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    this.Reload();
            //}));

            //if (!LastSyncMessageShown.HasValue || (DateTime.Now - LastSyncMessageShown.Value).TotalMinutes > 30)
            //{
            //    try
            //    {
            //        bool tmp = false;
            //        Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages), out tmp);

            //        if (tmp)
            //        {
            //            return;
            //        }

            //        if (dialog == null)
            //        {
            //            dialog = resolver.GetInstanceOf<IMessagingDialog>();
            //            if (dialog == null)
            //            {
            //                logger.Error("Failed to obtain reference to MessagingDialog");
            //                return;
            //            }
            //        }

            //        InAppMessage msg = new InAppMessage() { MessageID = "Toast-XS", MessageType = "Toast-XS", Theme = "Toast_XS", Body = System.Windows.Application.Current.FindResource("AccountSynced").ToString() };
            //        dialog.ShowSystemMessageDialog(msg);
            //    }
            //    catch (Exception exc)
            //    {
            //        logger.Error(exc.ToString());
            //    }

            //    LastSyncMessageShown = DateTime.Now;
            //}
        }

        void accountSettingsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var accountSettingsViewModel = this.TryFindResource("accountSettingsViewModel") as AccountSettingsViewModel;
            accountSettingsViewModel.resolver = resolver;
            this.accountSettingsUserControl.DataContext = accountSettingsViewModel;
        }

        //void pbData_LoginToProfile(CredentialsRequiredEventArgs obj)
        //{
        //    logger.Debug("pbData_LoginToProfile");
        //    Application.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        logger.Debug("pbData_LoginToProfile->Invoke");
        //        MasterPwdBox masterDialog = pbData.PinEnabled() ? new MasterPwdBox(true) : new MasterPwdBox();
        //        bool? res = masterDialog.ShowDialog() ?? false;
        //        if (res.Value)
        //        {
        //            if (pbData.PinEnabled())
        //            {
        //                obj.Password = pbData.GetMasterPwdFromPin(masterDialog.Password);
        //            }
        //            else
        //            {
        //                obj.Password = masterDialog.Password;
        //            }
        //        }
        //        else
        //        {
        //            obj.Cancel = true;
        //        }
        //    }));
        //}

        void ProfileLock(string eml)
        {
            logger.Debug("ProfileLock for {0}", eml);
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (this.Visibility == System.Windows.Visibility.Visible)
                {
                    SystemTray.Logout();
                }
            }));
        }

        public void Reload(Dictionary<string, object> parameters = null)
        {
            if (pbData.Locked)
            {
                return;
            }
            //vedo - async ?
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    bool _forceReloadFromDatabase = false;
                    if (parameters != null && parameters.ContainsKey("ForceReloadFromDatabase"))
                    {
                        object _tmp = parameters["ForceReloadFromDatabase"];
                        if (_tmp != null && _tmp is bool)
                        {
                            _forceReloadFromDatabase = (bool)_tmp;
                        }
                    }

                    foreach (var c in ContentPanels)
                    {
                        if (!c.Visible) continue;
                        if (c.ContentPanel != null && c.ContentPanel.DataContext != null)
                        {
                            try
                            {
                                if (_forceReloadFromDatabase)
                                {
                                    var _forceDatabase = c.ContentPanel.DataContext.GetType().GetMethod("ChangeValuesForDatabase");
                                    if (_forceDatabase != null) _forceDatabase.Invoke(c.ContentPanel.DataContext, null);
                                }


                                var refresh = c.ContentPanel.DataContext.GetType().GetMethod("RefreshData");
                                if (refresh != null) refresh.Invoke(c.ContentPanel.DataContext, null);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.Message);
                            }
                        }
                    }
                    RefreshSubcomponents();
                    UpdateAlertMessagesCount();
                    UpdateAlertNotificationCount();

                    bool _refreshDataForNewUser = false;
                    if (parameters != null && parameters.ContainsKey("NewUser"))
                    {
                        object _tmp = parameters["NewUser"];
                        if (_tmp != null && _tmp is bool)
                        {
                            _refreshDataForNewUser = (bool)_tmp;
                        }
                    }
                    if (_refreshDataForNewUser)
                    {
                        var vm = accountSettingsUserControl.DataContext as AccountSettingsViewModel;
                        if (vm != null)
                        {
                            try
                            {
                                vm.RefreshData();
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Main window reloading account settings: {0}", ex.ToString());
                            }
                        }
                    }
                }));
            });
        }

        public bool RefreshSubcomponents()
        {
            try
            {
                foreach (var s in subComponents)
                {
                    s.RefreshData();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return false;
        }

        public bool OpenView(string view, Dictionary<string, object> parameters = null)
        {
            switch (view)
            {
                case "MainSettings":
                    int? index = null;
                    if (parameters != null && parameters.ContainsKey("SelectedTabIndex"))
                    {
                        index = parameters["SelectedTabIndex"] as int?;
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        HandleAccountSettings(index.GetValueOrDefault(0));
                    }));
                    return true;
                case "ShowSecureItemEditor":
                    return ShowSecureItemEditor(parameters);
                case "ShowShareItemInShareCenter":
                    return ShowShareItemInShareCenter(parameters);
                case "OpenUrlInSecureBrowser":
                    return OpenUrlInSecureBrowser(parameters);
                case "OpenSecureBrowser":
                    return OpenSecureBrowser();
                case "StartBackupWithUI":
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("BackupNow", null);
                    break;
                case "StartBackupWithoutUI":
                    var vmb = accountSettingsUserControl.DataContext as AccountSettingsViewModel;
                    if (vmb != null) vmb.SyncDevicesData(true);
                    break;
                case "AddNewItemView":
                    addBtn_Click(addBtn, null);
                    break;
                case "Identities":
                    return OpenIdentities();

                default:
                    break;
            }

            return false;
        }

        private bool OpenUrlInSecureBrowser(Dictionary<string, object> parameters)
        {
            bool result = false;
            if (!(parameters != null && parameters.ContainsKey("url") && parameters["url"] != null)) return false;
            var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == "SecureBrowser").SingleOrDefault();
            if (c != null)
            {
                result = c.ExecuteCommand("OpenUrlInSecureBrowser", parameters);
                if (result)
                {
                    el_MenuButtonClick(c, new RoutedEventArgs());
                }
            }

            return result;
        }

        private bool OpenIdentities()
        {
            var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == "Identities").SingleOrDefault();

            if (c != null)
            {
                el_MenuButtonClick(c, new RoutedEventArgs());
                Show();
                Activate();
                return true;
            }

            return false;
        }

        private bool OpenSecureBrowser()
        {
            var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == "SecureBrowser").SingleOrDefault();

            if (c != null)
            {
                el_MenuButtonClick(c, new RoutedEventArgs());
                return true;
            }

            return false;
        }

        private bool ShowShareItemInShareCenter(Dictionary<string, object> parameters)
        {
            if (!(parameters != null && parameters.ContainsKey("id") && parameters["id"] != null)) return false;
            string id = (string)parameters["id"];
            var item = pbData.GetSharesByUuid(id);
            if (item == null) return false;
            string filter = "ShareCenter";
            var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == filter).SingleOrDefault();
            if (c == null) return false;
            el_MenuButtonClick(c, new RoutedEventArgs());
            Show();
            Activate();
            var p = new Dictionary<string, object>();
            p.Add("item", item);
            return c.ExecuteCommand("ShowShareItemList", p);
        }

        private bool ShowSecureItemEditor(Dictionary<string, object> parameters)
        {
            if (!(parameters != null && parameters.ContainsKey("id") && parameters["id"] != null)) return false;
            string id = (string)parameters["id"];
            var itm = pbData.GetSecureItemById(id);
            if (itm == null) return false;
            string filter = "";
            switch (itm.SecureItemTypeName)
            {
                case "PV":
                    filter = "PasswordVault";
                    break;
                case "DW":
                    filter = "DigitalWallet";
                    break;
                case "PI":
                    filter = "PersonalInfo";
                    break;
                default:
                    logger.Debug("Unknown secure item type");
                    return false;
            }
            var c = resolver.GetAllInstancesOf<IUIComponent>().Where(i => i.ID == filter).SingleOrDefault();
            if (c == null) return false;

            el_MenuButtonClick(c, new RoutedEventArgs());
            var p = new Dictionary<string, object>();
            p.Add("item", itm);
            // Added code to show Main window in foreground from extension

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
            Show();
            Activate();
            return c.ExecuteCommand("EditSecureItem", p);

        }

        private void OnMainClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        //private void LoadDialog(int currentDialog)
        //{
        //    if (StopShowingDialogs) return;
        //    bool _forceMenuClick = false;
        //    if (currentDialog <= dialogs.Count())
        //    {
        //        var newcurrentDialog = dialogs.Where(x => x.OrderNumber >= currentDialog && x.ShowOnStartup == true).OrderBy(x => x.OrderNumber).FirstOrDefault();
        //        if (dialogOrder == 0 && newcurrentDialog != null)
        //        {
        //            //initialize dialogOrder
        //            dialogOrder = newcurrentDialog.OrderNumber;
        //        }
        //        else if (dialogOrder == 0)
        //            dialogOrder = 1;
        //        if (newcurrentDialog != null)
        //        {
        //            newcurrentDialog.DialogClosed += dialog_DialogClosed;
        //            if (newcurrentDialog.ShowOnStartup)
        //                newcurrentDialog.Show(this);
        //        }
        //        //Force menu button selection for mini tour section
        //        else
        //        {
        //            _forceMenuClick = true;
        //        }

        //    }
        //    //Force menu button selection for mini tour section
        //    else
        //    {
        //        _forceMenuClick = true;
        //    }
        //    if(_forceMenuClick)
        //    {
        //        foreach (var menuButton in MenuButtons)
        //        {
        //            if (menuButton.Selected)
        //            {
        //                menuButton.Selected = true;
        //                return;
        //            }
        //        }
        //    }


        //}

        void dialog_DialogClosed(object arg1, RoutedEventArgs arg2)
        {
            Reload();
            //dialogOrder++;
            //LoadDialog(dialogOrder);
            currentDialog = null;
            DialogShowOnStartup();
        }

        private void LoadMenu()
        {
            var firstButton = MenuButtons.FirstOrDefault();
            foreach (var el in MenuButtons)
            {
                //el.Selected = false;
                el.MenuButtonClick += el_MenuButtonClick;
                MenuPanel.Children.Add(el.MenuButton);
                if (MenuPanel.Children.Count == 5)
                    MenuPanel.Children.Add(new Border() { Height = 0.5, Margin = new Thickness(0, 2, 0, 2), HorizontalAlignment = HorizontalAlignment.Stretch, Background = Application.Current.FindResource("LightGrayTextForegroundColor") as Brush });
                ContentPanel.Children.Add(el.ContentPanel);
            }
            foreach (var el in subComponents)
            {
                el.ContentPanel.Visibility = System.Windows.Visibility.Hidden;
                el.SubComponentAction += el_SubComponentAction;
                ContentSubPanel.Children.Add(el.ContentPanel);
            }

            ContentPanel.Children.Add(accountSettingsUserControl);

            if (firstButton != null)
                el_MenuButtonClick(firstButton, null);
        }


       

        void el_SubComponentAction(object arg1, RoutedEventArgs arg2, string arg3)
        {
            if (arg3 == "OpenAccountSecuritySettings")
            {
                AccountSettings_Click(arg1, arg2);
            }
        }

        private IEnumerable<IUIComponent> ContentPanels
        {
            get
            {
                return from c in components
                       where c.ContentPanel != null
                       select c;
            }
        }

        private IEnumerable<AddItem> AddItemsList
        {
            get
            {
                return from c in components
                       where c.ComponentTree != null
                       orderby c.MenuRank ascending
                       select c.ComponentTree;
            }
        }
        
        private IEnumerable<ISecureHolder> SecureHolderCollection
        {
            get
            {
                return components.Select(x => x.ViewModel).OfType<ISecureHolder>();
            }
        }

        private IEnumerable<IUIComponent> MenuButtons
        {
            get
            {
                return from e in components
                       where e.MenuButton != null
                       orderby e.MenuRank ascending
                       select e;
            }
        }

        void el_MenuButtonClick(object arg1, RoutedEventArgs arg2)
        {
            var comp = ((IUIComponent)arg1);
            selectedUIComponentID = comp.ID;

            HideAllComponents();

            if (accountSettingsUserControl.Visibility != System.Windows.Visibility.Visible)
            {
                var visibleSubComponents = subComponents.Where(p => p.UiComponentIDs.Contains(comp.ID)).ToList();

                var component = components.Where(p => p.ID.Contains(comp.ID)).FirstOrDefault();
                if (component != null)
                {
                    if (viewByTopPanel.DataContext != null && component.ViewModel is ISecureHolder)
                    {

                        var secureHolder = topPanelTextBox.DataContext as ISecureHolder;
                        if (secureHolder != null)
                        {
                            ((ISecureHolder)component.ViewModel).IsTileView = secureHolder.IsTileView;
                            ((ISecureHolder)component.ViewModel).SortBySelectedIndex = secureHolder.SortBySelectedIndex;
                            ((ISecureHolder)component.ViewModel).ExpandAll = secureHolder.ExpandAll;
                        }
                    }
                    topPanelTextBox.Titel = component.Header;
                    viewByTopPanel.DataContext = component.ViewModel;//component.TopPanel;
                    viewByTopPanel.Visibility = component.ViewModel is ISecureHolder ? Visibility.Visible : Visibility.Collapsed;
                }

                foreach (var c in visibleSubComponents)
                {
                    c.ContentPanel.Visibility = System.Windows.Visibility.Visible;
                }

                comp.Selected = true;
            }

            if (OnMenuItemSelected != null)
            {
                OnMenuItemSelected(comp.ID);
            }
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            resizer.DisplayResizeCursor(sender);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            resizer.ResetCursor();
        }

        private void Resize(object sender, MouseEventArgs e)
        {
            resizer.ResizeWindow(sender);
        }

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    PART_MAXIMIZE_RESTORE_Click(sender, e);
                }
                else
                {
                    DragMove();
                }
            }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            //this.Hide();
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            ((PBApp)Application.Current).Restore(this);
        }

        public void MenuSetFocus()
        {
            LeftMenuExpander.Focus();
        }

        private void HandleAccountSettings(int selectedIndex = 0, bool switchVisibility = false)
        {
            try
            {
               var settingsBtn= MenuButtons.FirstOrDefault(x => x.ID == "Settings");
                if (settingsBtn != null)
                    el_MenuButtonClick(settingsBtn, null);

                //IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                //if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_AccountManagement))
                //{
                //    return;
                //}
                //if (accountSettingsUserControl.Visibility == System.Windows.Visibility.Visible && switchVisibility == true)
                //{
                //    MenuSetFocus();
                //    return;
                //}

                //HideAllComponents();

                //if (OnMenuItemSelected != null)
                //{
                //    OnMenuItemSelected("Settings");
                //}

                //// If Expander is not expanded 
                //if (!LeftMenuExpander.IsExpanded)
                //{
                //    OpenMenuExpander(new Dictionary<string, object> { { "ShowOrHide", false } });
                //}

                //if (accountSettingsUserControl.Visibility != System.Windows.Visibility.Visible)
                //{
                //    (this.accountSettingsUserControl.DataContext as AccountSettingsViewModel).InitializeSettingsData();
                //}

                //accountSettingsUserControl.Visibility = System.Windows.Visibility.Visible;

                //var buttonImage = this.FindName("imgAccountSettings") as Image;
                //buttonImage.Source = (ImageSource)Application.Current.FindResource("imgAccountSettingHover");
                //var downArrowImage = this.FindName("AccountSettingsDownArrow") as Image;
                //downArrowImage.Source = (ImageSource)Application.Current.FindResource("imgDownArow");

                //var accountTab = accountSettingsUserControl.FindName("accountSettingsTab") as TabControl;

                //accountTab.SelectedIndex = selectedIndex;

                // Added code to show Main window in foreground from extension
                Show();
                //ShowInTaskbar = true;
                //WindowState = WindowState.Normal;
                Activate();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement elem = null;
            if (e != null && e.Source != null)
            {
                elem = e.Source as FrameworkElement;
            }

            int selectedIndex = 0;

            if (elem != null && elem.Tag != null && !string.IsNullOrWhiteSpace(elem.Tag.ToString()))
            {
                selectedIndex = int.Parse(elem.Tag.ToString());
            }

            bool switchVisibility = false;
            if (elem == null || elem.Tag == null)
            {
                switchVisibility = true;
            }

            HandleAccountSettings(selectedIndex, switchVisibility);
        }

        public void AccountSettingsClose()
        {
            var btn = MenuButtons.Where(p => p.ID == selectedUIComponentID).FirstOrDefault();
            accountSettingsUserControl.Visibility = System.Windows.Visibility.Collapsed;

            if (btn != null)
            {
                el_MenuButtonClick(btn, null);
            }
        }

        private void SyncClick(object sender, RoutedEventArgs e)
        {
            AccountSettings_Click(sender, e);
        }

        private void HideAllComponents()
        {
            searchViewModel.Search = string.Empty;

            foreach (var c in components)
            {
                c.Selected = false;
            }

            foreach (var c in subComponents)
            {
                c.ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
                c.NotifySubComponent("HideSubComponent");
            }


        }

        private void MouseHasInteraction(object sender, MouseButtonEventArgs e)
        {
            if (pbData != null) pbData.ResetTimer();
            KillInstallExtensionWindow();
        }

        private void KillInstallExtensionWindow()
        {
            foreach (var iw in Application.Current.Windows.OfType<Window>().Where(w => w.GetType().Name == "InstallExtensionWindow"))
            {
                iw.Close();
            }
        }

        private void KeyboardHasInteraction(object sender, KeyEventArgs e)
        {
            if (pbData != null) pbData.ResetTimer();
            KillInstallExtensionWindow();

            // Ctrl + L key handler 
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ((PBApp)Application.Current).Logout(true);
            }
        }

        private void GetPremiumButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetPremiumButton.Focus();

                var button = sender as FrameworkElement;

                if (button != null && button.Tag != null && (button.Tag as string) != null)
                {
                    try
                    {
                        BuyButtons buyButtonType = BuyButtons.TopNav;
                        if ((string)button.Tag == "TopNav")
                        {
                            buyButtonType = BuyButtons.TopNav;
                        }
                        else if ((string)button.Tag == "LeftNav")
                        {
                            buyButtonType = BuyButtons.LeftNav;
                        }

                        var subscription = pbData.GetSubscriptionInfo();
                        int? daysFromAccountCreation = null;
                        if (subscription != null && subscription.AccountCreated && subscription.AccountCreationDate.HasValue)
                        {
                            var currentDate = DateTime.Now;
                            //daysFromAccountCreation = (int)Math.Ceiling((currentDate - subscription.AccountCreationDate.GetValueOrDefault(currentDate)).TotalDays);
                            daysFromAccountCreation = (currentDate - subscription.AccountCreationDate.GetValueOrDefault(currentDate)).Days;
                        }

                        var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();

                        MessageHistory his = new MessageHistory
                        {
                            DaysSinceAccountCreated = daysFromAccountCreation,
                            ButtonClicked = MarketingActionType.GetPremium.ToString(),
                            BuyButton = buyButtonType.ToString()
                        };

                        pbData.InsertMessageHistory(his);
                        var mhItem = pbData.GetMessageHistoryById(his.Id);

                        var analytics2 = inAppAnalyitics.Get<Events.InAppMarketing, InAppMessageItem>();
                        var logItem = new InAppMessageItem(mhItem.RowId, mhItem.AnalyticsCode, mhItem.MsgType, mhItem.Theme, (MarketingActionType)Enum.Parse(typeof(MarketingActionType), mhItem.ButtonClicked), buyButtonType, mhItem.DaysSinceAccountCreated);
                        analytics2.Log(logItem);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                    }
                }


                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("GetPremium", null);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        public bool OpenMenuExpander(Dictionary<string, object> parameters)
        {
            if (!(parameters != null && parameters.ContainsKey("ShowOrHide") && parameters["ShowOrHide"] != null)) return false;
            bool _isExpanderVisible = (bool)parameters["ShowOrHide"];
            if (_isExpanderVisible)
            {
                LeftMenuExpander.IsExpanded = false;
                OtherItemsContainerGrid.Visibility = Visibility.Hidden;
                return true;
            }
            else
            {
                LeftMenuExpander.IsExpanded = true;
                OtherItemsContainerGrid.Visibility = Visibility.Visible;
                return true;
            }
        }


       

        private void JoinUsOnFacebook_Click(object sender, RoutedEventArgs args)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri("http://www.facebook.com/passwordboss"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadMenu();
            System.Windows.Application.Current.MainWindow = this;
            OpenDefaultMenuItem();
        }

        private void OpenDefaultMenuItem()
        {
            var firstButton = MenuButtons.FirstOrDefault();
            if (firstButton != null)
            {
                el_MenuButtonClick(firstButton, null);
            }
        }

        public bool? ShowMasterPasswordDialog(Dictionary<string, object> parameters)
        {
            bool? result = false;
            MasterPasswordDialog masterPass = new MasterPasswordDialog(pbData);
            result = masterPass.ShowDialog();
            return result;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccountSettings_Click(sender, null);
        }


        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItem componentTree=null;
            if (selectedUIComponentID != null)
            {
                componentTree = components.FirstOrDefault(x => x.ID == selectedUIComponentID).ComponentTree;
                if (!(componentTree is AddSecureItem))
                {
                    components.FirstOrDefault(x => x.ID == selectedUIComponentID).ExecuteCommand("AddItem", null);
                    return;
                }
            }

            AddItemChildWindow addChildWindow = new AddItemChildWindow(AddItemsList,componentTree);
            bool? dialogResult = addChildWindow.ShowDialog();
            if (dialogResult.Value)
            {
                var c = resolver.GetAllInstancesOf<IUIComponent>().FirstOrDefault(i => i.ID == addChildWindow.SelectedItemType.ComponentId);
                if (c == null) return;
               // var p = new Dictionary<string, object>();
                if (addChildWindow.SelectedItemType is AddSecureItem)
                {

                    var add = Application.Current.Resources["Add"] as string;
                    if (addChildWindow.SelectedSubItemType.CreateItemType == null)
                        return;
                    var secureItem = Activator.CreateInstance(addChildWindow.SelectedSubItemType.CreateItemType) as ISecureItemVM;
                    secureItem.ItemTitel = add + " " + addChildWindow.SelectedSubItemType.ItemTitel;
                    secureItem.FoldersList = pbData.GetFoldersBySecureItemType();
                    secureItem.Background = addChildWindow.SelectedSubItemType.BackgoundColor;
                    secureItem.Image = ((BitmapImage)addChildWindow.SelectedSubItemType.Icon).ToString();
                    secureItem.AddNewFolder_Clicked += (o,arg)=>
                    {
                        var folder = ServiceLocator.Get<IFolderService>().AddFolder();                        
                        if (!string.IsNullOrEmpty(folder))
                        {
                            secureItem.FoldersList = pbData.GetFoldersBySecureItemType();
                            secureItem.Folder = secureItem.FoldersList.FirstOrDefault(x => x.UUID == folder);
                        };
                    };
                    //p.Add("item", secureItem);

                    if (c.ViewModel is ISecureHolder)
                    {
                        ((ISecureHolder)c.ViewModel).AddNewItem(secureItem);
                    }
                }

                    
                

                //c.ExecuteCommand("AddItem", p);
            }
        }


               

        private void LeftMenuExpander_Collapsed(object sender, RoutedEventArgs e)
        {

        }
    }
        
}