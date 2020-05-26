using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Helpers;
using System.Windows;
using PasswordBoss.PBAnalytics;
using System.Collections.ObjectModel;

namespace PasswordBoss.ViewModel
{
    class PasswordGeneratorViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PasswordGeneratorViewModel));
        private IInAppAnalytics inAppAnalyitics;
        private IResolver resolver = null;
        private RandomPasswordGenerator randomGenerator;
        private PasswordScanner scanner;


        public enum Strength { VERYWEAK, WEAK, GOOD, STRONG, VERY_STRONG }

        public RelayCommand CopyCommand { get; set; }
        public RelayCommand CreatePasswordCommand { get; set; }
        public RelayCommand ShowHistoryCommand { get; set; }
        public RelayCommand HideHistoryCommand { get; set; }

        public PasswordGeneratorViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            randomGenerator = new RandomPasswordGenerator();
            scanner = new PasswordScanner();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            CopyCommand = new RelayCommand(CopyButtonClick);
            CreatePasswordCommand = new RelayCommand(CreatePasswordClick);
            ShowHistoryCommand = new RelayCommand(ShowHistoryClick);
            HideHistoryCommand = new RelayCommand(HideHistoryClick);
            ShowHistoryVisibility = true;
            HideHistoryVisibility = false;
            AllGeneratedPasswords = new ObservableCollection<string>();
           
        }

        private void ShowHistoryClick(object sender)
        {
            ShowHistoryVisibility = false;
            HideHistoryVisibility = true;
        }

        private void HideHistoryClick(object sender)
        {
            ShowHistoryVisibility = true;
            HideHistoryVisibility = false;
        }


        public void CheckComboboxes()
        {
            if(LettersCheck == false && NumbersCheck == false && SymbolsCheck == false && CapitalsCheck == false)
            {
                LettersCheck = true;
            }
        }

        private void CopyButtonClick(object sender)
        {
            if (GeneratedPassword != null && GeneratedPassword != string.Empty)
            {
                if (appCmd != null) appCmd.SetClipboardText(GeneratedPassword);
            }
        }

        public void DefaultView()
        {
            LettersCheck = true;
            NumbersCheck = true;
            SymbolsCheck = true;
            CapitalsCheck = true;
            PasswordLength = 12;
            GeneratedPassword = null;
            PasswordStrengthText = string.Empty;
            PasswordStrengthValue = 0;
            ShowHistoryVisibility = true;
            HideHistoryVisibility = false;
        }

        private void CreatePasswordClick(object sender)
        {
            GeneratedPassword = randomGenerator.generatePswd(PasswordLength, CapitalsCheck, NumbersCheck, SymbolsCheck, LettersCheck);

            Strength s = (Strength)scanner.scanPassword(GeneratedPassword);
            switch (s)
            {
                case (Strength.VERYWEAK):
                    PasswordStrengthValue = 10;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                    break;
                case (Strength.WEAK):
                    PasswordStrengthValue = 25;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                    break;
                case (Strength.GOOD):
                    PasswordStrengthValue = 50;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                    break;
                case (Strength.STRONG):
                    PasswordStrengthValue = 75;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                    break;
                case (Strength.VERY_STRONG):
                    PasswordStrengthValue = 100;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                    break;
                default:
                    PasswordStrengthValue = 0;
                    PasswordStrengthText = string.Empty;
                    break;
            }
            AllGeneratedPasswords.Add(GeneratedPassword);
            var an = inAppAnalyitics.Get<Events.GeneratedPassword, PasswordGeneratorItem>();
            an.Log(new PasswordGeneratorItem(PasswordGeneratorSource.MainUI));
        }

        private int _passwordLength = 12;

        public int PasswordLength
        {
            get { return _passwordLength; }
            set
            {
                _passwordLength = value;
                RaisePropertyChanged("PasswordLength");
            }
        }
        private string _generatedPassword;

        public string GeneratedPassword
        {
            get { return _generatedPassword; }
            set
            {
                _generatedPassword = value;
                RaisePropertyChanged("GeneratedPassword");
            }
        }

        private bool _lettersCheck = true;

        public bool LettersCheck
        {
            get { return _lettersCheck; }
            set
            {
                _lettersCheck = value;
                RaisePropertyChanged("LettersCheck");
                CheckComboboxes();
            }
        }

        private bool _capitalsCheck = true;

        public bool CapitalsCheck
        {
            get { return _capitalsCheck; }
            set
            {
                _capitalsCheck = value;
                RaisePropertyChanged("CapitalsCheck");
                CheckComboboxes();
            }
        }

        private bool _numbersCheck = true;

        public bool NumbersCheck
        {
            get { return _numbersCheck; }
            set
            {
                _numbersCheck = value;
                RaisePropertyChanged("NumbersCheck");
                CheckComboboxes();
            }
        }

        private bool _symbolsCheck = true;

        public bool SymbolsCheck
        {
            get { return _symbolsCheck; }
            set
            {
                _symbolsCheck = value;
                RaisePropertyChanged("SymbolsCheck");
                CheckComboboxes();
            }
        }

        private bool _showHistoryVisibility = true;

        public bool ShowHistoryVisibility
        {
            get { return _showHistoryVisibility; }
            set
            {
                _showHistoryVisibility = value;
                RaisePropertyChanged("ShowHistoryVisibility");
            }
        }

        private bool _hideHistoryVisibility = false;

        public bool HideHistoryVisibility
        {
            get { return _hideHistoryVisibility; }
            set
            {
                _hideHistoryVisibility = value;
                RaisePropertyChanged("HideHistoryVisibility");
            }
        }

        private string _passwordStrengthText;

        public string PasswordStrengthText
        {
            get { return _passwordStrengthText; }
            set
            {
                _passwordStrengthText = value;
                RaisePropertyChanged("PasswordStrengthText");
            }
        }

        private int _passwordStrengthValue;

        public int PasswordStrengthValue
        {
            get { return _passwordStrengthValue; }
            set
            {
                _passwordStrengthValue = value;
                RaisePropertyChanged("PasswordStrengthValue");
            }
        }

        private ObservableCollection<string> _allGeneratedPasswords;
        public ObservableCollection<string> AllGeneratedPasswords
        {
            get { return _allGeneratedPasswords; }
            set
            {
                _allGeneratedPasswords = value;
                RaisePropertyChanged("AllGeneratedPasswords");
            }
        }

        
        
    }
}
