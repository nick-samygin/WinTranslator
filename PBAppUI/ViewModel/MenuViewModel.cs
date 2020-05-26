using System;
using System.Windows;
using System.Windows.Media;
using PasswordBoss.Helpers;
using System.Collections.Generic;
using Microsoft.Win32;
using System.ComponentModel;
using System.Threading;
using PasswordBoss.DTO;
using PasswordBoss.Views.UserControls;
using PasswordBoss.Views;
using PasswordBoss.PBAnalytics;
using System.Windows.Controls;
using System.Linq;
using PasswordBoss.Import;

namespace PasswordBoss.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
		private readonly int monitorSleepDuration = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

        private const string DefaultColor = "WhiteColor";
        private const string CheckedColor = "ImportPasswordsBackgroundColor";

        private static readonly ILogger logger = Logger.GetLogger(typeof(MenuViewModel));

        private static readonly SolidColorBrush Colorgray = new SolidColorBrush(Colors.Gray);
		private LoginsImporter loginsImporter;
        Common common = new Common();
        ImportFromBrowserControl importDialog = null;
        ImportFromApplicationControl importFromApp = null;
        ImportFromSecureExportControl importFromSecure = null;
		#region Relay commands

        public RelayCommand ImportBrowserPasswordMenuCommand { get; set; }
        public RelayCommand ImportBrowserPasswordCancelCommand { get; set; }
        public RelayCommand ImportBrowserPasswordScreen1Command { get; set; }
        public RelayCommand ImportBrowserPasswordScreen2CancelCommand { get; set; }
        public RelayCommand ImportBrowserPasswordScreen3Command { get; set; }
        public RelayCommand ImportAppPasswordMenuCommand { get; set; }
        public RelayCommand ImportAppPasswordCommand { get; set; }
        public RelayCommand ImportAppPasswordCancelCommand { get; set; }
        public RelayCommand ImportFromSecureExportMenuCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand SyncCommand { get; set; }
        public RelayCommand ExportPasswordsCommand { get; set; }
        public RelayCommand ExportSecureFileCommand { get; set; }
        public RelayCommand LocalBackupCommand { get; set; }
        public RelayCommand LocalRestoreCommand { get; set; }
        public RelayCommand CheckForUpdatesCommand { get; set; }
        public RelayCommand ManageChromeCommand { get; set; }

		public RelayCommand ManageOperaCommand { get; set; }
        public RelayCommand ManageIeCommand { get; set; }
        public RelayCommand ManageFirefoxCommand { get; set; }
        public RelayCommand SelectFileCommand { get; set; }
        public RelayCommand SelectFileSecureExportCommand { get; set; }
        public RelayCommand PasswordChangedSecureExportCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> ImportFromSecureExportCommand { get; set; }
        public RelayCommand CloseImportCommand { get; set; }
        public RelayCommand ComboboxGotFocusCommand { get; set; }
        public RelayCommand ImportFromApplicationCommand { get; set; }
        public RelayCommand ImportFromApplicationScreen3Command { get; set; }
        public RelayCommand ImportFromApplicationScreen2CancelCommand { get; set; }
        public RelayCommand ProductTourCommand { get; set; }
        public RelayCommand AccountSettingsCommand { get; set; }
        public RelayCommand PasswordBossHelpCommand { get; set; }
        public RelayCommand GettingStartedCommand { get; set; }
        public RelayCommand CommunitySupportCommand { get; set; }
        public RelayCommand EnterPromoCodeCommand { get; set; }
        public RelayCommand AboutWindowCommand { get; set; }
        public RelayCommand ImportPasswordsFromSecureExportsHelpCommand { get; set; }
        public RelayCommand ImportPasswordsFromOtherPassManagerHelpCommand { get; set; }
        public RelayCommand ExportPasswordBossToCSVCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> ImportPasswordBossFromCSVCommand { get; set; }
        public RelayCommand MessageBoxAccountIEUninstalledOkCommand { get; set; }
        public RelayCommand MessageBoxAccountFFUninstalledOkCommand { get; set; }
        public RelayCommand MessageBoxImportErrorVisibilityOKCommand { get; set; }
        public RelayCommand RefreshMenuOptions { get; set; }
        
        public RelayCommand MessageBoxExportSuccessfullCommand { get; set; }
        
        #endregion

        #region install extensions properties

        private bool _enableManageIeExt = IsIeInstalled();
        public bool EnableManageIeExt
        {
            get { return _enableManageIeExt; }
            set
            {
                if (value == _enableManageIeExt) return;

                _enableManageIeExt = value;
                RaisePropertyChanged("EnableManageIeExt");
            }
        }

        private bool _enableManageFfExt = IsFirefoxInstalled();
        public bool EnableManageFfExt
        {
            get { return _enableManageFfExt; }
            set
            {
                if (value == _enableManageFfExt) return;

                _enableManageFfExt = value;
                RaisePropertyChanged("EnableManageFfExt");
            }
        }

		private bool _enableManageOperaExt = IsOperaInstalled();
		public bool EnableManageOperaExt
		{
			get { return _enableManageOperaExt; }
			set
			{
				if (value == _enableManageOperaExt) return;

				_enableManageOperaExt = value;
				RaisePropertyChanged("EnableManageOperaExt");
			}
		}


        private bool _enableManageChromeExt = IsChromeInstalled();
        public bool EnableManageChromeExt
        {
            get { return _enableManageChromeExt; }
            set
            {
                if (value == _enableManageChromeExt) return;

                _enableManageChromeExt = value;
                RaisePropertyChanged("EnableManageChromeExt");
            }
        }

        #endregion

        #region ImportFromApplicationProperties

        private bool _gridTwoEnabled = false;
        public bool GridTwoEnabled
        {
            get { return _gridTwoEnabled; }
            set
            {
                _gridTwoEnabled = value;
                RaisePropertyChanged("GridTwoEnabled");
            }
        }

        private bool _gridThreeEnabled = false;
        public bool GridThreeEnabled
        {
            get { return _gridThreeEnabled; }
            set
            {
                _gridThreeEnabled = value;
                RaisePropertyChanged("GridThreeEnabled");
            }
        }

        private bool _importFromApplicationScreen1Visibility = true;
        public bool ImportFromApplicationScreen1Visibility
        {
            get { return _importFromApplicationScreen1Visibility; }
            set
            {
                _importFromApplicationScreen1Visibility = value;
                RaisePropertyChanged("ImportFromApplicationScreen1Visibility");
            }
        }

        private bool _importFromApplicationScreen2Visibility = false;
        public bool ImportFromApplicationScreen2Visibility
        {
            get { return _importFromApplicationScreen2Visibility; }
            set
            {
                _importFromApplicationScreen2Visibility = value;
                RaisePropertyChanged("ImportFromApplicationScreen2Visibility");
            }
        }

        private bool _importFromApplicationScreen3Visibility = false;
        public bool ImportFromApplicationScreen3Visibility
        {
            get { return _importFromApplicationScreen3Visibility; }
            set
            {
                _importFromApplicationScreen3Visibility = value;
                RaisePropertyChanged("ImportFromApplicationScreen3Visibility");
            }
        }
     

        private string _filePathText = string.Empty;
        public string FilePathText
        {
            get { return _filePathText; }
            set
            {
                _filePathText = value;
				if (_filePathText.Equals(string.Empty))
                {
                    GridThreeEnabled = false;
                }
                else
                {
                    GridThreeEnabled = true;
                    GridTwoBackground = ReturnBackgroundColor(DefaultColor);
                    GridThreeBackground = ReturnBackgroundColor(CheckedColor);
                }
                RaisePropertyChanged("FilePathText");
            }
        }
       
        Dictionary<int, string> _applicationsDataSource;
        public Dictionary<int, string> ApplicationsDataSource
        {
            get { return _applicationsDataSource; }
            set
            {
                _applicationsDataSource = value;
                RaisePropertyChanged("ApplicationsDataSource");
            }
        }

        private List<string> _testDataSource = new List<string> { "Dashline", "Password Boss 1", "Password Boss 2", "Password Boss 3", "Password Boss 4" };
        public List<string> TestDataSource
        {
            get { return _testDataSource; }
            set
            {
                _testDataSource = value;
                RaisePropertyChanged("TestDataSource");
            }
        }

        private string selectedApplicationLocalization;
        public string SelectedApplicationLocalization
        {
            get
            {
                if (String.IsNullOrWhiteSpace(selectedApplicationLocalization)) return Application.Current.FindResource("SelectFileToExportData").ToString();
                return selectedApplicationLocalization;
            }
            set
            {

                selectedApplicationLocalization = value;
                RaisePropertyChanged("SelectedApplicationLocalization");
            }
        }

		private KeyValuePair<int, string> _selectedApplication;
        public KeyValuePair<int, string> SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                //if(SelectedApplication != null && SelectedApplication != string.Empty)
                //{
                    SelectedApplicationLocalization = String.Format("{0} {1}", Application.Current.FindResource("SelectFileToExportData").ToString(), value.Value);
                    GridTwoEnabled = true;
                    GridOneBackground = ReturnBackgroundColor(DefaultColor);
                    GridTwoBackground = ReturnBackgroundColor(CheckedColor);
                    GridThreeBackground = ReturnBackgroundColor(DefaultColor);
                    FilePathText = string.Empty;
                //}
                RaisePropertyChanged("SelectedApplication");
            }
        }


        private Brush _gridOneBackground = (Brush)System.Windows.Application.Current.FindResource(CheckedColor);
        public Brush GridOneBackground
        {
            get { return _gridOneBackground; }
            set
            {
                _gridOneBackground = value;
                RaisePropertyChanged("GridOneBackground");
            }
        }

        private Brush _gridTwoBackground = (Brush)System.Windows.Application.Current.FindResource(DefaultColor);
        public Brush GridTwoBackground
        {
            get { return _gridTwoBackground; }
            set
            {
                _gridTwoBackground = value;
                RaisePropertyChanged("GridTwoBackground");
            }
        }

        private Brush _gridThreeBackground = (Brush)System.Windows.Application.Current.FindResource(DefaultColor);
        public Brush GridThreeBackground
        {
            get { return _gridThreeBackground; }
            set
            {
                _gridThreeBackground = value;
                RaisePropertyChanged("GridThreeBackground");
            }
        }
        


        #endregion

        #region Import from secure export properties

        private bool _gridTwoEnabledSecureExport = false;
        public bool GridTwoEnabledSecureExport
        {
            get { return _gridTwoEnabledSecureExport; }
            set
            {
                _gridTwoEnabledSecureExport = value;
                RaisePropertyChanged("GridTwoEnabledSecureExport");
            }
        }

        private bool _gridThreeEnabledSecureExport = false;
        public bool GridThreeEnabledSecureExport
        {
            get { return _gridThreeEnabledSecureExport; }
            set
            {
                _gridThreeEnabledSecureExport = value;
                RaisePropertyChanged("GridThreeEnabledSecureExport");
            }
        }

        private bool _importFromSecureExportScreen1Visibility = true;
        public bool ImportFromSecureExportScreen1Visibility
        {
            get { return _importFromSecureExportScreen1Visibility; }
            set
            {
                _importFromSecureExportScreen1Visibility = value;
                RaisePropertyChanged("ImportFromSecureExportScreen1Visibility");
            }
        }

        private bool _importFromSecureExportScreen2Visibility = false;
        public bool ImportFromSecureExportScreen2Visibility
        {
            get { return _importFromSecureExportScreen2Visibility; }
            set
            {
                _importFromSecureExportScreen2Visibility = value;
                RaisePropertyChanged("ImportFromSecureExportScreen2Visibility");
            }
        }

        private bool _importFromSecureExportScreen3Visibility = false;
        public bool ImportFromSecureExportScreen3Visibility
        {
            get { return _importFromSecureExportScreen3Visibility; }
            set
            {
                _importFromSecureExportScreen3Visibility = value;
                RaisePropertyChanged("ImportFromSecureExportScreen3Visibility");
            }
        }


        private string _filePathTextSecureExport = string.Empty;
        public string FilePathTextSecureExport
        {
            get { return _filePathTextSecureExport; }
            set
            {
                _filePathTextSecureExport = value;
                if (!value.Equals(String.Empty))
                {                    
                    if (_filePathTextSecureExport.Equals(string.Empty))
                    {
                        GridThreeEnabledSecureExport = false;
                    }
                    else
                    {
                        GridTwoEnabledSecureExport = true;
                        GridOneBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                        GridTwoBackgroundSecureExport = ReturnBackgroundColor(CheckedColor);
                        GridThreeBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                    }
                    RaisePropertyChanged("FilePathTextSecureExport");
                }
            }
        }

        private string _EmailSecureExport = string.Empty;
        public string EmailSecureExport
        {
            get { return _EmailSecureExport; }
            set
            {
                _EmailSecureExport = value;
                if (!value.Equals(String.Empty))
                {                    
                    if ((!String.IsNullOrWhiteSpace(value) && !String.IsNullOrWhiteSpace(PasswordSecureExport)))
                    {
                        GridThreeEnabledSecureExport = true;
                    }
                    else
                    {
                        GridTwoEnabledSecureExport = true;
                        GridThreeEnabledSecureExport = false;
                        GridOneBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                        GridTwoBackgroundSecureExport = ReturnBackgroundColor(CheckedColor);
                        GridThreeBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                    }
                    RaisePropertyChanged("EmailSecureExport");
                }
            }
        }

        private string _PasswordSecureExport = string.Empty;
        public string PasswordSecureExport
        {
            get { return _PasswordSecureExport; }
            set
            {
                _PasswordSecureExport = value;
                if (!value.Equals(String.Empty))
                {                    
                    if (!String.IsNullOrWhiteSpace(EmailSecureExport) && !String.IsNullOrWhiteSpace(value))
                    {
                        GridThreeEnabledSecureExport = true;
                    }
                    else
                    {
                        GridTwoEnabledSecureExport = true;
                        GridThreeEnabledSecureExport = false;
                        GridOneBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                        GridTwoBackgroundSecureExport = ReturnBackgroundColor(CheckedColor);
                        GridThreeBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                    }
                    RaisePropertyChanged("PasswordSecureExport");
                }
            }
        }

        Dictionary<int, string> _applicationsDataSourceSecureExport;
        public Dictionary<int, string> ApplicationsDataSourceSecureExport
        {
            get { return _applicationsDataSourceSecureExport; }
            set
            {
                _applicationsDataSourceSecureExport = value;
                RaisePropertyChanged("ApplicationsDataSourceSecureExport");
            }
        }

        private List<string> _testDataSourceSecureExport = new List<string> { "Dashline", "Password Boss 1", "Password Boss 2", "Password Boss 3", "Password Boss 4" };
        public List<string> TestDataSourceSecureExport
        {
            get { return _testDataSourceSecureExport; }
            set
            {
                _testDataSourceSecureExport = value;
                RaisePropertyChanged("TestDataSourceSecureExport");
            }
        }

        private KeyValuePair<int, string> _selectedApplicationSecureExport;
        public KeyValuePair<int, string> SelectedApplicationSecureExport
        {
            get { return _selectedApplicationSecureExport; }
            set
            {
                _selectedApplicationSecureExport = value;
                //if(SelectedApplication != null && SelectedApplication != string.Empty)
                //{
                GridTwoEnabledSecureExport = true;
                GridOneBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                GridTwoBackgroundSecureExport = ReturnBackgroundColor(CheckedColor);
                GridThreeBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                FilePathText = string.Empty;
                //}
                RaisePropertyChanged("SelectedApplicationSecureExport");
            }
        }


        private Brush _gridOneBackgroundSecureExport = (Brush)System.Windows.Application.Current.FindResource(CheckedColor);
        public Brush GridOneBackgroundSecureExport
        {
            get { return _gridOneBackgroundSecureExport; }
            set
            {
                _gridOneBackgroundSecureExport = value;
                RaisePropertyChanged("GridOneBackgroundSecureExport");
            }
        }

        private Brush _gridTwoBackgroundSecureExport = (Brush)System.Windows.Application.Current.FindResource(DefaultColor);
        public Brush GridTwoBackgroundSecureExport
        {
            get { return _gridTwoBackgroundSecureExport; }
            set
            {
                _gridTwoBackgroundSecureExport = value;
                RaisePropertyChanged("GridTwoBackgroundSecureExport");
            }
        }

        private Brush _gridThreeBackgroundSecureExport = (Brush)System.Windows.Application.Current.FindResource(DefaultColor);
        public Brush GridThreeBackgroundSecureExport
        {
            get { return _gridThreeBackgroundSecureExport; }
            set
            {
                _gridThreeBackgroundSecureExport = value;
                RaisePropertyChanged("GridThreeBackgroundSecureExport");
            }
        }
        #endregion

        #region Import from browser properties

        private bool _chromeImport = false;
        public bool ChromeImport
        {
            get { return _chromeImport; }
            set
            {
                _chromeImport = value;
                RaisePropertyChanged("ChromeImport");
            }
        }

        private bool _firefoxImport = false;
        public bool FirefoxImport
        {
            get { return _firefoxImport; }
            set
            {
                _firefoxImport = value;
                RaisePropertyChanged("FirefoxImport");
            }
        }

        private bool _explorerImport = false;
        public bool ExplorerImport
        {
            get { return _explorerImport; }
            set
            {
                _explorerImport = value;
                RaisePropertyChanged("ExplorerImport");
            }
        }



        private int _numberOfImportedPasswords;
        public int NumberOfImportedPasswords
        {
            get { return _numberOfImportedPasswords; }
            set
            {
                _numberOfImportedPasswords = value;
                RaisePropertyChanged("NumberOfImportedPasswords");
            }
        }

        private int _numberOfExportedPasswords;
        public int NumberOfExportedPasswords
        {
            get { return _numberOfExportedPasswords; }
            set
            {
                _numberOfExportedPasswords = value;
                RaisePropertyChanged("NumberOfExportedPasswords");
            }
        }

        private int _totalNumberOfPasswordsForExporting;
        public int TotalNumberOfPasswordsForExporting
        {
            get { return _totalNumberOfPasswordsForExporting; }
            set
            {
                _totalNumberOfPasswordsForExporting = value;
                RaisePropertyChanged("TotalNumberOfPasswordsForExporting");
            }
        }

        private bool _importFromOtherAppHasError;
        public bool ImportFromOtherAppHasError
        {
            get { return _importFromOtherAppHasError; }
            set
            {
                _importFromOtherAppHasError = value;
                RaisePropertyChanged("ImportFromOtherAppHasError");
            }
        }

        private int _passwordsAlreadyInPasswordBoss;
        public int PasswordsAlreadyInPasswordBoss
        {
            get { return _passwordsAlreadyInPasswordBoss; }
            set
            {
                _passwordsAlreadyInPasswordBoss = value;
                RaisePropertyChanged("PasswordsAlreadyInPasswordBoss");
            }
        }
        #endregion

        private bool _messageBoxIEUninstalledVisibility;

        public bool MessageBoxIEUninstalledVisibility
        {
            get { return _messageBoxIEUninstalledVisibility; }
            set
            {
                _messageBoxIEUninstalledVisibility = value;
                RaisePropertyChanged("MessageBoxIEUninstalledVisibility");
            }
        }

        private bool _messageBoxFFUninstalledVisibility;

        public bool MessageBoxFFUninstalledVisibility
        {
            get { return _messageBoxFFUninstalledVisibility; }
            set
            {
                _messageBoxFFUninstalledVisibility = value;
                RaisePropertyChanged("MessageBoxFFUninstalledVisibility");
            }
        }

        private bool _setupWizardVisibility = true;

        public bool SetupWizardVisibility
        {
            get { return _setupWizardVisibility; }
            set
            {
                _setupWizardVisibility = value;
                RaisePropertyChanged("SetupWizardVisibility");
            }
        }

        private bool _messageBoxExportSuccesfullVisibility;

        public bool MessageBoxExportSuccesfullVisibility
        {
            get { return _messageBoxExportSuccesfullVisibility; }
            set
            {
                _messageBoxExportSuccesfullVisibility = value;
                RaisePropertyChanged("MessageBoxExportSuccesfullVisibility");
            }
        }

        

        private bool _messageBoxImportErrorVisibility;

        public bool MessageBoxImportErrorVisibility
        {
            get { return _messageBoxImportErrorVisibility; }
            set
            {
                _messageBoxImportErrorVisibility = value;
                RaisePropertyChanged("MessageBoxImportErrorVisibility");
            }
        }

        private string _messageBoxImportErrorMessageNumberOfPasswords;

        public string MessageBoxImportErrorMessageNumberOfPasswords
        {
            get { return _messageBoxImportErrorMessageNumberOfPasswords; }
            set
            {
                _messageBoxImportErrorMessageNumberOfPasswords = value;
                RaisePropertyChanged("MessageBoxImportErrorMessageNumberOfPasswords");
            }
        }

        private string _messageBoxImportErrorMessageNumberOfPasswordsDescription;

        public string MessageBoxImportErrorMessageNumberOfPasswordsDescription
        {
            get { return _messageBoxImportErrorMessageNumberOfPasswordsDescription; }
            set
            {
                _messageBoxImportErrorMessageNumberOfPasswordsDescription = value;
                RaisePropertyChanged("MessageBoxImportErrorMessageNumberOfPasswordsDescription");
            }
        }

        

        private IResolver resolver = null;
        private IPBData pbData = null;
        private IPBSync pbSync = null;
        private IPBDataImporter pbDataImporter = null;
        public MenuViewModel()
        {
            InitializeCommands();
            InitializeData();
            SetSetupWizardVisibility();

            pbSync.OnSyncFinished += pbSync_OnSyncFinished;
        }

        private void SetSetupWizardVisibility()
        {
            bool setupWizardFinished = false;
            if (bool.TryParse(pbData.GetUserSetting(DefaultProperties.Configuration_Key_SetupWizardFinished), out setupWizardFinished))
            {
                if (setupWizardFinished)
                    SetupWizardVisibility = false;
            }
        }

        void pbSync_OnSyncFinished(bool obj)
        {
            SetSetupWizardVisibility();
        }

        private void InitializeData()
        {
            ApplicationsDataSource = pbDataImporter.SupportedApplications().OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        public void InitializeCommands()
        {
            this.resolver = ((PBApp)Application.Current).GetResolver();
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.pbDataImporter = resolver.GetInstanceOf<IPBDataImporter>();
            if (pbData == null) pbData = resolver.GetInstanceOf<IPBData>();
			if (pbSync == null) pbSync = resolver.GetInstanceOf<IPBSync>();

			loginsImporter = new LoginsImporter(pbData);

            ImportBrowserPasswordMenuCommand = new RelayCommand(ImportBrowserPasswordClick);
            ImportBrowserPasswordCancelCommand = new RelayCommand(ImportBrowserPasswordCancelClick);
            ImportBrowserPasswordScreen1Command = new RelayCommand(ImportBrowserPasswordScreen1Click);
            ImportBrowserPasswordScreen2CancelCommand = new RelayCommand(ImportBrowserPasswordScreen2CancelClick);
            ImportBrowserPasswordScreen3Command = new RelayCommand(ImportBrowserPasswordScreen3Click);
            ImportAppPasswordMenuCommand = new RelayCommand(ImportingPasswordFromOtherAppClick);
            ImportAppPasswordCommand = new RelayCommand(ImportingAppPasswordClick);
            ImportAppPasswordCancelCommand = new RelayCommand(ImportingAppPasswordCancelClick);
            ImportFromSecureExportMenuCommand = new RelayCommand(ImportFromSecureExportMenuClick);
            LogoutCommand = new RelayCommand(LogoutClick);
            ExportPasswordsCommand = new RelayCommand(ExportPasswordsClick);
            ExportSecureFileCommand = new RelayCommand(ExportSecureFileClick);
            LocalBackupCommand = new RelayCommand(LocalBackupClick);
            CheckForUpdatesCommand = new RelayCommand(CheckForUpdatesClick);
            LocalRestoreCommand = new RelayCommand(LocalRestoreClick);
			ManageOperaCommand = new RelayCommand(ManageOpera, o => EnableManageOperaExt);
            ManageChromeCommand = new RelayCommand(ManageChrome, o => EnableManageChromeExt);
            ManageFirefoxCommand = new RelayCommand(ManageFirefox, o => EnableManageFfExt);
            ManageIeCommand = new RelayCommand(ManageIe, o => EnableManageIeExt);
            SelectFileCommand = new RelayCommand(SelectFileClick);
            SelectFileSecureExportCommand = new RelayCommand(SelectFileSecureExportClick);
            PasswordChangedSecureExportCommand = new RelayCommand(PasswordChangedSecureExportClick);
            CloseImportCommand = new RelayCommand(CloseImportClick);
            ComboboxGotFocusCommand = new RelayCommand(ComboboxGotFocusClick);
            ImportFromSecureExportCommand = new AsyncRelayCommand<LoadingWindow>(ImportFromSecureExportClick);
            ImportFromApplicationCommand = new RelayCommand(ImportFromApplicationClick);
            ImportFromApplicationScreen3Command = new RelayCommand(ImportFromApplicationScreen3Click);
            ImportFromApplicationScreen2CancelCommand = new RelayCommand(ImportFromApplicationScreen2CancelClick);
            ProductTourCommand = new RelayCommand(ProductTourClick);
            AccountSettingsCommand = new RelayCommand(AccountSettingsClick);
            PasswordBossHelpCommand = new RelayCommand(PasswordBossHelpClick);
            GettingStartedCommand = new RelayCommand(GettingStartedClick);
            CommunitySupportCommand = new RelayCommand(CommunitySupportClick);
            EnterPromoCodeCommand = new RelayCommand(EnterPromoCodeClick);
            AboutWindowCommand = new RelayCommand(AboutWindowClick);
            ImportPasswordsFromSecureExportsHelpCommand = new RelayCommand(ImportPasswordsFromSecureExportsHelpClick);
            ImportPasswordsFromOtherPassManagerHelpCommand = new RelayCommand(ImportPasswordsFromOtherPassManagerHelpClick);
            ExportPasswordBossToCSVCommand = new RelayCommand(ExportPasswordBossToClick);
            ImportPasswordBossFromCSVCommand = new AsyncRelayCommand<LoadingWindow>(ImportPasswordBossFromCSVClick);
            MessageBoxAccountIEUninstalledOkCommand = new RelayCommand(MessageBoxAccountIEUninstalledOkClick);
            MessageBoxAccountFFUninstalledOkCommand = new RelayCommand(MessageBoxAccountFFUninstalledOkClick);
            MessageBoxImportErrorVisibilityOKCommand = new RelayCommand(MessageBoxImportErrorVisibilityOKClick);
            RefreshMenuOptions = new RelayCommand(RefreshBrowserMenu);
            MessageBoxExportSuccessfullCommand = new RelayCommand(MessageBoxExportSuccessfullClick);
        }

        
  
        public void RefreshBrowserMenu(object obj)
        {

            //1. Check the Firefox
            //check whether the browser has been uninstalled in the meantime
            if (!IsFirefoxInstalled())
            {
                EnableManageFfExt = false;
                InstallFireFoxExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallFfMenuOption").ToString();
            }
            else if (BrowserHelper.IsFFExtInstalled && BrowserHelper.IsFFExtEnabled)
            {
                //FF extension is enabled, change the Menu title: Uninstall FireFox extension
                EnableManageFfExt = true;
                InstallFireFoxExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("UninstallFfMenuOption").ToString();
            }
            else
            {
                //FF extension is disabled, change the  Menu title: Install FireFox extension
                EnableManageFfExt = true;
                InstallFireFoxExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallFfMenuOption").ToString();
            }

            //2. Check the Internet explorer
            //check whether the browser has been uninstalled in the meantime
            if (!IsIeInstalled())
            {
                EnableManageIeExt = false;
                InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallIeMenuOption").ToString();
            }
			else if (BrowserHelper.IsIEExtInstalled && BrowserHelper.IsIEExtEnabled)
            {
                EnableManageIeExt = true;
                InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("UninstallIeMenuOption").ToString();
            }
            else
            {
                EnableManageIeExt = true;
                InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallIeMenuOption").ToString();

            }
            //3. Check the Chrome
            //check whether the browser has been uninstalled in the meantime
            if (!IsChromeInstalled())
            {
                EnableManageChromeExt = false;
                InstallChromeMenuItemTitle = System.Windows.Application.Current.FindResource("InstallChromeMenuOption").ToString();
            }
            else if (BrowserHelper.IsChromeExtInstalled && BrowserHelper.IsChromeExtEnabled)
            {
                //Chrome extension is enabled, change the Menu title: Manage Chrome extension
                EnableManageChromeExt = true;
                InstallChromeMenuItemTitle = System.Windows.Application.Current.FindResource("ManageChromeExtensionMenuOption").ToString();
            }
            else
            {
                //Chrome extension is disabled, change the Menu title: Install Chrome extension
                EnableManageChromeExt = true;
                InstallChromeMenuItemTitle = System.Windows.Application.Current.FindResource("InstallChromeMenuOption").ToString();
            }

			//4. Check the Opera
			//check whether the browser has been uninstalled in the meantime
			InstallOperaMenuItemTitle = System.Windows.Application.Current.FindResource("InstallOperaButton").ToString();
			EnableManageOperaExt = IsOperaInstalled();

        }


             internal Brush ReturnBackgroundColor(string resource)
        {
            return (Brush)System.Windows.Application.Current.FindResource(resource);
        }


        private void ComboboxGotFocusClick(object obj)
        {
            GridOneBackground = ReturnBackgroundColor(CheckedColor);
            GridTwoBackground = ReturnBackgroundColor(DefaultColor);
            GridThreeBackground = ReturnBackgroundColor(DefaultColor);
        }

        private void SelectFileClick(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
			if (fileDialog.ShowDialog() == true)
            {
                FilePathText = fileDialog.FileName;
            }   
        }
        
        private void SelectFileSecureExportClick(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
			if (fileDialog.ShowDialog() == true)
            {
                FilePathTextSecureExport = fileDialog.FileName;

                System.Threading.ThreadPool.QueueUserWorkItem(
                   (a) =>
                   {
                       System.Threading.Thread.Sleep(100);
                       Application.Current.Dispatcher.BeginInvoke((Action)delegate
                       {
                           Control cnt = (Control)obj;
                           cnt.Focus();
                       });
                   }
                   );
            }   
        }

        private void PasswordChangedSecureExportClick(object obj)
        {
            if (!String.IsNullOrWhiteSpace(EmailSecureExport) && !String.IsNullOrWhiteSpace(PasswordSecureExport))
            {
                GridThreeEnabledSecureExport = true;
                GridOneBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                GridTwoBackgroundSecureExport = ReturnBackgroundColor(DefaultColor);
                GridThreeBackgroundSecureExport = ReturnBackgroundColor(CheckedColor);
            }
            if (obj != null)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(
                       (a) =>
                       {
                           System.Threading.Thread.Sleep(100);
                           Application.Current.Dispatcher.BeginInvoke((Action)delegate
                           {
                               Control cnt = (Control)obj;
                               cnt.Focus();
                           });
                       }
                       );
            }
        }

        

        private void CloseImportClick(object obj)
        {
            GridTwoEnabled = false;
            GridThreeEnabled = false;
            FilePathText = string.Empty;
            //SelectedApplication = null;
            GridOneBackground = ReturnBackgroundColor(CheckedColor);
            GridTwoBackground = ReturnBackgroundColor(DefaultColor);
            GridThreeBackground = ReturnBackgroundColor(DefaultColor);
            ImportingPasswordFromOtherAppVisibility = false;
        }

        private string _InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallIeMenuOption").ToString();

        public string InstallInternetExplorerExtensionMenuItemTitle
        {
            get { return _InstallInternetExplorerExtensionMenuItemTitle; }
			set
			{
                    _InstallInternetExplorerExtensionMenuItemTitle = value;
                    RaisePropertyChanged("InstallInternetExplorerExtensionMenuItemTitle"); 
                }
        }

        private void ManageIe(object o)
        {
			resolver.GetInstanceOf<IBrowserMonitor>().SleepMonitor(monitorSleepDuration);
            //Remove IE extension ONLY if They are VISIBLE on the browser toolbar!!!
            if (BrowserHelper.IsIEExtInstalled && BrowserHelper.IsIEExtEnabled)
            {
                bool _proceedWithUninstall = true;
                // To-do check if we need confirmation dialog for IE
                //if(BrowserHelper.IsIEOpened)
                //{
                //    var dictionary = new Dictionary<string, object> { { "BrowserTitle", "Ie" } };
                //    _proceedWithUninstall = ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowDialogForClosingBrowser", dictionary);
                //}
				if (_proceedWithUninstall)
                {
                    //IE ext sucessfully removed, change the menu item title
                    if (BrowserHelper.UninstallIEExt())
                    {
                        MessageBoxIEUninstalledVisibility = true;
                        InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallIeMenuOption").ToString();
                    }
                }
                
            }
            //install IE ext in any other case
            else
            {
                bool _proceedWithInstalation = true;
                // To-do check if we need confirmation dialog for IE
                //if (BrowserHelper.IsIEOpened)
                //{
                //    var dictionary = new Dictionary<string, object> { { "BrowserTitle", "Ie" } };
                //    _proceedWithInstalation = ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowDialogForClosingBrowser", dictionary);
                //}
				if (_proceedWithInstalation)
                {
                    //If the installation of IE extension was OK => change the menu item title
                    if (BrowserHelper.InstallIEExt())
                    {
                        InstallInternetExplorerExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("UninstallIeMenuOption").ToString();

                        var inApp = resolver.GetInstanceOf<IInAppAnalytics>();
                        if (inApp != null)
                        {
                            inApp.Get<Events.BrowserExtensionInstall, BrowserExtensionInstallItem>().Log(new BrowserExtensionInstallItem(Browser.InternetExplorer, BrowserExtensionInstallationSource.Menu, null));
                        }
                    }
                }
                
            }
        }

        private void ManageFirefox(object o)
        {
			resolver.GetInstanceOf<IBrowserMonitor>().SleepMonitor(monitorSleepDuration);
            //Remove FireFox extension ONLY if They are VISIBLE on the browser toolbar!!!
            if (BrowserHelper.IsFFExtInstalled && BrowserHelper.IsFFExtEnabled)
            {
                bool _proceedWithUninstall = true;
                if (BrowserHelper.IsFFOpened)
                {
                    var dictionary = new Dictionary<string, object> { { "BrowserTitle", "Firefox" } };
                    _proceedWithUninstall = ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowDialogForClosingBrowser", dictionary);
                }
				if (_proceedWithUninstall)
                {
                    if (BrowserHelper.UninstallFFExt())
                    {
                        MessageBoxFFUninstalledVisibility = true;
                    }
                }
                
            }
            else
            {
                bool _proceedWithInstalation = true;
				if (BrowserHelper.IsFFOpened)
                {
                    var dictionary = new Dictionary<string, object> { { "BrowserTitle", "Firefox" } };
                    _proceedWithInstalation = ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowDialogForClosingBrowser", dictionary);
                }
				if (_proceedWithInstalation)
                {
					if (BrowserHelper.InstallFFExt())
                    {
                        var inApp = resolver.GetInstanceOf<IInAppAnalytics>();
                        if (inApp != null)
                        {
                            inApp.Get<Events.BrowserExtensionInstall, BrowserExtensionInstallItem>().Log(new BrowserExtensionInstallItem(Browser.Firefox, BrowserExtensionInstallationSource.Menu, null));
                        }
					}
				}

                    }
                }
                
		private string _InstallOperaMenuItemTitle = System.Windows.Application.Current.FindResource("InstallOperaButton").ToString();
		public string InstallOperaMenuItemTitle
		{
			get { return _InstallOperaMenuItemTitle; }
			set
			{
				_InstallOperaMenuItemTitle = value;
				RaisePropertyChanged("InstallOperaMenuItemTitle");
            }
        }

       //default title: Install Chrome extension
        private string _InstallChromeMenuItemTitle = System.Windows.Application.Current.FindResource("InstallChromeMenuOption").ToString();
        public string InstallChromeMenuItemTitle
        {
            get { return _InstallChromeMenuItemTitle; }
			set
			{
                    _InstallChromeMenuItemTitle = value; 
                    RaisePropertyChanged("InstallChromeMenuItemTitle"); 
                }
        }

        private string _installFireFoxExtensionMenuItemTitle = System.Windows.Application.Current.FindResource("InstallFfMenuOption").ToString();
        public string InstallFireFoxExtensionMenuItemTitle
        {
            get { return _installFireFoxExtensionMenuItemTitle; }
			set
			{
                    _installFireFoxExtensionMenuItemTitle = value;
                    RaisePropertyChanged("InstallFireFoxExtensionMenuItemTitle"); 
                }
        }


        private void ManageChrome(object o)
        {
			resolver.GetInstanceOf<IBrowserMonitor>().SleepMonitor(monitorSleepDuration);
            //different links, regarding is Chrome Extension installed or not
			if (BrowserHelper.IsChromeExtInstalled)
            {
                BrowserHelper.OpenInChrome(new Uri("https://www.passwordboss.com/getting-started/manage-chrome-extension"));
            }
            else
            {
                BrowserHelper.OpenInChrome(new Uri("https://www.passwordboss.com/install/ch/?utm_source=Menu&utm_medium=Chrome&utm_campaign=InstallBHO"));
                var inApp = resolver.GetInstanceOf<IInAppAnalytics>();
				if (inApp != null)
				{
					inApp.Get<Events.BrowserExtensionInstall, BrowserExtensionInstallItem>().Log(new BrowserExtensionInstallItem(Browser.Chrome, BrowserExtensionInstallationSource.Menu, null));
            }
        }
		}

		private void ManageOpera(object o)
		{
			resolver.GetInstanceOf<IBrowserMonitor>().SleepMonitor(monitorSleepDuration);
			BrowserHelper.OpenInOpera(new Uri("https://www.passwordboss.com/getting-started/opera/"));
			var inApp = resolver.GetInstanceOf<IInAppAnalytics>();
			if (inApp != null)
			{
				inApp.Get<Events.BrowserExtensionInstall, BrowserExtensionInstallItem>().Log(new BrowserExtensionInstallItem(Browser.Opera, BrowserExtensionInstallationSource.Menu, null));
			}
		}

		private static bool IsOperaInstalled()
		{
			return Browsers.BrowserVersionGetter.GetOperaVersion() != null;
		}

        private static bool IsChromeInstalled()
        {
            return Browsers.BrowserVersionGetter.GetChromeVersion() != null;
        }

        private static bool IsFirefoxInstalled()
        {
            return Browsers.BrowserVersionGetter.GetFFVersion() != null;
        }

        private static bool IsIeInstalled()
        {
            return Browsers.BrowserVersionGetter.GetIEVersion() != null;
        }

        
        public bool IeInstalled
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetIEVersion() != null; 
            }
        }

        public bool ChromeInstalled
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetChromeVersion() != null;
            }
        }

        public bool FirefoxInstalled
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetFFVersion() != null;
            }
        }

       

        #region properties

        /// available browsers
        public bool IsIeAvailable
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetIEVersion() != null;
            }
        }

        public bool IsFirefoxAvailable
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetFFVersion() != null;
            }
        }

        public bool IsChromeAvailable
        {
            get
            {
                return Browsers.BrowserVersionGetter.GetChromeVersion() != null;
            }
        }

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

        private Brush _importingBrowserPasswordScreen1BackColor;
        public Brush ImportingBrowserPasswordScreen1BackColor
        {
            get { return _importingBrowserPasswordScreen1BackColor; }
            set
            {
                if (Equals(_importingBrowserPasswordScreen1BackColor, value)) return;
                _importingBrowserPasswordScreen1BackColor = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen1BackColor");
            }
        }

        private bool _importingBrowserPasswordScreen1Visibility;
        public bool ImportingBrowserPasswordScreen1Visibility
        {
            get { return _importingBrowserPasswordScreen1Visibility; }
            set
            {
                _importingBrowserPasswordScreen1Visibility = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen1Visibility");
            }
        }
        private bool _importingBrowserPasswordScreen2Visibility;
        public bool ImportingBrowserPasswordScreen2Visibility
        {
            get { return _importingBrowserPasswordScreen2Visibility; }
            set
            {
                _importingBrowserPasswordScreen2Visibility = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen2Visibility");
            }
        }
        private bool _importingBrowserPasswordScreen3Visibility;
        public bool ImportingBrowserPasswordScreen3Visibility
        {
            get { return _importingBrowserPasswordScreen3Visibility; }
            set
            {
                _importingBrowserPasswordScreen3Visibility = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen3Visibility");
            }
        }
        private bool _importingPasswordFromOtherAppVisibility;
        public bool ImportingPasswordFromOtherAppVisibility
        {
            get { return _importingPasswordFromOtherAppVisibility; }
            set
            {
                _importingPasswordFromOtherAppVisibility = value;
                RaisePropertyChanged("ImportingPasswordFromOtherAppVisibility");
            }
        }

        private Brush _importingBrowserPasswordScreen2BackColor;
        public Brush ImportingBrowserPasswordScreen2BackColor
        {
            get { return _importingBrowserPasswordScreen2BackColor; }
            set
            {
                if (Equals(_importingBrowserPasswordScreen2BackColor, value)) return;
                _importingBrowserPasswordScreen2BackColor = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen2BackColor");
            }
        }

        private Brush _importingBrowserPasswordScreen3BackColor;
        public Brush ImportingBrowserPasswordScreen3BackColor
        {
            get { return _importingBrowserPasswordScreen3BackColor; }
            set
            {
                if (Equals(_importingBrowserPasswordScreen3BackColor, value)) return;
                _importingBrowserPasswordScreen3BackColor = value;
                RaisePropertyChanged("ImportingBrowserPasswordScreen3BackColor");
            }
        }

        private Brush _importingPasswordFromOtherAppBackColor;
        
        public Brush ImportingPasswordFromOtherAppBackColor
        {
            get { return _importingPasswordFromOtherAppBackColor; }
            set
            {
                if (Equals(_importingPasswordFromOtherAppBackColor, value)) return;
                _importingPasswordFromOtherAppBackColor = value;
                RaisePropertyChanged("ImportingPasswordFromOtherAppBackColor");
            }
        }

        #endregion


        #region other methods

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

        /// <summary>
        /// used to enable the ImportingPasswordFromBrowserScreen1 when clicked on file menu->Imports passwords->from browser
        /// </summary>
        /// <param name="obj"></param>
        private void ImportBrowserPasswordClick(object obj)
        {
            FirefoxImport = false;
            ChromeImport = false;
            ExplorerImport = false;
			if (obj != null)
            {
                string browser = obj as string;
				switch (browser)
                {
                    case "Firefox":
                        FirefoxImport = true;
                        break;
                    case "Chrome":
                        ChromeImport = true;
                        break;
                    case "Explorer":
                        ExplorerImport = true;
                        break;
                    default:
                        break;
                }
            }

            ImportingBrowserPasswordScreen3Visibility = false;
            ImportingBrowserPasswordScreen1Visibility = true;
            importDialog = new ImportFromBrowserControl(this);
            MainWindow main = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (main != null)
            {
                //main.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                importDialog.Owner = main;
                importDialog.WindowStartupLocation = WindowStartupLocation.Manual;
                importDialog.Top = main.Top;
                importDialog.Left = main.Left;
                importDialog.Height = main.ActualHeight;
                importDialog.Width = main.ActualWidth;
                importDialog.WindowStartupLocation = main.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
                importDialog.ShowDialog();
            }
            else
            {
                logger.Error("Main window null");
            }
        }

        /// <summary>
        /// used to disable the ImportingPasswordFromBrowserScreen1 screen 
        /// </summary>
        /// <param name="obj"></param>
        private void ImportBrowserPasswordCancelClick(object obj)
        {
			if (importDialog != null)
            {
                importDialog.Close();
            }
        }







        BackgroundWorker bkgImportFromApplication;
        private int _importFromApplicationProgress;
        public int ImportFromApplicationProgress
        {
            get { return _importFromApplicationProgress; }
            set
            {
                _importFromApplicationProgress = value;
                RaisePropertyChanged("ImportFromApplicationProgress");
            }
        }

        BackgroundWorker bkgImportFromSecureExport;
        private int _importFromSecureExportProgress;
        public int ImportFromSecureExportProgress
        {
            get { return _importFromSecureExportProgress; }
            set
            {
                _importFromSecureExportProgress = value;
                RaisePropertyChanged("ImportFromSecureExportProgress");
            }
        }

        private string _importFromSecureExportErrorMessage;
        public string ImportFromSecureExportErrorMessage
        {
            get { return _importFromSecureExportErrorMessage; }
            set
            {
                _importFromSecureExportErrorMessage = value;
                RaisePropertyChanged("ImportFromSecureExportErrorMessage");
            }
        }

        private string _ImportFromSecureExportStatusMessageHeader;
        public string ImportFromSecureExportStatusMessageHeader
        {
            get { return _ImportFromSecureExportStatusMessageHeader; }
            set
            {
                _ImportFromSecureExportStatusMessageHeader = value;
                RaisePropertyChanged("ImportFromSecureExportStatusMessageHeader");
            }
        }

        private string _ImportFromSecureExportStatusMessageDetails;
        public string ImportFromSecureExportStatusMessageDetails
        {
            get { return _ImportFromSecureExportStatusMessageDetails; }
            set
            {
                _ImportFromSecureExportStatusMessageDetails = value;
                RaisePropertyChanged("ImportFromSecureExportStatusMessageDetails");
            }
        }

        private void ImportFromSecureExportClick(object obj)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate 
            {

                ImportFromSecureExportErrorMessage = "";
                int pvCountBefore = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
                int dwCountBefore = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet);
                int piCountBefore = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo);

                PasswordBox box = (PasswordBox)obj;
                if (!string.IsNullOrWhiteSpace(EmailSecureExport) && !string.IsNullOrWhiteSpace(box.Password))
                {
                    bool result = pbData.ImportStoreFromFile(FilePathTextSecureExport, true, (cred) =>
                    {
                        cred.eMail = EmailSecureExport;
                        cred.Password = box.Password;
					}, forceAskingForPassword: true);
					if (!result)
                    {
                        ImportFromSecureExportErrorMessage = (string)System.Windows.Application.Current.FindResource("ImportPasswordsFromSecureExportsErrorMessage");
                    }
                    else
                    {
                        int pvCountAfter = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
                        int dwCountAfter = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet);
                        int piCountAfter = pbData.GetSecureItemCountByType(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo);

						if (pvCountBefore == pvCountAfter
                            && dwCountBefore == dwCountAfter
                            && piCountBefore == piCountAfter)
                        {
                            ImportFromSecureExportStatusMessageHeader = (string)System.Windows.Application.Current.FindResource("ImportPasswordsFromSecureExportsNoChangeHeader");
                            ImportFromSecureExportStatusMessageDetails = (string)System.Windows.Application.Current.FindResource("ImportPasswordsFromSecureExportsNoChangeDetail");
                        }
                        else
                        {
                            ImportFromSecureExportStatusMessageHeader = (string)System.Windows.Application.Current.FindResource("ImportSuccessful");
                            ImportFromSecureExportStatusMessageDetails = (string)System.Windows.Application.Current.FindResource("ImportPasswordLatinMessage");
                        }
                        


                        ImportFromSecureExportScreen1Visibility = false;
                        ImportFromSecureExportScreen3Visibility = true;
                        ImportFromSecureExportErrorMessage = "";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("ForceReloadFromDatabase", true);
                        ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", parameters);
                    }
                }

               
               
            });

        }
        

        private void ImportFromApplicationClick(object obj)
        {
			if (bkgImportFromApplication == null)
                bkgImportFromApplication = new BackgroundWorker();
            bkgImportFromApplication.WorkerSupportsCancellation = true;
            bkgImportFromApplication.WorkerReportsProgress = true;

            bkgImportFromApplication.DoWork += bkgImportFromApplication_DoWork;
            bkgImportFromApplication.ProgressChanged += bkgImportFromApplication_ProgressChanged;
            bkgImportFromApplication.RunWorkerCompleted += bkgImportFromApplication_RunWorkerCompleted;

            bkgImportFromApplication.RunWorkerAsync();

            ImportFromApplicationScreen1Visibility = false;
            ImportFromApplicationScreen2Visibility = true;

        }

        void bkgImportFromApplication_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (e.Cancelled)
            {
                ImportFromApplicationScreen2Visibility = false;  
                ImportFromApplicationScreen1Visibility = true;
               
            }
            else
            {
                ImportFromApplicationScreen2Visibility = false;
                ImportFromApplicationScreen3Visibility = true;
            }
            //ImportFromApplicationProgress = 0;
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
        }

        void bkgImportFromApplication_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ImportFromApplicationProgress = e.ProgressPercentage;

        }

        private int _numberOfImportedPVFromApp;
		public int NumberOfImportedPVFromApp
		{
            get { return _numberOfImportedPVFromApp; } 
            set 
            {
                _numberOfImportedPVFromApp = value;
                RaisePropertyChanged("NumberOfImportedPVFromApp");
            } 
        }

        private int _numberOfImportedDWFromApp;
        public int NumberOfImportedDWFromApp
        {
            get { return _numberOfImportedDWFromApp; }
            set
            {
                _numberOfImportedDWFromApp = value;
                RaisePropertyChanged("NumberOfImportedDWFromApp");
            }
        }

        private int _numberOfImportedPIFromApp;
        public int NumberOfImportedPIFromApp
        {
            get { return _numberOfImportedPIFromApp; }
            set
            {
                _numberOfImportedPIFromApp = value;
                RaisePropertyChanged("NumberOfImportedPIFromApp");
            }
        }

        void bkgImportFromApplication_DoWork(object sender, DoWorkEventArgs e)
        {
            NumberOfImportedDWFromApp = 0;
            NumberOfImportedPIFromApp = 0;
            NumberOfImportedPVFromApp = 0;
            PasswordsAlreadyInPasswordBoss = 0;
            NumberOfImportedPasswords = 0;
            ImportFromOtherAppHasError = false;

            if (pbData == null) pbData = resolver.GetInstanceOf<IPBData>();
            if (pbSync == null) pbSync = resolver.GetInstanceOf<IPBSync>();
            List<SecureItem> secureItemList = null;
            List<string> importResultMessage = new List<string>();
            ImportPasswordsSource? source = null;
            //FilePathText
            switch (SelectedApplication.Key)
            {
                case 0:
                    secureItemList = pbDataImporter.ImportFromOnePassword(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Onepassword;
                    break;
                case 1:
                    secureItemList = pbDataImporter.ImportFromLastPass(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Lastpass;
                    break;
                case 2:
                    secureItemList = pbDataImporter.ImportFromRoboForm(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Roboform;
                    break;
                case 3:
                    secureItemList = pbDataImporter.ImportFromDashlane(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Dashlane;
                    break;
                case 4:
                    secureItemList = pbDataImporter.ImportFromKeepass(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Keepass;
                    break;
                case 5:
                    secureItemList = pbDataImporter.ImportFromOnePassword(FilePathText, out importResultMessage);
                    source = ImportPasswordsSource.Passwordbox;
                    break;
                default:
                    break;
            }
             
            bkgImportFromApplication.ReportProgress(30);
           
			for (int i = 0; i < secureItemList.Count; i++)
            {
                if (bkgImportFromApplication.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                var secureItem = secureItemList[i];

                if (secureItem.SecureItemTypeName == DefaultProperties.SecurityItemType_PasswordVault)
                {
                    if (!secureItem.Site.Uri.Contains("http") && !secureItem.Site.Uri.Contains("https"))
                        secureItem.Site.Uri = "http://" + secureItem.Site.Uri;

					if (loginsImporter.ImportSiteToDatabase(secureItem) == PasswordImport.Results.ImportResultEnum.Imported)
                        NumberOfImportedPVFromApp++;
                }
                else if (secureItem.SecureItemTypeName == DefaultProperties.SecurityItemType_DigitalWallet)
                {
                    if (pbData.AddOrUpdateSecureItem(secureItem) != null)
                        NumberOfImportedDWFromApp++;
                    
                }
                else if (secureItem.SecureItemTypeName == DefaultProperties.SecurityItemType_PersonalInfo)
                {
                    if (pbData.AddOrUpdateSecureItem(secureItem) != null)
                        NumberOfImportedPIFromApp++;

                }
                
                
                bkgImportFromApplication.ReportProgress(30 + Convert.ToInt32(Decimal.Divide(i + 1, secureItemList.Count) * 100 * (decimal)0.6));
            }
            pbSync.RegisterSites();
            bkgImportFromApplication.ReportProgress(95);
            SecureItemHelper helper = new SecureItemHelper(pbData, pbSync);

            for (int i = 0; i < secureItemList.Count; i++)
            { 
                var secureItem = secureItemList[i];

                if (secureItem.SecureItemTypeName == DefaultProperties.SecurityItemType_PasswordVault
                    && secureItem.Id != null)
                {
                    helper.UpdateSecureItem(secureItem);
                }
            }
            bkgImportFromApplication.ReportProgress(100);
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (
                    (NumberOfImportedDWFromApp == 0 &&
                        NumberOfImportedPIFromApp == 0 &&
                        NumberOfImportedPVFromApp == 0 &&
                        PasswordsAlreadyInPasswordBoss == 0 &&
                        secureItemList.Count == 0))
                {
                    ImportFromOtherAppHasError = true;
                }
            }));

			if (source.HasValue)
            {
                ImportPasswordsItem item = new ImportPasswordsItem(NumberOfImportedPVFromApp, ImportPasswordsTrigger.MenuApps, source.Value);
                var analytics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.ImportPasswords, ImportPasswordsItem>();
                analytics.Log(item);
            }

        }


        /// <summary>
        ///  used to disable the ImportingPasswordFromBrowserScreen1 screen &
        /// enable the ImportingPasswordFromBrowserScreen2 screen 
        /// </summary>
        /// <param name="obj"></param>
        BackgroundWorker bkgImportFromBrowsers;
        private int _importFromBrowserProgress;
        public int ImportFromBrowserProgress
        {
            get { return _importFromBrowserProgress; }
            set
            {
                _importFromBrowserProgress = value;
                RaisePropertyChanged("ImportFromBrowserProgress");
            }
        }
        private void ImportBrowserPasswordScreen1Click(object obj)
        {
            bkgImportFromBrowsers = new BackgroundWorker();
            bkgImportFromBrowsers.WorkerSupportsCancellation = true;
            bkgImportFromBrowsers.WorkerReportsProgress = true;

            bkgImportFromBrowsers.DoWork += bkgImportFromBrowsers_DoWork;
            bkgImportFromBrowsers.ProgressChanged += bkgImportFromBrowsers_ProgressChanged;
            bkgImportFromBrowsers.RunWorkerCompleted += bkgImportFromBrowsers_RunWorkerCompleted;

            bkgImportFromBrowsers.RunWorkerAsync(System.Windows.Threading.Dispatcher.CurrentDispatcher);

            ImportingBrowserPasswordScreen1Visibility = false;
            ImportingBrowserPasswordScreen2Visibility = true;
            ImportingBrowserPasswordScreen2BackColor = Colorgray;
            Colorgray.Opacity = 0.4;
        }

        void bkgImportFromBrowsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ImportingBrowserPasswordScreen2Visibility = false;
                ImportingBrowserPasswordScreen1Visibility = true;
                ImportingBrowserPasswordScreen1BackColor = Colorgray;
                
                Colorgray.Opacity = 0.4;
            }
            else
            {
                ImportingBrowserPasswordScreen2Visibility = false;
                ImportingBrowserPasswordScreen3Visibility = true;
                ImportingBrowserPasswordScreen3BackColor = Colorgray;
                Colorgray.Opacity = 0.4;
            }
            ImportFromBrowserProgress = 0;

            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
        }

        void bkgImportFromBrowsers_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ImportFromBrowserProgress = e.ProgressPercentage;
            
        }

        void bkgImportFromBrowsers_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Folder> categoryList = pbData.GetFoldersBySecureItemType();
            //System.Windows.Threading.Dispatcher disp = e.Argument as System.Windows.Threading.Dispatcher;
            NumberOfImportedPasswords = 0;
            PasswordsAlreadyInPasswordBoss = 0;

            if (pbData == null) pbData = resolver.GetInstanceOf<IPBData>();
            if (pbSync == null) pbSync = resolver.GetInstanceOf<IPBSync>();

            List<LoginInfo> loginList = new List<LoginInfo>();
            
            ImportPasswordsSource source = ImportPasswordsSource.InternetExplorer;

            if (ChromeImport)
            {
                List<LoginInfo> chromeLoginInfo = pbData.GetChromeAccounts();
                bkgImportFromBrowsers.ReportProgress(10);
                if (chromeLoginInfo != null)
                {
                    loginList.AddRange(chromeLoginInfo);
                }
                source = ImportPasswordsSource.Chrome;
            }

            if (ExplorerImport)
            {
                List<LoginInfo> ieLoginInfo = pbData.GetIEAccounts();
                bkgImportFromBrowsers.ReportProgress(10);
                if (ieLoginInfo != null)
                    loginList.AddRange(ieLoginInfo);
                source = ImportPasswordsSource.InternetExplorer;
            }

			if (FirefoxImport)
            {
                List<LoginInfo> ffLoginInfo = null;
                int cnt = 0;
                Thread thr = null;
                thr = new Thread(new ThreadStart(() =>
                {
                    ffLoginInfo = pbData.GetFFAccounts(() =>
                    {
                        string pass = null;
                        Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            FirefoxMasterPasswordConfirm ffmpc = new FirefoxMasterPasswordConfirm();
                            bool? ffPass = ffmpc.ShowDialog();
							if (ffPass.GetValueOrDefault())
                            {
                                pass = ffmpc.Password;
								if (cnt++ >= 3) thr.Abort();
                            }
                            else
                                thr.Abort();
                        }));
                        return pass;
                    });
                }));
                thr.Start();
                thr.Join();
                bkgImportFromBrowsers.ReportProgress(10);

				if (ffLoginInfo != null)
                    loginList.AddRange(ffLoginInfo);
                source = ImportPasswordsSource.Firefox;
            }

            SecureItemHelper helper = new SecureItemHelper(pbData, pbSync);
            int numOfImported = 0;
            int alreadyInPassBoss = 0;
            int percent = 0;
            helper.OnProgressChanged += helper_OnProgressChanged;
            helper.ImportLoginInfoList(loginList.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Distinct().ToList(), ref numOfImported, ref alreadyInPassBoss);
            NumberOfImportedPasswords = numOfImported;
            PasswordsAlreadyInPasswordBoss = alreadyInPassBoss;

            ImportPasswordsItem item = new ImportPasswordsItem(NumberOfImportedPasswords, ImportPasswordsTrigger.MenuBrowser, source);
            var analytics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.ImportPasswords, ImportPasswordsItem>();
            analytics.Log(item);

            //int percentage = 0;
            //Common common = new Common();
            //for (int i = 0; i < loginList.Count; i++)
            //{
            //    if (bkgImportFromBrowsers.CancellationPending)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //    bool alreadyExists = false;
            //    LoginInfo li = loginList[i];

            //    if (string.IsNullOrWhiteSpace(li.UserName))
            //    {
            //        continue;
            //    }
            //    string url = li.Url;
            //    if (!li.Url.StartsWith("http") && !li.Url.StartsWith("https"))
            //        url = "http://" + li.Url;


            //    var secureItem = helper.LoginInfoToSecureItem(li);

            //    if (url != null && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            //    {
            //        var siteId = helper.UpdateWithAdditionalInfoAndRegisterSite(secureItem, url);

            //        if (pbData.PasswordVaultItemExists(siteId, li.UserName, li.Password))
            //            alreadyExists = true;
            //    }
            //    else
            //    {
            //        logger.Debug("Invalid url: {0}", url);
            //    }

            //    if (alreadyExists) PasswordsAlreadyInPasswordBoss++;
            //    else if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) != null)
            //    {
            //        NumberOfImportedPasswords++;
            //    }
            //    else
            //    {
            //        logger.Debug("This isn't OK: {0}", url);
            //    }


            //    percentage = 30 + Convert.ToInt32(Decimal.Divide(i + 1, loginList.Count) * 100 * (decimal)0.7);
            //    bkgImportFromBrowsers.ReportProgress(percentage);
            //}

        }

        void helper_OnProgressChanged(int obj)
        {
            bkgImportFromBrowsers.ReportProgress(obj);
        }
       
        /// <summary>
        /// used to disable the ImportingPasswordFromBrowserScreen2 screen &
        /// enable the ImportingPasswordFromBrowserScreen3 screen 
        /// </summary>
        /// <param name="obj"></param>
        private void ImportBrowserPasswordScreen2CancelClick(object obj)
        {
           
            bkgImportFromBrowsers.CancelAsync();
			if (importDialog != null)
            {
                importDialog.Close();
            }
        }

        private void ImportFromApplicationScreen2CancelClick(object obj)
        {
           
            bkgImportFromApplication.CancelAsync();

        }

        private void ProductTourClick(object o)
        {
            var instances = resolver.GetAllInstancesOf<IDialog>();
            var pTour = instances.FirstOrDefault(p => p.ID == "ProductTour");

			if (pTour != null && o != null)
            {
                pTour.Show(Window.GetWindow(o as DependencyObject));
            }

        }

        private void AccountSettingsClick(object o)
        {
            var wMain = Window.GetWindow(o as DependencyObject) as MainWindow;
            if (wMain == null) return;
            wMain.OpenView("MainSettings");
        }

        private void PasswordBossHelpClick(object o)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppSupportMenuLink));
        }

        private void GettingStartedClick(object o)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppSupportGettingStartedLink));
        }

        private void CommunitySupportClick(object o)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppCommunitySupportLink));
        }

        private void EnterPromoCodeClick(object o)
        {
            var dep = o as DependencyObject;
            if (dep == null)
            {
                return;
            }

            EnterPromoCodeControlDialog dlg = new EnterPromoCodeControlDialog(Window.GetWindow(dep));
            dlg.DataContext = new EnterPromoCodeControlDialogViewModel(resolver);
            dlg.ShowDialog();
        }

        private void AboutWindowClick(object o)
        {
            var dep = o as DependencyObject;
            if (dep == null)
            {
                return;
            }

            AboutControlDialog dlg = new AboutControlDialog(Window.GetWindow(dep), resolver);
            dlg.ShowDialog();
        }

        void setupWizard_DialogClosed(object arg1, RoutedEventArgs arg2)
        {
            
        }

        private void ImportPasswordsFromSecureExportsHelpClick(object o)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.LinkImportFromSecureExport, UriKind.RelativeOrAbsolute));
        }

        private void ImportPasswordsFromOtherPassManagerHelpClick(object o)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.LinkImportFromOtherPasswordManager, UriKind.RelativeOrAbsolute));
        }

        private void ExportPasswordBossToClick(object o)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = "PB_Export.csv";
            fileDialog.Filter = "CSV File (.csv)|*.csv";
            fileDialog.OverwritePrompt = true;
            NumberOfExportedPasswords = 0;
            TotalNumberOfPasswordsForExporting = 0;
            if (fileDialog.ShowDialog() == true)
            {
                SecureItemHelper helper = new SecureItemHelper(pbData, pbSync);
                KeyValuePair<int, int> results = helper.ExportData(fileDialog.FileName);
                NumberOfExportedPasswords = results.Key;
                TotalNumberOfPasswordsForExporting = results.Value;
                ExportNotEncryptedWindow _infoWindow = new ExportNotEncryptedWindow(this);
                _infoWindow.ShowDialog();
            }   
        }

        private void ImportPasswordBossFromCSVClick(object o)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV File (.csv)|*.csv";
            if (fileDialog.ShowDialog() == true)
            {
                if (pbSync == null) pbSync = resolver.GetInstanceOf<IPBSync>();
                SecureItemHelper helper = new SecureItemHelper(pbData, pbSync);
                int total = 0;
                int imported = 0;
                string errorMessage = null;
                helper.ImportData(fileDialog.FileName, out total, out imported, out errorMessage);
				if (errorMessage != null || total == 0)
                {
                    MessageBoxImportErrorMessageNumberOfPasswordsDescription = errorMessage;
                    MessageBoxImportErrorMessageNumberOfPasswords = Application.Current.Resources["MessageBoxErrorTitle"].ToString();
                }
                else
                {
                    MessageBoxImportErrorMessageNumberOfPasswords = string.Format("{0} / {1}", imported, total);
                    MessageBoxImportErrorMessageNumberOfPasswordsDescription = Application.Current.Resources["PasswordsImported"].ToString();

                    ImportPasswordsItem item = new ImportPasswordsItem(imported, ImportPasswordsTrigger.MenuApps, ImportPasswordsSource.CSV);
                    var analytics = resolver.GetInstanceOf<IInAppAnalytics>().Get<Events.ImportPasswords, ImportPasswordsItem>();
                    analytics.Log(item);
                }
                //PasswordsImported
                //MessageBoxImportErrorMessageNumberOfPasswordsDescription
                MessageBoxImportErrorVisibility = true;
                
            }   
        }

        private void MessageBoxAccountIEUninstalledOkClick(object o)
        {
            MessageBoxIEUninstalledVisibility = false;
        }

        private void MessageBoxAccountFFUninstalledOkClick(object o)
        {
            MessageBoxFFUninstalledVisibility = false;
        }

        private void MessageBoxExportSuccessfullClick(object o)
        {
            MessageBoxExportSuccesfullVisibility = false;
        }

        private void MessageBoxImportErrorVisibilityOKClick(object o)
        {
            MessageBoxImportErrorVisibility = false;
        }
        

        /// <summary>
        /// used to disable the ImportingPasswordFromBrowserScreen3 screen 
        /// </summary>
        /// <param name="obj"></param>
        private void ImportBrowserPasswordScreen3Click(object obj)
        {
            MainpageHitTestTrueOpacity();
            ImportingBrowserPasswordScreen3Visibility = false;
            if (importDialog != null)
            {
                importDialog.Close();
            }
        }

        /// <summary>
        ///  used to enable the ImportingPasswordFromOtherApp screens
        /// </summary>
        /// <param name="obj"></param>
        private void ImportingPasswordFromOtherAppClick(object obj)
        {
            //MainpageHitTestFalseOpacity();
            //ImportingPasswordFromOtherAppVisibility = true;
            //ImportingPasswordFromOtherAppBackColor = Colorgray;
            //Colorgray.Opacity = 0.4;

            importFromApp = new ImportFromApplicationControl(this);
            MainWindow main = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (main != null)
            {
                importFromApp.Owner = main;
                importFromApp.Top = main.Top;
                importFromApp.Left = main.Left;
                importFromApp.Height = main.ActualHeight;
                importFromApp.Width = main.ActualWidth;
                importFromApp.WindowStartupLocation = main.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
                importFromApp.ShowDialog();
            }
            else
            {
                logger.Error("Main window null");
            }
        }

        private void ImportFromSecureExportMenuClick(object obj)
        {
            //MainpageHitTestFalseOpacity();
            //ImportingPasswordFromOtherAppVisibility = true;
            //ImportingPasswordFromOtherAppBackColor = Colorgray;
            //Colorgray.Opacity = 0.4;

            importFromSecure = new ImportFromSecureExportControl(this);
            MainWindow main = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (main != null)
            {
                importFromSecure.Owner = main;
                importFromSecure.Top = main.Top;
                importFromSecure.Left = main.Left;
                importFromSecure.Height = main.ActualHeight;
                importFromSecure.Width = main.ActualWidth;
                importFromSecure.WindowStartupLocation = main.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
                importFromSecure.ShowDialog();
            }
            else
            {
                logger.Error("Main window null!");
            }
        }


        /// <summary>
        /// used to disable the ImportingPasswordFromOtherApp screens
        /// </summary>
        /// <param name="obj"></param>
        private void ImportingAppPasswordClick(object obj)
        {
            MainpageHitTestTrueOpacity();
            ImportFromApplicationScreen2Visibility = true;
        }

        /// <summary>
        ///  used to disable the ImportingPasswordFromOtherApp screens
        /// </summary>
        /// <param name="obj"></param>
        private void ImportingAppPasswordCancelClick(object obj)
        {
            MainpageHitTestTrueOpacity();
			if (importFromApp != null)
            {
                importFromApp.Close();
            }
            if (importFromSecure != null)
            {
                importFromSecure.Close();
            }
        }

        private void ImportFromApplicationScreen3Click(object obj)
        {
            ImportFromApplicationScreen3Visibility = false;
            ImportFromSecureExportScreen3Visibility = false;
            ImportFromApplicationScreen1Visibility = true;
            ImportFromSecureExportScreen1Visibility = true;
			if (importFromApp != null)
            {
                importFromApp.Close();
            }
			if (importFromSecure != null)
            {
                importFromSecure.Close();
            }
        }

        private void LogoutClick(object obj)
        {
            ((PBApp)Application.Current).Logout();
        }

        private void ExportPasswordsClick(object obj)
        {
        }

        private void ExportSecureFileClick(object obj)
        {
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("LocalBackup", null);
        }

        private void LocalBackupClick(object obj)
        {
            MainWindow main = ((PBApp)Application.Current).FindWindow<MainWindow>();
			if (main != null)
            {
                main.OpenView("StartBackupWithUI");
            }
            
        }

        private void CheckForUpdatesClick(object obj)
        {
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("CheckForUpdates", null);
           
        }

        
        private void LocalRestoreClick(object obj)
        {
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("LocalRestore", null);
        }

        
    }
}
