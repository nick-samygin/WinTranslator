using PasswordBoss.Helpers;
using PasswordBoss.Views;
using System.Windows;
using PasswordBoss.PBAnalytics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public class BrowserExtentionSetupViewModel : Account.AccountCreationFlowViewModelBase
	{
		private readonly ILogger logger = Logger.GetLogger(typeof(BrowserExtentionSetupViewModel));
		public event EventHandler Closed;
		private readonly SetupProviderBase setupProvider;


		public string BrowserShortName { get { return setupProvider.BrowserShortName; } }
		public string BrowserFullName { get { return setupProvider.BrowserFullName; } }

		public string BrowserIconPath { get { return setupProvider.BrowserIcon; } }
		public string BrowserScreenshotPath { get { return setupProvider.BrowserScreenshot; } }

		public RelayCommand GetButtonAndContinue { get; set; }

		public BrowserExtentionSetupViewModel(IResolver resolver, SetupProviderBase setupProvider)
			: base(resolver)
		{

			this.setupProvider = setupProvider;

			this.GetButtonAndContinue = new RelayCommand(GetButtonAndContinueExecute);

			Monitor.Enter(BrowserHelper.BrowserExtentionInstallLocker);
		}

		private void GetButtonAndContinueExecute(object obj)
		{
			LogStep(MarketingActionType.Continue);

			if (Closed != null)
				Closed(this, EventArgs.Empty);


			ShowMainWindow(forceOnTop: false);


			Task.Factory.StartNew(() =>
			{
				setupProvider.OnBeforeSetup();
				BrowserHelper.OpenInDefaultBrowser(new System.Uri(setupProvider.ExtentionGetLinkUrl));
				setupProvider.OnAfterSetup();

			});



			var browserMonitor = resolver.GetInstanceOf<IBrowserMonitor>();
			if (browserMonitor == null)
			{
				logger.Error("Cant found IBrowserMonitor");
				return;
			}

			browserMonitor.StartMonitor();

			Monitor.Exit(BrowserHelper.BrowserExtentionInstallLocker);
		}


		protected override void LogStep(MarketingActionType type)
		{
			var item = new BrowserExtentionInstallProductTourItem(BrowserShortName, type);
			inAppAnalyitics.Get<Events.BrowserExtentionInstallProductTourEvent, BrowserExtentionInstallProductTourItem>().Log(item);
		}
	}
}
