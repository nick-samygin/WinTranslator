using PasswordBoss.BusinessLayer;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using PasswordBoss.Views.Login;
using PasswordBoss.Views.UserControls;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel.Account
{
	public class CreateAccountViewModel : AccountCreationFlowViewModelBase
	{
		public static class ValidationProperties
		{
			//   public static readonly string IsActivated = "IsActivated";
			public static readonly string IsValidMasterPassword = "IsMasterPasswordValid";
			public static readonly string IsValidEmail = "IsValidEmail";
			public static readonly string IsPasswordsEqual = "IsPasswordsEqual";
			public static readonly string IsValidConfirmPassword = "IsConfirmPasswordValid";
			public static readonly string IsEmailHasValue = "IsEmailNotEmpty";
			public static readonly string IsMasterPasswordHasValue = "IsMasterPasswordNotEmpty";
			public static readonly string IsConfirmPasswordHasValue = "IsConfirmPasswordNotEmpty";
			//public static readonly string IsValidPasswords = "IsValidPasswords";
			//public static readonly string IsPasswordAllowed = "IsPasswordAllowed";
			// other properties in password wrapper
		}

		#region Fields

		public event EventHandler OnReset;
		public event EventHandler OnSubmit;
		SystemTray _systemTray = new SystemTray();
		private const string CurrentLoginWindow = "LoginWindow";
		private const string PrivacyPolicyLink = "http://www.passwordboss.com/privacy-policy/?utm_source=PC%26utm_medium=login%26utm_campaign=InAppSupport";
		private const string TermsOfUseLink = "http://www.passwordboss.com/terms-of-use/?utm_source=PC%26utm_medium=login%26utm_campaign=InAppSupport";
		private static readonly ILogger logger = Logger.GetLogger(typeof(CreateAccount));
		private IResolver resolver = null;
		private IPBData pbData = null;
		private IPBWebAPI webApi = null;
		private Common _commonObj = new Common();
		private readonly Validator<CreateAccountViewModel> validator;
		public const string LoginViewPath = "..\\Views\\Login\\Login.xaml";
		private readonly CreateAccountLayer createAccountLayer;
		private SetupCompleteView _accountCreatedDialog = null;
		private AccountExistingDialog _accountExistingDialog = null;

		#endregion

		public CreateAccountViewModel(IResolver resolver, string email)
			: base(resolver)
		{
			this.UserEmailId = email;
			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.webApi = resolver.GetInstanceOf<IPBWebAPI>();

			Task.Factory.StartNew(() =>
			{
				this.webApi.GetEndpoints();
			});

			// logic for account creation with provided callbacks for GUI calls
			this.createAccountLayer = new CreateAccountLayer(resolver, LogStep);
			this.createAccountLayer.BackToLoginScreenRequired += (o, e) => OnBackToLoginScreen();
			this.createAccountLayer.MessageRaised += (o, e) => OnMessageRaised(e.Message);
			this.createAccountLayer.VerificationRequired += (o, e) => OnVerificationRequired();
			this.createAccountLayer.SetupCompleteRequired += (o, e) => OnSetupCompleteViewRequired();

			this._userPasswordWrapper = new PasswordBoxViewModel(this, "UserPasswordWrapper");
			this._userConfirmPasswordWrapper = new PasswordBoxViewModel(this, "UserConfirmPasswordWrapper");

			this.CloseCommand = new RelayCommand(CloseCommandExecute);
			this.EmailTextChangedCommand = new RelayCommand(EmailTextChangedExecute);
			this.PrivacyPolicyCommand = new RelayCommand((x) => OpenUri(PrivacyPolicyLink));
			this.TermsOfServiceCommand = new RelayCommand((x) => OpenUri(TermsOfUseLink));
			this.PasswordFieldGotFocusCommand = new RelayCommand(UserPasswordGotFocus);
			this.LoginExistingButtonCommand = new RelayCommand(LoginExistingCommand);
			this.CreateNewButtonCommand = new RelayCommand(CreateNewButtonCommandExecute);

			// validator helper for validation properties
			this.validator = new Validator<CreateAccountViewModel>(this);
			this.validator.AddProperty(ValidationProperties.IsValidEmail, (v) => _commonObj.IsEmailValid(v.UserEmailId));
			this.validator.AddProperty(ValidationProperties.IsPasswordsEqual, (v) =>
			{
				return v.UserPasswordWrapper.Password == v.UserConfirmPasswordWrapper.Password ; 
			});
			this.validator.AddProperty(ValidationProperties.IsValidMasterPassword, (v) => v.UserPasswordWrapper.IsPasswordValid);
			this.validator.AddProperty(ValidationProperties.IsValidConfirmPassword, (v) => v.UserConfirmPasswordWrapper.IsPasswordValid);

			this.validator.AddProperty(ValidationProperties.IsEmailHasValue, (v) => !string.IsNullOrEmpty(v.UserEmailId));
			this.validator.AddProperty(ValidationProperties.IsMasterPasswordHasValue, (v) => !string.IsNullOrEmpty(v.UserPasswordWrapper.Password));
			this.validator.AddProperty(ValidationProperties.IsConfirmPasswordHasValue, (v) => !string.IsNullOrEmpty(v.UserConfirmPasswordWrapper.Password));

			this.validator.SetProperty(ValidationProperties.IsValidEmail, true);
			this.validator.SetProperty(ValidationProperties.IsPasswordsEqual, true);
		}

		#region Fader

		private bool isActivated = true;
		public bool IsActivated
		{
			get { return isActivated; }
			set
			{
				isActivated = value;
				RaisePropertyChanged("IsActivated");
			}
		}

		private void RunDialogAction(Action action)
		{
			IsActivated = false;
			action();
			IsActivated = true;
		}

		#endregion

		#region Validation fields

		public bool ValidateEmail()	{ return validator.Validate(ValidationProperties.IsValidEmail); }
		public bool ValidatePasswordsEqual() { return validator.Validate(ValidationProperties.IsPasswordsEqual); }
		public bool ValidateMasterPassword() { return validator.Validate(ValidationProperties.IsValidMasterPassword); }
		public bool ValidateConfirmPassword() { return validator.Validate(ValidationProperties.IsValidConfirmPassword); }

		// other validation delegated to password wrapper object.
		public bool IsValidEmail { get { return validator.IsValid(ValidationProperties.IsValidEmail); } }
		public bool IsPasswordsEqual { get { return validator.IsValid(ValidationProperties.IsPasswordsEqual); } }
		public bool IsValidMasterPassword { get { return validator.IsValid(ValidationProperties.IsValidMasterPassword); } }

		public bool IsEmailHasValue { get { return validator.IsValid(ValidationProperties.IsEmailHasValue); } }
		public bool IsPasswordHasValue { get { return validator.IsValid(ValidationProperties.IsMasterPasswordHasValue); } }
		public bool IsConfirmPasswordHasValue { get { return validator.IsValid(ValidationProperties.IsConfirmPasswordHasValue); } }

		#endregion

		#region Input Fields

		private string _userEmailId;

		public string UserEmailId
		{
			get { return _userEmailId; }
			set
			{
				if (_userEmailId != value)
				{
					_userEmailId = value;
					RaisePropertyChanged("UserEmailId");
				}
			}
		}

		private PasswordBoxViewModel _userPasswordWrapper;
		public PasswordBoxViewModel UserPasswordWrapper
		{
			get { return _userPasswordWrapper; }
		}

		private PasswordBoxViewModel _userConfirmPasswordWrapper;
		public PasswordBoxViewModel UserConfirmPasswordWrapper
		{
			get { return _userConfirmPasswordWrapper; }
		}

		#endregion

		#region Commands

		public RelayCommand EmailTextChangedCommand { get; set; }
		public RelayCommand PasswordFieldGotFocusCommand { get; set; }

		private AsyncRelayCommand<AccountCreationDialog> createMyAccountButtonCommand;
		public AsyncRelayCommand<AccountCreationDialog> CreateMyAccountButtonCommand
		{
			get
			{
				if (createMyAccountButtonCommand == null)
				{
					createMyAccountButtonCommand = new AsyncRelayCommand<AccountCreationDialog>(CreateMyAccountButtonCommandExecute);
				}

				return createMyAccountButtonCommand;
			}
			set
			{
				createMyAccountButtonCommand = value;
			}
		}
		public RelayCommand CloseCommand { get; set; }
		public RelayCommand PrivacyPolicyCommand { get; set; }
		public RelayCommand TermsOfServiceCommand { get; set; }
		public RelayCommand LoginExistingButtonCommand { get; set; }
		public RelayCommand CreateNewButtonCommand { get; set; }

		#endregion

		#region Methods

		private void OnBackToLoginScreen()
		{
			//createAccountLayer.CancelBackgroundTasks();
			Application.Current.Dispatcher.Invoke((Action)delegate
			{
				Login login = resolver.GetInstanceOf<Login>();
				login.EmailTextBox.Text = UserEmailId;
				Navigator.NavigationService.Navigate(login);
			});
		}

		private void OnMessageRaised(string message)
		{
			// To - Do Change style of Dialog
			MessageBox.Show(message);
		}

		private void OnVerificationRequired()
		{
			Application.Current.Dispatcher.Invoke((Action)delegate
			{
				var verificationScreen = new VerificationRequired(resolver, UserEmailId, UserPasswordWrapper.Password);// resolver.GetInstanceOf<VerificationRequired>();
				Navigator.NavigationService.Navigate(verificationScreen);
			});
		}

		private void OnSetupCompleteViewRequired()
		{
			Application.Current.Dispatcher.Invoke((Action)(() =>
			{
				_accountCreatedDialog = new SetupCompleteView(new SetupCompleteViewModel(this.resolver));
				_accountCreatedDialog.Owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();
				_accountCreatedDialog.Topmost = true;
				_accountCreatedDialog.Topmost = false;
				CreateMyAccountButtonCommand.HideLoadingWindow();

				RunDialogAction(() =>
				{
					_accountCreatedDialog.ShowDialog();
				});
			}));
		}

		private void CreateNewButtonCommandExecute(object elementObj)
		{
			if (_accountExistingDialog != null)
			{
				_accountExistingDialog.Close();
				_accountExistingDialog = null;
			}

			UserEmailId = String.Empty;
			UserPasswordWrapper.Password = string.Empty;
			UserConfirmPasswordWrapper.Password = string.Empty;

			if (OnReset != null)
				OnReset(this, EventArgs.Empty);

			validator.SetValidAll(true);
		}

		public void ResetValidation(string field)
		{
			validator.SetValid(field);
			RaisePropertyChanged(field);
		}

		/// <summary>
		/// used for to check all required fields are persent or not on  Email txtbox and change place holder text
		/// </summary>
		/// <param name="element"></param>
		private void EmailTextChangedExecute(object element)
		{
			var elements = _commonObj.ReturnElement(element);
			var passwordBox = elements[0] as PasswordTextBox;
			var textBox = elements[1] as TextBox;

			validator.IsValid(ValidationProperties.IsValidEmail);
		}

		private void UserPasswordGotFocus(object obj)
		{
			if (obj != null)
			{
				PasswordBox passwordBoxControl = obj as PasswordBox;
				passwordBoxControl.Focus();
			}
		}

		private void LoginExistingCommand(object elementObj)
		{
			//createAccountLayer.CancelBackgroundTasks();
			try
			{
				LogStep(MarketingActionType.ExistingAccount);
				if (_accountExistingDialog != null)
				{
					_accountExistingDialog.Close();
					_accountExistingDialog = null;
				}

				Login login = resolver.GetInstanceOf<Login>();
				login.EmailTextBox.Text = UserEmailId;
				Navigator.NavigationService.Navigate(login);

			}
			catch (Exception ex)
			{
				MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
				logger.Error(ex.Message);
			}
		}

		public void RefreshValidations()
		{
			validator.SetValidAll(true);
			validator.SetProperty(ValidationProperties.IsValidMasterPassword, false);
			validator.SetProperty(ValidationProperties.IsValidConfirmPassword, false);
		}

		private void CreateMyAccountButtonCommandExecute(object obj)
		{
			IsActivated = false;
			validator.Refresh(true);

			if (OnSubmit != null)
				OnSubmit(this, EventArgs.Empty);

			if (validator.IsAnyError())
			{
				IsActivated = true;
				return;
			}

			LogStep(MarketingActionType.Continue);

			//check if profile exists on local disk

			var authProfileExistsTuple = createAccountLayer.AuthenticateUser(UserEmailId, UserPasswordWrapper.Password);


			if (authProfileExistsTuple.Item1) // isAuthanticated
			{
				this.UserPasswordWrapper.Password = string.Empty;
				this.UserConfirmPasswordWrapper.Password = string.Empty;
			}

			if (authProfileExistsTuple.Item2) // is profile exists
			{
				Application.Current.Dispatcher.Invoke((Action)(() =>
					{
						_accountExistingDialog = new AccountExistingDialog(this);
						_accountExistingDialog.Owner = ((PBApp)Application.Current).FindWindow<LoginWindow>(); ;

						RunDialogAction(() =>
						{
							_accountExistingDialog.ShowDialog();
						});
					}));
			}
			else
			{
				RunDialogAction(() =>
				{
					createAccountLayer.CreateAccount(UserEmailId, UserPasswordWrapper.Password);
				});

			}
		}

		private void CloseCommandExecute(object obj)
		{
			LogStep(MarketingActionType.Close);
			var window = _systemTray.CurrentWindow(CurrentLoginWindow);
			_systemTray.WindowClose(window);
		}

		private void OpenUri(string url)
		{
			try
			{
				BrowserHelper.OpenInDefaultBrowser(new Uri(url));
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		protected override void LogStep(MarketingActionType type)
		{
			var item = new OnboardingItem(OnboardingSteps.AccountCreated, type);
			inAppAnalyitics.Get<Events.OnboardingEvent, OnboardingItem>().Log(item);
		}


		#endregion
	}
}