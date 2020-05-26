using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.Views.Login;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace PasswordBoss.Views
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	[Export]
	public partial class LoginWindow
	{
        public static class ScreenNames
        {
            public static readonly string AccountCreation = "AccountCreation";
            public static readonly string ShowProductTour = "ShowProductTour";
            public static readonly string LicenseScreen = "LicenseScreen";
            public static readonly string BrowserExtention = "BrowserExtention";
			public static readonly string PersonalInfoScreen = "PersonalInfoScreen";
        }
		private static readonly ILogger logger = Logger.GetLogger(typeof(LoginWindow));

		SystemTray _systemTray = new SystemTray();

		private IPBData pbData = null;
		private IResolver resolver;
		private LoginLanguage loginLanguage;
		private bool _isProductTourMode = false;

        private Task<CreateAccountViewModel> createAccountEmptyTask;
        
		[ImportingConstructor]
		public LoginWindow([Import(typeof(IResolver))] IResolver resolver)
		{
			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.pbData.OnInvalidVersion += pbData_OnInvalidVersion;

			InitializeComponent();

			this.Loaded += onLoaded;

			loginLanguage = new LoginLanguage(resolver);
			NavigateloginScreens(ScreenNames.ShowProductTour);
			Closing += OnLoginClosing;
			CenterWindowOnScreen();

            createAccountEmptyTask = new Task<CreateAccountViewModel>(() => new CreateAccountViewModel(resolver, ""));
            createAccountEmptyTask.Start();

			this.IsVisibleChanged += LoginWindow_IsVisibleChanged;
		}

		void LoginWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Visibility == System.Windows.Visibility.Visible)
			{
				resolver.GetInstanceOf<PasswordBoss.IBrowserMonitor>().StopMonitor();
			}
		}


		private Login.Login _loginPageInstance;
		public Login.Login Login
		{
			get
			{
				if (_loginPageInstance == null)
				{
					_loginPageInstance = new Login.Login(resolver);
				}
				return _loginPageInstance;
			}
		}

		private void CenterWindowOnScreen()
		{
			var info = pbData.GetSubscriptionInfo();

			if (true) // !PremiumExpiring.ShowUpgradePanel(info)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(
					   (a) =>
					   {
						   System.Threading.Thread.Sleep(100);
						   Application.Current.Dispatcher.BeginInvoke((Action)delegate
						   {
							   double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
							   double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
							   double windowWidth = Application.Current.MainWindow.Width;
							   double windowHeight = Application.Current.MainWindow.Height;
							   Application.Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
							   Application.Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2) - 70;
						   });
					   }
					   );
			}
		}

		void pbData_OnInvalidVersion(DBFileType dbt)
		{
			if (dbt == DBFileType.Store)
			{
				MessageBox.Show(Application.Current.Resources["InstallationIsOutdated"].ToString() + "\n" +
					Application.Current.Resources["InstallationIsOutdated_UpdateNow"].ToString());
				MainFrame.Navigate(Login);
			}
		}

		public void ShowNoMainUI()
		{
			if (Login != null) Login.OpenMainUI = false;
			base.Show();
		}

		private void onLoaded(object sender, RoutedEventArgs e)
		{
			if (_isProductTourMode)
			{
				this.Hide();
				this.ShowInTaskbar = false;
			}
		}

		private void OnLoginClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Window w = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
			if (w != null) _systemTray.WindowClose(w);
			e.Cancel = true;
		}

		/// <summary>
		/// navigate to login page
		/// </summary>
		public void NavigateloginScreens(string parameter = "")
		{
			this.Show();
			this.Topmost = true;
			this.ShowInTaskbar = true;
			if (parameter == ScreenNames.ShowProductTour && !this.pbData.AnyAccountExists())
			{
				var pTour = resolver.GetAllInstancesOf<IProductTour>().FirstOrDefault();
				if (pTour != null)
				{
					if (this.IsLoaded)
					{
						this.Hide();
						this.ShowInTaskbar = false;
					}

					if (!_isProductTourMode)
					{
						_isProductTourMode = true;

						this.Activate();
						this.Topmost = true;
						this.Topmost = false;
						this.Focus();
						pTour.Show(null);
                        // HACK:
                        // moved down because pTour object get constructed twice, and in second time we loose this binding, which is actually used.
                        // search for NavigateLoginScreens("ShowProductTour")
                        // pTour.DialogClosed += onDialogClosed;
					}

                    pTour.DialogClosed += onDialogClosed;

					if (pTour.ContentPanel is Window)
					{
						(pTour.ContentPanel as Window).Show();
					}

					return;
				}
			}

			if (parameter == ScreenNames.AccountCreation)
			{
				MainFrame.Navigate(new CreateAccount(resolver, createAccountEmptyTask.Result));
			}
			else if (parameter == ScreenNames.LicenseScreen)
			{

                try
                {
                    var pbSync = resolver.GetInstanceOf<IPBSync>();
                    bool syncRejectLicenseScreen = 
						pbSync.LastSyncData.SubscriptionInfo.Equals("paid", StringComparison.InvariantCultureIgnoreCase)
                        || pbSync.LastSyncData.SubscriptionInfo.Equals("third-party", StringComparison.InvariantCultureIgnoreCase);

                    if (syncRejectLicenseScreen)
                    {
                        logger.Debug("NavigateLoginScreens - PBSync.SubscriptionInfo signals to NOT show license screen");
                        NavigateloginScreens(ScreenNames.PersonalInfoScreen);
						return;
                    }
                }
                catch(Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                var licenseActivationBusinessLayer = new BusinessLayer.License.LicenseActivationBusinessLayer(resolver);
                var licenseFactory = new BusinessLayer.License.LicenseFactory();
                var installType = licenseActivationBusinessLayer.GetInstallTypeRegistryValue();
                var license = licenseFactory.CreateLicense(installType, licenseActivationBusinessLayer.GetLicenseTermDaysRegistryValue());

                if (license is BusinessLayer.License.DontShowLicenseType)
                {
                    logger.Debug("NavigateLoginScreens - LicenseScreen. License screen not required to show. skip.");
                    NavigateloginScreens(ScreenNames.PersonalInfoScreen);
                }
                else if (license == null)
                {
                    logger.Error("NavigateLoginScreens - LicenseScreen. Critical! Unable to create license screen!");
                    NavigateloginScreens(ScreenNames.PersonalInfoScreen);
                }
                else
                {
                    var enterLicenseKeyViewModel = new EnterLicenseKeyViewModel(resolver, license, licenseActivationBusinessLayer);
				    var dialog = new EnterLicenseKey(resolver, enterLicenseKeyViewModel);
				    MainFrame.Navigate(dialog);
                }

			}
			else if (parameter == ScreenNames.PersonalInfoScreen)
			{
				MainFrame.Navigate(new PersonalInfoSetup(resolver));
			}
            else if (parameter == ScreenNames.BrowserExtention)
			{
				var setupProvider = ViewModel.BrowserExtentions.SetupProviderFactory.GetDefaultBrowser();
				if (setupProvider != null)
				{
					MainFrame.Navigate(new BrowserExtentionSetup(resolver, setupProvider));
				}
				else
				{
                    logger.Debug("NavigateLoginScreens - BrowserExtention, unable to get default browser, show main window");
					SystemTray _systemTray = new SystemTray();
					var window = _systemTray.CurrentWindow(DefaultProperties.CurrentLoginWindow);
					_systemTray.WindowClose(window);
					var mainWindow = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (mainWindow == null)
						mainWindow = new MainWindow(resolver);
				}
			}
			else
			{
				MainFrame.Navigate(Login);
			}
		}

		private void onDialogClosed(object arg1, RoutedEventArgs arg2)
		{
			this.Topmost = true;

			_isProductTourMode = false;



			var pTour = resolver.GetInstanceOf<IProductTour>();

			if (pTour != null)
			{
				pTour.DialogClosed -= onDialogClosed;

				switch (pTour.ClosedType)
				{
					case ClosedType.AccountCreation:
						{
							MainFrame.Navigate(new CreateAccount(resolver, createAccountEmptyTask.Result));
							break;
						}

					case ClosedType.SignIn:
						{
							MainFrame.Navigate(Login);
							break;
						}
				}
			}

			this.Show();
			this.ShowInTaskbar = true;
		}

		private void HandleNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
		{
			if (e.NavigationMode == NavigationMode.Back)
			{
				e.Cancel = true;
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);

			if (sizeInfo.HeightChanged)
			{
				this.Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;
			}

			if (sizeInfo.WidthChanged)
			{
				this.Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
			}
		}
	}
}