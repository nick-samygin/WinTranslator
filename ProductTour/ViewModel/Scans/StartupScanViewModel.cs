using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.Analytics;
using System;

namespace ProductTour.ViewModel.Scans
{
    public class StartupScanViewModel : ScanViewModelBase, IAnalyticsLogger
    {
		protected ScanViewModelBase _currentVM = null;
		public ScanViewModelBase CurrentVM
		{
			get
			{
				return _currentVM;
			}
			set
			{
				_currentVM = value;

				RaisePropertyChanged("CurrentVM");
			}
		}

        #region Events

        public delegate void OnCloseEventHandler();
		public event OnCloseEventHandler OnCloseEvent;

        #endregion

        #region Fields

        protected static readonly ILogger _logger = Logger.GetLogger(typeof(StartupScanViewModel));

        protected IResolver _resolver = null;

        protected  Action onClose;

        #endregion


        public StartupScanViewModel(IResolver resolver, Action onClose) : base(resolver)
        {
            InitializeCommands();

            this._resolver = resolver;
            this.onClose = onClose;
        }

        #region Private methods

        private void InitializeCommands()
        {
            CloseWizardCommand = new RelayCommand(CloseWizardClick);
        }

        #endregion

        #region Work with commands

        private void CloseWizardClick(object obj)
        {
            var p = obj as string;

			var currentAnalyticsViewModel = CurrentVM as ScanViewModelBase;
			if (currentAnalyticsViewModel == null)
			{
				currentAnalyticsViewModel = this;
			}

            if (!string.IsNullOrEmpty(p))
            {
                switch (p)
                {
                    case "AccountCreation":
                        {
                            _closedType = ClosedType.AccountCreation;

							currentAnalyticsViewModel.LogStep(MarketingActionType.Continue);
                            break;
                        }

                    case "SignIn":
                        {
                            _closedType = ClosedType.SignIn;
							currentAnalyticsViewModel.LogStep(MarketingActionType.Close);
                            break;
                        }
					case "AccountCreation_Close":
						{
								_closedType = ClosedType.AccountCreation;
								currentAnalyticsViewModel.LogStep(MarketingActionType.Close);
								break;
						}
                }
            }

			ProductTour.ClosedTypeStatic = this._closedType;
			
			var ssVm = CurrentVM as StartupScanViewModel;
			if (ssVm != null)
			{
				ssVm._closedType = this._closedType;
				if (ssVm.OnCloseEvent != null)
				{
					ssVm.OnCloseEvent();
				}
			}

            onClose();
			if (OnCloseEvent != null)
			{
				OnCloseEvent();
			}
        }

        #endregion

        #region Commands

        public RelayCommand CloseWizardCommand
        {
            get;
            set;
        }

        #endregion

        #region Properties

        protected ClosedType _closedType = ClosedType.AccountCreation;
        public ClosedType ClosedType
        {
            get
            {
                return _closedType;
            }
        }

        #endregion
    }
}