using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.ViewModel
{
    public class WebsiteSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string urlFromDb;

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                IsValidUrl = !string.IsNullOrEmpty(Url);
                RaisePropertyChanged("Url");
            }
        }

        private bool isValidUrl = true;
        public bool IsValidUrl
        {
            get { return isValidUrl; }
            set
            {
                isValidUrl = value;
                RaisePropertyChanged("IsValidUrl");
            }
        }

       

        private bool subDomain;
        public bool SubDomain
        {
            get { return subDomain; }
            set
            {
                subDomain = value;
                RaisePropertyChanged("SubDomain");
            }
        }

        private bool useSecureBrowser;
        public bool UseSecureBrowser
        {
            get { return useSecureBrowser; }
            set
            {
                useSecureBrowser = value;
                RaisePropertyChanged("UseSecureBrowser");
            }
        }

        private bool requireMasterPassword;
        public bool RequireMasterPassword
        {
            get { return requireMasterPassword; }
            set
            {
                requireMasterPassword = value;
                RaisePropertyChanged("RequireMasterPassword");
            }
        }

        public override bool IsWebSite
        {
            get
            {
                return true;

            }

        }

        public WebsiteSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login;
            hasAddvancedSettings = true;

            InitCommand();

        }

        private  Common _common = new Common();

        public WebsiteSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
           
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login;
            hasAddvancedSettings = true;
            Username = item.Data.username != null ? item.Data.username : item.Data.email;
            Password = item.Data.password;
            LastAccess = item.LastAccess;

            if (item.Site != null)
            {
                Url = item.Site.Uri;
                urlFromDb = item.Site.Uri;
                Autologin = item.Data.autologin;
                SubDomain = item.Data.sub_domain;
                UseSecureBrowser = item.Data.use_secure_browser;
                RequireMasterPassword = item.Data.require_master_password;

                var imagePath = _common.GetImagePathForSite(item.Site.UUID, false);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    IsDefaultImage = false;
                    CanPickColor = false;
                    Image = imagePath;
                }
            }

            InitCommand();           

        }

        public WebsiteSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login;
            hasAddvancedSettings = true;

            InitCommand();

            var imagePath = _common.GetImagePathForSite(item.ImageId, false);
            if (!string.IsNullOrEmpty(imagePath))
            {
                IsDefaultImage = false;
                CanPickColor = false;
                Image = imagePath;
            }            

        }

        private void InitCommand()
        {
            if (Actions != null)
            {

                var openWebSite = new SecureItemsCommon.Helpers.ContextAction()
                {
                    Action = OpenInBrowserCommand,
                    Name = Application.Current.FindResource("MenuOpenWebsite") as string,
                    Icon = Application.Current.Resources["menuOpenSiteGrey"] as ImageSource,
                    IconHover = Application.Current.Resources["menuOpenSiteGreen"] as ImageSource
                };

            
                Actions.Insert(0, openWebSite);
            }



        }
        
        private  string ValidateUrl()
        {
            if (!_common.IsUrlValid(Url)) return Application.Current.FindResource("ValidationTextMessage").ToString();
            return string.Empty; 
        }

        public override bool Validate()
        {
          
            bool urlChanged = Url != urlFromDb;
            if (!string.IsNullOrEmpty(ValidateUrl()))
            {
                IsValidUrl = false;
               
            }
            base.Validate();
            return IsValidUrl && IsValidName;
        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.LoginUrl = Url;
            secureItem.Site = new Site()
            {

                Name = String.IsNullOrEmpty(Name) ? Url : Name,
                Uri = Url
            };

            secureItem.Name = String.IsNullOrEmpty(Name) ? Url : Name;
            secureItem.Data.username = Username;
            secureItem.Data.require_master_password = RequireMasterPassword;
            secureItem.Data.autologin = Autologin;
            secureItem.Data.password = Password;
            secureItem.Data.sub_domain = SubDomain;
            secureItem.Data.use_secure_browser = UseSecureBrowser;
            
            return secureItem;
        }

    }

    public class AppSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string application;
        public string Application
        {
            get { return application; }
            set
            {
                application = value;
                RaisePropertyChanged("Application");
            }
        }

        private string appType;
        public string AppType
        {
            get { return appType; }
            set
            {
                appType = value;
                RaisePropertyChanged("AppType");
            }
        }

        public AppSecureItemViewModel()
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Application;
        }

        public AppSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Application;

            ListViewSecondName = string.Empty;

            Application = item.Data.application;
            AppType = item.Data.type;

        }

        public AppSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Application;
 

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.application = Application;
            secureItem.Data.password = Password;
            secureItem.Data.username = Username;
            secureItem.Data.type = AppType;

            return secureItem;
        }

    }

    public class DatabaseSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                RaisePropertyChanged("ServerAddress");
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
            }
        }

        private string database;
        public string Database
        {
            get { return database; }
            set
            {
                database = value;
                RaisePropertyChanged("Database");
            }
        }


        public DatabaseSecureItemViewModel()
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Database;
        }

        public DatabaseSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Database;

            ListViewSecondName = string.Empty;
            ServerAddress = item.Data.server_address;
            Port = item.Data.port;
            Database = item.Data.database;

        }

        public DatabaseSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Database;

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.server_address = ServerAddress;
            secureItem.Data.port = Port;
            secureItem.Data.password = Password;
            secureItem.Data.username = Username;
            secureItem.Data.database = Database;

            return secureItem;
        }

    }

    public class EmailAccountSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                RaisePropertyChanged("EmailAddress");
            }
        }

        public EmailAccountSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_EmailAccount;
        }
        public EmailAccountSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_EmailAccount;


            EmailAddress = item.Data.email;

        }

        public EmailAccountSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_EmailAccount;

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.email = EmailAddress;
            secureItem.Data.password = Password;
            secureItem.Data.username = Username;
            return secureItem;
        }

    }

    public class InstantMessengerSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string application;
        public string Application
        {
            get { return application; }
            set
            {
                application = value;
                RaisePropertyChanged("Application");
            }
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                RaisePropertyChanged("ServerAddress");
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
            }
        }

        public InstantMessengerSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_InstantMessenger;
        }

        public InstantMessengerSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_InstantMessenger;


            Application = item.Data.application;
            ServerAddress = item.Data.server_address;
            Port = item.Data.port;
        }

        public InstantMessengerSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_InstantMessenger;

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.server_address = ServerAddress;
            secureItem.Data.port = Port;
            secureItem.Data.password = Password;
            secureItem.Data.username = Username;
            secureItem.Data.application = Application;

            return secureItem;
        }
    }

    public class ServerSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string application;
        public string Application
        {
            get { return application; }
            set
            {
                application = value;
                RaisePropertyChanged("Application");
            }
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                RaisePropertyChanged("ServerAddress");
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
            }
        }

        public ServerSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Server;
        }

        public ServerSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Server;


            Application = item.Data.application;
            ServerAddress = item.Data.server_address;
            Port = item.Data.port;
        }

        public ServerSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_Server;

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.server_address = ServerAddress;
            secureItem.Data.port = Port;
            secureItem.Data.password = Password;
            secureItem.Data.username = Username;
            secureItem.Data.application = Application;

            return secureItem;
        }

    }

    public class SSHKeySecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        private string serverAddress;
        public string ServerAddress
        {
            get { return serverAddress; }
            set
            {
                serverAddress = value;
                RaisePropertyChanged("ServerAddress");
            }
        }

        private string port;
        public string Port
        {
            get { return port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
            }
        }

        private string bitStrength;
        public string BitStrength
        {
            get { return bitStrength; }
            set
            {
                bitStrength = value;
                RaisePropertyChanged("BitStrength");
            }
        }

        private string format;
        public string Format
        {
            get { return format; }
            set
            {
                format = value;
                RaisePropertyChanged("Format");
            }
        }

        private string passphrase;
        public string Passphrase
        {
            get { return passphrase; }
            set
            {
                passphrase = value;
                RaisePropertyChanged("Passphrase");
            }
        }

        private string publicKey;
        public string PublicKey
        {
            get { return publicKey; }
            set
            {
                publicKey = value;
                RaisePropertyChanged("PublicKey");
            }
        }

        private string privateKey;
        public string PrivateKey
        {
            get { return privateKey; }
            set
            {
                privateKey = value;
                RaisePropertyChanged("PrivateKey");
            }
        }

        public SSHKeySecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_SSHKey;
        }

        public SSHKeySecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_SSHKey;


            ListViewSecondName=ServerAddress = item.Data.server_address;
            Port = item.Data.port;
            BitStrength = item.Data.bit_strength;
            Format = item.Data.format;
            Passphrase = item.Data.passphrase;
            PublicKey = item.Data.public_key;
            PrivateKey = item.Data.private_key;
        }

        public SSHKeySecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_SSHKey;

        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.server_address = ServerAddress;
            secureItem.Data.port = Port;
            secureItem.Data.bit_strength = BitStrength;
            secureItem.Data.format = Format;
            secureItem.Data.passphrase = Passphrase;
            secureItem.Data.public_key = PublicKey;
            secureItem.Data.private_key = PrivateKey;

            return secureItem;
        }
    }

    public class WifiSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithPasswordViewModel
    {
        private string ssid;
        public string SSID
        {
            get { return ssid; }
            set
            {
                ssid = value;
                RaisePropertyChanged("SSID");
            }
        }

        private string authentication;
        public string Authentication
        {
            get { return authentication; }
            set
            {
                authentication = value;
                RaisePropertyChanged("Authentication");
            }
        }

        private string encryption;
        public string Encryption
        {
            get { return encryption; }
            set
            {
                encryption = value;
                RaisePropertyChanged("Encryption");
            }
        }

        private string fipsMode;
        public string FIPSMode
        {
            get { return fipsMode; }
            set
            {
                fipsMode = value;
                RaisePropertyChanged("FIPSMode");
            }
        }

        private string keyType;
        public string KeyType
        {
            get { return keyType; }
            set
            {
                keyType = value;
                RaisePropertyChanged("KeyType");
            }
        }

        public WifiSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_WiFi;
        }

        public WifiSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_WiFi;


            ListViewSecondName=SSID = item.Data.ssid;
            Authentication = item.Data.authentication;
            encryption = item.Data.encryption;
            FIPSMode = item.Data.fips_mode;
            KeyType = item.Data.key_type;
        }

        public WifiSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PV_WiFi;

        }


        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.ssid = SSID;
            secureItem.Data.authentication = Authentication;
            secureItem.Data.encryption = Encryption;
            secureItem.Data.fips_mode = FIPSMode;
            secureItem.Data.key_type = KeyType;
            secureItem.Data.password = Password;

            return secureItem;
        }
    }
}
