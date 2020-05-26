using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.Model.PasswordVault
{
    public class secure_item
    {
       public string username { get; set; }
        public string nickname { get; set; }
        public bool require_master_password { get; set; }
        public bool autologin { get; set; }
        public bool favorite { get; set; }
        public string notes { get; set; }
        public string password { get; set; }
        public bool sub_domain { get; set; }
        public bool use_secure_browser { get; set; }
        

        public secure_item() { }
        public secure_item(string username, string nickname, bool require_master_password, bool autologin, bool favorite, string notes, string password, bool sub_domain, bool use_secure_browser)
        {
            this.autologin = autologin;
            this.favorite = favorite;
            this.nickname = nickname;
            this.notes = notes;
            this.password = password;
            this.sub_domain = sub_domain;
            this.require_master_password = require_master_password;
            this.username = username;
            this.use_secure_browser = use_secure_browser;
        }
        public secure_item(string json)
        {
            JsonConvert.DeserializeObject<secure_item>(json);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        
    }
}
