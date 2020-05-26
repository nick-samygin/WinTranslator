using PasswordBoss.Helpers;


namespace PasswordBoss.ViewModel
{
    public class WebsiteSecureItemViewModel : SecureItemWithPasswordViewModel
    {
        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                RaisePropertyChanged("Url");
            }
        }

        public WebsiteSecureItemViewModel()
        {
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_Login;
            hasAddvancedSettings = true;
        }
    }

    public class AppSecureItemViewModel : SecureItemWithPasswordViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_Application;
        }
    }

    public class DatabaseSecureItemViewModel : SecureItemWithPasswordViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_Database;
        }
    }

    public class EmailAccountSecureItemViewModel : SecureItemViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_EmailAccount;
        }
    }

    public class InstantMessengerSecureItemViewModel : SecureItemWithPasswordViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_InstantMessenger;
        }
    }

    public class ServerSecureItemViewModel : SecureItemWithPasswordViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_Server;
        }

    }

    public class SSHKeySecureItemViewModel : SecureItemViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_SSHKey;
        }
    }

    public class WifiSecureItemViewModel : SecureItemWithPasswordViewModel
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
            type = DefaultProperties.SecurityItemType_PasswordVault;
            subType = DefaultProperties.SecurityItemSubType_PV_WiFi;
        }
    }
}
