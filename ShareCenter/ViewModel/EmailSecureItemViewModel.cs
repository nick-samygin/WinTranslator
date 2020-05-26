using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
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
}
