using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
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
}
