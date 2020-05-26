using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.BusinessLayer;
using ProductTour.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProductTour.ViewModel.Scans
{

    class StartupScanEnabledViewModel : StartupScanViewModel, IScanSummary
    {
        private bool isScanSummaryPopupVisible = false;
        public bool IsScanSummaryPopupVisible
        {
            get { return isScanSummaryPopupVisible; }
            set
            {
                isScanSummaryPopupVisible = value;
                RaisePropertyChanged("IsScanSummaryPopupVisible");
            }
        }
        public IScanSummary ScanSummary { get; private set; }
        public int Insecure { get { return ScanSummary.Insecure; } }
        public int Duplicate { get { return ScanSummary.Duplicate; } }
        public int Weak { get { return ScanSummary.Weak; } }

        private readonly ScanPopupNotificationViewModel popupViewModel;
        public ScanPopupNotificationViewModel PopupViewModel
        {
            get { return popupViewModel; }
        }

        public RelayCommand ClosePopup { get; set; }

		private ScanViewModelBase mainViewModel;

		private readonly Action onCloseOut;

		private void OnClose()
		{
			var ssVm = CurrentVM as StartupScanViewModel;
			if (ssVm != null)
			{
				this._closedType = ssVm.ClosedType;
			}
			if (onCloseOut != null)
				onCloseOut();

		}
        public StartupScanEnabledViewModel(IResolver resolver, IRegistryManager registryManager, ILoginsReader loginsReader, Action onCloseOut, bool isAutoShowPopup)
            : base(resolver, null)
        {
			this.onCloseOut = onCloseOut;
			this.onClose = OnClose;
            // By default
            ScanStatus = DefaultProperties.ReturnString("Onboardv4StatusUnknown");
            ScanSummary = registryManager.GetScanSummary();

			popupViewModel = new ScanPopupNotificationViewModel(resolver, this, OnClose);

            _currentVM = mainViewModel;

            bool isScanSummaryEmpty = ScanSummary.Duplicate + ScanSummary.Weak + ScanSummary.Insecure == 0;
            IsScanSummaryPopupVisible = isAutoShowPopup && !isScanSummaryEmpty;

            if (IsScanSummaryPopupVisible)
            {
				mainViewModel = new ScanNowViewModel(_resolver, OnClose, () => StartScanCommand.Execute(this));
            }
            else
            {
				mainViewModel = new ProgressScanViewModel(_resolver, loginsReader, OnClose, OnScanCompleted);
            }

            ClosePopup = new RelayCommand((o) =>
            {
				popupViewModel.LogStep(MarketingActionType.Continue);
                IsScanSummaryPopupVisible = false;
				CurrentVM = new ScanNowViewModel(_resolver, OnClose, () =>
                {
                    StartScanCommand.Execute(o);
                });
            });
            
            StartScanCommand = new RelayCommand((o) =>
                    {
						mainViewModel = new ProgressScanViewModel(_resolver, loginsReader, OnClose, OnScanCompleted);
                        var vm = mainViewModel as ProgressScanViewModel;
                        if (vm != null)
                        {
                            CurrentVM = vm;
                            vm.StartScan(); // TODO: better move to View.Loaded event, where animation starts now
                        }
                    }
                );

            if (IsScanSummaryPopupVisible)
            {
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    StartScanCommand.Execute(this);
                });
            }
        }

        // dont rename. Reflection
        public void OnMenuChecked(object itemName)
        {
            if (itemName == null)
                return;
            if (itemName.ToString() == "ScanStatus")
            {
                CurrentVM = mainViewModel;
            }
            else
            {
                CurrentVM = new ScanResultNoItemsViewModel(_resolver, onClose);
            }
        }

        public RelayCommand StartScanCommand
        {
            get;
            set;
        }

        #region Event handlers

        private void OnScanCompleted()
        {

            var vm = CurrentVM as ProgressScanViewModel;

            if (vm != null)
            {
                if (vm.Result.ScanList.Length != 0)
                {
                    mainViewModel = new ScanResultWithItemsViewModel(_resolver, onClose, vm.Result);
                    ScanStatus = DefaultProperties.ReturnString("Onboardv4StatusAtRisk");
                }
                else
                {
                    mainViewModel = new ScanResultNoItemsViewModel(_resolver, onClose);
                    ScanStatus = DefaultProperties.ReturnString("Onboardv4StatusAtRisk");
                }

                CurrentVM = mainViewModel;
            }
        }

        #endregion

        #region Properties

       
        private string _scanStatus = null;
        public string ScanStatus
        {
            get
            {
                return _scanStatus;
            }
            set
            {
                _scanStatus = value;

                if (_scanStatus == DefaultProperties.ReturnString("Onboardv4StatusUnknown"))
                {
                    ScanStatusImage = DefaultProperties.ReturnImage("warningOrange");
                }

                if (_scanStatus == DefaultProperties.ReturnString("Onboardv4StatusAtRisk"))
                {
					ScanStatusImage = DefaultProperties.ReturnImage("warningRedSmall");
                }

                RaisePropertyChanged("ScanStatus");
				RaisePropertyChanged("ScanStatusImage");
            }
        }

        private ImageSource _scanStatusImage = null;
        public ImageSource ScanStatusImage
        {
            get
            {
                return _scanStatusImage;
            }
            set
            {
                _scanStatusImage = value;

                RaisePropertyChanged("ScanStatusImage");
            }
        }

        #endregion
   }
}