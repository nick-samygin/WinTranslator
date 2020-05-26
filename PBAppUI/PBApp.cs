using Microsoft.Win32;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.ViewModel.ApplicationUpdates;
using PasswordBoss.Views;
using PasswordBoss.Views.ApplicationSync;
using PasswordBoss.Views.ApplicationUpdates;
using PasswordBoss.Views.FeatureNotEnabled;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace PasswordBoss
{
	public class PBApp : Application, IAppCommand, IApplication
	{
		private IResolver moduleLoader = null;
		private readonly Dictionary<string, Uri> resourcePaths;
		private Dictionary<string, string> categories;
		private static readonly ILogger logger = Logger.GetLogger(typeof(PBApp));
		private IPBData pbData = null;
		private IPBSync sync = null;
		private IPBWebAPI webAPI = null;
		private IAppUpdater updater = null;
		internal readonly IBrowserMonitor BrowserMonitor;
		private SystemTray sysTray;
		private string currentLanguage = "en";
		private static object syncLock = new object();
		private readonly SystemIdlePoller systemIdlePoller;

		private Dictionary<string, MethodInfo> cmdCache = new Dictionary<string, MethodInfo>();
		private object cmdCacheLock = new object();

		public event EventHandler OnBeforeApplicationShutDown;
		private const int CLIPBOARD_TIMER_INTERVAL = 120;
		private System.Timers.Timer clipboardTimer = new System.Timers.Timer(CLIPBOARD_TIMER_INTERVAL * 1000);

		private readonly string[] args;
		public string[] ApplicationArguments { get { return args; } }

		public PBApp(IResolver moduleLoader, string[] args)
		{
			this.args = args == null ? new string[0] : args;
			clipboardTimer.Elapsed += clipboardTimer_Elapsed;
			this.moduleLoader = moduleLoader;
			pbData = moduleLoader.GetInstanceOf<IPBData>();
			// pbData.OnProfileUnlock += pbData_OnProfileUnlock;
			pbData.OnUserLoggedIn += pbData_OnUserLoggedIn;
			pbData.OnCloseProfile += pbData_OnCloseProfile;
			currentLanguage = pbData.GetConfigurationValueByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, "last_selected_lang");
			if (string.IsNullOrWhiteSpace(currentLanguage)) currentLanguage = "en";
			sync = moduleLoader.GetInstanceOf<IPBSync>();
			sync.OnGetMergePassword(GetMergePassword);
			webAPI = moduleLoader.GetInstanceOf<IPBWebAPI>();
			BrowserMonitor = moduleLoader.GetInstanceOf<IBrowserMonitor>();
			updater = moduleLoader.GetInstanceOf<IAppUpdater>();
			updater.UpdatedVersionDetected += UpdatedVersionDetected;
			categories = new Dictionary<string, string>();
			resourcePaths = new Dictionary<string, Uri>();
			resourcePaths.Add("Fonts", new Uri("/font;component/FontProperty.xaml", UriKind.RelativeOrAbsolute));
			resourcePaths.Add("Themes", new Uri("/theme;component/Theme.xaml", UriKind.RelativeOrAbsolute));
			resourcePaths.Add("Images", new Uri("/image;component/ImageResource.xaml", UriKind.RelativeOrAbsolute));
			resourcePaths.Add("LocalStyles1", new Uri("pack://application:,,,/PBAppUI;component/resources/dictionary/styles/passwordbossresource.xaml", UriKind.RelativeOrAbsolute));
			resourcePaths.Add("LocalStyles2", new Uri("pack://application:,,,/PBAppUI;component/resources/dictionary/styles/primarydashboardresource.xaml", UriKind.RelativeOrAbsolute));
			resourcePaths.Add("LocalStyles3", new Uri("pack://application:,,,/PBAppUI;component/resources/dictionary/styles/AddControlStyles.xaml", UriKind.RelativeOrAbsolute));
			foreach (var uri in resourcePaths.Values)
			{
				LoadAndMerge(uri);
			}
			SetLanguage(currentLanguage);
			SystemTray.SetResolver(moduleLoader);
			sysTray = new SystemTray();
			sysTray.InitializeTrayProperties();
			SystemTray.OnBeforeShutDownHandler += SystemTray_OnBeforeShutDownHandler;
			LoginWindow loginWindow = new LoginWindow(moduleLoader);
			Navigator.NavigationService = loginWindow.MainFrame.NavigationService;
			if (!pbData.ControllerIsUpToDate())
				pbData_OnInvalidVersion(DBFileType.Controller);
			else
				pbData.OnInvalidVersion += pbData_OnInvalidVersion;

			Application.Current.Dispatcher.UnhandledException += onDispatcherUnhandledException;
			systemIdlePoller = new SystemIdlePoller();

		}

		void clipboardTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Application.Current.Dispatcher.Invoke((Action)delegate
			{
				Clipboard.Clear();
			});

			clipboardTimer.Stop();
		}

		//private bool masterPasswordDiffers = false;

		void GetMergePassword(CredentialsRequiredEventArgs e)
		{
			ManualResetEvent evDone = new ManualResetEvent(false);
			Application.Current.Dispatcher.BeginInvoke((Action)(() =>
			{
				try
				{
					MasterPasswordDialog masterDialog = new MasterPasswordDialog(pbData);
					//MasterPwdBox masterDialog = new MasterPwdBox();
					masterDialog.Topmost = true;
					masterDialog.UpdatePwdTextVisibility = true;
					masterDialog.SkipPwdValidation = true;
					bool? res = masterDialog.ShowDialog();
					if (res.HasValue && res.Value)
					{
						e.Password = masterDialog.Pwd;
						//SystemTray._correctPasswordEntered = true;
						//masterPasswordDiffers = true;
					}
					else
					{
						//SystemTray._correctPasswordEntered = false;
						e.Cancel = true;
					}
				}
				catch (Exception exc)
				{
					logger.Error(exc.ToString());
				}
				evDone.Set();
			}));
			evDone.WaitOne();
		}

		void SystemTray_OnBeforeShutDownHandler(object sender, EventArgs e)
		{
			if (OnBeforeApplicationShutDown != null)
			{
				OnBeforeApplicationShutDown(typeof(PBApp), EventArgs.Empty);
			}
		}

		void pbData_OnCloseProfile()
		{
			invalidVersionOnSyncMsgSeen = false;
			invalidVersionMsgSeen = false;
		}

		private static bool invalidVersionMsgSeen = false, invalidVersionOnSyncMsgSeen = false;

		void pbData_OnInvalidVersion(DBFileType dbt)
		{
            if (!pbData.UpdateTonightScheduled())
            {
			AppVersion version = null;
			bool hasUpdate = updater.ServerHasUpdates(out version);
			UpdateAvailable win = new UpdateAvailable();

                UpdateAvailableViewModel dc = new UpdateAvailableViewModel(pbData);
			lock (UpdateAvailableViewModel.UpdateLocker)
			{
				if (!UpdateAvailable.IsShown)
				{
					var currentVersion = AppVersion.GetInstalledVersion();
					if (currentVersion.Rank > version.Rank)
					{
						logger.Debug("app already updated");
						return;
					}

					switch (dbt)
					{
						case DBFileType.Controller:
							lock (syncLock)
							{
								if (hasUpdate)
								{

									dc.LaterButtonVisibility = false;
									dc.ShowIcon = true;
									dc.HeaderText = Application.Current.Resources["InstallationIsOutdatedRequiredUpdate"].ToString();
									dc.BoldBodyText = Application.Current.Resources["InstallationIsOutdated_UpdateNowRequired"].ToString();
									win.DataContext = dc;
									dc.UpdateNowTriggered += (o, e) =>
									{
										if (!updater.RunUpdate())
											MessageBox.Show("Failed to update", "Password Boss updater", MessageBoxButton.OK);
									};
									win.ShowDialog();
								}
								Quit();
							}
							break;
						case DBFileType.Store:
							lock (syncLock)
							{
								if (!invalidVersionMsgSeen)
								{
									if (hasUpdate)
									{
										dc.LaterButtonVisibility = false;
										dc.ShowIcon = true;
										dc.HeaderText = Application.Current.Resources["InstallationIsOutdatedRequiredUpdate"].ToString();
										dc.BoldBodyText = Application.Current.Resources["InstallationIsOutdated_UpdateNowRequired"].ToString();
										win.DataContext = dc;
										dc.UpdateNowTriggered += (o, e) =>
										{
											if (!updater.RunUpdate())
												MessageBox.Show("Failed to update", "Password Boss updater", MessageBoxButton.OK);
										};
										win.ShowDialog();

									}
								}

								invalidVersionMsgSeen = true;
							}
							break;
						case DBFileType.SyncStore:
							lock (syncLock)
							{
								if (!invalidVersionOnSyncMsgSeen && hasUpdate)
								{
									dc.ShowIcon = true;
									dc.HeaderText = Application.Current.Resources["InstallationIsOutdated"].ToString();
									dc.BoldBodyText = Application.Current.Resources["InstallationIsOutdated_CloudSyncDisabledBold"].ToString();
									dc.RegularBodyText = Application.Current.Resources["InstallationIsOutdated_CloudSyncDisabled"].ToString();
									win.DataContext = dc;
									dc.UpdateNowTriggered += (o, e) =>
									{
										if (!updater.RunUpdate())
											MessageBox.Show("Failed to update", "Password Boss updater", MessageBoxButton.OK);
									};
									win.ShowDialog();
								}
								invalidVersionOnSyncMsgSeen = true;
							}
							break;
					}
				}
			}
		}
		}

		public IResolver GetResolver()
		{
			return moduleLoader;
		}

		public string CurrentLanguage { get { return currentLanguage; } }

		public void SetLanguage(string lang = "")
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
			}

			lock (syncLock)
			{
				var textResources = moduleLoader.GetAllInstancesOf<ITextResource>().Where(r => r.Lang == lang).ToList();
				if (textResources.Count < 1 && lang != "en")
				{
					lang = "en";
					textResources = moduleLoader.GetAllInstancesOf<ITextResource>().Where(r => r.Lang == lang).ToList();
				}
				foreach (var r in textResources)
				{
					if (resourcePaths.ContainsKey(r.ResourceKey))
					{
						resourcePaths[r.ResourceKey] = r.ResourcePath;
					}
					else
					{
						resourcePaths.Add(r.ResourceKey, r.ResourcePath);
					}
					LoadAndMerge(r.ResourcePath);
				}
				currentLanguage = lang;
				pbData.ChangeDefaultSetting("last_lang", currentLanguage);
				pbData.ChangeUserSetting("last_sync", null);
			}
			if (webAPI.CurrentLanguage != currentLanguage)
			{
				webAPI.SetLanguage(currentLanguage);
				ManualResetEvent evDone = new ManualResetEvent(false);
				Task.Factory.StartNew(() =>
				{
					try
					{
						if (!sync.UpdateTranslations())
						{
							logger.Debug("UpdateTranslations failed");
						}
					}
					catch (Exception ex)
					{
						logger.Error(ex.ToString());
					}
					evDone.Set();
				});
				evDone.WaitOne();
			}
			//Updating text resources for tray menu
			SystemTray.SetLanguageForTrayMenuItems();

			//var m = FindWindow<MainWindow>();
			//if (m != null) m.Reload();
			Logout(true);
		}

		void pbData_OnUserLoggedIn(string obj)
		{
			SetDBLockTimeout();
			Task.Factory.StartNew(() =>
			{
				try
				{
					pbData.CheckForDuplicatePasswords();
				}
				catch
				{
				}
			});
		}

		private void SetDBLockTimeout()
		{
			if (pbData == null) return;
			var autoLockMinutesString = pbData.GetPrivateSetting(DefaultProperties.Settings_Security_PasswordAutoLock);
			if (string.IsNullOrWhiteSpace(autoLockMinutesString))
			{
				pbData.PasswordTimeout = TimeSpan.FromMinutes(1440);
				pbData.ChangePrivateSetting(DefaultProperties.Settings_Security_PasswordAutoLock, "1440");
			}
			else
			{
				int timeOut = 0;
				var ok = int.TryParse(autoLockMinutesString, out timeOut);

				if (ok)
				{
					pbData.PasswordTimeout = timeOut <= 0 ? TimeSpan.FromMilliseconds(-1) : TimeSpan.FromMinutes(timeOut);
				}
				else
				{
					pbData.PasswordTimeout = TimeSpan.FromMinutes(1440);
					pbData.ChangePrivateSetting(DefaultProperties.Settings_Security_PasswordAutoLock, "1440");
				}
			}
				}

		public bool SetClipboardText(string text)
		{
			if (!String.IsNullOrWhiteSpace(text))
			{
				Clipboard.SetText(text);
				clipboardTimer.Stop();
				clipboardTimer.Start();
				return true;
			}
			return false;
		}

		/*
		 * Switch statement became too complex - changed this to reflection invocation
		 * Implement this signature: <private/public> bool <CommandName>(Dictionary<string, object> parameters)
		 * Current implementations are in #region ExecuteCommand - methods
		 */
		public bool ExecuteCommand(string name, Dictionary<string, object> parameters)
		{
			if (string.IsNullOrWhiteSpace(name)) return false;
			MethodInfo m = null;
			lock (cmdCacheLock)
			{
				if (!cmdCache.TryGetValue(name, out m))
				{
					m = typeof(PBApp).GetMethod(name,
						BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
						new Type[] { typeof(Dictionary<string, object>) }, null);
					cmdCache.Add(name, m);
				}
			}
			if (m == null)
			{
				logger.Debug("Command '{0}' not found", name);
				return false;
			}
			try
			{
				return (bool)m.Invoke(this, new object[] { parameters });
			}
			catch (Exception exc)
			{
				logger.Error("ExecuteCommand: {0} - {1}", name, exc.ToString());
			}
			return false;
		}

		public bool ShowProductTour()
		{
			LoginWindow wLogin = FindWindow<LoginWindow>();

			if (wLogin != null && !this.pbData.AnyAccountExists())
			{
				SystemTray.StartWindow(wLogin, true, WindowMode.ProductTour);

				return true;
			}

			return false;
		}

		#region ExecuteCommand - methods (reflection)

		private  PerformSyncDialog performSyncDialog;

		private bool BackupNow(Dictionary<string, object> parameters)
		{
			
			if (PerformSyncDialog.IsInSync)
			{
				logger.Debug("BackupNow - already in sync. quit");
				return true;
			}

			bool isSilent = false;
			if (parameters != null)
			{
				var key  = "isSilent";
				if (parameters.ContainsKey(key))
				{
					isSilent = (bool)parameters[key];
				}
			}

			logger.Debug("BackupNow - ready to go. IsSilent = {0}", isSilent);

			if (isSilent)
			{
				Task.Factory.StartNew(() =>
				{
					GetResolver().GetInstanceOf<IPBSync>().Sync();
				});
			}
			else
			{

				Current.Dispatcher.Invoke((Action)(() =>
					{
						performSyncDialog = new PerformSyncDialog(GetResolver());
						performSyncDialog.Show();

					}));
			}

			return true;
		}

		private bool PerformSync(Dictionary<string, object> parameters)
		{
			// rework for sync.
			CheckForUpdatesWindow checkingForUpdatesWindow = new CheckForUpdatesWindow();
			checkingForUpdatesWindow.Show();
			Task.Factory.StartNew(() =>
			{
				try
				{
					bool updateAvailable = updater.CheckForUpdatesAndNotify();

					Current.Dispatcher.BeginInvoke((Action)(() =>
					{
						checkingForUpdatesWindow.Close();

						if (!updateAvailable)
						{
							NoUpdateAvailable();
						}
					}));
				}
				catch (Exception e)
				{
					logger.Error(e.Message);
				}
			});
			return true;
		}

		private bool AreDialogsOpened(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			var _isOpen = wMain.dialogs.FirstOrDefault(x => x.IsShown);
			if (_isOpen == null) return false;
			return true;
		}

		private bool AreOtherWindowsOpen(Dictionary<string, object> parameters)
		{
			try
			{
				MainWindow wMain = FindWindow<MainWindow>();
				if (wMain == null) return false;
				int numberOfChildWindows = wMain.OwnedWindows.Count;
				if (numberOfChildWindows > 0)
				{

					return wMain.OwnedWindows.OfType<Window>().Where(x => x.IsVisible).Any();
				}
				return false;
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
				return false;
			}

		}

        private bool checkForUpdatesClicked = false;

		private bool CheckForUpdates(Dictionary<string, object> parameters)
		{
            checkForUpdatesClicked = true;
			CheckForUpdatesWindow checkingForUpdatesWindow = new CheckForUpdatesWindow();
			checkingForUpdatesWindow.Show();
			Task.Factory.StartNew(() =>
			{
				try
				{
					bool updateAvailable = updater.CheckForUpdatesAndNotify();

					Current.Dispatcher.BeginInvoke((Action)(() =>
					{
						checkingForUpdatesWindow.Close();

						if (!updateAvailable)
						{
							NoUpdateAvailable();
						}
					}));
				}
				catch (Exception e)
				{
					logger.Error(e.Message);
				}
			});
			return true;
		}

		private void NoUpdateAvailable()
		{
            checkForUpdatesClicked = false;
			NoUpdateAvailable noUpdatesWindow = new NoUpdateAvailable();
			noUpdatesWindow.Owner = Current.MainWindow;
			noUpdatesWindow.ShowDialog();
		}

		private void UpdatedVersionDetected(AppVersion obj)
		{
            if (checkForUpdatesClicked
                || (!checkForUpdatesClicked && !pbData.UpdateTonightScheduled()))
            {
                checkForUpdatesClicked = false;
			Current.Dispatcher.BeginInvoke((Action)(() =>
			{
				lock (UpdateAvailableViewModel.UpdateLocker)
				{
					if (!UpdateAvailable.IsShown)
					{
						AppVersion version = null;
						bool hasUpdate = updater.ServerHasUpdates(out version);
						var currentVersion = AppVersion.GetInstalledVersion();
						if (currentVersion.Rank > version.Rank)
						{
							logger.Debug("app already updated");
							return;
						}
						ShowUpdateAvailableDialog();
					}
				}
			}));
		}
		}

		private void ShowUpdateAvailableDialog()
		{
			var version = AppVersion.GetInstalledVersion();
			UpdateAvailableViewModel dc = new UpdateAvailableViewModel(pbData);

			UpdateAvailable win = new UpdateAvailable();
			win.Owner = Current.MainWindow;
			dc.HeaderText = Current.Resources["InstallationIsOutdated"].ToString();
			dc.BoldBodyText = Current.Resources["InstallationIsOutdated_UpdateNowBold"].ToString();
			dc.RegularBodyText = Current.Resources["InstallationIsOutdated_UpdateNow"].ToString();
			win.DataContext = dc;
			dc.UpdateNowTriggered += (o, e) =>
			{
				if (!updater.RunUpdate())
					MessageBox.Show("Failed to update", "Password Boss updater", MessageBoxButton.OK);
			};
			win.ShowDialog();
		}

		private bool RefreshSecurityScore(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.RefreshSubcomponents();
		}

        

		private bool ShowSecureItemEditor(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.OpenView("ShowSecureItemEditor", parameters);
		}

		private bool ShowShareItemInShareCenter(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.OpenView("ShowShareItemInShareCenter", parameters);
		}

        private bool MainSettings(Dictionary<string, object> parameters)
        {
            MainWindow wMain = FindWindow<MainWindow>();
            if (wMain == null) return false;
            return wMain.OpenView("MainSettings");
        }


        private bool AddNewItemView(Dictionary<string, object> parameters)
        {
            MainWindow wMain = FindWindow<MainWindow>();
            if (wMain == null) return false;
            return wMain.OpenView("AddNewItemView");
        }


		private bool OpenUrlInSecureBrowser(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.Activate();
			wMain.MenuSetFocus();
			return wMain.OpenView("OpenUrlInSecureBrowser", parameters);
		}

		private bool OpenSecureBrowser(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.OpenView("OpenSecureBrowser");
		}

        private bool OpenIdentities(Dictionary<string, object> parameters)
        {
            MainWindow wMain = FindWindow<MainWindow>();
            if (wMain == null) return false;
            return wMain.OpenView("Identities");
        }

        private bool ShowMainWindow(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.Show();
			return true;
		}

		private bool StopShowingDialogs(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.StopShowingDialogs = true;
			return true; ;
		}

		private bool GetPremium(Dictionary<string, object> parameters)
		{
			try
			{
				Application.Current.Dispatcher.BeginInvoke((Action)(() =>
				{
					try
					{
						string uri = pbData.GetPrivateSetting(DefaultProperties.Configuration_Purchase_Url_InApp);
						Dictionary<string, object> param = new Dictionary<string, object>();
						param.Add("url", uri);
						((IAppCommand)System.Windows.Application.Current).ExecuteCommand("OpenUrlInSecureBrowser", param);
					}
					catch (Exception ex)
					{
						logger.Error(ex.ToString());
					}
				}));
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
			return false;
		}

		private bool BuySB(Dictionary<string, object> parameters)
		{
			return GetPremium(parameters);
		}

		private bool ShowMenuExpander(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.OpenMenuExpander(parameters);
		}

		private bool AccountCreation(Dictionary<string, object> parameters)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				var window = FindWindow<LoginWindow>();
			

				window.NavigateloginScreens(LoginWindow.ScreenNames.AccountCreation);
			}));
			return true;
		}

		private bool BuyDB(Dictionary<string, object> parameters)
		{
			Configuration config = pbData.GetConfigurationByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, DefaultProperties.Configuration_Purchase_Url);
			if (config != null)
			{
				BrowserHelper.OpenInDefaultBrowser(new Uri(config.Value));
			}
			return true;
		}

		private bool ShowLoginWindow(Dictionary<string, object> parameters)
		{
			LoginWindow wLogin = FindWindow<LoginWindow>();

			if (wLogin == null)
			{
				return false;
			}

			bool openMainUI = true;

			if (parameters != null && parameters.ContainsKey("openMainUI"))
			{
				openMainUI = (bool)parameters["openMainUI"];
			}

			if (openMainUI)
			{
				wLogin.Show();
			}
			else
			{
				if (!pbData.AnyAccountExists())
				{
					wLogin.NavigateloginScreens("ShowProductTour");
				}
				else
				{
					wLogin.NavigateloginScreens();
				}
			}

			wLogin.Focus();

			return true;
		}

		private bool ClearEmailOnLoginWindow(Dictionary<string, object> parameters)
		{
			LoginWindow wLogin = FindWindow<LoginWindow>();
			if (wLogin == null || wLogin.Login == null) return false;
			var dc = wLogin.Login.DataContext as LoginViewModel;
			if (dc == null) return false;
			dc.UserEmail = string.Empty;
			Dispatcher.BeginInvoke(DispatcherPriority.Input,
				new Action(delegate()
				{
					wLogin.Login.EmailTextBox.Focus();         // Set Logical Focus
					Keyboard.Focus(wLogin.Login.EmailTextBox); // Set Keyboard Focus
				}));
			return true;
		}

		private bool LocalBackup(Dictionary<string, object> parameters)
		{
			return LocalBackup();
		}

		private bool LocalRestore(Dictionary<string, object> parameters)
		{
			return LocalRestore();
		}

		private bool ReloadData(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.Reload(parameters);
			return false;
		}

		private bool UpdateAlertNotificationCount(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.UpdateAlertNotificationCount();
			return false;
		}

		private bool UpdateAlertMessagesCount(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			wMain.UpdateAlertMessagesCount();
			return false;
		}

		private bool OpenFeatureNotEnabledPopUp(Dictionary<string, object> parameters)
		{
			MainWindow parent = FindWindow<MainWindow>();
			FeatureNotEnabledPopUp popUp = new FeatureNotEnabledPopUp(parent);
			popUp.ShowDialog();
			return false;
		}

		private bool Logout(Dictionary<string, object> parameters)
		{
			Logout();
			return false;
		}

		private bool OpenMasterPasswordDialog(Dictionary<string, object> parameters)
		{
			MainWindow wMain = FindWindow<MainWindow>();
			if (wMain == null) return false;
			return wMain.ShowMasterPasswordDialog(parameters) ?? false;
		}

		private bool ShowDialogForClosingBrowser(Dictionary<string, object> parameters)
		{
			string _headerParameter = string.Empty;
			string _headerText = string.Empty;
			string _bodyText = System.Windows.Application.Current.FindResource("BrowserWillCloseBodyText") as string;
			if (parameters != null && parameters.ContainsKey("BrowserTitle"))
				_headerParameter = (string)parameters["BrowserTitle"];
			if (!string.IsNullOrWhiteSpace(_headerParameter))
			{
				switch (_headerParameter)
				{
					case "Ie":
						_headerText = System.Windows.Application.Current.FindResource("IEWillClose") as string;
						break;
					case "Firefox":
						_headerText = System.Windows.Application.Current.FindResource("FirefoxWillClose") as string;
						break;
					case "Chrome":
						_headerText = System.Windows.Application.Current.FindResource("ChromeWillClose") as string;
						break;
					default:
						break;

				}
			}
			CloseBrowserInformationDialog _dialog = new CloseBrowserInformationDialog(_headerText, _bodyText);
			bool? result = _dialog.ShowDialog();
			if (result.HasValue && result.Value)
			{
				return true;
			}
			return false;
		}

		#endregion

		private bool LocalBackup()
		{
			SaveFileDialog dlgSave = new SaveFileDialog();
			dlgSave.OverwritePrompt = true;
			dlgSave.Title = (string)Resources["LocalBackupDialogTitle"];
			dlgSave.AddExtension = true;
			dlgSave.DefaultExt = "zip";
			dlgSave.Filter = string.Format("{0}|*.zip", Resources["ZipFiles"]);
			dlgSave.FilterIndex = 0;
			dlgSave.FileName = string.Format("PasswordBoss_backup_{0:yyyyMMddHHmmss}.zip", DateTime.Now);
			dlgSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var ret = dlgSave.ShowDialog(FindWindow<MainWindow>());
			if (ret.HasValue && ret.Value)
			{
				var result = pbData.ExportStoreToFile(dlgSave.FileName, new List<string>(
					new string[] { "secure_item_type","folder","site","site_image_size",
                        "site_image","site_uri","secure_item","secure_item_stats","secure_item_data" }
					));
				string _dialogText = string.Empty;
				if (result)
				{
					_dialogText = System.Windows.Application.Current.FindResource("ExportSuccessful") as string;
				}
				else
				{
					_dialogText = System.Windows.Application.Current.FindResource("ExportFailed") as string;
				}
				CustomInformationMessageBoxWindow window = new CustomInformationMessageBoxWindow(_dialogText);
				window.ShowDialog();
				return result;
			}
			return false;
		}

		private bool LocalRestore()
		{
			OpenFileDialog dlgOpen = new OpenFileDialog();
			dlgOpen.CheckFileExists = true;
			dlgOpen.Title = (string)Resources["LocalRestoreDialogTitle"];
			dlgOpen.AddExtension = true;
			dlgOpen.DefaultExt = "zip";
			dlgOpen.Filter = string.Format("{0}|*.zip", Resources["ZipFiles"]);
			dlgOpen.FilterIndex = 0;
			dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var mw = FindWindow<MainWindow>();
			var ret = dlgOpen.ShowDialog(mw);
			if (ret.HasValue && ret.Value)
			{
#warning Zamijeniti sa standardnim dijalogom, kada bude zavrsen
				MessageBoxResult userSelection = MessageBox.Show("Overwrite existing data ?", "Confirm action", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
				if (userSelection == MessageBoxResult.Cancel) return false;
				var result = pbData.ImportStoreFromFile(dlgOpen.FileName, userSelection == MessageBoxResult.Yes, (creds) =>
				{
					Application.Current.Dispatcher.Invoke(new Action(() =>
					{
						MasterPasswordDialog masterDialog = new MasterPasswordDialog(pbData);
						masterDialog.SkipPwdValidation = true;
						//MasterPwdBox masterDialog = new MasterPwdBox();
						bool? res = masterDialog.ShowDialog();
						if (res.HasValue && res.Value)
						{
							creds.Password = masterDialog.Pwd;
							//creds.eMail = TODO - Add option to change email for imports from different account
							return;
						}
						creds.Cancel = true;
					}));
				});
#warning Zamijeniti sa standardnim dijalogom, kada bude zavrsen
				MessageBox.Show(result ? "Success" : "Failure");
				if (result) mw.Reload();
				return result;
			}
			return false;
		}

		private bool CheckInstallation()
		{
			bool result = true;
			//load installation uuid from registry
			RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\PasswordBoss");
			if (rk == null)
				result = false;
			else
			{
				pbData.InstallationUUID = (string)rk.GetValue("UUID");
				if (String.IsNullOrEmpty(pbData.InstallationUUID))
					result = false;
				rk.Close();
			}

			if (!result)
			{
				MessageBox.Show("Installation error!");
			}
			return result;
		}

		private void LoadAndMerge(Uri dictionaryUri)
		{
			try
			{
				var dictionary = new ResourceDictionary();
				dictionary.Source = dictionaryUri;
				var cat = new HashSet<string>(dictionary.Keys.OfType<string>().Where(k => k.StartsWith("Category", StringComparison.InvariantCulture)).ToArray());
				categories.Clear();
				foreach (var c in cat)
				{
					if (!categories.ContainsKey(c))
					{
						categories.Add(c, dictionary[c] as string);
					}
					dictionary.Remove(c);
				}
				Application.Current.Resources.MergedDictionaries.Add(dictionary);
			}
			catch (Exception ex)
			{
				logger.Error(ex.ToString());
			}

		}

		internal static String ReturnResourceString(string key, string dictionary)
		{
			var resourceDictionary = new ResourceDictionary();
			resourceDictionary.Source = new Uri(dictionary, UriKind.RelativeOrAbsolute);

			if (resourceDictionary.Contains(key))
			{
				var value = resourceDictionary[key];
				return value.ToString();
			}
			return null;
		}

		internal static String ReturnResourceString(string key)
		{
			if (Application.Current.Resources.Contains(key))
			{
				return (string)Application.Current.Resources[key];
			}
			return null;
		}

		public void Logout(bool activate = false)
		{
			SystemTray.Logout(activate);
		}

		public void Quit()
		{
			sysTray.Exit();
		}

		public void ShowUI()
		{
			if (!CheckInstallation())
			{
				return;
			}

			if (String.IsNullOrEmpty(pbData.ActiveUser))
			{
				LoginWindow wLogin = FindWindow<LoginWindow>();

				if (wLogin == null)
				{
					wLogin = new LoginWindow(moduleLoader);
				}

				Navigator.NavigationService = wLogin.MainFrame.NavigationService;

				if (!this.pbData.AnyAccountExists())
				{
					wLogin.NavigateloginScreens("ShowProductTour");
				}
				else
				{
					wLogin.Show();
				}
			}
			else
			{
				MainWindow mainWindow = FindWindow<MainWindow>();

				if (mainWindow == null)
				{
					mainWindow = new MainWindow(moduleLoader);
				}

				mainWindow.Show();
			}
		}

		public void Restore(Window w)
		{
			if (w == null) return;
			if (w.WindowState == System.Windows.WindowState.Maximized)
				w.WindowState = System.Windows.WindowState.Normal;
			else
				w.WindowState = System.Windows.WindowState.Maximized;
		}

		public T FindWindow<T>()
		{
			T ret = default(T);
			foreach (var w in Application.Current.Windows)
			{
				if (w.GetType() == typeof(T))
				{
					ret = (T)w;
					break;
				}
			}
			return ret;
		}

		//Test - not finished
		protected override void OnStartup(StartupEventArgs e)
		{
			//Popup placement fix for tablet right handed and left handed users.
			var popupPlacement = SystemParameters.MenuDropAlignment;
			if (popupPlacement)
			{
				var _parameters = typeof(SystemParameters);
				var field = _parameters.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
				field.SetValue(null, false);

				popupPlacement = SystemParameters.MenuDropAlignment;
			}

			//Select all text in textboxes with double click
			EventManager.RegisterClassHandler(typeof(TextBox),
				TextBox.MouseDoubleClickEvent,
				new RoutedEventHandler(SelectAllText));

			//Date-time watermarks
			EventManager.RegisterClassHandler(typeof(DatePicker),
				DatePicker.LoadedEvent,
				new RoutedEventHandler(DatePicker_Loaded));

			//To-do: Date format for localization
			//      FrameworkElement.LanguageProperty.OverrideMetadata(
			//typeof(FrameworkElement),
			//new FrameworkPropertyMetadata(
			//    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
			//      base.OnStartup(e);
		}

		void SelectAllText(object sender, RoutedEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox != null)
				textBox.SelectAll();
		}

		public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj == null) return null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);

				var result = (child as T) ?? GetChildOfType<T>(child);
				if (result != null) return result;
			}
			return null;
		}

		void DatePicker_Loaded(object sender, RoutedEventArgs e)
		{
			var datePicker = sender as DatePicker;
			if (datePicker == null) return;

			var dateTextBox = GetChildOfType<DatePickerTextBox>(datePicker);
			if (dateTextBox == null) return;

			var watermark = dateTextBox.Template.FindName("PART_Watermark", dateTextBox) as ContentControl;
			if (watermark == null) return;

			watermark.Content = "MM/DD/YYYY";
		}

		public static bool IsWindowOpen<T>(string name = "") where T : Window
		{
			return string.IsNullOrEmpty(name)
			   ? Application.Current.Windows.OfType<T>().Any()
			   : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
		}

		private void onDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			try
			{
				var exception = e.Exception;

				if (exception != null)
				{
					logger.Error("PBApp.onDispatcherUnhandledException Message: " + exception.Message);
					logger.Error("PBApp.onDispatcherUnhandledException StackTrace: " + exception.StackTrace);
				}
			}
			catch (Exception exc)
			{
				logger.Error(exc.ToString());
			}

			e.Handled = true;
		}
	}
}