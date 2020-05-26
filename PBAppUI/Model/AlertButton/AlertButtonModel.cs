using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;

namespace PasswordBoss.Model.AlertButton
{
    public class Alert
    {
        Common c = new Common();
        IResolver resolver = null;
        IPBData pbData = null;
        public Alert() {
            resolver = ((PBApp)Application.Current).GetResolver();
            if (resolver != null) pbData = resolver.GetInstanceOf<IPBData>();
        }

        public Alert(AlertNotification an)
        {
            resolver = ((PBApp)Application.Current).GetResolver();
            if (resolver != null) pbData = resolver.GetInstanceOf<IPBData>();

            string path =AppHelper.ImageFolderLocation + an.image_id + ".png";
            if(File.Exists(path))
            {
                alertImage = c.GetImageForPath(path, loadDefaultIfNeeded: false);
                icon_width = 60;
                icon_height = 40;
            }
            else
            {
                alertImage = DefaultProperties.ReturnImage(DefaultProperties.SecurityAlertIcon);
                icon_width = 21;
                icon_height = 18;
            }
            if(an.AlertType == AlertType.SecurityAlert)
            {
                siteName = an.site_name;
                if(an.has_duplicate)
                {
                    status = (string)Application.Current.Resources["DuplicatePassword"];
                }
                else if(an.is_weak)
                {
                    status = (string)Application.Current.Resources["WeakPassword"];
                }
                else if(an.last_password_change.HasValue)
                {
                    int days = (int)DateTime.Now.Subtract(an.last_password_change.Value).TotalDays;
                    if (days >= 365) status = string.Format((string)Application.Current.Resources["OldPasswordDesktop"], days);
                }
            }
            else if(an.AlertType == AlertType.NewShare)
            {
                siteName = an.CustomMessage;
                if (pbData != null)
                {
                    email = an.sender == pbData.ActiveUser ? an.receiver : an.sender;
                    visible = true;
                }
                status =  an.nickname;// an.message;
                shareStatus = an.status;
                uuid = an.id;
                alertImage = (ImageSource)Application.Current.FindResource("imgShare2Hover");
                icon_width = 21;
                icon_height = 18;
            }
            if(uuid == null) uuid = an.secure_item_id;
            AlertNotification = an;
        }

        public string uuid { get; set; }
        public string siteName { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string sent { get; set; }
        public ImageSource alertImage { get; set; }
        public bool visible { get; set; }
        public AlertNotification AlertNotification { get; set; }
        public int icon_width { get; set; }
        public int icon_height { get; set; }
        public string shareStatus { get; set; }

        public bool IsHistory { get; set; }
    }
}
