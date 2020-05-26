using PasswordBoss.ViewModel.Account;
using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;

namespace PasswordBoss.Views.Login
{
	/// <summary>
	/// Interaction logic for EnterLicenseKey.xaml
	/// </summary>
	public partial class EnterLicenseKey
	{
		private readonly EnterLicenseKeyViewModel viewModel;
		private readonly LoginWindow owner;
		public EnterLicenseKey(IResolver resolver, EnterLicenseKeyViewModel viewModel)
		{
			owner = ((PBApp)System.Windows.Application.Current).FindWindow<LoginWindow>();
			owner.Topmost = false;
			InitializeComponent();
            this.viewModel = viewModel;
			DataContext = this.viewModel;
            this.viewModel.InitWithUICallback(CreateCallbacks());

			LicenseField.PreviewTextInput += LicenseField_PreviewTextInput;
			LicenseField.PreviewKeyDown += LicenseField_PreviewKeyDown;

			TitleGrid.MouseLeftButtonDown += (o, e) => owner.DragMove();
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			owner.Topmost = false;
		}

		private EnterLicenseKeyViewModel.UICallbacks CreateCallbacks()
		{
			return new EnterLicenseKeyViewModel.UICallbacks()
			{
				SetDefaultFocus = SetDefaultFocus
			};
		}

		private void SetDefaultFocus()
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate ()
			{
				LicenseField.Focus();
				//LicenseField.SelectAll();
			}));
		}

		private void LicenseField_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Space)
				e.Handled = true;
		}

		private void LicenseField_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text.Length == 1)
			{
				var c = e.Text[0];
				if (!EnterLicenseKeyViewModel.IsValidActivateChar(c))
				{
					e.Handled = true;
				}
			}
		}
	}
}
