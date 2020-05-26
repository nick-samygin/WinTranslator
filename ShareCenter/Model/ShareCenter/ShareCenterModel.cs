using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.Model.ShareCenter
{
    public class ShareCenterData
    {
        public string uuid { get; set; }
        public string recipient { get; set; }
        public string sender { get; set; }
        public string nickname { get; set; }
        public string sent { get; set; }
        public string expires { get; set; }
        public string sharedStatus { get; set; }
        public string localizedStatus { get; set; }
        public string action { get; set; }
        public bool visibleAction { get; set; }

        public string localizedSecureItemType { get; set; }
    }

    public class ShareCenterPassword
    {
        public string uuid { get; set; }
        public string sender { get; set; }
        public string nickname { get; set; }
        public string sharedStatus { get; set; }
        public string sent { get; set; }
        public string expires { get; set; }
        public string action { get; set; }
        public bool visibleAction { get; set; }
    }  
}
