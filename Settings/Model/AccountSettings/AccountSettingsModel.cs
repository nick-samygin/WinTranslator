using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PasswordBoss.DTO;

namespace Settings.Model.AccountSettings
{
    /// <summary>
    /// Model for sync devices 
    /// </summary>
    public class syncdevices
    {
        public string uuid { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string devicename { get; set; }

        [JsonProperty(PropertyName = "created_date")]
        public DateTime date { get; set; }

        public string InstallationId { get; set; }

        public bool DeleteEnable { get; set; }
    }

    public class disabledSite
    {
        public string uuid { get; set; }

        public string sitename { get; set; }

        public DateTime date { get; set; }

        public string type { get; set; }

        public string InstallationId { get; set; }

        public bool DeleteEnable { get; set; }
    }


    public class StorageRegionModel : StorageRegion
    {
        public bool Checked { get; set; }
    }
}
