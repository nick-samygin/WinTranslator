using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using PasswordBoss.DTO;

namespace PasswordBoss.Helpers
{

    public enum SecureItemSubType
    {
        Empty,

        Password,
        DigitalWallet,
        PersonalInfo,
        SecureNote,
        Indentity,
        SharedItem,
        EmergencyContact,
        Folder,

        Website,
        App,
        Database,
        EmailAccount,
        InstantMessenger,
        Server,
        SSHKey,
        Wifi,

        BankAccount,
        CreditCard,

        Address,
        Company,
        Email,
        Name,
        Phone,

        AlarmCode,
        DriversLicense,
        EstatePlan,
        FrequentFlyer,
        Note,
        HealthInsurance,
        HotelRewards,
        Insurance,
        MemberID,
        Passport,
        Prescription,
        SocialSecurity,
        SoftwareLicense
    }

    public class AddItem
    {
        public string ItemTitel { get; set; }

        public string ComponentId { get; set; }

        public ImageSource Icon { get; set; }
    }

    public class AddSecureItem : AddItem
    {
        public AddSecureItem() { SubTypesList = new List<AddSecureSubItem>(); }

        public string AddTitel { get; set; }

        public string AddSubTitel { get; set; }

        public List<AddSecureSubItem> SubTypesList { get; set; }
    }

    public class AddSecureSubItem : AddItem
    {
        public AddSecureSubItem()
        {
            BackgoundColor = new SolidColorBrush(Colors.Transparent);
        }

        public Brush BackgoundColor { get; set; }

        public SecureItemSubType ItemType { get; set; }

        public Type CreateItemType { get; set; }

        public bool IsEmpty
        {
            get
            {
                if (ItemType == SecureItemSubType.Empty)
                    return true;
                return false;
            }
        }
    }

}
