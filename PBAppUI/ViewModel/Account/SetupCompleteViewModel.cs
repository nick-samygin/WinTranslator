using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using PasswordBoss.Views.Login;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;

namespace PasswordBoss.ViewModel.Account
{

	public class SetupCompleteViewModel : ViewModelBase
	{
		private static readonly ILogger logger = Logger.GetLogger(typeof(SetupCompleteViewModel));
		private static object synLock = new object();
		private bool successVerification;

		public bool SuccessVerification
		{
			get { return successVerification; }
			set { successVerification = value; }
		}

		/// <summary>
		/// defining commands for UI elements
		/// </summary>
		public AsyncRelayCommand<LoadingWindow> SetPinButtonCommand { get; set; }
		public RelayCommand CloseCommand { get; set; }

		SystemTray _systemTray = new SystemTray();
		private IPBData pbData = null;
		private IPBWebAPI webApi = null;
		private IResolver resolver = null;
		MainWindow mainWindow = null;
		LoginWindow loginWindow = null;

		public event EventHandler OnDialogCloseRequired;

		// private string email = null;
		public SetupCompleteViewModel(IResolver resolver)
		{
			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.webApi = resolver.GetInstanceOf<IPBWebAPI>();
			SetPinButtonCommand = new AsyncRelayCommand<LoadingWindow>(SetPinButtonClick, completed: (obj) => SubmitCompleted(obj));
			CloseCommand = new RelayCommand(CloseWindow);
			successVerification = false;

			CreateWindows();

		}

		private void SetPinButtonClick(object obj)
		{
			//PerformInitialSync();
			//syncWaiter.WaitOne();
			PerformInitialSync();
		}

		private void PerformInitialSync()
		{
			//Moved to previous screen (ConfirmmasterPassworViewModel)
			successVerification = true;
		}

		private void CreateWindows()
		{
			lock (synLock)
			{
				Application.Current.Dispatcher.BeginInvoke((Action)(() =>
				{
					loginWindow = ((PBApp)Application.Current).FindWindow<LoginWindow>();
					if (loginWindow == null) loginWindow = new LoginWindow(resolver);

					mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (mainWindow == null)
						mainWindow = new MainWindow(resolver);
					else
					{
						Dictionary<string, object> forceReload = new Dictionary<string, object>();
						forceReload.Add("NewUser", true);
						mainWindow.Reload(forceReload);
					}				
				}));
			}
		}

		public void SubmitCompleted(object obj)
		{
			// RIO - Hot path
			lock (synLock)
			{
				if (successVerification)
				{
					Application.Current.Dispatcher.BeginInvoke((Action)(() =>
					{
						try
						{
							var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
							if (inAppAnalyitics != null)
							{
								AccountCreatedItem acItem = null;
								var lastMessage = pbData.GetLastMessageHistory();
								var subscriptionInfo = pbData.GetSubscriptionInfo();
								if (lastMessage == null)
								{
									acItem = new AccountCreatedItem(null, 0, subscriptionInfo.SubscriptionType);
								}
								else
								{
									acItem = new AccountCreatedItem(new InAppMessageItem(lastMessage.RowId, lastMessage.AnalyticsCode, lastMessage.MsgType, lastMessage.Theme, (MarketingActionType)Enum.Parse(typeof(MarketingActionType), lastMessage.ButtonClicked), null, null), lastMessage.RowId, subscriptionInfo.SubscriptionType);
								}
								inAppAnalyitics.Get<Events.AccountCreated, AccountCreatedItem>().Log(acItem);

							}
							pbData.TryToClearMessageHistory();
						}
						catch (Exception ex)
						{
							logger.Error(ex.ToString());
						}
					}));

					if (OnDialogCloseRequired != null)
						OnDialogCloseRequired(this, EventArgs.Empty);

					loginWindow.NavigateloginScreens(LoginWindow.ScreenNames.LicenseScreen);
				}
			}
		}

		void sync_OnSyncFinished()
		{
			SyncImagesHelper syncImages = new SyncImagesHelper(pbData, webApi);
			syncImages.SyncImages();
		}

		void ProgressInfo(int currentStep, int maxRemainingSteps)
		{
			logger.Debug("Current step: {0}, max.remaining steps: {1}", currentStep, maxRemainingSteps);
		}

		/// <summary>
		/// For Closing login window
		/// </summary>
		private void CloseWindow(object sender)
		{
			var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
			_systemTray.WindowClose(window);
		}
    }
}