using PasswordBoss.ViewModel.Account;
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
	/// Interaction logic for PersonalInfoSetup.xaml
	/// </summary>
	public partial class PersonalInfoSetup : Page
	{
		private Window _owner = null;
		public PersonalInfoSetup(IResolver resolver)
		{
			InitializeComponent();
			_owner = ((PBApp)Application.Current).FindWindow<LoginWindow>();
			DataContext = new PersonalInfoSetupViewModel(resolver);
			TitleGrid.MouseLeftButtonDown += OnTitleGridMouseLeftButtonDown;
			tbFirstName.Focus();
			_owner.Topmost = false;
		}

		private void OnTitleGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_owner.DragMove();
		}

	}
}
