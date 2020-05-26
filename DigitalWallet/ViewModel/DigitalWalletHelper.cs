using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using PasswordBoss.Model.DigitalWallet;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Helpers;
using System.Diagnostics;

namespace PasswordBoss.ViewModel
{
    internal class DigitalWalletHelper
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(DigitalWalletHelper));
        private const int CategoryfilterFlag = 0;
        private const int AZfilterFlag = 1;
        private const int ZAfilterFlag = 2;
        private const int LastUsedFlag = 3;
        private const int FavoritesFlag = 4;
        private IResolver resolver;
        Common _common = new Common();

        public DigitalWalletHelper(IResolver resolver)
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

        public List<SecureItemViewModel> GetViewItems(bool recommendedsiteFlag)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IPBData pbData = resolver.GetInstanceOf<IPBData>();
            IPBWebAPI pbWebApi = resolver.GetInstanceOf<IPBWebAPI>();
            List<SecureItem> secureItems;
            List<SecureItemViewModel> passVaultItems = new List<SecureItemViewModel>();
            try
            {

                if (recommendedsiteFlag)
                {
                    //passVaultItems.Add(new DefaultView() { Id = "2", Image = System.Windows.Application.Current.FindResource("22").ToString(), Name = System.Windows.Application.Current.FindResource("BankAccount").ToString(), Category = System.Windows.Application.Current.FindResource("BankAccount").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddBank").ToString() });
                    //passVaultItems.Add(new DefaultView() { Id = "1", Image = System.Windows.Application.Current.FindResource("21").ToString(), Name = System.Windows.Application.Current.FindResource("CreditCard").ToString(), Category = System.Windows.Application.Current.FindResource("CreditCard").ToString(), Favorite = false, Username = null, LastAccess = null, RecommendedHoverText = System.Windows.Application.Current.FindResource("AddCreditCard").ToString() });
                }
                else
                {
                    if ((secureItems = pbData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_DigitalWallet)) != null)
                    {
                        string text2 = String.Empty;
                        string text1 = String.Empty;
                        foreach (var item in secureItems)
                        {
                            var sItem = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == item.Type);

                            var secureItemVM = Activator.CreateInstance(sItem.CreateItemType, item, sItem.BackgoundColor, sItem.Icon) as SecureItemViewModel;

                            passVaultItems.Add(secureItemVM);

                            //text2 = String.Empty;
                            //text1 = item.Name;
                            //string image = String.Empty;
                            //switch (item.Type)
                            //{
                            //    case DefaultProperties.SecurityItemSubType_DW_Bank:
                            //        text2 = String.Empty;// item.Data.bank;
                            //        image = System.Windows.Application.Current.FindResource("22").ToString();
                            //        break;
                            //    case DefaultProperties.SecurityItemSubType_DW_CreditCard:
                            //        if (item.Data.cardNumber != null)
                            //        {
                            //            string num = string.Join(String.Empty, item.Data.cardNumber.Where(Char.IsLetterOrDigit).ToArray());

                            //            if (num.Length >= 4)
                            //                text2 += String.Format("**** - {0}", num.Substring(num.Length - 4, 4));
                            //            else
                            //                text2 += String.Format("**** - {0}", num);
                            //        }


                            //        if (item.Color == null) item.Color = "0";
                            //        if (String.IsNullOrEmpty(item.Color)) item.Color = "0";
                            //                                           break;
                            //    case DefaultProperties.SecurityItemSubType_DW_Paypal:
                            //        text2 = item.Data.username;
                            //        text1 = item.Name;
                            //        image = System.Windows.Application.Current.FindResource("23").ToString();
                            //        break;
                            //    default:
                            //        text2 = String.Empty;
                            //        text1 = String.Empty;
                            //        break;
                            //}
                            //passVaultItems.Add(new DefaultView() { Id = item.Id, Name = text1, Image = image, Category = item.Folder != null ? item.Folder.Name : "Other", Favorite = item.Favorite, Username = text2, LastAccess = item.LastAccess, shared = item.Share });
                            ////_items.Add(new DefaultView() { Id = item.Id, Name = item.Name, Image = image, Category = item.Category != null ? item.Category.Name : "Other", Favorite = item.Favorite, Username = text2, LastAccess = item.LastAccess, shared = item.Share });
                        }
                    }
                }

              //  passVaultItems = SortViewItems(sortIndex, passVaultItems);

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


        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {
                return new List<AddSecureSubItem>()
                    {
                            new AddSecureSubItem(){
                            ItemType = DefaultProperties.SecurityItemSubType_DW_Bank,
                            ItemTitel = Application.Current.Resources["ItemBankAccount"] as string,
                            Icon = Application.Current.Resources["addBankAccount"] as ImageSource,
                            CreateItemType=typeof(BankAccountItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(16,124,16))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_DW_CreditCard,
                            ItemTitel = Application.Current.Resources["ItemCreditCard"] as string,
                            Icon = Application.Current.Resources["addCreditCard"] as ImageSource,
                             CreateItemType=typeof(CreditCardItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(185,152,52))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType =string.Empty,
                            ItemTitel = Application.Current.Resources["ItemAddDifferent"] as string
                        }
                    };
               
            }
        }



    }
}
