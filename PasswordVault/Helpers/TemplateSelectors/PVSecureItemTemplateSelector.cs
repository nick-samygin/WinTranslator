using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.Helpers
{
    public class PVSecureItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _websiteTemplate;
       
        public DataTemplate WebsiteTemplate
        {
            get
            {
                return _websiteTemplate;
            }
            set
            {
                _websiteTemplate = value;
            }
        }


        private DataTemplate _appTemplate;

        public DataTemplate AppTemplate
        {
            get
            {
                return _appTemplate;
            }
            set
            {
                _appTemplate = value;
            }
        }


        private DataTemplate _databaseTemplate;

        public DataTemplate DatabaseTemplate
        {
            get
            {
                return _databaseTemplate;
            }
            set
            {
                _databaseTemplate = value;
            }
        }

        private DataTemplate _emailAddressTemplate;

        public DataTemplate EmailAddressTemplate
        {
            get
            {
                return _emailAddressTemplate;
            }
            set
            {
                _emailAddressTemplate = value;
            }
        }

        private DataTemplate _instantMessengerTemplate;

        public DataTemplate InstantMessengerTemplate
        {
            get
            {
                return _instantMessengerTemplate;
            }
            set
            {
                _instantMessengerTemplate = value;
            }
        }

        private DataTemplate _serverTemplate;

        public DataTemplate ServerTemplate
        {
            get
            {
                return _serverTemplate;
            }
            set
            {
                _serverTemplate = value;
            }
        }

        private DataTemplate _sshKeyTemplate;

        public DataTemplate SSHKeyTemplate
        {
            get
            {
                return _sshKeyTemplate;
            }
            set
            {
                _sshKeyTemplate = value;
            }
        }

        private DataTemplate _wifiTemplate;

        public DataTemplate WifiTemplate
        {
            get
            {
                return _wifiTemplate;
            }
            set
            {
                _wifiTemplate = value;
            }
        }

        public PVSecureItemTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is WebsiteSecureItemViewModel)
                return WebsiteTemplate;

            if (item is AppSecureItemViewModel)
                return AppTemplate;

            if (item is DatabaseSecureItemViewModel)
                return DatabaseTemplate;

            if (item is EmailAccountSecureItemViewModel)
                return EmailAddressTemplate;

            if (item is InstantMessengerSecureItemViewModel)
                return InstantMessengerTemplate;

            if (item is ServerSecureItemViewModel)
                return ServerTemplate;

            if (item is SSHKeySecureItemViewModel)
                return SSHKeyTemplate;

            if (item is WifiSecureItemViewModel)
                return WifiTemplate;


            return WebsiteTemplate;

        }
    }
}
