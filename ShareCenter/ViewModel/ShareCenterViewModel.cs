using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using PasswordBoss.Helpers;
using PasswordBoss.Model.ShareCenter;
using PasswordBoss.WEBApiJSON;
using PasswordBoss.DTO;

namespace PasswordBoss.ViewModel
{
    internal class ShareCenterViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(ShareCenterViewModel));

        #region Relay commands
        public RelayCommand ShareCenterSelectionChangedCommand { get; set; }
        public RelayCommand ShareCenterCreditCardCommand { get; set; }
        public RelayCommand ShareCenterBankAccountCommand { get; set; }
        public RelayCommand ShareCenterPayPalCommand { get; set; }
        public RelayCommand ShareCenterViewAllCommand { get; set; }

        public RelayCommand AcceptShareCommand { get; set; }
        public RelayCommand AcceptShareRequestCommand { get; set; }
        public RelayCommand RejectShareCommand { get; set; }
        public RelayCommand ResendShareCommand { get; set; }
        public RelayCommand RemoveShareCommand { get; set; }


        public RelayCommand CancelShareCommand { get; set; }
        public RelayCommand RevokeShareCommand { get; set; }
        public RelayCommand SendDataShareCommand { get; set; }

        public RelayCommand SortSharedByMeAscendingCommand { get; set; }
        public RelayCommand SortSharedByMeDescendingCommand { get; set; }

        public RelayCommand SortSharedWithMeAscendingCommand { get; set; }
        public RelayCommand SortSharedWithMeDescendingCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        #endregion
        IPBData pbData = null;
        IResolver resolver = null;
        IPBSync pbSync = null;
        ShareCommon shareCommon = null;
        List<Folder> categoryList = new List<Folder>();
        string currentUUID = null;
        public ShareCenterViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.pbSync = resolver.GetInstanceOf<IPBSync>();
            shareCommon = new ShareCommon(resolver);
            InitializeCommands();
            SelectedIndexTabControl = 0;
            //vedo - async
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                categoryList = pbData.GetFoldersBySecureItemType();
            });

            SortSharedWithMeAscendingVisibility = true;
            SortSharedWithMeDescendingVisibility = false;
            
            SortSharedByMeAscendingVisibility = true;
            SortSharedByMeDescendingVisibility = false;
        }

        #region otherMethods

        private void InitializeCommands()
        {
            ShareCenterSelectionChangedCommand = new RelayCommand(ShareCenterSelectionChanged);
            ShareCenterCreditCardCommand = new RelayCommand(ShareCenterCardClick);
            ShareCenterBankAccountCommand = new RelayCommand(ShareCenterAccountClick);
            ShareCenterPayPalCommand = new RelayCommand(ShareCenterPayPalClick);
            ShareCenterViewAllCommand = new RelayCommand(ShareCenterViewAll);
            
            AcceptShareCommand = new RelayCommand(AcceptShare);
            AcceptShareRequestCommand = new RelayCommand(AcceptShareRequest);
            RejectShareCommand = new RelayCommand(RejectShare);
            
            ResendShareCommand = new RelayCommand(ResendShare);
            RemoveShareCommand = new RelayCommand(RemoveShare);
            CancelShareCommand = new RelayCommand(CancelShare);
            RevokeShareCommand = new RelayCommand(RevokeShare);
            SendDataShareCommand = new RelayCommand(SendDataShare);

            SortSharedByMeAscendingCommand = new RelayCommand(SortSharedByMeAscending);
            SortSharedByMeDescendingCommand = new RelayCommand(SortSharedByMeDescending);

            SortSharedWithMeAscendingCommand = new RelayCommand(SortSharedWithMeAscending);
            SortSharedWithMeDescendingCommand = new RelayCommand(SortSharedWithMeDescending);

            CancelCommand = new RelayCommand(CancelClick);

        }


        private void CancelClick(object obj)
        {
            AcceptMessageBoxVisibility = false;
            RejectMessageBoxVisibility = false;
            CancelMessageBoxVisibility = false;
            RemoveMessageBoxVisibility = false;
            ResendMessageBoxVisibility = false;
            UnshareMessageBoxVisibility = false;
            currentUUID = null;
        }
        public void DisplaySharesForItem(Share item)
        {


            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(item.SecureItemType, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(item.SecureItemType, false, true, pbData);
            SortSharedByMeData();
            SortSharedWithMeData();
           
        }

        private void SortSharedWithMeDescending(object obj)
        {
           
            SortSharedWithMeAscendingVisibility = true;
            SortSharedWithMeDescendingVisibility = false;
            SortSharedWithMeData();
        }

        private void SortSharedWithMeAscending(object obj)
        {
           
            SortSharedWithMeAscendingVisibility = false;
            SortSharedWithMeDescendingVisibility = true;
            SortSharedWithMeData();
        }
        private void SortSharedWithMeData()
        {
            if (SortSharedWithMeAscendingVisibility)
            {
                ShareCenterReceivedData = ShareCenterReceivedData.OrderByDescending(x => x.sender).ToList<ShareCenterData>();
            }
            else if (SortSharedWithMeDescendingVisibility)
            {
                ShareCenterReceivedData = ShareCenterReceivedData.OrderBy(x => x.sender).ToList<ShareCenterData>();
            }
        }

        private void SortSharedByMeDescending(object obj)
        {
            
            SortSharedByMeAscendingVisibility = true;
            SortSharedByMeDescendingVisibility = false;
            SortSharedByMeData();
        }

        private void SortSharedByMeAscending(object obj)
        {
            
            SortSharedByMeAscendingVisibility = false;
            SortSharedByMeDescendingVisibility = true;
            SortSharedByMeData();
        }

        private void SortSharedByMeData()
        {
            if (SortSharedByMeAscendingVisibility)
            {
                ShareCenterSentData = ShareCenterSentData.OrderByDescending(x => x.recipient).ToList<ShareCenterData>();
            }
            else if (SortSharedByMeDescendingVisibility)
            {
                ShareCenterSentData = ShareCenterSentData.OrderBy(x => x.recipient).ToList<ShareCenterData>();
            }
        }


        #endregion


        #region Properties
        private bool _acceptMessageBoxVisibility;

        public bool AcceptMessageBoxVisibility
        {
            get { return _acceptMessageBoxVisibility; }
            set
            {
                _acceptMessageBoxVisibility = value;
                RaisePropertyChanged("AcceptMessageBoxVisibility");
            }
        }

        private bool _rejectMessageBoxVisibility;

        public bool RejectMessageBoxVisibility
        {
            get { return _rejectMessageBoxVisibility; }
            set
            {
                _rejectMessageBoxVisibility = value;
                RaisePropertyChanged("RejectMessageBoxVisibility");
            }
        }

        private bool _unshareMessageBoxVisibility;

        public bool UnshareMessageBoxVisibility
        {
            get { return _unshareMessageBoxVisibility; }
            set
            {
                _unshareMessageBoxVisibility = value;
                RaisePropertyChanged("UnshareMessageBoxVisibility");
            }
        }

        private bool _cancelMessageBoxVisibility;

        public bool CancelMessageBoxVisibility
        {
            get { return _cancelMessageBoxVisibility; }
            set
            {
                _cancelMessageBoxVisibility = value;
                RaisePropertyChanged("CancelMessageBoxVisibility");
            }
        }

        private bool _resendMessageBoxVisibility;

        public bool ResendMessageBoxVisibility
        {
            get { return _resendMessageBoxVisibility; }
            set
            {
                _resendMessageBoxVisibility = value;
                RaisePropertyChanged("ResendMessageBoxVisibility");
            }
        }

        private bool _removeMessageBoxVisibility;

        public bool RemoveMessageBoxVisibility
        {
            get { return _removeMessageBoxVisibility; }
            set
            {
                _removeMessageBoxVisibility = value;
                RaisePropertyChanged("RemoveMessageBoxVisibility");
            }
        }



        private bool _sortSharedByMeDescendingVisibility;

        public bool SortSharedByMeDescendingVisibility
        {
            get { return _sortSharedByMeDescendingVisibility; }
            set
            {
                _sortSharedByMeDescendingVisibility = value;
                RaisePropertyChanged("SortSharedByMeDescendingVisibility");
            }
        }

        private bool _sortSharedByMeAscendingVisibility;

        public bool SortSharedByMeAscendingVisibility
        {
            get { return _sortSharedByMeAscendingVisibility; }
            set
            {
                _sortSharedByMeAscendingVisibility = value;
                RaisePropertyChanged("SortSharedByMeAscendingVisibility");
            }
        }

        private bool _sortSharedWithMeDescendingVisibility;

        public bool SortSharedWithMeDescendingVisibility
        {
            get { return _sortSharedWithMeDescendingVisibility; }
            set
            {
                _sortSharedWithMeDescendingVisibility = value;
                RaisePropertyChanged("SortSharedWithMeDescendingVisibility");
            }
        }

        private bool _sortSharedWithMeAscendingVisibility;

        public bool SortSharedWithMeAscendingVisibility
        {
            get { return _sortSharedWithMeAscendingVisibility; }
            set
            {
                _sortSharedWithMeAscendingVisibility = value;
                RaisePropertyChanged("SortSharedWithMeAscendingVisibility");
            }
        }




        private bool _creditBtnIsChecked = true;
        public bool CreditBtnIsChecked
        {
            get { return _creditBtnIsChecked; }
            set
            {
                _creditBtnIsChecked = value;
                RaisePropertyChanged("CreditBtnIsChecked");
            }
        }

        private bool _bankBtnIsChecked;
        public bool BankBtnIsChecked
        {
            get { return _bankBtnIsChecked; }
            set
            {
                _bankBtnIsChecked = value;
                RaisePropertyChanged("BankBtnIsChecked");
            }
        }

        private bool _paypalBtnIsChecked;
        public bool PaypalBtnIsChecked
        {
            get { return _paypalBtnIsChecked; }
            set
            {
                _paypalBtnIsChecked = value;
                RaisePropertyChanged("PaypalBtnIsChecked");
            }
        }

        private int _selectedIndexTabControl;
        public int SelectedIndexTabControl
        {
            get { return _selectedIndexTabControl; }
            set
            {
                _selectedIndexTabControl = value;
                RaisePropertyChanged("SelectedIndexTabControl");
            }
        }

        List<ShareCenterData> _shareCenterSentData;
        public List<ShareCenterData> ShareCenterSentData
        {
            get { return _shareCenterSentData; }
            set
            {
                _shareCenterSentData = value;
                RaisePropertyChanged("ShareCenterSentData");
            }
        }

        List<ShareCenterData> _shareCenterReceivedData;
        public List<ShareCenterData> ShareCenterReceivedData
        {
            get { return _shareCenterReceivedData; }
            set
            {
                _shareCenterReceivedData = value;
                RaisePropertyChanged("ShareCenterReceivedData");
            }
        }

        List<ShareCenterData> _shareCenterPersonal;
        public List<ShareCenterData> ShareCenterPersonal
        {
            get { return _shareCenterPersonal; }
            set
            {
                _shareCenterPersonal = value;
                RaisePropertyChanged("ShareCenterPersonal");
            }
        }

        List<ShareCenterData> _shareCenterPersonalShared;
        public List<ShareCenterData> ShareCenterPersonalShared
        {
            get { return _shareCenterPersonalShared; }
            set
            {
                _shareCenterPersonalShared = value;
                RaisePropertyChanged("ShareCenterPersonalShared");
            }
        }

        #endregion

        /// <summary>
        /// view share center bank account grid
        /// </summary>
        /// <param name="obj"></param>
        private void ShareCenterAccountClick(object obj)
        {
            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, false, true, pbData);
            SortSharedWithMeData();
            SortSharedByMeData();
        }

        public void RefreshData()
        {
            string secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
            switch (SelectedIndexTabControl)
            {
                case 0:
                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
                    break;
                case 1:
                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
                    break;
                case 2:

                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
                    break;
            }


            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(secureItemType, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(secureItemType, false, true, pbData);
            SortSharedByMeData();
            SortSharedWithMeData();
        }

        /// <summary>
        /// view share center credit card grid
        /// </summary>
        /// <param name="obj"></param>
        private void ShareCenterCardClick(object obj)
        {
            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, false, true, pbData);
            SortSharedByMeData();
            SortSharedWithMeData();
        }

        /// <summary>
        /// view share center pay pal grid
        /// </summary>
        /// <param name="obj"></param>
        private void ShareCenterPayPalClick(object obj)
        {
            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, false, true, pbData);
            SortSharedByMeData();
            SortSharedWithMeData();
        }

        /// <summary>
        /// view all items in share center category
        /// </summary>
        /// <param name="obj"></param>
        private void ShareCenterViewAll(object obj)
        {
            CreditBtnIsChecked = false;
            BankBtnIsChecked = false;
            PaypalBtnIsChecked = false;
            
            ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, true, false, pbData);
            ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet, false, true, pbData);
            SortSharedByMeData();
            SortSharedWithMeData();
        }

        

        

        #region SENDER side actions
        private void RemoveShare(object obj)
        {
            try
            {
                if (!RemoveMessageBoxVisibility)
                {
                    if (obj == null) return;
                    currentUUID = obj as string;
                    RemoveMessageBoxVisibility = true;
                    return;
                }
                RemoveMessageBoxVisibility = false;
                if (currentUUID == null) return;
                var uuid = currentUUID as string;
                currentUUID = null;
                var share = pbData.GetSharesByUuid(uuid);

                shareCommon.UpdateShareStatus(uuid, ShareStatus.Removed, true, share.SecureItemId);
                UpdateData(true, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }


        private void ResendShare(object obj)
        {
            try
            {
                if (!ResendMessageBoxVisibility)
                {
                    if (obj == null) return;
                    currentUUID = obj as string;
                    ResendMessageBoxVisibility = true;
                    return;
                }
                ResendMessageBoxVisibility = false;
                if (currentUUID == null) return;
                var uuid = currentUUID as string;
                currentUUID = null;
                var share = pbData.GetSharesByUuid(uuid);

                TimeSpan tmp = DateTime.Now - share.CreatedDate;
                share.ExpirationDate.AddDays(tmp.Days);
                
                shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null);
                shareCommon.ShareItem(share.Receiver, share.Message, pbData.GetSecureItemById(share.SecureItemId), 0, share.Visible, share.ExpirationDate);

                
                UpdateData(true, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private void RevokeShare(object obj)
        {
            try
            {
                if (!UnshareMessageBoxVisibility)
                {
                    if (obj == null) return;
                    currentUUID = obj as string;
                    UnshareMessageBoxVisibility = true;
                    return;
                }
                UnshareMessageBoxVisibility = false;
                if (currentUUID == null) return;
                var uuid = currentUUID as string;
                currentUUID = null;
                if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Revoked, false, null))
                    UpdateData(true, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
        private void CancelShare(object obj)
        {
            try
            {
                if (!CancelMessageBoxVisibility)
                {
                    if (obj == null) return;
                    currentUUID = obj as string;
                    CancelMessageBoxVisibility = true;
                    return;
                }
                CancelMessageBoxVisibility = false;
                if (currentUUID == null) return;
                var uuid = currentUUID as string;
                currentUUID = null;
                if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Canceled, false, null))
                    UpdateData(true, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
        private void SendDataShare(object obj)
        {
            try
            {
                var uuid = obj as string;
                if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Pending, true, null))
                    UpdateData(true, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
        #endregion

        #region RECEIVER side actions

        private void AcceptShare(object obj)
        {
            if(!AcceptMessageBoxVisibility)
            {
                if (obj == null) return;
                currentUUID = obj as string;
                AcceptMessageBoxVisibility = true;
                return;
            }
            AcceptMessageBoxVisibility = false;
            if (currentUUID == null) return;
            var uuid = currentUUID as string;
            currentUUID = null;
            if (String.IsNullOrWhiteSpace(uuid)) return;
            try
            {
                bool isShareAllowed = true;
                bool isShareTresholdReached = pbData.IsShareTresholdReached(true, false);
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();

                isShareAllowed = featureChecker.IsEnabled(DefaultProperties.Features_ShareCenter_UnlimitedShares, showUIIfNotEnabled: false);

                if (!isShareAllowed)
                {
                    isShareAllowed = featureChecker.IsEnabled(DefaultProperties.Features_ShareCenter_UpTo5Shares, showUIIfNotEnabled: false) && !isShareTresholdReached;
                }

                if (!isShareAllowed)
                {
                    featureChecker.FireActionNotEnabledUI();
                    return;
                }


                
                //TODO extract data and save secureItem
                Share item = pbData.GetSharesByUuid(uuid);
                if (item != null)
                {
                    IPBWebAPI wepApi = resolver.GetInstanceOf<IPBWebAPI>();
                    dynamic share = wepApi.JsonStringToDynamic(item.Data);
                    if (share != null)
                    {
                        string encriptionKey = share.key.ToString();
                        UserInfo ui = pbData.GetUserInfo(pbData.ActiveUser);

                        encriptionKey = pbData.DecriptWithRSA(ui.RSAPrivateKey, encriptionKey);
                        AESKeySet keySet = AESKeySet.KeysFromString(encriptionKey);
                        string encriptedPayload = share.payload.ToString();
                        string data = pbData.DecryptAndVerifyWithAES(encriptedPayload, keySet);//.DecriptWithAES(encriptedPayload, encriptionKey);

                        SecureItemShare sharedSecureItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SecureItemShare>(data);
                        sharedSecureItem.data.password_visible_recipient = item.Visible;
                        string secureItemId = null;
                        if (SaveSecureItem(sharedSecureItem, out secureItemId))
                        {
                            if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Shared, true, secureItemId))
                                UpdateData(false, true);

                            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ReloadData", null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Error while Accepting share with uuid -> {0} with error-> {1}", uuid, ex.ToString()));
            }
        }

        private bool SaveSecureItem(SecureItemShare sharedSecureItem, out string secureItemId)
        {
            secureItemId = null;
            try
            {
                SecureItem si = new SecureItem();
                si.SecureItemTypeName = sharedSecureItem.secure_item_type_name;
                si.Type = sharedSecureItem.type;
                si.Folder = categoryList.FirstOrDefault(x => x.Id == sharedSecureItem.category_id);
                si.Data = sharedSecureItem.data;
                si.Name = sharedSecureItem.name;
                si.Color = sharedSecureItem.color;
                si.LoginUrl = sharedSecureItem.login_url;
                si.Favorite = sharedSecureItem.favorite;
               
                if (si.Folder == null)
                {
                    si.Folder = new Folder() { Name = sharedSecureItem.category_name };
                }

                if (si.SecureItemTypeName == DefaultProperties.SecurityItemType_PasswordVault)
                {
                    if (!sharedSecureItem.site_url.Contains("http") && !sharedSecureItem.site_url.Contains("https"))
                        sharedSecureItem.site_url = "http://" + sharedSecureItem.site_url;
                    //Uri siteUrl = new Uri(sharedSecureItem.site_url);
                    //var parts = siteUrl.Host.Split('.');
                    
                    si.Site = new Site();

                    var siteId = pbData.GetSiteIdByUriFullSearch(new Uri(sharedSecureItem.site_url));
                    if (siteId == null)
                    {
                        //TODO: Call service to retreive from server

                        string uuid = pbSync.RegisterSite(new Uri(sharedSecureItem.site_url));
                        if (uuid != null)
                        {
                            siteId = pbData.GetSiteIdByUUID(uuid);
                        }

                    }


                    si.Site.Id = siteId;
                }
                else
                {
                    si.Data.password_visible_recipient = null;
                }

                
               
                si = pbData.AddOrUpdateSecureItem(si);
                if (si.Id != null)
                {
                    secureItemId = si.Id;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        private void AcceptShareRequest(object obj)
        {
            try
            {
                var uuid = obj as string;
                if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Waiting4Data, true, null))
                    UpdateData(false, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private void RejectShare(object obj)
        {
            try
            {
                if (!RejectMessageBoxVisibility)
                {
                    if (obj == null) return;
                    currentUUID = obj as string;
                    RejectMessageBoxVisibility = true;
                    return;
                }
                RejectMessageBoxVisibility = false;
                if (currentUUID == null) return;
                var uuid = currentUUID as string;
                currentUUID = null;

                if (shareCommon.UpdateShareStatus(uuid, ShareStatus.Rejected, false, null))
                    UpdateData(false, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
        #endregion
       

        /// <summary>
        /// share center tab selection change event for cview changing
        /// </summary>
        /// <param name="obj"></param>
        private void ShareCenterSelectionChanged(object obj)
        {
            UpdateData();
        }


        private void UpdateData()
        {
            UpdateData(true, true);
        }
        private void UpdateData(bool updateSent, bool updateReceived)
        {
            string secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
            switch (SelectedIndexTabControl)
            {
                case 0:
                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PasswordVault;
                    break;
                case 1:
                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet;
                    break;
                case 2:
                   
                    secureItemType = SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo;
                    break;
            }
            if (updateSent)
            {
                ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(secureItemType, true, false, pbData);
                SortSharedByMeData();
            }
            if (updateReceived)
            {
                ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(secureItemType, false, true, pbData);
                SortSharedWithMeData();
            }

        }
        /// <summary>
        /// personal info combobox selection changed event to change view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var comboBox = sender as ComboBox;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 1:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 2:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 3:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 4:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 5:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
                case 6:
                    
                    ShareCenterSentData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, true, false, pbData);

                    
                    ShareCenterReceivedData = ShareCenterDataHelper.SharedCenterData(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo, false, true, pbData);
                    break;
            }
            SortSharedByMeData();
            SortSharedWithMeData();
        }

        
    }
}
