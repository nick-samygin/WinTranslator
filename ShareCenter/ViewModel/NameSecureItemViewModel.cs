using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
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
}
