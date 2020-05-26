using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using PasswordBoss.Helpers;
using PasswordBoss.Model.PersonalInfo;
using PasswordBoss.DTO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PasswordBoss.PBAnalytics;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using PasswordBoss.Views;
using PasswordBoss.Views.UserControls;
using System.Diagnostics;
using System.Windows.Input;

namespace PasswordBoss.ViewModel
{
    internal class PersonalInfoAddNewItemViewModel : ViewModelBase, IDataErrorInfo
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PersonalInfoAddNewItemViewModel));

        /// <summary>
        /// variables declarations
        /// </summary>
        internal const string focusText = "strong";
        internal const string SettingHover = "imgTabSettingHover";
        internal const string Share2 = "imgShare2";
        const string Share2Hover = "imgShare2Hover";
        const string tabSetting = "imgTabSetting";
        bool gridVisibility = false;
        Common _commonObj = new Common();
        ShareCommon shareCommon = null;
        string currentUUID = null;
        # region RelayCommands
        /// <summary>
        /// relay commands defination
        /// </summary>        
        public RelayCommand TabSelectionChangedCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand SecureShareStatusCommand { get; set; }
        public RelayCommand SecureShareVisibilityCommand { get; set; }
        public RelayCommand ShecureShareEyeCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand MessageBoxConfirmCommand { get; set; }
        public RelayCommand MessageBoxCancelCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddCategoryClickCommand { get; set; }
        public AsyncRelayCommand<LoadingWindow> SaveShareCommand { get; set; }
        public RelayCommand ClearDateCommand { get; set; }

        public RelayCommand ResendShareCommand { get; set; }
        public RelayCommand CancelShareCommand { get; set; }
        public RelayCommand RevokeShareCommand { get; set; }
        public RelayCommand SendDataShareCommand { get; set; }
        public RelayCommand InvalidShareDialogOkCommand { get; set; }

        public RelayCommand CancelShareActionCommand { get; set; }
        # endregion

        //PersonalInfoAddNewItemHelper _personalInfoAddNewItemHelper = new PersonalInfoAddNewItemHelper();
        private IResolver resolver = null;
        private IPBData pbData = null;
        private IInAppAnalytics inAppAnalyitics;
        public event EventHandler<RoutedEventArgs> RefreshList;
        #region properties
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

        public ObservableCollection<PasswordBoss.DTO.Folder> Categories { get; private set; }
        private ObservableCollection<KeyValuePair<string, string>> _countries;
        public ObservableCollection<KeyValuePair<string, string>> Countries 
        {
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

        private KeyValuePair<string, string>? selectedCountry;
        public KeyValuePair<string, string>? SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                if (value.HasValue && selectedCountry.HasValue)
                {
                    if (selectedCountry.Value.Key != value.Value.Key)
                    {
                        selectedCountry = value;
                        RaisePropertyChanged("SelectedCountry");
                    }
                }
                else
                {
                    selectedCountry = value;
                    RaisePropertyChanged("SelectedCountry");
                }
            }
        }

        private KeyValuePair<string, string>? socialSecuritySelectedNationality;
        public KeyValuePair<string, string>? SocialSecuritySelectedNationality
        {
            get
            {
                return socialSecuritySelectedNationality;
            }
            set
            {
                if (value.HasValue && socialSecuritySelectedNationality.HasValue)
                {
                    if (socialSecuritySelectedNationality.Value.Key != value.Value.Key)
                    {
                        socialSecuritySelectedNationality = value;
                        RaisePropertyChanged("SocialSecuritySelectedNationality");
                    }
                }
                else
                {
                    socialSecuritySelectedNationality = value;
                    RaisePropertyChanged("SocialSecuritySelectedNationality");
                }
            }
        }

        private KeyValuePair<string, string>? passportSelectedNationality;
        public KeyValuePair<string, string>? PassportSelectedNationality
        {
            get
            {
                return passportSelectedNationality;
            }
            set
            {
                if (value.HasValue && passportSelectedNationality.HasValue)
                {
                    if (passportSelectedNationality.Value.Key != value.Value.Key)
                    {
                        passportSelectedNationality = value;
                        RaisePropertyChanged("PassportSelectedNationality");
                    }
                }
                else
                {
                    passportSelectedNationality = value;
                    RaisePropertyChanged("PassportSelectedNationality");
                }
            }
        }

        private PasswordBoss.DTO.Folder selectedCategory;
        public PasswordBoss.DTO.Folder SelectedCategory
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

        private bool _settingsChangeDialogVisibility = false;
        public bool SettingsChangeDialogVisibility
        {
            get { return _settingsChangeDialogVisibility; }
            set
            {
                _settingsChangeDialogVisibility = value;
                RaisePropertyChanged("SettingsChangeDialogVisibility");
            }
        }


        private ImageSource newItemImage;
        public ImageSource NewItemImage
        {
            get
            {
                return newItemImage;
            }
            set
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                if (NamesContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("1").ToString(), UriKind.RelativeOrAbsolute);
                else if (AddressContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("2").ToString(), UriKind.RelativeOrAbsolute);
                else if (PhoneContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("3").ToString(), UriKind.RelativeOrAbsolute);
                else if (CompanyContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("4").ToString(), UriKind.RelativeOrAbsolute);
                else if (EmailContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("5").ToString(), UriKind.RelativeOrAbsolute);
                else if (LicenseContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("6").ToString(), UriKind.RelativeOrAbsolute);
                else if (PassportContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("7").ToString(), UriKind.RelativeOrAbsolute);
                else if (MemberContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("8").ToString(), UriKind.RelativeOrAbsolute);
                else if (SocialSecurityContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("9").ToString(), UriKind.RelativeOrAbsolute);
                else if (SecureNotesContentVisibility) bi.UriSource = new Uri(System.Windows.Application.Current.FindResource("10").ToString(), UriKind.RelativeOrAbsolute);
                
                bi.EndInit();
                newItemImage = bi as ImageSource;
                RaisePropertyChanged("NewItemImage");
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

        private SecureItem defaultSecureItem;
        private SecureItem existingSecureItem;

        private SecureItem secureItem;
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
                RaisePropertyChanged("IsShareEnabled");
                if (secureItem != null)
                {
                    ReadonlySecureItem = secureItem.Readonly;
                    HideVisibility();
                    DateCreated = secureItem.CreatedDate;
                    DateModified = secureItem.LastModifiedDate;
                    ShareLabel = secureItem.Name;
                    switch (secureItem.Type)
                    {
                        case DefaultProperties.SecurityItemSubType_PI_Names:
                            //set visibility
                            NamesContentVisibility = true;
                            //map data
                            if (secureItem.Data != null)
                            {
                                NamesFirstName = secureItem.Data.firstName;
                                NamesMiddleName = secureItem.Data.middleName;
                                NamesLastName = secureItem.Data.lastName;
                                NamesNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_PI_Address:
                            //set visibility
                            AddressContentVisibility = true;
                            //map data
                            AddressNickname = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                if (secureItem.Data.country != null)
                                    SelectedCountry = Countries.SingleOrDefault(x => x.Key == secureItem.Data.country);
                                Address1 = secureItem.Data.address1;
                                Address2 = secureItem.Data.address2;
                                AddressAptSuit = secureItem.Data.apt;
                                AddressCity = secureItem.Data.city;
                                AddressState = secureItem.Data.state;
                                AddressZipCode = secureItem.Data.zipCode;
                                AddressNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_PI_PhoneNumber:
                            //set visibility
                            PhoneContentVisibility = true;
                            //map data
                            PhoneNickname = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                PhoneNumber = secureItem.Data.phoneNumber;
                                PhoneNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_PI_Email:
                            //set visibility
                            EmailContentVisibility = true;
                            //map data
                            EmailNickName = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                Email = secureItem.Data.email;
                                EmailNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_SN_DriverLicense:
                            //set visibility
                            LicenseContentVisibility = true;
                            //map data
                            LicenseNickname = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                LicenseFirstName = secureItem.Data.firstName;
                                LicenseLastName = secureItem.Data.lastName;
                                if (secureItem.Data.country != null)
                                    LicenseCountry = Countries.SingleOrDefault(x => x.Key == secureItem.Data.country);
                                LicenseState = secureItem.Data.state;
                                LicenseNumber = secureItem.Data.driverLicenceNumber;
                                LicenseExpires = secureItem.Data.expires;
                                LicenseIssueDate = secureItem.Data.issueDate;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_SN_MemberIDs:
                            //set visibility
                            MemberContentVisibility = true;
                            //map data
                            MemberIdNickname = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                MemberIdFirstName = secureItem.Data.firstName;
                                MemberIdLastName = secureItem.Data.lastName;
                                MemberId = secureItem.Data.memberID;
                                MemberIdNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_SN_SocialSecurity:
                            //set visibility
                            SocialSecurityContentVisibility = true;
                            //map data
                            if (secureItem.Data != null)
                            {
                                if (secureItem.Data.nationality != null)
                                    SocialSecuritySelectedNationality = Countries.SingleOrDefault(x => x.Key == secureItem.Data.nationality);
                                //SocialSecurityNationality = secureItem.Data.nationality;
                                SocialSecurityFirstName = secureItem.Data.firstName;
                                SocialSecurityLastName = secureItem.Data.lastName;
                                SocialSecurityDateOfBirth = secureItem.Data.dateOfBirth;
                                SocialSecurityNumber = secureItem.Data.ssn;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_SN_Passport:
                            //set visibility
                            PassportContentVisibility = true;
                            //map data
                            PassportNickName = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                PassportFirstName = secureItem.Data.firstName;
                                PassportLastName = secureItem.Data.lastName;
                                //PassportNationality = secureItem.Data.nationality;
                                if (secureItem.Data.nationality != null)
                                    PassportSelectedNationality = Countries.SingleOrDefault(x => x.Key == secureItem.Data.nationality);
                                PassportDateOfBirth = secureItem.Data.dateOfBirth;
                                PassportNumber = secureItem.Data.passportNumber;
                                PassportIssueDate = secureItem.Data.issueDate;
                                PassportExpiers = secureItem.Data.expires;
                                PassportPlaceOfIssue = secureItem.Data.placeOfIssue;
                                Male = !Female;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_PI_Company:
                            //set visibility
                            CompanyContentVisibility = true;
                            //map data
                            Company = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                CompanyNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        case DefaultProperties.SecurityItemSubType_PI_SecureNotes:
                            //set visibility
                            SecureNotesContentVisibility = true;
                            //map data
                            SecureNoteTitle = secureItem.Name;
                            if (secureItem.Data != null)
                            {
                                SecureNoteNotes = secureItem.Data.notes;
                            }
                            SelectedCategory = secureItem.Folder != null ? Categories.SingleOrDefault(x => x.Id == secureItem.Folder.Id) : null;
                            break;
                        default:
                            break;
                    }
                    NewItemImage = null; 
                    
                    ShareCommon shareCommon = new ShareCommon(resolver);
                    SecuerShareData = shareCommon.BindingSecureShareList(secureItem.Id);
                }
               
                DeleteButtonVisible = value != null ? Visibility.Visible : Visibility.Hidden;

                if (value == null)
                {
                    existingSecureItem = value;
                }
                else
                {
                    existingSecureItem = CreateSecureItem();
                }
            }
        }

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
        private bool _namesContentVisibility;
        public bool NamesContentVisibility
        {
            get { return _namesContentVisibility; }
            set
            {
                _namesContentVisibility = value;
                RaisePropertyChanged("NamesContentVisibility");
            }
        }
        private bool _addressContentVisibility;
        public bool AddressContentVisibility
        {
            get { return _addressContentVisibility; }
            set
            {
                _addressContentVisibility = value;
                RaisePropertyChanged("AddressContentVisibility");
            }
        }
        private bool _phoneContentVisibility;
        public bool PhoneContentVisibility
        {
            get { return _phoneContentVisibility; }
            set
            {
                _phoneContentVisibility = value;
                RaisePropertyChanged("PhoneContentVisibility");
            }
        }
        private bool _emailContentVisibility;
        public bool EmailContentVisibility
        {
            get { return _emailContentVisibility; }
            set
            {
                _emailContentVisibility = value;
                RaisePropertyChanged("EmailContentVisibility");
            }
        }
        private bool _licenseContentVisibility;
        public bool LicenseContentVisibility
        {
            get { return _licenseContentVisibility; }
            set
            {
                _licenseContentVisibility = value;
                RaisePropertyChanged("LicenseContentVisibility");
            }
        }
        private bool _memberContentVisibility;
        public bool MemberContentVisibility
        {
            get { return _memberContentVisibility; }
            set
            {
                _memberContentVisibility = value;
                RaisePropertyChanged("MemberContentVisibility");
            }
        }
        private bool _socialSecurityContentVisibility;
        public bool SocialSecurityContentVisibility
        {
            get { return _socialSecurityContentVisibility; }
            set
            {
                _socialSecurityContentVisibility = value;
                RaisePropertyChanged("SocialSecurityContentVisibility");
            }
        }
        private bool _passportContentVisibility;
        public bool PassportContentVisibility
        {
            get { return _passportContentVisibility; }
            set
            {
                _passportContentVisibility = value;
                RaisePropertyChanged("PassportContentVisibility");
            }
        }
        private bool _companyContentVisibility;
        public bool CompanyContentVisibility
        {
            get { return _companyContentVisibility; }
            set
            {
                _companyContentVisibility = value;
                RaisePropertyChanged("CompanyContentVisibility");
            }
        }
        private bool _secureNotesContentVisibility;
        public bool SecureNotesContentVisibility
        {
            get { return _secureNotesContentVisibility; }
            set
            {
                _secureNotesContentVisibility = value;
                RaisePropertyChanged("SecureNotesContentVisibility");
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
        string _namesFirstName = "";
        public string NamesFirstName
        {
            get { return _namesFirstName; }
            set
            {
                _namesFirstName = value;
                RaisePropertyChanged("NamesFirstName");
            }
        }
        string _namesMiddleName = "";
        public string NamesMiddleName
        {
            get { return _namesMiddleName; }
            set
            {
                _namesMiddleName = value;
                RaisePropertyChanged("NamesMiddleName");
            }
        }
        string _namesLastName = "";
        public string NamesLastName
        {
            get { return _namesLastName; }
            set
            {
                _namesLastName = value;
                RaisePropertyChanged("NamesLastName");
            }
        }
        string _namesNotes = "";
        public string NamesNotes
        {
            get { return _namesNotes; }
            set
            {
                _namesNotes = value;
                RaisePropertyChanged("NamesNotes");
            }
        }
        string _addressNickname = "";
        public string AddressNickname
        {
            get { return _addressNickname; }
            set
            {
                _addressNickname = value;
                RaisePropertyChanged("AddressNickname");
            }
        }string _address1 = "";
        public string Address1
        {
            get { return _address1; }
            set
            {
                _address1 = value;
                RaisePropertyChanged("Address1");
            }
        }
        string _address2 = "";
        public string Address2
        {
            get { return _address2; }
            set
            {
                _address2 = value;
                RaisePropertyChanged("Address2");
            }
        }
        string _addressAptSuit = "";
        public string AddressAptSuit
        {
            get { return _addressAptSuit; }
            set
            {
                _addressAptSuit = value;
                RaisePropertyChanged("AddressAptSuit");
            }
        }
        string _addressCity = "";
        public string AddressCity
        {
            get { return _addressCity; }
            set
            {
                _addressCity = value;
                RaisePropertyChanged("AddressCity");
            }
        }
        string _addressState = "";
        public string AddressState
        {
            get { return _addressState; }
            set
            {
                _addressState = value;
                RaisePropertyChanged("AddressState");
            }
        }
        string _addressZipCode = "";
        public string AddressZipCode
        {
            get { return _addressZipCode; }
            set
            {
                _addressZipCode = value;
                RaisePropertyChanged("AddressZipCode");
            }
        }
        string _addressNotes = "";
        public string AddressNotes
        {
            get { return _addressNotes; }
            set
            {
                _addressNotes = value;
                RaisePropertyChanged("AddressNotes");
            }
        }
        string _phoneNickname = "";
        public string PhoneNickname
        {
            get { return _phoneNickname; }
            set
            {
                _phoneNickname = value;
                RaisePropertyChanged("PhoneNickname");
            }
        }
        string _phoneNumber = "";
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }
        string _phoneNotes = "";
        public string PhoneNotes
        {
            get { return _phoneNotes; }
            set
            {
                _phoneNotes = value;
                RaisePropertyChanged("PhoneNotes");
            }
        }
        string _emailNickName = "";
        public string EmailNickName
        {
            get { return _emailNickName; }
            set
            {
                _emailNickName = value;
                RaisePropertyChanged("EmailNickName");
            }
        }
        string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        string _emailNotes = "";
        public string EmailNotes
        {
            get { return _emailNotes; }
            set
            {
                _emailNotes = value;
                RaisePropertyChanged("EmailNotes");
            }
        }
        string _licenseNickname = "";
        public string LicenseNickname
        {
            get { return _licenseNickname; }
            set
            {
                _licenseNickname = value;
                RaisePropertyChanged("LicenseNickname");
            }
        }
        string _licenseFirstName = "";
        public string LicenseFirstName
        {
            get { return _licenseFirstName; }
            set
            {
                _licenseFirstName = value;
                RaisePropertyChanged("LicenseFirstName");
            }
        }
        string _licenseLastName = "";
        public string LicenseLastName
        {
            get { return _licenseLastName; }
            set
            {
                _licenseLastName = value;
                RaisePropertyChanged("LicenseLastName");
            }
        }

        private KeyValuePair<string, string>? licenseCountry;
        public KeyValuePair<string, string>? LicenseCountry
        {
            get
            {
                return licenseCountry;
            }
            set
            {
                if (value.HasValue && licenseCountry.HasValue)
                {
                    if (licenseCountry.Value.Key != value.Value.Key)
                    {
                        licenseCountry = value;
                        RaisePropertyChanged("LicenseCountry");
                    }
                }
                else
                {
                    licenseCountry = value;
                    RaisePropertyChanged("LicenseCountry");
                }
            }
        }

        string _licenseState = "";
        public string LicenseState
        {
            get { return _licenseState; }
            set
            {
                _licenseState = value;
                RaisePropertyChanged("LicenseState");
            }
        }
        string _licenseNumber = "";
        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set
            {
                _licenseNumber = value;
                RaisePropertyChanged("LicenseNumber");
            }
        }
        string _licenseExpires = "";
        public string LicenseExpires
        {
            get { return _licenseExpires; }
            set
            {
                _licenseExpires = value;
                RaisePropertyChanged("LicenseExpires");
            }
        }
        string _licenseIssueDate = "";
        public string LicenseIssueDate
        {
            get { return _licenseIssueDate; }
            set
            {
                _licenseIssueDate = value;
                RaisePropertyChanged("LicenseIssueDate");
            }
        }
        string _memberIdNickname = "";
        public string MemberIdNickname
        {
            get { return _memberIdNickname; }
            set
            {
                _memberIdNickname = value;
                RaisePropertyChanged("MemberIdNickname");
            }
        }
        string _memberIdFirstName = "";
        public string MemberIdFirstName
        {
            get { return _memberIdFirstName; }
            set
            {
                _memberIdFirstName = value;
                RaisePropertyChanged("MemberIdFirstName");
            }
        }
        string _memberIdLastName = "";
        public string MemberIdLastName
        {
            get { return _memberIdLastName; }
            set
            {
                _memberIdLastName = value;
                RaisePropertyChanged("MemberIdLastName");
            }
        }
        string _memberId = "";
        public string MemberId
        {
            get { return _memberId; }
            set
            {
                _memberId = value;
                RaisePropertyChanged("MemberId");
            }
        }
        string _memberIdNotes = "";
        public string MemberIdNotes
        {
            get { return _memberIdNotes; }
            set
            {
                _memberIdNotes = value;
                RaisePropertyChanged("MemberIdNotes");
            }
        }
        string _socialSecurityNationality = "";
        public string SocialSecurityNationality
        {
            get { return _socialSecurityNationality; }
            set
            {
                _socialSecurityNationality = value;
                RaisePropertyChanged("SocialSecurityNationality");
            }
        }
        string _socialSecurityFirstName = "";
        public string SocialSecurityFirstName
        {
            get { return _socialSecurityFirstName; }
            set
            {
                _socialSecurityFirstName = value;
                RaisePropertyChanged("SocialSecurityFirstName");
            }
        }
        string _socialSecurityLastName = "";
        public string SocialSecurityLastName
        {
            get { return _socialSecurityLastName; }
            set
            {
                _socialSecurityLastName = value;
                RaisePropertyChanged("SocialSecurityLastName");
            }
        }
        string _socialSecurityDateOfBirth = "";
        public string SocialSecurityDateOfBirth
        {
            get { return _socialSecurityDateOfBirth; }
            set
            {
                _socialSecurityDateOfBirth = value;
                RaisePropertyChanged("SocialSecurityDateOfBirth");
            }
        }
        string _socialSecurityNumber = "";
        public string SocialSecurityNumber
        {
            get { return _socialSecurityNumber; }
            set
            {
                _socialSecurityNumber = value;
                RaisePropertyChanged("SocialSecurityNumber");
            }
        }
        string _passportNickName = "";
        public string PassportNickName
        {
            get { return _passportNickName; }
            set
            {
                _passportNickName = value;
                RaisePropertyChanged("PassportNickName");
            }
        }
        string _passportFirstName = "";
        public string PassportFirstName
        {
            get { return _passportFirstName; }
            set
            {
                _passportFirstName = value;
                RaisePropertyChanged("PassportFirstName");
            }
        }
        string _passportLastName = "";
        public string PassportLastName
        {
            get { return _passportLastName; }
            set
            {
                _passportLastName = value;
                RaisePropertyChanged("PassportLastName");
            }
        }
        string _passportNationality = "";
        public string PassportNationality
        {
            get { return _passportNationality; }
            set
            {
                _passportNationality = value;
                RaisePropertyChanged("PassportNationality");
            }
        }
        string _passportDateOfBirth = "";
        public string PassportDateOfBirth
        {
            get { return _passportDateOfBirth; }
            set
            {
                _passportDateOfBirth = value;
                RaisePropertyChanged("PassportDateOfBirth");
            }
        }
        string _passportNumber = "";
        public string PassportNumber
        {
            get { return _passportNumber; }
            set
            {
                _passportNumber = value;
                RaisePropertyChanged("PassportNumber");
            }
        }
        string _passportIssueDate = "";
        public string PassportIssueDate
        {
            get { return _passportIssueDate; }
            set
            {
                _passportIssueDate = value;
                RaisePropertyChanged("PassportIssueDate");
            }
        }
        string _passportExpiers = "";
        public string PassportExpiers
        {
            get { return _passportExpiers; }
            set
            {
                _passportExpiers = value;
                RaisePropertyChanged("PassportExpiers");
            }
        }
        string _passportPlaceOfIssue = "";
        public string PassportPlaceOfIssue
        {
            get { return _passportPlaceOfIssue; }
            set
            {
                _passportPlaceOfIssue = value;
                RaisePropertyChanged("PassportPlaceOfIssue");
            }
        }
        bool _female = false;
        public bool Female
        {
            get { return _female; }
            set
            {
                _female = value;
                RaisePropertyChanged("Female");
            }
        }
        bool _male = false;
        public bool Male
        {
            get { return _male; }
            set
            {
                _male = value;
                RaisePropertyChanged("Male");
            }
        }
        string _company = "";
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                RaisePropertyChanged("Company");
            }
        }
        string _companyNotes = "";
        public string CompanyNotes
        {
            get { return _companyNotes; }
            set
            {
                _companyNotes = value;
                RaisePropertyChanged("CompanyNotes");
            }
        }
        string _secureNoteTitle = "";
        public string SecureNoteTitle
        {
            get { return _secureNoteTitle; }
            set
            {
                _secureNoteTitle = value;
                RaisePropertyChanged("SecureNoteTitle");
            }
        }
        string _secureNoteNotes = "";
        public string SecureNoteNotes
        {
            get { return _secureNoteNotes; }
            set
            {
                _secureNoteNotes = value;
                RaisePropertyChanged("SecureNoteNotes");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        List<PasswordBoss.Helpers.SecuerShareData> _secuerShareData;
        public List<SecuerShareData> SecuerShareData
        {
            get { return _secuerShareData; }
            set
            {
                _secuerShareData = value;
                RaisePropertyChanged("SecuerShareData");
            }
        }
        List<ComboboxItem> _personalNamesCategory;
        public List<ComboboxItem> PersonalNamesCategory
        {
            get { return _personalNamesCategory; }
            set
            {
                _personalNamesCategory = value;
                RaisePropertyChanged("PersonalNamesCategory");
            }
        }

        #endregion
        private UserControl ownerControl;
        private PersonalInfoContentPanel personalInfoPanel;

        //To enable resetting scroll viewer position
        private ScrollViewer _addControlScrollViewer;
        /// <summary>
        /// constructor for initilizing commands
        /// </summary>
        public PersonalInfoAddNewItemViewModel(IResolver resolver, UserControl ownerControl, PersonalInfoContentPanel panel)
        {
            this.resolver = resolver;
            this.personalInfoPanel = panel;
            pbData = resolver.GetInstanceOf<IPBData>();
            shareCommon = new ShareCommon(resolver);
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            this.ownerControl = ownerControl;
            TabSelectionChangedCommand = new RelayCommand(TabSelectionChanged);
            CancelCommand = new RelayCommand(CancelButtonClick);
            ContinueCommand = new RelayCommand(ContinueSettings);
            SecureShareStatusCommand = new RelayCommand(SecureShareStatusClick);
            SecureShareVisibilityCommand = new RelayCommand(SecureShareVisibilityClick);
            ShecureShareEyeCommand = new RelayCommand(ShecureShareEyeClick);
            SaveCommand = new RelayCommand(SaveItem);
            MessageBoxConfirmCommand = new RelayCommand(MessageBoxConfirmClick);
            MessageBoxCancelCommand = new RelayCommand(MessageBoxCancelClick);
            DeleteCommand = new RelayCommand(DeleteItem);

            ResendShareCommand = new RelayCommand(ResendShare);
            CancelShareCommand = new RelayCommand(CancelShare);
            RevokeShareCommand = new RelayCommand(RevokeShare);
            SendDataShareCommand = new RelayCommand(SendDataShare);
            InvalidShareDialogOkCommand = new RelayCommand(InvalidShareDialogOkClick);
            CancelShareActionCommand = new RelayCommand(CancelShareActionClick);
            
            //SecuerShareData = _personalInfoAddNewItemHelper.BindingSecureShareList();
            //vedo - async ?
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Categories = new ObservableCollection<PasswordBoss.DTO.Folder>(pbData.GetFoldersBySecureItemType());
                Countries = new ObservableCollection<KeyValuePair<string, string>>(pbData.GetCountries());
            });
            AddCategoryClickCommand = new RelayCommand(AddCategoryClick);
            SaveShareCommand = new AsyncRelayCommand<LoadingWindow>(SaveShare);
            ClearDateCommand = new RelayCommand(ClearDateClick);

            if (ownerControl != null)
            {
                try
                {
                    _addControlScrollViewer = this.ownerControl.FindName("PersonalInfoScroller") as ScrollViewer;
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                }
                
            }

            PopulateShareDurations();
            ExpirationPeriodIndex = 0;
            //PersonalInfoScroller
        }

        # region Methods

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

                    var pi = inAppAnalyitics.Get<Events.PersonalInfo, PersonalInfoItem>();
                    pi.Log(new PersonalInfoItem(SecureItemAction.Deleted, ApplicationSource.MainUI, DefaultProperties.GetPersonalInfoEventTypeBySecureItemType(secureItem.Type)));
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
                        Categories = new ObservableCollection<DTO.Folder>(pbData.GetFoldersBySecureItemType());
                        RaisePropertyChanged("Categories");
                        SelectedCategory = Categories.SingleOrDefault(x => x.Id == newCode);
                    }

                }
        }

        private void PopulateCategories()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                Categories = new ObservableCollection<PasswordBoss.DTO.Folder>(pbData.GetFoldersBySecureItemType());
                RaisePropertyChanged("Categories");
            });
        }

        private void PopulateShareDurations()
        {
            if (shareDurations == null)
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

        private void DeleteItem(object obj)
        {
            MessageBoxVisibility = true;
        }

        public void DeleteItem()
        {
            MessageBoxVisibility = true;
        }

        private void ClearDateClick(object obj)
        {
            if(obj != null)
            {
                switch(obj.ToString())
                {
                    case "1":
                        LicenseExpires = string.Empty;
                        break;
                    case "2":
                        LicenseIssueDate = string.Empty;
                        break;
                    case "3":
                        SocialSecurityDateOfBirth = string.Empty;
                        break;
                    case "4":
                        PassportDateOfBirth = string.Empty;
                        break;
                    case "5":
                        PassportIssueDate = string.Empty;
                        break;
                    case "6":
                        PassportExpiers = string.Empty;
                        break;
                    default:
                        break;
                }
            }
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
                        AfterSaveCompleted();
                    }
                }

            }));

        }

        private string ConvertSecureItemTypeToFeatureIdentifier()
        {
            string identifier = "";
            if (AddressContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageAddress;
            }
            else if(PhoneContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManagePhone;
            }
            else if (EmailContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageEmail;
            }
            else if (LicenseContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageDriverLicense;
            }
            else if (MemberContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageMemberId;
            }
            else if (SocialSecurityContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageSSN;
            }
            else if (PassportContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManagePassport;
            }
            else if (CompanyContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageCompany;
            }
            else if (SecureNotesContentVisibility)
            {
				identifier = DefaultProperties.Features.PersonalInfo.AddManageSecureNotes;
            }
            
            return identifier;
            //foreach (string property in NamesContentVisibility ? ValidatedPropertiesNames : AddressContentVisibility ? ValidatedPropertiesAddress : PhoneContentVisibility ? ValidatedPropertiesPhone :
            //        EmailContentVisibility ? ValidatedPropertiesEmail : LicenseContentVisibility ? ValidatedPropertiesLicense : MemberContentVisibility ? ValidatedPropertiesMember :
            //        SocialSecurityContentVisibility ? ValidatedPropertiesSocialSecurity : PassportContentVisibility ? ValidatedPropertiesPassport : CompanyContentVisibility ? ValidatedPropertiesCompany : ValidatedPropertiesSecureNotes)
            //    if (GetValidationErrors(property) != String.Empty)
            //        return false;
            //return true;
        }

        public bool HasModelChanged()
        {
            SecureItem secureItem = CreateSecureItem();

            if(secureItem == null)
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

        private SecureItem CreateSecureItem()
        {
            SecureItem secureItem = null;
            if (NamesContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = String.Format("{0} {1}", NamesFirstName, NamesLastName),
                    Type = DefaultProperties.SecurityItemSubType_PI_Names,
                    Data = new SecureItemData()
                    {
                        firstName = NamesFirstName,
                        middleName = NamesMiddleName,
                        lastName = NamesLastName,
                        notes = NamesNotes
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.Names) : SelectedCategory
                };
            }
            else if (AddressContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
					SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(AddressNickname) ? AddressNickname : Address1,
                    Type = DefaultProperties.SecurityItemSubType_PI_Address,
                    Data = new SecureItemData()
                    {
                        country = SelectedCountry.HasValue ? SelectedCountry.Value.Key : null,
                        address1 = Address1,
                        address2 = Address2,
                        apt = AddressAptSuit,
                        city = AddressCity,
                        state = AddressState,
                        zipCode = AddressZipCode,
                        notes = AddressNotes
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.Addresses) : SelectedCategory
                };
            }
            else if (PhoneContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(PhoneNickname) ? PhoneNickname : PhoneNumber,
                    Type = DefaultProperties.SecurityItemSubType_PI_PhoneNumber,
                    Data = new SecureItemData()
                    {
                        phoneNumber = PhoneNumber,
                        notes = PhoneNotes
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.PhoneNumbers) : SelectedCategory
                };
            }
            else if (EmailContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(EmailNickName) ? EmailNickName : Email,
                    Type = DefaultProperties.SecurityItemSubType_PI_Email,
                    Data = new SecureItemData()
                    {
                        email = Email,
                        notes = EmailNotes
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.Email) : SelectedCategory
                };
            }
            else if (LicenseContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(LicenseNickname) ? LicenseNickname : LicenseNumber,
                    Type = DefaultProperties.SecurityItemSubType_SN_DriverLicense,
                    Data = new SecureItemData()
                    {
                        firstName = LicenseFirstName,
                        lastName = LicenseLastName,
                        country = LicenseCountry.HasValue ? LicenseCountry.Value.Key : null,
                        state = LicenseState,
                        driverLicenceNumber = LicenseNumber//,
                        //expires = LicenseExpires,
                        //issueDate = LicenseIssueDate
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.DriverLicense) : SelectedCategory
                };

                if (!String.IsNullOrWhiteSpace(LicenseExpires))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(LicenseExpires, out tmp))
                    {
                        secureItem.Data.expires = tmp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    }
                }
                if (!String.IsNullOrWhiteSpace(LicenseIssueDate))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(LicenseIssueDate, out tmp))
                    {
                        secureItem.Data.issueDate = tmp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    }
                }

            }
            else if (MemberContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(MemberIdNickname) ? MemberIdNickname : MemberId,
                    Type = DefaultProperties.SecurityItemSubType_SN_MemberIDs,
                    Data = new SecureItemData()
                    {
                        firstName = MemberIdFirstName,
                        lastName = MemberIdLastName,
                        memberID = MemberId,
                        notes = MemberIdNotes
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.MemberIds) : SelectedCategory
                };
            }
            else if (SocialSecurityContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = SocialSecurityNumber,
                    Type = DefaultProperties.SecurityItemSubType_SN_SocialSecurity,
                    Data = new SecureItemData()
                    {
                        //nationality = SocialSecurityNationality,
                        nationality = SocialSecuritySelectedNationality.HasValue ? SocialSecuritySelectedNationality.Value.Key : null,
                        firstName = SocialSecurityFirstName,
                        lastName = SocialSecurityLastName,
                        dateOfBirth = SocialSecurityDateOfBirth,
                        ssn = SocialSecurityNumber
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.SocialSecurity) : SelectedCategory
                };
            }
            else if (PassportContentVisibility)
            {

              
                
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = !String.IsNullOrEmpty(PassportNickName) ? PassportNickName : PassportNumber,
                    Type = DefaultProperties.SecurityItemSubType_SN_Passport,
                    Data = new SecureItemData()
                    {
                        firstName = PassportFirstName,
                        lastName = PassportLastName,
                        //nationality = PassportNationality,
                        nationality = PassportSelectedNationality.HasValue ? PassportSelectedNationality.Value.Key : null,
                       // dateOfBirth = PassportDateOfBirth,
                        passportNumber = PassportNumber,
                        //issueDate = PassportIssueDate,
                        //expires = PassportExpiers,
                        placeOfIssue = PassportPlaceOfIssue,
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.Passport) : SelectedCategory
                };

                if (!String.IsNullOrWhiteSpace(PassportDateOfBirth))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(PassportDateOfBirth, out tmp))
                    {
                       secureItem.Data.dateOfBirth = tmp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    }
                }
                if (!String.IsNullOrWhiteSpace(PassportIssueDate))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(PassportIssueDate, out tmp))
                    {
                        secureItem.Data.issueDate = tmp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    }
                }
                if (!String.IsNullOrWhiteSpace(PassportExpiers))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(PassportExpiers, out tmp))
                    {
                        secureItem.Data.expires = tmp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    }
                }


            }
            else if (CompanyContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = Company,
                    Type = DefaultProperties.SecurityItemSubType_PI_Company,
                    Data = new SecureItemData()
                    {
                        notes = CompanyNotes,
                        company = Company
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.Company) : SelectedCategory
                };
            }
            else if (SecureNotesContentVisibility)
            {
                secureItem = new SecureItem()
                {
                    Id = SecureItem != null ? this.SecureItem.Id : null,
                    SecureItemTypeName = DefaultProperties.SecurityItemType.PersonalInfo,
                    Name = SecureNoteTitle,
                    Type = DefaultProperties.SecurityItemSubType_PI_SecureNotes,
                    Data = new SecureItemData()
                    {
                        notes = SecureNoteNotes,
                        //TODO this identifier doesn't exist anymore
                        //title = SecureNoteTitle
                    },
                    Folder = SelectedCategory == null ? Categories.SingleOrDefault(x => x.Id == DefaultCategories.SecureNotes) : SelectedCategory
                };
            }

            return secureItem;
        }

        private void SaveItem(object obj)
        {
            SettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;
            string featureIdentifier = ConvertSecureItemTypeToFeatureIdentifier();
            if(featureIdentifier != "")
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(featureIdentifier))
                {
                    return;
                }
            }
            if (!IsValid)
            {
                IsValidErrorMessageVisible = true;
                return;
            }
            IsValidErrorMessageVisible = false;

            try
            {
                SecureItem secureItem = CreateSecureItem();
               
                if (secureItem != null)
                {
                    if ((secureItem = pbData.AddOrUpdateSecureItem(secureItem)) == null)
                    {
                        MessageBox.Show("Error while saving item");
                    }
                    else
                    {
                        var pi = inAppAnalyitics.Get<Events.PersonalInfo, PersonalInfoItem>();
                        pi.Log(new PersonalInfoItem(SecureItemAction.Added, ApplicationSource.MainUI, DefaultProperties.GetPersonalInfoEventTypeBySecureItemType(secureItem.Type)));

                        //update shares
                        ShareCommon shareCommon = new ShareCommon(resolver);
                        shareCommon.UpdateShares(secureItem);

                        EventHandler<RoutedEventArgs> handler = RefreshList;
                        handler(this, null);
                    }
                }

                AfterSaveCompleted();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                MessageBox.Show("Error while saving item");
            }
        }

        private void AfterSaveCompleted()
        {
            HideVisibility();
            CloseAnimation();
            DefaultView();
            if(this.personalInfoPanel != null)
            {
                this.personalInfoPanel.PersonalInfoItemsContainer.listView.SelectedItems.Clear();
            }
        }

        private void CloseAnimation()
        {
            Storyboard sbClose = Application.Current.TryFindResource("StoryboardCloseNewItem") as Storyboard;
            Storyboard.SetTarget(sbClose, ownerControl);
            sbClose.Begin();
        }
        /// <summary>
        /// for adding combobox items
        /// </summary>

        /// <summary>
        /// parsing data for itemsource
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<ComboboxItem> GetComboxItem(PersonalInfoItemClass data)
        {
            List<ComboboxItem> returnItem = new List<ComboboxItem>();
            for (int i = 0; i < data.data.Length; i++)
            {
                for (int x = 0; x < data.data[i].categories.Length; x++)
                {
                    returnItem.Add(new ComboboxItem() { categoryName = data.data[i].categories[x].categoryName });
                }

            }
            return returnItem;
        }
        /// <summary>
        ///  handle eye click event
        /// </summary>
        /// <param name="obj"></param>
        private void ShecureShareEyeClick(object obj)
        {
            //_personalInfoAddNewItemHelper.ShowMessage(obj);
        }

        /// <summary>
        ///  handle status click event
        /// </summary>
        /// <param name="obj"></param>
        private void SecureShareStatusClick(object obj)
        {
            //  _personalInfoAddNewItemHelper.ShowMessage(obj);
        }

        /// <summary>
        ///  handle visibility click event
        /// </summary>
        /// <param name="obj"></param>
        private void SecureShareVisibilityClick(object obj)
        {
            //  _personalInfoAddNewItemHelper.ShowMessage(obj);
        }

        private void ContinueSettings(object obj)
        {

            SettingsChangeInvalidDialogVisibility = false;
            IsValidErrorMessageVisible = true;

            if(this.ownerControl != null)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    ownerControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                });
            }
        }

        /// <summary>
        /// cancel button event
        /// </summary>
        /// <param name="obj"></param>
        private void CancelButtonClick(object obj)
        {
            CloseAnimation();
            DefaultView();
            SettingsChangeDialogVisibility = false;
            SettingsChangeInvalidDialogVisibility = false;
            HideVisibility();
        }

        /// <summary>
        /// hide the visibility of all 10 items
        /// </summary>
        internal void HideVisibility()
        {
            NamesContentVisibility = gridVisibility;
            AddressContentVisibility = gridVisibility;
            PhoneContentVisibility = gridVisibility;
            EmailContentVisibility = gridVisibility;
            LicenseContentVisibility = gridVisibility;
            MemberContentVisibility = gridVisibility;
            SocialSecurityContentVisibility = gridVisibility;
            PassportContentVisibility = gridVisibility;
            CompanyContentVisibility = gridVisibility;
            SecureNotesContentVisibility = gridVisibility;
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
        /// applying names grid visibility
        /// </summary>
        internal void setNameVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.Names);
            NamesContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("1") as ImageSource;
        }

        /// <summary>
        /// applying address grid visibility
        /// </summary>
        internal void setAddressVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.Addresses);
            AddressContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("2") as ImageSource;
        }

        /// <summary>
        /// applying Phone phone visibility
        /// </summary>
        internal void setPhoneVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.PhoneNumbers);
            PhoneContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("3") as ImageSource;
        }

        /// <summary>
        /// applying Phone Email visibility
        /// </summary>
        internal void setEmailVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.Email);
            EmailContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("5") as ImageSource;
        }

        /// <summary>
        /// applying Phone License visibility
        /// </summary>
        internal void setLicenseVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.DriverLicense);
            LicenseContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("6") as ImageSource;
        }

        /// <summary>
        /// applying Phone Member visibility
        /// </summary>
        internal void setMemberVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.MemberIds);
            MemberContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("8") as ImageSource;
        }

        /// <summary>
        /// applying Phone SocialSecurity visibility
        /// </summary>
        internal void setSocialSecurityVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.SocialSecurity);
            SocialSecurityContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("9") as ImageSource;
        }

        /// <summary>
        /// applying Phone Passport visibility
        /// </summary>
        internal void setPassportVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.Passport);
            PassportContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("7") as ImageSource;
        }

        /// <summary>
        /// applying Phone Company visibility
        /// </summary>
        internal void setCompanyVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.Company);
            CompanyContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("4") as ImageSource;
        }

        /// <summary>
        /// applying Phone SecureNotes visibility
        /// </summary>
        internal void setSecureNotesVisibility()
        {
            SelectedCategory = Categories.SingleOrDefault(x => x.Id == DefaultCategories.SecureNotes);
            SecureNotesContentVisibility = true;
            NewItemImage = System.Windows.Application.Current.FindResource("10") as ImageSource;
        }


        internal void DefaultViewShare()
        {

            RecipientEmail = "";
            Message = "";
            ExpirationPeriodIndex = 0;
           
        }
        /// <summary>
        /// default properties of personal info addcontrol
        /// </summary>
        internal void DefaultView()
        {
            ReadonlySecureItem = false;
            IsValidErrorMessageVisible = false;
            PopulateCategories();
            PopulateShareDurations();

            DeleteButtonVisible = Visibility.Hidden;
            DateCreated = null;
            DateModified = null;
            DatesVisibility = false;

            SelectedCategory = null;
            string settingsCountry = pbData.GetPrivateSetting(DefaultProperties.Settings.Country);
            SelectedCountry = Countries.FirstOrDefault(x => x.Key == settingsCountry);
            newItemImage = null;

            RecipientEmail = "";
            Message = "";
            SelectedIndexTabControl = 0;
            NamesFirstName = "";
            NamesMiddleName = "";
            NamesLastName = "";
            NamesNotes = "";
            AddressNickname = "";
            Address1 = "";
            Address2 = "";
            AddressAptSuit = "";
            AddressCity = "";
            AddressState = "";
            AddressZipCode = "";
            AddressNotes = "";

            PhoneNickname = "";
            PhoneNumber = "";
            PhoneNotes = "";


            EmailNickName = "";
            Email = "";
            EmailNotes = "";

            LicenseNickname = "";
            LicenseFirstName = "";
            LicenseLastName = "";
            LicenseCountry = Countries.FirstOrDefault(x => x.Key == settingsCountry);
            LicenseState = "";
            LicenseNumber = "";
            LicenseExpires = "";
            LicenseIssueDate = "";

            MemberIdNickname = "";
            MemberIdFirstName = "";
            MemberIdLastName = "";
            MemberId = "";
            MemberIdNotes = "";

            //SocialSecurityNationality = "";
            SocialSecuritySelectedNationality = Countries.FirstOrDefault(x => x.Key == settingsCountry);
            SocialSecurityFirstName = "";
            SocialSecurityLastName = "";
            SocialSecurityDateOfBirth = "";
            SocialSecurityNumber = "";

            PassportNickName = "";
            PassportFirstName = "";
            PassportLastName = "";
            //PassportNationality = "";
            PassportSelectedNationality = Countries.FirstOrDefault(x => x.Key == settingsCountry);
            PassportDateOfBirth = "";
            PassportNumber = "";
            PassportIssueDate = "";
            PassportExpiers = "";
            PassportPlaceOfIssue = "";
            Female = false;
            Male = false;
            Company = "";
            CompanyNotes = "";

            SecureNoteTitle = "";
            SecureNoteNotes = "";

            SecuerShareData = null;
            ExpirationPeriodIndex = 0;

            if(_addControlScrollViewer != null)
            {
                _addControlScrollViewer.ScrollToHome();
            }

            Clear();
        }

        internal void Clear()
        {
            defaultSecureItem = CreateSecureItem();
            this.secureItem = null;
        }


       
        # endregion

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
        private void InvalidShareDialogOkClick(object obj)
        {
            InvalidShareDialogVisibility = false;
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
                foreach (string property in NamesContentVisibility ? ValidatedPropertiesNames : AddressContentVisibility ? ValidatedPropertiesAddress : PhoneContentVisibility ? ValidatedPropertiesPhone :
                    EmailContentVisibility ? ValidatedPropertiesEmail : LicenseContentVisibility ? ValidatedPropertiesLicense : MemberContentVisibility ? ValidatedPropertiesMember :
                    SocialSecurityContentVisibility ? ValidatedPropertiesSocialSecurity : PassportContentVisibility ? ValidatedPropertiesPassport : CompanyContentVisibility ? ValidatedPropertiesCompany : ValidatedPropertiesSecureNotes)
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
        static readonly string[] ValidatedPropertiesNames = { "NamesFirstName", "NamesLastName"};
        static readonly string[] ValidatedPropertiesAddress = { "Address1" };
        static readonly string[] ValidatedPropertiesPhone = { "PhoneNumber" };
        static readonly string[] ValidatedPropertiesEmail = { "Email" };
        static readonly string[] ValidatedPropertiesLicense = { "LicenseNumber" };
        static readonly string[] ValidatedPropertiesMember = { "MemberId" };
        static readonly string[] ValidatedPropertiesSocialSecurity = { "SocialSecurityNumber" };
        static readonly string[] ValidatedPropertiesPassport = { "PassportNumber" };
        static readonly string[] ValidatedPropertiesCompany = { "Company" };
        static readonly string[] ValidatedPropertiesSecureNotes = { "SecureNoteTitle"};

        string GetValidationErrors(string propertyName)
        {
            string error = String.Empty;
            switch (propertyName)
            {
                case "SelectedCategory":
                    error = ValidateRequiredCategory(SelectedCategory);
                    break;
                case "SelectedCountry":
                    error = ValidateRequiredKeyValue(SelectedCountry);
                    break;

                /*static readonly string[] ValidatedPropertiesNames = { "NamesFirstName", "NamesLastName", "SelectedCategory" };*/
                case "NamesFirstName":
                    error = ValidateRequiredField(NamesFirstName);
                    break;
                case "NamesLastName":
                    error = ValidateRequiredField(NamesLastName);
                    break;

                /*static readonly string[] ValidatedPropertiesAddress = { "AddressNickname", "SelectedCountry", "Address1", "Address2", "AddressAptSuit", "AddressCity", "AddressState", "AddressZipCode", "SelectedCategory" };*/
                case "AddressNickname":
                    error = ValidateRequiredField(AddressNickname);
                    break;
                case "Address1":
                    error = ValidateRequiredField(Address1);
                    break;
                case "Address2":
                    error = ValidateRequiredField(Address2);
                    break;
                case "AddressAptSuit":
                    error = ValidateRequiredField(AddressAptSuit);
                    break;
                case "AddressCity":
                    error = ValidateRequiredField(AddressCity);
                    break;
                case "AddressState":
                    error = ValidateRequiredField(AddressState);
                    break;
                case "AddressZipCode":
                    error = ValidateRequiredField(AddressZipCode);
                    break;


                /*static readonly string[] ValidatedPropertiesPhone = { "PhoneNickname", "PhoneNumber", "SelectedCategory" };*/
                case "PhoneNickname":
                    error = ValidateRequiredField(PhoneNickname);
                    break;
                case "PhoneNumber":
                    error = ValidateRequiredField(PhoneNumber);
                    break;

                /*        static readonly string[] ValidatedPropertiesEmail = { "EmailNickName", "Email", "SelectedCategory" };*/
                case "EmailNickName":
                    error = ValidateRequiredField(EmailNickName);
                    break;
                case "Email":
                    error = ValidateRequiredField(Email);
                    break;

                /*static readonly string[] ValidatedPropertiesLicense = { "LicenseNickname", "LicenseFirstName", "LicenseLastName", "LicenseCountry", "LicenseState", "LicenseNumber", "LicenseExpires", "LicenseIssueDate", "SelectedCategory" };*/
                case "LicenseNickname":
                    error = ValidateRequiredField(LicenseNickname);
                    break;
                case "LicenseFirstName":
                    error = ValidateRequiredField(LicenseFirstName);
                    break;
                case "LicenseLastName":
                    error = ValidateRequiredField(LicenseLastName);
                    break;
                case "LicenseCountry":
                    error = ValidateRequiredKeyValue(LicenseCountry);
                    break;
                case "LicenseState":
                    error = ValidateRequiredField(LicenseState);
                    break;
                case "LicenseNumber":
                    error = ValidateRequiredField(LicenseNumber);
                    break;
                case "LicenseExpires":
                    error = ValidateRequiredField(LicenseExpires);
                    break;
                case "LicenseIssueDate":
                    error = ValidateRequiredField(LicenseIssueDate);
                    break;



                /*        static readonly string[] ValidatedPropertiesMember = { "MemberIdNickname", "MemberIdFirstName", "MemberIdLastName", "MemberId", "SelectedCategory" };*/
                case "MemberIdNickname":
                    error = ValidateRequiredField(MemberIdNickname);
                    break;
                case "MemberIdFirstName":
                    error = ValidateRequiredField(MemberIdFirstName);
                    break;
                case "MemberIdLastName":
                    error = ValidateRequiredField(MemberIdLastName);
                    break;
                case "MemberId":
                    error = ValidateRequiredField(MemberId);
                    break;


                /*static readonly string[] ValidatedPropertiesSocialSecurity = { "SocialSecurityNationality", "SocialSecurityFirstName", "SocialSecurityLastName", "SocialSecurityDateOfBirth", "SocialSecurityNumber" };*/
                case "SocialSecurityNationality":
                    //error = ValidateRequiredField(SocialSecurityNationality);
                    error = ValidateRequiredKeyValue(SocialSecuritySelectedNationality);
                    break;
                case "SocialSecurityFirstName":
                    error = ValidateRequiredField(SocialSecurityFirstName);
                    break;
                case "SocialSecurityLastName":
                    error = ValidateRequiredField(SocialSecurityLastName);
                    break;
                case "SocialSecurityDateOfBirth":
                    error = ValidateRequiredField(SocialSecurityDateOfBirth);
                    break;
                case "SocialSecurityNumber":
                    error = ValidateRequiredField(SocialSecurityNumber);
                    break;


                /*static readonly string[] ValidatedPropertiesPassport = { "PassportNickName","PassportFirstName", "PassportLastName", "PassportNationality", "PassportDateOfBirth", "PassportNumber", "PassportIssueDate", "PassportExpiers", "PassportPlaceOfIssue" };*/
                case "PassportNickName":
                    error = ValidateRequiredField(PassportNickName);
                    break;
                case "PassportFirstName":
                    error = ValidateRequiredField(PassportFirstName);
                    break;
                case "PassportLastName":
                    error = ValidateRequiredField(PassportLastName);
                    break;
                case "PassportNationality":
                    //error = ValidateRequiredField(PassportNationality);
                    error = ValidateRequiredKeyValue(PassportSelectedNationality);
                    break;
                case "PassportDateOfBirth":
                    error = ValidateRequiredField(PassportDateOfBirth);
                    break;
                case "PassportNumber":
                    error = ValidateRequiredField(PassportNumber);
                    break;
                case "PassportIssueDate":
                    error = ValidateRequiredField(PassportIssueDate);
                    break;
                case "PassportExpiers":
                    error = ValidateRequiredField(PassportExpiers);
                    break;
                case "PassportPlaceOfIssue":
                    error = ValidateRequiredField(PassportPlaceOfIssue);
                    break;
                /* static readonly string[] ValidatedPropertiesCompany = { "Company", "SelectedCategory" };*/
                case "Company":
                    error = ValidateRequiredField(Company);
                    break;

                /* static readonly string[] ValidatedPropertiesSecureNotes = { "SecureNoteTitle", "SecureNoteNotes", "SelectedCategory" }; */
                case "SecureNoteTitle":
                    error = ValidateRequiredField(SecureNoteTitle);
                    break;
                case "SecureNoteNotes":
                    error = ValidateRequiredField(SecureNoteNotes);
                    break;

            };
            return error;
        }


        private string ValidateRequiredField(string fieldValue)
        {
            fieldValue = fieldValue != null ? fieldValue.Trim() : null;
            if (string.IsNullOrEmpty(fieldValue)) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        private string ValidateRequiredKeyValue(KeyValuePair<string, string>? fieldValue)
        {
            if (!fieldValue.HasValue) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        private string ValidateRequiredCategory(PasswordBoss.DTO.Folder fieldValue)
        {
            if (fieldValue == null) return System.Windows.Application.Current.FindResource("ValidationTextMessage").ToString();
            return String.Empty;
        }
        #endregion
    }
}
