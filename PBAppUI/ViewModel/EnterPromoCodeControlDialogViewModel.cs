using System;
using System.Windows;
using PasswordBoss.Helpers;
using PasswordBoss.Views;
using PasswordBoss.Views.UserControls;
using PasswordBoss.WEBApiJSON;

namespace PasswordBoss.ViewModel
{
    public class EnterPromoCodeControlDialogViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(EnterPromoCodeControlDialogViewModel));
        private IResolver resolver;
        private IPBData pbData;
        IPBWebAPI webAPI;

        private bool validPromoCode = false;

        public AsyncRelayCommand<LoadingWindow> NextButtonCommand { get; set; }
        public RelayCommand ClosePromoCodeDialogCommand { get; set; }

        private bool _enterPromoCodeScreenVisibility;
        public bool EnterPromoCodeScreenVisibility
        {
            get { return _enterPromoCodeScreenVisibility; }
            set
            {
                _enterPromoCodeScreenVisibility = value;
                RaisePropertyChanged("EnterPromoCodeScreenVisibility");
            }
        }

        private bool _promoCodeSuccessScreenVisibility;
        public bool PromoCodeSuccessScreenVisibility
        {
            get { return _promoCodeSuccessScreenVisibility; }
            set
            {
                _promoCodeSuccessScreenVisibility = value;
                RaisePropertyChanged("PromoCodeSuccessScreenVisibility");
            }
        }

        private Visibility _errorMessageVisibility = Visibility.Hidden;

        public Visibility ErrorMessageVisibility
        {
            get { return _errorMessageVisibility; }
            set
            {
                _errorMessageVisibility = value;
                RaisePropertyChanged("ErrorMessageVisibility");
            }
        }

        private string _errorMessageText;

        public string ErrorMessageText
        {
            get
            {
                return _errorMessageText;
            }
            set
            {
                _errorMessageText = value;
                RaisePropertyChanged("ErrorMessageText");
            }
        }

        private string _promotionCode;

        public string PromotionCode
        {
            get { return _promotionCode; }
            set
            {
                _promotionCode = value;
                RaisePropertyChanged("PromotionCode");
            }
        }

        public EnterPromoCodeControlDialogViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            webAPI = resolver.GetInstanceOf<IPBWebAPI>();
            EnterPromoCodeScreenVisibility = true;
            InitializeCommands();
        }

        public void InitializeCommands()
        {
            ClosePromoCodeDialogCommand = new RelayCommand(ClosePromoCodeDialogClick);
            NextButtonCommand = new AsyncRelayCommand<LoadingWindow>(NextButtonClick, completed: (obj) => NextButtonClickCompleted(obj));
        }

        public void NextButtonClick(object o)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PromotionCode))
                {
                    validPromoCode = false;
                    ShowErrorMessage(false);
                    return;
                }

                var response = webAPI.SubmitPromoCode(new SubmitPromoCodeRequest { promotion = PromotionCode.Trim() }, pbData.ActiveUser + "|" + pbData.DeviceUUID);

                if (response == null || response.error != null)
                {
                    validPromoCode = false;
                    if(response.error.code == 400)
                    {
                        ShowErrorMessage(true);
                    }
                    else
                    {
                        ShowErrorMessage(false);
                    }
                    
                    return;
                }

                validPromoCode = true;

                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    var main = Application.Current.MainWindow as MainWindow;
                    if (main != null)
                    {
                        main.OpenView("StartBackupWithoutUI");
                    }
                }));

            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                ShowErrorMessage(false);
            }
        }

        private void ShowErrorMessage(bool usedCodeError)
        {
            if(usedCodeError)
            {
                ErrorMessageText = Application.Current.FindResource("PromoCodeUsedError").ToString();
            }
            else
            {
                ErrorMessageText = Application.Current.FindResource("PromoCodeError").ToString();
            }
            ErrorMessageVisibility = Visibility.Visible;
        }

        public void NextButtonClickCompleted(object o)
        {
            if(validPromoCode)
            {
                EnterPromoCodeScreenVisibility = false;
                PromoCodeSuccessScreenVisibility = true;
            }
        }

        private void ClosePromoCodeDialogClick(object o)
        {
            var window = o as Window;
            if (window != null)
            {
                window.Close();
            }
        }

    }
}
