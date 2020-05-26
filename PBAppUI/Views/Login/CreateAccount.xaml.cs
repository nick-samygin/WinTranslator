using PasswordBoss.ViewModel.Account;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PasswordBoss.Views.Login
{
	public partial class CreateAccount
	{
		#region Fields

		private static readonly ILogger _logger = Logger.GetLogger(typeof(CreateAccount));

		private IResolver _resolver = null;
		private IPBData _pbData = null;
		private IInAppAnalytics _analytics = null;
		private PasswordBox _masterPasswordTextBox = null;
		private TextBox _masterPasswordPlaceHolder = null;
		private PasswordBox _confirmPasswordTextBox = null;
		private TextBox _confirmPasswordPlaceHolder = null;
		private Window _owner = null;
		private Popup _popup = null;
		private CreateAccountViewModel _vm = null;

		#endregion

		private enum PasswordBoxTagState
		{
			Error = 0,
			Ok = 1,
			Clear = 2
		}

		private enum PasswordBoxObjectType
		{
			Master,
			Confirm
		}

		public CreateAccount(IResolver resolver, CreateAccountViewModel viewModel)
		{
			InitializeComponent();

			_resolver = resolver;
			_pbData = _resolver.GetInstanceOf<IPBData>();
			_analytics = _resolver.GetInstanceOf<IInAppAnalytics>();
			_owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();

			Loaded += OnLoaded;

			TitleGrid.MouseLeftButtonDown += OnTitleGridMouseLeftButtonDown;
			EmailTextBox.KeyDown += OnEmailTextBoxKeyDown;
			EmailTextBox.LostFocus += (o, e) =>
				{
					if (_vm.IsEmailHasValue)
					{
						_vm.ValidateEmail();
					}
				};

			_vm = viewModel;
			DataContext = _vm;

			_vm.OnSubmit += (o, e) =>
			{
				_vm.ValidateEmail();
				
				SetTag(PasswordBoxObjectType.Master, _vm.IsValidMasterPassword);
				SetTag(PasswordBoxObjectType.Confirm, _vm.IsPasswordsEqual && _vm.IsValidMasterPassword);

				Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
				{
					if (!_vm.IsValidEmail)
						EmailTextBox.Focus();
					else if (!_vm.IsValidMasterPassword)
						_masterPasswordTextBox.Focus();
					else if (!_vm.IsPasswordsEqual)
						_confirmPasswordTextBox.Focus();
				}));
			};

			_vm.OnReset += (o, e) =>
			{
				Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
				{
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsPasswordsEqual);
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsValidConfirmPassword);
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsConfirmPasswordHasValue);
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsEmailHasValue);
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsValidMasterPassword);
					_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsValidEmail);
					Clear(PasswordBoxObjectType.Master, true);
					Clear(PasswordBoxObjectType.Confirm, true);
					EmailTextBox.Focus();
					Keyboard.Focus(EmailTextBox);
					EmailTextBox.Select(0, 0);
				}));
			};

			_owner.Topmost = false;

			EmailTextBox.Focus();
		}

		public CreateAccount(IResolver resolver, string email)
			: this(resolver, new CreateAccountViewModel(resolver, email))
		{
		}

		#region Private methods

		private void Clear(PasswordBoxObjectType target, bool clearPasswords)
		{
			PasswordBox passwordBox = null;
			TextBox placeholder = null;

			switch (target)
			{
				case PasswordBoxObjectType.Master:
					passwordBox = _masterPasswordTextBox;
					placeholder = _masterPasswordPlaceHolder;
					if (clearPasswords)
						_vm.UserPasswordWrapper.Password = string.Empty;
					break;

				case PasswordBoxObjectType.Confirm:
					passwordBox = _confirmPasswordTextBox;
					placeholder = _confirmPasswordPlaceHolder;
					if (clearPasswords)
						_vm.UserConfirmPasswordWrapper.Password = string.Empty;
					break;
				default:
					throw new ArgumentException();
			}

			if (clearPasswords)
				passwordBox.Password = string.Empty;

			passwordBox.Tag = PasswordBoxTagState.Clear;
			placeholder.Tag = PasswordBoxTagState.Clear;
		}

		// HACK: need to group placeholder and password box to one item, or make them share same properties
		private void SetTag(PasswordBoxObjectType target, bool validValue)
		{
			PasswordBox passwordBox = null;
			TextBox placeholder = null;

			switch (target)
			{
				case PasswordBoxObjectType.Master:
					passwordBox = _masterPasswordTextBox;
					placeholder = _masterPasswordPlaceHolder;
					break;

				case PasswordBoxObjectType.Confirm:
					passwordBox = _confirmPasswordTextBox;
					placeholder = _confirmPasswordPlaceHolder;
					break;
				default:
					throw new ArgumentException();
			}

			Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
			{

				passwordBox.Tag = ConvertToPasswordBoxTagState(validValue);
				placeholder.Tag = ConvertToPasswordBoxTagState(validValue);
			}));
		}

		private PasswordBoxTagState ConvertToPasswordBoxTagState(bool validValue)
		{
			return validValue ? PasswordBoxTagState.Ok : PasswordBoxTagState.Error;
		}

		private void ShowPopup(Control control, Control placementTarget)
		{
			ClosePopup();

			_popup = new Popup();
			_popup.Child = control;
			_popup.PlacementTarget = placementTarget;
			_popup.Placement = PlacementMode.Right;
			_popup.VerticalOffset = _popup.VerticalOffset - control.Height / 2 + 19; // 19 is GlobalPasswordTextBox.Height / 2
			_popup.PopupAnimation = PopupAnimation.Scroll;
			_popup.AllowsTransparency = true;
			_popup.StaysOpen = true;
			_popup.IsOpen = true;
			_popup.DataContext = DataContext;

			_owner.Activated += OnOwnerActivated;
			_owner.Deactivated += OnOwnerDeactivated;
			_owner.StateChanged += OnOwnerStateChanged;
			_owner.LocationChanged += OnOwnerLocationChanged;
		}

		private void ReopenPopup()
		{
			if (_popup != null)
			{
				_popup.IsOpen = true;
			}
		}

		private void ClosePopup(bool destroy = true)
		{
			if (_popup != null)
			{
				_popup.IsOpen = false;
				_popup = destroy ? null : _popup;
			}

			if (destroy)
			{
				_owner.Activated -= OnOwnerActivated;
				_owner.Deactivated -= OnOwnerDeactivated;
				_owner.StateChanged -= OnOwnerStateChanged;
				_owner.LocationChanged -= OnOwnerLocationChanged;
			}
		}

		private bool IsTabKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Tab)
			{
				e.Handled = true;

				return true;
			}

			return false;
		}

		#endregion

		#region Event handlers

		private Binding GenerateBinding(string prop, BindingMode mode = BindingMode.OneWay, IValueConverter valueConverter = null)
		{
			return new Binding()
			{
				Source = DataContext,
				Converter = valueConverter,
				Path = new PropertyPath(prop),
				Mode = mode,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			// Apply corresponding template, bind events, bind data, return object
			Func<ContentControl, string, PasswordBox> getPasswordBoxWithAppliedTemplate = (cc, vm_source_name) =>
			{
				Func<string, string> format_prop = (prop) => string.Format("{0}.{1}", vm_source_name, prop);
				// var vm_pass_property_name = format_prop("Password");
				var vm_placeholder_property_name = format_prop("PlaceHolderText");
				var vm_eye_image_visibility_property_name = format_prop("EyeImageVisibility");
				cc.ApplyTemplate();
				var tb = cc.Template.FindName("GlobalPasswordTextBox", cc) as PasswordBox;

				/*
                BindingOperations.SetBinding(tb,
                    PasswordHelper.PasswordProperty,
                    GenerateBinding(vm_pass_property_name, BindingMode.TwoWay));
                */

				var place_holder_text = cc.Template.FindName("ShowTextBox", cc) as TextBox;
				BindingOperations.SetBinding(place_holder_text,
					TextBox.TextProperty,
					GenerateBinding(vm_placeholder_property_name));

				var eye_image_box = cc.Template.FindName("ShowPasswordCharsCheckBox", cc) as ToggleButton;
				BindingOperations.SetBinding(eye_image_box,
					Image.VisibilityProperty,
					GenerateBinding(vm_eye_image_visibility_property_name, valueConverter: new BooleanToVisibilityConverter()));

				Helpers.PasswordHelper.SetAttach(tb, true);

				return tb;
			};

			_masterPasswordTextBox = getPasswordBoxWithAppliedTemplate(MasterPasswordContentControl, "UserPasswordWrapper");
			_masterPasswordPlaceHolder = MasterPasswordContentControl.Template.FindName("ShowTextBox", MasterPasswordContentControl) as TextBox;

			_confirmPasswordTextBox = getPasswordBoxWithAppliedTemplate(ConfirmPasswordContentControl, "UserConfirmPasswordWrapper");
			_confirmPasswordPlaceHolder = ConfirmPasswordContentControl.Template.FindName("ShowTextBox", ConfirmPasswordContentControl) as TextBox;

			// master password binding
			_masterPasswordTextBox.KeyDown += OnMasterPasswordTextBoxKeyDown;

			_masterPasswordTextBox.GotFocus += (o, ev)
				=> ShowPopup(new MasterPasswordPopup(), MasterPasswordContentControl);
			_masterPasswordTextBox.LostFocus += (o, ev)
				=> ClosePopup();

			//_masterPasswordTextBox.GotFocus += (o, ev) => Clear(PasswordBoxObjectType.Master, false);
			_masterPasswordTextBox.LostFocus += (o, ev) =>
				{
					if (_vm.IsPasswordHasValue)
						SetTag(PasswordBoxObjectType.Master, _vm.ValidateMasterPassword());
				};

			_masterPasswordTextBox.PasswordChanged += OnMasterPasswordTextBoxPasswordChanged;


			// confirm password binding			
			_confirmPasswordTextBox.GotFocus += (o, ev)
				=> ShowPopup(new ConfirmPasswordPopup(), ConfirmPasswordContentControl);

			_confirmPasswordTextBox.LostFocus += (o, ev)
				=>
				{
					if (!_vm.ValidatePasswordsEqual())
					{
						tbPasswordNotMatched.Visibility = Visibility.Visible;
					}
					ClosePopup();
				};

			//_confirmPasswordTextBox.GotFocus += (o, ev)	=> Clear(PasswordBoxObjectType.Confirm, false);
			_confirmPasswordTextBox.LostFocus += (o, ev) =>
				{
					
						SetTag(PasswordBoxObjectType.Confirm, _vm.ValidatePasswordsEqual() && _vm.ValidateConfirmPassword());
				};

			_confirmPasswordTextBox.PasswordChanged += OnConfirmPasswordTextBoxPasswordChanged;

			Clear(PasswordBoxObjectType.Master, true);
			Clear(PasswordBoxObjectType.Confirm, true);
			_vm.RefreshValidations();
		}

		private void OnTitleGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_owner.DragMove();
		}

		private void CloseClick(object sender, object e)
		{
			_owner.Close();

			((CreateAccountViewModel)DataContext).CloseCommand.Execute(sender);
		}

		private void OnEmailTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (sender is TextBox)
			{
				if (IsTabKeyDown(e))
				{
					if (_masterPasswordTextBox != null)
					{
						_masterPasswordTextBox.Focus();
					}

				}
			}
		}

		private void OnMasterPasswordTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (sender is PasswordBox)
			{
				if (IsTabKeyDown(e))
				{
					if (_confirmPasswordTextBox != null)
					{
						_confirmPasswordTextBox.Focus();
					}
				}

			}
		}

		private void OnMasterPasswordTextBoxPasswordChanged(object sender, RoutedEventArgs e)
		{
			_vm.ResetValidation(CreateAccountViewModel.ValidationProperties.IsPasswordsEqual);
			
			_vm.UserPasswordWrapper.Password = _masterPasswordTextBox.Password;

			if (sender != null)
			{
				OnMasterPasswordTextBoxPasswordChanged(null, null);
			}
			Clear(PasswordBoxObjectType.Master, false);
			Clear(PasswordBoxObjectType.Confirm, false);

			if (_vm.ValidateMasterPassword())
			{
				SetTag(PasswordBoxObjectType.Master, true);
			}
		}

		private void OnConfirmPasswordTextBoxPasswordChanged(object sender, RoutedEventArgs e)
		{
			
			_vm.UserConfirmPasswordWrapper.Password = _confirmPasswordTextBox.Password;
			Clear(PasswordBoxObjectType.Confirm, false);

			tbPasswordNotMatched.Visibility = Visibility.Collapsed;

			// set field to green only. 
			if (_vm.ValidatePasswordsEqual() && _vm.ValidateConfirmPassword())
			{
				SetTag(PasswordBoxObjectType.Confirm, true);
			}

			if (sender != null)
			{
				OnConfirmPasswordTextBoxPasswordChanged(null, null);
			}


		}

		private void OnOwnerActivated(object sender, EventArgs e)
		{
			ReopenPopup();
		}

		private void OnOwnerDeactivated(object sender, EventArgs e)
		{
			ClosePopup(destroy: false);
		}

		private void OnOwnerStateChanged(object sender, EventArgs e)
		{
			ClosePopup();
		}

		private void OnOwnerLocationChanged(object sender, EventArgs e)
		{
			double offset = _popup.HorizontalOffset;

			_popup.HorizontalOffset = offset + 1;
			_popup.HorizontalOffset = offset;
		}

		#endregion
	}
}