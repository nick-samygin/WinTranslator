using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;

namespace PasswordBoss.Model.SecurityNotification
{
    public class Notification
    {
        public Notification(AlertMessage am)
        {
            switch(am.IconType)
            {
                default:
                    alertImage = DefaultProperties.ReturnImage(DefaultProperties.SecurityAlertIcon);
                    break;
            }
            siteName = am.Headline;
            status = am.Message;
            if(am.PublishedDate.HasValue)
            {
                int days = (int)DateTime.Now.Subtract(am.PublishedDate.Value).TotalDays;
                if (days >= 365) status = string.Format((string)Application.Current.Resources["OldPasswordDesktop"], days);
            }
            uuid = am.UUID;
            AlertMessage = am;
        }

        public string uuid { get; set; }
        public string siteName { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string sent { get; set; }
        public ImageSource alertImage { get; set; }
        public bool visible { get; set; }
        public AlertMessage AlertMessage { get; set; }

        public bool IsHistory { get; set; }
    }
}
