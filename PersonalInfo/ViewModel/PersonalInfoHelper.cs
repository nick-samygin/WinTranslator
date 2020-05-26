using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using PasswordBoss.DTO;
using PasswordBoss.Model.PersonalInfo;
using PasswordBoss.Helpers;
using System.Diagnostics;

namespace PasswordBoss.ViewModel
{
    internal class PersonalInfoHelper
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PersonalInfoHelper));
        private const int CategoryfilterFlag = 0;
        private const int AZfilterFlag = 1;
        private const int ZAfilterFlag = 2;
        private const int LastUsedFlag = 3;
        private const int FavoritesFlag = 4;
        private IResolver resolver;
        Common _common = new Common();

        public PersonalInfoHelper(IResolver resolver)
        {
            this.resolver = resolver;
        }

        internal ImageSource ReturnImage(int viewType)
        {
            ImageSource returnIcon = null;
            switch (viewType)
            {
                case 1:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgViewGrid");
                    break;
                case 2:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgViewList");
                    break;
            }
            return returnIcon;
        }

        /// <summary>
        /// return selected Hovered icon as per view
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        internal ImageSource ReturnImageHover(int viewType)
        {
            ImageSource returnIcon = null;
            switch (viewType)
            {
                case 1:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgViewGridHover");
                    break;
                case 2:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgViewListHover");
                    break;
            }
            return returnIcon;
        }

        public List<DefaultView> GetSortedViewItems(int sortIndex, bool recommendedsiteFlag)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IPBData pbData = resolver.GetInstanceOf<IPBData>();
            IPBWebAPI pbWebApi = resolver.GetInstanceOf<IPBWebAPI>();
            List<SecureItem> secureItems;
            List<DefaultView> passVaultItems = new List<DefaultView>();
            try
            {

                if (recommendedsiteFlag)
                {
                    passVaultItems.Add(new DefaultView() { Id = "1", Image = System.Windows.Application.Current.FindResource("1").ToString(), Name = System.Windows.Application.Current.FindResource("Names").ToString(), Category = System.Windows.Application.Current.FindResource("Names").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddName").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "2", Image = System.Windows.Application.Current.FindResource("2").ToString(), Name = System.Windows.Application.Current.FindResource("Address").ToString(), Category = System.Windows.Application.Current.FindResource("Address").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddAddress").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "3", Image = System.Windows.Application.Current.FindResource("3").ToString(), Name = System.Windows.Application.Current.FindResource("PhoneNumbers").ToString(), Category = System.Windows.Application.Current.FindResource("PhoneNumbers").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddPhoneNumber").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "4", Image = System.Windows.Application.Current.FindResource("4").ToString(), Name = System.Windows.Application.Current.FindResource("Company").ToString(), Category = System.Windows.Application.Current.FindResource("Company").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddCompany").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "5", Image = System.Windows.Application.Current.FindResource("5").ToString(), Name = System.Windows.Application.Current.FindResource("Email").ToString(), Category = System.Windows.Application.Current.FindResource("Email").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddEmail").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "6", Image = System.Windows.Application.Current.FindResource("6").ToString(), Name = System.Windows.Application.Current.FindResource("DriverLicense").ToString(), Category = System.Windows.Application.Current.FindResource("DriverLicense").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddDriverLicense").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "7", Image = System.Windows.Application.Current.FindResource("7").ToString(), Name = System.Windows.Application.Current.FindResource("Passport").ToString(), Category = System.Windows.Application.Current.FindResource("Passport").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddPassport").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "8", Image = System.Windows.Application.Current.FindResource("8").ToString(), Name = System.Windows.Application.Current.FindResource("MemberIDs").ToString(), Category = System.Windows.Application.Current.FindResource("MemberIDs").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddMemberID").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "9", Image = System.Windows.Application.Current.FindResource("9").ToString(), Name = System.Windows.Application.Current.FindResource("SocialSecurity").ToString(), Category = System.Windows.Application.Current.FindResource("SocialSecurity").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddSocialSecurity").ToString() });
                    passVaultItems.Add(new DefaultView() { Id = "10", Image = System.Windows.Application.Current.FindResource("10").ToString(), Name = System.Windows.Application.Current.FindResource("SecureNotes").ToString(), Category = System.Windows.Application.Current.FindResource("SecureNotes").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddSecureNotes").ToString() });

                }
                else
                {
                    if ((secureItems = pbData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PersonalInfo)) != null)
                    {
                        string text2 = String.Empty;
                        string text1 = String.Empty;
                        foreach (var item in secureItems)
                        {
                            text2 = String.Empty;
                            text1 = item.Name;
                            string image = String.Empty;
                            text2 = String.Empty;
                            text1 = item.Name;
                            switch (item.Type)
                            {
                                case DefaultProperties.SecurityItemSubType_PI_Address:
                                    text2 = item.Data.address1;
                                    break;
                                case DefaultProperties.SecurityItemSubType_PI_Company:
                                    text2 = String.Empty;
                                    break;
                                case DefaultProperties.SecurityItemSubType_SN_DriverLicense:
                                    text2 = item.Data.driverLicenceNumber;
                                    break;
                                case DefaultProperties.SecurityItemSubType_PI_Email:
                                    text2 = item.Data.email;
                                    break;
                                case DefaultProperties.SecurityItemSubType_SN_MemberIDs:
                                    text2 = item.Data.memberID;
                                    break;
                                case DefaultProperties.SecurityItemSubType_PI_Names:
                                    text2 = String.Empty;
                                    break;
                                case DefaultProperties.SecurityItemSubType_SN_Passport:
                                    text2 = item.Data.passportNumber;
                                    break;
                                case DefaultProperties.SecurityItemSubType_PI_PhoneNumber:
                                    text2 = item.Data.phoneNumber;
                                    break;
                                case DefaultProperties.SecurityItemSubType_PI_SecureNotes:
                                    text2 = String.Empty;
                                    break;
                                case DefaultProperties.SecurityItemSubType_SN_SocialSecurity:
                                    text2 = String.Empty;
                                    break;
                                default:
                                    text2 = String.Empty;
                                    break;
                            }
                            passVaultItems.Add(new DefaultView() { Id = item.Id, Name = text1, Image = GetItemImage(item.Type), Category = item.Folder != null ? item.Folder.Name : "Other", Favorite = item.Favorite, Username = text2, LastAccess = item.LastAccess, shared = item.Share });
                        }
                    }
                }

                passVaultItems = SortViewItems(sortIndex, passVaultItems);

            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            watch.Stop();
            logger.Info("executed in: {0} ms", watch.ElapsedMilliseconds);
            return passVaultItems;
        }




        public List<DefaultView> SortViewItems(int sortType, List<DefaultView> itemsData)
        {
            List<DefaultView> returnItems = null;
            switch (sortType)
            {
                case CategoryfilterFlag:
                    returnItems = itemsData.OrderBy(x => x.Category).ToList();
                    break;
                case AZfilterFlag:
                    returnItems = itemsData.OrderBy(x => x.Name).ToList();
                    break;
                case ZAfilterFlag:
                    returnItems = itemsData.OrderByDescending(x => x.Name).ToList();
                    break;
                case LastUsedFlag:
                    returnItems = itemsData.OrderByDescending(x => x.LastAccess).ToList(); //TODO: change this
                    break;
                case FavoritesFlag:
                    returnItems = itemsData.OrderByDescending(x => x.Favorite).ToList();
                    break;
            }
            return returnItems;
        }

        private string GetItemImage(string itemType)
        {

            if (String.IsNullOrEmpty(itemType)) return "";
            switch (itemType)
            {
                case DefaultProperties.SecurityItemSubType_PI_Names:
                    return System.Windows.Application.Current.FindResource("1").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Address:
                    return System.Windows.Application.Current.FindResource("2").ToString();
                case DefaultProperties.SecurityItemSubType_PI_PhoneNumber:
                    return System.Windows.Application.Current.FindResource("3").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Company:
                    return System.Windows.Application.Current.FindResource("4").ToString();
                case DefaultProperties.SecurityItemSubType_PI_Email:
                    return System.Windows.Application.Current.FindResource("5").ToString();
                case DefaultProperties.SecurityItemSubType_SN_DriverLicense:
                    return System.Windows.Application.Current.FindResource("6").ToString();
                case DefaultProperties.SecurityItemSubType_SN_Passport:
                    return System.Windows.Application.Current.FindResource("7").ToString();
                case DefaultProperties.SecurityItemSubType_SN_MemberIDs:
                    return System.Windows.Application.Current.FindResource("8").ToString();
                case DefaultProperties.SecurityItemSubType_SN_SocialSecurity:
                    return System.Windows.Application.Current.FindResource("9").ToString();
                case DefaultProperties.SecurityItemSubType_PI_SecureNotes:
                    return System.Windows.Application.Current.FindResource("10").ToString();
                default:
                    break;
            }
            return "";
        }
    }
}
