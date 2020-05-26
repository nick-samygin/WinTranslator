using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using PasswordBoss.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss
{
	enum WindowMode
	{
		Default,
		ProductTour,
		SignIn,
	}

	class SystemTray
	{
		private static readonly ILogger logger = Logger.GetLogger(typeof(SystemTray));
		private IMessagingDialog dialog;
		private IPBSync sync;

		private static DateTime? LastSyncMessageShown;
		private bool forceMessageShowing;
		private static readonly string globalLockID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
		private static EventWaitHandle evQuit;
		/// <summary>
		/// favicon path
		/// </summary>   
		static string TrayIconPath = Application.Current.FindResource("TrayIcon") as string;
		static Stream TrayIconStream = Application.GetResourceStream(new Uri(TrayIconPath)).Stream;
		/// <summary>
		/// constant string variables
		/// </summary>
		static readonly String AppTitleKey = "PasswordBoss";
		static readonly String ToolTipKey = "PasswordBossIsRunning";
		static readonly String OpenKey = "Open";
		static readonly String ManageDevicesKey = "MenuManageDevices";
		static readonly String CheckUpdatesKey = "CheckForUpdatesMenuItem";
		static readonly String BackupNowKey = "BackupNow";
		static readonly String SettingKey = "AccountSetting";
		static readonly String SupportKey = "MenuGetSupport";

		static readonly String SysInfoKey = "SysInfoTrayMenuItem";
		static readonly String ExitKey = "Exit";

		private static string SupportUrl = DefaultProperties.InAppSupportTrayLink;

		//Added to disable "Account synced" popup if no password is entered on "master password changed window"
		public static bool _correctPasswordEntered;

		#region "Tray Menu objects"
		/// <summary>
		/// Object creation for system tray with menus
		/// </summary>
		/// 
		public System.Windows.Forms.NotifyIcon _trayNotify;

		static readonly System.Windows.Forms.ContextMenuStrip _trayMenu = new System.Windows.Forms.ContextMenuStrip();
		static readonly System.Windows.Forms.ToolStripItem _menuOpen = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuManageDevices = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuBackupNow = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuSetting = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuSupport = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuSysInfo = new System.Windows.Forms.ToolStripMenuItem();
		static readonly System.Windows.Forms.ToolStripItem _menuExit = new System.Windows.Forms.ToolStripMenuItem();

		static readonly MenuItem _menuLockUnlock = new MenuItem();
		static readonly MenuItem _menuLogout = new MenuItem();

		private static IPBData pbData;
		public static event EventHandler OnBeforeShutDownHandler;

		private static IResolver resolver;

		#endregion

		static SystemTray()
		{
			evQuit = new EventWaitHandle(false, EventResetMode.ManualReset, "Global\\PBEvQT_DskApp"/* + globalLockID*/);
		}

		public static void SetResolver(IResolver resolverInstance)
		{
			resolver = resolverInstance;
		}

		public static void SetLanguageForTrayMenuItems()
		{
			_menuOpen.Text = PBApp.ReturnResourceString(OpenKey);
			_menuManageDevices.Text = PBApp.ReturnResourceString(ManageDevicesKey);
			_menuCheckUpdates.Text = PBApp.ReturnResourceString(CheckUpdatesKey);
			_menuBackupNow.Text = PBApp.ReturnResourceString(BackupNowKey);
			_menuSetting.Text = PBApp.ReturnResourceString(SettingKey);
			_menuSupport.Text = PBApp.ReturnResourceString(SupportKey);
			_menuSysInfo.Text = PBApp.ReturnResourceString(SysInfoKey);
			_menuExit.Text = PBApp.ReturnResourceString(ExitKey);
		}

		/// <summary>
		/// Sets the system tray icon with menus and events binded
		/// </summary>
		public void InitializeTrayProperties()
		{
			sync = resolver.GetInstanceOf<IPBSync>();
			if (sync != null)
			{
				sync.OnSyncSuccess += syncSuccessed;
			}
			forceMessageShowing = true;

			if (_trayNotify != null) return;
			//Set up the system tray icon
			_trayNotify = new System.Windows.Forms.NotifyIcon
			{
				//BalloonTipTitle = PBApp.ReturnResourceString(AppTitleKey),
				//BalloonTipText = PBApp.ReturnResourceString(ToolTipKey),
				Text = PBApp.ReturnResourceString(AppTitleKey)
			};

			_trayNotify.MouseClick += _trayNotify_MouseClick;
			_trayNotify.MouseDoubleClick += _trayNotify_MouseDoubleClick;

			_trayMenu.Renderer = new MyRenderer();
			_trayMenu.Opening += _trayMenu_Opening;

			_menuOpen.MouseEnter += item_MouseEnter;
			_menuOpen.MouseLeave += item_MouseLeave;
			_menuOpen.Click += _menuOpen_Click;
			_menuOpen.BackColor = System.Drawing.Color.White;

			_trayMenu.Items.Add(_menuOpen);

			_trayMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());

			_menuManageDevices.MouseEnter += item_MouseEnter;
			_menuManageDevices.MouseLeave += item_MouseLeave;
			_menuManageDevices.Click += _menuManageDevices_Click;
			_menuManageDevices.BackColor = System.Drawing.Color.White;

			_trayMenu.Items.Add(_menuManageDevices);

			_menuCheckUpdates.MouseEnter += item_MouseEnter;
			_menuCheckUpdates.MouseLeave += item_MouseLeave;
			_menuCheckUpdates.Click += _menuCheckUpdates_Click;
			_menuCheckUpdates.BackColor = System.Drawing.Color.White;

			_trayMenu.Items.Add(_menuCheckUpdates);

			_menuBackupNow.MouseEnter += item_MouseEnter;
			_menuBackupNow.MouseLeave += item_MouseLeave;
			_menuBackupNow.Click += _menuBackupNow_Click;
			_menuBackupNow.BackColor = System.Drawing.Color.White;

			_trayMenu.Items.Add(_menuBackupNow);

			_trayMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());

			_menuSetting.MouseEnter += item_MouseEnter;
			_menuSetting.MouseLeave += item_MouseLeave;
			_menuSetting.Click += _menuSetting_Click;
			_menuSetting.BackColor = System.Drawing.Color.White;

			_trayMenu.Items.Add(_menuSetting);

			_menuSupport.MouseEnter += item_MouseEnter;
			_menuSupport.MouseLeave += item_MouseLeave;
			_menuSupport.Click += _menuSupport_Click;
			_menuSupport.BackColor = System.Drawing.Color.White;
			_trayMenu.Items.Add(_menuSupport);

			_menuSysInfo.MouseEnter += item_MouseEnter;
			_menuSysInfo.MouseLeave += item_MouseLeave;
			_menuSysInfo.Click += _menuSysInfo_Click;
			_menuSysInfo.BackColor = System.Drawing.Color.White;
			_trayMenu.Items.Add(_menuSysInfo);

#if DEBUG
			_trayMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());

			_menuExit.MouseEnter += item_MouseEnter;
			_menuExit.MouseLeave += item_MouseLeave;
			_menuExit.Click += _menuExit_Click;
			_menuExit.BackColor = System.Drawing.Color.White;
			_trayMenu.Items.Add(_menuExit);

#endif
			_trayNotify.ContextMenuStrip = _trayMenu;

			try
			{
				_trayNotify.Icon = new System.Drawing.Icon(TrayIconStream);
				if (_trayNotify != null)
				{
					// Show the icon
					_trayNotify.Visible = true;

					// Show the balloon tip
					// _trayNotify.ShowBalloonTip(2000);
				}
			}

			catch (Exception ex)
			{
				MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
				logger.Error(ex.Message);
			}

		}

		void _trayNotify_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_trayNotify_MouseClick(sender, e);
		}

		void _trayNotify_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button != System.Windows.Forms.MouseButtons.Right)
			{
				try
				{
					if (Application.Current.Windows.Count < 1)
					{
						((PBApp)Application.Current).ShowUI();
					}
					Window window = null;
					if (PBData == null || string.IsNullOrWhiteSpace(PBData.ActiveUser))
					{
						window = ((PBApp)Application.Current).FindWindow<LoginWindow>();
						if (window == null) window = Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
					}
					else
					{
						window = ((PBApp)Application.Current).FindWindow<MainWindow>();
						if (PBData.Locked)
						{
							Logout();
							return;
						}
					}

					if (!pbData.AnyAccountExists())
					{
						StartWindow(window, true, WindowMode.ProductTour);
					}
					else
					{
						StartWindow(window, true);
					}
				}
				catch (Exception ex)
				{
					logger.Error(ex.Message);
				}

			}
		}

		void _menuCheckUpdates_Click(object sender, EventArgs e)
		{
			((IAppCommand)System.Windows.Application.Current).ExecuteCommand("CheckForUpdates", null);
		}

		#region Menu item events

		void _menuOpen_Click(object sender, EventArgs e)
		{
			try
			{
				if (Application.Current.Windows.Count < 1)
				{
					((PBApp)Application.Current).ShowUI();
				}
				Window window = null;
				if (PBData == null || string.IsNullOrWhiteSpace(PBData.ActiveUser))
				{
					window = ((PBApp)Application.Current).FindWindow<LoginWindow>();
					if (window == null) window = Application.Current.Windows.Count > 0 ? Application.Current.Windows[0] : null;
				}
				else
				{
					window = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (PBData.Locked)
					{
						Logout();
						return;
					}
				}

				if (!pbData.AnyAccountExists())
				{
					StartWindow(window, true, WindowMode.ProductTour);
				}
				else
				{
					StartWindow(window, true);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		void _menuManageDevices_Click(object sender, EventArgs e)
		{
			try
			{
				if (PBData == null || string.IsNullOrWhiteSpace(PBData.ActiveUser))
				{
					LoginWindow window = ((PBApp)Application.Current).FindWindow<LoginWindow>();
					StartWindow(window, true);
				}
				else
				{
					MainWindow mw = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (mw != null)
					{
						_menuOpen_Click(sender, e);
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						dictionary.Add("SelectedTabIndex", 3);
						mw.OpenView("MainSettings", dictionary);

					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
				logger.Error(ex.Message);
			}
		}

		void _menuBackupNow_Click(object sender, EventArgs e)
		{
			forceMessageShowing = true;
			((IAppCommand)System.Windows.Application.Current).ExecuteCommand("BackupNow", null);
		}

		void _menuSetting_Click(object sender, EventArgs e)
		{
			try
			{
				if (PBData == null || string.IsNullOrWhiteSpace(PBData.ActiveUser))
				{
					LoginWindow window = ((PBApp)Application.Current).FindWindow<LoginWindow>();
					StartWindow(window, true);
				}
				else
				{
					MainWindow mw = ((PBApp)Application.Current).FindWindow<MainWindow>();
					if (mw != null)
					{
						_menuOpen_Click(sender, e);
						mw.OpenView("MainSettings");
					}
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		void _menuSupport_Click(object sender, EventArgs e)
		{
			try
			{
				BrowserHelper.OpenInDefaultBrowser(new Uri(SupportUrl));
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		void _menuSysInfo_Click(object sender, EventArgs e)
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\plugins", ""), "PBSysInfo.exe");
					Process.Start(path);
				}
				catch (Exception exc)
				{
					logger.Error(exc.ToString());
				}
			});
		}

		void _menuExit_Click(object sender, EventArgs e)
		{
			try
			{
				evQuit.Set();
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		#endregion

		private static IPBData PBData
		{
			get
			{
				if (resolver != null && pbData == null)
				{
					pbData = resolver.GetInstanceOf<IPBData>();
				}
				return pbData;
			}
		}

		void _trayMenu_Opening(object sender, CancelEventArgs e)
		{
			CheckMenuItems();
		}

		void item_MouseLeave(object sender, EventArgs e)
		{
			System.Windows.Forms.ToolStripMenuItem TSMI = sender as System.Windows.Forms.ToolStripMenuItem;
			TSMI.ForeColor = System.Drawing.Color.Black;
		}

		void item_MouseEnter(object sender, EventArgs e)
		{
			System.Windows.Forms.ToolStripMenuItem TSMI = sender as System.Windows.Forms.ToolStripMenuItem;
			TSMI.ForeColor = System.Drawing.Color.White;
		}

		public void CheckMenuItems()
		{
			bool _areOtherWindowsOpened = ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreOtherWindowsOpen", null);
			if (_areOtherWindowsOpened)
			{
				_menuSetting.Enabled = false;
				_menuManageDevices.Enabled = false;
				_menuBackupNow.Enabled = false;
				_menuCheckUpdates.Enabled = false;
			}
			else if (PBData == null || string.IsNullOrWhiteSpace(PBData.ActiveUser))
			{
				_menuSetting.Enabled = false;
				_menuManageDevices.Enabled = false;
				_menuBackupNow.Enabled = false;
				_menuCheckUpdates.Enabled = true;
			}
			else
			{
				_menuSetting.Enabled = true;
				_menuManageDevices.Enabled = true;
				_menuBackupNow.Enabled = true;
				_menuCheckUpdates.Enabled = true;
			}

		}

		public static void Logout(bool activate = false)
		{
			if (PBData != null)
			{
				if (!string.IsNullOrWhiteSpace(PBData.ActiveUser))
				{
					PBData.CloseProfile();
				}
			}

			try
			{
				if (PBData != null)
				{
					Configuration currentLoginAccount = PBData.GetConfigurationByAccountAndKey(DefaultProperties.Configuration_DefaultAccount, DefaultProperties.Configuration_Key_LastLoginEmail);

					if (currentLoginAccount == null)
					{
						((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ClearEmailOnLoginWindow", null);
					}
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex.ToString());
			}

			MainWindow mw = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (mw != null) CloseWindow(mw);
			LoginWindow lw = ((PBApp)Application.Current).FindWindow<LoginWindow>();
			System.Windows.Application.Current.MainWindow = lw;
			StartWindow(lw, activate, WindowMode.SignIn);
		}

		/// <summary>
		/// Returns specific Window as return type based on window title consume as parameter
		/// </summary>
		/// <param name="windowTitleString"></param>
		/// <returns></returns>
		internal Window CurrentWindow(string windowTitleString)
		{
			switch (windowTitleString)
			{
				case "LoginWindow":
					LoginWindow window = ((PBApp)Application.Current).FindWindow<LoginWindow>();
					return window;
				case "ForgotMasterPasswordWindow":
					ForgotMasterPassword forgotMPW = ((PBApp)Application.Current).FindWindow<ForgotMasterPassword>();
					return forgotMPW;

				default:
					return null;
			}
		}

		/// <summary>
		/// On close button click minimize the app with task bar visibility hidden
		/// </summary>
		/// <param name="window"></param>
		internal void WindowClose(Window window)
		{
			CloseWindow(window);
		}

		private static void CloseWindow(Window window)
		{
			if (window != null)
			{
				int numberOfChildWindows = window.OwnedWindows.Count;
				if (numberOfChildWindows > 0)
				{
					Window[] childwindows = new Window[numberOfChildWindows];
					for (int count = 0; count < numberOfChildWindows; count++)
					{
						childwindows[count] = window.OwnedWindows[count];
					}
					foreach (Window aChildWindow in childwindows)
					{
						aChildWindow.Hide();
					}
				}

				window.Hide();
			}
		}

		private static void ShowWindow(Window window, WindowMode mode)
		{
			if (window is LoginWindow)
			{
				switch (mode)
				{
					case WindowMode.ProductTour:
						{
							(window as LoginWindow).NavigateloginScreens("ShowProductTour");
							break;
						}

					case WindowMode.SignIn:
						{
							(window as LoginWindow).NavigateloginScreens();
							break;
						}

					default:
						{
							window.Show();
							break;
						}
				}
			}
			else
			{
				window.Show();
			}
		}

		public static void StartWindow(Window window, bool activate = false, WindowMode mode = WindowMode.Default)
		{
			if (window != null)
			{
				if (window.WindowState == WindowState.Minimized)
				{
					window.WindowState = WindowState.Normal;
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

					ShowWindow(window, mode);
				}
				else
				{
					window.ShowActivated = window.WindowState == WindowState.Maximized;
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

					ShowWindow(window, mode);
				}

				System.Windows.Application.Current.MainWindow = window;

				if (activate)
				{
					window.Activate();

					SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("User32.dll")]
		static extern bool SetForegroundWindow(IntPtr hwnd);

		public void Exit()
		{
			Task.Factory.StartNew(() =>
			{
				try
				{
					RaiseOnBeforeShutDownEvent();
				}
				catch
				{
				}
			}).Wait(2000);
			_trayNotify.Visible = false;
			_trayNotify.Dispose();
			evQuit.Set();
		}

		private static void RaiseOnBeforeShutDownEvent()
		{
			var handler = OnBeforeShutDownHandler;
			if (handler != null)
				handler(typeof(SystemTray), EventArgs.Empty);
		}

		public void syncSuccessed()
		{
			if (forceMessageShowing == true || (LastSyncMessageShown.HasValue && (DateTime.Now - LastSyncMessageShown.Value).TotalMinutes > 90))
			{
				try
				{
					bool tmp = false;

					// To-do handle exception !!!
					// Boolean.TryParse(pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_DisableStatusMessages), out tmp);
					if (tmp)
					{
						return;
					}

					if (dialog == null)
					{
						dialog = resolver.GetInstanceOf<IMessagingDialog>();
						if (dialog == null)
						{
							logger.Error("Failed to obtain reference to MessagingDialog");
							return;
						}
					}

					Task.Factory.StartNew(() =>
					{
						InAppMessage msg = new InAppMessage() { MessageID = "Toast-XS", MessageType = "Toast-XS", Theme = "Toast_XS", Body = System.Windows.Application.Current.FindResource("AccountSynced").ToString() };
						dialog.ShowSystemMessageDialog(msg);
					});

				}
				catch (Exception exc)
				{
					logger.Error(exc.ToString());
				}

				LastSyncMessageShown = DateTime.Now;
				forceMessageShowing = false;
			}
			_correctPasswordEntered = true;
		}
	}

	// Class for changing design of menu items
	public class MyRenderer : System.Windows.Forms.ToolStripProfessionalRenderer
	{
		protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
		{
			if (!e.Item.Selected) base.OnRenderMenuItemBackground(e);
			else
			{

				string hex = "#00D1a7";
				Color c = System.Drawing.ColorTranslator.FromHtml(hex);
				SolidBrush s = new SolidBrush(c);
				Pen p = new Pen(c);
				Rectangle rc = new Rectangle(System.Drawing.Point.Empty, e.Item.Size);
				e.Graphics.FillRectangle(s, rc);
				e.Graphics.DrawRectangle(p, 1, 0, rc.Width, rc.Height);
			}
		}
	}
}