using PasswordBoss.ViewModel.BrowserExtentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views.Login
{
	/// <summary>
	/// Interaction logic for BrowserExtentionSetup.xaml
	/// </summary>
	public partial class BrowserExtentionSetup 
	{
		private readonly IResolver resolver;
		private readonly BrowserExtentionSetupViewModel viewModel;
		public BrowserExtentionSetup(IResolver resolver, ViewModel.BrowserExtentions.SetupProviderBase setupProviderBase)
		{
			InitializeComponent();

			if (resolver == null)
				throw new ArgumentNullException("resolver");

			if (setupProviderBase == null)
				throw new ArgumentNullException("setupProvider");

			this.resolver = resolver;
			this.viewModel = new BrowserExtentionSetupViewModel(resolver, setupProviderBase);
			this.DataContext = viewModel;


			var _owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();

			viewModel.Closed += viewModel_Closed;

			OnboardingHeader.MouseLeftButtonDown += (o,e)=> 
			{
				_owner.DragMove();
			};
			_owner.Topmost = false;
		}

		void viewModel_Closed(object sender, EventArgs e)
		{
			var _owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();
			_owner.NavigateloginScreens();
		}
	}
}
