using PasswordBoss.Helpers;
using PasswordBoss.Views.UserControls;
using Svg2Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.ViewModel.AccountSettings
{
    public class TwoStepVerificationViewModel: ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(TwoStepVerificationViewModel));

        #region Properties

        public AccountSettingsViewModel AccountSettingsViewModel { get; set; }

        private bool _TwoStepVerificationUserControlVisibility = false;

        public bool TwoStepVerificationUserControlVisibility
        {
            get { return _TwoStepVerificationUserControlVisibility; }
            set
            {
                _TwoStepVerificationUserControlVisibility = value;
                if (TwoStepVerificationUserControlVisibility)
                {
                    InitTwoStepScreen();
                }
                RaisePropertyChanged("TwoStepVerificationUserControlVisibility");
            }
        }

        private bool _TwoStepInitialScreenVisibility = false;

        public bool TwoStepInitialScreenVisibility
        {
            get { return _TwoStepInitialScreenVisibility; }
            set
            {
                _TwoStepInitialScreenVisibility = value;
                RaisePropertyChanged("TwoStepInitialScreenVisibility");
            }
        }

        private bool _TwoStepMasterPasswordScreenVisibility = false;

        public bool TwoStepMasterPasswordScreenVisibility
        {
            get { return _TwoStepMasterPasswordScreenVisibility; }
            set
            {
                _TwoStepMasterPasswordScreenVisibility = value;
                IncorrectMasterPasswordVisibility = false;
                RaisePropertyChanged("TwoStepMasterPasswordScreenVisibility");
            }
        }

        private bool _TwoStepSecretKeyScreenVisibility = false;

        public bool TwoStepSecretKeyScreenVisibility
        {
            get { return _TwoStepSecretKeyScreenVisibility; }
            set
            {
                _TwoStepSecretKeyScreenVisibility = value;
                RaisePropertyChanged("TwoStepSecretKeyScreenVisibility");
            }
        }

        private bool _TwoStepConfirmationScreenVisibility = true;

        public bool TwoStepConfirmationScreenVisibility
        {
            get { return _TwoStepConfirmationScreenVisibility; }
            set
            {
                _TwoStepConfirmationScreenVisibility = value;
                RaisePropertyChanged("TwoStepConfirmationScreenVisibility");
            }
        }

        private bool _TwoStepMobilePhoneNumberScreenVisibility = false;

        public bool TwoStepMobilePhoneNumberScreenVisibility
        {
            get { return _TwoStepMobilePhoneNumberScreenVisibility; }
            set
            {
                _TwoStepMobilePhoneNumberScreenVisibility = value;
                RaisePropertyChanged("TwoStepMobilePhoneNumberScreenVisibility");
            }
        }

        private bool _TwoStepBackupSecurityCodeScreenVisibility = false;

        public bool TwoStepBackupSecurityCodeScreenVisibility
        {
            get { return _TwoStepBackupSecurityCodeScreenVisibility; }
            set
            {
                _TwoStepBackupSecurityCodeScreenVisibility = value;
                RaisePropertyChanged("TwoStepBackupSecurityCodeScreenVisibility");
            }
        }

        private bool _TwoStepCompletedScreenVisibility = false;

        public bool TwoStepCompletedScreenVisibility
        {
            get { return _TwoStepCompletedScreenVisibility; }
            set
            {
                _TwoStepCompletedScreenVisibility = value;
                RaisePropertyChanged("TwoStepCompletedScreenVisibility");
            }
        }

        private bool _IncorrectMasterPasswordVisibility = false;

        public bool IncorrectMasterPasswordVisibility
        {
            get { return _IncorrectMasterPasswordVisibility; }
            set
            {
                _IncorrectMasterPasswordVisibility = value;
                RaisePropertyChanged("IncorrectMasterPasswordVisibility");
            }
        }

        private bool _IncorrectConfirmationCodeVisibility = false;

        public bool IncorrectConfirmationCodeVisibility
        {
            get { return _IncorrectConfirmationCodeVisibility; }
            set
            {
                _IncorrectConfirmationCodeVisibility = value;
                RaisePropertyChanged("IncorrectConfirmationCodeVisibility");
            }
        }

        

        private string _SecretKey = "";

        public string SecretKey
        {
            get { return _SecretKey; }
            set
            {
                _SecretKey = value;
                RaisePropertyChanged("SecretKey");
            }
        }

        private DrawingImage _BarcodeSource = null;

        public DrawingImage BarcodeSource
        {
            get { return _BarcodeSource; }
            set
            {
                _BarcodeSource = value;
                RaisePropertyChanged("BarcodeSource");
            }
        }

        private DrawingImage _BarcodeSourceTMP = null;

        public DrawingImage BarcodeSourceTMP
        {
            get { return _BarcodeSourceTMP; }
            set
            {
                _BarcodeSourceTMP = value;
            }
        }
        

        private string _ConfirmationCode = "";

        public string ConfirmationCode
        {
            get { return _ConfirmationCode; }
            set
            {
                _ConfirmationCode = value;
                RaisePropertyChanged("ConfirmationCode");
            }
        }

        private string _MobilePhone = "";

        public string MobilePhone
        {
            get { return _MobilePhone; }
            set
            {
                _MobilePhone = value;
                RaisePropertyChanged("MobilePhone");
            }
        }

        private string _TwoStepBackupSecurityCode = "";

        public string TwoStepBackupSecurityCode
        {
            get { return _TwoStepBackupSecurityCode; }
            set
            {
                _TwoStepBackupSecurityCode = value;
                if (_TwoStepBackupSecurityCode != null && _TwoStepBackupSecurityCode.Length > 4)
				{
                    var parts = SplitByLength(_TwoStepBackupSecurityCode, 4).ToList();
                    StringBuilder partConcatenator = new StringBuilder();
                    foreach(var part in parts)
                    {
                        partConcatenator.AppendFormat("{0} ", part);
                    }
                    TwoStepBackupSecurityCodeFormatted = partConcatenator.ToString();
				}
                RaisePropertyChanged("TwoStepBackupSecurityCode");
            }
        }

        public IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            int index = 0;
            while (index + maxLength < str.Length)
            {
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }

            yield return str.Substring(index);
        }

        private string _TwoStepBackupSecurityCodeFormatted = "";

        public string TwoStepBackupSecurityCodeFormatted
        {
            get { return _TwoStepBackupSecurityCodeFormatted; }
            set
            {
                _TwoStepBackupSecurityCodeFormatted = value;
                RaisePropertyChanged("TwoStepBackupSecurityCodeFormatted");
            }
        }

        public RelayCommand CloseTwoStepVerificationDialogCommand { get; set; }
        public RelayCommand ShowTwoStepMasterPasswordScreenCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> ShowTwoStepSecreetKeyScreenCommand { get; set; }
        public RelayCommand ShowTwoStepConfirmationScreenCommand { get; set; }
        public RelayCommand ShowTwoStepMobilePhoneNumberScreenCommand { get; set; }
        public RelayCommand ShowTwoStepMobilePhoneNumberScreenBackCommand { get; set; }
        
        public RelayCommand ShowTwoStepBackupSecurityCodeScreenCommand { get; set; }
        public RelayCommand ShowTwoStepCompletedScreenCommand { get; set; }

        public RelayCommand TwoStepOpenLearnMoreLinkCommand { get; set; }
        public RelayCommand TwoStepOpenSupportedAppListCommand { get; set; }

        #endregion
        public TwoStepVerificationViewModel(AccountSettingsViewModel accountSettingsViewModel)
        {
            AccountSettingsViewModel = accountSettingsViewModel;
            CloseTwoStepVerificationDialogCommand = new RelayCommand(CloseTwoStepVerificationDialog);
            ShowTwoStepMasterPasswordScreenCommand = new RelayCommand(ShowTwoStepMasterPasswordScreen);
            //ShowTwoStepSecreetKeyScreenCommand = new RelayCommand(ShowTwoStepSecreetKeyScreen);
            ShowTwoStepSecreetKeyScreenCommand = new AsyncRelayCommand<LoadingWindow>(null
                                                                                    , executionFunction: (obj) => CheckMasterPassAndGetTwoStepSecreetKeyScreenInfo(obj)
                                                                                    , completed: (obj) => ShowTwoStepSecretKeyScreen(obj));
            //ShowTwoStepSecreetKeyScreenCommand = new AsyncRelayCommand<LoadingWindow>(ShowTwoStepSecreetKeyScreen);
            ShowTwoStepConfirmationScreenCommand = new RelayCommand(ShowTwoStepConfirmationScreen);
            ShowTwoStepMobilePhoneNumberScreenCommand = new RelayCommand(ShowTwoStepMobilePhoneNumberScreenScreen);
            ShowTwoStepMobilePhoneNumberScreenBackCommand = new RelayCommand(ShowTwoStepMobilePhoneNumberScreenBackScreen);
            ShowTwoStepBackupSecurityCodeScreenCommand = new RelayCommand(ShowTwoStepBackupSecurityCodeScreen);
            ShowTwoStepCompletedScreenCommand = new RelayCommand(ShowTwoStepCompletedScreen);
            TwoStepOpenLearnMoreLinkCommand = new RelayCommand(TwoStepOpenLearnMoreLink);
            TwoStepOpenSupportedAppListCommand = new RelayCommand(TwoStepOpenSupportedAppList);
        }

        #region Methods
        public void CloseTwoStepVerificationDialog(object obj)
        {
            if(!TwoStepCompletedScreenVisibility)
            {
                AccountSettingsViewModel.TwoStepVerification = false;
            }
            else
            {
                AccountSettingsViewModel.TwoStepVerification = true;
            }

            if(AccountSettingsViewModel.TwoStepVerification)
            {
                var pbData = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBData>();
                IPBWebAPI api = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBWebAPI>();
                var resp = api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { multi_factor_authentication = AccountSettingsViewModel.TwoStepVerification });

                if (resp == null)
                {
                    throw new Exception("API call failed");
                }

                pbData.UpdateMultiFactorAuthentication(AccountSettingsViewModel.TwoStepVerification);

                var inAppAnalyitics = this.AccountSettingsViewModel.resolver.GetInstanceOf<IInAppAnalytics>();
                if (inAppAnalyitics != null)
                {
                    inAppAnalyitics.Get<Events.TwoFactorVerificationEvent, AOItemType>().Log(AOItemType.Enabled);
                }
            }
            
            HideAllStepScreens();
            TwoStepVerificationUserControlVisibility = false;
            AccountSettingsViewModel.Content.Close();
        }

        public void ShowTwoStepMasterPasswordScreen(object obj)
        {
            HideAllStepScreens(); 
            TwoStepMasterPasswordScreenVisibility = true;
        }

        private static object SecretKeyLoadedLock = false;
        private static bool SecretKeyLoaded = false;
        public dynamic LoadSecretKey()
        {
            dynamic twoStepResp = null;
            lock (SecretKeyLoadedLock)
            {
                //if(!SecretKeyLoaded)
                //{
                    var pbData = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBData>();
                    IPBWebAPI api = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBWebAPI>();
                    twoStepResp = api.GetAccount2StepInfo(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { storage_region = this.AccountSettingsViewModel.CurrentStorageRegionUUID, email = pbData.ActiveUser, installation = pbData.InstallationUUID, synchronize = this.AccountSettingsViewModel.EnableStorageCloudBackup });
                //}
            }

            return twoStepResp;
        }

        public bool ConfirmCode()
        {
            bool isConfirmed = false;
            var pbData = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBData>();
            IPBWebAPI api = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBWebAPI>();
            var twoStepResp = api.ConfirmAccount2StepSecretKeyCode(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.SecretKeyConfirmationRequest { code=ConfirmationCode });
            string twoStepAuthResp = twoStepResp.auth;
            bool.TryParse(twoStepAuthResp, out isConfirmed);
            
            return isConfirmed;
        }

        public void ShowTwoStepSecretKeyScreen(object obj)
        {
            dynamic data = obj; //result from async method

            if (data.IsMasterPassValid)
            {
                HideAllStepScreens();
                TwoStepSecretKeyScreenVisibility = true;
                IncorrectMasterPasswordVisibility = false;
            }
            else
            {
                if (data.ForceShowingControl)
                {
                    HideAllStepScreens();
                    TwoStepSecretKeyScreenVisibility = true;
                    IncorrectMasterPasswordVisibility = false;
                }
                else
                {
                    IncorrectMasterPasswordVisibility = true;
                }
                
            }

            SecretKey = data.TwoStepResp.auth.multi_factor_code;
            this.TwoStepBackupSecurityCode = data.TwoStepResp.auth.multi_factor_one_time_code;
            string twoStepQRResp = data.TwoStepResp.auth.qr;

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(twoStepQRResp)))
            {
                var source = SvgReader.Load(xmlReader);
                source.Freeze();
                BarcodeSource = source;

            }
            SecretKeyLoaded = true;

        }
        public dynamic CheckMasterPassAndGetTwoStepSecreetKeyScreenInfo(object obj)
        {
            //Debugger.Launch();
            var pbData = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBData>();
            dynamic actionResult = new ExpandoObject();
            
            dynamic twoStepResp = LoadSecretKey();
            bool isMasterPassValid = false;
            if(obj != null)
            {
                PasswordBox pBox = obj as PasswordBox;
                if(pBox != null)
                {
                    var password = pBox.Password;
                    if(password != null)
                    {
                        isMasterPassValid = pbData.CheckMasterPassword(password);
                    }
                }
                actionResult.ForceShowingControl = false;
            }
            else
            {
                actionResult.ForceShowingControl = true;
            }
           
            actionResult.TwoStepResp = twoStepResp;
            actionResult.IsMasterPassValid = isMasterPassValid;
            return actionResult;
        }

        public void ShowTwoStepConfirmationScreen(object obj)
        {
            HideAllStepScreens(); 
            TwoStepConfirmationScreenVisibility = true;
        }

        public void ShowTwoStepMobilePhoneNumberScreenScreen(object obj)
        {
           
            if(ConfirmCode())
            {
                IncorrectConfirmationCodeVisibility = false;
                HideAllStepScreens();
                TwoStepMobilePhoneNumberScreenVisibility = true;
                ConfirmationCode = "";
            }
            else
            {
                IncorrectConfirmationCodeVisibility = true;
            }
            
        }

        public void ShowTwoStepMobilePhoneNumberScreenBackScreen(object obj)
        {
            HideAllStepScreens();
            TwoStepMobilePhoneNumberScreenVisibility = true;
            IncorrectConfirmationCodeVisibility = false;
        }
        

        public void ShowTwoStepBackupSecurityCodeScreen(object obj)
        {
            HideAllStepScreens(); 
            TwoStepBackupSecurityCodeScreenVisibility = true;
            if(!string.IsNullOrWhiteSpace(MobilePhone))
            {
                IPBWebAPI api = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBWebAPI>();
                var pbData = this.AccountSettingsViewModel.resolver.GetInstanceOf<IPBData>();
                var resp = api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { backup_phone = MobilePhone });
            }
        }

        public void ShowTwoStepCompletedScreen(object obj)
        {
            HideAllStepScreens(); 
            TwoStepCompletedScreenVisibility = true;
        }

        public void TwoStepOpenLearnMoreLink(object obj)
        {
            Uri uri = new Uri(DefaultProperties.LinkLearnMore);
            BrowserHelper.OpenInDefaultBrowser(uri);
        }

        public void TwoStepOpenSupportedAppList(object obj)
        {
            Uri uri = new Uri(DefaultProperties.LinkAuthenticatorApps);
            BrowserHelper.OpenInDefaultBrowser(uri);
        }
        
        
        public void InitTwoStepScreen()
        {
            HideAllStepScreens();
            TwoStepInitialScreenVisibility = true;
        }

        private void HideAllStepScreens()
        {
            TwoStepInitialScreenVisibility = false;
            TwoStepMasterPasswordScreenVisibility = false;
            TwoStepSecretKeyScreenVisibility = false;
            TwoStepConfirmationScreenVisibility = false;
            TwoStepMobilePhoneNumberScreenVisibility = false;
            TwoStepBackupSecurityCodeScreenVisibility = false;
            TwoStepCompletedScreenVisibility = false;
            
        }

        #endregion
    }
}
