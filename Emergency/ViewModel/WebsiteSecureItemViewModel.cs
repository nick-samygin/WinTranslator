using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PasswordBoss;
using PasswordBoss.DTO;

namespace Emergency.ViewModel
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

                var imagePath = GetImagePathForSite(item.Site.UUID, false);
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

            var imagePath = GetImagePathForSite(item.ImageId, false);
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

        private string ValidateUrl()
        {
            if (!IsUrlValid(Url)) return Application.Current.FindResource("ValidationTextMessage").ToString();
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

        #region common
        public bool IsImageForSiteExisting(string siteUUID)
        {
            bool isExisting = false;

            if (siteUUID != null)
            {
                string imageDirectory = AppHelper.ImageFolderLocation;
                string imageFileName = null;
                if (siteUUID != null)
                {
                    imageFileName = imageDirectory + siteUUID + ".png";
                }

                isExisting = File.Exists(imageFileName);
            }

            return isExisting;
        }

        public string GetImagePathForSite(string siteUUID = null, bool returnDefultIfNeeded = true)
        {
            if (IsImageForSiteExisting(siteUUID))
            {
                string imageDirectory = AppHelper.ImageFolderLocation;
                string imageFileName = null;
                if (siteUUID != null)
                {
                    imageFileName = imageDirectory + siteUUID + ".png";
                }

                return imageFileName;
            }

            if (returnDefultIfNeeded)
            {
                var img = Application.Current.Resources["imgPbOwlGrey"] as BitmapImage;
                if (img != null)
                {
                    return img.ToString();
                }
            }
            return null;
        }

        public bool IsUrlValid(string url, UriKind uriKind = UriKind.RelativeOrAbsolute)
        {
            Uri result;
            if (Uri.TryCreate(url, uriKind, out result))
            {
                if (result.IsAbsoluteUri && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
                {
                    return true;
                }
                else if (!result.IsAbsoluteUri)
                {
                    return true;
                }


            }
            return false;
        }
        #endregion
    }
}
