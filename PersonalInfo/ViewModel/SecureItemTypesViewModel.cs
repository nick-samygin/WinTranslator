using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SecureItemsCommon.ViewModels;
using PasswordBoss.DTO;
using System.Windows.Media;

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
            secureItem.Data.country = SelectedCountry!=null ? SelectedCountry.Code : null;
                       
            return secureItem;
        }
    }

    public class CompanySecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        public CompanySecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Company;
        }

        public CompanySecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Company;

        }
        public CompanySecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Company;
        }

    }

    public class EmailSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                ListViewSecondName = emailAddress;
                RaisePropertyChanged("EmailAddress");
            }
        }


        public EmailSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email;
        }

        public EmailSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email;


            EmailAddress = item.Data.email;
         }

        public EmailSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email;
        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.email = EmailAddress;
            return secureItem;
        }
    }

    public class NameSecureItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        //private string firstName;
        //public string FirstName
        //{
        //    get { return firstName; }
        //    set
        //    {
        //        firstName = value;
        //        RaisePropertyChanged("FirstName");
        //    }
        //}

        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                RaisePropertyChanged("MiddleName");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                ListViewSecondName = lastName;
                RaisePropertyChanged("LastName");
            }
        }
        public NameSecureItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names;
        }

        public NameSecureItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names;

           
            MiddleName = item.Data.middleName;
            LastName = item.Data.lastName;
        }

        public NameSecureItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names;


        }

        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.firstName = Name;
            secureItem.Data.middleName = MiddleName;
            secureItem.Data.lastName = LastName;

            return secureItem;
        }
    }

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

            secureItem.Data.country = SelectedCountry!=null ? SelectedCountry.Code : null;
            secureItem.Data.phoneNumber = Mobile;

            return secureItem;
        }
    }

}