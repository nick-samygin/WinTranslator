using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductTour.ViewModel.Scans
{
	public class ScanNowViewModel : StartupScanViewModel
	{

		public RelayCommand ScanNow;

		public ScanNowViewModel(IResolver resolver, Action onClose, Action onScanNow)
			: base(resolver, onClose)
		{
			ScanNow = new RelayCommand((o) =>
				{
					LogStep(MarketingActionType.Continue);
					onScanNow();
				});
		}
	}
}
