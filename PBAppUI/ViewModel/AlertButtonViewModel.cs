using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Model.AlertButton;
using PasswordBoss.Views;
using PasswordBoss.Views.UserControls;

namespace PasswordBoss.ViewModel
{
    public class AlertButtonViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(AlertButtonViewModel));
        private const string HiglightedBorderStrokecolor = "HiglightedBorderStrokecolor";
        private const string AlertBorderStrokecolor = "AlertBorderStrokecolor";
        private IResolver resolver;
        private IPBData db;

        private List<AlertNotification> notifications;

        #region Relay commands
        public RelayCommand AlertNotificationCommand { get; set; }
        public RelayCommand ViewAlertHistoryCheckedCommand { get; set; }
        public RelayCommand ViewAlertHistoryUncheckedCommand { get; set; }
        public RelayCommand AlertCloseCommand { get; set; }
        public RelayCommand AlertItemClickCommand { get; set; }
        public RelayCommand UpdatePasswordContinueCommand { get; set; }
        

        #endregion

        public AlertButtonViewModel()
        {
            InitializeCommands();
            if (Db != null)
            {
                Alert = BindingAlertData(false);
                AlertHistory = BindingAlertData(true);
            }

            var setting = db.GetUserSetting(DefaultProperties.Configuration_Key_SecurityScoreInfo);
            if (!string.IsNullOrWhiteSpace(setting))
            {
                bool ret;
                if (bool.TryParse(setting, out ret))
                {
                    ShowUpdatePasswordInfo = !ret;
                }
            }
        }

        private IResolver Resolver
        {
            get
            {
                if(resolver == null)
                {
                    try
                    {
                        resolver = ((PBApp)Application.Current).GetResolver();
                    }
                    catch
                    {
                    }
                }
                return resolver;
            }
        }

        private IPBData Db
        {
            get
            {
                if(db == null)
                {
                    if(Resolver != null) db = Resolver.GetInstanceOf<IPBData>();
                }
                return db;
            }
        }

        public void InitializeCommands()
        {
            AlertNotificationCommand = new RelayCommand(AlertNotificationClick);
            AlertItemClickCommand = new RelayCommand(AlertItemClick);
            ViewAlertHistoryCheckedCommand = new RelayCommand(ViewAlertHistoryChecked);
            ViewAlertHistoryUncheckedCommand = new RelayCommand(ViewAlertHistoryUnchecked);
            AlertCloseCommand = new RelayCommand(AlertCloseClick);
            UpdatePasswordContinueCommand = new RelayCommand(UpdatePasswordContinueClick);
        }

        #region properties
        private Brush _alertButtonBackColor;
        public Brush AlertButtonBackColor
        {
            get { return _alertButtonBackColor; }
            set
            {
                if (Equals(_alertButtonBackColor, value)) return;
                _alertButtonBackColor = value;
                RaisePropertyChanged("AlertButtonBackColor");
            }
        }

        private Brush _alertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(AlertBorderStrokecolor);
        public Brush AlertBorderStrokeColor
        {
            get { return _alertBorderStrokeColor; }
            set
            {
                if (Equals(_alertBorderStrokeColor, value)) return;
                _alertBorderStrokeColor = value;
                RaisePropertyChanged("AlertBorderStrokeColor");
            }
        }

        private bool _historyVisibility = false;
        public bool HistoryVisibility
        {
            get { return _historyVisibility; }
            set
            {
                _historyVisibility = value;
                RaisePropertyChanged("HistoryVisibility");
            }
        }

        private bool _showHistoryVisibility = true;
        public bool ShowHistoryVisibility
        {
            get { return _showHistoryVisibility; }
            set
            {
                _showHistoryVisibility = value;
                RaisePropertyChanged("ShowHistoryVisibility");
            }
        }
        private bool _hideHistoryVisibility = false;
        public bool HideHistoryVisibility
        {
            get { return _hideHistoryVisibility; }
            set
            {
                _hideHistoryVisibility = value;
                RaisePropertyChanged("HideHistoryVisibility");
            }
        }

        public bool NotificationHistoryVisibility
        {
            get { return _historyVisibility; }
            set
            {
                _historyVisibility = value;
                RaisePropertyChanged("NotificationHistoryVisibility");
            }
        }

        private bool _hasAlertItems;
        public bool HasAlertItems
        {
            get { return _hasAlertItems; }
            set
            {
                _hasAlertItems = value;
                HasAlertItemsNegated = !value;
                RaisePropertyChanged("HasAlertItems");
            }
        }

        private bool _hasAlertItemsNegated;
        public bool HasAlertItemsNegated
        {
            get { return _hasAlertItemsNegated; }
            set
            {
                _hasAlertItemsNegated = value;
                RaisePropertyChanged("HasAlertItemsNegated");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        List<Alert> _alert;
        public List<Alert> Alert
        {
            get { return _alert; }
            set
            {
                _alert = value;
                if(_alert.Count > 0)
                {
                    HasAlertItems = true;
                }
                else
                {
                    HasAlertItems = false;
                }
                RaisePropertyChanged("Alert");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        List<Alert> _alertHistory;
        public List<Alert> AlertHistory
        {
            get { return _alertHistory; }
            set
            {
                _alertHistory = value;
                RaisePropertyChanged("AlertHistory");
            }
        }

        public Visibility AlertCountVisibility { get { return NotSeenCount == 0 ? Visibility.Collapsed : Visibility.Visible; } }
        public string AlertCountString
        {
            get
            {
                RaisePropertyChanged("AlertCountVisibility");
                return NotSeenCount.ToString();
            }
        }

        private int NotSeenCount
        {
            get
            {
                var q = notifications.Where(n => !n.last_alert.HasValue);
                int cnt = q.Where(i => i.AlertType == AlertType.NewShare).Count();

                int sub = Db.GetAlertNotifications().Where(i => i.AlertType == AlertType.SecurityAlert && i.last_alert.HasValue && i.last_alert.Value.Date == DateTime.Now.Date).Count();
                int take = 2 - sub;
                cnt += q.Where(i => i.AlertType == AlertType.SecurityAlert).Take(take < 0 ? 0 : take).Count();

                return cnt;
            }
        }

        private string _itemUri;

        public string ItemUri
        {
            get { return _itemUri; }
            set
            {
                _itemUri = value;
                RaisePropertyChanged("ItemUri");
            }
        }

        private bool _neverShowChecked;

        public bool NeverShowChecked
        {
            get { return _neverShowChecked; }
            set
            {
                _neverShowChecked = value;
                RaisePropertyChanged("NeverShowChecked");
            }
        }

        private bool _showUpdatePasswordInfo = true;

        public bool ShowUpdatePasswordInfo
        {
            get { return _showUpdatePasswordInfo; }
            set
            {
                _showUpdatePasswordInfo = value;
                RaisePropertyChanged("ShowUpdatePasswordInfo");
            }
        }

        #endregion

        #region other methods
        /// <summary>
        /// used to return Alert list
        /// </summary>
        /// <returns></returns>
        internal List<Alert> BindingAlertData(bool HistoryVisibility)
        {
            List<Alert> items = new List<Alert>();
            var q = from r in Db.GetAlertNotifications()
                    select r;
            var shr = from r in q
                      where r.AlertType == AlertType.NewShare
                      orderby r.last_alert, r.last_modified_date
                      select r;
            if(!HistoryVisibility)
            {
                shr = from r in shr
                      where !r.last_alert.HasValue
                      orderby r.last_alert, r.last_modified_date
                      select r;
            }

            var shrItems = shr.Select(p => new Alert(p));
            items.AddRange(shrItems);

            notifications = shr.ToList();

            q = from r in q
                where r.AlertType == AlertType.SecurityAlert
                orderby r.last_alert, r.last_modified_date
                select r;
            if (!HistoryVisibility)
            {
                q = from r in q
                    where !r.last_alert.HasValue
                    select r;
            }
            notifications.AddRange(q);

            var cnt = NotSeenCount - shr.Count(p => !p.last_alert.HasValue);

            foreach (var i in q)
            {
                if(!i.last_alert.HasValue && cnt > 0)
                {
                    items.Add(new Alert(i));
                    cnt--;
                }
                
                if(i.last_alert.HasValue)
                {
                    items.Add(new Alert(i));
                }
            }

            items = items.OrderBy(p => p.AlertNotification.last_alert).ThenBy(p => p.AlertNotification.last_modified_date).ToList();

            if(HistoryVisibility)
            {
                items.ForEach(x => x.IsHistory = true);
            }
            //var q = notifications.Where(n => !n.last_alert.HasValue);
            //int cnt = q.Where(i => i.AlertType == AlertType.NewShare).Count();
            //int tmp = q.Where(i => i.AlertType == AlertType.SecurityAlert).Count();
            //cnt += tmp > 2 ? 2 : tmp;
            //int sub = Db.GetAlertNotifications().Where(i => i.AlertType == AlertType.SecurityAlert && i.last_alert.HasValue && i.last_alert.Value.Date == DateTime.Now.Date).Count();
            //cnt -= sub > 2 ? 2 : sub;
            //return cnt < 0 ? 0 : cnt;

            //if (!HistoryVisibility)
            //{

            //    items.Add(new Alert() { siteName = "Amazon", status = "Password 30 weeks old", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Amazon), sent = "2 days", uuid = "1", visible = false });
            //    items.Add(new Alert() { siteName = "Dropbox", email = "becky@gmail.com", status = "accepted your invitation", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Dropbox), sent = "1 days", uuid = "2", visible = true });
            //    items.Add(new Alert() { siteName = "Hulu", status = "Duplicate passwords", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Hulu), sent = "3 days", uuid = "3", visible = false });
            //    items.Add(new Alert() { siteName = "Paypal", status = "Password 30 weeks old", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Amazon), sent = "2 days", uuid = "1", visible = false });

            //}
            //else
            //{
            //    items.Add(new Alert() { siteName = "Amazon", status = "Password 10 weeks old", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Amazon), sent = "2 days", uuid = "1", visible = false });
            //    items.Add(new Alert() { siteName = "Dropbox", email = "becky@gmail.com", status = "accepted your invitation", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Dropbox), sent = "1 days", uuid = "2", visible = true });
            //    items.Add(new Alert() { siteName = "Hulu", status = "Duplicate passwords", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Hulu), sent = "3 days", uuid = "3", visible = false });
            //    items.Add(new Alert() { siteName = "Amazon", status = "Password 10 weeks old", alertImage = DefaultProperties.ReturnImage(DefaultProperties.Amazon), sent = "2 days", uuid = "1", visible = false });

            //}
            return items;
        }

        #endregion

        public void UpdateAlertNotificationCount()
        {
            Alert = BindingAlertData(false);
            AlertHistory = BindingAlertData(true);
            RaisePropertyChanged("AlertCountString");
        }

        private Popup alertPop;

        /// <summary>
        /// used to enable the alert popup and assign close and Open events 
        /// </summary>
        /// <param name="obj"></param>
        private void AlertNotificationClick(object obj)
        {
            IFeatureChecker featureChecker = Resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_ShowNotificationAndAlerts))
            {
                return;
            }
            alertPop = obj as Popup;
            if (alertPop != null)
            {
                alertPop.IsOpen = true;
                AlertButtonBackColor = DefaultProperties.AlertBackgroundcolor();
                AlertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(HiglightedBorderStrokecolor);
                alertPop.Closed += alertPop_Closed;
                Alert = BindingAlertData(false);
                AlertHistory = BindingAlertData(true);
            }
        }

        /// <summary>
        /// apply transparent color when alert popup is open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void alertPop_Closed(object sender, EventArgs e)
        {
            AlertButtonBackColor = DefaultProperties.TransparentColor();
            AlertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(AlertBorderStrokecolor);
        }

        /// <summary>
        /// used to disable histry grid of alert & security norifiacation 
        /// </summary>
        /// <param name="obj"></param>
        private void ViewAlertHistoryChecked(object obj)
        {
            HideHistoryVisibility = true;
            ShowHistoryVisibility = false;
            HistoryVisibility = true;
            NotificationHistoryVisibility = true;
            Alert = BindingAlertData(false);
            AlertHistory = BindingAlertData(true);
        }

        /// <summary>
        /// used to enable histry grid of alert & security norifiacation 
        /// </summary>
        /// <param name="obj"></param>
        private void ViewAlertHistoryUnchecked(object obj)
        {
            HideHistoryVisibility = false;
            ShowHistoryVisibility = true;
            HistoryVisibility = false;
            NotificationHistoryVisibility = false;
            Alert = BindingAlertData(false);
            AlertHistory = BindingAlertData(true);
        }

        /// <summary>
        /// close button event of Alert popup
        /// </summary>
        /// <param name="obj"></param>
        private void AlertCloseClick(object obj)
        {
            if(obj == null) return;
            var a = ((Alert)obj).AlertNotification;
            if(a.AlertType == AlertType.SecurityAlert || a.AlertType == AlertType.NewShare)
            {
                if(Db.AlertNotificationSeen(a))
                {
                    Alert = BindingAlertData(false);
                    AlertHistory = BindingAlertData(true);
                    RaisePropertyChanged("AlertCountString");
                }
                else
                {
                    logger.Debug("AlertCloseClick failed");
                }
            }
            if(alertPop != null) alertPop.IsOpen = false;
        }

        private void AlertItemClick(object obj)
        {
            if(obj == null) return;
            var a = ((Alert)obj).AlertNotification;

            if(!string.IsNullOrWhiteSpace(a.secure_item_id))
            {
                var _secureItem = db.GetSecureItemById(a.secure_item_id);
                if(_secureItem != null && _secureItem.Site != null)
                {
                    ItemUri = _secureItem.LoginUrl;
                }
                
            }
            
            if(a.AlertType == AlertType.SecurityAlert)
            {
                // Open info screen
                if (ShowUpdatePasswordInfo)
                {
                    UpdatePasswordsInfoDialog dlg = new UpdatePasswordsInfoDialog(Application.Current.MainWindow);
                    dlg.DataContext = this;
                    dlg.ShowDialog();

                    
                    if (NeverShowChecked)
                    {
                        Configuration configDontShowInfoDialog = new Configuration()
                        {
                            AccountEmail = db.ActiveUser,
                            Key = DefaultProperties.Configuration_Key_SecurityScoreInfo,
                            Value = true.ToString()
                        };
                        db.AddOrUpdateConfiguration(configDontShowInfoDialog);
                    }

                    ShowUpdatePasswordInfo = !NeverShowChecked;
                }

                // Only update password from browser
                else
                {
                    OpenSiteToUpdatePassword();
                }

                //var dictionary = new Dictionary<string, object> { { "id", a.secure_item_id } };
                //((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowSecureItemEditor", dictionary);
            }
            else if(a.AlertType == AlertType.NewShare)
            {
                if (((Alert)obj).shareStatus == ShareStatus.Pending)
                {
                    ((MainWindow)Application.Current.MainWindow).MenuSetFocus();
                    var dictionary = new Dictionary<string, object> { { "id", ((Alert)obj).uuid } };
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowShareItemInShareCenter", dictionary);
                }
            }
            if(alertPop != null) alertPop.IsOpen = false;
        }

        private void UpdatePasswordContinueClick(object obj)
        {
            OpenSiteToUpdatePassword();
        }

        private void OpenSiteToUpdatePassword()
        {
            if (ItemUri != null && ItemUri != string.Empty)
            {
                BrowserHelper.OpenInDefaultBrowser(new Uri(ItemUri, UriKind.RelativeOrAbsolute));
            }
            ItemUri = string.Empty;
        }
    }
}
