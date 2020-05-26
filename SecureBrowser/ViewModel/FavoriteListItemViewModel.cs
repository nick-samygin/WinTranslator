using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PasswordBoss.ViewModel
{
    public class FavoriteListItemViewModel : ViewModelBase
    {
        public SecureBrowserViewModel Model { get; set; }
        public Favorite Favorite { get; set; }
        public FavoriteListItemViewModel(SecureBrowserViewModel model)
        {
            Model = model;
        }
        private ImageSource _siteImage;
        public ImageSource SiteImage
        {
            get
            {
                return _siteImage;
            }
            set
            {
                _siteImage = value;
                if (SiteImage == null)
                {
                    
                        //do get icon
                        if (!String.IsNullOrEmpty(Favorite.Url))
                        {
                            Model.PBWebAPI.GetFaviconAsync(Favorite.Url).DownloadDataCompleted += FavoriteListItemViewModel_DownloadDataCompleted;
                        }
                   
                }
                RaisePropertyChanged("SiteImage");
            }
        }

        void FavoriteListItemViewModel_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(e.Result);
                image.EndInit();
                SiteImage = image as ImageSource;
            }
            catch
            {
                _siteImage = null;
            }
        }
    }
}
