using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Model.ShareCenter;
using PasswordBoss.DTO;
using System.Windows;


namespace PasswordBoss.Helpers
{
    public class ShareCenterDataHelper
    {
     
        /// <summary>
        /// share center password  data
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<ShareCenterData> SharedCenterData(string secureItemTypeName, bool sent, bool received, IPBData pbData)
        { 
            Common common = new Common();
            List<ShareCenterData> items = new List<ShareCenterData>();
            if(pbData.Locked) return items;
            try
            {
                List<Share> data = pbData.GetShares(sent, received, secureItemTypeName, null);
                string unit = String.Empty;
                foreach (var item in data)
                {
                    //localize status
                    object locStatus = Application.Current.TryFindResource("State_"+item.Status);
                    string status = "Error";
                    if (locStatus != null)
                        status = locStatus.ToString();
                    ShareCenterData shd = new ShareCenterData()
                    {
                        recipient = item.Receiver,
                        sender = item.Sender,
                        localizedStatus = status,
                        sharedStatus = item.Status != null ? item.Status : "unknown",
                        sent = String.Format("{0}{1} ago", common.TimeDiffFromNow(item.CreatedDate, out unit).ToString(), unit),
                        expires = String.Format("{0}{1}", common.TimeDiffToDate(item.ExpirationDate, out unit).ToString(), unit),
                        action = "action",
                        uuid = item.Id,
                        visibleAction = true,
                        nickname = item.Nickname != null ? item.Nickname : String.Empty
                    };

                    if (shd.expires.Contains("---")) shd.expires = "---";

                    switch (item.SecureItemType)
	                {
                        case SecurityItemsDefaultProperties.SecurityItemType_PasswordVault:
                            shd.localizedSecureItemType = Application.Current.FindResource("PasswordVault").ToString();
                            break;
                        case SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet:
                            shd.localizedSecureItemType = Application.Current.FindResource("DigitalWallet").ToString();
                            break;
                        case SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo:
                            shd.localizedSecureItemType = Application.Current.FindResource("PersonalInfo").ToString();
                            break;
		
	                }
                    

                    items.Add(shd);
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error while reading share data");
            }
            
            return items;
        }

    

    }
}
