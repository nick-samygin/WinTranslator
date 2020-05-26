using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settings.Model.Account
{
    class AccountModel
    {
    }
    public class User
    {
        public bool active { get; set; }
        public string created_date { get; set; }
        public string last_modified_date { get; set; }
        public string emailId { get; set; }
        public string first_name { get; set; }
        public int overlay { get; set; }
        public int mobile_no { get; set; }
        public string public_key { get; set; }
        public string private_key { get; set; }
        public string country_id { get; set; }
        public string pin_number { get; set; }
        public string hash { get; set; }
        public string password { get; set; }
    }
    public class Languageobject
    {
        public string status { get; set; }
        public object message { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string iso { get; set; }
    }

    public class LanguageItem
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string iso { get; set; }
    }
}
