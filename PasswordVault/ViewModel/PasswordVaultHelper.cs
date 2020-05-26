using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Model.PasswordVault;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PasswordBoss.ViewModel
{
    internal class PasswordVaultHelper
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PasswordVaultHelper));
        private const int CategoryfilterFlag = 0;
        private const int AZfilterFlag = 1;
        private const int ZAfilterFlag = 2;
        private const int LastUsedFlag = 3;
        private const int FavoritesFlag = 4;
        private IResolver resolver;
        Common _common = new Common();

        public PasswordVaultHelper(IResolver resolver)
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

        public List<SecureItemViewModel> GetViewItems( bool recommendedsiteFlag)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IPBData pbData = resolver.GetInstanceOf<IPBData>();
            IPBWebAPI pbWebApi = resolver.GetInstanceOf<IPBWebAPI>();
            List<SecureItem> sites;
            List<SecureItemViewModel> passVaultItems = new List<SecureItemViewModel>();
            try
            {
               
<<<<<<< HEAD
                //if (recommendedsiteFlag)
                //{
                //    var recommendedSites = pbData.GetRecommendedSites();
                //    if(recommendedSites != null)
                //    {
                //        foreach (var site in recommendedSites)
                //        {
                //            passVaultItems.Add(new DefaultView() { Id = site.Id, Image = _common.GetImagePathForSite(site.UUID), Name = site.FriendlyName, Category = site.Folder != null ? site.Folder : "Other", Favorite = false, Username = null, LastAccess = null });
=======
                if (recommendedsiteFlag)
                {
                    var recommendedSites = pbData.GetRecommendedSites();
                    if(recommendedSites != null)
                    {
                        foreach (var site in recommendedSites)
                        {

                            passVaultItems.Add(new DefaultView() { Id = site.Id, Image = Common.GetImagePathForSite(site.UUID), Name = site.FriendlyName, Category = site.Folder != null ? site.Folder.Name : "Other", Favorite = false, Username = null, LastAccess = null });
>>>>>>> PBD-1604_2_0

                //        }
                //    }
                //}
                //else
                //{
                    if((sites = pbData.GetSecureItemsByItemType(DefaultProperties.SecurityItemType_PasswordVault)) != null)
                    {
                        foreach(var site in sites)
                        {
                            if(site.Data==null)
                            {
                                logger.Error("GetSortedViewItems: site data is null");
                                continue;
                            }
<<<<<<< HEAD
                            var item = SubItemsComponentTree.FirstOrDefault(x => x.ItemType == site.Type);
                            
                            var secureItemVM=Activator.CreateInstance(item.CreateItemType, site,item.BackgoundColor,item.Icon) as SecureItemViewModel;
                               
                            passVaultItems.Add(secureItemVM);
=======

                            passVaultItems.Add(new DefaultView() { Id = site.Id, Image = Common.GetImagePathForSite(site.Site.UUID), Name = site.Name, Category = site.Folder != null ? site.Folder.Name : "Other", Favorite = site.Favorite, Username = site.Data.username != null ? site.Data.username : site.Data.email, LastAccess = site.LastAccess, shared = site.Share });

>>>>>>> PBD-1604_2_0
                        }
                    }
               // }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            watch.Stop();
            logger.Info("items got: {0}, executed in: {1} ms", passVaultItems.Count, watch.ElapsedMilliseconds);
            return passVaultItems;
        }

        private void LogoDownloadCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            
        }


       
        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {
                return new List<AddSecureSubItem>()
                    {
                            new AddSecureSubItem(){
                            ItemType = DefaultProperties.SecurityItemSubType_PV_Login,
                            ItemTitel = Application.Current.Resources["ItemWebsite"] as string,
                            Icon = Application.Current.Resources["addWebsite"] as ImageSource,
                            CreateItemType=typeof(WebsiteSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(0,120,215))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_Application,
                            ItemTitel = Application.Current.Resources["ItemApp"] as string,
                            Icon = Application.Current.Resources["addApp"] as ImageSource,
                            CreateItemType=typeof(AppSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(79,77,160))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_Database,
                            ItemTitel = Application.Current.Resources["ItemDatabase"] as string,
                            Icon = Application.Current.Resources["addDatabase"] as ImageSource,
                            CreateItemType=typeof(DatabaseSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(191,0,119))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType =  DefaultProperties.SecurityItemSubType_PV_EmailAccount,
                            ItemTitel = Application.Current.Resources["ItemEmailAccount"] as string,
                            Icon = Application.Current.Resources["addEmailAccount"] as ImageSource,
                            CreateItemType=typeof(EmailAccountSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(46,204,113))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_InstantMessenger,
                            ItemTitel = Application.Current.Resources["ItemInstantMessenger"] as string,
                            Icon = Application.Current.Resources["addInstantMessenger"] as ImageSource,
                            CreateItemType=typeof(InstantMessengerSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(218,59,1))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_Server,
                            ItemTitel = Application.Current.Resources["ItemServer"] as string,
                            Icon = Application.Current.Resources["addServer"] as ImageSource,
                            CreateItemType=typeof(ServerSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(72,104,96))
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_SSHKey,
                            ItemTitel = Application.Current.Resources["ItemSSHKeys"] as string,
                            Icon = Application.Current.Resources["addSSHKey"] as ImageSource,
                             CreateItemType=typeof(SSHKeySecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(33,33,33))
                        },
                        new AddSecureSubItem()
        {
                            ItemType = DefaultProperties.SecurityItemSubType_PV_WiFi,
                            ItemTitel = Application.Current.Resources["ItemWiFi"] as string,
                            Icon = Application.Current.Resources["addWifi"] as ImageSource,
                            CreateItemType=typeof(WifiSecureItemViewModel),
                            BackgoundColor=new SolidColorBrush(Color.FromRgb(136,23,152))
                        },
                        new AddSecureSubItem()
            {
                            ItemType = string.Empty,
                            ItemTitel = Application.Current.Resources["ItemAddDifferent"] as string
                        }

                };
            }
        }

        
    }
}
