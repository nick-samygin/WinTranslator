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
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.DTO;

namespace PasswordBoss.Views.InAppAdvertising
{
    /// <summary>
    /// Interaction logic for LoginWindowAdvertising.xaml
    /// </summary>
    public partial class LoginWindowAdvertising : UserControl
    {
        IPBData pbData = null;
        IPBSync sync = null;
        IResolver resolver;

        private PremiumExpiring wind;

        public LoginWindowAdvertising(IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            sync = resolver.GetInstanceOf<IPBSync>();
            if (sync != null) sync.OnSyncFinished += sync_OnSyncSuccess;
            if (pbData != null )
            {
                pbData.OnUserLoggedIn += pbData_OnUserLoggedIn;
            }

            SetChildren();
        }

        void pbData_OnUserLoggedIn(string obj)
        {
            if(wind != null)
            {
                if(!wind.UpgradeToPremiumClicked)
                {
                    // PBD-1442 -> Don't log this event if user subscription is premium
                    var userSubscriptionInfo = pbData.GetSubscriptionInfo();
                    if (userSubscriptionInfo.SubscriptionType == SubscriptionType.Free || userSubscriptionInfo.SubscriptionType == SubscriptionType.Trial)
                        wind.InAppMarketingLoginLog(MarketingActionType.Close);
                }

                wind.UpgradeToPremiumClicked = false;
            }
        }

        void sync_OnSyncSuccess(bool status)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                SetChildren();
            });
        }

        private void SetChildren()
        {
            container.Children.Clear();

            var info = pbData.GetSubscriptionInfo();
            if (PremiumExpiring.ShowUpgradePanel(info))
            {
                var border = new Border() { Margin = new Thickness(0, 1, 0, 1), BorderThickness = new Thickness(0, 0.5, 0, 0), BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#565C5E")) };
                wind = new PremiumExpiring(info, true, resolver) { Margin = new Thickness(9, 0, 9, 0) };
                border.Child = wind;
                container.Children.Add(border);
            }
        }
    }
}
