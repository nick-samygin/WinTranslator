using System.Windows.Media;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
    public class BankAccountItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        private string bankName;
        public string BankName
        {
            get { return bankName; }
            set
            {
                bankName = value;
                ListViewSecondName = bankName;
                RaisePropertyChanged("BankName");
            }
        }

        private string accountName;
        public string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                RaisePropertyChanged("AccountName");
            }
        }

        private string accountNumber;
        public string AccountNumber
        {
            get { return accountNumber; }
            set
            {
                accountNumber = value;
                RaisePropertyChanged("AccountNumber");
            }
        }

        private string routingNumber;
        public string RoutingNumber
        {
            get { return routingNumber; }
            set
            {
                routingNumber = value;
                RaisePropertyChanged("RoutingNumber");
            }
        }

        private string bic;
        public string BIC
        {
            get { return bic; }
            set
            {
                bic = value;
                RaisePropertyChanged("BIC");
            }
        }

        private string iban;
        public string IBAN
        {
            get { return iban; }
            set
            {
                iban = value;
                RaisePropertyChanged("IBAN");
            }
        }

        private string pin;
        public string Pin
        {
            get { return pin; }
            set
            {
                pin = value;
                RaisePropertyChanged("Pin");
            }
        }

        private string bankPhone;
        public string BankPhone
        {
            get { return bankPhone; }
            set
            {
                bankPhone = value;
                RaisePropertyChanged("BankPhone");
            }
        }

        public BankAccountItemViewModel()
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_Bank;
        }


        public BankAccountItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_Bank;

            BankName = item.Data.bank_name;
            AccountName = item.Data.nameOnAccount;
            AccountNumber = item.Data.accountNumber;
            RoutingNumber = item.Data.iban;
            Pin = item.Data.pin;
            BankPhone = item.Data.bank_phone;
            BIC = item.Data.swift;


        }

        public BankAccountItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_Bank;

        }


        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.bank_name = BankName;
            secureItem.Data.accountNumber = AccountNumber;
            secureItem.Data.routingNumber = RoutingNumber;
            secureItem.Data.iban = IBAN;
            secureItem.Data.swift = BIC;
            secureItem.Data.pin = Pin;
            secureItem.Data.bank_phone = BankPhone;
            secureItem.Data.nameOnAccount = AccountName;
            return secureItem;
        }

    }
}
