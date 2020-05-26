using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.Models;
using System;
using System.Linq;

namespace ProductTour.ViewModel.Scans
{
    public class ScanResultWithItemsViewModel : StartupScanViewModel, IScanSummary
	{
		#region Properties

        public int Weak { get; private set; }
        public int Duplicate { get; private set; }
        public int Insecure { get; private set; }
        public ScanItemViewModel[] ScanList { get; private set; }

		#endregion
		#region Fields
		private readonly IResolver resolver;
		#endregion

        #region Commands


        #endregion

        public ScanResultWithItemsViewModel(IResolver resolver, Action onClose, ScanResult scanSummary) : base(resolver, onClose)
		{
			this.resolver = resolver;
            this.ScanList = scanSummary.ScanList.Select(s => new ScanItemViewModel(s)).ToArray();
            this.Weak = scanSummary.Weak;
            this.Duplicate = scanSummary.Duplicate;
            this.Insecure = scanSummary.Insecure;
		}

   
    }
}
