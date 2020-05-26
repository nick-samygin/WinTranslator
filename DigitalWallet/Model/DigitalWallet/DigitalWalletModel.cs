using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PasswordBoss.Model.DigitalWallet
{
    //public class DefaultView
    //{
    //    private int id;
    //    private int category;
    //    private string image;
    //    private string name;

    //    public int Id
    //    {
    //        get { return id; }
    //        set { id = value; }
    //    }

    //    public int Category
    //    {
    //        get { return category; }
    //        set { category = value; }
    //    }


    //    public string Image
    //    {
    //        get { return image; }
    //        set { image = value; }
    //    }



    //    public string Name
    //    {
    //        get { return name; }
    //        set { name = value; }
    //    }

    //    public DefaultView(int id, int cat, string im, string n)
    //    {
    //        id = id;
    //        category = cat;
    //        image = im;
    //        name = n;
    //    }

    //    public DefaultView() { }
    //}

    /// <summary>
    /// It hold database fields of category table  
    /// </summary>
    public class category
    {
        /// <summary>
        /// it hold the category id
        /// </summary>
        public int id { get; set; }

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
        public long id { get; set; }
        public string name { get; set; }
        public bool share_flag { get; set; }
        public bool favorite_flag { get; set; }
        public long category_id { get; set; }

        public string shareflag { get; set; }
        public string favoriteflag { get; set; }
        //public ImageSource ShareImageSource { get; set; }
        //public ImageSource favoriteImageSource { get; set; }
    }




    /// <summary>
    /// Secure share model
    /// </summary>
    public class SecuerShareData
    {
        public string recipient { get; set; }
        public string status { get; set; }
        public bool visibleAction { get; set; }
        public string sent { get; set; }
        public string uuid { get; set; }
        public ImageSource visible { get; set; }
    }
}
