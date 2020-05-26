using PasswordBoss.Helpers;
using PasswordBoss.ViewModel.Account;
using PasswordBoss.Views.InAppAdvertising;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PasswordBoss.Views.Login
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	[Export]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public partial class Login
	{
		private PasswordBox GlobalPasswordTextBox;

		/// <summary>
		/// Constant variable initialization
		/// </summary>
		private const string CreateAccountPath = "..\\Views\\Login\\CreateAccount.xaml";
		SystemTray _systemTray = new SystemTray();
		Common _commonObj = new Common();

		private IPBData pbData = null;
		private IResolver resolver;

		/// <summary>
		/// Initialize components & call to LoadDll Method
		/// </summary>
		[ImportingConstructor]
		public Login([Import(typeof(IResolver))]IResolver resolver)
		{

			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			InitializeComponent();
			var advertControl = new LoginWindowAdvertising(resolver);
			inAppAdvertisingGrid.Children.Add(advertControl);
			DataContext = new LoginViewModel(resolver, inAppAdvertisingGrid);
			//PasswordContentControl.ApplyTemplate();
		}

		public bool OpenMainUI
		{
			set
			{
				if (DataContext != null)
				{
					((LoginViewModel)DataContext).OpenMainUI = value;
				}
			}
		}

		/// <summary>
		/// navigate to create free account screen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>        
		private void HyperlinkSignUp_Click(object sender, RoutedEventArgs e)
		{
			var navService = NavigationService.GetNavigationService(this);

			if (navService != null)
			{
				CreateAccount createAccount = new CreateAccount(resolver, EmailTextBox.Text);
				navService.Navigate(createAccount);
			}
		}

		/// <summary>
		/// returns true if tab key is pressed
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool IsTabKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Tab)
			{
				e.Handled = true;
				return true;
			}

			return false;
		}

		/// <summary>
		/// returns true if shift key is down
		/// </summary>
		/// <returns></returns>
		private bool IsShiftKeyDown()
		{
			return (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
		}

		/// <summary>
		/// used for handling key down event for focusing elements in serial
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Elements_KeyDown(object sender, KeyEventArgs e)
		{
			if (sender is TextBox)
			{
				if (IsTabKeyDown(e))
				{
					if (IsShiftKeyDown())
						BtnClose.Focus();
					else
						if (GlobalPasswordTextBox != null)
						{
							GlobalPasswordTextBox.Focus();
						}
				}
			}
			else if (sender is PasswordBox)
			{
				if (IsTabKeyDown(e))
				{
					if (IsShiftKeyDown())
						EmailTextBox.Focus();
					else
						SignInButton.Focus();
				}
			}
			else if (sender is Button)
			{
				var button = sender as Button;
				if (button.Name == "SignInButton")
				{
					if (IsTabKeyDown(e))
					{
						if (IsShiftKeyDown())
							if (GlobalPasswordTextBox != null)
							{
								GlobalPasswordTextBox.Focus();
							}
							else
								hyperlinkSignUp.Focus();
					}
				}
				else
				{
					if (IsTabKeyDown(e))
					{
						if (IsShiftKeyDown())
							hyperlinkSignUp.Focus();
						else
							EmailTextBox.Focus();
					}
				}
			}
			else if (sender is System.Windows.Documents.Hyperlink)
			{
				if (IsTabKeyDown(e))
				{
					if (IsShiftKeyDown())
						SignInButton.Focus();
					else
						BtnClose.Focus();
				}
			}
		}

		/// <summary>
		/// Will darag move window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DragGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var window = _systemTray.CurrentWindow("LoginWindow");
			window.DragMove();
		}

		private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_commonObj.ElementFocusedChanged(EmailTextBox, "TextBoxStyle", "textbox");
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			PasswordContentControl.ApplyTemplate();
			GlobalPasswordTextBox = PasswordContentControl.Template.FindName("GlobalPasswordTextBox", PasswordContentControl) as PasswordBox;
		}
	}
}