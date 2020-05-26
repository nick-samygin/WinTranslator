using Newtonsoft.Json;
//using OpenSSL.Crypto;
using PasswordBoss.DTO;
using PasswordBoss.WEBApiJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.Helpers
{


    internal class ShareCommon
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(ShareCommon));
        IPBData pbData = null;
        IPBWebAPI pbWebApi = null;
        IInAppAnalytics inAppAnalyitics = null;
        IResolver resolver;
        public ShareCommon(IResolver resolver)
        {
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.pbWebApi = resolver.GetInstanceOf<IPBWebAPI>();
            inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
            this.resolver = resolver;
        }


        #region refactored EncriptItemForShare
        public string EncriptItemForShare(Share share, string receiverPublicKey)
        {
            string payloadPlain = share.GetShareJson();
            string encriptionKey = String.Empty;
            AESKeySet keySet = null;
            string payload = pbData.EcryptAndSignWithAES(payloadPlain, out keySet);//.EncriptWithAES(payloadPlain, out encriptionKey);
            encriptionKey = pbData.EncriptWithRSA(receiverPublicKey, keySet.KeysToString());
            return JsonConvert.SerializeObject(new { key = encriptionKey, payload = payload }, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        #endregion

        BackgroundWorker bkgUpdateShare;
        public void UpdateShares(SecureItem item)
        {
            bkgUpdateShare = new BackgroundWorker();
            bkgUpdateShare.WorkerReportsProgress = true;
            bkgUpdateShare.DoWork += bkgUpdateShare_DoWork;
            bkgUpdateShare.RunWorkerAsync(item);
        }

        void bkgUpdateShare_DoWork(object sender, DoWorkEventArgs e)
        {
            var item = e.Argument as SecureItem;
            //List<Share> shares = pbData.GetSharesBySecureItemId(item.Id);
            List<Share> shares = pbData.GetSharesBySecureItemIdAndReceiverOrSender(item.Id);
            int progress = 0;
            //foreach (var share in shares)
            for (int i = 0; i < shares.Count; i++)
            {
                var share = shares[i];
                if (share.Status != ShareStatus.Pending && share.Status != ShareStatus.Shared && share.Status != ShareStatus.Updated) continue;
                if (share.Sender == pbData.ActiveUser)
                {
                    UpdateShare(share, item, item.Active ? ShareStatus.Updated : ShareStatus.Revoked);
                }
                else if (share.Receiver == pbData.ActiveUser)
                {
                    UpdateShare(share, item, ShareStatus.Rejected);
                }

                progress = Convert.ToInt32(Decimal.Divide(i + 1, shares.Count) * 100 * 100);
                bkgUpdateShare.ReportProgress(progress);
            }
        }

        public bool UpdateShare(Share share, SecureItem item, string newStatus)
        {
            try
            {
                ShareRequest reqData = new ShareRequest();
                reqData.uuid = share.Id.ToString();

                reqData.data = EncriptItemForShare(share, Encoding.UTF8.GetString(Convert.FromBase64String(share.ReceiverPrivateKey)));

                dynamic response = pbWebApi.RequestShare(reqData, String.Format("{0}|{1}", pbData.ActiveUser, pbData.DeviceUUID));

                if (response.error != null)
                {
                    MessageBox.Show(response.error.details.ToString(), response.error.message.ToString());
                    return false;
                }

                share.Data = reqData.data;
                share.Status = reqData.status;
                pbData.AddOrUpdateShare(share);
                return true;
            }
            catch
            {

            }
            return false;
        }
        #region refactored ShareItem
         public List<SecuerShareData> ShareItem(string receiver, string message, SecureItem secureItem, int expirationPeriodIndex, bool passwordVisibleToRecipient)
        {
             return ShareItem(receiver,message, secureItem,expirationPeriodIndex, passwordVisibleToRecipient);
        }
        public List<SecuerShareData> ShareItem(string receiver, string message,SecureItem secureItem, int expirationPeriodIndex, bool passwordVisibleToRecipient, DateTime? expirationDate)
        {
            Share share = new Share()
            {
                Receiver = receiver,
                Message = message,
                Visible = passwordVisibleToRecipient,
                ExpirationDate = expirationDate ?? DateTime.Now.AddYears(50).ToUniversalTime()
        };

            Share shareItem = new Share();
            logger.Info("Start ShareItem");
            try
            {
                bool isShareAllowed = true;
                bool isShareTresholdReached = pbData.IsShareTresholdReached(false, true);
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();

                isShareAllowed = featureChecker.IsEnabled(DefaultProperties.Features_ShareCenter_UnlimitedShares, showUIIfNotEnabled: false);

                if (!isShareAllowed)
                {
                    isShareAllowed = featureChecker.IsEnabled(DefaultProperties.Features_ShareCenter_UpTo5Shares, showUIIfNotEnabled: false) && !isShareTresholdReached;
                }

                if (!isShareAllowed)
                {
                    featureChecker.FireActionNotEnabledUI();
                    return new List<SecuerShareData>();
                }

                //validate emails
                List<string> receiverList = new List<string>();
                share.Receiver = share.Receiver.Replace(" ", String.Empty);
                if (share.Receiver.Contains(','))
                    receiverList.AddRange(share.Receiver.Replace(" ", String.Empty).Split(','));
                if (share.Receiver.Contains(';'))
                    receiverList.AddRange(share.Receiver.Replace(" ", String.Empty).Split(';'));

                if (receiverList.Count == 0)
                    receiverList.Add(share.Receiver);

                Common cm = new Common();

                //Changed
                List<Share> shareList = pbData.GetShares(true,false,null,null);

                List<string> alreadyShared = shareList.Select(x => x.Receiver).ToList<string>();

                foreach (var rec in receiverList)
                {
                    if (!cm.IsEmailValid(rec))
                        continue;
                    if (rec == pbData.ActiveUser)
                        continue;
                    if (alreadyShared.Contains(rec))
                        continue;
                    else
                        alreadyShared.Add(rec);

                    ShareRequest reqData = new ShareRequest()
                    {
                        receiver = rec,
                        secure_item_type =null,
                        status = ShareStatus.Waiting,
                        nickname = share.Nickname,
                        expiration_date =share.ExpirationDateString,
                        message = share.Message
                    };

                    dynamic response = pbWebApi.RequestShare(reqData, String.Format("{0}|{1}", pbData.ActiveUser, pbData.DeviceUUID));

                    if (response.error != null)
                    {
                        MessageBox.Show(response.error.details.ToString(), response.error.message.ToString());
                        return new List<SecuerShareData>();
                    }

                    

                    if (response.shares.sent != null)
                    {
                        if (response.shares.sent[0] != null)
                        {
                            dynamic sent = response.shares.sent[0];
                         
                          
                            if (!String.IsNullOrEmpty(sent.public_key.ToString()))
                            {

                                string receiverPublicKey = sent.public_key.ToString();

                                // Don't make recipient item as favorite. UATD-387
                                if(share.SharedItem == SharedItems.folder)
                                {
                                    foreach(Folder folder in share.Folders)
                                    {
                                        foreach(SecureItem item in folder.SecureItems)
                                        {
                                item.Favorite = false;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (SecureItem item in share.SecureItems)
                                    {
                                        item.Favorite = false;
                                    }
                                }
                                reqData.data = EncriptItemForShare(share, Encoding.UTF8.GetString(Convert.FromBase64String(receiverPublicKey)));// JsonConvert.SerializeObject(new { key = encriptionKey, payload = payload }, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                                reqData.status = ShareStatus.Pending;
                                reqData.message = share.Message;


                                if (!expirationDate.HasValue)
                                {
                                    switch (expirationPeriodIndex)
                                    {
                                        case 0:
                                            share.ExpirationDate = DateTime.Now.AddYears(50).ToUniversalTime();
                                            break;
                                        case 1:
                                            share.ExpirationDate = DateTime.Now.AddDays(1).ToUniversalTime();
                                            break;
                                        case 2:
                                            share.ExpirationDate = DateTime.Now.AddDays(7).ToUniversalTime();
                                            break;
                                        case 3:
                                            share.ExpirationDate = DateTime.Now.AddMonths(1).ToUniversalTime();
                                            break;
                                        case 4:
                                            share.ExpirationDate = DateTime.Now.AddYears(1).ToUniversalTime();
                                            break;
                                        default:
                                            share.ExpirationDate = DateTime.Now.AddYears(50).ToUniversalTime();
                                            break;
                                    }
                                }
                                reqData.expiration_date = share.ExpirationDateString;
                                reqData.order = "0";
                                reqData.uuid = sent.uuid.ToString();
                                reqData.visible = share.Visible;


                                response = pbWebApi.RequestShare(reqData, String.Format("{0}|{1}", pbData.ActiveUser, pbData.DeviceUUID));

                                if (response.error != null)
                                {
                                    MessageBox.Show(response.error.details.ToString(), response.error.message.ToString());
                                    return new List<SecuerShareData>();
                                }
                                sent = response.shares.sent[0];
                                shareItem.Receiver = reqData.receiver;
                                shareItem.Sender = pbData.ActiveUser;
                                shareItem.ExpirationDate = expirationDate.Value;
                                shareItem.Data = reqData.data;

                                shareItem.Id = sent.uuid.ToString();
                                shareItem.UUID = sent.uuid.ToString();
                                shareItem.Status = sent.status;
                                shareItem.Nickname = sent.nickname;
                                shareItem.Message = reqData.message;

                                shareItem.SecureItemType = reqData.secure_item_type;
                                shareItem.ReceiverPrivateKey = receiverPublicKey;
                                shareItem.Visible = reqData.visible;
                                pbData.AddOrUpdateShare(shareItem);

                                //return BindingSecureShareList(item.Id);
                                //CASE #1
                            }
                            else
                            {
                                sent = response.shares.sent[0];
                                shareItem.Receiver = reqData.receiver;
                                shareItem.Sender = pbData.ActiveUser;
                                shareItem.ExpirationDate = DateTime.Now.AddDays(1).ToUniversalTime();
                                shareItem.Data = reqData.data;

                                shareItem.Id = sent.uuid.ToString();
                                shareItem.UUID = sent.uuid.ToString();
                                shareItem.Status = sent.status;
                                shareItem.Nickname = sent.nickname;
                                shareItem.Message = reqData.message;

                                shareItem.SecureItemType = reqData.secure_item_type;
                                shareItem.ReceiverPrivateKey = null;
                                shareItem.Visible = reqData.visible;
                                pbData.AddOrUpdateShare(shareItem);

                                //return BindingSecureShareList(item.Id);
                                //CASE #2 - user does not exist - no data encription
                            }


                            if (inAppAnalyitics != null)
                            {

                                doShareAnalytics(shareItem, ShareEventStatus.Shared);
                            }
                        }

                    }
                }
                //((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
                return BindingSecureShareList(shareItem);

            }
            catch (Exception ex)
            {
                logger.Error("Error in ShareItem -->" + ex.ToString());
                MessageBox.Show("Error while saving share");
            }
            return new List<SecuerShareData>();
        }
        #endregion

        #region old BindingSecureShareList (in use: ViewModel classes)
        public List<SecuerShareData> BindingSecureShareList(string secureItemId)
        {
            Common common = new Common();
            List<Share> shares = new List<Share>();
            List<SecuerShareData> items = new List<SecuerShareData>();
            try
            {
                shares = pbData.GetSharesBySecureItemId(secureItemId);
                string unit = String.Empty;
                foreach (var share in shares)
                {
                    TimeSpan ts = (DateTime.Now - share.CreatedDate);
                    items.Add(new SecuerShareData()
                    {
                        recipient = share.Receiver,
                        status = share.Status != null ? share.Status : "unknown",
                        sent = String.Format(System.Windows.Application.Current.FindResource("ShareSentLabel").ToString(), String.Format("{0}{1}", common.TimeDiffFromNow(share.CreatedDate, out unit).ToString(), unit)),// ts.Days > 0 ? ts.Days.ToString() + " Day" : ts.Hours > 0 ? ts.Hours.ToString() + " hour" : ts.Minutes > 0 ? ts.Minutes.ToString() + " minute" : ts.Seconds.ToString() + " second",
                        uuid = share.Id,
                        visible = share.Visible,
                        localizedStatus = System.Windows.Application.Current.FindResource("State_" + share.Status).ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO log exception
            }


            return items;
        }
        #endregion

        #region new BindingSecureShareList 
        public List<SecuerShareData> BindingSecureShareList(Share share)
        {
            Common common = new Common();
            string unit = String.Empty;
            List<SecuerShareData> items = new List<SecuerShareData>();
                    TimeSpan ts = (DateTime.Now - share.CreatedDate);
                    items.Add(new SecuerShareData()
                    {
                        recipient = share.Receiver,
                        status = share.Status != null ? share.Status : "unknown",
                        sent = String.Format(System.Windows.Application.Current.FindResource("ShareSentLabel").ToString(), String.Format("{0}{1}", common.TimeDiffFromNow(share.CreatedDate, out unit).ToString(), unit)),
                        uuid = share.Id,
                        visible = share.Visible,
                        localizedStatus = System.Windows.Application.Current.FindResource("State_" + share.Status).ToString()
                    });
            return items;
        }
        #endregion

        public bool UpdateShareStatus(string shareUuid, string status, bool active, string secureItemId)
        {

            ShareRequest reqData = new ShareRequest()
            {
                uuid = shareUuid,
                status = status
            };

            dynamic response = pbWebApi.RequestShare(reqData, String.Format("{0}|{1}", pbData.ActiveUser, pbData.DeviceUUID));
            if (response.error != null)
            {
                System.Windows.Forms.MessageBox.Show(response.error.details.ToString(), response.error.message.ToString());
                return false;
            }
            else
            {
                if (response.shares.received.Count > 0)
                {
                    if (response.shares.received[0].status.ToString().ToLower() != status.ToLower())
                    {
                        MessageBox.Show("Error while executing action on server");
                        return false;
                    }
                }
                else if (response.shares.sent.Count > 0)
                {
                    if (response.shares.sent[0].status.ToString().ToLower() != status.ToLower())
                    {
                        MessageBox.Show("Error while executing action on server");
                        return false;
                    }
                }

                var shareItem = pbData.GetSharesByUuid(shareUuid);

                #region refactored part UpdateShareStatus
                var share = new Share();
                share = shareItem;
                share.Status = status;
                share.Active = active;


                if (status == ShareStatus.Shared || status == ShareStatus.Rejected || status == ShareStatus.Revoked)
                {
                    share.Data = null;
                }
                //update share status
                if (!pbData.UpdateShareStatus(share))
                {
                    System.Windows.Forms.MessageBox.Show("Error while saving share, please perform sync");
                    return false;
                }
                #endregion



                ShareEventStatus? evStatus = null;

                switch (status)
                {
                    case ShareStatus.Rejected: evStatus = ShareEventStatus.Rejected;
                        break;
                    case ShareStatus.Shared: evStatus = ShareEventStatus.Accepted;
                        break;
                    case ShareStatus.Expired: evStatus = ShareEventStatus.Expired;
                        break;
                    case ShareStatus.Canceled: evStatus = ShareEventStatus.Canceled;
                        break;
                    case ShareStatus.Revoked: evStatus = ShareEventStatus.Canceled;
                        break;
                }

                #region refactored Analytics
                if (inAppAnalyitics != null)
                {
                    doShareAnalytics(shareItem, evStatus);
                }
                #endregion

            }
            return true;
        }

        private void doShareAnalytics(Share shareItem, ShareEventStatus? evStatus)
        {
                try
                {
                if (shareItem != null)
                    {
                    if (shareItem.SharedItem == SharedItems.folder)
                    {
                        if (shareItem.Folders.Count > 0)
                        {
                            foreach (var folder in shareItem.Folders)
                            {
                                foreach (var item in folder.SecureItems)
                                {
                                    doAnalytics(evStatus, item);
                                }
                            }
                        }
                    }
                    else {
                        if (shareItem.SecureItems.Count > 0)
                        {
                            foreach (var item in shareItem.SecureItems)
                            {
                                doAnalytics(evStatus, item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
            }
        }

        private void doAnalytics (ShareEventStatus? evStatus,SecureItem item)
        {
                        if (evStatus.HasValue)
                        {
                            var analytics2 = inAppAnalyitics.Get<Events.Sharing, SharingItem>();


                if (item.SecureItemTypeName == DefaultProperties.SecurityItemType_PasswordVault)
                            {
                                analytics2.Log(new SharingItem(ShareItemType.Password, evStatus.Value));
                            }

                if (item.SecureItemTypeName == DefaultProperties.SecurityItemType_DigitalWallet)
                            {
                                var shareType = SharingItem.GetShareEventItemTypeBaySecureItemType(item.Type);
                                if (shareType.HasValue)
                                {
                                    analytics2.Log(new SharingItem(shareType.Value, evStatus.Value));
                                }
                            }

                if (item.SecureItemTypeName == DefaultProperties.SecurityItemType_PersonalInfo)
                            {
                                var shareType = SharingItem.GetShareEventItemTypeBaySecureItemType(item.Type);
                                if (shareType.HasValue)
                                {
                                    analytics2.Log(new SharingItem(shareType.Value, evStatus.Value));
                                }
                            }
                        }
                    }

    }


    public class SecuerShareData
    {
        public string recipient { get; set; }
        public string status { get; set; }
        public string localizedStatus { get; set; }
        public bool visible { get; set; }
        public string sent { get; set; }
        public string uuid { get; set; }
        //public ImageSource visible { get; set; }
    }
}
