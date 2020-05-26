using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace PasswordBoss.ViewModel.Account
{
    [Export]
    class LoginLanguageViewModel : ViewModelBase
    {
        public Dictionary<string, string> Languages { get; set; }

        /// <summary>
        /// defining commands for UI elements
        /// </summary>
        public RelayCommand SelectionChangedCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        SystemTray _systemTray = new SystemTray();

        private const string CurrentLoginWindow = "LoginWindow";
        private IPBData pbData = null;
        private IResolver resolver = null;
        IInAppAnalytics inAppAnalyitics;

        /// <summary>
        /// login lannguage constructor to initialize command and fill combo box data
        /// </summary>
        /// <param name="loginCombobox"></param>

        public LoginLanguageViewModel(ComboBox loginCombobox, IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();

            InitializeCommands();
            FillComboboxItem(loginCombobox);
        }

        /// <summary>
        /// Initialize commands with function
        /// </summary>
        private void InitializeCommands()
        {
            SelectionChangedCommand = new RelayCommand(LanguageSelectionChanged);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        /// <summary>
        /// Fill combobox item 
        /// </summary>
        /// <param name="loginCombobox"></param>
        private void FillComboboxItem(ComboBox loginCombobox)
        {
            IPBWebAPI pbWebAPI = resolver.GetInstanceOf<IPBWebAPI>();
            var langs = pbData.GetLanguages();
            if (langs != null && langs.Count > 0)
            {
                Languages = new Dictionary<string, string>();
                foreach (var lan in langs)
                {
                    Languages.Add(lan.Code, lan.TranslationName);
                }
            }
            else
            {
                try
                {
                    dynamic languageList = pbWebAPI.GetLanguages();
                    Languages = new Dictionary<string, string>();

                    foreach (var lan in languageList.languages)
                    {
                        Languages.Add(lan.code.ToString(), lan.translated_name.ToString());
                    }
                }
                catch
                {
                }
            }
            string localUserLanguage = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (localUserLanguage != null)
            {
                SelectedLanguage = Languages.FirstOrDefault(x => x.Key.ToLower() == localUserLanguage.ToLower());
            }
            if (SelectedLanguage.Key == null || SelectedLanguage.Value == null)
            {
                SelectedLanguage = Languages.FirstOrDefault(x => x.Key.ToLower() == "en");
            }
        }

        public KeyValuePair<string, string> selectedLanguage;
        public KeyValuePair<string, string> SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }

        /// <summary>
        /// For Closing login window
        /// </summary>
        private void CloseWindow(object obj)
        {
            var window = _systemTray.CurrentWindow(CurrentLoginWindow);
            inAppAnalyitics.Get<Events.AccountCreationFlow, AccountCreationFlowItem>().Log(new AccountCreationFlowItem(1, AccountCreationFlowSteps.LanguageSelection, string.Empty, MarketingActionType.Close));
            _systemTray.WindowClose(window);
        }

        /// <summary>
        /// Language selection from combobox and binding dictionary based on selection
        /// </summary>
        /// <param name="sender"></param>
        private void LanguageSelectionChanged(object sender)
        {
            var languageComboBox = sender as ComboBox;
            if (languageComboBox == null) return;
            var selectedItem = (KeyValuePair<string, string>)languageComboBox.SelectedItem;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(selectedItem.Key);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(selectedItem.Key);

            //TODO save languag to DB 
            Configuration config = new Configuration() { AccountEmail = DefaultProperties.Configuration_DefaultAccount, Key = "last_lang", Value = selectedItem.Key };
            pbData.AddOrUpdateConfiguration(config);
            pbData.ChangeDefaultSetting("last_selected_lang", selectedItem.Key);
            ((PBApp)System.Windows.Application.Current).SetLanguage(selectedItem.Key);

            //Disable Topmost property that is required only on Login language screen
            var _loginWindowInstance = _systemTray.CurrentWindow(CurrentLoginWindow);
            _loginWindowInstance.Topmost = false;

            inAppAnalyitics.Get<Events.AccountCreationFlow, AccountCreationFlowItem>().Log(new AccountCreationFlowItem(1, AccountCreationFlowSteps.LanguageSelection, selectedItem.Value, MarketingActionType.Continue));

            var loginWindow = ((PBApp)System.Windows.Application.Current).FindWindow<LoginWindow>();
            if (loginWindow == null)
            {
                throw new ApplicationException("Login language viewmodel, login window null");
            }

            //Navigator.NavigationService.Navigate(loginWindow.Login);
            loginWindow.NavigateloginScreens(!this.pbData.AnyAccountExists() ? "AccountCreation" : null);
        }
    }
}