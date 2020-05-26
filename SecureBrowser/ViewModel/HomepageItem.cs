using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PasswordBoss.ViewModel
{
    public class HomepageItem
    {
        public IPBData PBData { get; set; }
        private static readonly ILogger logger = Logger.GetLogger(typeof(HomepageItem));
        public static string FAVORITE_ITEM = "FAV";
        public static string MOST_USED_ITEM = "MOST_USED";
        public static string RECENTLY_USED_ITEM = "REC_USED";
        public static string RECOMMENDED_SITES = "RECOMMENDED_SITES";
        public static string SB_LAST_VIEW_KEY = "SB_LAST_VIEW_KEY";
        public string Key { get; set; } //we will determine visibility of grid by this key
        public string Name { get; set; }
        public RelayCommand OpenCommand { get; set; }

        public SecureBrowserViewModel SecureBrowserViewModel { get; set; }

        public HomepageItem(SecureBrowserViewModel model)
        {
            //PBData = resolver.GetInstanceOf<IPBData>();
            SecureBrowserViewModel = model;
            OpenCommand = new RelayCommand(OpenClick);
        }

        public void OpenClick(object obj)
        {
            try
            {
                if(SecureBrowserViewModel.SelectedTabItem != null)
                {
                    //SecureBrowserViewModel.SelectedTabItem.HideAllHompageContainerItems();
                    if (Key == FAVORITE_ITEM)
                    {
                        SecureBrowserViewModel.LoadFavoriteItems();
                        //SecureBrowserViewModel.SelectedTabItem.IsHomepageContainerFavoriteListVisible = Visibility.Visible;
                    }
                    else if (Key == MOST_USED_ITEM)
                    {
                        SecureBrowserViewModel.LoadMostUsedItems();
                        //SecureBrowserViewModel.SelectedTabItem.IsHomepageContainerMostVisitedListVisible = Visibility.Visible;
                    }
                    else if (Key == RECENTLY_USED_ITEM)
                    {
                        SecureBrowserViewModel.LoadRecentlyUsedItems();
                        //SecureBrowserViewModel.SelectedTabItem.IsHomepageContainerRecentlyUsedListVisible = Visibility.Visible;
                    }
                    else if(Key == RECOMMENDED_SITES)
                    {
                        SecureBrowserViewModel.LoadRecommendedSites();
                    }
                    SecureBrowserViewModel.PBData.ChangePrivateSetting(SB_LAST_VIEW_KEY, Key);
                    
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                // MessageBox.Show(ex.ToString());
            }
        }
    }
}
