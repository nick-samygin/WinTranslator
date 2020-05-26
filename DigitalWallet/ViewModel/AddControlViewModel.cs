using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.PBAnalytics;
using System.Windows.Media.Animation;
using PasswordBoss.Views;
using PasswordBoss.Views.UserControls;
using System.Windows.Input;

namespace PasswordBoss.ViewModel
{
    public enum CreditCardColorsEnum
    {
        c_ED6B73 = 0,
        c_F9E0BC,
        c_E0D438,
        c_8DC63F,
        c_45C2F4,
        c_203AC4,
        c_793DA8,
        c_010101,
        c_696F73
    }
    internal class AddControlViewModel : ViewModelBase, IDataErrorInfo
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(AddControlViewModel));

        /// <summary>
        /// variables declarations
        /// </summary>
        internal const string focusText = "strong";
        internal const string SettingHover = "imgTabSettingHover";
        internal const string Share2 = "imgShare2";
        const string Share2Hover = "imgShare2Hover";
        const string tabSetting = "imgTabSetting";
        //private const string IssueBankOnFocusStyle = "IssueBankTextboxWithotPlaceholder";
        //private const string IssueBankLostFocusStyle = "IssueBankTextbox";
        bool gridVisibility = false;
        public enum Strength { NORMAL, WEAK, MEDIUM, STRONG, VERY_STRONG }
        private IPBData pbData = null;
        private IResolver resolver = null;
        private IInAppAnalytics inAppAnalyitics;
        public event EventHandler<RoutedEventArgs> RefreshList;

        private SecureItem secureItem;
        private SecureItem defaultSecureItem;
        private SecureItem existingSecureItem;
        private bool allowPasswordView = false;
        ShareCommon shareCommon = null;
        string currentUUID = null;

        public ObservableCollection<string> shareDurations;
        public ObservableCollection<string> ShareDurations
        {
            get { return shareDurations; }
            set
            {
                shareDurations = value;
                RaisePropertyChanged("ShareDurations");
            }
        }

        private bool _settingsChangeInvalidDialogVisibility = false;
        public bool SettingsChangeInvalidDialogVisibility
        {
            get { return _settingsChangeInvalidDialogVisibility; }
            set
            {
                _settingsChangeInvalidDialogVisibility = value;
                RaisePropertyChanged("SettingsChangeInvalidDialogVisibility");
            }
        }

        private bool isValidErrorMessageVisible = false;
        public bool IsValidErrorMessageVisible
        {
            get { return isValidErrorMessageVisible; }
            set
            {
                isValidErrorMessageVisible = value;
                RaisePropertyChanged("IsValidErrorMessageVisible");
            }
        }

        private bool _invalidShareDialogVisibility = false;
        public bool InvalidShareDialogVisibility
        {
            get
            {
                return _invalidShareDialogVisibility;
            }
            set
            {
                _invalidShareDialogVisibility = value;
                RaisePropertyChanged("InvalidShareDialogVisibility");
            }
        }


        bool _isPaypalItem;
        public bool IsPaypalItem
        {
            get
            {
                return _isPaypalItem;
            }
            set
            {
                _isPaypalItem = value;
                RaisePropertyChanged("IsPaypalItem");
            }
        }

        private string shareLabel;
        public string ShareLabel
        {
            get { return shareLabel; }
            set
            {
                shareLabel = value;
                RaisePropertyChanged("ShareLabel");
            }
        }

        private CreditCardColorsEnum creditCardColor = CreditCardColorsEnum.c_45C2F4;
        public CreditCardColorsEnum CreditCardColor
        {
            get
            {
                return creditCardColor;
            }
            set
            {
                creditCardColor = value;
                
                RaisePropertyChanged("CreditCardColor");
                if (SelectedCreditCard != null && SelectedCreditCard.HasValue)
                    SetItemImage(SelectedCreditCard.Value.Key, ((int)creditCardColor).ToString());
            }
        }
        


        public ObservableCollection<Folder> Categories { get; private set; }

        private ObservableCollection<KeyValuePair<string, string>> _countries;
        public ObservableCollection<KeyValuePair<string, string>> Countries {

            get
            {
                return _countries;
            }
            set
            {
                _countries = value;
                RaisePropertyChanged("Countries");
            }
        }
        public ObservableCollection<KeyValuePair<string, string>> CreditCards { get; private set; }

        private ObservableCollection<SecureItem> addresses;
        public ObservableCollection<SecureItem> Addresses
        {
            get { return addresses; }
            set
            {
                addresses = value;
                RaisePropertyChanged("Addresses");
               
            }
        }

        public ObservableCollection<string> ExpiresMonths { get; private set; }
        public ObservableCollection<string> ExpiresYears { get; private set; }

        private ImageSource itemImage;
        public ImageSource ItemImage
        {
            get
            {
                return itemImage;
            }
            set
            {
                itemImage = value;
                RaisePropertyChanged("ItemImage");
            }
        }

        private string expiresMonth;
        public string ExpiresMonth
        {
            get
            {
                return expiresMonth;
            }
            set
            {
                expiresMonth = value;
                RaisePropertyChanged("ExpiresMonth");
            }
        }
        private string expiresYear;
        public string ExpiresYear
        {
            get
            {
                return expiresYear;
            }
            set
            {
                expiresYear = value;
                RaisePropertyChanged("ExpiresYear");
            }
        }
        private KeyValuePair<string, string>? selectedCreditCard;
        public KeyValuePair<string, string>? SelectedCreditCard
        {
            get
            {
                return selectedCreditCard;
            }
            set
            {
                if (value.HasValue && selectedCreditCard.HasValue)
                {
                    if (selectedCreditCard.Value.Key != value.Value.Key)
                    {
                        selectedCreditCard = value;
                        RaisePropertyChanged("SelectedCreditCard");
                        if (value != null && value.HasValue)
                            SetItemImage(value.Value.Key, ((int)creditCardColor).ToString());
                    }
                }
                else
                {
                    selectedCreditCard = value;
                    RaisePropertyChanged("SelectedCreditCard");
                    if(value != null && value.HasValue)
                        SetItemImage(value.Value.Key, ((int)creditCardColor).ToString());
                }
            }
        }

        private KeyValuePair<string, string>? selectedCountry;
        public KeyValuePair<string, string>? SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                if (value.HasValue && value.Value.Key != null && selectedCountry.HasValue)
                {
                    if (selectedCountry.Value.Key != value.Value.Key)
                    {
                        selectedCountry = value;
                        RaisePropertyChanged("SelectedCountry");

                        if(!value.Value.Key.ToLower().Equals("us", StringComparison.InvariantCultureIgnoreCase))
                        {
                            IssuingDateVisibility = true;
                        }
                        else
                        {
                            IssuingDateVisibility = false;
                        }
                    }
                }
                else {
                    selectedCountry = value;
                    RaisePropertyChanged("SelectedCountry");
                    if (!value.Value.Key.ToLower().Equals("us", StringComparison.InvariantCultureIgnoreCase))
                    {
                        IssuingDateVisibility = true;
                    }
                    else
                    {
                        IssuingDateVisibility = false;
                    }
                }
            }
        }

        private Folder selectedCategory;
        public Folder SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    RaisePropertyChanged("SelectedCategory");
                }
            }
        }

        private SecureItem selectedAddress;
        public SecureItem SelectedAddress
        {
            get
            {
                return selectedAddress;
            }
            set
            {
                selectedAddress = value;
                RaisePropertyChanged("SelectedAddress");
            }
        }

        Visibility _deleteButtonVisible = Visibility.Hidden;

        public Visibility DeleteButtonVisible
        {
            get { return _deleteButtonVisible; }
            set
            {
                _deleteButtonVisible = value;
                RaisePropertyChanged("DeleteButtonVisible");
            }
        }

        List<SecuerShareData> _secuerShareData;
        public List<SecuerShareData> SecuerShareData
        {
            get { return _secuerShareData; }
            set
            {
                _secuerShareData = value;
                RaisePropertyChanged("SecuerShareData");
            }
        }

        private int expirationPeriodIndex;
        public int ExpirationPeriodIndex
        {
            get { return expirationPeriodIndex; }
            set
            {
                expirationPeriodIndex = value;
                RaisePropertyChanged("ExpirationPeriodIndex");
            }
        }

        private bool _messageBoxVisibility;

        public bool MessageBoxVisibility
        {
            get { return _messageBoxVisibility; }
            set
            {
                _messageBoxVisibility = value;
                RaisePropertyChanged("MessageBoxVisibility");
            }
        }

        private bool _creditCardSettingsChangeDialogVisibility;

        public bool CreditCardSettingsChangeDialogVisibility
        {
            get { return _creditCardSettingsChangeDialogVisibility; }
            set
            {
                _creditCardSettingsChangeDialogVisibility = value;
                RaisePropertyChanged("CreditCardSettingsChangeDialogVisibility");
            }
        }

        private bool _bankAccountSettingsChangeDialogVisibility;

        public bool BankAccountSettingsChangeDialogVisibility
        {
            get { return _bankAccountSettingsChangeDialogVisibility; }
            set
            {
                _bankAccountSettingsChangeDialogVisibility = value;
                RaisePropertyChanged("BankAccountSettingsChangeDialogVisibility");
            }
        }

        DateTime? _dateCreated;
        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                if (value.HasValue) DatesVisibility = true;
                RaisePropertyChanged("DateCreated");
            }
        }
        DateTime? _dateModified;
        public DateTime? DateModified
        {
            get { return _dateModified; }
            set
            {
                _dateModified = value;
                if (value.HasValue) DatesVisibility = true;
                RaisePropertyChanged("DateModified");
            }
        }

        bool _datesVisibility;
        public bool DatesVisibility
        {
            get
            {
                return _datesVisibility;
            }
            set
            {
                _datesVisibility = value;
                RaisePropertyChanged("DatesVisibility");
            }
        }

        public bool IsShareEnabled
        {
            get
            {
                if (SecureItem != null)
                {
                    if (SecureItem.Id != null)
                    {
                        if (Enabled)
                            return true;
                    }
                }
                return false;
            }
        }

        private void SetItemImage(string cardType, string color)
        {
            //switch (cardType)
            //{
            //    case "Master":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardMaster_" + color);
            //        break;
            //    case "Visa":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardVisa_" + color);
            //        break;
            //    case "American Express":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardAmex_" + color);
            //        break;
            //    case "JCB":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardJCB_" + color);
            //        break;
            //    case "Discover":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardDiscover_" + color);
            //        break;
            //    case "Other":
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardOther_" + color);
            //        break;
            //    default:
            //        ItemImage = (ImageSource)Application.Current.FindResource("imgCreditCardOther_" + color);
            //        break;
            //}
        }

        public bool Enabled
        {
            get
            {
                return !ReadonlySecureItem;
            }
        }

        private bool readonlySecureItem;
        public bool ReadonlySecureItem
        {
            get { return readonlySecureItem; }
            set
            {
                readonlySecureItem = value;
                RaisePropertyChanged("ReadonlySecureItem");
                RaisePropertyChanged("Enabled");
                RaisePropertyChanged("IsShareEnabled");
            }
        }

        public SecureItem SecureItem
        {
            get
            {
                return secureItem;
            }
            set
            {
                DefaultView();
                secureItem = value;
                existingSecureItem = value;
                RaisePropertyChanged("IsShareEnabled");
                if (secureItem != null)
                {
                    ReadonlySecureItem = secureItem.Readonly;
                    HideVisibility();
                    //TODO handle show data based on type
                    DateCreated = secureItem.CreatedDate;
                    DateModified = secureItem.LastModifiedDate;
                    ShareLabel = secureItem.Name;
                    switch (secureItem.Type)
                    {
                        case DefaultProperties.SecurityItemSubType_DW_CreditCard:
                            //set visibility
                            CreditCardVisibility = true;
                            IsPaypalItem = false;
                            int color = -1;
                            if (int.TryParse(secureItem.Color, out color))
                            {
                                if(color >= 0 && color <= 8)
                                    CreditCardColor = (CreditCardColorsEnum)int.Parse(secureItem.Color);
                            }
                            CreditCardNickname = secureItem.Name;


                            
                            if (secureItem.Data != null)
                            {

                                SetItemImage(secureItem.Data.cardType, secureItem.Color);
                           


                                if (secureItem.Data.country != null)
                                    SelectedCountry = Countries.SingleOrDefault(x => x.Key == secureItem.Data.country);

                                if(secureItem.Color != null)
                                {

                                }
                                NameOnCard = secureItem.Data.nameOnCard;
                                CardNumber = secureItem.Data.cardNumber;
                                if (secureItem.Data.cardType != null) SelectedCreditCard = CreditCards.SingleOrDefault(x => x.Key == secureItem.Data.cardType);
                                if (secureItem.Data.expires != null)
                                {

                                    DateTime expires = new DateTime();
                                    if (DateTime.TryParse(secureItem.Data.expires, out expires))
                                    {
                                        ExpiresMonth = expires.ToUniversalTime().Month.ToString();
                                        ExpiresYear = expires.ToUniversalTime().Year.ToString();
                                    }
                                }

                                IssueBank = secureItem.Data.issuingBank;
                                IssuingDate = SecureItem.Data.issuingDate;
                                Cvv = secureItem.Data.security_code;
                                Pin = secureItem.Data.pin;
                                if (secureItem.Data.addressRef != null)
                                    SelectedAddress = Addresses.SingleOrDefault(x => x.Id == secureItem.Data.addressRef);
                            }
                            if (secureItem.Folder != null)
                                SelectedCategory = Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id);
                            existingSecureItem = CreateCreditCardSecureItem();
                            break;
                        case DefaultProperties.SecurityItemSubType_DW_Bank:
                            //set visibility
                            BankAccountVisibility = true;
                            IsPaypalItem = false;

                            BankAccountNickname = secureItem.Name;
                            ItemImage = (ImageSource)Application.Current.FindResource("22");
                            //map data
                            if (secureItem.Data != null)
                            {
                                if (secureItem.Data.country != null)
                                    SelectedCountry = Countries.SingleOrDefault(x => x.Key == secureItem.Data.country);

                                Bank = secureItem.Data.bank_name;
                                NameOnAccount = secureItem.Data.nameOnAccount;
                                BicSwift = secureItem.Data.swift;
                                Iban = secureItem.Data.iban;
                                RoutingNumber = secureItem.Data.routingNumber;
                                AccountNumber = secureItem.Data.accountNumber;
                                BankAccountNotes = secureItem.Data.notes;
                            }
                            if (secureItem.Folder != null)
                                SelectedCategory = Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id);
                            existingSecureItem = CreateBankAccountSecureItem();
                            break;
                        case DefaultProperties.SecurityItemSubType_DW_Paypal:
                            //set visibility
                            PaypalVisibility = true;
                            IsPaypalItem = true;
                            ItemImage = (ImageSource)Application.Current.FindResource("23");
                            //map data
                            PaypalNickname = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                Username = secureItem.Data.username;
                                Password = secureItem.Data.password;
                                PaypalNotes = secureItem.Data.notes;
                                PasswordTextVisible = false;
                            }
                            if (secureItem.Folder != null)
                                SelectedCategory = Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id);
                            
                            break;
                        default:
                            break;
                    }
                    ShareCommon shareCommon = new ShareCommon(resolver);
                    SecuerShareData =  shareCommon.BindingSecureShareList(secureItem.Id);
                }

                DeleteButtonVisible = value != null ? Visibility.Visible : Visibility.Hidden;
            }
        }


        private UserControl ownerControl;

        //To enable resetting scroll viewer position
        private ScrollViewer _bankAccountScrollViewer;
        private ScrollViewer _creditCardScrollViewer;
        private ScrollViewer _paypalScrollViewer;
        private DigitalWalletContentPanel digitalWalletPanel;

        /// <summary>
        /// DigitalWalletAddControlViewModel constructer
        /// </summary>
        public AddControlViewModel(IResolver resolver, UserControl ownerControl, DigitalWalletContentPanel panel)
        {
            InitializeCommands();
            this.resolver = resolver;
            this.digitalWalletPanel = panel;
            pbData =  resolver.GetInstanceOf<IPBData>();
            shareCommon = new ShareCommon(resolver);
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            
            this.ownerControl = ownerControl;
            InitializeCollections();

            if(ownerControl != null)
            {
                try
                {
                    _bankAccountScrollViewer = this.ownerControl.FindName("BankAccountScroller") as ScrollViewer;
                    _creditCardScrollViewer = this.ownerControl.FindName("CreditCardScrollViewer") as ScrollViewer;
                    _paypalScrollViewer = this.ownerControl.FindName("PaypalScrollViewer") as ScrollViewer;
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                }
                
            }
            ExpirationPeriodIndex = 0;
            //   SecureShareStatusCommand = new RelayCommand(SecureShareStatusClick);
            //  SecureShareVisibilityCommand = new RelayCommand(SecureShareVisibilityClick);
            //       ShecureShareEyeCommand = new RelayCommand(ShecureShareEyeClick);
            //SecuerShareData=_digitalWalletAddNewItemHelper.BindingSecureShareList();            
        }

        private void InitializeCollections()
        {
            //vedo - async ?
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Categories = new ObservableCollection<Folder>(pbData.GetFoldersBySecureItemType());
                Countries = new ObservableCollection<KeyValuePair<string, string>>(pbData.GetCountries());
            });
            List<KeyValuePair<string,string>> creditCardList = new List<KeyValuePair<string,string>>();
            creditCardList.Add(new KeyValuePair<string, string>("American Express", Application.Current.FindResource("CreditCardAmericanExpress").ToString()));
            creditCardList.Add(new KeyValuePair<string, string>("Discover", Application.Current.FindResource("CreditCardDiscover").ToString()));
            creditCardList.Add(new KeyValuePair<string, string>("JCB", Application.Current.FindResource("CreditCardJCB").ToString()));
            creditCardList.Add(new KeyValuePair<string, string>("Master", Application.Current.FindResource("CreditCardMasterCard").ToString()));
            creditCardList.Add(new KeyValuePair<string, string>("Visa", Application.Current.FindResource("CreditCardVisa").ToString()));
            creditCardList.Add(new KeyValuePair<string, string>("Other", Application.Current.FindResource("CreditCardOther").ToString()));

        

            CreditCards = new ObservableCollection<KeyValuePair<string, string>>(creditCardList);

            List<string> monthList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            List<string> yearList = new List<string>();
            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 11; i++)
                yearList.Add(i.ToString());

            ExpiresMonths = new ObservableCollection<string>(monthList);
            ExpiresYears = new ObservableCollection<string>(yearList);
            InitializeAddressCollection();

            PopulateShareDurations();
        }

        public void InitializeAddressCollection()
        {
            //vedo - async ?
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    Addresses = new ObservableCollection<SecureItem>(pbData.GetSecureItemsByType(DefaultProperties.SecurityItemSubType_PI_Address));
                    addresses.Add(new SecureItem() { Id=null, Name = Application.Current.FindResource("None").ToString() });
                    RaisePropertyChanged("Addresses");

                    // When item source collection changes, selected item also needs to be updated
                    if (secureItem != null && secureItem.Data.addressRef != null)
                        SelectedAddress = Addresses.SingleOrDefault(x => x.Id == secureItem.Data.addressRef);
                    else
                        SelectedAddress = Addresses.FirstOrDefault(x => x.Id == null);
                }));
            });
        }

        private void PopulateCategories()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Categories = new ObservableCollection<Folder>(pbData.GetFoldersBySecureItemType());
                RaisePropertyChanged("Categories");
            });
        }

        private void PopulateShareDurations()
        {
            if (ShareDurations == null)
            {
                ShareDurations = new ObservableCollection<string>();
            }
            else ShareDurations.Clear();
            ShareDurations.Add(System.Windows.Application.Current.FindResource("UntilCancel").ToString());
            ShareDurations.Add(System.Windows.Application.Current.FindResource("_OneDay").ToString());
            ShareDurations.Add(System.Windows.Application.Current.FindResource("_1Week").ToString());
            ShareDurations.Add(System.Windows.Application.Current.FindResource("_1Month").ToString());
            ShareDurations.Add(System.Windows.Application.Current.FindResource("_1Year").ToString());
            RaisePropertyChanged("ShareDurations");
        }

        private void InitializeCommands()
        {
            TabSelectionChangedCommand = new RelayCommand(TabSelectionChanged);
            // ElementTextChangedCommand = new RelayCommand(ElementTextChanged);
            PasswordClickCommand = new RelayCommand(ShowPasswordClick);
            PwdBoxGotFocusCommand = new RelayCommand(PwdBoxGotFocus);
            PasswordGeneratorCreateCommand = new RelayCommand(PasswordGeneratorCreate);
            PwdBoxTextChangedCommand = new RelayCommand(PwdBoxTextChanged);
            CancelCommand = new RelayCommand(CancelButtonClick);
            ContinueCommand = new RelayCommand(ContinueSettings);
            //SaveCommand = new RelayCommand(SaveItem);
            SaveCreditCardCommand = new RelayCommand(SaveCreditCardItem);
            SaveBankAccountCommand = new RelayCommand(SaveBankAccountItem);
            SavePaypalCommand = new RelayCommand(SavePaypalItem);
            ClosePasswordGeneratorCommand = new RelayCommand(PasswordGeneratorCloseClick);
            MessageBoxConfirmCommand = new RelayCommand(MessageBoxConfirmClick);
            MessageBoxCancelCommand = new RelayCommand(MessageBoxCancelClick);
            DeleteCommand = new RelayCommand(DeleteItem);
            AddCategoryClickCommand = new RelayCommand(AddCategoryClick);
            SaveShareCommand = new AsyncRelayCommand<LoadingWindow>(SaveShare);
            InvalidShareDialogOkCommand = new RelayCommand(InvalidShareDialogOkClick);
            CopyCommand = new RelayCommand(CopyClick);

            ResendShareCommand = new RelayCommand(ResendShare);
            CancelShareCommand = new RelayCommand(CancelShare);
            RevokeShareCommand = new RelayCommand(RevokeShare);
            SendDataShareCommand = new RelayCommand(SendDataShare);
            CancelShareActionCommand = new RelayCommand(CancelShareActionClick);
           
        }

        private void CopyClick(object obj)
        {
            if(obj != null)
            {
                string parameter = obj as string;
                switch (parameter)
                {
                    case "CardNumber":
                        if(CardNumber != string.Empty)
                        {
                            if (appCmd != null) appCmd.SetClipboardText(CardNumber);
                        }
                        break;
                    case "Username":
                        if (Username != string.Empty)
                        {
                            if (appCmd != null) appCmd.SetClipboardText(Username);
                        }
                        break;
                    case "Password":
                        if (Password != string.Empty)
                        {
                            if (appCmd != null) appCmd.SetClipboardText(Password);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        

        private void MessageBoxConfirmClick(object obj)
        {
            if (SecureItem != null)
            {
                SecureItem.Active = false;
                if ((secureItem = pbData.AddOrUpdateSecureItem(SecureItem)) == null)
                {
                    MessageBoxVisibility = false;
                    MessageBox.Show("Error while saving item");
                }
                else
                {

                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(SecureItem);

                    var dw = inAppAnalyitics.Get<Events.DigitalWallet, DigitalWalletItem>();
                    
                    if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_Paypal)
                    {
                        dw.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.PayPal));
                    }

                    if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_CreditCard)
                    {
                        dw.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.CreditCard));
                    }

                    if (secureItem.Type == DefaultProperties.SecurityItemSubType_DW_Bank)
                    {
                        dw.Log(new DigitalWalletItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DWItemType.BankAccount));
                    }
                     
                    this.SecureItem = null;
                    DefaultView();
                    EventHandler<RoutedEventArgs> handler = RefreshList;
                    handler(this, null);
                }
            }

            MessageBoxVisibility = false;
        }

        private void MessageBoxCancelClick(object obj)
        {
            MessageBoxVisibility = false;
        }

        public void DeleteItem(object obj)
        {
            MessageBoxVisibility = true;
        }

        public void DeleteItem()
        {
            MessageBoxVisibility = true;
        }

        private void PasswordGeneratorCloseClick(object obj)
        {
            PasswordGeneratorGridVisibility = false;
        }

        public bool HasCreditCardModelChanged()
        {
            SecureItem secureItem = CreateCreditCardSecureItem();
            if (this.existingSecureItem != null)
            {
                if (this.existingSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }
            else if (defaultSecureItem != null)
            {
                if (defaultSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }

            return false;
        }

        private SecureItem CreateCreditCardSecureItem()
        {
            string itemName = CreditCardNickname;
            if (String.IsNullOrWhiteSpace(CreditCardNickname))
            {
                itemName = Application.Current.FindResource("CreditCardOther").ToString();
                if (selectedCreditCard.HasValue)
                    if (SelectedCreditCard.Value.Key != null)
                    {
                        if (selectedCreditCard.Value.Key == "American Express")
                            itemName = "Amex";
                        else
                            itemName = SelectedCreditCard.Value.Value;
                    }
                if (CardNumber != null)
                {
                    if (!String.IsNullOrWhiteSpace(itemName))
                        itemName += " ";
                    string tmp = string.Join(String.Empty, CardNumber.Where(Char.IsLetterOrDigit).ToArray());
                    if (tmp.Length >= 4)
                        itemName += String.Format(":**** - {0}", tmp.Substring(tmp.Length - 4, 4));
                    else
                        itemName += String.Format(":**** - {0}", tmp);
                }
            }

            SecureItem secureItem = new SecureItem()
            {
                Id = SecureItem != null ? this.SecureItem.Id : null,

                SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet,
                Name = itemName,
                Type = DefaultProperties.SecurityItemSubType_DW_CreditCard,
                Color = ((int)CreditCardColor).ToString(),
                Data = new SecureItemData()
                {
                    country = SelectedCountry.HasValue ? SelectedCountry.Value.Key : null,
                    nameOnCard = NameOnCard,
                    cardNumber = CardNumber,
                    cardType = SelectedCreditCard.HasValue ? SelectedCreditCard.Value.Key : "Other",
                    expires = ExpiresMonth != null && ExpiresYear != null ? new DateTime(int.Parse(ExpiresYear), int.Parse(ExpiresMonth), DateTime.DaysInMonth(int.Parse(ExpiresYear), int.Parse(ExpiresMonth))).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null,

                    issuingBank = IssueBank,
                    issuingDate = IssuingDate,
                    security_code = Cvv,
                    pin = Pin,
                    addressRef = SelectedAddress != null ? SelectedAddress.Id : null
                },
                Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryCreditCard) : SelectedCategory

            };

            return secureItem;
        }

        private void OnSaveComplete()
        {
            CloseAnimation();
            CreditCardSettingsChangeDialogVisibility = false;
            BankAccountSettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;

            this.SecureItem = null;
            DefaultView();
            //if(this.digitalWalletPanel != null)
            //{
            //    this.digitalWalletPanel.DigitalWalletItemsContainer.listView.SelectedItems.Clear();
            //}
            
        }

        private void CloseAnimation()
        {
            Storyboard sbClose = Application.Current.TryFindResource("StoryboardCloseNewItem") as Storyboard;
            Storyboard.SetTarget(sbClose, ownerControl);
            sbClose.Begin();
        }

        private void SaveCreditCardItem(object obj)
        {
            CreditCardSettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;
            try
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(DefaultProperties.Features_DigitalWallet_AddManageCreditCard))
                {
                    return;
                }

                if (!IsValid)
                {
                    IsValidErrorMessageVisible = true;
                    //TODO show validation message
                    return;
                }
                IsValidErrorMessageVisible = false;

                SecureItem secureItem = CreateCreditCardSecureItem();

                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                {
                    MessageBox.Show("Error while saving item");
                }
                else
                {
                    var dw = inAppAnalyitics.Get<Events.DigitalWallet, DigitalWalletItem>();
                    dw.Log(new DigitalWalletItem(SecureItemAction.Added, ApplicationSource.MainUI, DWItemType.CreditCard));

                    //update shares
                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(secureItem);

                    EventHandler<RoutedEventArgs> handler = RefreshList;
                    handler(this, null);

                   
                }

                OnSaveComplete();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                MessageBox.Show("Error while saving item");
            }
        }

        public bool HasModelChanged()
        {
            SecureItem secureItem = CreateSecureItem();

            if (secureItem == null)
            {
                return false;
            }

            if (this.existingSecureItem != null)
            {
                if (this.existingSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }
            else if (defaultSecureItem != null)
            {
                if (defaultSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }
            return false;
        }

        public SecureItem CreateSecureItem()
        {
            SecureItem _item = null;
            if(BankAccountVisibility)
            {
                _item = CreateBankAccountSecureItem();
            }
            if(CreditCardVisibility)
            {
                _item = CreateCreditCardSecureItem();
            }
            return _item;
        }

        public bool HasBankAccountModelChanged()
        {
            SecureItem secureItem = CreateBankAccountSecureItem();
            if (this.existingSecureItem != null)
            {
                if (this.existingSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }
            else if (defaultSecureItem != null)
            {
                if (defaultSecureItem.Hash != secureItem.Hash)
                {
                    return true;
                }
            }

            return false;
        }

        private SecureItem CreateBankAccountSecureItem()
        {
            SecureItem secureItem = new SecureItem()
            {
                Id = SecureItem != null ? this.SecureItem.Id : null,

                SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet,
                Name = !String.IsNullOrWhiteSpace(BankAccountNickname) ? BankAccountNickname : Bank,
                Type = DefaultProperties.SecurityItemSubType_DW_Bank,
                Data = new SecureItemData()
                {
                    country = SelectedCountry.HasValue ? SelectedCountry.Value.Key : null,
                    bank_name = Bank,
                    nameOnAccount = NameOnAccount,
                    swift = BicSwift,
                    iban = Iban,
                    routingNumber = RoutingNumber,
                    accountNumber = AccountNumber,
                    notes = BankAccountNotes
                },

                Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryBankAccount) : SelectedCategory
            };

            return secureItem;
        }

        private void SaveBankAccountItem(object obj)
        {
            BankAccountSettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_DigitalWallet_AddManageBankAccount))
            {
                return;
            }

            if (!IsValid)
            {
                IsValidErrorMessageVisible = true;
                //TODO show validation message
                return;
            }
            IsValidErrorMessageVisible = false;

            try
            {
                SecureItem secureItem = CreateBankAccountSecureItem();

                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                {
                    MessageBox.Show("Error while saving item");
                }
                else
                {
                    var dw = inAppAnalyitics.Get<Events.DigitalWallet, DigitalWalletItem>();
                    dw.Log(new DigitalWalletItem(SecureItemAction.Added, ApplicationSource.MainUI, DWItemType.BankAccount));

                    //update shares
                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(secureItem);


                    EventHandler<RoutedEventArgs> handler = RefreshList;
                    handler(this, null);
                }

                OnSaveComplete();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error while saving item");
            }
        }
        private void SavePaypalItem(object obj)
        {
            if (!IsValid)
            {
                IsValidErrorMessageVisible = true;
                //TODO show validation message
                return;
            }
            IsValidErrorMessageVisible = false;
            SettingsChangeInvalidDialogVisibility = false;
            var pwdBox = obj as System.Windows.Controls.PasswordBox;
            try
            {
                SecureItem secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,

                    SecureItemTypeName = DefaultProperties.SecurityItemType_DigitalWallet,
                    Name = !String.IsNullOrEmpty(PaypalNickname) ? PaypalNickname : Username,
                    Type = DefaultProperties.SecurityItemSubType_DW_Paypal,
                    Data = new SecureItemData()
                    {
                        username = Username,
                        password = Password,
                        notes = PaypalNotes
                    },
                    Folder = SelectedCategory,
                };

                if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                {
                    MessageBox.Show("Error while saving item");
                }
                else
                {
                    Storyboard sbClose = Application.Current.TryFindResource("StoryboardCloseNewItem") as Storyboard;
                    Storyboard.SetTarget(sbClose, ownerControl);
                    sbClose.Begin();

                    var dw = inAppAnalyitics.Get<Events.DigitalWallet, DigitalWalletItem>();
                    dw.Log(new DigitalWalletItem(SecureItemAction.Added, ApplicationSource.MainUI, DWItemType.PayPal));

                    //update shares
                    ShareCommon shareCommon = new ShareCommon(resolver);
                    shareCommon.UpdateShares(secureItem);

                    EventHandler<RoutedEventArgs> handler = RefreshList;
                    handler(this, null);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error while saving item");
            }
        }

        private void InvalidShareDialogOkClick(object obj)
        {
            InvalidShareDialogVisibility = false;
        }

        private void SaveShare(object obj)
        {
            ShareCommon share = new ShareCommon(resolver);
            List<SecuerShareData> shareData = null;
            bool isSharingWithCurrentUser = false;

            if (pbData.ActiveUser == RecipientEmail.Trim())
            {
                isSharingWithCurrentUser = true;
                //InvalidShareDialogVisibility = true;
            }
            else
            {

                shareData = share.ShareItem(RecipientEmail, Message, SecureItem, ExpirationPeriodIndex, false);
            }

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (isSharingWithCurrentUser == true)
                {
                    InvalidShareDialogVisibility = true;
                }
                else
                {

                    SecuerShareData = shareData;
                    if (SecuerShareData.Count > 0)
                    {
                        DefaultViewShare();
                        OnSaveComplete();
                    }
                }

            }));

        }

        # region RealyCommands
        /// <summary>
        /// relay commands defination
        /// </summary>        
        public RelayCommand IssueBankGotFocusCommand { get; set; }
        public RelayCommand PasswordClickCommand { get; set; }
        public RelayCommand TabSelectionChangedCommand { get; set; }
        public RelayCommand ElementTextChangedCommand { get; set; }
        public RelayCommand PwdBoxGotFocusCommand { get; set; }
        public RelayCommand PasswordGeneratorCreateCommand { get; set; }
        public RelayCommand PwdBoxTextChangedCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }
       // public RelayCommand SaveCommand { get; set; }
        public RelayCommand SaveCreditCardCommand { get; set; }
        public RelayCommand SavePaypalCommand { get; set; }
        public RelayCommand SaveBankAccountCommand { get; set; }
        public RelayCommand SecureShareStatusCommand { get; set; }
        public RelayCommand SecureShareVisibilityCommand { get; set; }
        public RelayCommand ShecureShareEyeCommand { get; set; }
        public RelayCommand ClosePasswordGeneratorCommand { get; set; }
        public RelayCommand MessageBoxConfirmCommand { get; set; }
        public RelayCommand MessageBoxCancelCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddCategoryClickCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> SaveShareCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }


        public RelayCommand ResendShareCommand { get; set; }
        public RelayCommand CancelShareCommand { get; set; }
        public RelayCommand RevokeShareCommand { get; set; }
        public RelayCommand SendDataShareCommand { get; set; }
        public RelayCommand InvalidShareDialogOkCommand { get; set; }
        public RelayCommand CancelShareActionCommand { get; set; }
        # endregion
        
     //   AddControlHelper _addControlHelper = new AddControlHelper();
     //   DigitalWalletAddNewItemHelper _digitalWalletAddNewItemHelper = new DigitalWalletAddNewItemHelper();
       
        #region Properties

        ImageSource _passwordEyeImage;
        public ImageSource PasswordEyeImage
        {
            get { return _passwordEyeImage; }
            set
            {
                _passwordEyeImage = value;
                RaisePropertyChanged("PasswordEyeImage");
            }
        }

        bool _passwordTextVisible;
        public bool PasswordTextVisible
        {
            get { return _passwordTextVisible; }
            set
            {
                _passwordTextVisible = value;
                RaisePropertyChanged("PasswordTextVisible");
                if (_passwordTextVisible)
                {
                    PasswordEyeImage = (ImageSource)System.Windows.Application.Current.FindResource("imgEyeClose");
                }
                else
                {
                    PasswordEyeImage = (ImageSource)System.Windows.Application.Current.FindResource("imgEyeHoverClose");
                }
            }
        }

        ///ProgressbarGenerater Value property
        bool _passwordGeneratorGridVisibility;
        public bool PasswordGeneratorGridVisibility
        {
            get { return _passwordGeneratorGridVisibility; }
            set
            {
                _passwordGeneratorGridVisibility = value;
                RaisePropertyChanged("PasswordGeneratorGridVisibility");
            }
        }

        ///ProgressbarGenerater Value property
        double _progressbarGeneraterValue = 0;
        public double ProgressbarGeneraterValue
        {
            get { return _progressbarGeneraterValue; }
            set
            {
                _progressbarGeneraterValue = value;
                RaisePropertyChanged("ProgressbarGeneraterValue");
            }
        }
        int _selectedIndexTabControl;
        public int SelectedIndexTabControl
        {
            get { return _selectedIndexTabControl; }
            set
            {
                _selectedIndexTabControl = value;
                RaisePropertyChanged("SelectedIndexTabControl");
            }
        }
        string _recipientEmail = "";
        public string RecipientEmail
        {
            get { return _recipientEmail; }
            set
            {
                _recipientEmail = value;
                RaisePropertyChanged("RecipientEmail");
            }
        }
        string _message = "";
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        string _passwordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
        public string PasswordStrengthText
        {
            get { return _passwordStrengthText; }
            set
            {
                _passwordStrengthText = value;
                RaisePropertyChanged("PasswordStrengthText");
            }
        }

        string _creditCardNickname = "";
        public string CreditCardNickname
        {
            get { return _creditCardNickname; }
            set
            {
                _creditCardNickname = value;
                RaisePropertyChanged("CreditCardNickname");
            }
        }
        string _nameOnCard = "";
        public string NameOnCard
        {
            get { return _nameOnCard; }
            set
            {
                _nameOnCard = value;
                RaisePropertyChanged("NameOnCard");
            }
        }
        string _cardNumber = "";
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                RaisePropertyChanged("CardNumber");
            }
        }

        bool _issuingDateVisibility;
        public bool IssuingDateVisibility
        {
            get { return _issuingDateVisibility; }
            set 
            { 
                _issuingDateVisibility = value;
                RaisePropertyChanged("IssuingDateVisibility");
            }
        }

        string _issuingDate = "";
        public string IssuingDate
        {
            get { return _issuingDate; }
            set
            {
                _issuingDate = value;
                RaisePropertyChanged("IssuingDate");
            }
        }

        string _issueBank = "";
        public string IssueBank
        {
            get { return _issueBank; }
            set
            {
                _issueBank = value;
                RaisePropertyChanged("IssueBank");
            }
        }
        string _cvv = "";
        public string Cvv
        {
            get { return _cvv; }
            set
            {
                _cvv = value;
                RaisePropertyChanged("Cvv");
            }
        }
        string _pin = "";
        public string Pin
        {
            get { return _pin; }
            set
            {
                _pin = value;
                RaisePropertyChanged("Pin");
            }
        }
        string _bankAccountNickname = "";
        public string BankAccountNickname
        {
            get { return _bankAccountNickname; }
            set
            {
                _bankAccountNickname = value;
                RaisePropertyChanged("BankAccountNickname");
            }
        }
        string _bank = "";
        public string Bank
        {
            get { return _bank; }
            set
            {
                _bank = value;
                RaisePropertyChanged("Bank");
            }
        }
        string _nameOnAccount = "";
        public string NameOnAccount
        {
            get { return _nameOnAccount; }
            set
            {
                _nameOnAccount = value;
                RaisePropertyChanged("NameOnAccount");
            }
        }
        string _bicSwift = "";
        public string BicSwift
        {
            get { return _bicSwift; }
            set
            {
                _bicSwift = value;
                RaisePropertyChanged("BicSwift");
            }
        }
        string _iban = "";
        public string Iban
        {
            get { return _iban; }
            set
            {
                _iban = value;
                RaisePropertyChanged("Iban");
            }
        }
        string _routingNumber = "";
        public string RoutingNumber
        {
            get { return _routingNumber; }
            set
            {
                _routingNumber = value;
                RaisePropertyChanged("RoutingNumber");
            }
        }

        string _accountNumber = "";
        public string AccountNumber
        {
            get { return _accountNumber; }
            set
            {
                _accountNumber = value;
                RaisePropertyChanged("AccountNumber");
            }
        }

        string _bankAccountNotes = "";
        public string BankAccountNotes
        {
            get { return _bankAccountNotes; }
            set
            {
                _bankAccountNotes = value;
                RaisePropertyChanged("BankAccountNotes");
            }
        }
        string _paypalNickname = "";
        public string PaypalNickname
        {
            get { return _paypalNickname; }
            set
            {
                _paypalNickname = value;
                if (!String.IsNullOrWhiteSpace(value))
                {
                    ValidatedPropertiesPayPal = new string[] { };
                }
                else if (String.IsNullOrWhiteSpace(value) && String.IsNullOrWhiteSpace(Username))
                {
                    ValidatedPropertiesPayPal = new string[]{"PaypalNickname", "Username" };
                }
                RaisePropertyChanged("PaypalNickname");
            }
        }
        string _username = "";
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                if (!String.IsNullOrWhiteSpace(value))
                {
                    ValidatedPropertiesPayPal = new string[]{};
                }
                else if (String.IsNullOrWhiteSpace(value) && String.IsNullOrWhiteSpace(PaypalNickname))
                {
                    ValidatedPropertiesPayPal = new string[] { "PaypalNickname", "Username" };
                }
                RaisePropertyChanged("Username");
            }
        }

        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
                ScanPassword();
            }
        }

        int _passwordLength = 12;
        public int PasswordLength
        {
            get { return _passwordLength; }
            set
            {
                _passwordLength = value;
                RaisePropertyChanged("PasswordLength");
            }
        }

        string _paypalNotes = "";
        public string PaypalNotes
        {
            get { return _paypalNotes; }
            set
            {
                _paypalNotes = value;
                RaisePropertyChanged("PaypalNotes");
            }
        }
        bool _letters = true;
        public bool Letters
        {
            get { return _letters; }
            set
            {
                _letters = value;
                RaisePropertyChanged("Letters");
                CheckComboboxProperties();
            }
        }
        bool _capitals = true;
        public bool Capitals
        {
            get { return _capitals; }
            set
            {
                _capitals = value;
                RaisePropertyChanged("Capitals");
                CheckComboboxProperties();
            }
        }
        bool _numbers = true;
        public bool Numbers
        {
            get { return _numbers; }
            set
            {
                _numbers = value;
                RaisePropertyChanged("Numbers");
                CheckComboboxProperties();
            }
        }
        bool _symbols = true;
        public bool Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
                RaisePropertyChanged("Symbols");
                CheckComboboxProperties();
            }
        }
        private ImageSource _settingTabIcon = DefaultProperties.ReturnImage(SettingHover);
        public ImageSource SettingTabIcon
        {
            get { return _settingTabIcon; }
            set
            {
                if (Equals(_settingTabIcon, value)) return;
                _settingTabIcon = value;
                RaisePropertyChanged("SettingTabIcon");
            }
        }
        private ImageSource _secureShareTabIcon = DefaultProperties.ReturnImage(Share2);
        public ImageSource SecureShareTabIcon
        {
            get { return _secureShareTabIcon; }
            set
            {
                if (Equals(_secureShareTabIcon, value)) return;
                _secureShareTabIcon = value;
                RaisePropertyChanged("SecureShareTabIcon");
            }
        }


        private bool _radioBtnChecking = false;
        public bool RadioBtnChecking
        {
            get { return _radioBtnChecking; }
            set
            {
                _radioBtnChecking = value;
                RaisePropertyChanged("RadioBtnChecking");
            }
        }
        private bool _radioBtnCheced=true;
        public bool RadioBtnCheced
        {
            get { return _radioBtnCheced; }
            set
            {
                _radioBtnCheced = value;
                RaisePropertyChanged("RadioBtnCheced");
            }
        }


        #region grid visibility
        private bool _creditCardVisibility;
        public bool CreditCardVisibility
        {
            get { return _creditCardVisibility; }
            set
            {
                _creditCardVisibility = value;
                if (_creditCardVisibility)
                {
                    InitializeAddressCollection();
                }

                RaisePropertyChanged("CreditCardVisibility");
            }
        }

        private bool _paypalVisibility;
        public bool PaypalVisibility
        {
            get { return _paypalVisibility; }
            set
            {
                _paypalVisibility = value;
                RaisePropertyChanged("PaypalVisibility");
            }
        }

        private bool _bankAccountVisibility;
        public bool BankAccountVisibility
        {
            get { return _bankAccountVisibility; }
            set
            {
                _bankAccountVisibility = value;
                RaisePropertyChanged("BankAccountVisibility");
            }
        }
        #endregion
        #endregion


        #region methods

        private void ScanPassword()
        {
            PasswordGeneratorGridVisibility = false;
            if (Password == string.Empty)
            {
                ProgressbarGeneraterValue = 0;
                PasswordStrengthText = string.Empty;
            }
            else
            {
                PasswordScanner scanner = new PasswordScanner();
                Strength s = (Strength)scanner.scanPassword(Password);
                switch (s)
                {
                    case (Strength.WEAK):
                        ProgressbarGeneraterValue = 10;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                        break;
                    case (Strength.NORMAL):
                        ProgressbarGeneraterValue = 25;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                        break;
                    case (Strength.MEDIUM):
                        ProgressbarGeneraterValue = 50;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                        break;
                    case (Strength.STRONG):
                        ProgressbarGeneraterValue = 75;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                        break;
                    case (Strength.VERY_STRONG):
                        ProgressbarGeneraterValue = 100;
                        PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                        break;
                    default:
                        ProgressbarGeneraterValue = 0;
                        PasswordStrengthText = string.Empty;
                        break;
                }
            }
        }

        private void AddCategoryClick(object param)
        {
            CategoryBox catDialog = new CategoryBox();
            bool? result = catDialog.ShowDialog();
            if (result.HasValue)
                if (result.Value)
                {
                    string newCode = pbData.AddFolder(catDialog.NewCategory, catDialog.UseSecureBrowser);
                    if (newCode != null)
                    {
                        Categories = new ObservableCollection<Folder>(pbData.GetFoldersBySecureItemType());
                        RaisePropertyChanged("Categories");
                        SelectedCategory = Categories.SingleOrDefault(x => x.Id == newCode);
                    }

                }
        }
        private void CheckComboboxProperties()
        {
            if (Numbers == false && Letters == false && Symbols == false && Capitals == false)
                Letters = true;
        }

        /// <summary>
        /// Ths event hides PwdGeneratorGrid for now
        /// </summary>
        /// <param name="obj"></param>
        private void PasswordGeneratorCreate(object obj)
        {
            PasswordScanner scanner = new PasswordScanner();
            RandomPasswordGenerator passwordGenerator = new RandomPasswordGenerator();
            var passwordBox = obj as PasswordBox;
            String generatedPassword = passwordGenerator.generatePswd(PasswordLength, Capitals, Numbers, Symbols, Letters);
            passwordBox.Password = generatedPassword;
            
            Strength s = (Strength)scanner.scanPassword(generatedPassword);
            switch (s)
            {
                case (Strength.WEAK):
                    ProgressbarGeneraterValue = 10;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                    break;
                case (Strength.NORMAL):
                    ProgressbarGeneraterValue = 25;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                    break;
                case (Strength.MEDIUM):
                    ProgressbarGeneraterValue = 50;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                    break;
                case (Strength.STRONG):
                    ProgressbarGeneraterValue = 75;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                    break;
                case (Strength.VERY_STRONG):
                    ProgressbarGeneraterValue = 100;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                    break;
                default:
                    ProgressbarGeneraterValue = 0;
                    PasswordStrengthText = string.Empty;
                    break;
            }
            
            PasswordGeneratorGridVisibility = false;
        }

        /// <summary>
        /// Ths event shows PwdGeneratorGrid
        /// </summary>
        /// <param name="obj"></param>
        private void PwdBoxGotFocus(object obj)
        {
            PasswordGeneratorGridVisibility = true;
            if (Password.Equals(String.Empty))
            {
                Letters = true;
                Symbols = true;
                Capitals = true;
                Numbers = true;
                PasswordLength = 12;
            }
            else
            {
                
                Symbols = false;
                Capitals = false;
                Numbers = false;
                String specialCharsArray = "~!@#$%^&*()-_=+[{]}|;:<>/?";
                PasswordLength = Password.Length;
                for (int i = 0; i < PasswordLength; i++)
                {
                    char c = Password.ElementAt(i);

                    if (c >= 'A' && c <= 'Z')
                    {
                        Capitals = true;
                    }
                    if (c >= '0' && c <= '9')
                    {
                        Numbers = true;
                    }
                    if (specialCharsArray.Contains("" + c))
                    {
                        Symbols = true;
                    }
                    Letters = false;
                    if (c >= 'a' && c <= 'z')
                    {
                        Letters = true;
                    }

                }
            }
        }

        private void PwdBoxTextChanged(object obj)
        {
            PasswordGeneratorGridVisibility = false;
            PasswordScanner scanner = new PasswordScanner();
            Strength s = (Strength)scanner.scanPassword(Password);
            switch (s)
            {
                case (Strength.WEAK):
                    ProgressbarGeneraterValue = 10;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryWeak");
                    break;
                case (Strength.NORMAL):
                    ProgressbarGeneraterValue = 25;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Weak");
                    break;
                case (Strength.MEDIUM):
                    ProgressbarGeneraterValue = 50;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Good");
                    break;
                case (Strength.STRONG):
                    ProgressbarGeneraterValue = 75;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("Strong");
                    break;
                case (Strength.VERY_STRONG):
                    ProgressbarGeneraterValue = 100;
                    PasswordStrengthText = (string)System.Windows.Application.Current.FindResource("VeryStrong");
                    break;
                default:
                    ProgressbarGeneraterValue = 0;
                    PasswordStrengthText = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void CancelButtonClick(object obj)
        {
            OnSaveComplete();
        }

        private void ContinueSettings(object obj)
        {
            SettingsChangeInvalidDialogVisibility = false;
            IsValidErrorMessageVisible = true;
            if (this.ownerControl != null)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    ownerControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridVisibility"></param>
        internal void HideVisibility()
        {
            CreditCardVisibility = gridVisibility;
            BankAccountVisibility = gridVisibility;
            PaypalVisibility = gridVisibility;          
        }

        /// <summary>
        /// Enable the Prompt  Window   based on paramter name
        /// </summary>
        /// <param name="obj"></param>
        private void ShowPasswordClick(object stringName)
        {

            if (PasswordTextVisible)
            {
                PasswordTextVisible = false;
            }
            else
            {
                if (allowPasswordView) PasswordTextVisible = true;
                else
                {
                    MasterPasswordConfirm masterPass = new MasterPasswordConfirm(pbData);
                    bool? result = masterPass.ShowDialog();
                    //TODO Validate master password before show
                    if (result.HasValue)
                        if (result.Value)
                        {
                            PasswordTextVisible = true;
                            allowPasswordView = masterPass.AlwaysAllow;
                        }
                }
            }
            //var MessageboxName = stringName as string;
            //_addControlHelper.ShowMessageBox(MessageboxName);
        }

        internal void DefaultViewShare()
        {
            
            RecipientEmail = "";
            Message = "";
            ExpirationPeriodIndex = 0;
            
        }

        /// <summary>
        /// default properties of degital wallet addcontrol
        /// </summary>
        internal void DefaultView()
        {
            ReadonlySecureItem = false;
            IsValidErrorMessageVisible = false;

            //CreditCardColor = CreditCardColorsEnum.c_ED6B73;
            CreditCardColor = CreditCardColorsEnum.c_45C2F4;
            
            SelectedCreditCard = null;// CreditCards.FirstOrDefault(x => x.Key == "Visa");
            

            DeleteButtonVisible = Visibility.Hidden;
            DateCreated = null;
            DateModified = null;
            DatesVisibility = false;

            PopulateCategories();
            PopulateShareDurations();
            SelectedAddress = Addresses.FirstOrDefault(x => x.Id == null);
            SelectedCategory = null;
            string settingsCountry = pbData.GetPrivateSetting(DefaultProperties.Settings_Country);
            SelectedCountry = Countries.FirstOrDefault(x => x.Key == settingsCountry);
            
            ExpiresMonth = null;
            ExpiresYear = null;

            RadioBtnCheced = true;
            RadioBtnChecking = false;         
            
            RecipientEmail = "";
            Message = "";
            CreditCardNickname = "";
            
            NameOnCard = "";
            NameOnAccount = "";
            List<SecureItem> nameOnCardDefaults = pbData.GetSecureItemsByType(DefaultProperties.SecurityItemSubType_PI_Names, true, true);
            if(nameOnCardDefaults != null)
                if (nameOnCardDefaults.Count > 0)
                {
                    if(nameOnCardDefaults[0].Data != null){
                        NameOnAccount = NameOnCard = String.Format("{0} {1}", nameOnCardDefaults[0].Data.firstName, nameOnCardDefaults[0].Data.lastName);
                        
                    }
                    
                }
            
            CardNumber = "";
            IssueBank = "";
            IssuingDate = "";
            Cvv = "";
            Pin = "";

            BankAccountNickname = "";
            Bank = "";
            

            BicSwift = "";
            Iban = "";
            RoutingNumber = "";
            AccountNumber = "";
            BankAccountNotes = "";

            PaypalNickname = "";
            Username = "";
            PaypalNotes = "";
            SelectedIndexTabControl = 0;
            Symbols = false;
            Numbers = false;
            Capitals = false;
            Letters = false;
            PasswordGeneratorGridVisibility = false;
            Password = "";
            PasswordTextVisible = true;

            SecuerShareData = null;
            ExpirationPeriodIndex = 0;

            ItemImage = null;

            if(_bankAccountScrollViewer != null)
            {
                _bankAccountScrollViewer.ScrollToHome();
            }
            if(_paypalScrollViewer != null)
            {
                _paypalScrollViewer.ScrollToHome();
            }
            if (_creditCardScrollViewer != null)
            {
                _creditCardScrollViewer.ScrollToHome();
            }

            defaultSecureItem = null;
            this.secureItem = null;
        }


        /// <summary>
        /// tab control selection event for changing header icon
        /// </summary>
        /// <param name="obj"></param>
        private void TabSelectionChanged(object obj)
        {
            if (SelectedIndexTabControl == 0)
            {
                SettingTabIcon = DefaultProperties.ReturnImage(SettingHover);
                SecureShareTabIcon = DefaultProperties.ReturnImage(Share2);
            }
            else
            {
                SettingTabIcon = DefaultProperties.ReturnImage(tabSetting);
                SecureShareTabIcon = DefaultProperties.ReturnImage(Share2Hover);
            }
        }

        /// <summary>
        /// applying carditcard grid visibility
        /// </summary>
        internal void setCreditCardVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryCreditCard);
            CreditCardVisibility = true;
            if(SelectedCreditCard != null)
            {
                SetItemImage(SelectedCreditCard.Value.Key, ((int)CreditCardColor).ToString());
            }

            defaultSecureItem = CreateCreditCardSecureItem();
        }

        /// <summary>
        /// applying Bank account grid visibility
        /// </summary>
        internal void setBankAccountVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryBankAccount);
            BankAccountVisibility = true;
            ItemImage = (ImageSource)Application.Current.FindResource("22");

            defaultSecureItem = CreateBankAccountSecureItem();
        }

        /// <summary>
        ///   applying paypal grid visibility
        /// </summary>
        internal void setPaypalVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.CategoryPaypal);
            PaypalVisibility = true;
            ItemImage = (ImageSource)Application.Current.FindResource("23");
        }
        
        #endregion

        #region SENDER side actions

        private void ResendShare(object obj)
        {
            if (!ResendMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                ResendMessageBoxVisibility = true;
                return;
            }
            ResendMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            var share = pbData.GetSharesByUuid(uuid);

            TimeSpan tmp = DateTime.Now - share.CreatedDate;
            share.ExpirationDate.AddDays(tmp.Days);
            
            shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null);
            shareCommon.ShareItem(share.Receiver, share.Message, SecureItem, 0, share.Visible, share.ExpirationDate);

            

            SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
        }

        private void RevokeShare(object obj)
        {
            if (!UnshareMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                UnshareMessageBoxVisibility = true;
                return;
            }
            UnshareMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Revoked, false, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }
        private void CancelShare(object obj)
        {
            if (!CancelMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                CancelMessageBoxVisibility = true;
                return;
            }
            CancelMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }
        private void SendDataShare(object obj)
        {
            var uuid = obj as string;
            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Pending, true, null))
            {
                SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
            }
        }
        public void CancelShareActionClick(object obj)
        {
            CancelMessageBoxVisibility = false;
            ResendMessageBoxVisibility = false;
            UnshareMessageBoxVisibility = false;
            currentUUID = null;
        }

        private bool _unshareMessageBoxVisibility;

        public bool UnshareMessageBoxVisibility
        {
            get { return _unshareMessageBoxVisibility; }
            set
            {
                _unshareMessageBoxVisibility = value;
                RaisePropertyChanged("UnshareMessageBoxVisibility");
            }
        }

        private bool _resendMessageBoxVisibility;

        public bool ResendMessageBoxVisibility
        {
            get { return _resendMessageBoxVisibility; }
            set
            {
                _resendMessageBoxVisibility = value;
                RaisePropertyChanged("ResendMessageBoxVisibility");
            }
        }

        private bool _cancelMessageBoxVisibility;

        public bool CancelMessageBoxVisibility
        {
            get { return _cancelMessageBoxVisibility; }
            set
            {
                _cancelMessageBoxVisibility = value;
                RaisePropertyChanged("CancelMessageBoxVisibility");
            }
        }
        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { return null; }
        }

        public bool IsValid
        {
            get
            {
                foreach (string property in CreditCardVisibility ? ValidatedPropertiesCreditCard : BankAccountVisibility ? ValidatedPropertiesBank : ValidatedPropertiesPayPal)
                    if (GetValidationErrors(property) != String.Empty)
                        return false;
                return true;
            }
        }
        public string this[string propertyName]
        {
            get
            {
                return GetValidationErrors(propertyName);
            }
        }


        #endregion

        #region Validation
        static readonly string[] ValidatedPropertiesCreditCard = 
        {
            "NameOnCard",
            "CardNumber"
        };
        static readonly string[] ValidatedPropertiesBank = 
        {
            "Bank",
            "NameOnAccount"
        };
        static string[] ValidatedPropertiesPayPal = 
        {
            "PaypalNickname",
            "Username"
        };
        string GetValidationErrors(string propertyName)
        {
            string error = String.Empty;
            switch (propertyName)
            {
                //case "SelectedCategory":
                //    error = ValidateRequiredCategory(SelectedCategory);
                //    break;
                //case "SelectedCountry":
                //    error = ValidateRequiredKeyValue(SelectedCountry);
                //    break;
                //case "CreditCardNickname":
                //    error = ValidateRequiredField(CreditCardNickname);
                //    break;
                case "NameOnCard":
                    error = ValidateRequiredField(NameOnCard);
                    break;
                case "CardNumber":
                    error = ValidateRequiredField(CardNumber);
                    break;
                //case "SelectedCreditCard":
                //    error = ValidateRequiredKeyValue(SelectedCreditCard);
                //    break;
                //case "ExpiresMonth":
                //    error = ValidateRequiredField(ExpiresMonth);
                //    break;
                //case "ExpiresYear":
                //    error = ValidateRequiredField(ExpiresYear);
                //    break;
                //case "IssueBank":
                //    error = ValidateRequiredField(IssueBank);
                //    break;
                //case "Cvv":
                //    error = ValidateRequiredField(Cvv);
                //    break;
                //case "Pin":
                //    error = ValidateRequiredField(Pin);
                //    break;



                //case "BankAccountNickname":
                //    error = ValidateRequiredField(BankAccountNickname);
                //    break; 
                case "Bank":
                    error = ValidateRequiredField(Bank);
                    break;
                case "NameOnAccount":
                    error = ValidateRequiredField(NameOnAccount);
                    break;
                //case "BicSwift":
                //    error = ValidateRequiredField(BicSwift);
                //    break;
                //case "Iban":
                //    error = ValidateRequiredField(Iban);
                //    break;
                //case "RoutingNumber":
                //    error = ValidateRequiredField(RoutingNumber);
                //    break;
                //case "AccountNumber":
                //    error = ValidateRequiredField(AccountNumber);
                //    break;
                


                case "PaypalNickname":
                    error = ValidateRequiredField(PaypalNickname);
                    break;
                case "Username":
                    error = ValidateRequiredField(Username);
                    break;
                //case "Password":
                //    error = ValidateRequiredField(Password);
                //    break;
            };
            return error;
        }

       
        private string ValidateRequiredField(string fieldValue)
        {
            fieldValue = fieldValue != null ? fieldValue.Trim() : null;
            if (string.IsNullOrEmpty(fieldValue)) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        private string ValidateRequiredKeyValue(KeyValuePair<string,string>? fieldValue)
        {
            if (!fieldValue.HasValue) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        private string ValidateRequiredCategory(Folder fieldValue)
        {
            if (fieldValue == null) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        #endregion
    }
}
