using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using PasswordBoss.Views.UserControls;
using System.Windows;
using PasswordBoss.DTO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using PasswordBoss.Views;
using PasswordBoss.PBAnalytics;
using System.Windows.Media.Animation;
using System.Windows.Input;
using System.Threading.Tasks;

namespace PasswordBoss.ViewModel
{
    public class AddControlViewModel : ViewModelBase, IDataErrorInfo
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(AddControlViewModel));
        /// <summary>
        /// variables declarations
        /// </summary>
        internal const string focusText = "strong";
        internal const string SettingHover = "imgTabSettingHover";
        internal const string Share2 = "imgShare2";
        const string Share2Hover = "imgShare2Hover";
        const string tabSetting = "imgTabSetting";

        private IResolver resolver;
        private IPBData pbData;
        private IPBSync sync;

        private IInAppAnalytics inAppAnalyitics;
        private bool allowPasswordView = false;
        private IFeatureChecker featureChecker;
        ShareCommon shareCommon = null;
        public enum Strength { Normal, Weak, Medium, Strong, VeryStrong }
        string currentUUID = null;

        # region RelayCommands
                
        public RelayCommand PasswordClickCommand { get; set; }
        public RelayCommand CopyPasswordCommand { get; set; }
        public RelayCommand AddCategoryClickCommand { get; set; }
        public RelayCommand PasswordBoxGotFocusCommand { get; set; }
        public RelayCommand PasswordBoxTextChangedCommand { get; set; }
        public RelayCommand PasswordGeneratorCreateCommand { get; set; }
        public RelayCommand TabSelectionChangedCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand ClosePasswordGeneratorCommand { get; set; }
        public RelayCommand MessageBoxConfirmCommand { get; set; }
        public RelayCommand MessageBoxCancelCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> SaveShareCommand { get; set; }
        public RelayCommand CopyUsernameCommand { get; set; }

        public RelayCommand ResendShareCommand { get; set; }
        public RelayCommand CancelShareCommand { get; set; }
        public RelayCommand RevokeShareCommand { get; set; }
        public RelayCommand SendDataShareCommand { get; set; }
        public RelayCommand CancelShareActionCommand { get; set; }

        public RelayCommand OpenInBrowserCommand { get; set; }
        public RelayCommand InvalidShareDialogOkCommand { get; set; }


        #endregion

        Common _common = new Common();
        public event EventHandler<RoutedEventArgs> RefreshList;

        # region Properties

        public ObservableCollection<string> shareDurations;
        public ObservableCollection<string> ShareDurations
        {
            get { return shareDurations; }
            set
            {
                shareDurations = value;
                RaisePropertyChanged("ShareDurations");
            }
        }

        private int expirationPeriodIndex;
        public int ExpirationPeriodIndex
        {
            get { return expirationPeriodIndex; }
            set
            {
                expirationPeriodIndex = value;
                RaisePropertyChanged("ExpirationPeriodIndex");
            }
        }

        public ObservableCollection<Folder> Categories { get; set; }

        private Folder selectedCategory;
        public Folder SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    RaisePropertyChanged("SelectedCategory");
                }
                if (selectedCategory != null && SecureItem == null)
                {
                    UseSecureBrowser = value.UseSecureBrowser;
                }
            }
        }


        public bool IsShareEnabled
        {
            get
            {
                if (SecureItem != null)
                {
                    if (SecureItem.Id != null)
                    {
                        if (Enabled)
                            return true;
                    }
                }
                return false;
            }
        }

        public bool Enabled
        {
            get
            {
                return !ReadonlySecureItem;
            }
        }

        private bool readonlySecureItem;
        public bool ReadonlySecureItem
        {
            get { return readonlySecureItem; }
            set
            {
                readonlySecureItem = value;
                RaisePropertyChanged("ReadonlySecureItem");
                RaisePropertyChanged("Enabled");
                RaisePropertyChanged("IsShareEnabled");
            }
        }

        private SecureItem defaultSecureItem;
        private SecureItem existingSecureItem;

        private SecureItem secureItem;
        private string secureItemFromDb;
        public SecureItem SecureItem
        {
            get
            {
                return secureItem;
            }
            set
            {
                DefaultView();
                secureItemFromDb = String.Empty;
                secureItem = value;
                RaisePropertyChanged("IsShareEnabled");
                if (secureItem != null)
                {
                    ReadonlySecureItem = secureItem.Readonly;
                    secureItemFromDb = secureItem.Site.Uri;
                    //  SecuerShareData = _addControlHelper.BindingSecureShareList(secureItem.Id);
                    string uri = null;
                    if (secureItem.LoginUrl != null)
                    {
                        uri = secureItem.LoginUrl;
                    }
                    else
                    {
                        uri = secureItem.Site.Uri;
                    }

                    SiteName = secureItem.Name;
                    Url = uri;//secureItem.SiteData.Uri;
                    SiteImage = null;
                    DateCreated = secureItem.CreatedDate;
                    DateModified = secureItem.LastModifiedDate;
                    LastAccess = secureItem.LastAccess;
                    AccessCount = secureItem.AccessCount;
                    if (secureItem.Data != null)
                    {
                        UserName = secureItem.Data.username;// != null ? secureItem.Data.username : secureItem.Data.email;
                        ReenterMasterPassword = secureItem.Data.require_master_password;
                        AutoLogin = secureItem.Data.autologin;
                        Notes = secureItem.Data.notes;
                        Password = secureItem.Data.password;
                        ThisSubdomainOnly = secureItem.Data.sub_domain;
                        UseSecureBrowser = secureItem.Data.use_secure_browser;

                        if (SecureItem.Data.password_visible_recipient.HasValue)
                        {
                            if (!SecureItem.Data.password_visible_recipient.Value)
                            {
                                PasswordEyeVisible = false;
                            }
                            else { PasswordEyeVisible = true; }
                        }
                        else { PasswordEyeVisible = true; }
                    }

                    if (secureItem.Folder != null)
                        SelectedCategory = Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id);


                    SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                    PasswordTextVisible = false;
                }

                DeleteButtonVisible = value != null ? Visibility.Visible : Visibility.Hidden;
                if (_addControlScrollViewer != null)
                {
                    _addControlScrollViewer.ScrollToHome();
                }

                if (value == null)
                {
                    existingSecureItem = value;
                }
                else
                {
                    existingSecureItem = CreateSecureItem();
                }
            }
        }

        Visibility _deleteButtonVisible = Visibility.Hidden;

        public Visibility DeleteButtonVisible
        {
            get { return _deleteButtonVisible; }
            set
            {
                _deleteButtonVisible = value;
                RaisePropertyChanged("DeleteButtonVisible");
            }
        }

        private bool _settingsChangeDialogVisibility = false;
        public bool SettingsChangeDialogVisibility
        {
            get { return _settingsChangeDialogVisibility; }
            set
            {
                _settingsChangeDialogVisibility = value;
                RaisePropertyChanged("SettingsChangeDialogVisibility");
            }
        }

        private bool _settingsChangeInvalidDialogVisibility = false;
        public bool SettingsChangeInvalidDialogVisibility
        {
            get { return _settingsChangeInvalidDialogVisibility; }
            set
            {
                _settingsChangeInvalidDialogVisibility = value;
                RaisePropertyChanged("SettingsChangeInvalidDialogVisibility");
            }
        }

        private bool _invalidShareDialogVisibility = false;
        public bool InvalidShareDialogVisibility
        {
            get
            {
                return _invalidShareDialogVisibility;
            }
            set
            {
                _invalidShareDialogVisibility = value;
                RaisePropertyChanged("InvalidShareDialogVisibility");
            }
        }

        private bool _passwordEyeVisible = true;
        public bool PasswordEyeVisible
        {
            get { return _passwordEyeVisible; }
            set
            {
                _passwordEyeVisible = value;
                RaisePropertyChanged("PasswordEyeVisible");
            }
        }

        ImageSource _passwordEyeImage;
        public ImageSource PasswordEyeImage
        {
            get { return _passwordEyeImage; }
            set
            {
                _passwordEyeImage = value;
                RaisePropertyChanged("PasswordEyeImage");
            }
        }

        bool _passwordTextVisible;
        public bool PasswordTextVisible
        {
            get { return _passwordTextVisible; }
            set
            {
                _passwordTextVisible = value;
                RaisePropertyChanged("PasswordTextVisible");
                if (_passwordTextVisible)
                {
                    PasswordEyeImage = (ImageSource)Application.Current.FindResource("imgEyeClose");
                }
                else
                {
                    PasswordEyeImage = (ImageSource)Application.Current.FindResource("imgEyeHoverClose");
                }
            }
        }

        ///PwdGeneratorGrid visibility property
        bool _passwordGeneratorGridVisibility;
        public bool PasswordGeneratorGridVisibility
        {
            get { return _passwordGeneratorGridVisibility; }
            set
            {
                _passwordGeneratorGridVisibility = value;
                RaisePropertyChanged("PasswordGeneratorGridVisibility");
            }
        }

        ///ProgressbarGenerater Value property
        double progressbarGeneraterValue = 0;
        public double ProgressbarGeneraterValue
        {
            get { return progressbarGeneraterValue; }
            set
            {
                progressbarGeneraterValue = value;
                RaisePropertyChanged("ProgressbarGeneraterValue");
                if (progressbarGeneraterValue == 0)
                {
                    PasswordStrengthText = string.Empty;
                }
            }
        }
        int _selectedIndexTabControl;
        public int SelectedIndexTabControl
        {
            get { return _selectedIndexTabControl; }
            set
            {
                _selectedIndexTabControl = value;
                RaisePropertyChanged("SelectedIndexTabControl");
            }
        }
        private ImageSource _settingTabIcon = DefaultProperties.ReturnImage(SettingHover);
        public ImageSource SettingTabIcon
        {
            get { return _settingTabIcon; }
            set
            {
                if (Equals(_settingTabIcon, value)) return;
                _settingTabIcon = value;
                RaisePropertyChanged("SettingTabIcon");
            }
        }
        private ImageSource _secureShareTabIcon = DefaultProperties.ReturnImage(Share2);
        public ImageSource SecureShareTabIcon
        {
            get { return _secureShareTabIcon; }
            set
            {
                if (Equals(_secureShareTabIcon, value)) return;
                _secureShareTabIcon = value;
                RaisePropertyChanged("SecureShareTabIcon");
            }
        }

        DateTime? _dateCreated;
        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                if (value.HasValue) DatesVisibility = true;
                RaisePropertyChanged("DateCreated");
            }
        }
        DateTime? _dateModified;
        public DateTime? DateModified
        {
            get { return _dateModified; }
            set
            {
                _dateModified = value;
                if (value.HasValue) DatesVisibility = true;
                RaisePropertyChanged("DateModified");
            }
        }

        bool _datesVisibility;
        public bool DatesVisibility
        {
            get
            {
                return _datesVisibility;
            }
            set
            {
                _datesVisibility = value;
                RaisePropertyChanged("DatesVisibility");
            }
        }
        private string TempUrl
        {
            get
            {
                string tmpUrl = Url;
                if (tmpUrl != null && tmpUrl != "" && !tmpUrl.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!tmpUrl.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tmpUrl = "http://" + tmpUrl;
                    }
                }
                return tmpUrl;
            }
        }
        string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _url = value;

                RaisePropertyChanged("Url");
                RaisePropertyChanged("IsValid");
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
                if (SiteImage == null)
                {
                    bool isSiteIdPopulated = false;
                    if (this.SecureItem != null)
                    {
                        if (this.SecureItem.Site != null)
                        {

                            _siteImage = _common.GetDefaultImageOrImageForSite(this.secureItem.Site.UUID);
                            isSiteIdPopulated = true;
                        }
                    }

                    if (!isSiteIdPopulated)
                    {
                        _siteImage = _common.GetDefaultImageOrImageForSite();
                    }

                }
                RaisePropertyChanged("SiteImage");
            }
        }

        string _siteName;
        public string SiteName
        {
            get { return _siteName; }
            set
            {
                _siteName = value;
                RaisePropertyChanged("SiteName");
            }
        }

        DateTime? _lastAccess;
        public DateTime? LastAccess
        {
            get { return _lastAccess; }
            set
            {
                _lastAccess = value;
                RaisePropertyChanged("LastAccess");
            }
        }

        uint _accessCount;
        public uint AccessCount
        {
            get { return _accessCount; }
            set
            {
                _accessCount = value;
                RaisePropertyChanged("AccessCount");
            }
        }
        string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }
        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
                ScanPassword();
            }
        }

        string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged("Notes");
            }
        }
        string _recipientEmail = string.Empty;
        public string RecipientEmail
        {
            get { return _recipientEmail; }
            set
            {
                _recipientEmail = value;
                RaisePropertyChanged("RecipientEmail");
            }
        }
        string _message = string.Empty;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        bool _passwordVisibleToRecipient = false;
        public bool PasswordVisibleToRecipient
        {
            get
            {
                return _passwordVisibleToRecipient;
            }
            set
            {
                _passwordVisibleToRecipient = value;
                RaisePropertyChanged("PasswordVisibleToRecipient");
            }
        }

        int _passwordLength = 12;
        public int PasswordLength
        {
            get { return _passwordLength; }
            set
            {
                _passwordLength = value;
                RaisePropertyChanged("PasswordLength");
            }
        }

        string _passwordStrengthText = string.Empty;
        public string PasswordStrengthText
        {
            get { return _passwordStrengthText; }
            set
            {
                _passwordStrengthText = value;
                RaisePropertyChanged("PasswordStrengthText");
            }
        }



        bool _letters = true;
        public bool Letters
        {
            get { return _letters; }
            set
            {
                _letters = value;
                RaisePropertyChanged("Letters");
                CheckComboboxProperties();
            }
        }
        bool _capitals = true;
        public bool Capitals
        {
            get { return _capitals; }
            set
            {
                _capitals = value;
                RaisePropertyChanged("Capitals");
                CheckComboboxProperties();
            }
        }
        bool _numbers = true;
        public bool Numbers
        {
            get { return _numbers; }
            set
            {
                _numbers = value;
                RaisePropertyChanged("Numbers");
                CheckComboboxProperties();
            }
        }
        bool _symbols = true;
        public bool Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
                RaisePropertyChanged("Symbols");
                CheckComboboxProperties();
            }
        }

        bool _use_secure_browser = false;
        public bool UseSecureBrowser
        {
            get { return _use_secure_browser; }
            set
            {
                _use_secure_browser = value;
                RaisePropertyChanged("UseSecureBrowser");
            }
        }

        bool _auto_login = false;
        public bool AutoLogin
        {
            get { return _auto_login; }
            set
            {
                _auto_login = value;
                RaisePropertyChanged("AutoLogin");
            }
        }

        bool _reenter_master_password = false;
        public bool ReenterMasterPassword
        {
            get { return _reenter_master_password; }
            set
            {
                _reenter_master_password = value;
                RaisePropertyChanged("ReenterMasterPassword");
            }
        }

        bool _this_subdomain_only = false;
        public bool ThisSubdomainOnly
        {
            get { return _this_subdomain_only; }
            set
            {
                _this_subdomain_only = value;
                RaisePropertyChanged("ThisSubdomainOnly");
            }
        }

        List<SecuerShareData> _secuerShareData;
        public List<SecuerShareData> SecuerShareData
        {
            get { return _secuerShareData; }
            set
            {
                _secuerShareData = value;
                RaisePropertyChanged("SecuerShareData");
            }
        }

        private bool _messageBoxVisibility;

        public bool MessageBoxVisibility
        {
            get { return _messageBoxVisibility; }
            set
            {
                _messageBoxVisibility = value;
                RaisePropertyChanged("MessageBoxVisibility");
            }
        }

        # endregion


        private UserControl ownerControl;
        private readonly IPBExtSecureBrowserBridge _pbExtSecureBrowserBridge;

        //To enable resetting scroll viewer position
        private ScrollViewer _addControlScrollViewer;
        private PasswordVaultContentPanel passwordVaultPanel;
        /// <summary>
        /// 
        /// </summary>
        public AddControlViewModel(IResolver resolver)
        {
            this.resolver = resolver;
           // passwordVaultPanel = panel;
            pbData = resolver.GetInstanceOf<IPBData>();
            sync = resolver.GetInstanceOf<IPBSync>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();

            shareCommon = new ShareCommon(resolver);
            featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            InitializeCommands();
            //vedo - async ?
            PopulateCategories();

            SecuerShareData = new List<SecuerShareData>();

            //this.ownerControl = ownerControl;

            //if (ownerControl != null)
            //{
            //    _addControlScrollViewer = this.ownerControl.FindName("PasswordVaultScroller") as ScrollViewer;
            //}
            _pbExtSecureBrowserBridge = resolver.GetInstanceOf<IPBExtSecureBrowserBridge>();

            PopulateShareDurations();

            ExpirationPeriodIndex = 0;
        }

        private void PopulateCategories()
        {
            Task.Factory.StartNew(() =>
            {
                Categories = new ObservableCollection<Folder>(pbData.GetFoldersBySecureItemType());
                RaisePropertyChanged("Categories");
            });
        }

        private void PopulateShareDurations()
        {
            if (ShareDurations == null)
            {
                ShareDurations = new ObservableCollection<string>();
            }
            else ShareDurations.Clear();

            ShareDurations.Add(Application.Current.FindResource("UntilCancel").ToString());
            ShareDurations.Add(Application.Current.FindResource("_OneDay").ToString());
            ShareDurations.Add(Application.Current.FindResource("_1Week").ToString());
            ShareDurations.Add(Application.Current.FindResource("_1Month").ToString());
            ShareDurations.Add(Application.Current.FindResource("_1Year").ToString());
            RaisePropertyChanged("ShareDurations");
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveItemAsync);

            CancelCommand = new RelayCommand(CancelNewItem);
            ContinueCommand = new RelayCommand(ContinueSettings);
            DeleteCommand = new RelayCommand(DeleteItem);
            PasswordClickCommand = new RelayCommand(ShowPasswordClick);
            CopyPasswordCommand = new RelayCommand(CopyPasswordClick);
            AddCategoryClickCommand = new RelayCommand(AddCategoryClick);
            PasswordBoxGotFocusCommand = new RelayCommand(PwdBoxGotFocus);
            PasswordGeneratorCreateCommand = new RelayCommand(PasswordGeneratorCreate);
            TabSelectionChangedCommand = new RelayCommand(TabSelectionChanged);
            ClosePasswordGeneratorCommand = new RelayCommand(PasswordGeneratorCloseClick);
            MessageBoxConfirmCommand = new RelayCommand(MessageBoxConfirmClick);
            MessageBoxCancelCommand = new RelayCommand(MessageBoxCancelClick);
            SaveShareCommand = new AsyncRelayCommand<LoadingWindow>(SaveShare);
            CopyUsernameCommand = new RelayCommand(CopyUsernameClick);

            ResendShareCommand = new RelayCommand(ResendShare);
            CancelShareCommand = new RelayCommand(CancelShare);
            RevokeShareCommand = new RelayCommand(RevokeShare);
            SendDataShareCommand = new RelayCommand(SendDataShare);
            CancelShareActionCommand = new RelayCommand(CancelShareActionClick);
            OpenInBrowserCommand = new RelayCommand(OpenInBrowserClick);
            InvalidShareDialogOkCommand = new RelayCommand(InvalidShareDialogOkClick);
        }

        /// <summary>
        /// Checks if all parameters on the Model are valid and ready to be saved
        /// </summary>
        ///
        private void CopyUsernameClick(object obj)
        {

            if (UserName != String.Empty)
            {
                if (appCmd != null) appCmd.SetClipboardText(UserName);
            }
        }



        private void CopyPasswordClick(object obj)
        {
            if (Password != String.Empty)
            {
                if (appCmd != null) appCmd.SetClipboardText(Password);
            }
        }

        private void CheckComboboxProperties()
        {
            if (Numbers == false && Letters == false && Symbols == false && Capitals == false)
                Letters = true;
        }
        protected bool CanSave(object obj)
        {
            return this.IsValid;
        }

        private void PasswordGeneratorCloseClick(object obj)
        {
            PasswordGeneratorGridVisibility = false;
        }

        private void MessageBoxConfirmClick(object obj)
        {
            if (SecureItem != null)
            {
                SecureItem.Active = false;
                if ((secureItem = pbData.AddOrUpdateSecureItem(SecureItem)) == null)
                {
                    MessageBoxVisibility = false;
                    MessageBox.Show("Error while saving item");
                }
                else
                {

                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(SecureItem);

                    var pw = inAppAnalyitics.Get<Events.PasswordVault, PasswordVaultItem>();
                    //var pw = inAppAnalyitics.Get<Events.DeletedPasswordVault, Nothing>();
                    pw.Log(new PasswordVaultItem(SecureItemAction.Deleted, ApplicationSource.MainUI));
                    SecureItem = null;
                    DefaultView();
                    EventHandler<RoutedEventArgs> handler = RefreshList;
                    handler(this, null);
                    if (appCmd != null) appCmd.ExecuteCommand("UpdateAlertNotificationCount", null);
                }
            }

            MessageBoxVisibility = false;
        }

        private void MessageBoxCancelClick(object obj)
        {
            MessageBoxVisibility = false;
        }

        public void DeleteItem(object obj)
        {
            MessageBoxVisibility = true;
        }

        private void InvalidShareDialogOkClick(object obj)
        {
            InvalidShareDialogVisibility = false;
        }

        private void SaveShare(object obj)
        {
            ShareCommon share = new ShareCommon(resolver);
            List<SecuerShareData> shareData = null;
            bool isSharingWithCurrentUser = false;

            if (pbData.ActiveUser == RecipientEmail.Trim())
            {
                isSharingWithCurrentUser = true;
                //InvalidShareDialogVisibility = true;
            }
            else
            {

                shareData = share.ShareItem(RecipientEmail, Message, SecureItem, ExpirationPeriodIndex, PasswordVisibleToRecipient);
            }

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (isSharingWithCurrentUser == true)
                {
                    InvalidShareDialogVisibility = true;
                }
                else
                {

                    SecuerShareData = shareData;
                    if (SecuerShareData.Count > 0)
                    {
                        DefaultViewShare();
                        AfterSaveCompleted();
                    }
                }

            }));

        }


        private bool isValidErrorMessageVisible = false;
        public bool IsValidErrorMessageVisible
        {
            get { return isValidErrorMessageVisible; }
            set
            {
                isValidErrorMessageVisible = value;
                RaisePropertyChanged("IsValidErrorMessageVisible");
            }
        }

        public bool HasModelChanged()
        {
            SecureItem secureItem = CreateSecureItem();
            if (existingSecureItem != null)
            {
                if (existingSecureItem.Hash != secureItem.Hash && secureItem.Hash != defaultSecureItem.Hash)
                {
                    return true;
                }
            }
            else if (defaultSecureItem != null)
            {
                if (defaultSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }

            return false;
        }


        private SecureItem CreateSecureItem()
        {
            SecureItem secureItem = new SecureItem()
            {
                Id = SecureItem != null ? this.SecureItem.Id : null,
                SecureItemTypeName = DefaultProperties.SecurityItemType_PasswordVault,
                Type = DefaultProperties.SecurityItemSubType_PV_Login,
                LoginUrl = Url, // TempUrl,
                Site = new Site()
                {

                    Name = String.IsNullOrEmpty(SiteName) ? Url : SiteName,
                    Uri = TempUrl
                },
                Name = String.IsNullOrEmpty(SiteName) ? Url : SiteName,
                AccessCount = AccessCount,
                LastAccess = LastAccess,
                Data = new SecureItemData()
                {
                    password_visible_recipient = SecureItem != null ? this.SecureItem.Data != null ? this.SecureItem.Data.password_visible_recipient : null : null,
                    username = UserName,// !String.IsNullOrEmpty(UserName) ? !_common.IsEmailValid(UserName) ? UserName : null : null,
                    //email = !String.IsNullOrEmpty(UserName) ? _common.IsEmailValid(UserName) ? UserName : null : null,
                    // nickname = SiteName,
                    require_master_password = ReenterMasterPassword,
                    autologin = AutoLogin,
                    notes = Notes,
                    password = Password,
                    sub_domain = ThisSubdomainOnly,
                    use_secure_browser = UseSecureBrowser
                },
                Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryOther) : SelectedCategory
            };

            return secureItem;
        }

        private void SaveItem()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SettingsChangeDialogVisibility = false;
                SettingsChangeInvalidDialogVisibility = false;
            }));

            try
            {
                if (!featureChecker.IsEnabled(DefaultProperties.Features_PasswordValt_AddManageLogins))
                {
                    return;
                }
                if (_auto_login && !featureChecker.IsEnabled(DefaultProperties.Features_PasswordValt_AutoLogin))
                {
                    return;
                }

                bool urlChanged = TempUrl != secureItemFromDb;
                if (!IsValid)
                {
                    IsValidErrorMessageVisible = true;
                    //TODO show validation message
                    return;
                }
                IsValidErrorMessageVisible = false;

                SecureItem secureItem = CreateSecureItem();
                Common common = new Common();
                if (urlChanged && TempUrl != null && common.IsUrlValid(TempUrl, uriKind: UriKind.Absolute))
                {
                    var siteId = pbData.GetSiteIdByUriFullSearch(new Uri(TempUrl));
                    secureItem.Site.Id = siteId;
                }

                bool updateSiteName = false;
                if (string.IsNullOrEmpty(SiteName))
                {
                    updateSiteName = true;
                }
                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Error while saving item");
                    }));
                }
                else
                {
                    //update shares

                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(secureItem);

                    if (SecureItem == null)
                    {
                        var pw = inAppAnalyitics.Get<Events.PasswordVault, PasswordVaultItem>();
                        pw.Log(new PasswordVaultItem(SecureItemAction.Added, ApplicationSource.MainUI));
                    }

                    Task.Factory.StartNew(() =>
                    {
                        sync.RegisterSites();
                        if (updateSiteName)
                        {
                            secureItem.Name = pbData.GetSiteById(secureItem.Site.Id).FriendlyName;
                        }
                        pbData.AddOrUpdateSecureItem(secureItem);
                    });
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    SaveItemCompleted();
                }));
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show((string)Application.Current.FindResource("GeneralErrorText"));
                }));
                logger.Error(ex.Message);
            }
        }

        private void SaveItemAsync(object obj)
        {
            Task.Factory.StartNew(() =>
            {
                SaveItem();
            });
        }

        private void CloseAnimation()
        {
            Storyboard sbClose = Application.Current.TryFindResource("StoryboardCloseNewItem") as Storyboard;
            Storyboard.SetTarget(sbClose, ownerControl);
            sbClose.Begin();
        }

        private void SaveItemCompleted()
        {
            if (!IsValid)
            {
                //skip when not valid
                return;
            }

            AfterSaveCompleted();
        }

        private void AfterSaveCompleted()
        {
            CloseAnimation();
            SecureItem = null;
            DefaultView();
            EventHandler<RoutedEventArgs> handler = RefreshList;
            if (handler != null)
            {
                handler(this, null);
            }
            if (appCmd != null) appCmd.ExecuteCommand("UpdateAlertNotificationCount", null);
            //if (passwordVaultPanel != null)
            //{
            //    //passwordVaultPanel.PasswordVaultItemsContainer.listView.SelectedItems.Clear();
           // }

            //Quick fix of bug with window disappearing after save
            Application.Current.MainWindow.Activate();
        }

        private void ContinueSettings(object obj)
        {

            SettingsChangeInvalidDialogVisibility = false;
            IsValidErrorMessageVisible = true;
            if (this.ownerControl != null)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    ownerControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                });
            }
        }

        private void CancelNewItem(object obj)
        {
            SettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;
            SecureItem = null;
            DefaultView();
            CloseAnimation();
            // DelegateCommand d  = new DelegateCommand()
        }

        /// <summary>
        /// tab control selection event for changing header icon
        /// </summary>
        /// <param name="obj"></param>
        private void TabSelectionChanged(object obj)
        {
            if (SelectedIndexTabControl == 0)
            {
                SettingTabIcon = DefaultProperties.ReturnImage(SettingHover);
                SecureShareTabIcon = DefaultProperties.ReturnImage(Share2);
            }
            else
            {
                SettingTabIcon = DefaultProperties.ReturnImage(tabSetting);
                SecureShareTabIcon = DefaultProperties.ReturnImage(Share2Hover);
            }
        }

        /// <summary>
        /// Ths event hides PwdGeneratorGrid for now
        /// </summary>
        /// <param name="obj"></param>
        private void PasswordGeneratorCreate(object element)
        {
            PasswordScanner scanner = new PasswordScanner();
            RandomPasswordGenerator passwordGenerator = new RandomPasswordGenerator();
            var passwordBox = element as PasswordBox;
            String generatedPassword = passwordGenerator.generatePswd(PasswordLength, Capitals, Numbers, Symbols, Letters);
            passwordBox.Password = generatedPassword;
            Strength s = (Strength)scanner.scanPassword(generatedPassword);
            switch (s)
            {
                case (Strength.Weak):
                    ProgressbarGeneraterValue = 10;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                    break;
                case (Strength.Normal):
                    ProgressbarGeneraterValue = 25;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                    break;
                case (Strength.Medium):
                    ProgressbarGeneraterValue = 50;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                    break;
                case (Strength.Strong):
                    ProgressbarGeneraterValue = 75;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                    break;
                case (Strength.VeryStrong):
                    ProgressbarGeneraterValue = 100;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                    break;
                default:
                    ProgressbarGeneraterValue = 0;
                    PasswordStrengthText = string.Empty;
                    break;
            }

            PasswordGeneratorGridVisibility = false;

            var an = inAppAnalyitics.Get<Events.GeneratedPassword, PasswordGeneratorItem>();
            an.Log(new PasswordGeneratorItem(PasswordGeneratorSource.PasswordVault));

        }

        /// <summary>
        /// Ths event shows PwdGeneratorGrid
        /// </summary>
        /// <param name="obj"></param>
        private void PwdBoxGotFocus(object obj)
        {
            PasswordGeneratorGridVisibility = true;
            if (string.IsNullOrWhiteSpace(Password))
            {

                Letters = true;
                Symbols = true;
                Capitals = true;
                Numbers = true;
                PasswordLength = 12;
            }
            else
            {
                Symbols = false;
                Capitals = false;
                Numbers = false;
                Letters = false;

                String specialCharsArray = "~!@#$%^&*()-_=+[{]}|;:<>/?";
                PasswordLength = Password.Length;
                for (int i = 0; i < PasswordLength; i++)
                {
                    char c = Password.ElementAt(i);


                    if (c >= 'A' && c <= 'Z')
                    {
                        Capitals = true;
                    }
                    if (c >= '0' && c <= '9')
                    {
                        Numbers = true;
                    }
                    if (specialCharsArray.Contains("" + c))
                    {
                        Symbols = true;
                    }
                    if ((c >= 'a' && c <= 'z') || Capitals)
                    {
                        Letters = true;
                    }

                }
            }

            //ProgressbarGeneraterValue = 0;
        }

        private void ScanPassword()
        {
            PasswordGeneratorGridVisibility = false;
            if (Password == string.Empty)
            {
                ProgressbarGeneraterValue = 0;
                PasswordStrengthText = string.Empty;
            }
            else
            {
                PasswordScanner scanner = new PasswordScanner();
                Strength s = (Strength)scanner.scanPassword(Password);
                switch (s)
                {
                    case (Strength.Weak):
                        ProgressbarGeneraterValue = 10;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                        break;
                    case (Strength.Normal):
                        ProgressbarGeneraterValue = 25;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                        break;
                    case (Strength.Medium):
                        ProgressbarGeneraterValue = 50;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                        break;
                    case (Strength.Strong):
                        ProgressbarGeneraterValue = 75;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                        break;
                    case (Strength.VeryStrong):
                        ProgressbarGeneraterValue = 100;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                        break;
                    default:
                        ProgressbarGeneraterValue = 0;
                        PasswordStrengthText = string.Empty;
                        break;
                }
            }
        }


        private void AddCategoryClick(object param)
        {
            CategoryBox catDialog = new CategoryBox();
            bool? result = catDialog.ShowDialog();
            if (result.HasValue)
                if (result.Value)
                {
                    string newCode = pbData.AddFolder(catDialog.NewCategory, catDialog.UseSecureBrowser);
                    if (newCode != null)
                    {
                        Categories = new ObservableCollection<Folder>(pbData.GetFoldersBySecureItemType());
                        RaisePropertyChanged("Categories");
                        SelectedCategory = Categories.SingleOrDefault(x => x.Id == newCode);
                    }

                }
        }

        /// <summary>
        /// Enable the Prompt  Window   based on paramter name
        /// </summary>
        /// <param name="obj"></param>
        private void ShowPasswordClick(object stringName)
        {

            if (PasswordTextVisible)
            {
                PasswordTextVisible = false;
            }
            else
            {
                if (allowPasswordView) PasswordTextVisible = true;
                else
                {
                    bool? result = true;
                    //MasterPasswordConfirm masterPass = new MasterPasswordConfirm(pbData);
                    //result = masterPass.ShowDialog();
                    //TODO Validate master password before show
                    if (result.HasValue)
                        if (result.Value)
                        {
                            PasswordTextVisible = true;
                            //allowPasswordView = masterPass.AlwaysAllow;
                        }
                }
            }

        }

        internal void DefaultViewShare()
        {
            SiteImage = null;
            RecipientEmail = "";
            Message = "";
            ExpirationPeriodIndex = 0;
            PasswordVisibleToRecipient = false;
        }

        /// <summary>
        /// default properties of password vault addcontrol
        /// </summary>
        public void DefaultView(string url = "", string categoryId = null, string siteName = "")
        {
            ReadonlySecureItem = false;
            PopulateCategories();
            PopulateShareDurations();
            PasswordEyeVisible = true;
            IsValidErrorMessageVisible = false;

            DateCreated = null;
            DateModified = null;
            DatesVisibility = false;


            RecipientEmail = "";
            Message = "";
            SiteName = siteName;
            UserName = "";
            Notes = "";
            SelectedIndexTabControl = 0;
            Symbols = false;
            Numbers = false;
            Capitals = false;
            Letters = false;
            PasswordGeneratorGridVisibility = false;

            Password = "";
            PasswordTextVisible = true;
            SecuerShareData = null;
            ExpirationPeriodIndex = 0;

            bool tmp = false;
            //load advanced settings
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin), out tmp);
            AutoLogin = tmp;

            //AutoLogin = true;
            ReenterMasterPassword = false;

            if (categoryId != null)
            {
                SelectedCategory = Categories.SingleOrDefault(x => x.Id == categoryId);
            }
            else
            {
                SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryOther);
            }


            UseSecureBrowser = SelectedCategory != null ? SelectedCategory.UseSecureBrowser : false;
            Url = url;
            this.SiteImage = null;

            if (this._addControlScrollViewer != null)
            {
                _addControlScrollViewer.ScrollToHome();
            }

            defaultSecureItem = CreateSecureItem();
            secureItem = null;

            //Update security score
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("RefreshSecurityScore", null);
        }



        #region SENDER side actions

        private void ResendShare(object obj)
        {
            if (!ResendMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                ResendMessageBoxVisibility = true;
                return;
            }
            ResendMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;

            var share = pbData.GetSharesByUuid(uuid);

            TimeSpan tmp = DateTime.Now - share.CreatedDate;
            share.ExpirationDate.AddDays(tmp.Days);

            shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null);
            shareCommon.ShareItem(share.Receiver, share.Message, SecureItem, 0, share.Visible, share.ExpirationDate);



            SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
        }

        private void RevokeShare(object obj)
        {
            if (!UnshareMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                UnshareMessageBoxVisibility = true;
                return;
            }
            UnshareMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Revoked, false, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }
        private void CancelShare(object obj)
        {
            if (!CancelMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                CancelMessageBoxVisibility = true;
                return;
            }
            CancelMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }
        private void SendDataShare(object obj)
        {
            var uuid = obj as string;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Pending, true, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }

        public void CancelShareActionClick(object obj)
        {
            CancelMessageBoxVisibility = false;
            ResendMessageBoxVisibility = false;
            UnshareMessageBoxVisibility = false;
            currentUUID = null;
        }

        private bool _unshareMessageBoxVisibility;

        public bool UnshareMessageBoxVisibility
        {
            get { return _unshareMessageBoxVisibility; }
            set
            {
                _unshareMessageBoxVisibility = value;
                RaisePropertyChanged("UnshareMessageBoxVisibility");
            }
        }

        private bool _resendMessageBoxVisibility;

        public bool ResendMessageBoxVisibility
        {
            get { return _resendMessageBoxVisibility; }
            set
            {
                _resendMessageBoxVisibility = value;
                RaisePropertyChanged("ResendMessageBoxVisibility");
            }
        }

        private bool _cancelMessageBoxVisibility;

        public bool CancelMessageBoxVisibility
        {
            get { return _cancelMessageBoxVisibility; }
            set
            {
                _cancelMessageBoxVisibility = value;
                RaisePropertyChanged("CancelMessageBoxVisibility");
            }
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { return null; }
        }

        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationErrors(property) != String.Empty)
                        return false;
                return true;
            }
        }
        public string this[string propertyName]
        {
            get
            {
                return GetValidationErrors(propertyName);
            }
        }


        #endregion

        #region Validation
        static readonly string[] ValidatedProperties =
        {
            "Url"

        };

        string GetValidationErrors(string propertyName)
        {
            string error = String.Empty;
            switch (propertyName)
            {
                case "Url":
                    error = ValidateRequiredField(Url);
                    if (String.IsNullOrEmpty(error))
                        error = ValidateUrl();
                    break;

            };
            return error;
        }

        private string ValidateUrl()
        {
            if (!_common.IsUrlValid(TempUrl)) return Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty; ;
        }
        private string ValidateRequiredField(string fieldValue)
        {
            fieldValue = fieldValue != null ? fieldValue.Trim() : null;
            if (string.IsNullOrEmpty(fieldValue)) return Application.Current.FindResource("ValidationTextMessage").ToString();
            return string.Empty;
        }
        #endregion



        public void OpenInBrowserClick(object o)
        {
            try
            {
                if (SecureItem == null)
                {
                    if (string.IsNullOrWhiteSpace(TempUrl)) return;
                    if (UseSecureBrowser)
                    {
                        var dictionary = new Dictionary<string, object> { { "url", TempUrl } };
                        ((IAppCommand)Application.Current).ExecuteCommand("OpenUrlInSecureBrowser", dictionary);
                    }
                    else
                    {
                        BrowserHelper.OpenInDefaultBrowser(new Uri(TempUrl));
                    }
                }
                else
                {
                    _pbExtSecureBrowserBridge.OneClickLogin(SecureItem.Id, false);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

        }
    }
}
