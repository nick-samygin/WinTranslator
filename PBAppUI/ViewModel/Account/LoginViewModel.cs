using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using PasswordBoss.Views.Login;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PasswordBoss.PBAnalytics;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PasswordBoss.BusinessLayer;

namespace PasswordBoss.ViewModel.Account
{
	public class LoginViewModel : ViewModelPasswordBox
	{
		/// <summary>
		/// defining commands for UI elements
		/// </summary>
		public AsyncRelayCommand<LoadingWindow> SubmitCommand { get; set; }
		public RelayCommand SubmitCodeVerificationCommand { get; set; }

		public RelayCommand CloseCommand { get; set; }
		public RelayCommand ElementTextChangedCommand { get; set; }
		public RelayCommand CodeTextChangedCommand { get; set; }
		//public RelayCommand PasswordGotFocusCommand { get; set; }
		public RelayCommand CodeVerificationGotFocusCommand { get; set; }

		public RelayCommand ForgotLinkCommand { get; set; }
		public RelayCommand LostPhoneLinkCommand { get; set; }
		public RelayCommand CloseForgotMasterPasswordDialogCommand { get; set; }
		public RelayCommand CancelCommand { get; set; }
		//public RelayCommand PasswordChanged { get; set; }
		public RelayCommand PasswordFieldGotFocusCommand { get; set; }
		public RelayCommand PasswordFieldLostFocusCommand { get; set; }
		public bool IsThisComputerTrusted { get; set; }

		private readonly GenerateKeysStepResult generateKeysStep = new GenerateKeysStep().Execute();

		/// <summary>
		/// creating object of Login present in model folder
		/// </summary>
		SystemTray _systemTray = new SystemTray();
		Common _commonObj = new Common();

		private IPBData pbData = null;
		private IPBWebAPI webApi = null;
		private IResolver resolver = null;
		private bool IsTurningPassSavingExecuted = false;
		private static readonly ILogger logger = Logger.GetLogger(typeof(LoginViewModel));
		private Grid advertisingGrid = null;
		private string _masterPass = null;
		ForgotMasterPassword mp = null;
		public ForgotMasterPasswordViewModel ForgotMasterPasswordModel { get; set; }
		/// <summary>
		/// defining properties for UI elements
		/// </summary>
		#region properties

		//public bool IsTwoStepVerificationPassed { get; set; }

		private string _userEmail;
		public string UserEmail
		{
			get { return _userEmail; }
			set
			{
				if (_userEmail != value)
				{
					_userEmail = value;
					ErrorMessage = string.Empty;
					RaisePropertyChanged("UserEmail");
				}
			}
		}

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
						_placeHolderText = (string)System.Windows.Application.Current.FindResource("MasterPassword");
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
					PlaceHolderText = (string)System.Windows.Application.Current.FindResource("MasterPassword");
				}

				//}
			}
		}

		private string _errorMessage = string.Empty;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				if (_errorMessage != value)
				{
					_errorMessage = value;
					RaisePropertyChanged("ErrorMessage");
				}
			}
		}

		private string _CodeVerificationErrorMessageCode = string.Empty;
		public string CodeVerificationErrorMessageCode
		{
			get { return _CodeVerificationErrorMessageCode; }
			set
			{
				if (_CodeVerificationErrorMessageCode != value)
				{
					_CodeVerificationErrorMessageCode = value;
					RaisePropertyChanged("CodeVerificationErrorMessageCode");
				}
			}
		}


		private string _VerificationCode = string.Empty;
		public string VerificationCode
		{
			get { return _VerificationCode; }
			set
			{
				if (_VerificationCode != value)
				{
					_VerificationCode = value;
					RaisePropertyChanged("VerificationCode");
				}
			}
		}


		///Pin Failed Grid visibility property
		private bool _pinFailedGridVisibility;
		public bool PinFailedGridVisibility
		{
			get { return _pinFailedGridVisibility; }
			set
			{
				_pinFailedGridVisibility = value;
				RaisePropertyChanged("PinFailedGridVisibility");
			}
		}
		///2 step verification Grid visibility property
		private bool _codeVerificationGridVisibility;
		public bool CodeVerificationGridVisibility
		{
			get { return _codeVerificationGridVisibility; }
			set
			{
				_codeVerificationGridVisibility = value;
				RaisePropertyChanged("CodeVerificationGridVisibility");
			}
		}

		private bool _ForgotMasterPasswordControlVisibility;
		public bool ForgotMasterPasswordControlVisibility
		{
			get { return _ForgotMasterPasswordControlVisibility; }
			set
			{
				_ForgotMasterPasswordControlVisibility = value;
				RaisePropertyChanged("ForgotMasterPasswordControlVisibility");
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

		/// <summary>
		/// Initialize command with function
		/// </summary>
		public LoginViewModel(IResolver resolver, Grid advertisingGrid)
		{
			//generateKeysStep.ExecuteAsync();


			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.webApi = resolver.GetInstanceOf<IPBWebAPI>();

			SubmitCommand = new AsyncRelayCommand<LoadingWindow>(Submit);
			SubmitCodeVerificationCommand = new RelayCommand(SubmitCodeVerification);
			CloseCommand = new RelayCommand(CloseWindow);
			CancelCommand = new RelayCommand(CancelVerification);
			ElementTextChangedCommand = new RelayCommand(ElementTextChanged);
			CodeTextChangedCommand = new RelayCommand(CodeTextChanged);
			//PasswordGotFocusCommand = new RelayCommand(PasswordGotFocus);
			CodeVerificationGotFocusCommand = new RelayCommand(CodeVerificationGotFocus);
			ForgotLinkCommand = new RelayCommand(ForgotLinkClick);
			LostPhoneLinkCommand = new RelayCommand(LostPhoneLinkClick);
			CloseForgotMasterPasswordDialogCommand = new RelayCommand(CloseForgotMasterPasswordDialogClick);
			//PasswordChanged = new RelayCommand(PasswordChangedEvent);
			PasswordFieldGotFocusCommand = new RelayCommand(UserPasswordGotFocus);
			PasswordFieldLostFocusCommand = new RelayCommand(UserPasswordLostFocus);
			//ApplyDefaultStyle(passwordBox, DefaultProperties.MasterPasswordStyle, DefaultProperties.ToggleEyeBigIconStyle);
			this.pbData.OnUserLoggedIn += pbData_OnUserLoggedIn;
			this.advertisingGrid = advertisingGrid;
			PrefillUserEmail();
			ForgotMasterPasswordModel = new ForgotMasterPasswordViewModel(resolver, this);
			PlaceHolderText = (string)System.Windows.Application.Current.FindResource("MasterPassword");
			OpenMainUI = true;
		}


		private void UserPasswordGotFocus(object obj)
		{
			//ErrorMessage = String.Empty;
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
				PlaceHolderText = (string)System.Windows.Application.Current.FindResource("MasterPassword");
			}

		}

		//private void PasswordChangedEvent(object obj)
		//{
		//    if(obj != null)
		//    {
		//        var PasswordControl = obj as PasswordBox;
		//        UserPassword = PasswordControl.Password;
		//    }
		//}

		void pbData_OnUserLoggedIn(string obj)
		{
			var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
			if (inAppAnalyitics != null)
			{
				//inAppAnalyitics.Get<Events.LoggedIn, EmptyString>().Log(EmptyString.Empty);
				inAppAnalyitics.Get<Events.ActiveUser, Yes>().Log(Yes.Yes);
			}

		}

		private void PrefillUserEmail()
		{
			string lastEmail = pbData.GetConfigurationValueByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, DefaultProperties.Configuration_Key_LastLoginEmail);
			if (lastEmail != null) UserEmail = lastEmail;
		}


		private void CloseForgotMasterPasswordDialogClick(object obj)
		{
			mp.Close();
			mp = null;
			ForgotMasterPasswordControlVisibility = false;
			var window = _systemTray.CurrentWindow("LoginWindow");
			if (window != null)
			{
				window.Show();
			}
		}

		private void LostPhoneLinkClick(object obj)
		{
			BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.LinkLostMyPhone));
		}
		/// <summary>
		/// changing visibility for 2 step verification
		/// </summary>
		/// <param name="obj"></param>
		private void ForgotLinkClick(object obj)
		{
			if (_commonObj.IsEmailValid(UserEmail) && string.IsNullOrEmpty(pbData.ActiveUser))
			{
				ForgotMasterPasswordControlVisibility = true;
				if (Application.Current.MainWindow != null)
				{
					Application.Current.MainWindow.Hide();
				}
				mp = new ForgotMasterPassword();
				mp.DataContext = this;

				if (Application.Current.MainWindow != null)
				{
					mp.Owner = Application.Current.MainWindow;
				}
				ForgotMasterPasswordModel.ShowFirstScreenClick(null); //we always show first screen
				mp.ShowDialog();
			}
			else if (!string.IsNullOrEmpty(pbData.ActiveUser))
			{
				ErrorMessage = (string)System.Windows.Application.Current.FindResource("DatabaseLocked");
			}
			else
			{
				ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailFormatMessage");
			}
		}

		void mp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var window = Application.Current.MainWindow;
			if (window != null)
			{
				window.DragMove();
			}
		}

		private void CancelVerification(object obj)
		{
			advertisingGrid.Visibility = Visibility.Visible;
			CodeVerificationGridVisibility = false;
			VerificationCode = "";
			CodeVerificationErrorMessageCode = "";
		}

		/// <summary>
		/// Password user control box got focus event for assigning events
		/// </summary>
		/// <param name="elementObj"></param>

		private void CodeVerificationGotFocus(object elementObj)
		{
			if (CodeVerificationGridVisibility)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(
				   (a) =>
				   {
					   System.Threading.Thread.Sleep(100);
					   Application.Current.Dispatcher.BeginInvoke((Action)delegate
					   {
						   var txtBox = elementObj as Control;
						   Keyboard.Focus(txtBox);
						   CodeVerificationErrorMessageCode = "";
					   });
				   }
				   );
			}
		}

		/// <summary>
		/// hide and shows code place holder text by changing style 
		/// </summary>
		/// <param name="element"></param>
		private void CodeTextChanged(object element)
		{
			if (element is TextBox)
			{
				_commonObj.ElementTextChanged(element, DefaultProperties.EmailOnFocusStyle, DefaultProperties.CodeOffFocusStyle);
			}
		}

		/// <summary>
		/// hide and shows place holder text by changing style 
		/// </summary>
		/// <param name="element"></param>
		private void ElementTextChanged(object element)
		{
			if (element is TextBox)
			{
				_commonObj.ElementTextChanged(element, DefaultProperties.EmailOnFocusStyle, DefaultProperties.EmailOffFocusStyle);
			}
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
		/// subbmit button command function which passing parameter to validate user
		/// </summary>
		/// <param name="element">passowrdbox control for getting password</param>
		internal void Submit(object element)
		{
			// RIO - Hot path
			try
			{
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					ErrorMessage = String.Empty;
					//var passwordBox = element as PasswordTextBox;
					//if (passwordBox != null)
					//{
					//    pass = passwordBox.GlobalPasswordTextBox.Password;
					//    passwordBox.GlobalPasswordTextBox.Password = null;
					//}
				});
				//PlaceHolderText = string.Empty;
				SignIn(UserEmail, UserPassword);
				//UserPassword = string.Empty;

			}
			catch (Exception ex)
			{
				logger.Error("{0}", ex);
				throw;
			}


		}

		internal void SubmitCodeVerification(object element)
		{
			ErrorMessage = String.Empty;

			if (ConfirmCode())
			{
				bool profileExists = false;
				var isAuthenticated = pbData.AuthenticateUser(UserEmail, _masterPass, out profileExists);
				
				if (!isAuthenticated)
				{
					Application.Current.Dispatcher.Invoke((Action)delegate
					{
						ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailOrPasswordMessage");
					});
					return;
				}

				pbData.OpenProfile(UserEmail, _masterPass, out profileExists);
                _masterPass = string.Empty;
                VerificationCode = "";
				CodeVerificationErrorMessageCode = "";
				//installationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
				pbData.ChangePrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrustedLastChecked, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
				if (IsThisComputerTrusted)
				{
					pbData.ChangePrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrusted, IsThisComputerTrusted.ToString());
				}
				//IsTwoStepVerificationPassed = true;

				ShowMainWindow();

				CodeVerificationGridVisibility = false;
				advertisingGrid.Visibility = Visibility.Visible;
			}
			else
			{
				CodeVerificationErrorMessageCode = "Invalid confirmation code, please try again";
			}
		}

		public bool ConfirmCode()
		{
			bool isConfirmed = false;
			//if(pbData.ActiveUser == null)
			//{
			//    ErrorMessage = "Database is locked, please log in again";
			//    CodeVerificationGridVisibility = false;
			//    advertisingGrid.Visibility = Visibility.Visible;
			//    VerificationCode = "";
			//    return isConfirmed;
			//}

			var twoStepResp = webApi.ConfirmAccount2StepSecretKeyCode(UserEmail + "|" + pbData.DeviceUUID, new WEBApiJSON.SecretKeyConfirmationRequest { code = VerificationCode });
			string twoStepAuthResp = twoStepResp.auth;
			bool.TryParse(twoStepAuthResp, out isConfirmed);

			return isConfirmed;
		}

		public bool OpenMainUI { get; set; }

		internal void SignIn(string userEmailId, string masterPassword)
		{
			logger.Debug("SignIn for user {0} ", userEmailId);
			try
			{
				if (string.IsNullOrWhiteSpace(masterPassword))
				{
					logger.Error("MasterPassword is empty");
					Application.Current.Dispatcher.Invoke((Action)delegate
					{
						//MessageBox.Show("Username or Password is incorrect");
						ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailOrPasswordMessage");
						//UserEmail = String.Empty;
					});
					return;
				}
				var correctEmail = userEmailId != null && (_commonObj.IsEmailValid(userEmailId));
				if (!correctEmail)
				{
					logger.Error("Incorrect email - {0}", userEmailId);
					Application.Current.Dispatcher.Invoke((Action)delegate
					{
						//MessageBox.Show("Incorrect Email Format.");
						ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailFormatMessage");
						//UserEmail = String.Empty;
					});
					return;
				}

				bool profileExists = false;
				bool hasDeviceID = false;

				logger.Debug("Authenticate user start");
				var isAuthenticated = pbData.AuthenticateUser(userEmailId, masterPassword, out profileExists);
				logger.Debug("Authenticate user end. IsAuthenticated - {0}", isAuthenticated);
				if (isAuthenticated)
				{
					pbData.OpenProfile(UserEmail, masterPassword, out profileExists);
					_masterPass = masterPassword;
					UserPassword = string.Empty;

					if (profileExists)
					{
						logger.Debug("Proifle exists");
						hasDeviceID = pbData.GetDevice(pbData.InstallationUUID) != null;
						logger.Debug("HasDeviceId = {0}", hasDeviceID);
					}

					Application.Current.Dispatcher.Invoke((Action)delegate
					{
						pbData.ActivatePinIfEnabled(masterPassword, DefaultProperties.Configuration_Key_EnablePinAccess);
					});

					Task.Factory.StartNew(() => Application.Current.Dispatcher.Invoke((Action)delegate
					{
						Task.Factory.StartNew(() =>
						{
							logger.Debug("SyncImagesHelper called in async");
							new SyncImagesHelper(pbData, webApi).SyncImages();
						});
					}));
				}
				else
				{
					_masterPass = null;
					if (profileExists)
					{
						logger.Error("ProfileExists");
						Application.Current.Dispatcher.Invoke((Action)delegate
						{
							//MessageBox.Show("Username or Password is incorrect");
							ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailOrPasswordMessage");
							//UserEmail = String.Empty;
						});
						return;
					}
				}

				logger.Debug("Calling tryToGetDeviceID");
				var tryToGetDeviceId = TryToGetDeviceID(userEmailId, masterPassword, profileExists, hasDeviceID);
				logger.Debug("TryToGetDeviceId result - {0}", tryToGetDeviceId);

				if (!tryToGetDeviceId)
				{

					logger.Debug("Calling TryToShowMainWindow");
					var tryToShowMainWindowResult = TryToShowMainWindow(isAuthenticated);
					logger.Debug("TryToShowMainWindow result - {0}", tryToShowMainWindowResult);
					if (!tryToShowMainWindowResult)
					{
						logger.Error("Unable to show main window");
						Application.Current.Dispatcher.Invoke((Action)delegate
						{
							//MessageBox.Show("Username or Password is incorrect");
							ErrorMessage = (string)System.Windows.Application.Current.FindResource("IncorrectMailOrPasswordMessage");
							//UserEmail = String.Empty;
						});
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
				logger.Error(ex.Message);
			}
			logger.Debug("SignIn finished");
		}

		private bool TryToShowMainWindow(bool isAuthenticated)
		{
			logger.Debug("TryToShowMainWindow called; isAuthenticated - {0}", isAuthenticated);
			if (!isAuthenticated)
			{
				logger.Error("Cant show main window - not authenticated");
				return false;
			}

			logger.Debug("Getting device");
			Device device = pbData.GetDevice(pbData.InstallationUUID);
			if (device != null)
			{
				pbData.DeviceUUID = device.UUID;
				logger.Debug("found device - {0}", pbData.DeviceUUID);
			}
			else
			{
				logger.Error("Cant find device");
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					string _message = System.Windows.Application.Current.FindResource("ProblemReadingDeviceIdLoginMessage") as string;
					CustomInformationMessageBoxWindow c = new CustomInformationMessageBoxWindow(_message);
					c.ShowDialog();
				});

				return true;
			}

			IPBWebAPI pbWebAPI = resolver.GetInstanceOf<IPBWebAPI>();


			logger.Debug("Get countries");
			if (pbData.GetCountries().Count == 0)
			{
				logger.Debug("Get countries - request from web API");
				dynamic countries = pbWebAPI.RequestCountry(String.Format("{0}|{1}", pbData.ActiveUser, pbData.DeviceUUID));
				pbData.AddCounties(countries);
			}


			logger.Debug("GetConfigurationByAccountAndKey {0} < {1}", UserEmail, DefaultProperties.Configuration_Key_RememberEmail);
			var rememberLoginEmail = pbData.GetConfigurationByAccountAndKey(UserEmail, DefaultProperties.Configuration_Key_RememberEmail);
			if (rememberLoginEmail == null)
			{
				logger.Debug("GetConfigurationByAccountAndKey failed, adding configuration");
				rememberLoginEmail = new Configuration()
				{
					AccountEmail = UserEmail,
					Key = DefaultProperties.Configuration_Key_RememberEmail,
					Value = true.ToString()
				};
				pbData.AddOrUpdateConfiguration(rememberLoginEmail);
				logger.Debug("Condifuration added");
			}
			bool isEnabledRememberLoginEmail = false;
			logger.Debug("TRying to parse rememberLoginEmail");
			if (bool.TryParse(rememberLoginEmail.Value, out isEnabledRememberLoginEmail))
			{
				logger.Debug("rememberLoginEmail parsed - {0}", isEnabledRememberLoginEmail);
				if (isEnabledRememberLoginEmail)
				{
					var lastLogin = new Configuration()
					{
						AccountEmail = DefaultProperties.Configuration_DefaultAccount,
						Key = DefaultProperties.Configuration_Key_LastLoginEmail,
						Value = UserEmail
					};
					pbData.AddOrUpdateConfiguration(lastLogin);
					logger.Debug("Configuration updated");
				}
			}

			bool isDeviceTrusted = false;
			var isDeviceTrustedStr = pbData.GetPrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrusted);
			if (!string.IsNullOrWhiteSpace(isDeviceTrustedStr))
			{
				bool.TryParse(isDeviceTrustedStr, out isDeviceTrusted);
			}

			logger.Debug("IsDeviceTrusted - {0}", isDeviceTrusted);

			if (isDeviceTrusted)
			{
				DateTimeOffset lastCheckedDateTimeOffset = new DateTimeOffset();
				DateTime lastCheckedDate = new DateTime();
				var isDeviceTrustedLastCheckedStr = pbData.GetPrivateSetting(DefaultProperties.Settings_Device_IsDeviceTrustedLastChecked);
				bool isParsed = DateTimeOffset.TryParse(isDeviceTrustedLastCheckedStr, out lastCheckedDateTimeOffset);
				if (isParsed)
				{
					lastCheckedDate = lastCheckedDateTimeOffset.UtcDateTime;
					var totalDaysCount = (DateTime.UtcNow - lastCheckedDate).TotalDays;

					if (totalDaysCount > DefaultProperties.Settings_Device_IsDeviceTrustedLastCheckedMinDays)
					{
						logger.Debug("device not trusted - totalDaysCount > DefaultProperties.Settings_Device_IsDeviceTrustedLastCheckedMinDays");
						isDeviceTrusted = false; //notify we need to enter this info
					}
				}
				else
				{
					isDeviceTrusted = false;
				}
			}
			logger.Debug("IsTurningPassSavingExecuted - {0}", IsTurningPassSavingExecuted);

			if (!IsTurningPassSavingExecuted)
			{
				IsTurningPassSavingExecuted = true;
				bool turnOffPassSaving = false;
				Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving), out turnOffPassSaving);

				logger.Debug("TurnOffPassSaving - {0}", turnOffPassSaving);
				if (turnOffPassSaving)
				{
					pbData.DisableStoringPasswordInBrowsers();
				}
				else
				{
					pbData.EnableStoringPasswordInBrowsers();
				}

			}
			//Task.Factory.StartNew(() => CheckRSA(generateKeysStep, pbData, webApi));
			logger.Debug("CheckRsa Calling generateKeysStep - {0}", generateKeysStep);
			CheckRSA(generateKeysStep, pbData, webApi);

			//pbData.TryToClearMessageHistory();

			logger.Debug("OpenMainUI - {0}", OpenMainUI);
			if (OpenMainUI)
			{
				ShowMainWindowIfNeeded(isDeviceTrusted);
			}
			else
			{
				Application.Current.Dispatcher.BeginInvoke((Action)(() =>
				{
					CloseLoginWindow();
					MainWindow mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (mainWindow == null)
					{
						mainWindow = new MainWindow(resolver);
					}
				}));
			}

			return true;
		}

		private bool TryToGetDeviceID(string userEmailId, string masterPassword, bool profileExists, bool hasDeviceID)
		{
			logger.Debug("TryToGetDeviceId started. profileExists - {0}, hasDeviceId - {1}", profileExists, hasDeviceID);
			var canProceed = !profileExists || !hasDeviceID;
			if (!canProceed)
				return false; // refactoring complexity of conditions. 


			if (!webApi.HasInetConn)
			{
				logger.Error("TryToGetDeviceID - No internet connection - exit");
				return false;
			}

			IPBWebAPI webAPI = resolver.GetInstanceOf<IPBWebAPI>();


			// try to register device
			dynamic deviceRegistrationResponse = webAPI.RegisterDevice(
				new WEBApiJSON.DeviceRegistrationRequest()
				{
					installation = pbData.InstallationUUID,
					nickname = System.Windows.Forms.SystemInformation.ComputerName,
					software_version = Assembly.GetAssembly(typeof(PasswordBoss.PBApp)).GetName().Version.ToString()
				}, userEmailId);


			if (deviceRegistrationResponse == null)
			{
				logger.Error("deviceRegistrationResponse is empty");
				//MessageBox.Show("Error in device registration");
				Application.Current.Dispatcher.Invoke((Action)delegate
				{
					string _message = System.Windows.Application.Current.FindResource("ErrorInDeviceRegistrationLoginMessage") as string;
					CustomInformationMessageBoxWindow c = new CustomInformationMessageBoxWindow(_message);
					c.ShowDialog();
				});
				//return false;
				return true;
			}
			else
			{

				if (deviceRegistrationResponse.error != null)
				{
					logger.Debug("DeviceRegistrationResponse.error = {0}", deviceRegistrationResponse.error);

					//MessageBox.Show(deviceRegistrationResponse.error.message.ToString());
					if (deviceRegistrationResponse.error.code.ToString() == "412")
					{
						//Before information label on login
						//var createAccountScreen = new CreateAccount(resolver, userEmailId);
						//Navigator.NavigationService.Navigate(createAccountScreen);

						Application.Current.Dispatcher.Invoke((Action)delegate
						{
							ErrorMessage = (string)System.Windows.Application.Current.FindResource("AccountDoesNotExists");
						});


						// createAccountScreen.EmailTextBox.Text = UserEmail; // resolver.GetInstanceOf<CreateAccount>();

					}
					else if (deviceRegistrationResponse.error.code.ToString() == "403")
					{
						logger.Debug("Calling requestVerificationCode");
						webAPI.RequestVerificationCode(userEmailId);
						logger.Debug("Called requestVerificationCode");

						Application.Current.Dispatcher.Invoke((Action)delegate
						{
							var verificationScreen = new VerificationRequired(resolver, userEmailId, masterPassword);// resolver.GetInstanceOf<VerificationRequired>();
							Navigator.NavigationService.Navigate(verificationScreen);
						});

					}

					return true;
				}
				else
				{
					logger.Error("deviceRegistrationResponse didnt return error code");
					Application.Current.Dispatcher.Invoke((Action)delegate
					{
						string _message = System.Windows.Application.Current.FindResource("ServerErrorLoginMessage") as string;
						CustomInformationMessageBoxWindow c = new CustomInformationMessageBoxWindow(_message);
						c.ShowDialog();
					});
				}
				return true;
			}

			//pbData.DeviceUUID = deviceRegistrationResponse.devices[0].uuid.ToString();
			//todo write device uuid into device table

			//ServerErrorLoginMessage
			//MessageBox.Show("Local profile does not exist. Create new account or register device with existing account!");
			return true;
		}

		internal static void CheckRSA(GenerateKeysStepResult generateKeysStepResult, IPBData db, IPBWebAPI web)
		{
			// RIO - Hot path
			if (db == null || web == null) return;
			UserInfo ui = db.GetUserInfo(db.ActiveUser);
			if (ui == null) return;
			bool createKeys = true;
			if (ui.RSAPrivateKey != null)
			{
				using (var rsa = RSAKeyManagement.ImportPEMToRSACSP(ui.RSAPrivateKey))
				{
					createKeys = !(rsa != null && rsa.KeySize == 2048);
				}
			}
			if (createKeys)
			{
				Func<dynamic, string> extractPublicKeyFromAccountJSON = (dynamic json) =>
				{
					if (json == null)
					{
						logger.Error("CheckRSA: json is null");
					}
					else if (json.error != null)
					{
						logger.Error("CheckRSA: {0}", json.error);
					}
					else if (json.ToString().Contains("\"public_key\""))
					{
						return (string)json.account.public_key;
					}
					return null;
				};

				//generateKeysStep.Wait();

				byte[] publicKeyPem = generateKeysStepResult.PublicKey;
				ProtectedDataBlock privateKeyPem = generateKeysStepResult.PrivateKey;

				string public_keyB64 = Convert.ToBase64String(publicKeyPem);
				string pub_key = extractPublicKeyFromAccountJSON(web.GetAccount(db.ActiveUser + "|" + db.DeviceUUID, new PasswordBoss.WEBApiJSON.AccountRequest { }));
              
				if (!string.IsNullOrWhiteSpace(pub_key))
				{
					using (RSACryptoServiceProvider rsaPub = RSAKeyManagement.ImportPEMToRSACSP(Encoding.UTF8.GetString(Convert.FromBase64String(pub_key))))
					{
						if (rsaPub != null && rsaPub.KeySize == 2048) return;
					}
				}
				dynamic accountResponse = web.UpdateAccount(db.ActiveUser + "|" + db.DeviceUUID,
					new PasswordBoss.WEBApiJSON.AccountRequest { public_key = public_keyB64 });
				if (accountResponse == null)
				{
					logger.Error("CheckRSA: UpdateAccount failed");
				}
				else if (accountResponse.error != null)
				{
					logger.Error("CheckRSA: UpdateAccount - {0}", accountResponse.error);
				}
				else
				{
					pub_key = extractPublicKeyFromAccountJSON(accountResponse);
					if (pub_key != null && Encoding.UTF8.GetString(Convert.FromBase64String(pub_key)) == Encoding.UTF8.GetString(publicKeyPem))
					{
						ui.RSAPrivateKey = privateKeyPem;
						ui.PublicKey = publicKeyPem;
						if (!db.UpdateUserKeys(ui)) logger.Error("CheckRSA: UpdateUserKeys failed");
					}
				}
			}
		}

		private void ShowMainWindowIfNeeded(bool isDeviceTrusted)
		{
            logger.Debug("isDeviceTrusted log " + isDeviceTrusted);
			logger.Debug("ShowMainWindowIfNeeded start");
			bool showMainWindow = IsValidForShowingMainWindow(isDeviceTrusted);

			Application.Current.Dispatcher.Invoke((Action)delegate
			{
				if (showMainWindow)
				{
					_masterPass = null;
					var browserMonitor = resolver.GetInstanceOf<IBrowserMonitor>();
					if (browserMonitor == null)
					{
						logger.Error("Cant found IBrowserMonitor");
						return;
					}

					browserMonitor.StartMonitor();
					ShowMainWindow();
				}
				else
				{
                    if (webApi.HasInetConn)
                    {
                        CodeVerificationGridVisibility = true;
                        advertisingGrid.Visibility = Visibility.Collapsed;
                        //CenterWindowOnScreen();
                    }
				}
			});


		}

		private void CenterWindowOnScreen()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(
				   (a) =>
				   {
					   System.Threading.Thread.Sleep(100);
					   Application.Current.Dispatcher.BeginInvoke((Action)delegate
					   {
						   //LoginWindow loginWindow = ((PBApp)Application.Current).FindWindow<LoginWindow>();
						   //loginWindow.Top = loginWindow.Top - 100;
						   double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
						   double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
						   double windowWidth = Application.Current.MainWindow.Width;
						   double windowHeight = Application.Current.MainWindow.Height;
						   Application.Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
						   Application.Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2);
					   });
				   }
				   );
		}

		private Window CloseLoginWindow()
		{
			logger.Debug("Closing login window");
			OpenMainUI = true;
			LoginWindow loginWindow = ((PBApp)Application.Current).FindWindow<LoginWindow>();
			if (loginWindow == null) loginWindow = new LoginWindow(resolver);
			SystemTray _systemTray = new SystemTray();
			var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
			_systemTray.WindowClose(window);
			logger.Debug("Login window closed");
			return window;
		}

		private void ShowMainWindow()
		{
			// RIO - Hot path
			MainWindow mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (mainWindow == null)
			{
				mainWindow = new MainWindow(resolver);
			}
			else
			{
				mainWindow.Reload();
			}
			var window = CloseLoginWindow();
			System.Windows.Application.Current.MainWindow = mainWindow;
			mainWindow.Show();
			if (window.WindowState != WindowState.Maximized) mainWindow.Activate();
		}

		private bool IsValidForShowingMainWindow(bool isDeviceTrusted)
		{
			logger.Debug("IsValidForShowindMainWindow start. IsDeviceTrusted - {0}", isDeviceTrusted);
			//Debugger.Launch();
			bool showMainWindow = true;

			//if(IsTwoStepVerificationPassed)
			//{
			//    return showMainWindow; //we already went with two step verification
			//}

			IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
			if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_TwoStepAuthentication, showUIIfNotEnabled: false))
			{
				logger.Debug("two step verification not enabled - show main window");
				return showMainWindow;
			}

			bool multiFactorAuthEnabled = false;
			bool isOnline = false;

			logger.Debug("Getting user info");
			var userInfo = pbData.GetUserInfo(pbData.ActiveUser);


			if (userInfo != null)
			{
				logger.Debug("userInfo got - {0}", userInfo);
				isOnline = webApi.HasInetConn;

				if (userInfo.MultiFactorAuthentication)
				{
					logger.Debug("multifactor authentication!");
					if (!isOnline)
					{
						logger.Debug("No internet - show main window");
						ErrorMessage = "You have 2-step authentication enabled and must be online to do this";

						showMainWindow = false;
						multiFactorAuthEnabled = true;
						return showMainWindow; //we skip furter checking
					}
				}

				if (isOnline)
				{
					logger.Debug("have internet");
					//retreive info from web if this is enabled anyway

					logger.Debug("GetAccount calling");
					dynamic twoStepResp = webApi.GetAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new WEBApiJSON.AccountRequest { });
					logger.Debug("GetAccount calling");
					string multiFactorAuth = null;
					if (twoStepResp.error == null)
					{
						logger.Debug("TwoStepResponse.error == null");
						multiFactorAuth = twoStepResp.account.multi_factor_authentication;
						logger.Debug("MultiFactorAuth - {0}", multiFactorAuth);
					}
					else
					{
						logger.Debug("TwoStepResponse.error != null");
						logger.Error(twoStepResp.error.ToString());
					}

					if (!string.IsNullOrWhiteSpace(multiFactorAuth))
					{

						bool.TryParse(multiFactorAuth, out multiFactorAuthEnabled);
						if (multiFactorAuthEnabled)
						{
							logger.Debug("multifactorAuthEnabled - true, dont show main window");
							showMainWindow = false; //we will show two step dialog instead
						}

						if (userInfo.MultiFactorAuthentication != multiFactorAuthEnabled) //update DB only if it's different
						{
							logger.Debug("userInfo.MultiFactorAuthentication != multiFactorAuthEnabled");
							pbData.UpdateMultiFactorAuthentication(multiFactorAuthEnabled);
						}
					}
				}
			}

			logger.Debug("multiFactorAuthEnabled - {0},  isDeviceTrusted - {1}", multiFactorAuthEnabled, isDeviceTrusted);
			if (multiFactorAuthEnabled && isDeviceTrusted)
			{
				logger.Debug("ShowMainWindow set true");
				showMainWindow = true;
			}
			else if (multiFactorAuthEnabled && !isDeviceTrusted)
			{
				logger.Debug("ShowMainWindow set false");
				showMainWindow = false;
			}

			logger.Debug("ShowMainWindow - {0}", showMainWindow);
			if (!showMainWindow)
			{
				logger.Debug("Close profile");
				pbData.CloseProfile(); //two step needs to be passed
			}
			else
			{
				logger.Debug("UserPassword = string.Empty;");
				UserPassword = string.Empty;
			}

			logger.Debug("IsValidForShowingMainWindow ended. result - {0}", showMainWindow);
			return showMainWindow;
		}
	}
}
