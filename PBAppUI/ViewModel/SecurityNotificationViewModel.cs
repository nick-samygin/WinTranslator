using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.Model.SecurityNotification;
using PasswordBoss.Views;

namespace PasswordBoss.ViewModel
{
    internal class SecurityNotificationViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecurityNotificationViewModel));
        private const string AlertBorderStrokecolor = "AlertBorderStrokecolor";
        private const string HiglightedBorderStrokecolor = "HiglightedBorderStrokecolor";
        private IResolver resolver;
        private IPBData db;

        private List<AlertMessage> messages;

        #region Relay commands
        public RelayCommand SecurityNotificationCommand { get; set; }
        public RelayCommand NotificationItemClickCommand { get; set; }
        public RelayCommand ViewAlertHistoryCheckedCommand { get; set; }
        public RelayCommand ViewAlertHistoryUncheckedCommand { get; set; }
        public RelayCommand AlertCloseCommand { get; set; }

        #endregion

        public SecurityNotificationViewModel()
        {
            InitializeCommands();
            if(Db != null) BindingSecurityNotificationData(HistoryVisibility);
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
            SecurityNotificationCommand = new RelayCommand(SecurityNotificationClick);
            NotificationItemClickCommand = new RelayCommand(NotificationItemClick);
            ViewAlertHistoryCheckedCommand = new RelayCommand(ViewAlertHistoryChecked);
            ViewAlertHistoryUncheckedCommand = new RelayCommand(ViewAlertHistoryUnchecked);
            AlertCloseCommand = new RelayCommand(AlertCloseClick);
        }

        #region properties

        private Brush _securityAlertBackColor;
        public Brush SecurityAlertBackColor
        {
            get { return _securityAlertBackColor; }
            set
            {
                if (Equals(_securityAlertBackColor, value)) return;
                _securityAlertBackColor = value;
                RaisePropertyChanged("SecurityAlertBackColor");
            }
        }

        private Brush _securityAlertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(AlertBorderStrokecolor);
        public Brush SecurityAlertBorderStrokeColor
        {
            get { return _securityAlertBorderStrokeColor; }
            set
            {
                if (Equals(_securityAlertBorderStrokeColor, value)) return;
                _securityAlertBorderStrokeColor = value;
                RaisePropertyChanged("SecurityAlertBorderStrokeColor");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        List<Notification> _notification;
        public List<Notification> Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                if (_notification.Count > 0)
                {
                    HasNotificationItems = true;
                }
                else
                {
                    HasNotificationItems = false;
                }
                RaisePropertyChanged("Notification");
            }
        }

        private bool _hasNotificationItems;
        public bool HasNotificationItems
        {
            get { return _hasNotificationItems; }
            set
            {
                _hasNotificationItems = value;
                HasNotificationItemsNegated = !value;
                RaisePropertyChanged("HasNotificationItems");
            }
        }

        private bool _hasNotificationItemsNegated;
        public bool HasNotificationItemsNegated
        {
            get { return _hasNotificationItemsNegated; }
            set
            {
                _hasNotificationItemsNegated = value;
                RaisePropertyChanged("HasNotificationItemsNegated");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        List<Notification> _notificationHistory;
        public List<Notification> NotificationHistory
        {
            get { return _notificationHistory; }
            set
            {
                _notificationHistory = value;
                RaisePropertyChanged("NotificationHistory");
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

        #endregion
        private List<AlertMessage> NotSeen
        {
            get
            {
                return messages.Where(n => !n.MessageSeenDate.HasValue ).ToList();
            }
        }

        public Visibility MessageCountVisibility { get { return NotSeen.Count() == 0 ? Visibility.Collapsed : Visibility.Visible; } }
        public string MessageCountString
        {
            get
            {
                RaisePropertyChanged("MessageCountVisibility");
                return NotSeen.Count().ToString();
            }
        }

        #region other methods

        /// <summary>
        /// used to return security notification  list
        /// </summary>
        internal List<Notification> BindingSecurityNotificationData(bool HistoryVisibility)
        {
            List<Notification> items = new List<Notification>();
            var q = from m in Db.GetAlertMessages()
                    select m;
            if(_showHistoryVisibility)
            {
                q = from m in q
                    where !m.MessageSeenDate.HasValue
                    select m;
            }
            q = from m in q
                orderby m.MessageSeenDate ascending, m.PublishedDate
                select m;
            messages = q.ToList();
            foreach(var i in messages) items.Add(new Notification(i));

            
            //if (!HistoryVisibility)
            //{
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST", message = "TEST MSG", icon_type = DefaultProperties.SecurityAlertIcon,  message_seen_date = DateTime.Now, uuid = "ada"}));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST", message = "TEST MSG", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST", message = "TEST MSG", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST", message = "TEST MSG", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST", message = "TEST MSG", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //}
            //else
            //{
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST HIST", message = "TEST MSG HIST", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST HIST", message = "TEST MSG HIST", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST HIST", message = "TEST MSG HIST", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST HIST", message = "TEST MSG HIST", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //    items.Add(new Notification(new AlertMessage() { headline = "TEST HIST", message = "TEST MSG HIST", icon_type = DefaultProperties.SecurityAlertIcon, message_seen_date = DateTime.Now, uuid = "ada" }));
            //}

            if (HistoryVisibility)
            {
                items.ForEach(x => x.IsHistory = true);
            }
            return items;
        }

        #endregion
        Popup securityAlertPop;
        /// <summary>
        /// used to enable the alert securtiy popup and assign close and Open events 
        /// </summary>
        /// <param name="obj"></param>
        private void SecurityNotificationClick(object obj)
        {
            IFeatureChecker featureChecker = Resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_Miscellaneous_ShowNotificationAndAlerts))
            {
                return;
            }
            securityAlertPop = obj as Popup;
            if (securityAlertPop != null)
            {
                securityAlertPop.IsOpen = true;
                SecurityAlertBackColor = DefaultProperties.AlertBackgroundcolor();
                SecurityAlertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(HiglightedBorderStrokecolor);
                securityAlertPop.Closed += SecurityAlertPop_Closed;
                Notification = BindingSecurityNotificationData(false);
                NotificationHistory = BindingSecurityNotificationData(true);
            }
        }

        /// <summary>
        /// apply focused color when popup is open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecurityAlertPop_Closed(object sender, EventArgs e)
        {
            SecurityAlertBackColor = DefaultProperties.TransparentColor();
            SecurityAlertBorderStrokeColor = DefaultProperties.AlertBorderStrokecolor(AlertBorderStrokecolor);
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
            Notification = BindingSecurityNotificationData(false);
            NotificationHistory = BindingSecurityNotificationData(true);
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
            Notification = BindingSecurityNotificationData(false);
            NotificationHistory = BindingSecurityNotificationData(true);
        }

        /// <summary>
        /// close button event of Alert popup
        /// </summary>
        /// <param name="obj"></param>
        private void AlertCloseClick(object obj)
        {
            if(Db.AlertMessageSeen(((Notification)obj).AlertMessage))
            {
                Notification = BindingSecurityNotificationData(false);
                NotificationHistory = BindingSecurityNotificationData(true);
                RaisePropertyChanged("MessageCountString");
            }
            else
            {
                logger.Debug("AlertCloseClick failed");
            }
            //if(securityAlertPop != null) securityAlertPop.IsOpen = false;
        }
        public void UpdateAlertMessagesCount()
        {
            Notification = BindingSecurityNotificationData(false);
            NotificationHistory = BindingSecurityNotificationData(true);
            RaisePropertyChanged("MessageCountString");
        }

        private void NotificationItemClick(object obj)
        {
            if(obj ==null ) return;
            ((MainWindow)Application.Current.MainWindow).MenuSetFocus();
            Notification msg = (Notification)obj;
            var dictionary = new Dictionary<string, object> { { "url", msg.AlertMessage.Url } };
            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("OpenUrlInSecureBrowser", dictionary);
            if(securityAlertPop != null) securityAlertPop.IsOpen = false;
        }
    }
}
