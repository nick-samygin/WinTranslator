using PasswordBoss.PBAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PasswordBoss.Helpers
{
	internal static class DefaultCategories
	{
		public const string CategoryBankAccount = "CategoryBankAccount";
		public const string CategoryGames = "CategoryGames";
		public const string CategoryContactData = "CategoryContactData";
		public const string CategoryBusiness = "CategoryBusiness";
		public const string CategoryEntertainment = "CategoryEntertainment";
		public const string CategoryFinance = "CategoryFinance";
		public const string CategoryDriverLicense = "CategoryDriverLicense";
		public const string CategorySocialSecurity = "CategorySocialSecurity";
		public const string CategoryNames = "CategoryNames";
		public const string CategoryShopping = "CategoryShopping";
		public const string CategorySocialMedia = "CategorySocialMedia";
		public const string CategoryOther = "CategoryOther";
		public const string CategorySecureNotes = "CategorySecureNotes";
		public const string CategoryMemberIDs = "CategoryMemberIDs";
		public const string CategoryPassport = "CategoryPassport";
		public const string CategoryCompany = "CategoryCompany";
		public const string CategoryPhoneNumbers = "CategoryPhoneNumbers";
		public const string CategoryCreditCard = "CategoryCreditCard";
		public const string CategoryEmail = "CategoryEmail";
		public const string CategoryAddresses = "CategoryAddresses";
		public const string CategoryPaypal = "CategoryPaypal";
	}

	internal static class ShareStatus
	{
		public const string Waiting = "waiting";
		public const string Waiting4Data = "waiting4data";
		public const string Pending = "pending";
		public const string Accepted = "accepted";
		public const string Sent = "sent";
		public const string Shared = "shared";
		public const string Rejected = "rejected";
		public const string Expired = "expired";
		public const string Revoked = "revoked";
		public const string Updated = "updated";
		public const string Invalid = "invalid";
		public const string Canceled = "canceled";
		public const string Removed = "removed";
	}

	internal static class ShareAction
	{
		public const string Accept = "accept";
		public const string AcceptRequest = "acceptRequest";
		public const string ShareData = "shareData";
		public const string Reject = "reject";
		public const string Revoke = "revoke";
		public const string Cancel = "cancel";
		public const string Resend = "resend";
		public const string Remove = "remove";
	}

	internal static class DefaultProperties
	{
		public const string Settings_DefaultCountryCode = "us";

		public const string Settings_Country = "country";
		public const string Settings_Name = "name";
		public const string Settings_Email = "email";
		public const string Settings_Mobile = "mobile";
		public const string Settings_CloudStorage = "enableStorageCloudBackup";
		public const string Settings_SearchProviderUrl = "search_provider_url";
		public const string Settings_StartPageUrl = "start_page_url";

		public const string Settings_General_AutoFill = "auto_fill";
		public const string Settings_General_OpenOnStartup = "open_on_startup";
		public const string Settings_General_TwoStepVerification = "two_step_verification";

		public const string Settings_Security_PasswordAutoLock = "password_auto_lock";

		public const string Settings_Advanced_AutoLogin = "auto_login";

		public const string Settings_Advanced_RequireMasterPasswordForAll = "require_master_password_for_all_form_filing";

		public const string Settings_Advanced_TurnOffPassSaving = "turn_off_pass_saving";

		public const string Settings_Advanced_DisableStatusMessages = "disable_status_messages";
		public const string Settings_Advanced_AutoStoreData = "auto_store_data";
		public const string Settings_Advanced_ClearPasswordsFromBrowsers = "clear_passwords_from_browsers";
		public const string Settings_Device_IsDeviceTrusted = "is_device_trusted";
		public const string Settings_Device_IsDeviceTrustedLastChecked = "is_device_trusted_last_checked";
		public const int Settings_Device_IsDeviceTrustedLastCheckedMinDays = 30; //number of days that must pass from last pin enter if device is trusted

        public const string Settings_SetupProgress_CurrentStep = "current_setup_progress_step";
        public const string Settings_SetupProgress_DefaultStep = "2";
        public const string Configuration_DefaultAccount = "defaults@local.install";

		public const string Configuration_Key_RememberEmail = "rememberEmail";
		public const string Configuration_Key_LastLoginEmail = "lastLoginEmail";
		public const string Configuration_Key_GettingStartedOnStartup = "dontGettingStartedOnStartup";
		public const string Configuration_Key_SetupWizardFinished = "setupWizardFinished";
		public const string Configuration_Key_ProductTourOnStartup = "dontProductTourOnStartup";
		public const string Configuration_Key_PasswordVaultInfo = "passwordVaultInfoScreen";

		public const string Configuration_Key_SecurityScoreInfo = "securityScoreInfoScreen";

		public const string Configuration_Purchase_Url = "purchase_url";
		public const string Configuration_Purchase_Url_InApp = "purchase_url_in_app";
		public const string Configuration_Purchase_Url_Discount = "discount_buy_url";

		public const string Configuration_Key_EnablePinAccess = "enablePinAccess";

        public const string SecurityItemType_PasswordVault = "PV";
        public const string SecurityItemType_DigitalWallet = "DW";
        public const string SecurityItemType_PersonalInfo = "PI";
        public const string SecurityItemType_SecureNotes = "SN";
        public const string SecurityItemType_PasswordGenerator = "PG";
			internal static class PurchaseUrl
			{
				public const string Url = "purchase_url";
				public const string InAppUrl = "purchase_url_in_app";
			}
		

        public const string SecurityItemSubType_PV_Login = "Website"; // renamed from PV
        public const string SecurityItemSubType_PV_Website = "Website"; // renamed from PV
        public const string SecurityItemSubType_PV_Application = "Application";
        public const string SecurityItemSubType_PV_Database = "Database";
        public const string SecurityItemSubType_PV_EmailAccount = "EmailAccount";
        public const string SecurityItemSubType_PV_InstantMessenger = "InstantMessenger";
        public const string SecurityItemSubType_PV_Server = "Server";
        public const string SecurityItemSubType_PV_SSHKey = "SSHKey";
        public const string SecurityItemSubType_PV_WiFi = "WiFi";

        [ObsoleteAttribute("This property is obsolete. Should be removed.", false)]
        public const string SecurityItemSubType_DW_Paypal = "Paypal";
        public const string SecurityItemSubType_DW_Bank = "Bank";
        public const string SecurityItemSubType_DW_CreditCard = "CreditCard";


        public const string SecurityItemSubType_PI_Names = "Names";
        public const string SecurityItemSubType_PI_Address = "Address";
        public const string SecurityItemSubType_PI_PhoneNumber = "PhoneNumber";
        public const string SecurityItemSubType_PI_Company = "Company";
        public const string SecurityItemSubType_PI_Email = "Email";
                      
        [ObsoleteAttribute("This property is obsolete. Use SecurityItemSubType_PI_GenericNote instead.", false)]
        public const string SecurityItemSubType_PI_SecureNotes = "SecureNotes";

        public const string SecurityItemSubType_PI_SocialSecurity = "SocialSecurity";
        public const string SecurityItemSubType_SN_Passport = "Passport";
        public const string SecurityItemSubType_SN_DriverLicense = "DriverLicense";
        public const string SecurityItemSubType_SN_MemberIDs = "MemberIDs";
        public const string SecurityItemSubType_SN_AlarmCode = "AlarmCode";
        public const string SecurityItemSubType_SN_EstatePlan = "EstatePlan";
        public const string SecurityItemSubType_SN_FrequentFlyer = "FrequentFlyer";
        public const string SecurityItemSubType_SN_GenericNote = "GenericNote";
        public const string SecurityItemSubType_SN_HealthInsurance = "HealthInsurance";
        public const string SecurityItemSubType_SN_HotelRewards = "HotelRewards";
        public const string SecurityItemSubType_SN_Insurance = "Insurance";
        public const string SecurityItemSubType_SN_Prescription = "Prescription";
        public const string SecurityItemSubType_SN_SoftwareLicense = "SoftwareLicense";

		public const string ImagesDefaultDirectory = "\\db\\images";
		public const string GoogleS2 = @"http://www.google.com/s2/favicons?domain=";
		public const string PasswordBossBlockedSiteUrl = @"http://www.passwordboss.com/blocked";
		public const string LastImageSyncDate = "last_image_sync_date";

		public const string Site_SiteType_Desktop = "desktop_login";
		public const string Site_SiteType_Full = "full";
		public const string Site_LookupMethod_Full = "full";
		public const string Site_LookupMethod_Domain = "domain";
		public static string CurrentImageDirectory
		{
			get
			{
				return AppHelper.ImageFolderLocation;
			}
		}

		internal const string LoginShowEye = "imgLoginShowEye";
		internal const string LoginHideEye = "imgLoginHideEye";
		internal const string EyeClose = "imgEyeClose";
		internal const string Dropbox = "imgDropbox";
		internal const string Amazon = "imgAmazon";
		internal const string Hulu = "imgHulu";
		internal const string SecurityAlertIcon = "imgSecurityAlertIcon";
		internal const string EyeHoverClose = "imgEyeHoverClose";
		internal const string LoginHideEyeIcon = "imgLoginHideEye";
		internal const string EyeCloseIcon = "imgEyeHoverClose";
		internal const string CurrentLoginWindow = "LoginWindow";
		internal const string VerificationOffFocusStyle = "VerificationTextBoxStyle";
		internal const string CodeOffFocusStyle = "CodeTextBoxStyle";
		internal const string EmailOffFocusStyle = "EmailTextBoxStyle";
		internal const string EmailOnFocusStyle = "TextBoxStyle";
		internal const string PasswordBoxOnFocusStyle = "PasswordBoxStyle";
		internal const string ConfirmPasswordBoxOffFocusStyle = "ConfirmrPasswordStyle";
		internal const string PasswordBoxOffFocusStyle = "PasswordBoxEnterPasswordStyle";
		internal const string SearchOffFocusStyle = "SearchTextboxNormalStyle";
		internal const string SearchOnFocusStyle = "SearchTextboxGotFocusStyle";
		internal const string Dashboard = "dashboard";
		internal const string login = "login";
		internal const string Searhbox = "searchbox";
		internal const string Foregroundselectedcolor = "WhiteColor";
		internal const string FillEllipsesColor = "PasswordBossGreenColor";
		internal const string SecurityScoreDynamicContentColor = "SecurityScoreDynamicContentColor";
		public const string LoginViewPath = "..\\Views\\Login\\Login.xaml";
		public const string CreateAccountViewPath = "..\\Views\\Login\\CreateAccount.xaml";
		public const string confirmPWD = "..\\Views\\Login\\ConfirmMasterPassword.xaml";
		public const string UpdateDataViewPath = "..\\Views\\Login\\UpdateData.xaml";
		public const string ProductViewPath = "..\\Views\\Login\\ProductTour.xaml";
		public const string SetPinViewPath = "..\\Views\\Login\\SetPINScreen.xaml";
		public const string VerificationRequiredViewPath = "..\\Views\\Login\\VerificationRequired.xaml";
		public const string EnterPinViewPath = "..\\Views\\Login\\EnterPINCodeScreen.xaml";
		public const string SetupCompleteViewPath = "..\\Views\\Login\\SetupComplete.xaml";
		internal const string ForegroundDefaultColor = "LightGrayTextForegroundColor";
		internal const string ForegroundChangeColor = "AccountSettingsGrayBackgroundColor";
		public const string LoginPINFailedViewPath = "..\\Views\\Login\\LoginPINFailed.xaml";
		internal const string MasterPasswordStyle = "PasswordBoxEnterPasswordStyle";
		internal const string ToggleEyeBigIconStyle = "ToggleEyeBigIconStyle";
		internal const string ToggleEyeSmallIconStyle = "ToggleEyeSmallIconStyle";
		internal const string ConfirmMasterPasswordStyle = "ConfirmMasterPasswordBoxStyle";

		internal const string SemiboldWeight = "SemiboldWeight";
		internal const string NormalWeight = "NormalWeight";
		internal const string ProximaRegularFamily = "ProximaRegularFamily";
		internal const string ProximaSemiBoldfamily = "ProximaSemiBoldfamily";

		public const string LinkLearnMore = "https://support.passwordboss.com/customer/portal/articles/1839157-what-is-2-step-verification-?b_id=6281&utm_source=PC&utm_medium=Wizard&utm_campaign=2StepSetup";
		public const string LinkAuthenticatorApps = "https://support.passwordboss.com/customer/portal/articles/1958846-what-2-step-verification-apps-does-password-boss-support-?b_id=6279&utm_source=PC&utm_medium=Wizard&utm_campaign=2StepSetup";
		public const string LinkImportFromSecureExport = "https://support.passwordboss.com/customer/portal/articles/1962472-how-do-i-import-passwords-from-a-password-boss-export-file-?b_id=6279&utm_source=PC&utm_medium=Wizard&utm_campaign=ImportFromPB";
		public const string LinkImportFromOtherPasswordManager = "https://support.passwordboss.com/customer/portal/articles/1838969-can-i-import-passwords-from-another-password-manager-?b_id=6279";
		public const string LinkLostMyPhone = "https://support.passwordboss.com/customer/portal/articles/1839168-i-lost-my-phone-and-2-step-verification-is-enabled-how-do-i-access-password-boss-?b_id=6281&utm_source=PC&utm_medium=Login&utm_campaign=LostPhone";

		public const string InAppSupportTrayLink = "https://www.passwordboss.com/support/?utm_source=PC&utm_medium=Tray&utm_campaign=InAppSupport";
		public const string InAppSupportMenuLink = "https://www.passwordboss.com/support/?utm_source=PC&utm_medium=menu&utm_campaign=InAppSupport";
		public const string InAppSupportGettingStartedLink = "https://support.passwordboss.com/?b_id=6279&utm_source=PC&utm_medium=menu&utm_campaign=InAppSupport";
		public const string InAppCommunitySupportLink = "https://community.passwordboss.com/?utm_source=PC&utm_medium=menu&utm_campaign=InAppSupport";
		public const string InAppSupportPrivacyPolicyMenuLink = "http://www.passwordboss.com/privacy-policy/?utm_source=PC&utm_medium=menu&utm_campaign=InAppSupport";
		public const string InAppTermsAndConditionsMenuLink = "http://www.passwordboss.com/terms-of-use/?utm_source=PC&utm_medium=menu&utm_campaign=InAppSupport";
		public const string InAppPortalLoginLink = "https://portal.passwordboss.com/?utm_source=PC&utm_medium=InApp&utm_campaign=PortalLogin";
		public const string InAppFirefoxInstalledLink = "http://www.passwordboss.com/firefox-installed/?utm_source=PC&utm_medium=menu&utm_campaign=InstallBrowserExtension";

		public const string Features_PasswordValt_AddManageLogins = "1002";
		public const string Features_PasswordValt_AutoLogin = "1003";
		public const string Features_PasswordValt_ManageFavorites = "1004";

		public const string Features_DigitalWallet_AddManageCreditCard = "2001";
		public const string Features_DigitalWallet_AddManageBankAccount = "2002";

		public const string Features_PersonalInfo_AddManageDriverLicense = "3001";
		public const string Features_PersonalInfo_AddManagePassport = "3002";
		public const string Features_PersonalInfo_AddManageAddress = "3003";
		public const string Features_PersonalInfo_AddManagePhone = "3004";
		public const string Features_PersonalInfo_AddManageCompany = "3005";
		public const string Features_PersonalInfo_AddManageEmail = "3006";
		public const string Features_PersonalInfo_AddManageMemberId = "3007";
		public const string Features_PersonalInfo_AddManageSSN = "3008";
		public const string Features_PersonalInfo_AddManageSecureNotes = "3009";
		public const string Features_PersonalInfo_ManageFavorites = "3010";

		public const string Features_SecureBrowser_AccessSecureBrowser = "4001";
		public const string Features_PasswordGenerator_AccessPasswordGenerator = "5001";

		public const string Features_SecurityScore_ShowSecurityScore = "7001";

		public const string Features_Miscellaneous_ShowNotificationAndAlerts = "8002";
		public const string Features_Miscellaneous_TwoStepAuthentication = "8003";
		public const string Features_Miscellaneous_AccountManagement = "8005";

		public const string Features_SyncAndCloudStorage_SyncAcrossDevices = "9001";
		public const string Features_SyncAndCloudStorage_OnlineBackup = "9002";
		public const string Features_SyncAndCloudStorage_ChooseDataCenter = "9003";

		public const string Features_ShareCenter_UpTo5Shares = "6001";
		public const string Features_ShareCenter_UnlimitedShares = "6002";
		public const string Features_ShareCenter_ManageShares = "6003";

		/*
			Add/Manage Driver License	3001
			Add/Manage Passport	3002
			Add/Manage Address	3003
			Add/Manage Phone	3004
			Add/Manage Company	3005
			Add/Manage Email	3006
			Add/Manage Member ID	3007
			Add/Manage Social Security Number	3008
			Add/Manage Secure Notes	3009
			Manage Favorites	3010
		 */

		/// <summary>
		/// this function returns Transparent color
		/// </summary>
		/// <returns></returns>
		internal static Brush TransparentColor()
		{
			return new SolidColorBrush(Colors.Transparent);
		}

		/// <summary>
		/// this function returns default properties of background color
		/// </summary>
		/// <returns></returns>
		internal static Brush AlertBackgroundcolor()
		{
			return (Brush)Application.Current.FindResource("AlertButtonBackgroundcolor");
		}

		/// <summary>
		/// this function returns default properties of background color
		/// </summary>
		/// <returns></returns>
		internal static Brush AlertBorderStrokecolor(string KeyName)
		{
			return (Brush)Application.Current.FindResource(KeyName);
		}

		/// <summary>
		/// this function returns default properties of background color
		/// </summary>
		/// <returns></returns>
		internal static Brush ReturnEllipsesColor(int index)
		{
			Brush returnColor = null;
			switch (index)
			{
				case 1:
					returnColor = (Brush)Application.Current.FindResource("FillEllipsesDefaultcolor");
					break;
				case 2:
					returnColor = (Brush)Application.Current.FindResource("PasswordBossGreenColor");
					break;
			}
			return returnColor;
		}

		/// <summary>
		/// this function returns default properties of background color for text
		/// </summary>
		/// <returns></returns>
		internal static Brush DefaultTextForegroundColor()
		{
			return (Brush)Application.Current.FindResource("LightGrayTextForegroundColor");
		}

		internal static Brush ReturnTextForegroundColor(string resource)
		{
			return (Brush)Application.Current.FindResource(resource);
		}

		/// <summary>
		/// this function returns default properties of background color for text
		/// </summary>
		/// <returns></returns>
		internal static Brush SearchBoxGridBackgroundColor(int index)
		{
			Brush returnColor = null;
			switch (index)
			{
				case 1:
					returnColor = (Brush)Application.Current.FindResource("TransparentColor");
					break;
				case 2:
					returnColor = (Brush)Application.Current.FindResource("HiglightedBorderStrokecolor");
					break;
			}
			return returnColor;
		}

		/// <summary>
		/// this function returns string
		/// </summary>
		/// <returns></returns>
		internal static string ReturnString(string resource)
		{
			return (string)Application.Current.FindResource(resource);
		}

		/// <summary>
		/// this function returns image for wizard
		/// </summary>
		/// <returns></returns>
		internal static ImageSource ReturnImage(string resource)
		{
			return (ImageSource)Application.Current.FindResource(resource);
		}

		/// <summary>
		/// this function returns font family
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		internal static FontFamily ReturnFontFamily(string resource)
		{
			return (FontFamily)Application.Current.FindResource(resource);
		}

		/// <summary>
		/// this function returns font weight
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		internal static FontWeight ReturnFontWeight(string resource)
		{
			return (FontWeight)Application.Current.FindResource(resource);
		}

		public static PIItemType GetPIEventTypeBySecureItemType(string type)
		{
			switch (type)
			{
                case DefaultProperties.SecurityItemSubType_PI_Names: return PIItemType.Name;
                case DefaultProperties.SecurityItemSubType_PI_Address: return PIItemType.Address;
                case DefaultProperties.SecurityItemSubType_PI_PhoneNumber: return PIItemType.Phone;
                case DefaultProperties.SecurityItemSubType_PI_Email: return PIItemType.Email;
                case DefaultProperties.SecurityItemSubType_SN_DriverLicense: return PIItemType.DriversLicence;
                case DefaultProperties.SecurityItemSubType_SN_MemberIDs: return PIItemType.MemberId;
                case DefaultProperties.SecurityItemSubType_PI_SocialSecurity: return PIItemType.SSN;
                case DefaultProperties.SecurityItemSubType_SN_Passport: return PIItemType.Passport;
                case DefaultProperties.SecurityItemSubType_PI_Company: return PIItemType.Company;
                case DefaultProperties.SecurityItemSubType_PI_SecureNotes: return PIItemType.SecureNote;
				default: throw new ArgumentException("Undefined secure item type= " + type);
			}
		}
	}
}