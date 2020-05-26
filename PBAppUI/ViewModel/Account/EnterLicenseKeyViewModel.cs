using PasswordBoss.BusinessLayer.License;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using PasswordBoss.Views.Dialogs.License;
using PasswordBoss.Views.Login;
using System;
using System.Linq;
using System.Windows;
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.ViewModel.Account
{
	public class EnterLicenseKeyViewModel : AccountCreationFlowViewModelBase
	{
		#region Commands
		public RelayCommand CloseCommand { get; set; }

		public AsyncRelayCommand<LicenseActivationDialog> ActivateCommand { get; set; }

		public RelayCommand BuyNowCommand { get; set; }

		public RelayCommand ContinueCommand { get; set; }

		public RelayCommand ActivationSuccessDialogOk { get; set; }
		#endregion

		#region Properties

		public bool IsActivateError { get { return !string.IsNullOrEmpty(errorText); } }

		private string errorText;
		public string ErrorText
		{
			get { return errorText; }
			set
			{
				errorText = value;
				RaisePropertyChanged("ErrorText");
				RaisePropertyChanged("IsActivateError");
			}
        }

		public string PremiumFeatureType { get { return LicenseMessages.LicenseType; } }
		public string PremiumFeatureDuration { get { return LicenseMessages.LicenseDurationStr; } }

		public string PromotionMessage { get { return LicenseMessages.LicenseMessage; } }

		private string activationKey = "";
		public string ActivationKey
		{
			get { return activationKey.ToString(); }
			set
			{
				if (!activationKey.Equals(value))
				{
					ErrorText = "";
				}
				activationKey = value;
				RaisePropertyChanged("ActivationKey");
				RaisePropertyChanged("IsActivateEnabled");
				RaisePropertyChanged("IsActivateError");
			}
		}

		private string ActivationKeyStripped
		{
			get { return activationKey.ToString().Replace("-", ""); }
		}

		public static bool IsValidActivateChar(char activateChar)
		{
			if (activateChar == ' ')
				return false;
			return char.IsLetterOrDigit(activateChar) || activateChar == '-';
		}
				
		public bool IsActivateEnabled { get { return ActivationKeyStripped.Any(); } }
		
		private bool isModalShown;

		public bool IsModalViewShown
		{
			get { return isModalShown; }
			private set
			{
				isModalShown = value;
				RaisePropertyChanged("IsModalViewShown");
			}
		}

        private int? _installType = null;
        public int? InstallType
        {
            get
            {
                return _installType;
            }
            private set
            {
                _installType = value;

                RaisePropertyChanged("InstallType");
            }
        }

		#endregion

		#region Fields
		private readonly ViewFadeTimerHelper fadeTimerHelper;
		private LoginWindow owner = null;
		private readonly IResolver resolver;
		private readonly ActivationSuccessfullDialog operationCompletedDialog = new ActivationSuccessfullDialog();
		private readonly LicenseActivationTypeBase LicenseMessages;
		private readonly IPBWebAPI webAPI;
		private readonly IPBData data;
		private UICallbacks uiCallbacks;
        private readonly LicenseActivationBusinessLayer licenseActivationBusinessLayer;
		
		#endregion

		#region Init
		public EnterLicenseKeyViewModel(IResolver resolver, LicenseActivationTypeBase licenseMessages, LicenseActivationBusinessLayer licenseActivationBusinessLayer) : base(resolver)
		{
			if (resolver == null)
				throw new ArgumentException("resolver");

			this.InstallType = licenseMessages.InstallType;

			this.resolver = resolver;
			this.webAPI = resolver.GetInstanceOf<IPBWebAPI>();
			this.data = resolver.GetInstanceOf<IPBData>();

			this.owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();
            this.LicenseMessages = licenseMessages;
        	this.ActivateCommand = CreateActivateCommand();
			this.BuyNowCommand = new RelayCommand(BuyNowCommandExecute);
			this.ContinueCommand = new RelayCommand(ContinueCommandExecute);
            this.ActivationSuccessDialogOk = new RelayCommand(ActivationSuccessDialogOkExecute);
			this.fadeTimerHelper = new ViewFadeTimerHelper(() => IsModalViewShown = true, () => IsModalViewShown = false, 3000);
            this.licenseActivationBusinessLayer = licenseActivationBusinessLayer;
			this.owner.Topmost = false;			
		}

        public void InitWithUICallback(UICallbacks callbacks)
        {
            this.uiCallbacks = callbacks ?? new UICallbacks();
        }

		private AsyncRelayCommand<LicenseActivationDialog> CreateActivateCommand()
		{
			Action<object> doWork = (obj) => fadeTimerHelper.FadeAction(() => ActivateCommandExecute());
			Action<object> onCompleted = (obj) => uiCallbacks.SetDefaultFocus();
			return new AsyncRelayCommand<LicenseActivationDialog>(doWork, completed: onCompleted, ownerWindow: owner);
		}
		#endregion

		#region CommandExecute

		private void ActivationSuccessDialogOkExecute(object obj)
		{
			operationCompletedDialog.Close();
			this.owner.NavigateloginScreens(LoginWindow.ScreenNames.PersonalInfoScreen);
		}
		
		private void ActivateCommandExecute()
		{
			var activationStatus = licenseActivationBusinessLayer.Activate(ActivationKeyStripped);
			ProcessActivationError(activationStatus);
			TryToProcessActivationSuccess(activationStatus);
			uiCallbacks.SetDefaultFocus();
        }

		private void ProcessActivationError(ActivateStatus activationStatus)
		{
			ErrorText = licenseActivationBusinessLayer.GetErrorStringFromActivateStatus(activationStatus);			
		}

		private void TryToProcessActivationSuccess(ActivateStatus activationStatus)
		{
			if (activationStatus != ActivateStatus.Success)
			{
				return;
			}

			OnSetupCompleteViewRequired();
		}
		
		private void BuyNowCommandExecute(object obj)
		{
			licenseActivationBusinessLayer.GetPremium();
        }

		private void ContinueCommandExecute(object obj)
		{
			this.owner.NavigateloginScreens(LoginWindow.ScreenNames.PersonalInfoScreen);
		}

		private void OnSetupCompleteViewRequired()
		{
			Application.Current.Dispatcher.Invoke((Action)(() =>
			{
				operationCompletedDialog.DataContext = this;
				operationCompletedDialog.Owner = owner;
				operationCompletedDialog.ShowDialog();
			}));
		}
		#endregion

		#region etc

		

		public class UICallbacks
		{
			private Action setDefaultFocus = () => { };
			public Action SetDefaultFocus
			{
				get
				{
					return setDefaultFocus;
				}
				set
				{
					if (value != null)
						setDefaultFocus = value;
				}
			}
		}
		#endregion

		protected override void LogStep(MarketingActionType type)
		{
            var item = new EnterLicenseKeyProductTourItem(7, type, InstallType);
			inAppAnalyitics.Get<Events.EnterLicenseProductTourEvent, EnterLicenseKeyProductTourItem>().Log(item);
		}
	}
}
