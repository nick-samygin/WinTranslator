using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordBoss.DTO;
using PasswordBoss.PBAnalytics;

namespace PasswordBoss.Views.InAppAdvertising
{
    /// <summary>
    /// Interaction logic for PremiumExpiring.xaml
    /// </summary>
    public partial class PremiumExpiring : UserControl
    {
        private bool _isLoginScreen = false;
        private int _daysFromAccountCreation = 0;
        IResolver resolver;
        private IPBData pbData = null;
        private IInAppAnalytics inAppAnalyitics = null;

        public bool UpgradeToPremiumClicked { get; set; }

        public PremiumExpiring(SubscriptionInfo info, bool isLoginScreen, IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            this.inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();

            _isLoginScreen = isLoginScreen;

            int daysRemaining = GetDaysRemaining(info);
            _daysFromAccountCreation = GetDaysFromAccountCreation(info);

            if (isLoginScreen)
            {
                PremiumExpiringLoginWindow(daysRemaining);
            }
            else
            {
                PremiumExpiringMainWindow(daysRemaining);
            }

        }

        private void PremiumExpiringMainWindow(int daysRemaining)
        {
            var brush = GetTextColourByExpirationDays(daysRemaining);

            tbSubHeader2.Foreground = brush;
            progresBar.Foreground = brush;
            progresBar.BorderBrush = brush;
            progresBar.Value = daysRemaining;
            tbHeader.Foreground = brush;
            tbHeader.Foreground = Brushes.White;

            if (daysRemaining > 0)
            {
                tbHeader.Inlines.Add(System.Windows.Application.Current.FindResource("PremiumExpiresHeaderMainScreen").ToString());
                Run run = new Run(string.Format(" {0} {1}.", daysRemaining.ToString(), daysRemaining == 1 ? System.Windows.Application.Current.FindResource("Day") : System.Windows.Application.Current.FindResource("Days")));
                run.Foreground = brush;
                tbHeader.Inlines.Add(run);
            }
            else
            {
                tbHeader.Inlines.Add(System.Windows.Application.Current.FindResource("PremiumExpiredHeaderMainScreen").ToString());
                Run run = new Run(string.Format(" {0}", System.Windows.Application.Current.FindResource("PremiumExpiredHeaderMainScreenExpired").ToString()));
                run.Foreground = brush;
                tbHeader.Inlines.Add(run);
            }

            this.Margin = new Thickness(-20, 0, 95, 0);
        }

        private void PremiumExpiringLoginWindow(int daysRemaining)
        {
            tbSubHeader.Text = System.Windows.Application.Current.FindResource("PremiumExpiresSubHeaderLoginScreen").ToString();
            tbSubHeader2.Text = string.Format("{0} {1}.", daysRemaining.ToString(), daysRemaining == 1 ? System.Windows.Application.Current.FindResource("Day") : System.Windows.Application.Current.FindResource("Days"));

            var brush = GetTextColourByExpirationDays(daysRemaining);

            tbSubHeader2.Foreground = brush;
            progresBar.Foreground = brush;
            progresBar.BorderBrush = brush;
            progresBar.Value = daysRemaining;
            tbHeader.Foreground = brush;

            if (daysRemaining > 0)
            {
                tbHeader.Text = System.Windows.Application.Current.FindResource("PremiumExpiresHeaderLoginScreen").ToString();
            }
            else
            {
                tbHeader.Text = System.Windows.Application.Current.FindResource("PremiumExpiredHeaderLoginScreen").ToString();
                tbSubHeader.Text = string.Empty;
                tbSubHeader2.Text = string.Empty;
            }


            Grid.SetRow(btnConfirmAction, 1);
            Grid.SetColumn(btnConfirmAction, 0);
            Grid.SetColumnSpan(spText, 2);
            Grid.SetColumnSpan(btnConfirmAction, 3);
            mainGrid.Height = 199;
            btnConfirmAction.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            btnConfirmAction.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            btnConfirmAction.Margin = new Thickness(0, 0, 0, 20);
            btnConfirmAction.Height = 32;
            btnConfirmAction.Width = 172;
            progresBar.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            btnConfirmAction.Content = System.Windows.Application.Current.FindResource("UpgradeNow").ToString();
            wrapPanelSubHeader.Visibility = System.Windows.Visibility.Visible;
        }

        private SolidColorBrush GetTextColourByExpirationDays(int daysRemaining)
        {
            SolidColorBrush brush = Brushes.Orange;

            if (daysRemaining > 20)
            {
                brush = new SolidColorBrush(Color.FromRgb(243, 199, 66));
            }
            else if (daysRemaining > 10)
            {
                brush = new SolidColorBrush(Color.FromRgb(247, 136, 78));
            }
            else if (daysRemaining > 0)
            {
                brush = new SolidColorBrush(Color.FromRgb(247, 110, 81));
            }
            else
            {
                brush = new SolidColorBrush(Color.FromRgb(247, 110, 81));
            }

            return brush;
        }

        private void btnConfirmAction_Click(object sender, RoutedEventArgs e)
        {
            UpgradeToPremiumClicked = true;
            if(inAppAnalyitics != null)
            {
                if(_isLoginScreen)
                {
                    //var analytics = inAppAnalyitics.Get<Events.LoginPanelClick, int>();
                    //analytics.Log(_daysFromAccountCreation);

                    //var analytics2 = inAppAnalyitics.Get<Events.InAppMessageContinue, NonNullableString>();
                    //analytics2.Log(new NonNullableString("UP-L-0001"));

                    InAppMarketingLoginLog(MarketingActionType.Continue);
                }
                else
                {
                    //var analytics = inAppAnalyitics.Get<Events.DashboardPanelClick, int>();
                    //analytics.Log(_daysFromAccountCreation);

                    //var analytics2 = inAppAnalyitics.Get<Events.InAppMessageContinue, NonNullableString>();
                    //analytics2.Log(new NonNullableString("UP-M-0001"));

                    MessageHistory his = new MessageHistory
                    {
                        AnalyticsCode = "UP-M-0001",
                        MsgType = "dashboard panel",
                        Theme = "PB-0007",
                        DaysSinceAccountCreated = _daysFromAccountCreation,
                        ButtonClicked = MarketingActionType.Continue.ToString(),
                        BuyButton = BuyButtons.DashboardBottom.ToString()
                    };

                    pbData.InsertMessageHistory(his);
                    var mhItem = pbData.GetMessageHistoryById(his.Id);

                    var analytics2 = inAppAnalyitics.Get<Events.InAppMarketing, InAppMessageItem>();
                    var logItem = new InAppMessageItem(mhItem.RowId, mhItem.AnalyticsCode, mhItem.MsgType, mhItem.Theme, (MarketingActionType)Enum.Parse(typeof(MarketingActionType), mhItem.ButtonClicked), BuyButtons.DashboardBottom, mhItem.DaysSinceAccountCreated);
                    analytics2.Log(logItem);
                }
            }

            ((IAppCommand)System.Windows.Application.Current).ExecuteCommand(_isLoginScreen ? "BuyDB" : "BuySB", null);
        }

        public void InAppMarketingLoginLog(MarketingActionType action)
        {
            MessageHistory his = new MessageHistory
            {
                AnalyticsCode = "UP-L-0001",
                MsgType = "logon panel",
                Theme = "PB-0006",
                DaysSinceAccountCreated = _daysFromAccountCreation,
                ButtonClicked = action.ToString(),
                BuyButton = BuyButtons.LoginScreen.ToString()
            };

            pbData.InsertMessageHistory(his);
            var mhItem = pbData.GetMessageHistoryById(his.Id);

            var analytics2 = inAppAnalyitics.Get<Events.InAppMarketing, InAppMessageItem>();
            var logItem = new InAppMessageItem(mhItem.RowId, mhItem.AnalyticsCode, mhItem.MsgType, mhItem.Theme, (MarketingActionType)Enum.Parse(typeof(MarketingActionType), mhItem.ButtonClicked), BuyButtons.LoginScreen, mhItem.DaysSinceAccountCreated);
            analytics2.Log(logItem);
        }

        private static int GetDaysRemainingTotal(SubscriptionInfo info)
        {
            var currentDate = DateTime.Now;
            return (int)Math.Ceiling((info.ExpirationDate.GetValueOrDefault(currentDate) - currentDate).TotalDays);
            //return (info.ExpirationDate.GetValueOrDefault(currentDate) - currentDate).Days;
        }

        private static int GetDaysFromAccountCreation(SubscriptionInfo info)
        {
            var currentDate = DateTime.Now;
            //return (int)Math.Ceiling((currentDate - info.AccountCreationDate.GetValueOrDefault(currentDate)).TotalDays);
            return (currentDate - info.AccountCreationDate.GetValueOrDefault(currentDate)).Days;
        }

        private static int GetDaysRemaining(SubscriptionInfo info)
        {

            int daysRemaining = GetDaysRemainingTotal(info);
            daysRemaining = daysRemaining < 0 ? 0 : daysRemaining;
            //daysRemaining = daysRemaining > 30 ? 30 : daysRemaining;

            if (info.SubscriptionType == SubscriptionType.Free)
            {
                daysRemaining = 0;
            }

            return daysRemaining;
        }

        public static bool ShowUpgradePanel(SubscriptionInfo info)
        {
            if (info != null && (info.SubscriptionType == DTO.SubscriptionType.Free || info.SubscriptionType == DTO.SubscriptionType.Trial))
            {
                if(GetDaysRemaining(info) < 30)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
