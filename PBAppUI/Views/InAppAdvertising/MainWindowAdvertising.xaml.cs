using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace PasswordBoss.Views.InAppAdvertising
{
    /// <summary>
    /// Interaction logic for MainWindowAdvertising.xaml
    /// </summary>
    public partial class MainWindowAdvertising : UserControl
    {
        IPBData pbData = null;
        IPBSync sync = null;
        IResolver resolver;

        private string lastMenuitemClicked;

        public MainWindowAdvertising(IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
            pbData = resolver.GetInstanceOf<IPBData>();

            sync = resolver.GetInstanceOf<IPBSync>();
            if (sync != null) sync.OnSyncFinished += sync_OnSyncSuccess;

            this.Loaded += MainWindowAdvertising_Loaded;

           
            SetChildren();
        }

        void MainWindowAdvertising_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            mainWindow.OnMenuItemSelected += mainWindow_OnMenuItemSelected;

        }

        void mainWindow_OnMenuItemSelected(string obj)
        {
            lastMenuitemClicked = obj;
            SetChildren();
        }

        void sync_OnSyncSuccess(bool status)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                SetChildren();
            });  
        }


        private void SetChildren()
        {
            container.Children.Clear();

            var info = pbData.GetSubscriptionInfo();

            if (lastMenuitemClicked != "SecureBrowser" && PremiumExpiring.ShowUpgradePanel(info))
            {
                var control = new PremiumExpiring(info, false, resolver);
                container.Children.Add(control);
            }
        }
    }
}
