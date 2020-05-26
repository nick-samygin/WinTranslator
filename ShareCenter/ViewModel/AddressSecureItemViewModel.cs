using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
    public class AddressSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemWithCountryViewModel
    {
        private string address1;
        public string Address1
        {
            get { return address1; }
            set
            {
                address1 = value;
                ListViewSecondName = address1;
                RaisePropertyChanged("Address1");
            }
        }

        private string address2;
        public string Address2
        {
            get { return address2; }
            set
            {
                address2 = value;
                RaisePropertyChanged("Address2");
            }
        }

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }

        private string state;
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }

        private string zipCode;
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                zipCode = value;
                RaisePropertyChanged("ZipCode");
            }
        }




        public AddressSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Address;
        }

        public AddressSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Address;

            Address1 = item.Data.address1;
            Address2 = item.Data.address2;
            City = item.Data.city;
            State = item.Data.state;
            ZipCode = item.Data.zipCode;

        }


        public AddressSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Address;
        }


        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.address1 = Address1;
            secureItem.Data.address2 = Address2;
            secureItem.Data.city = City;
            secureItem.Data.state = State;
            secureItem.Data.zipCode = ZipCode;
            secureItem.Data.country = SelectedCountry != null ? SelectedCountry.Code : null;

            return secureItem;
        }
    }
}
