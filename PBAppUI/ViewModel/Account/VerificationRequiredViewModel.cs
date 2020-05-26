using PasswordBoss.BusinessLayer;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using PasswordBoss.Views.Login;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel.Account
{

    public class VerificationRequiredViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(VerificationRequiredViewModel));

        private bool successVerification;

        public bool SuccessVerification
        {
            get { return successVerification; }
            set { successVerification = value; }
        }
        /// <summary>
        /// defining commands for UI elements
        /// </summary>
        public AsyncRelayCommand<LoadingWindow> SubmitButtonCommand { get; set; }
        public RelayCommand ElementTextChangedCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand TryAgainButtonCommand { get; set; }
        public RelayCommand ResendButtonCommand { get; set; }
        public RelayCommand CancelButtonCommand { get; set; }
        public RelayCommand MessageBoxInfoConfirmButtonCommand { get; set; }

        Common _commonObj = new Common();
        SystemTray _systemTray = new SystemTray();
        private IPBData pbData = null;
        private IResolver resolver = null;
        IPBWebAPI webAPI = null;
        private string email = null;
        private string mastePassword = null;
        public VerificationRequiredViewModel(IResolver resolver, string email, string password)
        {
            this.email = email;
            this.mastePassword = password;
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.webAPI = resolver.GetInstanceOf<IPBWebAPI>();
            SubmitButtonCommand = new AsyncRelayCommand<LoadingWindow>(SubmitButtonClick, completed: (obj) => SubmitCompleted(obj));
            ElementTextChangedCommand = new RelayCommand(ElementTextChanged);
            CloseCommand = new RelayCommand(CloseWindow);
            TryAgainButtonCommand = new RelayCommand(TryAgainCommand);
            ResendButtonCommand = new RelayCommand(ResendCommand);
            CancelButtonCommand = new RelayCommand(CancelCommand);
            MessageBoxInfoConfirmButtonCommand = new RelayCommand(MessageBoxInfoConfirmCommand);
            successVerification = false;
        }


        private bool _messageBoxInfoVisibility;
        public bool MessageBoxInfoVisibility
        {
            get { return _messageBoxInfoVisibility; }
            set
            {
                _messageBoxInfoVisibility = value;
                RaisePropertyChanged("MessageBoxInfoVisibility");
            }
        }

        private string _messageBoxInfoText;
        public string MessageBoxInfoText
        {
            get { return _messageBoxInfoText; }
            set
            {
                _messageBoxInfoText = value;
                RaisePropertyChanged("MessageBoxInfoText");
            }
        }

        /// Account Exists message popup visibility property
        private bool _verificationApiErrorVisibility;
        public bool VerificationApiErrorVisibility
        {
            get
            {
                return _verificationApiErrorVisibility;
            }
            set
            {
                _verificationApiErrorVisibility = value;
                RaisePropertyChanged("VerificationApiErrorVisibility");
            }
        }

        private string _verificationErrorMessage;
        public string VerificationErrorMessage
        {
            get
            {
                return _verificationErrorMessage;
            }
            set
            {
                _verificationErrorMessage = value;
                RaisePropertyChanged("VerificationErrorMessage");
            }
        }

        private string _verificationText;
        public string VerificationText
        {
            get
            {
                return _verificationText;
            }
            set
            {
                _verificationText = value;
                if (_verificationText != null)
                {
                    _verificationText = _verificationText.Trim();
                }
                RaisePropertyChanged("VerificationText");
            }
        }

        private void MessageBoxInfoConfirmCommand(object obj)
        {
            MessageBoxInfoVisibility = false;
        }

        private void CancelCommand(object obj)
        {
            var verificationCodeTextBox = obj as TextBox;
            verificationCodeTextBox.Text = String.Empty;
            VerificationApiErrorVisibility = false;
            Login login = resolver.GetInstanceOf<Login>();
            //login.EmailTextBox.Text = email;
            Navigator.NavigationService.Navigate(login);
        }

        private void TryAgainCommand(object obj)
        {
            var verificationCodeTextBox = obj as TextBox;
            VerificationApiErrorVisibility = false;
        }

        private void ResendCommand(object obj)
        {
            var verificationCodeTextBox = obj as TextBox;
            verificationCodeTextBox.Text = String.Empty;
            dynamic verificationRequestResponse = webAPI.RequestVerificationCode(email);
            VerificationApiErrorVisibility = false;
        }

        /// <summary>
        /// For Closing login window
        /// </summary>
        private void CloseWindow(object sender)
        {
            var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
            _systemTray.WindowClose(window);
        }
        /// <summary>
        /// hide and shows place holder text by changing style 
        /// </summary>
        /// <param name="element"></param>
        private void ElementTextChanged(object element)
        {
            _commonObj.ElementTextChanged(element, DefaultProperties.EmailOnFocusStyle, DefaultProperties.VerificationOffFocusStyle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SubmitButtonClick(object obj)
        {
            try
            {
                evtDone.Reset();
                successVerification = false;

                if (String.IsNullOrWhiteSpace(VerificationText))
                {
                    MessageBoxInfoVisibility = true;
                    MessageBoxInfoText = Application.Current.FindResource("CodeVerificationErrorNoCode").ToString();
                    return;
                }


                dynamic deviceRegistrationResponse = webAPI.RegisterDevice(new WEBApiJSON.DeviceRegistrationRequest()
                {
                    installation = pbData.InstallationUUID,
                    nickname = System.Windows.Forms.SystemInformation.ComputerName,
                    software_version = Assembly.GetAssembly(typeof(PasswordBoss.PBApp)).GetName().Version.ToString(),
                    verification = VerificationText
                }, email);
                if (deviceRegistrationResponse == null)
                {
                    MessageBoxInfoVisibility = true;
                    MessageBoxInfoText = Application.Current.FindResource("CodeVerificationErrorDeviceRegistration").ToString();
                    return;
                }
                else
                {
                    if (deviceRegistrationResponse.error != null)
                    {                  
                        VerificationErrorMessage = Application.Current.FindResource("IncorrectVerificationCodeDescription").ToString();
                        VerificationApiErrorVisibility = true;
                       
                        return;
                    }
                }
                pbData.DeviceUUID = deviceRegistrationResponse.devices[0].uuid.ToString();
                Guid g;
                if(!Guid.TryParse(pbData.DeviceUUID, out g))
                {
                    MessageBoxInfoVisibility = true;
                    MessageBoxInfoText = "Invalid device ID";
                    return;
                }
                if (!pbData.CreateProfile(email, mastePassword))
                {
                    MessageBoxInfoVisibility = true;
                    MessageBoxInfoText = Application.Current.FindResource("ErrorSecureDatabaseCreating").ToString();
                }
                if(pbData.AddDevice(
                    new DTO.Device() { InstallationId = pbData.InstallationUUID, UUID = pbData.DeviceUUID, Nickname = System.Windows.Forms.SystemInformation.ComputerName }) == null)
                {
                    MessageBoxInfoVisibility = true;
                    MessageBoxInfoText = "Failed to save device data";
                    return;
                }
                // Account created, can delete pre-registrate message history
                pbData.TryToClearMessageHistory();

                IPBSync sync = resolver.GetInstanceOf<IPBSync>();
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
                
                evtDone.WaitOne();
                sync.OnSyncFinished -= sync_OnSyncFinished;
                if(_finishVerification)
                {
                    ConfirmMasterPasswordViewModel.SetDefaultSettings(pbData);

                    successVerification = true;
                }
                
            }
            catch (Exception ex)
            {
                logger.Debug(ex.ToString());
            }
        }

        private AutoResetEvent evSyncDone = new AutoResetEvent(false);
        private ManualResetEvent evtDone = new ManualResetEvent(false);
        private bool _finishVerification = false;

        void sync_OnSyncFinished(bool status)
        {
            // RIO - Hot path
            if(!status)
            {
                try
                {
                    string _path = pbData.StorePath;

                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        SystemTray.Logout();
                    }));

                    int cnt = 3;
                    do
                    {
                        Thread.Sleep(250);
                        try
                        {
                            if (Directory.Exists(_path))
                            {
                                Directory.Delete(_path, true);
                            }
                            cnt = 0;
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.ToString());
                        }
                        cnt--;
                    } while (cnt > 0);

                    _finishVerification = false;
                }
                catch(Exception ex)
                {
                    logger.Error(ex.ToString());
                }
                finally
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LoginWindow loginWindow = ((PBApp)Application.Current).FindWindow<LoginWindow>();
                        loginWindow.NavigateloginScreens();
                    }));
                    evtDone.Set();
                }
                
            }
            else
            {
                try
                {
                    LoginViewModel.CheckRSA(new GenerateKeysStep().Execute(), pbData, webAPI);
                    SyncImagesHelper syncImages = new SyncImagesHelper(this.pbData, this.webAPI);
                    syncImages.SyncImages();
                    _finishVerification = true;
                }
                finally
                {
                    evtDone.Set();
                }
            }
            
        }

        void ProgressInfo(int currentStep, int totalNumberOfSteps)
        {
        }

        private bool masterPasswordDiffers = false;

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

        public void SubmitCompleted(object obj)
        {
            if(successVerification)
            {
                MainWindow mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
                if (mainWindow == null)
                    mainWindow = new MainWindow(resolver);
                else
                    mainWindow.Reload();

                SystemTray _systemTray = new SystemTray();
                var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
                _systemTray.WindowClose(window);
                mainWindow.Show();
            }
        }

        public void CheckTextBox(object obj)
        {
            var verificationCodeTextBox = obj as TextBox;
            if (verificationCodeTextBox.Text.Trim().Length == 0)
            {
                MessageBoxInfoVisibility = true;
                MessageBoxInfoText = Application.Current.FindResource("CodeVerificationErrorNoCode").ToString();
                return;
            }
        }

    }
}