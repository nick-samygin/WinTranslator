using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
    public class Card
    {
        public string Type { get; set; }
        public string Image { get; set; }
        public string RegularExpression { get; set; }
    }

    public class CreditCardItemViewModel : SecureItemsCommon.ViewModels.SecureItemViewModel
    {
        private string nameOnCard;
        public string NameOnCard
        {
            get { return nameOnCard; }
            set
            {
                nameOnCard = value;
                RaisePropertyChanged("NameOnCard");
            }
        }

        private string cardNumber;
        public string CardNumber
        {
            get { return cardNumber; }
            set
            {
                if (cardNumber != value)
                {
                    cardNumber = value;
                    if (cardNumber != null && cardNumber.Length > 4)
                    {
                        ListViewSecondName = cardNumber.Substring(cardNumber.Length - 4);
                    }

                    RaisePropertyChanged("CardNumber");
                    foreach (var card in CreditCards)
                    {
                        if (card.RegularExpression != null && Regex.IsMatch(cardNumber, card.RegularExpression))
                        {
                            SelectedCreditCard = card;
                            return;
                        }
                    }


                }
            }
        }

        public ObservableCollection<Card> CreditCards { get; private set; }

        public ObservableCollection<string> ExpiresMonths { get; private set; }
        public ObservableCollection<string> ExpiresYears { get; private set; }

        private Card selectedCreditCard;
        public Card SelectedCreditCard
        {
            get
            {
                return selectedCreditCard;
            }
            set
            {
                if (value != null && selectedCreditCard != null)
                {
                    if (selectedCreditCard.Type != value.Type)
                    {
                        selectedCreditCard = value;
                        RaisePropertyChanged("SelectedCreditCard");
                        Image = selectedCreditCard.Image;
                    }
                }
                else
                {
                    selectedCreditCard = value;
                    RaisePropertyChanged("SelectedCreditCard");
                    if (value != null)
                        Image = value.Image;
                }
            }
        }

        private string issuingBank;
        public string IssuingBank
        {
            get { return issuingBank; }
            set
            {
                issuingBank = value;
                RaisePropertyChanged("IssuingBank");
            }
        }

        private string securityCode;
        public string SecurityCode
        {
            get { return securityCode; }
            set
            {
                securityCode = value;
                RaisePropertyChanged("SecurityCode");
            }
        }

        private string pin;
        public string PIN
        {
            get { return pin; }
            set
            {
                pin = value;
                RaisePropertyChanged("PIN");
            }
        }

        private string issueDate;
        public string IssueDate
        {
            get { return issueDate; }
            set
            {
                issueDate = value;
                RaisePropertyChanged("IssueDate");
            }
        }

        private string expiresMonth;
        public string ExpiresMonth
        {
            get
            {
                return expiresMonth;
            }
            set
            {
                expiresMonth = value;
                RaisePropertyChanged("ExpiresMonth");
            }
        }
        private string expiresYear;
        public string ExpiresYear
        {
            get
            {
                return expiresYear;
            }
            set
            {
                expiresYear = value;
                RaisePropertyChanged("ExpiresYear");
            }
        }

        public CreditCardItemViewModel()
        {

            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_CreditCard;
            InitializeCollections();
        }

        public CreditCardItemViewModel(SecureItem item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            IsDefaultImage = false;
            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_CreditCard;
            InitializeCollections();



            NameOnCard = item.Data.nameOnCard;
            CardNumber = item.Data.cardNumber;

            if (item.Data.expires != null)
            {

                DateTime expires = new DateTime();
                if (DateTime.TryParse(item.Data.expires, out expires))
                {
                    ExpiresMonth = expires.ToUniversalTime().Month.ToString();
                    ExpiresYear = expires.ToUniversalTime().Year.ToString();
                }
            }
            if (item.Data.cardType != null) SelectedCreditCard = CreditCards.FirstOrDefault(x => x.Type == item.Data.cardType);


            IssuingBank = item.Data.issuingBank;
            SecurityCode = item.Data.security_code;
            PIN = item.Data.pin;
            IssueDate = item.Data.issueDate;

        }

        public CreditCardItemViewModel(SecureItemSearchResult item, System.Windows.Media.Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {
            IsDefaultImage = false;
            type = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
            subType = SecurityItemsDefaultProperties.SecurityItemSubType_DW_CreditCard;
            InitializeCollections();

            SelectedCreditCard = CreditCards.FirstOrDefault(x => x.Type == item.CreditCardType);


        }

        private void InitializeCollections()
        {


            List<Card> creditCardList = new List<Card>();
            creditCardList.Add(new Card()
            {
                Type = "American Express",
                Image = ((BitmapImage)Application.Current.FindResource("creditCardAmex")).ToString(),
                RegularExpression = "^3[47][0-9]{13}$"
            });
            creditCardList.Add(new Card()
            {
                Type = "Discover",
                Image = ((BitmapImage)Application.Current.FindResource("creditCarddDiscover")).ToString(),
                RegularExpression = "^6(?:011|5[0-9]{2})[0-9]{12}$"
            });
            creditCardList.Add(new Card()
            {
                Type = "JCB",
                Image = ((BitmapImage)Application.Current.FindResource("creditCardJcb")).ToString(),
                RegularExpression = @"^(?:2131|1800|35\d{3})\d{11}$"
            });
            creditCardList.Add(new Card()
            {
                Type = "Master",
                Image = ((BitmapImage)Application.Current.FindResource("creditCardMaster")).ToString(),
                RegularExpression = "^5[1-5][0-9]{14}$"
            });
            creditCardList.Add(new Card()
            {
                Type = "Visa",
                Image = ((BitmapImage)Application.Current.FindResource("creditCardVisa")).ToString(),
                RegularExpression = "^4[0-9]{12}(?:[0-9]{3})?$"
            });
            creditCardList.Add(new Card()
            {
                Type = "Other",
                Image = ((BitmapImage)Application.Current.FindResource("creditCardOther")).ToString(),
                RegularExpression = "."
            });




            CreditCards = new ObservableCollection<Card>(creditCardList);

            List<string> monthList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            List<string> yearList = new List<string>();
            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 11; i++)
                yearList.Add(i.ToString());

            ExpiresMonths = new ObservableCollection<string>(monthList);
            ExpiresYears = new ObservableCollection<string>(yearList);

        }



        public override SecureItem CreateSecureItem()
        {
            var secureItem = base.CreateSecureItem();

            secureItem.Data.nameOnCard = NameOnCard;
            secureItem.Data.cardNumber = CardNumber;
            secureItem.Data.expires = ExpiresMonth != null && ExpiresYear != null ? new DateTime(int.Parse(ExpiresYear), int.Parse(ExpiresMonth), DateTime.DaysInMonth(int.Parse(ExpiresYear), int.Parse(ExpiresMonth))).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
            secureItem.Data.cardType = SelectedCreditCard != null ? SelectedCreditCard.Type : "Other";

            secureItem.Data.issuingBank = IssuingBank;
            secureItem.Data.security_code = SecurityCode;
            secureItem.Data.pin = PIN;
            secureItem.Data.issueDate = IssueDate;

            return secureItem;
        }
    }
}
