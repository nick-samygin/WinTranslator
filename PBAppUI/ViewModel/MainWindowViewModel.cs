using PasswordBoss.Helpers;
using PasswordBoss.Views.ApplicationSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		public MainWindowViewModel()
		{
			PerformSyncDialog.Started += (o, e) => IsInSync = true;
			PerformSyncDialog.Finished += (o, e) => IsInSync = false;

		}

		private bool isInSync;
		public bool IsInSync
		{
			get { return isInSync; }
			set
			{
				isInSync = value;
				RaisePropertyChanged("IsInSync");
			}
		}
	}
}
