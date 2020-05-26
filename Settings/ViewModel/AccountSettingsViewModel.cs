using PasswordBoss;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using PasswordBoss.Views.UserControls;
using Settings.Model.AccountSettings;
using Settings.ViewModel.ViewModel;
using Settings.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Settings.ViewModel.AccountSettings
{
    public class AccountSettingsViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(AccountSettingsViewModel));

        const int Letter = 1;
        const int Capital = 2;
        const int Numeric = 3;
        const int Symbol = 4;
        const int Zero = 0;
        const int PasswordLength = 7;

        Common _commonObj = new Common();

        private const string AccountSettingImage = "imgAccountSettingIcon";
        private const string AccountSettingHoverImage = "imgAccountSettingHover";

        private const string GreenForegroundColor = "PasswordBossGreenColor";
        private const string RedForegroundColor = "PasswordBossRedColor";

        private bool _initialized = false;
        public TwoStepVerificationViewModel TwoStepVerificationModel { get; set; }

        private IPBData pbData;
        private IBrowserMonitor browserMonitor;
        private IPBWebAPI api;
        private IInAppAnalytics analytics;

        private IResolver _resolver;
        public IResolver resolver
        {
            get { return _resolver; }
            set
            {
                _resolver = value;
                pbData = _resolver.GetInstanceOf<IPBData>();
                RefreshData();
            }
        }

        public void RefreshData()
        {
            InitilaizeBrowserMonitor();
            InitializeGrid();
            InitializeDisabledSitesGrid();
            InitializeSettingsData();
            Initialize();
        }

        private TwoStepVerificationUserControl content = null;

        public TwoStepVerificationUserControl Content
        {
            get { return content; }
            set { content = value; }
        }
        public AccountSettingsViewModel(IResolver resolver)
        {
            InitializeCommands();
            ChangePasswordVisibility = false;
            ChangePINVisibility = false;
            SetAutoLockVisibility = false;
            TwoStepVerificationModel = new TwoStepVerificationViewModel(this);
            this.resolver = resolver;
            //this.pbData = resolver.GetInstanceOf<IPBData>();

        }

        private void InitilaizeBrowserMonitor()
        {
            browserMonitor = _resolver.GetInstanceOf<IBrowserMonitor>();
            if (browserMonitor != null)
            {
                browserMonitor.ChromeStopped += browserMonitor_BrowserStopped;
                browserMonitor.FFStopped += browserMonitor_BrowserStopped;
                browserMonitor.IEStopped += browserMonitor_BrowserStopped;
            }
        }

        void browserMonitor_BrowserStopped()
        {
            ClearPasswordsFromBrowser();
            EnableDisableStoringPasswordInBrowsers();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            var currentDevice = pbData.GetDevice(pbData.InstallationUUID);
            if (currentDevice != null)
            {
                //this.db.ChangePrivateSetting("last_store_backup_date", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                PopulateLastSyncDate();
                //LastSync = currentDevice.LatestSync;
            }

            IPBSync sync = resolver.GetInstanceOf<IPBSync>();
            if (sync != null) sync.OnSyncFinished += sync_OnSyncFinished;
        }

        private void PopulateLastSyncDate()
        {
            var lastStoreBackupDateStr = pbData.GetPrivateSetting("last_store_backup_date");
            if (!string.IsNullOrWhiteSpace(lastStoreBackupDateStr))
            {
                DateTime lastDate = DateTime.MinValue;
                if (DateTime.TryParse(lastStoreBackupDateStr, out lastDate))
                {
                    LastSync = lastDate;
                }
            }
            else
            {
                LastSync = null;
            }
        }

        public void InitializeSettingsData()
        {
            #region Security

            securityConfigRememberLastLogin = pbData.GetConfigurationByAccountAndKey(pbData.ActiveUser, DefaultProperties.Configuration_Key_RememberEmail);
            if (securityConfigRememberLastLogin != null)
            {
                bool tmpBool = false;
                if (bool.TryParse(securityConfigRememberLastLogin.Value, out tmpBool)) SecurityRememberLastLogin = tmpBool;
            }
            else
            {
                SecurityRememberLastLogin = true;
            }

            securityConfigEnablePinAccess = pbData.GetConfigurationByAccountAndKey(pbData.ActiveUser, DefaultProperties.Configuration_Key_EnablePinAccess);
            if (securityConfigEnablePinAccess != null)
            {
                bool tmpBool = false;
                if (bool.TryParse(securityConfigEnablePinAccess.Value, out tmpBool)) EnablePinAccessIsChecked = tmpBool;
            }
            else
            {
                EnablePinAccessIsChecked = false;
            }

            #endregion

            LoadTabPreferences();

            LoadTabGeneraData();

            AccountFoldersItemSource = null;
            AccountFoldersItemSource = GetFoldersHierarchyCollection();

            LoadTabAdvancedData();

            var userInfo = pbData.GetUserInfo(pbData.ActiveUser);
            if (userInfo != null)
            {
                TwoStepVerification = userInfo.MultiFactorAuthentication;
            }

            if (int.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Security_PasswordAutoLock), out _autoLockMinutes))
            {
                AutoLockMinutes = _autoLockMinutes;
            }
            else
            {
                AutoLockMinutes = 1440;
            }

            RaisePropertyChanged("TwoStepTrustedDevice");
        }

        private void LoadTabAdvancedData()
        {
            bool tmp = false;

            //load advanced settings
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin), out tmp);
            AutoLogin = tmp; tmp = false;
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_RequireMasterPasswordForAll), out tmp);
            RequireMasterPasswordForAll = tmp; tmp = false;
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_ClearPasswordsFromBrowsers), out tmp);
            ClearPasswordsFromBrowsers = tmp; tmp = false;
            //Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_ClearPasswords), out tmp);
            //ClearPasswords = tmp; tmp = false;
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving), out tmp);
            TurnOffPassSaving = tmp; tmp = false;
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages), out tmp);
            DisableStatusMessages = tmp; tmp = false;
            Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData), out tmp);
            AutoStoreData = tmp;

            //TabAdvancedChanged = false;
        }

        public void LogStatusMessagesAnalytcis(bool disableStatusMessagesOld)
        {
            if (analytics == null)
            {
                analytics = resolver.GetInstanceOf<IInAppAnalytics>();
            }

            if (analytics != null && disableStatusMessagesOld != DisableStatusMessages)
            {
                var analyticsEvent = analytics.Get<Events.SyncMessagesEnabled, YesOrNo>();
                analyticsEvent.Log(DisableStatusMessages ? YesOrNo.Y : YesOrNo.N);
            }
        }

        private void SaveTabAdvancedData(object obj)
        {
            try
            {
                bool disableStatusMessagesOld = false;
                Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages), out disableStatusMessagesOld);

                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin, AutoLogin.ToString());
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_ClearPasswordsFromBrowsers, ClearPasswordsFromBrowsers.ToString());
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_RequireMasterPasswordForAll, RequireMasterPasswordForAll.ToString());
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving, TurnOffPassSaving.ToString());
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages, DisableStatusMessages.ToString());
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData, AutoStoreData.ToString());

                EnableDisableStoringPasswordInBrowsers();
                ClearPasswordsFromBrowser();

                var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
                if (inAppAnalyitics != null && disableStatusMessagesOld != DisableStatusMessages)
                {
                    var analytics = inAppAnalyitics.Get<Events.SyncMessagesEnabled, YesOrNo>();
                    analytics.Log(DisableStatusMessages ? YesOrNo.Y : YesOrNo.N);
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }

            LoadTabAdvancedData();
        }

        private void ClearPasswordsFromBrowser()
        {
            if (ClearPasswordsFromBrowsers)
            {
                if (!BrowserHelper.IsIEOpened)
                {
                    pbData.ClearIEAccounts();
                }
                if (!BrowserHelper.IsChromeRunning)
                {
                    pbData.ClearChromeAccounts();
                }
                if (!BrowserHelper.IsFFOpened)
                {
                    pbData.ClearFFAccounts();
                }
            }
        }

        private void EnableDisableStoringPasswordInBrowsers()
        {
            if (TurnOffPassSaving)
            {
                if (!BrowserHelper.IsIEOpened)
                {
                    pbData.DisablePasswordsIE();
                }
                if (!BrowserHelper.IsChromeRunning)
                {
                    pbData.DisablePasswordsChrome();
                }
                if (!BrowserHelper.IsFFOpened)
                {
                    pbData.DisablePasswordsFF();
                }
            }
            else
            {
                if (!BrowserHelper.IsIEOpened)
                {
                    pbData.EnableStoringPasswordIE();
                }
                if (!BrowserHelper.IsChromeRunning)
                {
                    pbData.EnableStoringPasswordChrome();
                }
                if (!BrowserHelper.IsFFOpened)
                {
                    pbData.EnableStoringPasswordFF();
                }
            }
        }

        private void LoadTabPreferences()
        {
            _enableStorageCloudBackupChanged = false;

            string tmpCloudEnabled = pbData.GetPrivateSetting(DefaultProperties.Settings_CloudStorage);

            if (tmpCloudEnabled != null)
            {
                bool tmpBool = false;
                if (bool.TryParse(tmpCloudEnabled, out tmpBool)) EnableStorageCloudBackup = tmpBool;
            }
            else
            {
                EnableStorageCloudBackup = false;
            }

            BackupEnabled = EnableStorageCloudBackup;
            InitializeDataStorageGrid();

            TabPreferencesChanged = false;

            if (EnableStorageCloudBackup && StorageRegions != null)
            {
                CloudBackupMainWindow = (string)System.Windows.Application.Current.FindResource("Enabled");
                StorageRegion s = StorageRegions.FirstOrDefault(p => p.Checked);

                if (s != null)
                {
                    DataStorageMainWindow = s.Name;
                }

                CloudBackupColor = ReturnTextColor(GreenForegroundColor);
            }
            else
            {
                CloudBackupMainWindow = CloudBackupMainWindow = (string)System.Windows.Application.Current.FindResource("Disabled");
                DataStorageMainWindow = (string)System.Windows.Application.Current.FindResource("LocalDataStorage");
                CloudBackupColor = ReturnTextColor(RedForegroundColor);
            }
        }

        private void SaveTabPreferences(object obj)
        {
            //LoadingWindow lw = new LoadingWindow();
            //lw.Show();

            try
            {
                if (api == null)
                {
                    api = _resolver.GetInstanceOf<IPBWebAPI>();
                }


                var resp = api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new PasswordBoss.WEBApiJSON.AccountRequest { storage_region = CurrentStorageRegionUUID, email = pbData.ActiveUser, installation = pbData.InstallationUUID, synchronize = EnableStorageCloudBackup });

                if (resp == null || resp.error != null || resp.account == null || resp.account.storage_region == null || resp.account.storage_region.uuid != CurrentStorageRegionUUID)
                {
                    if (resp != null && resp.error != null)
                    {
                        logger.Error(resp.error);
                    }

                    if (resp == null)
                    {
                        if (!api.HasInetConn)
                        {
                            throw new Exception("Connection");
                        }

                    }
                    throw new Exception("Server");

                }

                if (EnableStorageCloudBackup)
                {
                    CloudBackupMainWindow = (string)System.Windows.Application.Current.FindResource("Enabled");
                    StorageRegion s = StorageRegions.FirstOrDefault(p => p.Checked);
                    DataStorageMainWindow = s.Name;
                    CloudBackupColor = ReturnTextColor(GreenForegroundColor);
                }
                else
                {
                    CloudBackupMainWindow = CloudBackupMainWindow = (string)System.Windows.Application.Current.FindResource("Disabled");
                    DataStorageMainWindow = (string)System.Windows.Application.Current.FindResource("LocalDataStorage");
                    CloudBackupColor = ReturnTextColor(RedForegroundColor);
                }

                pbData.ChangePrivateSetting(DefaultProperties.Settings_CloudStorage, EnableStorageCloudBackup.ToString());

                pbData.UpdateCurrentStorageRegion(CurrentStorageRegionUUID);

                if (_enableStorageCloudBackupChanged)
                {
                    _enableStorageCloudBackupChanged = false;
                    var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
                    if (inAppAnalyitics != null)
                    {
                        inAppAnalyitics.Get<Events.BackupEnabled, YesOrNo>().Log(EnableStorageCloudBackup ? YesOrNo.Y : YesOrNo.N);
                    }
                }

                //try
                //{
                //    StorageRegion tmp = StorageRegions.FirstOrDefault(p => p.Checked);
                //    DataStorageMainWindow = tmp.Name;
                //}
                //catch (Exception ex)
                //{
                //    logger.Error(ex.Message);
                //}
            }
            catch (Exception ex)
            {
                string _dialogText = string.Empty;
                switch (ex.Message)
                {
                    case "Connection":
                        _dialogText = System.Windows.Application.Current.FindResource("UserNotConnectedToNetwork") as string;
                        break;
                    case "Server":
                        _dialogText = System.Windows.Application.Current.FindResource("ServerErrorLoginMessage") as string;
                        break;
                    default:
                        _dialogText = System.Windows.Application.Current.FindResource("GeneralErrorText") as string;
                        break;
                }
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    PasswordBoss.UserControls.CustomInformationMessageBoxWindow c = new PasswordBoss.UserControls.CustomInformationMessageBoxWindow(_dialogText);
                    c.Show();
                });

                //logger.Error(ex.Message);
            }

            //lw.Close();

            LoadTabPreferences();
        }

        private void LoadTabGeneralDataNonInputFields()
        {
            var licence = pbData.GetSubscriptionInfo();

            if (licence != null)
            {
                UpgradeVisibility = false;
                RenewVisibility = false;
                SubscriptionImage = (ImageSource)Application.Current.FindResource("imgRedArrow");

                if (licence.SubscriptionType == SubscriptionType.Free || licence.SubscriptionType == SubscriptionType.Trial)
                {
                    UpgradeVisibility = true;
                }

                if (licence.SubscriptionType == SubscriptionType.Paid || licence.SubscriptionType == SubscriptionType.Donation)
                {
                    SubscriptionImage = (ImageSource)Application.Current.FindResource("imgArrowGreen");
                    RenewVisibility = true;
                }

                MembershipExpiresDate = licence.ExpirationDate;
                MembershipType = licence.LocalizedSubscriptionType;
            }
        }

        private void LoadTabGeneraData()
        {
            var userInfo = pbData.GetUserInfo(pbData.ActiveUser);

            if (userInfo != null)
            {
                FirstName = userInfo.FirstName;
                LastName = userInfo.LastName;
                Mobile = userInfo.Phone;
                Email = userInfo.Email;
                //MembershipType = userInfo.Subscription;
            }

            // load languages
            if (Languages == null || Languages.Count == 0)
            {
                Languages = pbData.GetLanguages();
            }


            //load countries and set default or saved
            if (Countries == null || Countries.Count == 0)
            {
                Countries = pbData.GetCountryList();
            }

            string country = pbData.GetPrivateSetting(DefaultProperties.Settings_Country);
            if (String.IsNullOrEmpty(country))
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Country, DefaultProperties.Settings_DefaultCountryCode);
                country = DefaultProperties.Settings_DefaultCountryCode;
            }
            SelectedCountry = Countries.FirstOrDefault(x => x.Code == country);

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                var last_selected_lang = pbData.GetConfigurationValueByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, "last_selected_lang");
                if (last_selected_lang != null)
                {
                    SelectedLanguage = Languages.FirstOrDefault(x => x.Code.ToLower() == last_selected_lang.ToLower());
                }
                else
                {
                    SelectedLanguage = Languages.FirstOrDefault(x => x.Code.ToLower() == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower());
                }
            }));
            LoadTabGeneralDataNonInputFields();

            TabGeneralChanged = false;
            ApplicationLanguageChanged = false;
        }

        private void InitializeGrid()
        {

            List<PasswordBoss.DTO.Device> device = pbData.GetDevices();
            if (device != null)
                SyncItemSource = device.Select(p => new syncdevices { date = DateTimeOffset.Parse(p.CreatedDateString).UtcDateTime, devicename = p.Nickname, uuid = p.UUID, InstallationId = p.InstallationId, DeleteEnable = pbData.InstallationUUID != p.InstallationId }).ToList();
        }

        private void InitializeDisabledSitesGrid()
        {
            List<PasswordBoss.DTO.Site> blacklist = pbData.GetBlacklistedSites();
            if (blacklist != null)
                DisabledItemSource = blacklist.Select(p => new disabledSite { uuid = p.UUID, sitename = p.FriendlyName, date = p.CreatedDate, type = PasswordBoss.DTO.Site.BlacklistToString(p.Blacklist) }).ToList();
        }

        void sync_OnSyncFinished(bool status)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                OnSyncFinished();
            });
        }

        private void InitializeDataStorageGrid()
        {
            StorageRegions = null;
            var currentRegion = pbData.GetUserInfo(pbData.ActiveUser);
            if (currentRegion != null)
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                bool isEnabled = featureChecker.IsEnabled(DefaultProperties.Features_SyncAndCloudStorage_OnlineBackup, showUIIfNotEnabled: false);

                CurrentStorageRegionUUID = currentRegion.StorageRegionUUID;
                var regions = pbData.GetStorageRegions();
                if (regions != null && currentRegion != null)
                    StorageRegions = new ObservableCollection<StorageRegionModel>(regions.Select(p => new StorageRegionModel { UUID = p.UUID, Name = p.Name, Checked = p.UUID == currentRegion.StorageRegionUUID && isEnabled }).ToList());
            }

        }

        public RelayCommand AccountSettingCommand { get; set; }
        public RelayCommand AccSettingPasswordVaultCommand { get; set; }
        public RelayCommand AccSettingDigitalWalletCommand { get; set; }
        public RelayCommand AccSettingPersonalInfoCommand { get; set; }
        public RelayCommand ChangeMasterPasswordCommand { get; set; }
        public RelayCommand ChangePINCommand { get; set; }
        public RelayCommand SetAutoLockCommand { get; set; }
        public RelayCommand SyncDeleteCommand { get; set; }
        public RelayCommand DeletePopupCancelCommand { get; set; }
        public RelayCommand DeletePopupButtonCommand { get; set; }
        public RelayCommand StorageRegionChangedCommand { get; set; }
        //public RelayCommand ClearFromBrowsersCommand { get; set; }
        public RelayCommand ClearSitesAutoSaveCommand { get; set; }


        //public RelayCommand MessageBoxStorageRegionChangeConfirmCommand { get; set; }
        //public RelayCommand MessageBoxStorageRegionChangeCancelCommand { get; set; }
        //public RelayCommand MessageBoxEnableStorageBackupOnCloudConfirmCommand { get; set; }
        //public RelayCommand MessageBoxEnableStorageBackupOnCloudCancelCommand { get; set; }
        public RelayCommand MessageBoxMasterPasswordChangedOKCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> MessageBoxAccountSettingsChangedConfirmCommand { get; set; }
        public RelayCommand MessageBoxAccountSettingsChangedCancelCommand { get; set; }
        public RelayCommand EnableStorageCloudBackupCommand { get; set; }
        public RelayCommand DeleteFolderCommand { get; set; }
        public RelayCommand EditFolderCommand { get; set; }
        public RelayCommand AddNewFolderCommand { get; set; }
        public RelayCommand ApplyPasswordBoxFocusedStyleCommand { get; set; }
        public RelayCommand ApplyPasswordBoxDefaultStyleCommand { get; set; }
        public RelayCommand LoginPortalCommand { get; set; }
        public RelayCommand TabLostFocusCommand { get; set; }
        public RelayCommand TabControlLostFocusCommand { get; set; }
        public RelayCommand AccountSettingsComboboxChangedCommand { get; set; }
        public RelayCommand ExistingPinLostFocusCommand { get; set; }
        public RelayCommand NewPinLostFocusCommand { get; set; }
        public RelayCommand ConfirmPinLostFocusCommand { get; set; }
        public RelayCommand ExistingPinChangedCommand { get; set; }
        public RelayCommand NewPinChangedCommand { get; set; }
        public RelayCommand ConfirmPinChangedCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> SaveTabGeneralDataCommand { get; set; }
        public RelayCommand SaveTabAdvancedDataCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> SaveTabPreferencesCommand { get; set; }
        public RelayCommand UpgradeCommand { get; set; }
        public RelayCommand RenewCommand { get; set; }
        public RelayCommand ShowTwoStepVerificationDialogCommand { get; set; }
        public RelayCommand MasterPasswordChangeCommand { get; set; }
        public RelayCommand MasterPasswordChangeCancelCommand { get; set; }
        public RelayCommand PinChangeCommand { get; set; }
        public RelayCommand PinChangeCancelCommand { get; set; }
        public RelayCommand SortCategoriesAscendingCommand { get; set; }
        public RelayCommand SortCategoriesDescendingCommand { get; set; }
        public RelayCommand SortCategoriesCommand { get; set; }


        private void InitializeCommands()
        {
            AccountSettingCommand = new RelayCommand(AccountSettingClick);
            AccSettingPasswordVaultCommand = new RelayCommand(Acc_SettingPasswordVaultClick);
            AccSettingDigitalWalletCommand = new RelayCommand(Acc_SettingDigitalWalletClick);
            AccSettingPersonalInfoCommand = new RelayCommand(Acc_SettingPersonalInfoClick);
            ChangeMasterPasswordCommand = new RelayCommand(ChangeMasterPasswordClick);
            ChangePINCommand = new RelayCommand(ChangePINClick);
            SetAutoLockCommand = new RelayCommand(SetAutoLockClick);
            SyncDeleteCommand = new RelayCommand(SyncDeleteClick);
            DeletePopupCancelCommand = new RelayCommand(DeletePopupCancelClick);
            DeletePopupButtonCommand = new RelayCommand(DeletePopupButtonClick);
            StorageRegionChangedCommand = new RelayCommand(StorageRegionChanged);
            //ClearFromBrowsersCommand = new RelayCommand(ClearFromBrowsersClick);
            ClearSitesAutoSaveCommand = new RelayCommand(ClearSitesAutoSaveClick);
            //MessageBoxStorageRegionChangeConfirmCommand = new RelayCommand(MessageBoxStorageRegionChangeConfirm);
            //MessageBoxStorageRegionChangeCancelCommand = new RelayCommand(MessageBoxStorageRegionChangeCancel);
            //MessageBoxEnableStorageBackupOnCloudConfirmCommand = new RelayCommand(MessageBoxEnableStorageBackupOnCloudConfirm);
            //MessageBoxEnableStorageBackupOnCloudCancelCommand = new RelayCommand(MessageBoxEnableStorageBackupOnCloudCancel);
            MessageBoxMasterPasswordChangedOKCommand = new RelayCommand((o) => { MessageBoxMasterPasswordChangedVisibility = false; });
            MessageBoxAccountSettingsChangedConfirmCommand = new AsyncRelayCommand<LoadingWindow>(MessageBoxAccountSettingsChangedConfirm, null, null, null, null, null, null, true);
            MessageBoxAccountSettingsChangedCancelCommand = new RelayCommand(MessageBoxAccountSettingsChangedCancel);
            EnableStorageCloudBackupCommand = new RelayCommand(EnableStorageCloudBackupChanged);
            DeleteFolderCommand = new RelayCommand(DeleteFolderClick);
            EditFolderCommand = new RelayCommand(EditFolderClick);
            AddNewFolderCommand = new RelayCommand(AddNewFolderClick);
            ApplyPasswordBoxFocusedStyleCommand = new RelayCommand(ApplyPasswordBoxFocusedStyle);
            ApplyPasswordBoxDefaultStyleCommand = new RelayCommand(ApplyPasswordBoxDefaultStyle);
            LoginPortalCommand = new RelayCommand(LoginPortal);
            TabLostFocusCommand = new RelayCommand(TabLostFocus);
            TabControlLostFocusCommand = new RelayCommand(TabControlLostFocus);
            AccountSettingsComboboxChangedCommand = new RelayCommand(AccountSettingsComboboxChanged);
            ExistingPinLostFocusCommand = new RelayCommand(ExistingPinLostFocus);
            NewPinLostFocusCommand = new RelayCommand(NewPinLostFocus);
            ConfirmPinLostFocusCommand = new RelayCommand(ConfirmPinLostFocus);
            ExistingPinChangedCommand = new RelayCommand(ExistingPinChanged);
            NewPinChangedCommand = new RelayCommand(NewPinChanged);
            ConfirmPinChangedCommand = new RelayCommand(ConfirmPinChanged);
            SaveTabGeneralDataCommand = new AsyncRelayCommand<LoadingWindow>(SaveTabGeneralData, null, null, null, null, null, null, true);
            SaveTabAdvancedDataCommand = new RelayCommand(SaveTabAdvancedData);
            SaveTabPreferencesCommand = new AsyncRelayCommand<LoadingWindow>(SaveTabPreferences, null, null, null, null, null, null, true);
            UpgradeCommand = new RelayCommand(Upgrade);
            RenewCommand = new RelayCommand(Renew);
            ShowTwoStepVerificationDialogCommand = new RelayCommand(ShowTwoStepVerificationDialog);
            MasterPasswordChangeCommand = new RelayCommand(MasterPasswordChange);
            MasterPasswordChangeCancelCommand = new RelayCommand(MasterPasswordChangeCancel);
            PinChangeCommand = new RelayCommand(PinChange);
            PinChangeCancelCommand = new RelayCommand(PinChangeCancel);

        }

        private Configuration securityConfigRememberLastLogin;
        private Configuration securityConfigEnablePinAccess;
        //private Configuration securityConfigStorageCloudBackupEnabled;


        private void SaveTabGeneralData(object obj)
        {
            if (TabGeneralChanged)
            {
                try
                {
                    if (api == null)
                    {
                        api = _resolver.GetInstanceOf<IPBWebAPI>();
                    }

                    var oldUserInfo = pbData.GetUserInfo(pbData.ActiveUser);

                    var resp = api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID,
                        new PasswordBoss.WEBApiJSON.AccountRequest
                        {
                            first_name = _firstName,
                            last_name = _lastName,
                            phone = mobile,
                            email = pbData.ActiveUser,
                            language = selectedLanguage != null ? selectedLanguage.Code : string.Empty
                            /*, 
                            country = selectedCountry != null ? selectedCountry.Code : string.Empty*/
                        });

                    if (resp == null)
                    {
                        throw new Exception("UpdateAccount API call failed");
                    }

                    if (resp.error != null)
                    {
                        throw new Exception("UpdateAccount API returned error with code " + resp.error.code);
                    }

                    UserInfo info = new UserInfo { FirstName = FirstName, LastName = LastName, Phone = Mobile };
                    if (!pbData.UpdateUserInfo(info))
                    {
                        throw new Exception("Error update user info");
                    }

                    //Save First Name personal info
                    if (string.IsNullOrWhiteSpace(oldUserInfo.FirstName) && string.IsNullOrWhiteSpace(oldUserInfo.LastName)
                        && (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName)) && (FirstName != oldUserInfo.FirstName || LastName != oldUserInfo.LastName))
                    {
                        var siName = new SecureItem();
                        siName.SecureItemTypeName = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
                        siName.Type = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names;
                        siName.Data = new SecureItemData();
                        siName.Data.firstName = FirstName;
                        siName.Data.lastName = LastName;
                        siName.Name = String.Format("{0} {1}", _firstName, _lastName);
                        siName.Folder = pbData.GetFoldersBySecureItemType().FirstOrDefault(x => x.Id == DefaultCategories.CategoryNames);
                        siName = pbData.AddOrUpdateSecureItem(siName);
                    }

                    //Save Mobile personal info
                    if (string.IsNullOrWhiteSpace(oldUserInfo.Phone) && !string.IsNullOrWhiteSpace(Mobile) && oldUserInfo.Phone != Mobile)
                    {
                        var siMobile = new SecureItem();
                        siMobile.SecureItemTypeName = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
                        siMobile.Type = SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber;
                        siMobile.Name = !String.IsNullOrEmpty(_firstName) && !String.IsNullOrEmpty(_lastName) ? String.Format("{0} {1}", _firstName, _lastName) : String.Format("{0}", mobile);
                        siMobile.Data = new SecureItemData();
                        siMobile.Data.phoneNumber = mobile;
                        siMobile.Folder = pbData.GetFoldersBySecureItemType().FirstOrDefault(x => x.Id == DefaultCategories.CategoryPhoneNumbers);
                        siMobile = pbData.AddOrUpdateSecureItem(siMobile);
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (selectedLanguage != null && ApplicationLanguageChanged)
                        {
                            //Save Language
                            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(selectedLanguage.Code);
                            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(selectedLanguage.Code);

                            Configuration config = new Configuration() { AccountEmail = DefaultProperties.Configuration_DefaultAccount, Key = "last_lang", Value = selectedLanguage.Code };
                            pbData.AddOrUpdateConfiguration(config);
                            pbData.ChangeDefaultSetting("last_selected_lang", selectedLanguage.Code);

                            //TODO
                            if (appCmd != null) appCmd.SetLanguage(selectedLanguage.Code);
                        }
                    }));
                    //Save Country
                    if (selectedCountry != null)
                        pbData.ChangePrivateSetting(DefaultProperties.Settings_Country, selectedCountry.Code);

                }
                catch (Exception ex)
                {
                    MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                    logger.Error(ex.Message);
                }



                LoadTabGeneraData();
            }
        }

        private ImageSource _subscriptionImage = (ImageSource)Application.Current.FindResource("imgArrowGreen");

        public ImageSource SubscriptionImage
        {
            get { return _subscriptionImage; }
            set
            {
                _subscriptionImage = value;
                RaisePropertyChanged("SubscriptionImage");
            }
        }

        private bool _tabGeneralChanged;
        public bool TabGeneralChanged
        {
            get { return _tabGeneralChanged; }
            set { _tabGeneralChanged = value; }
        }

        private bool _applicationLanguageChanged = false;
        public bool ApplicationLanguageChanged
        {
            get { return _applicationLanguageChanged; }
            set { _applicationLanguageChanged = value; }
        }

        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set
            {
                if (mobile != value)
                {
                    mobile = value;
                    TabGeneralChanged = true;
                    RaisePropertyChanged("Mobile");
                }
            }
        }




        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    TabGeneralChanged = true;
                    RaisePropertyChanged("FirstName");
                }
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    TabGeneralChanged = true;
                    RaisePropertyChanged("LastName");
                }
            }
        }

        //private string name;
        //public string Name
        //{
        //    get { return name; }
        //    set
        //    {
        //        if (name != value)
        //        {

        //            name = value;
        //            if (!String.IsNullOrEmpty(value))
        //            {
        //                if (value.Contains(" "))
        //                {
        //                    List<string> names = value.Split(' ').ToList<string>();
        //                    if (names.Count > 2)
        //                    {
        //                        siName.Data.lastName = names[names.Count - 1];
        //                        names.RemoveAt(names.Count - 1);
        //                        siName.Data.firstName = String.Join(" ", names.ToArray());
        //                    }
        //                    else
        //                    {
        //                        siName.Data.firstName = names[0];
        //                        siName.Data.lastName = names[1];
        //                    }

        //                }
        //                else
        //                    siName.Data.firstName = value;
        //                pbData.AddOrUpdateSecureItem(siName);
        //            }
        //            else
        //            { pbData.ChangePrivateSetting(DefaultProperties.Settings_Name, null); }
        //            RaisePropertyChanged("Name");
        //        }
        //    }
        //}

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                //if (email != value)
                //{
                //    email = value;
                //    if (!String.IsNullOrEmpty(value))
                //    {
                //        siEmail.Data.email = value;
                //        pbData.AddOrUpdateSecureItem(siEmail);
                //    }
                //    else
                //    {
                //        { pbData.ChangePrivateSetting(DefaultProperties.Settings_Email, null); }
                //    }
                //    RaisePropertyChanged("Email");
                //}

                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private bool autoFill;
        public bool AutoFill
        {
            get { return autoFill; }
            set
            {
                if (autoFill != value)
                {
                    autoFill = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_General_AutoFill, value.ToString());
                    RaisePropertyChanged("AutoFill");
                }
            }
        }

        private bool openOnStartup;
        public bool OpenOnStartup
        {
            get { return openOnStartup; }
            set
            {
                if (openOnStartup != value)
                {
                    openOnStartup = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_General_OpenOnStartup, value.ToString());
                    RaisePropertyChanged("OpenOnStartup");
                }
            }
        }

        private bool twoStepTrustedDevice;
        public bool TwoStepTrustedDevice
        {
            get
            {
                bool.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrusted), out twoStepTrustedDevice);
                return twoStepTrustedDevice;
            }
            set
            {
                if (twoStepTrustedDevice != value)
                {
                    openOnStartup = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrusted, value.ToString());
                    RaisePropertyChanged("TwoStepTrustedDevice");
                }
            }
        }

        private bool twoStepVerification;
        public bool TwoStepVerification
        {
            get { return twoStepVerification; }
            set
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_TwoStepAuthentication, showUIIfNotEnabled: false))
                {
                    value = false;
                }
                if (twoStepVerification != value)
                {
                    twoStepVerification = value;
                    if (value == false)
                    {
                        if (api == null)
                        {
                            api = _resolver.GetInstanceOf<IPBWebAPI>();
                        }
                        var resp = api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new PasswordBoss.WEBApiJSON.AccountRequest { multi_factor_authentication = value });

                        if (resp == null)
                        {
                            throw new Exception("API call failed");
                        }
                        pbData.UpdateMultiFactorAuthentication(value); //THIS won't come from dialog so we need to disable it here

                        var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
                        if (inAppAnalyitics != null)
                        {
                            inAppAnalyitics.Get<Events.TwoFactorVerificationEvent, AOItemType>().Log(AOItemType.Disabled);
                        }

                        TwoStepTrustedDevice = false;
                    }
                    //pbData.ChangePrivateSetting(DefaultProperties.Settings_General_TwoStepVerification, value.ToString());
                    RaisePropertyChanged("TwoStepVerification");
                }
            }
        }

        private bool _tabAdvancedChanged = false;
        public bool TabAdvancedChanged
        {
            get { return _tabAdvancedChanged; }
            set { _tabAdvancedChanged = value; }
        }

        private bool _tabPreferencesChanged;
        public bool TabPreferencesChanged
        {
            get { return _tabPreferencesChanged; }
            set { _tabPreferencesChanged = value; }
        }

        private bool _tabSyncChanged = false;
        public bool TabSyncChanged
        {
            get { return _tabSyncChanged; }
            set
            {
                _tabSyncChanged = value;
            }
        }

        private bool _tabDisabledSitesChanged = false;
        public bool TabDisabledSitesChanged
        {
            get { return _tabDisabledSitesChanged; }
            set
            {
                _tabDisabledSitesChanged = value;
            }
        }

        private bool autoLogin;
        public bool AutoLogin
        {
            get { return autoLogin; }
            set
            {
                if (autoLogin != value)
                {
                    autoLogin = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin, autoLogin.ToString());
                    RaisePropertyChanged("AutoLogin");
                    //TabAdvancedChanged = true;
                }
            }
        }

        private bool clearPasswordsFromBrowsers;

        public bool ClearPasswordsFromBrowsers
        {
            get { return clearPasswordsFromBrowsers; }
            set
            {
                if (clearPasswordsFromBrowsers != value)
                {
                    clearPasswordsFromBrowsers = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_ClearPasswordsFromBrowsers, clearPasswordsFromBrowsers.ToString());
                    RaisePropertyChanged("ClearPasswordsFromBrowsers");
                    if (value)
                    {
                        ClearPasswordsFromBrowser();
                    }

                    //TabAdvancedChanged = true;
                }
            }
        }

        //private bool rememberForLogin;
        //public bool RememberForLogin
        //{
        //    get { return rememberForLogin; }
        //    set
        //    {
        //        if (rememberForLogin != value)
        //        {
        //            rememberForLogin = value;
        //            pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_RememberForLogin, value.ToString());
        //            RaisePropertyChanged("RememberForLogin");
        //        }
        //    }
        //}

        private bool requireMasterPasswordForAll;
        public bool RequireMasterPasswordForAll
        {
            get { return requireMasterPasswordForAll; }
            set
            {
                if (requireMasterPasswordForAll != value)
                {
                    requireMasterPasswordForAll = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_RequireMasterPasswordForAll, requireMasterPasswordForAll.ToString());
                    RaisePropertyChanged("RequireMasterPasswordForAll");
                    //TabAdvancedChanged = true;
                }
            }
        }

        //private bool clearPasswords;
        //public bool ClearPasswords
        //{
        //    get { return clearPasswords; }
        //    set
        //    {
        //        if (clearPasswords != value)
        //        {
        //            clearPasswords = value;
        //            pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_ClearPasswords, value.ToString());
        //            RaisePropertyChanged("ClearPasswords");
        //        }
        //    }
        //}

        private bool turnOffPassSaving;
        public bool TurnOffPassSaving
        {
            get { return turnOffPassSaving; }
            set
            {
                if (turnOffPassSaving != value)
                {
                    turnOffPassSaving = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving, turnOffPassSaving.ToString());
                    EnableDisableStoringPasswordInBrowsers();
                    RaisePropertyChanged("TurnOffPassSaving");
                    //TabAdvancedChanged = true;
                }
            }
        }

        //private bool newTabPage;
        //public bool NewTabPage
        //{
        //    get { return newTabPage; }
        //    set
        //    {
        //        if (newTabPage != value)
        //        {
        //            newTabPage = value;
        //            pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_NewTabPage, value.ToString());
        //            RaisePropertyChanged("NewTabPage");
        //        }
        //    }
        //}

        private bool disableStatusMessages;
        public bool DisableStatusMessages
        {
            get { return disableStatusMessages; }
            set
            {
                if (disableStatusMessages != value)
                {
                    disableStatusMessages = value;

                    bool disableStatusMessagesOld = false;
                    Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages), out disableStatusMessagesOld);
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages, disableStatusMessages.ToString());
                    LogStatusMessagesAnalytcis(disableStatusMessagesOld);
                    RaisePropertyChanged("DisableStatusMessages");
                    //TabAdvancedChanged = true;
                }
            }
        }

        private bool autoStoreData;
        public bool AutoStoreData
        {
            get { return autoStoreData; }
            set
            {
                if (autoStoreData != value)
                {
                    autoStoreData = value;
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData, autoStoreData.ToString());
                    RaisePropertyChanged("AutoStoreData");
                    //TabAdvancedChanged = true;
                }
            }
        }
        public List<Country> Countries { get; set; }
        private Country selectedCountry;
        public Country SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                if (selectedCountry != value && value != null)
                {
                    selectedCountry = value;

                    TabGeneralChanged = true;
                    RaisePropertyChanged("SelectedCountry");
                }
            }
        }

        public List<Language> Languages { get; set; }
        private Language selectedLanguage;
        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                if (value != null)
                {
                    if (selectedLanguage != value)
                    {
                        TabGeneralChanged = true;
                        ApplicationLanguageChanged = true;
                    }
                    selectedLanguage = value;
                    RaisePropertyChanged("SelectedLanguage");
                }
            }
        }

        private bool _securityRememberLastLogin;
        public bool SecurityRememberLastLogin
        {
            get
            {
                return _securityRememberLastLogin;
            }
            set
            {
                if (_securityRememberLastLogin != value)
                {
                    _securityRememberLastLogin = value;


                    if (securityConfigRememberLastLogin == null)
                    {
                        securityConfigRememberLastLogin = new Configuration()
                        {
                            AccountEmail = pbData.ActiveUser,
                            Key = DefaultProperties.Configuration_Key_RememberEmail,
                            Active = true

                        };
                    }
                    securityConfigRememberLastLogin.Value = value.ToString();
                    pbData.AddOrUpdateConfiguration(securityConfigRememberLastLogin);

                    Configuration currentLoginAccount = pbData.GetConfigurationByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, DefaultProperties.Configuration_Key_LastLoginEmail);
                    if (value)
                    {
                        if (currentLoginAccount == null)
                        {
                            currentLoginAccount = new PasswordBoss.DTO.Configuration()
                            {
                                AccountEmail = DefaultProperties.Configuration_DefaultAccount,
                                Key = DefaultProperties.Configuration_Key_LastLoginEmail,
                                Value = pbData.ActiveUser,
                                Active = true
                            };
                        }
                        else
                        {
                            currentLoginAccount.Value = pbData.ActiveUser;
                        }
                        pbData.AddOrUpdateConfiguration(currentLoginAccount);
                    }
                    else
                    {
                        if (currentLoginAccount != null)
                        {
                            currentLoginAccount.Active = false;
                            pbData.AddOrUpdateConfiguration(currentLoginAccount);
                        }
                    }

                    RaisePropertyChanged("SecurityRememberLastLogin");
                }
            }
        }

        private bool _sortCategoriesDescendingVisibility;

        public bool SortCategoriesDescendingVisibility
        {
            get { return _sortCategoriesDescendingVisibility; }
            set
            {
                _sortCategoriesDescendingVisibility = value;
                RaisePropertyChanged("SortCategoriesDescendingVisibility");
            }
        }

        private bool _sortCategoriesAscendingVisibility;

        public bool SortCategoriesAscendingVisibility
        {
            get { return _sortCategoriesAscendingVisibility; }
            set
            {
                _sortCategoriesAscendingVisibility = value;
                RaisePropertyChanged("SortCategoriesAscendingVisibility");
            }
        }

        private bool _renewVisibility;
        public bool RenewVisibility
        {
            get { return _renewVisibility; }
            set
            {
                _renewVisibility = value;
                RaisePropertyChanged("RenewVisibility");
            }
        }

        private bool _upgradeVisibility;
        public bool UpgradeVisibility
        {
            get { return _upgradeVisibility; }
            set
            {
                _upgradeVisibility = value;
                RaisePropertyChanged("UpgradeVisibility");
            }
        }

        private bool _progressBarVisibility;

        public bool ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set
            {
                _progressBarVisibility = value;
                RaisePropertyChanged("ProgressBarVisibility");
            }
        }

        private bool _syncEnabled = true;

        public bool SyncEnabled
        {
            get { return _syncEnabled; }
            set
            {
                _syncEnabled = value;
                RaisePropertyChanged("SyncEnabled");
                //BackupEnabled = value;
            }
        }

        public bool _backupEnabled;
        public bool BackupEnabled
        {
            get { return _backupEnabled; }
            set
            {
                _backupEnabled = value;
                RaisePropertyChanged("BackupEnabled");
            }
        }

        private int _progressBarValue;

        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                if (_progressBarMaxValue > 0)
                {
                    ProgresBarPercentage = (int)decimal.Round((decimal)_progressBarValue / (((decimal)_progressBarMaxValue) / 100), 0);
                }

                RaisePropertyChanged("ProgressBarValue");
            }
        }

        private int _progressBarMaxValue;

        public int ProgressBarMaxValue
        {
            get { return _progressBarMaxValue; }
            set
            {
                _progressBarMaxValue = value;
                RaisePropertyChanged("ProgressBarMaxValue");
            }
        }

        private int _progresBarPercentage;

        public int ProgresBarPercentage
        {
            get { return _progresBarPercentage; }
            set
            {
                _progresBarPercentage = value;
                RaisePropertyChanged("ProgresBarPercentage");
            }
        }

        private bool _opaqueGridVisibility;
        public bool OpaqueGridVisibility
        {
            get { return _opaqueGridVisibility; }
            set
            {
                _opaqueGridVisibility = value;
                RaisePropertyChanged("OpaqueGridVisibility");
            }
        }

        private bool _progressBarOpaqueGridVisibility;
        public bool ProgressBarOpaqueGridVisibility
        {
            get { return _progressBarOpaqueGridVisibility; }
            set
            {
                _progressBarOpaqueGridVisibility = value;
                RaisePropertyChanged("ProgressBarOpaqueGridVisibility");
            }
        }


        private string _deletePopupHeader;
        public string DeletePopupHeader
        {
            get { return _deletePopupHeader; }
            set
            {
                if (_deletePopupHeader != value)
                {
                    _deletePopupHeader = value;
                    RaisePropertyChanged("DeletePopupHeader");
                }
            }
        }
        private string _deletePopupText;
        public string DeletePopupText
        {
            get { return _deletePopupText; }
            set
            {
                if (_deletePopupText != value)
                {
                    _deletePopupText = value;
                    RaisePropertyChanged("DeletePopupText");
                }
            }
        }

        private bool _deletePopupVisibility;
        public bool DeletePopupVisibility
        {
            get { return _deletePopupVisibility; }
            set
            {
                _deletePopupVisibility = value;
                RaisePropertyChanged("DeletePopupVisibility");
            }
        }

        private bool _accountSettingVisibility;
        public bool AccountSettingVisibility
        {
            get { return _accountSettingVisibility; }
            set
            {
                _accountSettingVisibility = value;
                RaisePropertyChanged("AccountSettingVisibility");
            }
        }

        private ImageSource _accountSettingIcon = DefaultProperties.ReturnImage(AccountSettingImage);

        public ImageSource AccountSettingIcon
        {
            get { return _accountSettingIcon; }
            set
            {
                if (Equals(_accountSettingIcon, value)) return;
                _accountSettingIcon = value;
                RaisePropertyChanged("AccountSettingIcon");
            }
        }

        private int _passwordMeterValue;

        public int PasswordMeterValue
        {
            get { return _passwordMeterValue; }
            set
            {
                _passwordMeterValue = value;
                RaisePropertyChanged("PasswordMeterValue");
            }
        }

        private ImageSource _backupImage;

        public ImageSource BackupImage
        {
            get { return _backupImage; }
            set
            {
                _backupImage = value;
                RaisePropertyChanged("BackupImage");
            }
        }


        public string _passwordMeterText;

        public string PasswordMeterText
        {
            get { return _passwordMeterText; }
            set
            {
                _passwordMeterText = value;
                RaisePropertyChanged("PasswordMeterText");
            }
        }

        public void ChangePrivateSetting(string key, string value)
        {
            pbData.ChangePrivateSetting(key, value.ToString());
        }

        #region MasterPwd properties

        // IsChecked properties for radio button 
        private bool _charactersChecked;

        public bool CharactersChecked
        {
            get
            {
                return _charactersChecked;
            }
            set
            {
                if (_charactersChecked != value)
                {
                    _charactersChecked = value;
                    RaisePropertyChanged("CharactersChecked");
                }
            }
        }

        private bool _numbersChecked;

        public bool NumbersChecked
        {
            get
            {
                return _numbersChecked;
            }
            set
            {
                if (_numbersChecked != value)
                {
                    _numbersChecked = value;
                    RaisePropertyChanged("NumbersChecked");
                }
            }
        }

        private bool _lettersChecked;

        public bool LettersChecked
        {
            get
            {
                return _lettersChecked;
            }
            set
            {
                if (_lettersChecked != value)
                {
                    _lettersChecked = value;
                    RaisePropertyChanged("LettersChecked");
                }
            }
        }

        private bool _symbolsChecked;

        public bool SymbolsChecked
        {
            get
            {
                return _symbolsChecked;
            }
            set
            {
                if (_symbolsChecked != value)
                {
                    _symbolsChecked = value;
                    RaisePropertyChanged("SymbolsChecked");
                }
            }
        }

        private bool _capitalChecked;

        public bool CapitalChecked
        {
            get
            {
                return _capitalChecked;
            }
            set
            {
                if (_capitalChecked != value)
                {
                    _capitalChecked = value;
                    RaisePropertyChanged("CapitalChecked");
                }
            }
        }

        private string _newMasterPasswordErrorMessage = string.Empty;
        public string NewMasterPasswordErrorMessage
        {
            get { return _newMasterPasswordErrorMessage; }
            set
            {
                if (_newMasterPasswordErrorMessage != value)
                {
                    _newMasterPasswordErrorMessage = value;
                    RaisePropertyChanged("NewMasterPasswordErrorMessage");
                }
            }
        }

        private string _existingMasterPasswordErrorMessage = string.Empty;
        public string ExistingMasterPasswordErrorMessage
        {
            get { return _existingMasterPasswordErrorMessage; }
            set
            {
                if (_existingMasterPasswordErrorMessage != value)
                {
                    _existingMasterPasswordErrorMessage = value;
                    RaisePropertyChanged("ExistingMasterPasswordErrorMessage");
                }
            }
        }

        private bool _pinChangeEnabled = false;
        public bool PinChangeEnabled
        {
            get { return _pinChangeEnabled; }
            set
            {
                _pinChangeEnabled = value;
                RaisePropertyChanged("PinChangeEnabled");
            }
        }

        private string _confirmMasterPasswordErrorMessage = string.Empty;
        public string ConfirmMasterPasswordErrorMessage
        {
            get { return _confirmMasterPasswordErrorMessage; }
            set
            {
                if (_confirmMasterPasswordErrorMessage != value)
                {
                    _confirmMasterPasswordErrorMessage = value;
                    RaisePropertyChanged("ConfirmMasterPasswordErrorMessage");
                }
            }
        }

        #endregion

        public string CurrentInstallationId { get; set; }


        #region validation master pwd checks
        /// <summary>
        /// Get the password character count based on condition whether it is numeric,symbol,letter and capital
        /// </summary>
        /// <param name="masterPassword">Used for validation</param>
        /// <param name="condition">validates based on condition</param>
        /// <returns></returns>
        internal int GetCharCount(string masterPassword, int condition)
        {
            int charCount = 0;

            switch (condition)
            {
                case 1:
                    charCount = masterPassword.Count(char.IsLower);
                    break;
                case 2:
                    charCount = masterPassword.Count(char.IsUpper);
                    break;
                case 3:
                    charCount = masterPassword.Count(char.IsDigit);
                    break;
                case 4:
                    charCount = masterPassword.Count(p => !char.IsLetterOrDigit(p));
                    break;
            }

            return charCount;
        }

        /// <summary>
        /// update character radio button UI and progress bar UI on master password validation
        /// </summary>
        void UpdateCharacterCheck(string password)
        {
            if (password.Length > PasswordLength)
            {
                CharactersChecked = true;
            }
            else
            {
                CharactersChecked = false;
            }
        }

        /// <summary>
        /// update Capital radio button UI and progress bar UI on master password validation
        /// </summary>
        void UpdateCapitalCheck(string password)
        {
            if (GetCharCount(password, Capital) > Zero)
            {
                CapitalChecked = true;
            }
            else
            {
                CapitalChecked = false;
            }
        }

        /// <summary>
        /// update Letters radio button UI and progress bar UI on master password validation
        /// </summary>
        void UpdateLettersCheck(string password)
        {
            if (GetCharCount(password, Letter) > Zero)
            {
                LettersChecked = true;
            }
            else
            {
                LettersChecked = false;
            }
        }

        /// <summary>
        /// update Symbols radio button UI and progress bar UI on master password validation
        /// </summary>
        void UpdateSymbolsCheck(string password)
        {
            if (GetCharCount(password, Symbol) > Zero)
            {
                SymbolsChecked = true;
            }
            else
            {
                SymbolsChecked = false;
            }
        }

        /// <summary>
        /// update numbers radio button UI and progress bar UI on master password validation
        /// </summary>
        void UpdateNumbersCheck(string password)
        {
            if (GetCharCount(password, Numeric) > Zero)
            {
                NumbersChecked = true;
            }
            else
            {
                NumbersChecked = false;
            }
        }

        void UpdatePasswordMeter(string password)
        {
            PasswordScanner scanner = new PasswordScanner();
            PasswordBoss.PasswordScanner.Strength s = (PasswordBoss.PasswordScanner.Strength)scanner.scanPassword(password);
            switch (s)
            {
                case (PasswordBoss.PasswordScanner.Strength.VERYWEAK):
                    PasswordMeterValue = 10;
                    PasswordMeterText = "Very weak";
                    break;
                case (PasswordBoss.PasswordScanner.Strength.GOOD):
                    PasswordMeterValue = 50;
                    PasswordMeterText = "Good";
                    break;
                case (PasswordBoss.PasswordScanner.Strength.STRONG):
                    PasswordMeterValue = 75;
                    PasswordMeterText = "Strong";
                    break;
                case (PasswordBoss.PasswordScanner.Strength.VERY_STRONG):
                    PasswordMeterValue = 100;
                    PasswordMeterText = "Very strong";
                    break;
                default:
                    PasswordMeterValue = 25;
                    PasswordMeterText = "Weak";
                    break;
            }
        }

        /// <summary>
        /// update Entire UI check
        /// </summary>
        void UpdateEntireCheck(string password)
        {
            CheckMasterPasswordValidity(CapitalChecked, LettersChecked, SymbolsChecked,
                                                                 NumbersChecked, CharactersChecked, pbData.ActiveUser, password);
        }
        #endregion

        internal bool CheckMasterPasswordValidity(bool isCapitalChecked, bool isLetterChecked,
            bool isSymbolChecked, bool isNumberChecked, bool isCharacterChecked, string emailId, string password)
        {
            var value = false;
            try
            {
                if (isCapitalChecked && isLetterChecked &&
                    isSymbolChecked && isNumberChecked && isCharacterChecked
                    && emailId != null && _commonObj.IsEmailValid(emailId) && password.Length > Zero)
                {
                    value = true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            return value;
        }

        private bool CheckMasterPasswordsMatch(PasswordTextBox pBox)
        {
            var container = FindAncestor<Grid>(pBox, "changepassword_grid");
            if (container != null)
            {
                PasswordTextBox pb = null;
                if (pBox.Name == "SetNewPasswordBox")
                {
                    pb = container.FindName("ConfirmPasswordBox") as PasswordTextBox;
                }
                else
                {
                    pb = container.FindName("SetNewPasswordBox") as PasswordTextBox;
                }

                if (pb != null && pb.GlobalPasswordTextBox.Password == pBox.GlobalPasswordTextBox.Password)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckOldMasterPassword(PasswordTextBox pBox)
        {
            PasswordTextBox pb = FindOldPasswordBox(pBox);// container.FindName("OldPasswordBox") as PasswordTextBox;

            if (pb != null && !string.IsNullOrWhiteSpace(pb.GlobalPasswordTextBox.Password) && pbData.CheckMasterPassword(pb.GlobalPasswordTextBox.Password))
            {
                return true;
            }

            return false;
        }

        private bool CheckOldMasterPasswordEqualsNew(PasswordTextBox pBox)
        {
            PasswordTextBox pb = FindOldPasswordBox(pBox);

            if (pb != null && pb.GlobalPasswordTextBox.Password.Equals(pBox.GlobalPasswordTextBox.Password))
            {
                return false;
            }

            return true;
        }

        private PasswordTextBox FindOldPasswordBox(PasswordTextBox pBox)
        {
            var container = FindAncestor<Grid>(pBox, "changepassword_grid");

            if (container != null)
            {
                PasswordTextBox pb = container.FindName("OldPasswordBox") as PasswordTextBox;

                if (pb != null)
                {
                    return pb;
                }
            }
            return null;
        }

        private void MasterPasswordChange(object obj)
        {
            var passwordBox = obj as PasswordTextBox;

            var validNewPass = CheckMasterPasswordValidity(CapitalChecked, LettersChecked, SymbolsChecked,
                                                      NumbersChecked, CharactersChecked, pbData.ActiveUser, passwordBox.GlobalPasswordTextBox.Password);
            NewMasterPasswordErrorMessage = string.Empty;
            if (!validNewPass)
            {
                NewMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("AllPasswordConditions");
            }

            if (validNewPass)
            {
                validNewPass = CheckOldMasterPasswordEqualsNew(passwordBox);
                if (!validNewPass)
                {
                    NewMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("NewAndExistingPasswordsAreTheSame");
                }
            }

            var validConfirmPass = CheckMasterPasswordsMatch(passwordBox);
            ConfirmMasterPasswordErrorMessage = string.Empty;
            if (!validConfirmPass)
            {
                ConfirmMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("PasswordDoesNowMatch");
            }

            var validOldPass = CheckOldMasterPassword(passwordBox);
            ExistingMasterPasswordErrorMessage = string.Empty;
            if (!validOldPass)
            {
                ExistingMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("WrongPassword");
            }

            if (validNewPass && validOldPass && validConfirmPass)
            {
                try
                {
                    bool failed = false;
                    SyncDevicesData(false, () => failed = true);
                    evtWrkDone.WaitOne();
                    if (!failed)
                    {
                        if (pbData.ChangeMasterPassword(passwordBox.GlobalPasswordTextBox.Password))
                        {
                            SyncDevicesData(true);
                            ClearMasterPasswordChangeForm(passwordBox);
                        }
                        else
                        {
                            MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                        }
                        MessageBoxMasterPasswordChangedVisibility = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                    logger.Error(ex.Message);
                }
            }
        }

        private void MasterPasswordChangeCancel(object obj)
        {
            var passwordBox = obj as PasswordTextBox;

            ClearMasterPasswordChangeForm(passwordBox);
        }

        private void ClearMasterPasswordChangeForm(PasswordTextBox pb)
        {
            var container = FindAncestor<Grid>(pb, "changepassword_grid");

            if (container != null)
            {
                var pBoxes = FindLogicalChildren<PasswordTextBox>(container);// container.Children.OfType<PasswordTextBox>();

                foreach (var pBox in pBoxes)
                {
                    pBox.GlobalPasswordTextBox.Password = string.Empty;
                }
            }

            ExistingMasterPasswordErrorMessage = string.Empty;
            NewMasterPasswordErrorMessage = string.Empty;
            ConfirmMasterPasswordErrorMessage = string.Empty;
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        if (child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }


        private void AccountSettingClick(object obj)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_SyncAndCloudStorage_OnlineBackup))
            {
                return;
            }
            //var listView = obj as ListView;
            //var content = obj as Grid;
            //content.Children.Clear();

            ApplyDefaultStyles();

            ApplyDefaultProperties();

            ApplyDigitalWalletItemVisibilityState();
            ExpandCollapseMenu(true);
            //AccountSettingVisibility = true;
            /*
            AccountSettingIcon = DefaultProperties.ReturnImage(AccountSettingHoverImage);
            //DisplayXamlTab = true;
            //SelectedIndexTabControl = 0;
            SyncDevicesData();
             * */
            SyncDevicesData();
        }

        private void SetAutoLockClick(object obj)
        {
            SetAutoLockVisibility = true;
            ChangePasswordVisibility = false;
            ChangePINVisibility = false;
        }

        private void ChangeMasterPasswordClick(object obj)
        {
            ChangePasswordVisibility = true;
            SetAutoLockVisibility = false;
            ChangePINVisibility = false;
            var pb = obj as PasswordTextBox;
            if (pb != null)
            {
                ClearMasterPasswordChangeForm(pb);
            }
        }

        /// <summary>
        ///  visibile change PIN grid
        /// </summary>
        /// <param name="obj"></param>
        private void ChangePINClick(object obj)
        {
            ChangePINVisibility = true;
            ChangePasswordVisibility = false;
            SetAutoLockVisibility = false;
            ClearPinChangeForm();
        }

        private void ClearPinChangeForm()
        {
            ExistingPin = string.Empty;
            NewPin = string.Empty;
            ConfirmPin = string.Empty;
            ExistingPinErrorMessage = string.Empty;
            NewPinErrorMessage = string.Empty;
            ConfirmPinErrorMessage = string.Empty;
        }

        private bool _enableStorageCloudBackup;

        public bool EnableStorageCloudBackup
        {
            get { return _enableStorageCloudBackup; }
            set
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(DefaultProperties.Features_SyncAndCloudStorage_OnlineBackup, showUIIfNotEnabled: _enableStorageCloudBackupChanged))
                {
                    value = false;
                }

                if (_enableStorageCloudBackup != value)
                {
                    TabPreferencesChanged = true;
                }
                _enableStorageCloudBackup = value;
                RaisePropertyChanged("EnableStorageCloudBackup");

                _enableStorageCloudBackupChanged = true;
                if (_enableStorageCloudBackup)
                {
                    BackupImage = (ImageSource)System.Windows.Application.Current.FindResource("backupEnabled");
                }
                else
                {
                    BackupImage = (ImageSource)System.Windows.Application.Current.FindResource("backupDisabled");
                }
                BackupEnabled = value;

                if (StorageRegions != null && StorageRegions.Count > 0)
                {
                    if (!BackupEnabled)
                    {

                        var regions = pbData.GetStorageRegions();
                        if (regions != null)
                            StorageRegions = new ObservableCollection<StorageRegionModel>(regions.Select(p => new StorageRegionModel { UUID = p.UUID, Name = p.Name, Checked = false }).ToList());
                    }
                    else
                    {
                        var regions = pbData.GetStorageRegions();
                        if (regions != null)
                            StorageRegions = new ObservableCollection<StorageRegionModel>(regions.Select(p => new StorageRegionModel { UUID = p.UUID, Name = p.Name, Checked = p.UUID == CurrentStorageRegionUUID }).ToList());
                    }
                }

            }
        }

        private bool _enableStorageCloudBackupChanged = false;


        private string _cloudBackupMainWindow;
        public string CloudBackupMainWindow
        {
            get { return _cloudBackupMainWindow; }
            set
            {
                if (_cloudBackupMainWindow != value)
                {
                    _cloudBackupMainWindow = value;
                    RaisePropertyChanged("CloudBackupMainWindow");
                }
            }
        }

        private string _dataStorageMainWindow;
        public string DataStorageMainWindow
        {
            get { return _dataStorageMainWindow; }
            set
            {
                if (_dataStorageMainWindow != value)
                {
                    _dataStorageMainWindow = value;
                    RaisePropertyChanged("DataStorageMainWindow");
                }
            }
        }

        private Brush _cloudBackupColor;

        public Brush CloudBackupColor
        {
            get { return _cloudBackupColor; }
            set
            {
                if (Equals(_cloudBackupColor, value)) return;
                _cloudBackupColor = value;
                RaisePropertyChanged("CloudBackupColor");
            }
        }

        private DateTime? _membershipExpiresDate;
        public DateTime? MembershipExpiresDate
        {
            get { return _membershipExpiresDate; }
            set
            {
                if (_membershipExpiresDate != value)
                {
                    _membershipExpiresDate = value;
                    RaisePropertyChanged("MembershipExpiresDate");
                }
            }
        }

        private string _membershipType;

        public string MembershipType
        {
            get { return _membershipType; }
            set
            {
                if (_membershipType != value)
                {
                    _membershipType = value;
                    RaisePropertyChanged("MembershipType");
                }
            }
        }

        private string _newPinErrorMessage = string.Empty;
        public string NewPinErrorMessage
        {
            get { return _newPinErrorMessage; }
            set
            {
                if (_newPinErrorMessage != value)
                {
                    _newPinErrorMessage = value;
                    RaisePropertyChanged("NewPinErrorMessage");
                }
            }
        }

        private string _existingPinErrorMessage = string.Empty;
        public string ExistingPinErrorMessage
        {
            get { return _existingPinErrorMessage; }
            set
            {
                if (_existingPinErrorMessage != value)
                {
                    _existingPinErrorMessage = value;
                    RaisePropertyChanged("ExistingPinErrorMessage");
                }
            }
        }

        private string _confirmPinErrorMessage = string.Empty;
        public string ConfirmPinErrorMessage
        {
            get { return _confirmPinErrorMessage; }
            set
            {
                if (_confirmPinErrorMessage != value)
                {
                    _confirmPinErrorMessage = value;
                    RaisePropertyChanged("ConfirmPinErrorMessage");
                }
            }
        }


        public bool TouchScreenDetected
        {
            get { return DeviceHelper.HasTouchCapabilities(); }
        }

        private bool _enablePinAccessIsChecked = false;

        public bool EnablePinAccessIsChecked
        {
            get { return _enablePinAccessIsChecked; }
            set
            {
                //_enablePinAccessIsChecked = value;
                //RaisePropertyChanged("EnablePinAccessIsChecked");

                //var touchExists = Tablet.TabletDevices.OfType<TabletDevice>().Any(dev => dev.Type == TabletDeviceType.Touch);

                //if(touchExists)
                //{
                //    MessageBox.Show("Touch screen detected!");
                //}
                //else
                //{
                //    MessageBox.Show("Touch screen is not detected!");
                //}

                if (!DeviceHelper.HasTouchCapabilities() && value)
                {
                    MessageBox.Show("Touch screen is not detected!");
                    return;
                }



                if (_enablePinAccessIsChecked != value)
                {
                    _enablePinAccessIsChecked = value;


                    if (securityConfigEnablePinAccess == null)
                    {
                        securityConfigEnablePinAccess = new Configuration()
                        {
                            AccountEmail = pbData.ActiveUser,
                            Key = DefaultProperties.Configuration_Key_EnablePinAccess,
                            Active = true

                        };
                    }
                    securityConfigEnablePinAccess.Value = value.ToString();
                    pbData.AddOrUpdateConfiguration(securityConfigEnablePinAccess);

                    RaisePropertyChanged("EnablePinAccessIsChecked");
                }
            }
        }


        private bool _setautolockvisibility = false;
        public bool SetAutoLockVisibility
        {
            get { return _setautolockvisibility; }
            set
            {
                _setautolockvisibility = value;
                RaisePropertyChanged("SetAutoLockVisibility");
            }
        }

        private int _autoLockMinutes;

        public int AutoLockMinutes
        {
            get
            {
                return _autoLockMinutes;
            }
            set
            {
                if (value != _autoLockMinutes)
                {
                    pbData.PasswordTimeout = value <= 0 ? TimeSpan.FromMilliseconds(-1) : TimeSpan.FromMinutes(value);
                    pbData.ChangePrivateSetting(DefaultProperties.Settings_Security_PasswordAutoLock, value.ToString());
                }

                _autoLockMinutes = value;

                RaisePropertyChanged("AutoLockMinutes");
            }
        }

        private bool _changePINvisibility;
        public bool ChangePINVisibility
        {
            get { return _changePINvisibility; }
            set
            {
                _changePINvisibility = value;
                RaisePropertyChanged("ChangePINVisibility");
            }
        }

        private bool _changePasswordvisibility;
        public bool ChangePasswordVisibility
        {
            get { return _changePasswordvisibility; }
            set
            {
                _changePasswordvisibility = value;
                RaisePropertyChanged("ChangePasswordVisibility");
            }
        }

        ObservableCollection<AccountFolder> _accountFoldersItemSource;
        public ObservableCollection<AccountFolder> AccountFoldersItemSource
        {
            get { return _accountFoldersItemSource; }
            set
            {
                _accountFoldersItemSource = value;
                RaisePropertyChanged("AccountFoldersItemSource");
            }
        }

        private bool _acsettingpasswordvaultvisibility = true;
        public bool AccountSettingPasswordVaultVisibility
        {
            get { return _acsettingpasswordvaultvisibility; }
            set
            {
                _acsettingpasswordvaultvisibility = value;
                RaisePropertyChanged("AccountSettingPasswordVaultVisibility");
            }
        }

        List<syncdevices> _syncItemSource;
        public List<syncdevices> SyncItemSource
        {
            get { return _syncItemSource; }
            set
            {
                _syncItemSource = value;
                NumberOfSyncedDevices = SyncItemSource.Count();
                RaisePropertyChanged("SyncItemSource");
            }
        }

        private int _numberOfSyncedDevices;
        public int NumberOfSyncedDevices
        {
            get { return _numberOfSyncedDevices; }
            set
            {
                _numberOfSyncedDevices = value;
                RaisePropertyChanged("NumberOfSyncedDevices");
            }
        }

        public List<disabledSite> _disabledItemSource;
        public List<disabledSite> DisabledItemSource
        {
            get { return _disabledItemSource; }
            set
            {
                _disabledItemSource = value;
                RaisePropertyChanged("DisabledItemSource");
            }
        }


        ObservableCollection<StorageRegionModel> _storageRegions;

        public ObservableCollection<StorageRegionModel> StorageRegions
        {
            get { return _storageRegions; }
            set
            {
                _storageRegions = value;
                RaisePropertyChanged("StorageRegions");
            }
        }

        //private bool _messageBoxStorageRegionChangeVisibility;

        //public bool MessageBoxStorageRegionChangeVisibility
        //{
        //    get { return _messageBoxStorageRegionChangeVisibility; }
        //    set
        //    {
        //        _messageBoxStorageRegionChangeVisibility = value;
        //        RaisePropertyChanged("MessageBoxStorageRegionChangeVisibility");
        //    }
        //}


        private bool _messageBoxEnableStorageBackupOnCloudVisibility;

        public bool MessageBoxEnableStorageBackupOnCloudVisibility
        {
            get { return _messageBoxEnableStorageBackupOnCloudVisibility; }
            set
            {
                _messageBoxEnableStorageBackupOnCloudVisibility = value;
                RaisePropertyChanged("MessageBoxEnableStorageBackupOnCloudVisibility");
            }
        }

        private bool _messageBoxAccountSettingsChangedVisibility;

        public bool MessageBoxAccountSettingsChangedVisibility
        {
            get { return _messageBoxAccountSettingsChangedVisibility; }
            set
            {
                _messageBoxAccountSettingsChangedVisibility = value;
                RaisePropertyChanged("MessageBoxAccountSettingsChangedVisibility");
            }
        }

        private bool _messageBoxMasterPasswordChangedVisibility;

        public bool MessageBoxMasterPasswordChangedVisibility
        {
            get { return _messageBoxMasterPasswordChangedVisibility; }
            set
            {
                _messageBoxMasterPasswordChangedVisibility = value;
                RaisePropertyChanged("MessageBoxMasterPasswordChangedVisibility");
            }
        }

        private DateTime? _lastSync;

        public DateTime? LastSync
        {
            get { return _lastSync; }
            set
            {
                _lastSync = value;
                RaisePropertyChanged("LastSync");
            }
        }

        public string _existingPin;

        public string ExistingPin
        {
            get { return _existingPin; }
            set
            {
                _existingPin = value;
                RaisePropertyChanged("ExistingPin");
            }
        }

        public string _newPin;
        public string NewPin
        {
            get { return _newPin; }
            set
            {
                _newPin = value;
                RaisePropertyChanged("NewPin");
            }
        }

        public string _confirmPinPin;
        public string ConfirmPin
        {
            get { return _confirmPinPin; }
            set
            {
                _confirmPinPin = value;
                RaisePropertyChanged("ConfirmPin");
            }
        }

        public string CurrentStorageRegionUUID { get; set; }


        private void Acc_SettingPasswordVaultClick(object obj)
        {

            AccountFoldersList();
        }

        /// <summary>
        ///  view account setting digital wallet grid
        /// </summary>
        /// <param name="obj"></param>
        private void Acc_SettingDigitalWalletClick(object obj)
        {

            DigitalCategoryList();
        }

        /// <summary>
        ///  view account setting personal id's & notes grid
        /// </summary>
        /// <param name="obj"></param>
        private void Acc_SettingPersonalInfoClick(object obj)
        {

            // PersonalCategoryList();
        }

        private void AccountFoldersList()
        {
            AccountFoldersItemSource = null;
            //AccountCategoryItemSource = GetUserData.PasswordVaultItemSource;
        }

        /// <summary>
        /// account setting passwordvault category section data
        /// </summary>
        private IEnumerable<AccountFolder> GetFoldersCollection()
        {
            var result = new List<AccountFolder>() { new AccountFolder() { Name = string.Empty } };
            foreach (var item in pbData.GetFoldersBySecureItemType())
            {
                result.Add(new AccountFolder()
                {
                    uuid = item.Id,
                    parentId = item.ParentId,
                    Name = item.Name,
                    ChildList = new List<AccountFolder>()

                });

            }

            return result;
        }


        private ObservableCollection<AccountFolder> GetFoldersHierarchyCollection()
        {
            var tempList = new ObservableCollection<AccountFolder>();
            var folders = pbData.GetFoldersBySecureItemType();
            if (folders == null)
                return null;
            foreach (var item in folders)
            {
                tempList.Add(new AccountFolder()
                {
                    uuid = item.Id,
                    parentId = item.ParentId,
                    Name = item.Name,
                    ChildList = new List<AccountFolder>()

                });

            }

            var itemsToRemove = new List<AccountFolder>();

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

        /// <summary>
        /// account setting passwordvault category section data
        /// </summary>
        private void DigitalCategoryList()
        {
            AccountFoldersItemSource = null;
            AccountFoldersItemSource = GetFoldersHierarchyCollection();
        }



        private void ApplyDefaultStyles()
        {
            /*
            PasswordVaultBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            PasswordVaultBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            PasswordVaultFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            PasswordVaultIcon = _dashboardHelper.ReturnIcon(PasswordVault);

            DigitalWalletBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            DigitalWalletBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            DigitalWalletFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            DigitalWalletIcon = _dashboardHelper.ReturnIcon(DigitalWallet);

            PersonalInfoBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            PersonalInfoBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            PersonalInfoFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            PersonalInfoIcon = _dashboardHelper.ReturnIcon(PersonalInfo);

            SecureBrowserBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            SecureBrowserBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            SecureBrowserFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            SecureBrowserIcon = _dashboardHelper.ReturnIcon(SecureBrowser);

            PasswordGeneratorBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            PasswordGeneratorBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            PasswordGeneratorFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            PasswordGeneratorIcon = _dashboardHelper.ReturnIcon(PasswordGenerator);

            ShareCenterBackColor = _dashboardHelper.ReturnBackgroundDefaultColor();
            ShareCenterBorderColor = _dashboardHelper.ReturnBorderDefaultColor();
            ShareCenterFrontColor = _dashboardHelper.ReturnForegroundDefaultColor();
            ShareCenterIcon = _dashboardHelper.ReturnIcon(ShareCenter);
             * */
            AccountSettingIcon = DefaultProperties.ReturnImage(AccountSettingImage);
        }

        private void ApplyDefaultProperties()
        {
            //SelectedSortIndex = Default;
            //ApplyVisibilityState();
            //MainpageHitTestTrueOpacity();
            //HelpIcon = _dashboardHelper.ReturnHelpImage(Off);
        }

        private void ApplyDigitalWalletItemVisibilityState()
        {
            /*
            AddNewItemVisibility = false;
            DigitalWalletAddNewItemVisibility = false;
            PersonalInfoAddNewItemVisibility = false;
            DigitalWalletAddControlVisibility = false;
            PersonalInfoAddControlVisibility = false;
             * */
        }

        private void ExpandCollapseMenu(bool setProperty)
        {
            /*
            LeftMenuExpander = setProperty;
            PasswordVaultTextVisibility = setProperty;
            DigitalWalletTextVisibility = setProperty;
            PersonalInfoTextVisibility = setProperty;
            SecureBrowserTextVisibility = setProperty;
            PasswordGeneratorTextVisibility = setProperty;
            ShareCenterTextVisibility = setProperty;
            SetupGridVisibility = setProperty;
            MenuBelowGridVisibility = setProperty;
            MenuBorderVisibility = setProperty;
             * */
        }

        /// <summary>
        /// sync delete devices
        /// </summary>
        /// <param name="obj"></param>
        private void SyncDeleteClick(object obj)
        {
            CurrentInstallationId = obj as string;
            DeletePopupHeader = (string)System.Windows.Application.Current.FindResource("DeleteDevice");
            DeletePopupText = (string)System.Windows.Application.Current.FindResource("SyncDeleteMessage");
            DeletePopupVisibility = true;
            OpaqueGridVisibility = true;

            //_tabChanged = true;
            TabSyncChanged = true;
            //TabSelectedIndex = 3;
        }

        void ProgressInfo(int currentStep, int maxRemainingSteps)
        {
            //Treba dodati wait dijalog sa progresom
            ProgressBarMaxValue = maxRemainingSteps + currentStep;
            ProgressBarValue = currentStep;
            logger.Debug("Current step: {0}, max.remaining steps: {1}", currentStep, maxRemainingSteps);
        }

        //private bool masterPasswordDiffers = false;

        //void GetMergePassword(CredentialsRequiredEventArgs e)
        //{
        //    ManualResetEvent evDone = new ManualResetEvent(false);
        //    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        //    {
        //        try
        //        {
        //            MasterPwdBox masterDialog = new MasterPwdBox();
        //            bool? res = masterDialog.ShowDialog();
        //            if (res.HasValue && res.Value)
        //            {
        //                e.Password = masterDialog.Password;
        //                masterPasswordDiffers = true;
        //            }
        //            else
        //            {
        //                e.Cancel = true;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            logger.Error(exc.ToString());
        //        }
        //        evDone.Set();
        //    }));
        //    evDone.WaitOne();
        //}

        void ProgressInfo(int currentStep)
        {
            //Treba dodati wait dijalog sa progresom
            ProgressBarMaxValue = stepCount + currentStep;
            ProgressBarValue = currentStep;
            logger.Debug("Current step: {0}, max.remaining steps: {1}", currentStep, stepCount);
        }

        private int stepCount = 0;
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //e.Result = e.Argument;
                IPBSync sync = resolver.GetInstanceOf<IPBSync>();
                stepCount = sync.StepCount;
                //sync.OnGetMergePassword(GetMergePassword);
                if (!sync.Sync(ProgressInfo))
                {
                    var a = e.Argument as Action;
                    if (a != null) a();
                    logger.Error("Initial sync failed");
                }
                /*else
                {
                    if (masterPasswordDiffers)
                    {
                        //Treba reci da se pass razlikuje lokalno i na cloud-u i pitati da li zeli promijeniti lokalni pass na onaj sa cloud-a
                        //Ako kaze da onda treba uraditi pbData.ChangeMasterPassword(newPass)
                        masterPasswordDiffers = true;
                    }
                }*/
            }
            catch (Exception exc)
            {
                logger.Debug(exc.ToString());
            }
        }

        private readonly ManualResetEvent evtWrkDone = new ManualResetEvent(false);

        public void SyncDevicesData(bool silent = false, Action onFailed = null)
        {
            try
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(DefaultProperties.Features_SyncAndCloudStorage_OnlineBackup))
                {
                    return;
                }

                BackgroundWorker worker = new BackgroundWorker();

                worker.WorkerReportsProgress = true;
                //LoadingWindow ls = new LoadingWindow();

                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                //worker.ProgressChanged += worker_ProgressChanged;

                //worker.RunWorkerAsync(ls);
                evtWrkDone.Reset();
                worker.RunWorkerAsync(onFailed);
                SyncEnabled = false;
                //ls.ShowDialog();
                if (!silent)
                {
                    ProgressBarVisibility = true;
                    ProgressBarOpaqueGridVisibility = true;
                }

            }
            catch (Exception exc)
            {
                logger.Debug(exc.ToString());
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnSyncFinished();
            evtWrkDone.Set();
            //LoadingWindow ls = (LoadingWindow)e.Result;
            //ls.Close();
        }

        private void OnSyncFinished()
        {
            RefreshData();
            SyncEnabled = true;
            ProgressBarVisibility = false;
            ProgressBarOpaqueGridVisibility = false;
            ProgressBarValue = 0;


            if (!pbData.Locked)
            {
                var currentDevice = pbData.GetDevice(pbData.InstallationUUID);
                if (currentDevice != null)
                {
                    PopulateLastSyncDate();
                    //LastSync = currentDevice.LatestSync;
                }

                LoadTabGeneralDataNonInputFields();


            }
        }

        private void StorageRegionChanged(object obj)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_SyncAndCloudStorage_ChooseDataCenter))
            {
                return;
            }
            CurrentStorageRegionUUID = obj.ToString();
            TabPreferencesChanged = true;
            //MessageBoxStorageRegionChangeVisibility = true;
        }
        //private void ClearFromBrowsersClick(object obj)
        //{

        //    pbData.ClearChromeAccounts();
        //    pbData.ClearFFAccounts();
        //    pbData.ClearIEAccounts();
        //    //MessageBoxStorageRegionChangeVisibility = true;
        //}

        private void ClearSitesAutoSaveClick(object obj)
        {
            pbData.UpdateAutoSaveInfoForAllSites(false);
            //MessageBoxStorageRegionChangeVisibility = true;
        }



        //private void MessageBoxStorageRegionChangeConfirm(object obj)
        //{
        //    IPBWebAPI api = _resolver.GetInstanceOf<IPBWebAPI>();

        //    api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { storage_region = CurrentStorageRegionUUID, email = pbData.ActiveUser, installation = pbData.InstallationUUID });
        //    pbData.UpdateCurrentStorageRegion(CurrentStorageRegionUUID);
        //    InitializeDataStorageGrid();
        //    MessageBoxStorageRegionChangeVisibility = false;
        //    try
        //    {
        //        StorageRegion tmp = StorageRegions.FirstOrDefault(p => p.Checked);
        //        DataStorageMainWindow = tmp.Name;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //    }

        //}

        //private void MessageBoxStorageRegionChangeCancel(object obj)
        //{
        //    InitializeDataStorageGrid();
        //    MessageBoxStorageRegionChangeVisibility = false;
        //}

        //private void MessageBoxEnableStorageBackupOnCloudConfirm(object obj)
        //{
        //    MessageBoxEnableStorageBackupOnCloudVisibility = false;
        //    //LoadingWindow lw = new LoadingWindow();
        //    //lw.Show();
        //    //IPBWebAPI api = _resolver.GetInstanceOf<IPBWebAPI>();

        //    //api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { email = pbData.ActiveUser, installation = pbData.InstallationUUID, synchronize = EnableStorageCloudBackup });

        //    //pbData.ChangePrivateSetting(DefaultProperties.Settings_CloudStorage, EnableStorageCloudBackup.ToString());
        //    //lw.Close();
        //}

        //private void MessageBoxEnableStorageBackupOnCloudCancel(object obj)
        //{
        //    MessageBoxEnableStorageBackupOnCloudVisibility = false;
        //    EnableStorageCloudBackup = !EnableStorageCloudBackup;
        //}

        private void MessageBoxAccountSettingsChangedConfirm(object obj)
        {
            if (TabGeneralChanged)
            {
                SaveTabGeneralData(obj);
            }

            //if (TabAdvancedChanged)
            //{
            //    SaveTabAdvancedData(obj);
            //}

            if (TabPreferencesChanged)
            {
                SaveTabPreferences(obj);
            }

            MessageBoxAccountSettingsChangedVisibility = false;

            CloseSettingsOrMoveToNextTab();
        }

        private void MessageBoxAccountSettingsChangedCancel(object obj)
        {
            if (TabGeneralChanged)
            {
                LoadTabGeneraData();
            }

            if (TabAdvancedChanged)
            {
                LoadTabAdvancedData();
            }

            if (TabPreferencesChanged)
            {
                LoadTabPreferences();
            }

            MessageBoxAccountSettingsChangedVisibility = false;

            CloseSettingsOrMoveToNextTab();
        }

        private void CloseSettingsOrMoveToNextTab()
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                //TODO
                //if (!_tabChanged)
                //{
                //    MainWindow mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
                //    mainWindow.AccountSettingsClose();
                //}
                //else
                //{
                //    TabSelectedIndex = _tabNewIndex;
                //    _tabChanged = false;
                //}
            });
        }

        private void EnableStorageCloudBackupChanged(object obj)
        {
            MessageBoxEnableStorageBackupOnCloudVisibility = true;
        }

        private void DeleteFolderClick(object obj)
        {
            var uuid = obj as string;
            //todo Deleted
            if (pbData.DeleteFolder(uuid))
            {
                AccountFoldersItemSource = GetFoldersHierarchyCollection();
            }
        }






        internal Brush ReturnTextColor(string resource)
        {
            return (Brush)System.Windows.Application.Current.FindResource(resource);
        }

        private PasswordTextBox SetNewPasswordBox;
        private PasswordTextBox OldPasswordBox;
        private PasswordTextBox ConfirmPasswordBox;

        private void ApplyPasswordBoxDefaultStyle(object obj)
        {
            var pb = obj as PasswordTextBox;
            if (pb != null)
            {
                if (pb.Name == "SetNewPasswordBox")
                {
                    if (SetNewPasswordBox == null)
                    {
                        pb.GlobalPasswordTextBox.PasswordChanged += (sender, e) => PasswordChanged(pb.GlobalPasswordTextBox);
                    }
                    SetNewPasswordBox = pb;
                }

                if (pb.Name == "OldPasswordBox")
                {
                    if (OldPasswordBox == null)
                    {
                        pb.GlobalPasswordTextBox.PasswordChanged += (sender, e) => OldPasswordChanged(pb.GlobalPasswordTextBox.Password);
                        pb.GlobalPasswordTextBox.LostFocus += (sender, e) => OldPasswordLostFocus(pb.GlobalPasswordTextBox.Password);
                    }
                    OldPasswordBox = pb;
                }

                if (pb.Name == "ConfirmPasswordBox")
                {
                    if (ConfirmPasswordBox == null)
                    {
                        pb.GlobalPasswordTextBox.LostFocus += (sender, e) => ConfirmPasswordLostFocus(pb);
                    }
                    ConfirmPasswordBox = pb;
                }

                if (pb.IsEnabled)
                {
                    pb.GlobalPasswordTextBox.Background = Brushes.White;
                    pb.GlobalPasswordTextBox.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));
                }
                else
                {
                    pb.GlobalPasswordTextBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#eff3f3"));
                    pb.GlobalPasswordTextBox.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));// (SolidColorBrush)(new BrushConverter().ConvertFrom("#e4e9e9"));
                }

            }
        }

        private void LoginPortal(object obj)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppPortalLoginLink));
        }

        private void Upgrade(object obj)
        {
            LogUpgradeEvent();
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("GetPremium", null);
        }

        private void Renew(object obj)
        {
            LogUpgradeEvent();
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("GetPremium", null);
        }

        private void LogUpgradeEvent()
        {
            try
            {
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
                    BuyButton = BuyButtons.SettingsProfile.ToString()
                };

                pbData.InsertMessageHistory(his);
                var mhItem = pbData.GetMessageHistoryById(his.Id);

                var analytics2 = inAppAnalyitics.Get<Events.InAppMarketing, InAppMessageItem>();
                var logItem = new InAppMessageItem(mhItem.RowId, mhItem.AnalyticsCode, mhItem.MsgType, mhItem.Theme, (MarketingActionType)Enum.Parse(typeof(MarketingActionType), mhItem.ButtonClicked), BuyButtons.SettingsProfile, mhItem.DaysSinceAccountCreated);
                analytics2.Log(logItem);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private void ShowTwoStepVerificationDialog(object obj)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_TwoStepAuthentication))
            {
                TwoStepVerification = false;
                return;
            }

            var parent = Application.Current.MainWindow;
            Content = new TwoStepVerificationUserControl();
            Content.DataContext = this;
            Content.Owner = parent;
            Content.Top = parent.Top;
            Content.Left = parent.Left;
            Content.Height = parent.ActualHeight;
            Content.Width = parent.ActualWidth;
            Content.WindowStartupLocation = parent.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
            TwoStepVerificationModel.ConfirmationCode = string.Empty;
            TwoStepVerificationModel.IncorrectConfirmationCodeVisibility = false;
            TwoStepVerificationModel.IncorrectMasterPasswordVisibility = false;

            if (TwoStepVerification)
            {
                TwoStepVerification = false;
                TwoStepVerificationModel.TwoStepVerificationUserControlVisibility = true;
                Content.ShowDialog();
            }

            //TODO: Call action like upgrade above when url become available on API

        }

        private bool IsTabChanged()
        {
            return TabGeneralChanged || TabAdvancedChanged || TabPreferencesChanged || TabSyncChanged || TabDisabledSitesChanged;
        }

        private int GetChangedTabIndex()
        {
            if (TabGeneralChanged) return 0;
            if (TabAdvancedChanged) return 5;
            if (TabPreferencesChanged) return 1;
            if (TabSyncChanged) return 3;
            if (TabDisabledSitesChanged) return 6;
            return -1;
        }

        private void TabLostFocus(object obj)
        {
            var item = (TabItem)obj;
            if (!item.IsKeyboardFocusWithin && IsTabChanged())
            {
                if (_accountSettingsComboboxChanged)
                {
                    _accountSettingsComboboxChanged = false;
                    return;
                }

                System.Threading.ThreadPool.QueueUserWorkItem(
                   (a) =>
                   {
                       System.Threading.Thread.Sleep(100);
                       Application.Current.Dispatcher.BeginInvoke((Action)delegate
                       {
                           MessageBoxAccountSettingsChangedVisibility = true;
                       });
                   }
                   );
            }
        }

        private bool _tabChanged = false;
        private int _tabNewIndex = 0;

        private int _tabSelectedIndex = 0;

        public int TabSelectedIndex
        {
            get
            {
                return _tabSelectedIndex;
            }
            set
            {
                _tabSelectedIndex = value;
                RaisePropertyChanged("TabSelectedIndex");
            }
        }


        private bool _accountSettingsComboboxChanged = false;

        private void AccountSettingsComboboxChanged(object obj)
        {
            if (obj != null)
            {
                ComboBox comboboxSender = obj as ComboBox;
                if (comboboxSender.IsFocused || comboboxSender.IsKeyboardFocused)
                    _accountSettingsComboboxChanged = true;
                else
                    _accountSettingsComboboxChanged = false;
            }


        }

        private void TabControlLostFocus(object obj)
        {
            var control = (TabControl)obj;

            if (!control.IsKeyboardFocusWithin && !IsTabChanged())
            {
                CloseSettingsOrMoveToNextTab();
                //MainWindow mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
                //mainWindow.AccountSettingsClose();
            }

            if (IsTabChanged())
            {
                if (control.SelectedIndex != GetChangedTabIndex())
                {
                    _tabChanged = true;
                    _tabNewIndex = control.SelectedIndex;
                }
                control.SelectedIndex = GetChangedTabIndex();
            }
        }

        private bool CheckExistingPinNumber()
        {
            var device = pbData.GetDevice(pbData.InstallationUUID);

            if (string.IsNullOrWhiteSpace(ExistingPin))
            {
                PinChangeEnabled = false;
                return false;
            }

            if (device != null && device.PinNumber.HasValue && device.PinNumber != int.Parse(ExistingPin))
            {
                PinChangeEnabled = false;
                return false;
            }

            PinChangeEnabled = true;
            return true;
        }

        private bool CheckPinMatch()
        {
            return NewPin == ConfirmPin;
        }

        private bool CheckPinValidity()
        {
            return NewPin != null && NewPin.Length == 4;
        }

        private void ExistingPinLostFocus(object obj)
        {
            if (!CheckExistingPinNumber())
            {
                ExistingPinErrorMessage = (string)System.Windows.Application.Current.FindResource("WrongPin");
            }
        }

        private void ExistingPinChanged(object obj)
        {
            ExistingPinErrorMessage = string.Empty;
            CheckExistingPinNumber();
        }

        private void NewPinLostFocus(object obj)
        {
            if (!CheckPinValidity())
            {
                NewPinErrorMessage = (string)System.Windows.Application.Current.FindResource("InvalidPin");
            }
        }

        private void NewPinChanged(object obj)
        {
            NewPinErrorMessage = string.Empty;
            ConfirmPinErrorMessage = string.Empty;
        }

        private void ConfirmPinLostFocus(object obj)
        {
            if (!CheckPinMatch())
            {
                ConfirmPinErrorMessage = (string)System.Windows.Application.Current.FindResource("PinDoesNowMatch");
            }
        }

        private void ConfirmPinChanged(object obj)
        {
            ConfirmPinErrorMessage = string.Empty;
        }

        private void PinChange(object obj)
        {
            bool valid = true;

            if (!CheckExistingPinNumber())
            {
                valid = false;
                ExistingPinErrorMessage = (string)System.Windows.Application.Current.FindResource("WrongPin");
            }

            if (!CheckPinValidity())
            {
                valid = false;
                NewPinErrorMessage = (string)System.Windows.Application.Current.FindResource("InvalidPin");
            }

            if (!CheckPinMatch())
            {
                valid = false;
                ConfirmPinErrorMessage = (string)System.Windows.Application.Current.FindResource("PinDoesNowMatch");
            }

            if (valid)
            {
                try
                {
                    pbData.UpdateCurrentDevicePinNumber(int.Parse(NewPin));
                    ClearPinChangeForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                    logger.Error(ex.Message);
                }
            }
        }

        private void PinChangeCancel(object obj)
        {
            ClearPinChangeForm();
        }

        private void ApplyPasswordBoxFocusedStyle(object obj)
        {
            var pb = obj as PasswordTextBox;
            if (pb != null)
            {
                pb.GlobalPasswordTextBox.Background = Brushes.White;
                pb.GlobalPasswordTextBox.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#e4e9e9"));
            }
        }

        /// <summary>
        /// Finds an ancestor object by name and type
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current, string parentName) where T : DependencyObject
        {
            while (current != null)
            {
                if (!string.IsNullOrEmpty(parentName))
                {
                    var frameworkElement = current as FrameworkElement;
                    if (current is T && frameworkElement != null && frameworkElement.Name == parentName)
                    {
                        return (T)current;
                    }
                }
                else if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            };

            return null;
        }

        private void OldPasswordChanged(string password)
        {
            ExistingMasterPasswordErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(password) || !pbData.CheckMasterPassword(password))
            {
                SetNewPasswordBox.IsEnabled = false;
                ConfirmPasswordBox.IsEnabled = false;
            }
            else
            {
                SetNewPasswordBox.IsEnabled = true;
                ConfirmPasswordBox.IsEnabled = true;
            }

            ApplyPasswordBoxDefaultStyle(SetNewPasswordBox);
            ApplyPasswordBoxDefaultStyle(ConfirmPasswordBox);
        }

        private void OldPasswordLostFocus(string password)
        {
            if (!string.IsNullOrWhiteSpace(password) && !pbData.CheckMasterPassword(password))
            {
                ExistingMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("WrongPassword");
            }
        }

        private void ConfirmPasswordLostFocus(PasswordTextBox pBox)
        {
            ConfirmMasterPasswordErrorMessage = string.Empty;
            if (!CheckMasterPasswordsMatch(pBox))
            {
                ConfirmMasterPasswordErrorMessage = (string)System.Windows.Application.Current.FindResource("PasswordDoesNowMatch");
            }
        }


        /// <summary>
        /// password changed event for validation 
        /// </summary>
        /// <param name="element"></param>
        private void PasswordChanged(object element)
        {
            NewMasterPasswordErrorMessage = String.Empty;
            ConfirmMasterPasswordErrorMessage = string.Empty;
            //IsSubmitted = false;
            var passwordBox = element as PasswordBox;
            if (passwordBox != null)
                PasswordBox_PasswordChanged(passwordBox.Password);
        }

        /// <summary>
        /// for unchecking radio button, assign checked to radio button & assign color to radio button,
        /// and for checking the radio button  related to that validation 
        /// </summary>
        internal void PasswordBox_PasswordChanged(string masterPassword)
        {
            try
            {
                UpdateCharacterCheck(masterPassword);

                UpdateCapitalCheck(masterPassword);

                UpdateLettersCheck(masterPassword);

                UpdateSymbolsCheck(masterPassword);

                UpdateNumbersCheck(masterPassword);

                UpdateEntireCheck(masterPassword);

                UpdatePasswordMeter(masterPassword);

            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
        }

        public void AddNewFolderClick(object obj)
        {
            var folder = ServiceLocator.Get<IFolderService>().AddFolder();
            if (!string.IsNullOrEmpty(folder))
            {
                AccountFoldersItemSource = GetFoldersHierarchyCollection();

            };
        }
        private void EditFolderClick(object obj)
        {
            var uuid = obj as string;
            if (ServiceLocator.Get<IFolderService>().UpdateFolder(pbData.GetFolderById(uuid)))
            {
                AccountFoldersItemSource = GetFoldersHierarchyCollection();

            };          
        }

        /// <summary>
        /// shows delete device popup
        /// </summary>
        /// <param name="obj"></param>
        private void DeletePopupCancelClick(object obj)
        {
            TabSyncChanged = false;
            TabSelectedIndex = 2;
            TabSelectedIndex = 3;
            DeletePopupVisibility = false;
            OpaqueGridVisibility = false;
        }

        /// <summary>
        /// delete item as per tab selection in account setting
        /// </summary>
        /// <param name="obj"></param>
        private void DeletePopupButtonClick(object obj)
        {
            TabSyncChanged = false;
            TabSelectedIndex = 2;
            TabSelectedIndex = 3;

            if (!string.IsNullOrWhiteSpace(CurrentInstallationId))
            {
                DeleteSynDevice();
            }
            else
            {
                MessageBox.Show("Device delete error");
            }
            //if (SelectedIndexTabControl == 3)
            //    DeleteCategory();
            //else if (SelectedIndexTabControl == 2)
            //DeleteSynDevice();
        }


        /// <summary>
        /// delete sync device
        /// </summary>
        private void DeleteSynDevice()
        {
            var deviceToDelete = pbData.GetDevice(CurrentInstallationId);
            deviceToDelete.Active = false;
            pbData.UpdateDevice(deviceToDelete);
            InitializeGrid();
            DeletePopupVisibility = false;
            OpaqueGridVisibility = false;
            SyncDevicesData(true);
        }
    }

    public class AccountFolder
    {
        public string uuid { get; set; }
        public List<AccountFolder> ChildList { get; set; }
        public string parentId { get; set; }
        public string Name { get; set; }
        public bool browser { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }
    }
}