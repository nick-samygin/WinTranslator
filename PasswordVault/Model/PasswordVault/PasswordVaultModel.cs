using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PasswordBoss.Model.PasswordVault
{
   
   

    /// <summary>
    /// It hold database fields of category table  
    /// </summary>
    public class category
    {
        /// <summary>
        /// it hold the category id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// it hold the category name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// it hold the category parent id
        /// </summary>
        public string parent { get; set; }
    }

    /// <summary>
    /// item table model 
    /// </summary>
    public class item
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool share_flag { get; set; }
        public bool favorite_flag { get; set; }
        public string category_id { get; set; }

        public string shareflag { get; set; }
        public string favoriteflag { get; set; }
        //public ImageSource ShareImageSource { get; set; }
        //public ImageSource favoriteImageSource { get; set; }
    }

}
