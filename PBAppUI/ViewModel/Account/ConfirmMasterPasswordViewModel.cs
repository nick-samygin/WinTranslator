using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views.Login;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel.Account
{
    public class ConfirmMasterPasswordViewModel : ViewModelPasswordBox
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(ConfirmMasterPasswordViewModel));
        /// <summary>
        /// defining commands for UI elements
        /// </summary>
        //public RelayCommand PasswordChangedCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> NextButtonCommand { get; set; }
        //public RelayCommand PasswordGotFocusCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand PasswordFieldGotFocusCommand { get; set; }
        public RelayCommand PasswordFieldLostFocusCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        Common _commonObj = new Common();
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IPBWebAPI webApi = null;
        private IResolver resolver = null;
        IInAppAnalytics inAppAnalyitics;
        private string masterPassword = null;
        private string email = null;
        public ConfirmMasterPasswordViewModel(string email, string password, IResolver resolver)
        {
            this.email = email;
            this.masterPassword = password;
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.webApi = resolver.GetInstanceOf<IPBWebAPI>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            //PasswordChangedCommand = new RelayCommand(PasswordChanged);
            NextButtonCommand = new AsyncRelayCommand<LoadingWindow>(NextButtonClick);
            CloseCommand = new RelayCommand(CloseWindow);
            PasswordFieldGotFocusCommand = new RelayCommand(UserPasswordGotFocus);
            PasswordFieldLostFocusCommand = new RelayCommand(UserPasswordLostFocus);
            GoBackCommand = new RelayCommand(GoBackClick);
            PlaceHolderText = (string)System.Windows.Application.Current.FindResource("ReEnterMassterPass");
            //PasswordGotFocusCommand = new RelayCommand(PasswordGotFocus);

            //ApplyDefaultStyle(passwordBox, DefaultProperties.ConfirmMasterPasswordStyle, DefaultProperties.ToggleEyeBigIconStyle);
        }

        #region Properties

        private string _placeHolderText;
        public string PlaceHolderText
        {
            get { return _placeHolderText; }
            set
            {
                if (_placeHolderText != value)
                {
                    _placeHolderText = value;
                    if (_userPassword == string.Empty && _placeHolderText != string.Empty)
                    {
                        _placeHolderText = (string)System.Windows.Application.Current.FindResource("ReEnterMassterPass");
                    }
                    RaisePropertyChanged("PlaceHolderText");
                }
            }
        }

        private string _userPassword = string.Empty;
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                /*if (_userPassword != value)
                {*/
                _userPassword = value;
                RaisePropertyChanged("UserPassword");
                if (_userPassword != string.Empty)
                {
                    EyeImageVisibility = true;
                    PlaceHolderText = UserPassword;
                }
                if (_userPassword == string.Empty)
                {
                    EyeImageVisibility = false;
                    PlaceHolderText = (string)System.Windows.Application.Current.FindResource("ReEnterMassterPass");
                }
                if (this.masterPassword != null && UserPassword != null && this.masterPassword == UserPassword)
                {
                    IsNextButtonEnabled = true;
                }
                else
                {
                    IsNextButtonEnabled = false;
                }
                //}
            }
        }

        private bool _isNextButtonEnabled = false;
        public bool IsNextButtonEnabled
        {
            get { return _isNextButtonEnabled; }
            set
            {
                _isNextButtonEnabled = value;
                RaisePropertyChanged("IsNextButtonEnabled");
            }
        }

        private bool _eyeImageVisibility;
        public bool EyeImageVisibility
        {
            get { return _eyeImageVisibility; }
            set
            {
                _eyeImageVisibility = value;
                RaisePropertyChanged("EyeImageVisibility");
            }
        }

        #endregion

        #region Methods

        private void GoBackClick(object obj)
        {
            try
            {
                UserPassword = string.Empty;
                var nextScreen = new CreateAccount(resolver, email);
                Navigator.NavigationService.Navigate(nextScreen);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

        }

        private void UserPasswordGotFocus(object obj)
        {
            PlaceHolderText = string.Empty;
            if (obj != null)
            {
                PasswordBox passwordBoxControl = obj as PasswordBox;
                passwordBoxControl.Focus();
            }

        }

        private void UserPasswordLostFocus(object obj)
        {
            if (_userPassword == string.Empty)
            {
                PlaceHolderText = (string)System.Windows.Application.Current.FindResource("ReEnterMassterPass");
            }
        }

        /// <summary>
        /// For Closing login window
        /// </summary>
        private void CloseWindow(object sender)
        {
            inAppAnalyitics.Get<Events.AccountCreationFlow, AccountCreationFlowItem>().Log(new AccountCreationFlowItem(3, AccountCreationFlowSteps.ConfirmMP, string.Empty, MarketingActionType.Close));
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            _systemTray.WindowClose(window);
        }

        private void NextButtonClick(object element)
        {
            try
            {
                logger.Debug("Started account request and creating rsaKeys");
                if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(masterPassword))
                {
                    // To - Do Change style of Dialog
                    MessageBox.Show("Error retreiving account information");
                    return;
                }

                if (this.masterPassword != UserPassword)
                {
                    // To - Do Change style of Dialog
                    MessageBox.Show("Master password not matching");
                    return;
                }
                UserPassword = string.Empty;
                //Generate a public/private key pair
                /*RSA rsaBase = new RSA();
                rsaBase.GenerateKeys(1024, 65537, null, null);
                string privateKeyPem = rsaBase.PrivateKeyAsPEM;
                string publicKeyPem = rsaBase.PublicKeyAsPEM;

                logger.Debug("Call webAPI.RequestAccount");
                string publicKeyPem = rsaBase.PublicKeyAsPEM;*/
                byte[] publicKeyPem = null;
                ProtectedDataBlock privateKeyPem = null;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
                {
                    privateKeyPem = new ProtectedDataBlock(Encoding.UTF8.GetBytes(RSAKeyManagement.ExportPrivateKeyToPEM(rsa)));
                    publicKeyPem = Encoding.UTF8.GetBytes(RSAKeyManagement.ExportPublicKeyToPEM(rsa));
                }

                IPBWebAPI webAPI = resolver.GetInstanceOf<IPBWebAPI>();
                dynamic accountResponse = webAPI.RequestAccount(new WEBApiJSON.AccountRequest() { email = email, language = "English", installation = pbData.InstallationUUID, public_key = Convert.ToBase64String(publicKeyPem) });
                if (accountResponse == null)
                {
                    // To - Do Change style of Dialog
                    MessageBox.Show("Error in account registration");
                    return;
                }
                else
                {
                    if (accountResponse.error != null)
                    {
                        //MessageBox.Show(accountResponse.error.details[0].ToString());
                        if (accountResponse.error.code == "400")
                        {
                            //try to register device
                            dynamic deviceRegistrationResponse = webAPI.RegisterDevice(new WEBApiJSON.DeviceRegistrationRequest()
                            {
                                installation = pbData.InstallationUUID,
                                nickname = Environment.MachineName,
                                software_version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                            }, email);
                            if (deviceRegistrationResponse == null)
                            {
                                // To - Do Change style of Dialog
                                MessageBox.Show("Error in device registration");
                                return;
                            }
                            else
                            {

                                if (deviceRegistrationResponse.error != null)
                                {
                                    //MessageBox.Show(deviceRegistrationResponse.error.message.ToString());
                                    if (deviceRegistrationResponse.error.code.ToString() == "403")
                                    {
                                        //send verification code for new device
                                        dynamic verificationRequestResponse = webAPI.RequestVerificationCode(email);
                                        Application.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            var verificationScreen = new VerificationRequired(resolver, email, masterPassword);// resolver.GetInstanceOf<VerificationRequired>();

                                            Navigator.NavigationService.Navigate(verificationScreen);
                                        });
                                    }

                                    return;
                                }
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    Login login = resolver.GetInstanceOf<Login>();
                                    login.EmailTextBox.Text = email;
                                    Navigator.NavigationService.Navigate(login);
                                });
                            }

                        }
                    }
                }
                logger.Debug("Creating profile");
                if (!pbData.CreateProfile(email, masterPassword))
                {
                    // To - Do Change style of Dialog
                    MessageBox.Show("Error while creating secure database");
                }
                pbData.AddUserInfo(new DTO.UserInfo() { Email = email, RSAPrivateKey = privateKeyPem, PublicKey = publicKeyPem });
                logger.Debug("Performing initial sync");
                PerformInitialSync();
                SetDefaultSettings(pbData);

                inAppAnalyitics.Get<Events.AccountCreationFlow, AccountCreationFlowItem>().Log(new AccountCreationFlowItem(3, AccountCreationFlowSteps.ConfirmMP, string.Empty, MarketingActionType.Continue));

                // Added dispatcher because background worker can't create new UI elements
                Application.Current.Dispatcher.Invoke((Action)delegate
                {

                    var nextScreen = new SetupComplete(resolver);// resolver.GetInstanceOf<SetupComplete>();

                    Navigator.NavigationService.Navigate(nextScreen);
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public static void SetDefaultSettings(IPBData pbData)
        {
            string tmpCloudEnabled = pbData.GetPrivateSetting(DefaultProperties.Settings_CloudStorage);
            if (tmpCloudEnabled == null)
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_CloudStorage, bool.TrueString);
            }

            var rememberLoginEmail = pbData.GetConfigurationByAccountAndKey(pbData.ActiveUser, DefaultProperties.Configuration_Key_RememberEmail);
            if (rememberLoginEmail == null)
            {
                rememberLoginEmail = new Configuration()
                {
                    AccountEmail = pbData.ActiveUser,
                    Key = DefaultProperties.Configuration_Key_RememberEmail,
                    Value = true.ToString()
                };
                pbData.AddOrUpdateConfiguration(rememberLoginEmail);
            }
            bool isEnabledRememberLoginEmail = false;
            if (bool.TryParse(rememberLoginEmail.Value, out isEnabledRememberLoginEmail) && isEnabledRememberLoginEmail)
            {

                var lastLogin = new Configuration()
                {
                    AccountEmail = DefaultProperties.Configuration_DefaultAccount,
                    Key = DefaultProperties.Configuration_Key_LastLoginEmail,
                    Value = pbData.ActiveUser
                };
                pbData.AddOrUpdateConfiguration(lastLogin);
            }

            string autoLogin = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin);

            if (autoLogin == null)
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin, bool.TrueString);
            }

            string passwordSavingInBrowser = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving);

            if (passwordSavingInBrowser == null)
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving, bool.TrueString);
            }

            string autoStoreData = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData);

            if (autoStoreData == null)
            {
                pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData, bool.TrueString);
            }

            var userInfo = pbData.GetUserInfo(pbData.ActiveUser);

            if (userInfo != null && userInfo.StorageRegionUUID == null)
            {
                var region = pbData.GetStorageRegions().FirstOrDefault();

                if (region != null)
                {
                    pbData.UpdateCurrentStorageRegion(region.UUID);
                }
            }
        }

        void ProgressInfo(int currentStep, int totalNumberOfSteps)
        {
        }

        private bool masterPasswordDiffers = false;

        private void PerformInitialSync()
        {
            try
            {

                //TODO register device
                //if not default device goto verification

                dynamic deviceRegistrationResponse = webApi.RegisterDevice(
                    new WEBApiJSON.DeviceRegistrationRequest()
                    { /*application_language = "en-us", device_language = "en-us",*/
                        installation = pbData.InstallationUUID,
                        nickname = Environment.MachineName,
                        software_version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                    }, pbData.ActiveUser);
                if (deviceRegistrationResponse == null)
                {
                    MessageBox.Show("Error in device registration");
                    return;
                }
                else
                {
                    if (deviceRegistrationResponse.error != null)
                    {
                        MessageBox.Show(deviceRegistrationResponse.error.message.ToString());
                        if (deviceRegistrationResponse.error.code.ToString() == "403")
                        {
                            //todo send verification post
                            dynamic verificationRequestResponse = webApi.RequestVerificationCode(pbData.ActiveUser);
                            var verificationScreen = resolver.GetInstanceOf<VerificationRequired>();
                            Navigator.NavigationService.Navigate(verificationScreen);
                        }

                        return;
                    }
                }
                pbData.DeviceUUID = deviceRegistrationResponse.devices[0].uuid.ToString();
                Guid g;
                if (!Guid.TryParse(pbData.DeviceUUID, out g))
                {
                    MessageBox.Show("Invalid device ID");
                    return;
                }
                logger.Debug("Adding device");
                if (pbData.AddDevice(
                    new DTO.Device() { InstallationId = pbData.InstallationUUID, UUID = pbData.DeviceUUID, Nickname = System.Windows.Forms.SystemInformation.ComputerName }) == null)
                {
                    MessageBox.Show("Failed to save device data");
                    return;
                }

                evDone.Reset();
                IPBSync sync = resolver.GetInstanceOf<IPBSync>();
                //sync.OnGetMergePassword(GetMergePassword);
                sync.OnSyncFinished += sync_OnSyncFinished;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (!sync.Sync(3, ProgressInfo))
                        {
                            logger.Error("Initial sync failed");
                        }
                        else
                        {
                            if (masterPasswordDiffers)
                            {
                                //Treba reci da se pass razlikuje lokalno i na cloud-u i pitati da li zeli promijeniti lokalni pass na onaj sa cloud-a
                                //Ako kaze da onda treba uraditi pbData.ChangeMasterPassword(newPass)
                                masterPasswordDiffers = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                    finally
                    {
                        evSyncDone.Set();
                    }

                });

                evSyncDone.WaitOne();

                evDone.WaitOne();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private AutoResetEvent evSyncDone = new AutoResetEvent(false);

        private ManualResetEvent evDone = new ManualResetEvent(false);

        void sync_OnSyncFinished(bool status)
        {
            try
            {
                IPBSync sync = resolver.GetInstanceOf<IPBSync>();
                sync.OnSyncFinished -= sync_OnSyncFinished;
				var browserImporters = resolver.GetAllInstancesOf<BrowserImportStrategyBase>();

				var results = browserImporters.Select(s => s.Import());

				var importedSitesNum = results.Sum(r => r.Imported);
				var alredyImportedSitesNum = 0;

				List<SecureItem> userSites = pbData.GetSecureItemsByItemType(PasswordBoss.Helpers.DefaultProperties.SecurityItemType_PasswordVault).Where(x => x.Site.IsRecommendedSite == false).ToList();

                if (userSites.Count == 0)
                {
                    alredyImportedSitesNum = 0; //we didn't have anything previous. alredyImportedSitesNum can also mean overlapping between browsers
                }
                else
                {
					alredyImportedSitesNum = results.Sum(r => r.AlreadyImported);
                }

                var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
                if (inAppAnalyitics != null)
                {
                    ImportPasswordsItem item = new ImportPasswordsItem(importedSitesNum, ImportPasswordsTrigger.Installer, null);
                    var analytics = inAppAnalyitics.Get<Events.ImportPasswords, ImportPasswordsItem>();
                    analytics.Log(item);
                }

                pbData.ChangePrivateSetting("setup_wizard_imported_passwords_number", importedSitesNum.ToString());
                pbData.ChangePrivateSetting("setup_wizard_already_passwords_number", alredyImportedSitesNum.ToString());

                AddEmailToPI();

                logger.Debug("sync images");
                SyncImagesHelper syncImages = new SyncImagesHelper(pbData, webApi);
                //syncImages.SyncImagesAsync();
                syncImages.SyncImages();

                logger.Debug("Finished");
            }
            finally
            {
                evDone.Set();
            }
        }

        private void AddEmailToPI()
        {
            var SelectedCategory = new Folder();
            SelectedCategory.Id = DefaultCategories.CategoryEmail;
            SelectedCategory.Name = DefaultCategories.CategoryEmail;

            SecureItem secureItem = new SecureItem()
            {

                SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
                Name = pbData.ActiveUser,
                Type = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email,
                Data = new SecureItemData()
                {
                    email = pbData.ActiveUser,
                    notes = ""
                },
                Folder = SelectedCategory
            };

            bool isExisting = pbData.IsSecureItemExistingBySimpleRule(pbData.ActiveUser, SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email);

            if (!isExisting)
            {
                pbData.AddOrUpdateSecureItem(secureItem);
            }
        }

        private void PasswordChanged(object element)
        {
            //_commonObj.ElementTextChanged(element, DefaultProperties.PasswordBoxOnFocusStyle, DefaultProperties.ConfirmPasswordBoxOffFocusStyle);
        }

        #endregion
    }
}