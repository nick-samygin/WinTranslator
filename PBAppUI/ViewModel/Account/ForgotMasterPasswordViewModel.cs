using PasswordBoss.Helpers;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace PasswordBoss.ViewModel.Account
{
    public class ForgotMasterPasswordViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(ForgotMasterPasswordViewModel));
        private const string ForgotPasswordLink = "https://support.passwordboss.com/customer/portal/articles/1839106-i-forgot-my-master-password-what-do-i-do-?b_id=6281%26utm_source=PC%26utm_medium=Login%26utm_campaign=FMP";
        private IResolver _resolver;
        private IPBWebAPI webAPI;
        private IPBData pbData;
        LoginViewModel _loginViewModel;

        public RelayCommand OpenSupportPageCommand { get; set; }
        public RelayCommand OpenGetCodeCommand { get; set; }
        public RelayCommand ShowInitialScreenCommand { get; set; }
        public RelayCommand ShowSecondScreenCommand { get; set; }
        public RelayCommand ShowInitialScreenSecondPartCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> ShowThirdScreenCommand { get; set; }


        private const string _getCodeLink = "https://portal.passwordboss.com/account/recover?utm_source=PC&utm_medium=Login&utm_campaign=RMP";

        private string _ErrorMessage = "";

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                RaisePropertyChanged("ErrorMessage");
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


        private bool _ForgotMasterPasswordInitialScreenVisibility = false;

        public bool ForgotMasterPasswordInitialScreenVisibility
        {
            get { return _ForgotMasterPasswordInitialScreenVisibility; }
            set
            {
                _ForgotMasterPasswordInitialScreenVisibility = value;
                RaisePropertyChanged("ForgotMasterPasswordInitialScreenVisibility");
            }
        }

        private bool _ForgotMasterPasswordSecondScreenVisibility = false;

        public bool ForgotMasterPasswordSecondScreenVisibility
        {
            get { return _ForgotMasterPasswordSecondScreenVisibility; }
            set
            {
                _ForgotMasterPasswordSecondScreenVisibility = value;
                RaisePropertyChanged("ForgotMasterPasswordSecondScreenVisibility");
            }
        }

        private bool _ForgotMasterPasswordThirdScreenVisibility = false;

        public bool ForgotMasterPasswordThirdScreenVisibility
        {
            get { return _ForgotMasterPasswordThirdScreenVisibility; }
            set
            {
                _ForgotMasterPasswordThirdScreenVisibility = value;
                RaisePropertyChanged("ForgotMasterPasswordThirdScreenVisibility");
            }
        }

        private bool _ForgotMasterPasswordInitialScreenSecondPartVisibility = false;

        public bool ForgotMasterPasswordInitialScreenSecondPartVisibility
        {
            get { return _ForgotMasterPasswordInitialScreenSecondPartVisibility; }
            set
            {
                _ForgotMasterPasswordInitialScreenSecondPartVisibility = value;
                RaisePropertyChanged("ForgotMasterPasswordInitialScreenSecondPartVisibility");
            }
        }

        private bool _ForgotMasterPasswordInitialScreenFirstPartVisibility = false;

        public bool ForgotMasterPasswordInitialScreenFirstPartVisibility
        {
            get { return _ForgotMasterPasswordInitialScreenFirstPartVisibility; }
            set
            {
                _ForgotMasterPasswordInitialScreenFirstPartVisibility = value;
                RaisePropertyChanged("ForgotMasterPasswordInitialScreenFirstPartVisibility");
            }
        }
        

        
        public ForgotMasterPasswordViewModel(IResolver resolver, LoginViewModel loginViewModel)
        {
            _resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            webAPI = resolver.GetInstanceOf<IPBWebAPI>();
            _loginViewModel = loginViewModel;

            OpenSupportPageCommand = new RelayCommand(OpenSupport);
            OpenGetCodeCommand = new RelayCommand(OpenGetCode);
            ShowInitialScreenCommand = new RelayCommand(ShowFirstScreenClick);
            ShowSecondScreenCommand = new RelayCommand(ShowSecondScreenClick);
            ShowInitialScreenSecondPartCommand = new RelayCommand(ShowInitialScreenSecondPartClick);
            ShowThirdScreenCommand = new AsyncRelayCommand<LoadingWindow>(ShowThirdScreenClick);
        }

        private void OpenSupport(object obj)
        {
            try
            {
                BrowserHelper.OpenInDefaultBrowser(new Uri(ForgotPasswordLink));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void OpenGetCode(object obj)
        {
            try
            {
                //string _uri = (string)System.Windows.Application.Current.FindResource("GetCodeLink");
                BrowserHelper.OpenInDefaultBrowser(new Uri(_getCodeLink));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void ShowFirstScreenClick(object obj)
        {
            HideAllScreens();
            ForgotMasterPasswordInitialScreenVisibility = true;
            ConfirmationCode = "";
            ForgotMasterPasswordInitialScreenSecondPartVisibility = false;
            ForgotMasterPasswordInitialScreenFirstPartVisibility = true;
        }

        private void ShowSecondScreenClick(object obj)
        {
            HideAllScreens();
            ForgotMasterPasswordSecondScreenVisibility = true;
        }

        private void ShowThirdScreenClick(object obj)
        {
            
            //TODO: call service here
            if (string.IsNullOrEmpty(ConfirmationCode))
            {
                ErrorMessage = Application.Current.FindResource("ForgotMasterPasswordInvalidCode").ToString();
                return;
            }
            dynamic deviceRegistrationResponse = webAPI.RegisterDevice(new WEBApiJSON.DeviceRegistrationRequest()
            {
                installation = pbData.InstallationUUID,
                nickname = System.Windows.Forms.SystemInformation.ComputerName,
                software_version = Assembly.GetAssembly(typeof(PasswordBoss.PBApp)).GetName().Version.ToString(),
                verification = ConfirmationCode
            }, _loginViewModel.UserEmail);
            if (deviceRegistrationResponse == null)
            {
                ErrorMessage = Application.Current.FindResource("ForgotMasterPasswordInvalidCode").ToString();
                return;
            }
            else
            {
                if (deviceRegistrationResponse.error != null)
                {
                    logger.Error(deviceRegistrationResponse.error.ToString());
                    ErrorMessage = Application.Current.FindResource("ForgotMasterPasswordInvalidCode").ToString();
                   
                    //System.Windows.Forms.MessageBox.Show(deviceRegistrationResponse.error.message.ToString());
                    return;
                }
            }

            var storePath = AppHelper.DBFolderLocation + _loginViewModel.UserEmail;
            
            try
            {
                pbData.CloseProfile();
                if (Directory.Exists(storePath))
                {
                    Directory.Delete(storePath, true);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                ErrorMessage = "Error while deleting DB";
            }
            
            
            
            HideAllScreens();
            ForgotMasterPasswordThirdScreenVisibility = true;
            
        }

        private void ShowInitialScreenSecondPartClick(object obj)
        {
            ForgotMasterPasswordInitialScreenFirstPartVisibility = !ForgotMasterPasswordInitialScreenFirstPartVisibility;
            ForgotMasterPasswordInitialScreenSecondPartVisibility = !ForgotMasterPasswordInitialScreenSecondPartVisibility;
        }

        private void HideAllScreens()
        {
            ForgotMasterPasswordInitialScreenVisibility = false;
            ForgotMasterPasswordSecondScreenVisibility = false;
            ForgotMasterPasswordThirdScreenVisibility = false;
           
            ErrorMessage = "";
        }
    }
}
