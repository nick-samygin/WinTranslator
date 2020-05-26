using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PasswordBoss.Model.PersonalInfo
{

    public class PersonalInfoItemClass
    {
        public string status { get; set; }
        public string message { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string parentName { get; set; }
        public string parentId { get; set; }
        public Category[] categories { get; set; }
    }

    public class Category
    {
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string secureBrowser { get; set; }
        public string activate { get; set; }
    }

    //public class SecuerShareData
    //{
    //    public string recipient { get; set; }
    //    public string status { get; set; }
    //    public bool visibleAction { get; set; }
    //    public string sent { get; set; }
    //    public string uuid { get; set; }
    //    public ImageSource visible { get; set; }
    //}

    public class ComboboxItem
    {
        public string categoryName { get; set; }
    }

}
