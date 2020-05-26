using PasswordBoss.ViewModel.ApplicationUpdates;
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
using System.Windows.Threading;

namespace PasswordBoss.Views.ApplicationUpdates
{
    /// <summary>
    /// Interaction logic for UpdateAvailable.xaml
    /// </summary>
    public partial class UpdateAvailable : Window
    {
        DispatcherTimer _timer;

        // This is wrong and I should feel bad, but it would require too much time to refactor update-showing mechanism and PBD-1538 is urgent.
        public static bool IsShown { get; private set; }
        public UpdateAvailable()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(5, 55, 0);
            _timer.IsEnabled = true;
            _timer.Tick += IdleTick;

            Closing += UpdateAvailableClosing;
        }

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			var viewModel = this.DataContext as UpdateAvailableViewModel;
			if (viewModel != null)
			{
				viewModel.UpdateLaterCommand.Execute(null); // to prevent call Close again
			}
		}

		private void UpdateAvailableClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsShown = false;
            if (_timer != null)
            {
                _timer.IsEnabled = false;
            }
        }

        private void IdleTick(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            IsShown = true;
        }
    }
}
