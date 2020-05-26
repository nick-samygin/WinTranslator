using PasswordBoss.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace PasswordBoss.Model.SecurityScore
{
    public class SecurityScoreItemType
    {
        public const string old = "old";
        public const string duplicate = "duplicate";
        public const string week = "week";
        public const string all = "all";
    }
    public class SecurityScoreModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string securityScore { get; set; }
        public Type[] types { get; set; }
    }

    public class Type
    {
        public string type { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string uuid { get; set; }
        public string siteName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }


    /// <summary>
    /// Security score List Table
    /// </summary>
    public class SecurityScoreData : ViewModelBase
    {
        //public string site { get; set; }
        //public string username { get; set; }
        //public string password { get; set; }
        //public string uuid { get; set; }

        private bool _passwordVisibility;
        private bool _negatePasswordVisibility = true;

        public string id { get; set; }
        public string siteName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string passwordAge { get; set; }
        public string siteUri { get; set; }
        public bool PasswordVisibility { get { return _passwordVisibility; } set { _passwordVisibility = value; NegatePasswordVisibility = !value; RaisePropertyChanged("PasswordVisibility"); } }

        [DependsOn("PasswordVisibility")]
        public bool NegatePasswordVisibility { get { return _negatePasswordVisibility; } private set { _negatePasswordVisibility = value; RaisePropertyChanged("NegatePasswordVisibility"); } }

        public DateTime LastModifiedDate { get; set; }
        public bool ReEnterPassword { get; set; }
    }
}
