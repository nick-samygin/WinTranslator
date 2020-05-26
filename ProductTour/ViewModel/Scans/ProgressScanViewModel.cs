using PasswordBoss;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.BusinessLayer;
using ProductTour.BusinessLayer.Stubs;
using ProductTour.Models;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace ProductTour.ViewModel.Scans
{
    public class ProgressScanViewModel : StartupScanViewModel
	{
		#region Fields

		private ILoginsReader loginsReader;

		private ScanResult result = new ScanResult();
		public ScanResult Result { get { return result; } }

        private static bool isScanStarted = false;
        private static object scanLocker = new object();

        private readonly Action onScanCompleted;

        private Task scanTask = null;

		#endregion

		public ProgressScanViewModel(IResolver resolver, ILoginsReader loginsReader, Action onClose, Action onScanCompleted) 
            : base(resolver, onClose)
		{
            this.loginsReader = loginsReader;
			this._resolver = resolver;
        
            if (onScanCompleted == null)
            {
                var text= "onScanCompleted = null";
                _logger.Error(text);
                throw new NullReferenceException(text);
            }

            this.onScanCompleted = onScanCompleted;

		}

        public void StartScan()
        {
            lock (scanLocker)
            {
                if (isScanStarted)
                {
                    return;
                }

                scanTask = new Task(() => result = loginsReader.ScanBrowsers());
                scanTask.Start();
            }
        }

		private static bool animationCompleted = false;
        public void OnAnimationScanCompleted()
        {
            scanTask.Wait();
            onScanCompleted();

			// prevent accidantly duplication
			if (!animationCompleted)
			{
				animationCompleted = true;
				LogStep(MarketingActionType.Continue);
			}
        }

		#region Properties

		private int _scannedValue = 0;
		public int ScannedValue
		{
			get
			{
				return _scannedValue;
			}
			set
			{
				_scannedValue = value;

				RaisePropertyChanged("ScannedValue");

			}
		}

		private int _remainingValue = 0;
		public int RemainingValue
		{
			get
			{
				return _remainingValue;
			}
			set
			{
				_remainingValue = value;

				RaisePropertyChanged("RemainingValue");

			}
		}

		#endregion
    }
}