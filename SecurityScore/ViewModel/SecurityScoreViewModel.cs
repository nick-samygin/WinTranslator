using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Model.SecurityScore;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views.UserControls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel
{
    public class Step: INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler IsStepExpandedChanged;



        private string _titel;
        public string Titel
        {
            get { return _titel; }
            set { _titel = value; }
        }

        private int _stepNumber;
        public int StepNumber
        {
            get { return _stepNumber; }
            set { _stepNumber = value; }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                 ShowCheckIcon = value == 0;

                RaisePropertyChanged("Count");
            }
        }


        private Brush _color;
        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private bool _isStepExpanded;
        public bool IsStepExpanded
        {
            get { return _isStepExpanded; }
            set
            {
                if (_isStepExpanded != value)
                {
                    _isStepExpanded = value;
                    if (IsStepExpandedChanged != null)
                        IsStepExpandedChanged(this, null);
                    RaisePropertyChanged("IsStepExpanded");
                }
            }
        }

       

        private bool _showCheckIcon;
        public bool ShowCheckIcon
        {
            get { return _showCheckIcon; }
            set
            {
                if (_showCheckIcon != value)
                {
                    _showCheckIcon = value;
                    RaisePropertyChanged("ShowCheckIcon");
                }
            }
        }

        private bool _showCount;
        public bool ShowCount
        {
            get { return _showCount; }
            set
            {
                if (_showCount != value)
                {
                    _showCount = value;
                    RaisePropertyChanged("ShowCount");
                }
            }
        }


        List<SecurityScoreData> _securityScoreData;
        public List<SecurityScoreData> SecurityScoreData
        {
            get { return _securityScoreData; }
            set
            {
                _securityScoreData = value;
                RaisePropertyChanged("SecurityScoreData");
            }
        }

        public virtual bool ShowArrow
        {
            get { return false; }

        }

        public UserControl StepContent
        {
            get; set;
        }
    }

    public class DublicatePasswordsStep : Step
    {

        public  override bool ShowArrow
        {
            get { return true; }
            
        }

        IEnumerable<IGrouping<string,SecurityScoreData>> _dublicateSecurityScoreData;
        public IEnumerable<IGrouping<string,SecurityScoreData>> DublicateSecurityScoreData
        {
            get { return _dublicateSecurityScoreData; }
            set
            {
                _dublicateSecurityScoreData = value;
                RaisePropertyChanged("DublicateSecurityScoreData");
            }
        }
    }

    public class SecurityScoreViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecurityScoreViewModel));
        
        private static readonly SolidColorBrush Colorgray = new SolidColorBrush(Colors.Gray);
        private IPBData pbData;
        private IInAppAnalytics inAppAnalyitics;
        private IResolver resolver;

        #region Relay commands
        public RelayCommand SecurityScoreExpandedCommand { get; set; }
        public RelayCommand SecurityScoreCollapsedCommand { get; set; }
        public RelayCommand SecurityScoreToggleExpanderCommand { get; set; }
        public RelayCommand SecurityScoreUpdateCommand { get; set; }
        public RelayCommand SecurityScoreCancelCommand { get; set; }
        public RelayCommand SecurityScoreOkCommand { get; set; }
        public RelayCommand SecurityScoreEyeCommand { get; set; }
        public RelayCommand UpdateSecurityScoreCommand { get; set; }
        public RelayCommand DuplicatePasswordCommand { get; set; }
        public RelayCommand WeakPasswordCommand { get; set; }
        public RelayCommand OldPasswordCommand { get; set; }
        public RelayCommand DuplicatePasswordMouseEnterCommand { get; set; }
        public RelayCommand DuplicatePasswordMouseLeaveCommand { get; set; }
        public RelayCommand WeakPasswordMouseEnterCommand { get; set; }
        public RelayCommand WeakPasswordMouseLeaveCommand { get; set; }
        public RelayCommand OldPasswordMouseEnterCommand { get; set; }
        public RelayCommand OldPasswordMouseLeaveCommand { get; set; }
       // public RelayCommand MainContentLostFocusCommand { get; set; }
        public RelayCommand UpdatePasswordContinueCommand { get; set; }
        #endregion

        private SecurityScoreDataHelper _securityScoreDataHelper;
        public SecurityScoreDataHelper SecurityScoreDataHelper
        {
            get { return _securityScoreDataHelper; }
            private set{_securityScoreDataHelper = value;}
        }

        public List<Step> Steps { get; set; }
       
        public SecurityScoreViewModel(IResolver resolver)
        {            
            pbData = resolver.GetInstanceOf<IPBData>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            _securityScoreDataHelper = new Helpers.SecurityScoreDataHelper(resolver);
            this.resolver = resolver;

            RefreshStats();
            InitializeCommands();

            var setting = pbData.GetUserSetting(DefaultProperties.Configuration_Key_SecurityScoreInfo);
            if(!string.IsNullOrWhiteSpace(setting))
            {
                bool ret;
                if (bool.TryParse(setting, out ret))
                {
                    ShowUpdatePasswordInfo = !ret;
                }
            }
        }

        #region otherMethods

        /// <summary>
        /// Old password Default style
        /// </summary>
        private void OldPasswordDefaultStyle()
        {
            PasswordsFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.SemiboldWeight);
            PasswordsFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaSemiBoldfamily);
            ThreePasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
            PasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
        }

        /// <summary>
        /// used to cahnge font color on  mouse enter style 
        /// </summary>
        /// <param name="obj"></param>
        private void WeakPasswordMouseEnter(object obj)
        {
            WeakPasswordDefaultStyle();
        }

        /// <summary>
        /// used to cahnge font color on  mouse leave style 
        /// </summary>
        /// <param name="obj"></param>
        private void WeakPasswordMouseLeave(object obj)
        {
            if (_weakPasswordFlag == 0)
            {
                OneWeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.FillEllipsesColor);
                WeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.SecurityScoreDynamicContentColor);
                WeakPasswordFontWeight = DefaultProperties.ReturnFontWeight(DefaultProperties.NormalWeight);
                WeakPasswordFontFamily = DefaultProperties.ReturnFontFamily(DefaultProperties.ProximaRegularFamily);
            }
        }

        /// <summary>
        /// Weak password Default style
        /// </summary>
        private void WeakPasswordDefaultStyle()
        {

            WeakPasswordFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.SemiboldWeight);
            WeakPasswordFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaSemiBoldfamily);
            OneWeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
            WeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
        }

        /// <summary>
        /// Duplicate password Default style
        /// </summary>
        private void DuplicatePasswordDefaultStyle()
        {
            DublicatePasswordFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.SemiboldWeight);
            DublicatePasswordFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaSemiBoldfamily);
            TwoDublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
            DublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.Foregroundselectedcolor);
        }

        public List<SecurityScoreData> BindingSecurityScoreList(string type)
        {
            return SecurityScoreDataHelper.GetSecurityScoreData(type);
        }


        /// <summary>
        /// Default font property for security score 
        /// </summary>
        private void DefaultFontPropertySecurityScore()
        {
            TwoDublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.FillEllipsesColor);
            DublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.SecurityScoreDynamicContentColor);
            ThreePasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.FillEllipsesColor);
            PasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.SecurityScoreDynamicContentColor);
            OneWeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.FillEllipsesColor);
            WeakPasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(SecurityScoreHelper.SecurityScoreDynamicContentColor);

            DublicatePasswordFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.NormalWeight);
            DublicatePasswordFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaRegularFamily);
            WeakPasswordFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.NormalWeight);
            WeakPasswordFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaRegularFamily);
            PasswordsFontWeight = SecurityScoreHelper.ReturnFontWeight(SecurityScoreHelper.NormalWeight);
            PasswordsFontFamily = SecurityScoreHelper.ReturnFontFamily(SecurityScoreHelper.ProximaRegularFamily);

            _duplicatePasswordFlag = 0;
            _weakPasswordFlag = 0;
            _oldPasswordFlag = 0;
        }

        /// <summary>
        /// Used for counting security score list data
        /// </summary>
        public void SecurityScoreDataCount()
        {
            if(pbData.Locked)
            {
                return;
            }
            AllPasswordsCount = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
            var SecurityScoreDuplicateDataItems = BindingSecurityScoreList(SecurityScoreItemType.duplicate);
            Steps[0].Count= SecurityScoreDuplicateDataItems.Count();
            (Steps[0]  as DublicatePasswordsStep).DublicateSecurityScoreData = SecurityScoreDuplicateDataItems.GroupBy(x=>x.password);
           

            var SecurityScoreWeakDataItems = BindingSecurityScoreList(SecurityScoreItemType.week);
            Steps[1].Count = SecurityScoreWeakDataItems.Count();
            Steps[1].SecurityScoreData = SecurityScoreWeakDataItems;

            var SecurityScoreOldDataItems = BindingSecurityScoreList(SecurityScoreItemType.old);
            Steps[2].Count = SecurityScoreOldDataItems.Count();
            Steps[2].SecurityScoreData = SecurityScoreOldDataItems;

            _securityScoreDecimal = SecurityScoreDataHelper.GetSecurityScorePercentage(SecurityScoreDuplicateDataItems, SecurityScoreWeakDataItems, SecurityScoreOldDataItems);
            SecurityScore = string.Format("{0:0}%", _securityScoreDecimal);
            
        }

        public void RefreshStats()
        {
            //vedo - async
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                SecurityScoreDataCount();

                var scLog = inAppAnalyitics.Get<Events.AnalyticsLog, AnalyticsItem>();

                var item = new AnalyticsItem(Convert.ToInt32(_securityScoreDecimal), DuplicatePasswordCounter, WeakPasswordCounter, OldPasswordCounter, SecurityScoreDataHelper.GetSecurityScoreData(SecurityScoreItemType.all).Count());

                scLog.Log(item);
            }));
        }

        /// <summary>
        /// used to assign opacity 0.2 & hit test false to Mainpage_maingrid
        /// </summary>
        public void MainpageHitTestFalseOpacity()
        {
            MainGridIsHitTest = false;
            // MainPageGrid.Opacity = 0.2;
        }

        /// <summary>
        /// used to assign opacity 1 & hit test true to Mainpage_maingrid
        /// </summary>
        public void MainpageHitTestTrueOpacity()
        {
            MainGridIsHitTest = true;
            // MainPageGrid.Opacity = 1;
        }

        #endregion

        #region Properties

        
        private int _allPasswordsCount;

        public int AllPasswordsCount
        {
            get { return _allPasswordsCount; }
            set
            {
                _allPasswordsCount = value;
                RaisePropertyChanged("AllPasswordsCount");
            }
        }

       
        private Brush _twoDublicatePasswordForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.Foregroundselectedcolor);

        public Brush TwoDublicatePasswordForgroundColor
        {
            get { return _twoDublicatePasswordForgroundColor; }
            set
            {
                if (Equals(_twoDublicatePasswordForgroundColor, value)) return;
                _twoDublicatePasswordForgroundColor = value;
                RaisePropertyChanged("TwoDublicatePasswordForgroundColor");
            }
        }

        private Brush _dublicatePasswordForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.Foregroundselectedcolor);

        public Brush DublicatePasswordForgroundColor
        {
            get { return _dublicatePasswordForgroundColor; }
            set
            {
                if (Equals(_dublicatePasswordForgroundColor, value)) return;
                _dublicatePasswordForgroundColor = value;
                RaisePropertyChanged("DublicatePasswordForgroundColor");
            }
        }
        private FontWeight _dublicatePasswordFontWeight = (FontWeight)Application.Current.FindResource("SemiboldWeight");
        public FontWeight DublicatePasswordFontWeight
        {
            get { return _dublicatePasswordFontWeight; }
            set
            {
                if (Equals(_dublicatePasswordFontWeight, value)) return;
                _dublicatePasswordFontWeight = value;
                RaisePropertyChanged("DublicatePasswordFontWeight");
            }
        }

        private FontFamily _dublicatePasswordFontFamily = (FontFamily)Application.Current.FindResource("ProximaSemiBoldfamily");
        public FontFamily DublicatePasswordFontFamily
        {
            get { return _dublicatePasswordFontFamily; }
            set
            {
                if (Equals(_dublicatePasswordFontFamily, value)) return;
                _dublicatePasswordFontFamily = value;
                RaisePropertyChanged("DublicatePasswordFontFamily");
            }
        }
        private Brush _oneWeakPasswordForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.FillEllipsesColor);

        public Brush OneWeakPasswordForgroundColor
        {
            get { return _oneWeakPasswordForgroundColor; }
            set
            {
                if (Equals(_oneWeakPasswordForgroundColor, value)) return;
                _oneWeakPasswordForgroundColor = value;
                RaisePropertyChanged("OneWeakPasswordForgroundColor");
            }
        }

        private Brush _weakPasswordForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.SecurityScoreDynamicContentColor);

        public Brush WeakPasswordForgroundColor
        {
            get { return _weakPasswordForgroundColor; }
            set
            {
                if (Equals(_weakPasswordForgroundColor, value)) return;
                _weakPasswordForgroundColor = value;
                RaisePropertyChanged("WeakPasswordForgroundColor");
            }
        }
        private FontWeight _weakPasswordFontWeight = (FontWeight)Application.Current.FindResource("NormalWeight");
        public FontWeight WeakPasswordFontWeight
        {
            get { return _weakPasswordFontWeight; }
            set
            {
                if (Equals(_weakPasswordFontWeight, value)) return;
                _weakPasswordFontWeight = value;
                RaisePropertyChanged("WeakPasswordFontWeight");
            }
        }

        private FontFamily _weakPasswordFontFamily = (FontFamily)Application.Current.FindResource("ProximaRegularFamily");
        public FontFamily WeakPasswordFontFamily
        {
            get { return _weakPasswordFontFamily; }
            set
            {
                if (Equals(_weakPasswordFontFamily, value)) return;
                _weakPasswordFontFamily = value;
                RaisePropertyChanged("WeakPasswordFontFamily");
            }
        }

        private Brush _threePasswordsForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.FillEllipsesColor);

        public Brush ThreePasswordsForgroundColor
        {
            get { return _threePasswordsForgroundColor; }
            set
            {
                if (Equals(_threePasswordsForgroundColor, value)) return;
                _threePasswordsForgroundColor = value;
                RaisePropertyChanged("ThreePasswordsForgroundColor");
            }
        }

        private Brush _passwordsForgroundColor = SecurityScoreHelper.AlertBorderStrokecolor(SecurityScoreHelper.SecurityScoreDynamicContentColor);

        public Brush PasswordsForgroundColor
        {
            get { return _passwordsForgroundColor; }
            set
            {
                if (Equals(_passwordsForgroundColor, value)) return;
                _passwordsForgroundColor = value;
                RaisePropertyChanged("PasswordsForgroundColor");
            }
        }
        private FontWeight _passwordsFontWeight = (FontWeight)Application.Current.FindResource("NormalWeight");
        public FontWeight PasswordsFontWeight
        {
            get { return _passwordsFontWeight; }
            set
            {
                if (Equals(_passwordsFontWeight, value)) return;
                _passwordsFontWeight = value;
                RaisePropertyChanged("PasswordsFontWeight");
            }
        }

        private FontFamily _passwordsFontFamily = (FontFamily)Application.Current.FindResource("ProximaRegularFamily");
        public FontFamily PasswordsFontFamily
        {
            get { return _passwordsFontFamily; }
            set
            {
                if (Equals(_passwordsFontFamily, value)) return;
                _passwordsFontFamily = value;
                RaisePropertyChanged("PasswordsFontFamily");
            }
        }

        /// <summary>
        /// keeping flag for button checked & unchecked 
        /// if 0 then uncheck if 1 then checked
        /// </summary>
        private int _duplicatePasswordFlag = 1;
        private int _weakPasswordFlag;
        private int _oldPasswordFlag;

        private Brush _securityUpdatePopupBackColor;

        public Brush SecurityUpdatePopupBackColor
        {
            get { return _securityUpdatePopupBackColor; }
            set
            {
                if (Equals(_securityUpdatePopupBackColor, value)) return;
                _securityUpdatePopupBackColor = value;
                RaisePropertyChanged("SecurityUpdatePopupBackColor");
            }
        }

        private bool _securityUpdatePopupVisibility;
        public bool SecurityUpdatePopupVisibility
        {
            get { return _securityUpdatePopupVisibility; }
            set
            {
                _securityUpdatePopupVisibility = value;
                RaisePropertyChanged("SecurityUpdatePopupVisibility");
            }
        }

        public string RedirectUrl { get; set; }

        /// IsHitTest properties for Grid 
        private bool _mainGridIsHitTest;

        public bool MainGridIsHitTest
        {
            get { return _mainGridIsHitTest; }
            set
            {
                _mainGridIsHitTest = value;
                RaisePropertyChanged("MainGridIsHitTest");
            }
        }

        private decimal _securityScoreDecimal;

        private string _securityScore;
        public string SecurityScore
        {
            get { return _securityScore; }
            set
            {
                if (_securityScore != value)
                {
                    _securityScore = value;
                    RaisePropertyChanged("SecurityScore");
                }
            }
        }

        private int _duplicatePasswordCounter;
        public int DuplicatePasswordCounter
        {
            get { return _duplicatePasswordCounter; }
            set
            {
                _duplicatePasswordCounter = value;
                RaisePropertyChanged("DuplicatePasswordCounter");
            }
        }
        private int _weakPasswordCounter;
        public int WeakPasswordCounter
        {
            get { return _weakPasswordCounter; }
            set
            {
                _weakPasswordCounter = value;
                RaisePropertyChanged("WeakPasswordCounter");
            }
        }
        private int _oldPasswordCounter;
        public int OldPasswordCounter
        {
            get { return _oldPasswordCounter; }
            set
            {
                _oldPasswordCounter = value;
                RaisePropertyChanged("OldPasswordCounter");
            }
        }

        private bool _securityScoreVisibility;

        public bool SecurityScoreVisibility
        {
            get { return _securityScoreVisibility; }
            set
            {
                _securityScoreVisibility = SecurityScoreExpanded = value;
                RaisePropertyChanged("SecurityScoreVisibility");
            }
        }

        private bool _securityScoreExpanded;

        public bool SecurityScoreExpanded
        {
            get { return _securityScoreExpanded; }
            set
            {
                _securityScoreExpanded = value;
                RaisePropertyChanged("SecurityScoreExpanded");
            }
        }

        List<SecurityScoreData> _securityScoreData;
        public List<SecurityScoreData> SecurityScoreData
        {
            get { return _securityScoreData; }
            set
            {
                _securityScoreData = value;
                RaisePropertyChanged("SecurityScoreData");
            }
        }

        private bool _neverShowChecked;

        public bool NeverShowChecked
        {
            get { return _neverShowChecked; }
            set
            {
                _neverShowChecked = value;
                RaisePropertyChanged("NeverShowChecked");
            }
        }

        private bool _showUpdatePasswordInfo = true;

        public bool ShowUpdatePasswordInfo
        {
            get { return _showUpdatePasswordInfo; }
            set
            {
                _showUpdatePasswordInfo = value;
                RaisePropertyChanged("ShowUpdatePasswordInfo");
            }
        }

        private string _itemUri;

        public string ItemUri
        {
            get { return _itemUri; }
            set
            {
                _itemUri = value;
                RaisePropertyChanged("ItemUri");
            }
        }

        private bool _duplicatePasswordsExpanded;

        public bool DuplicatePasswordsExpanded
        {
            get { return _duplicatePasswordsExpanded; }
            set
            {
                _duplicatePasswordsExpanded = value;
                RaisePropertyChanged("DuplicatePasswordsExpanded");
            }
        }

        private bool _isStepExpanded;

        public bool IsStepExpanded
        {
            get { return _isStepExpanded; }
            set
            {
                _isStepExpanded = value;
                RaisePropertyChanged("IsStepExpanded");
            }
        }


        #endregion

        private void InitializeCommands()
        {
            SecurityScoreExpandedCommand = new RelayCommand(SecurityScoreExpandedEvent);
            SecurityScoreCollapsedCommand = new RelayCommand(SecurityScoreCollapsedEvent);
            SecurityScoreToggleExpanderCommand = new RelayCommand(SecurityScoreToggleExpander);
            SecurityScoreUpdateCommand = new RelayCommand(SecurityScoreUpdateClick);
            SecurityScoreCancelCommand = new RelayCommand(SecurityScoreCancelClick);
            SecurityScoreOkCommand = new RelayCommand(SecurityScoreOkClick);
            SecurityScoreEyeCommand = new RelayCommand(SecurityScoreEyeClick);
            UpdateSecurityScoreCommand = new RelayCommand(UpdateScurityButtonClick);
            DuplicatePasswordCommand = new RelayCommand(DuplicatePasswordChecked);
            WeakPasswordCommand = new RelayCommand(WeakPasswordChecked);
            OldPasswordCommand = new RelayCommand(OldPasswordChecked);
            DuplicatePasswordMouseEnterCommand = new RelayCommand(DuplicatePasswordMouseEnter);
            DuplicatePasswordMouseLeaveCommand = new RelayCommand(DuplicatePasswordMouseLeave);
            WeakPasswordMouseEnterCommand = new RelayCommand(WeakPasswordMouseEnter);
            WeakPasswordMouseLeaveCommand = new RelayCommand(WeakPasswordMouseLeave);
            OldPasswordMouseEnterCommand = new RelayCommand(OldPasswordMouseEnter);
            OldPasswordMouseLeaveCommand = new RelayCommand(OldPasswordMouseLeave);
            //MainContentLostFocusCommand = new RelayCommand(MainContentLostFocus);
            UpdatePasswordContinueCommand = new RelayCommand(UpdatePasswordContinueClick);

            Steps = new List<Step>
            {
                new DublicatePasswordsStep() {ShowCount=true, Titel=Application.Current.FindResource("SecurityScoreStep1Headline").ToString(),StepNumber=1,Color=Application.Current.FindResource("DuplicatePasswordsColor") as SolidColorBrush,Count=DuplicatePasswordCounter,
                StepContent=new DuplicatePasswords()},
                new Step() {ShowCount=true, Titel=Application.Current.FindResource("SecurityScoreStep2Headline").ToString(),StepNumber=2,Color=Application.Current.FindResource("WeakPasswordsColor") as SolidColorBrush,Count=WeakPasswordCounter,
                StepContent=new OtherPasswords()},
                new Step() {ShowCount=true, Titel=Application.Current.FindResource("SecurityScoreStep3Headline").ToString(), StepNumber=3,Color=Application.Current.FindResource("OldPasswordsColor") as SolidColorBrush,Count=OldPasswordCounter,
                StepContent=new OtherPasswords()},
                new Step() { Titel=Application.Current.FindResource("SecurityScoreStep4Headline").ToString(),StepNumber=4,
                StepContent=new ClearBrowserPasswords()},
                new Step() { Titel=Application.Current.FindResource("SecurityScoreStep5Headline").ToString(),StepNumber=5,
                StepContent=new DisablePasswords()},

            };
            Steps.ForEach(x=> x.IsStepExpandedChanged +=IsStepExpandedChanged);
        }

        private void IsStepExpandedChanged(object sender, EventArgs e)
        {
            var step = sender as Step;
            if (step == null)
                return;
            if (step.IsStepExpanded)
                foreach (var s in Steps.Where(x => x.IsStepExpanded && x != step))
                    s.IsStepExpanded = false;

            IsStepExpanded = Steps.Any(x => x.IsStepExpanded);
        }


        /// <summary>
        /// Disables visibility fo security score grid
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityScoreExpandedEvent(object obj)
        {
            //IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            //if (!featureChecker.IsEnabled(DefaultProperties.Features_SecurityScore_ShowSecurityScore))
            //{
            //    return;
            //}

            SecurityScoreDataCount();
            SecurityScoreVisibility = true;

            //var elem = (FrameworkElement)obj;
            //elem.Focus();
        }

        /// <summary>
        /// Enables visibility for security score grid
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityScoreCollapsedEvent(object obj)
        {
            SecurityScoreVisibility = false;
        }

        private void SecurityScoreToggleExpander(object obj)
        {
            if(SecurityScoreVisibility)
            {
                SecurityScoreCollapsedEvent(obj);
            }
            else
            {
                SecurityScoreExpandedEvent(obj);
            }
        }

        /// <summary>
        /// Prompt Update grid on screen
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityScoreUpdateClick(object obj)
        {
            if (obj != null)
            {
                ItemUri = obj as string;

                // Open info screen
                if (ShowUpdatePasswordInfo)
                {
                    UpdatePasswordsInfoDialog dlg = new UpdatePasswordsInfoDialog(Application.Current.MainWindow);
                    dlg.DataContext = this;
                    dlg.ShowDialog();

                    if (NeverShowChecked)
                    {
                        IPBData pbData = resolver.GetInstanceOf<IPBData>();
                        Configuration configDontShowInfoDialog = new Configuration()
                        {
                            AccountEmail = pbData.ActiveUser,
                            Key = DefaultProperties.Configuration_Key_SecurityScoreInfo,
                            Value = true.ToString()
                        };
                        pbData.AddOrUpdateConfiguration(configDontShowInfoDialog);
                    }

                    ShowUpdatePasswordInfo = !NeverShowChecked;
                }
                else
                {
                    // Only update password from browser
                    OpenSiteToUpdatePassword();
                }
            }


            
        }

        /// <summary>
        /// Hides update grid
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityScoreCancelClick(object obj)
        {
            MainpageHitTestTrueOpacity();
            SecurityUpdatePopupVisibility = false;
        }

        private void SecurityScoreOkClick(object obj)
        {
            MainpageHitTestTrueOpacity();
            SecurityUpdatePopupVisibility = false;
            BrowserHelper.OpenInDefaultBrowser(new Uri(RedirectUrl, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// SecurityScore Eye click Event 
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityScoreEyeClick(object obj)
        {
            var item = SecurityScoreData.Where(p => p.id == (string)obj).SingleOrDefault();
            if(item != null)
            {
                if (!item.PasswordVisibility)
                {
                    if(item.ReEnterPassword)
                    {
                        MasterPasswordConfirm masterPass = new MasterPasswordConfirm(pbData);
                        bool? result = masterPass.ShowDialog();
                        //TODO Validate master password before show
                        if (result.HasValue)
                            if (result.Value)
                            {
                                item.PasswordVisibility = result.Value;
                            }
                    }
                    else
                    {
                        item.PasswordVisibility = true;
                    }
                    
                }
                else
                {
                    item.PasswordVisibility = !item.PasswordVisibility;
                }
            }
        }

        /// <summary>
        /// to visible Account Setting
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateScurityButtonClick(object obj)
        {
            //AccountSettingVisibility = true;
            //DisplayXamlTab = true;
        }

        /// <summary>
        /// used to cahnge font color on  mouse leave style 
        /// </summary>
        /// <param name="obj"></param>
        private void DuplicatePasswordMouseLeave(object obj)
        {
            if (_duplicatePasswordFlag == 0)
            {
                TwoDublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.FillEllipsesColor);
                DublicatePasswordForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.SecurityScoreDynamicContentColor);
                DublicatePasswordFontWeight = DefaultProperties.ReturnFontWeight(DefaultProperties.NormalWeight);
                DublicatePasswordFontFamily = DefaultProperties.ReturnFontFamily(DefaultProperties.ProximaRegularFamily);
            }

        }

        /// <summary>
        /// used to cahnge font color on  mouse enter style 
        /// </summary>
        /// <param name="obj"></param>
        private void DuplicatePasswordMouseEnter(object obj)
        {
            DuplicatePasswordDefaultStyle();
        }

        /// <summary>
        /// used to change the forground colors of WeakPasswordbtn,
        /// OldPasswordbtn,DuplicatePasswordbtn content
        /// </summary>
        /// <param name="obj"></param>
        private void DuplicatePasswordChecked(object obj)
        {
            DefaultFontPropertySecurityScore();
            DuplicatePasswordDefaultStyle();
            SecurityScoreData = BindingSecurityScoreList(SecurityScoreItemType.duplicate);
            _duplicatePasswordFlag = 1;
            _weakPasswordFlag = 0;
            _oldPasswordFlag = 0;
        }

        /// <summary>
        /// used to change the forground colors of WeakPasswordbtn,
        /// OldPasswordbtn,DuplicatePasswordbtn content 
        /// </summary>
        /// <param name="obj"></param>
        private void WeakPasswordChecked(object obj)
        {
            DefaultFontPropertySecurityScore();
            WeakPasswordDefaultStyle();
            SecurityScoreData = BindingSecurityScoreList(SecurityScoreItemType.week);
            _duplicatePasswordFlag = 0;
            _weakPasswordFlag = 1;
            _oldPasswordFlag = 0;
        }

        /// <summary>
        /// used to change the forground colors of WeakPasswordbtn,
        /// OldPasswordbtn,DuplicatePasswordbtn content
        /// </summary>
        /// <param name="obj"></param>
        private void OldPasswordChecked(object obj)
        {
            DefaultFontPropertySecurityScore();
            OldPasswordDefaultStyle();
            SecurityScoreData = BindingSecurityScoreList(SecurityScoreItemType.old);
            _duplicatePasswordFlag = 0;
            _weakPasswordFlag = 0;
            _oldPasswordFlag = 1;
        }

        /// <summary>
        /// used to cahnge font color on mouse enter style 
        /// </summary>
        /// <param name="obj"></param>
        private void OldPasswordMouseEnter(object obj)
        {
            OldPasswordDefaultStyle();
        }

        /// <summary>
        /// used to cahnge font color on  mouse leave style 
        /// </summary>
        /// <param name="obj"></param>
        private void OldPasswordMouseLeave(object obj)
        {
            if (_oldPasswordFlag == 0)
            {
                ThreePasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.FillEllipsesColor);
                PasswordsForgroundColor = SecurityScoreHelper.ReturnWizardcolors(DefaultProperties.SecurityScoreDynamicContentColor);
                PasswordsFontFamily = DefaultProperties.ReturnFontFamily(DefaultProperties.ProximaRegularFamily);
                PasswordsFontWeight = DefaultProperties.ReturnFontWeight(DefaultProperties.NormalWeight);
            }
        }

        //private void MainContentLostFocus(object obj)
        //{
        //    if(obj != null)
        //    {
        //        bool isKeyboardFocusWithin = (bool)obj;
        //        if(!isKeyboardFocusWithin)
        //        {
        //            SecurityScoreVisibility = false;
        //        }
        //    }
        //}

        private void UpdatePasswordContinueClick(object obj)
        {
            //var window = obj as Window;
            //if(window != null)
            //{
            //    window.Close();
            //}
            OpenSiteToUpdatePassword();
            
        }

        private void OpenSiteToUpdatePassword()
        {
            if (ItemUri != null && ItemUri != string.Empty)
            {
                BrowserHelper.OpenInDefaultBrowser(new Uri(ItemUri, UriKind.RelativeOrAbsolute));
            }
            ItemUri = string.Empty;
        }
    }
}
