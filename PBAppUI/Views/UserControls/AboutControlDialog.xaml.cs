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

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AboutControlDialog.xaml
    /// </summary>
    public partial class AboutControlDialog : Window
    {
        IResolver resolver = null;
        IPBData pbData = null;

        public AboutControlDialog(Window owner, IResolver resolver)
        {
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();
            this.Owner = owner;
            this.Height = owner.ActualHeight;
            this.Width = owner.ActualWidth;
            this.Left = owner.Left;
            this.Top = owner.Top;
            this.WindowStartupLocation = owner.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var info = pbData.GetSubscriptionInfo();
            runVersion.Text = DeviceHelper.GetProductVerson();

            if(info != null)
            {
                runLicense.Text = info.LocalizedSubscriptionType;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void supportCenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppSupportMenuLink));
        }

        private void privacyPolicy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppSupportPrivacyPolicyMenuLink));
        }

        private void termsAndConditions_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BrowserHelper.OpenInDefaultBrowser(new Uri(DefaultProperties.InAppTermsAndConditionsMenuLink));
        }
    }
}
