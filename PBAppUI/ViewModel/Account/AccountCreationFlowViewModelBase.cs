using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using System.Windows;

namespace PasswordBoss.ViewModel.Account
{
	public abstract class AccountCreationFlowViewModelBase : ViewModelBase
	{
		protected readonly IInAppAnalytics inAppAnalyitics = null;
		protected readonly IResolver resolver;

		public AccountCreationFlowViewModelBase(IResolver resolver)
		{
			this.inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
			this.resolver = resolver;
		}

		protected abstract void LogStep(MarketingActionType type);

		protected void ShowMainWindow(bool forceOnTop = true)
		{
			SystemTray _systemTray = new SystemTray();
			var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
			_systemTray.WindowClose(window);
			var mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (mainWindow == null)
				mainWindow = new MainWindow(resolver);

			if (forceOnTop)
			{
				mainWindow.Topmost = true;
				mainWindow.Activate();
				mainWindow.Topmost = false;
			}
			else
			{
				mainWindow.Topmost = false;
			}

			mainWindow.Show();

		}		
	}
}
