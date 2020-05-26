using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
    public class PhoneSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithCountryViewModel
    {

        private string countryMobileCode;
        public string CountryMobileCode
        {
            get { return countryMobileCode; }
            set
            {
                countryMobileCode = value;
                RaisePropertyChanged("CountryMobileCode");
            }
        }

        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set
            {
                mobile = value;
                ListViewSecondName = mobile;
                RaisePropertyChanged("Mobile");
            }
        }

        public PhoneSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber;
        }

        public PhoneSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber;

            Mobile = item.Data.phoneNumber;
        }

        public PhoneSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber;


        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.country = SelectedCountry != null ? SelectedCountry.Code : null;
            secureItem.Data.phoneNumber = Mobile;

            return secureItem;
        }
    }
}
