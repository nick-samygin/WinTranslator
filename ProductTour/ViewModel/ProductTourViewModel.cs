using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using System;
using System.Windows.Media;

namespace ProductTour.ViewModel
{
    class ProductTourViewModel : ViewModelBase
    {
        #region Fields

        private const string STEP_1 = "NewUserOnBoardingProc_3a_slide1";
        private const string STEP_2 = "NewUserOnBoardingProc_3a_slide2";
        private const string STEP_3 = "NewUserOnBoardingProc_3a_slide3";
        private const string STEP_4 = "NewUserOnBoardingProc_3a_slide4";
        private const string STEP_5 = "NewUserOnBoardingProc_3a_slide5";

        private static readonly ILogger _logger = Logger.GetLogger(typeof(ProductTourViewModel));

        private IResolver _resolver = null;
        private IPBData _pbData = null;
        private IAnalytics<OnboardingItem> _analytics = null;

        #endregion

        public ProductTourViewModel(IResolver resolver)
        {
            InitializeCommands();

            _resolver = resolver;
            _pbData = resolver.GetInstanceOf<IPBData>();
            // TODO: remove entire class.
            _analytics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.ScanEventDisabled, OnboardingItem>();

            SelectedIndex = 0;
            IsIncludedWithYourDownload = DeviceHelper.GetInstallType() == "2";
        }

        #region Private methods

        private void InitializeCommands()
        {
            WizardNextCommand = new RelayCommand(WizardNextClick);
            CloseWizardCommand = new RelayCommand(CloseWizardClick);
        }

        private void ChangeScreen()
        {
            switch (SelectedIndex)
            {
                case 0:
                    WizardStepOne();
                    break;

                case 1:
                    WizardStepTwo();
                    break;

                case 2:
                    WizardStepThree();
                    break;

                case 3:
                    WizardStepFour();
                    break;

                case 4:
                    WizardStepFive();
                    break;

                case 5:
                    WizardStepSix();
                    break;
            }
        }

        private void ChangeImage(string resource)
        {
            ProductTourCurrentImage = DefaultProperties.ReturnImage(resource);
        }

        private void WizardStepOne()
        {
            ChangeImage(STEP_1);
        }

        private void WizardStepTwo()
        {
            ChangeImage(STEP_2);
        }

        private void WizardStepThree()
        {
            ChangeImage(STEP_3);
        }

        private void WizardStepFour()
        {
            ChangeImage(STEP_4);
        }

        private void WizardStepFive()
        {
            ChangeImage(STEP_5);
        }

        private void WizardStepSix()
        {
        }

        private void LogProductTourItem(MarketingActionType actionType)
        {
            try
            {
                OnboardingSteps? step;

                var index = SelectedIndex + 1;

                switch (index)
                {
                    case 6:
                        // TODO:
                        step = null;
                        break;

                    default:
                        step = null;
                        break;
                }

                if (step.HasValue)
                {
                    var item = new OnboardingItem(step.Value, actionType);

                    _analytics.Log(item);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        #endregion

        #region Work with commands

        private void WizardNextClick(object obj)
        {
            var p = obj as string;

            if (!string.IsNullOrEmpty(p) && p == "AccountCreation")
            {
                _closedType = ClosedType.AccountCreation;
            }

            LogProductTourItem(MarketingActionType.Continue);

            SelectedIndex++;
        }

        private void CloseWizardClick(object obj)
        {
            var p = obj as string;

            if (!string.IsNullOrEmpty(p))
            {
                switch (p)
                {
                    case "AccountCreation":
                        {
                            _closedType = ClosedType.AccountCreation;
                            LogProductTourItem(MarketingActionType.Close);
                            break;
                        }

                    case "SignIn":
                        {
                            _closedType = ClosedType.SignIn;
                            break;
                        }
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand WizardNextCommand
        {
            get;
            set;
        }

        public RelayCommand CloseWizardCommand
        {
            get;
            set;
        }

        #endregion

        #region Properties

        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;

                ChangeScreen();

                RaisePropertyChanged("SelectedIndex");
            }
        }

        private bool _isIncludedWithYourDownload = false;
        public bool IsIncludedWithYourDownload
        {
            get
            {
                return _isIncludedWithYourDownload;
            }
            set
            {
                _isIncludedWithYourDownload = value;

                RaisePropertyChanged("IsIncludedWithYourDownload");
            }
        }

        private ImageSource _productTourCurrentImage = null;
        public ImageSource ProductTourCurrentImage
        {
            get
            {
                return _productTourCurrentImage;
            }
            set
            {
                if (Equals(_productTourCurrentImage, value))
                {
                    return;
                }

                _productTourCurrentImage = value;

                RaisePropertyChanged("ProductTourCurrentImage");
            }
        }

        private ClosedType _closedType = ClosedType.AccountCreation;
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