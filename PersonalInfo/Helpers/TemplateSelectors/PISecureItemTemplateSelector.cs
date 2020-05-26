using PasswordBoss.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.Helpers.TemplateSelectors
{
    public class PISecureItemTemplateSelector: DataTemplateSelector
    {
        private DataTemplate _addressTemplate;

        public DataTemplate AddressTemplate
        {
            get
            {
                return _addressTemplate;
            }
            set
            {
                _addressTemplate = value;
            }
        }


        private DataTemplate _companyTemplate;

        public DataTemplate CompanyTemplate
        {
            get
            {
                return _companyTemplate;
            }
            set
            {
                _companyTemplate = value;
            }
        }


        private DataTemplate _emailTemplate;

        public DataTemplate EmailTemplate
        {
            get
            {
                return _emailTemplate;
            }
            set
            {
                _emailTemplate = value;
            }
        }

        private DataTemplate _nameTemplate;

        public DataTemplate NameTemplate
        {
            get
            {
                return _nameTemplate;
            }
            set
            {
                _nameTemplate = value;
            }
        }

        private DataTemplate _phoneTemplate;

        public DataTemplate PhoneTemplate
        {
            get
            {
                return _phoneTemplate;
            }
            set
            {
                _phoneTemplate = value;
            }
        }

      

        public PISecureItemTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is AddressSecureItemViewModel)
                return AddressTemplate;

            if (item is CompanySecureItemViewModel)
                return CompanyTemplate;

            if (item is EmailSecureItemViewModel)
                return EmailTemplate;

            if (item is NameSecureItemViewModel)
                return NameTemplate;

            if (item is PhoneSecureItemViewModel)
                return PhoneTemplate;            

            return AddressTemplate;

        }
    }
}
