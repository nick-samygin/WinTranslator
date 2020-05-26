using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using PasswordBoss.Views;
using PasswordBoss.ViewModel;
using PasswordBoss.Helpers;
using PasswordBoss.DTO;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Views.UserControls;

namespace PasswordBoss
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class ShareCenter : IUIComponent
    {
        private ShareCenterMenuButton btnShareCenter = null;
        private NewShareCenterContentPanel content = null;
        private ShareCenterMiniTour miniTourWindow = null;
        private IResolver resolver;
        private IPBData pbData;
        private NewShareCenterViewModel _viewModel;


        [ImportingConstructor]
        public ShareCenter([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/ShareCenter;component/resources/dictionary/ShareCenterResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "ShareCenter"; }
        }

        public float MenuRank
        {
            get { return 6f; }
        }

        public Control MenuButton
        {
            get
            {
                if (btnShareCenter == null)
                {
                    btnShareCenter = new ShareCenterMenuButton();
                    btnShareCenter.Click += btnShareCenter_Click;
                }
                return btnShareCenter;
            }
        }

        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {

                return new List<AddSecureSubItem>()
                    {
                            new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login){
                            ItemTitel = Application.Current.Resources["ItemWebsite"] as string,
                            CreateItemType=typeof(WebsiteSecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_Application)
                        {
                            ItemTitel = Application.Current.Resources["ItemApp"] as string,
                            CreateItemType=typeof(AppSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_Database)
                         {
                            ItemTitel = Application.Current.Resources["ItemDatabase"] as string,
                            CreateItemType=typeof(DatabaseSecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_EmailAccount)
                        {
                            ItemTitel = Application.Current.Resources["ItemEmailAccount"] as string,
                            CreateItemType=typeof(EmailAccountSecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_InstantMessenger)
                         {
                            ItemTitel = Application.Current.Resources["ItemInstantMessenger"] as string,
                            CreateItemType=typeof(InstantMessengerSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_Server)
                         {
                            ItemTitel = Application.Current.Resources["ItemServer"] as string,
                            CreateItemType=typeof(ServerSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_SSHKey)
                         {
                            ItemTitel = Application.Current.Resources["ItemSSHKeys"] as string,
                             CreateItemType=typeof(SSHKeySecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault,SecurityItemsDefaultProperties.SecurityItemSubType_PV_WiFi)
                         {
                            ItemTitel = Application.Current.Resources["ItemWiFi"] as string,
                            CreateItemType=typeof(WifiSecureItemViewModel),
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = string.Empty,
                            ItemTitel = Application.Current.Resources["AddItemDifferent"] as string
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet,SecurityItemsDefaultProperties.SecurityItemSubType_DW_Bank)
                        {
                            ItemTitel = Application.Current.Resources["ItemBankAccount"] as string,
                            CreateItemType=typeof(BankAccountItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet,SecurityItemsDefaultProperties.SecurityItemSubType_DW_CreditCard)
                        {
                            ItemTitel = Application.Current.Resources["ItemCreditCard"] as string,
                            CreateItemType=typeof(CreditCardItemViewModel),
                        },
                        new AddSecureSubItem()
                        {
                            ItemType =string.Empty,
                            ItemTitel = Application.Current.Resources["AddItemDifferent"] as string
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo,SecurityItemsDefaultProperties.SecurityItemSubType_PI_Address)
                        {
                            ItemTitel = Application.Current.Resources["ItemAddress"] as string,
                            CreateItemType=typeof(AddressSecureItemViewModel),
                        },
                       new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo,SecurityItemsDefaultProperties.SecurityItemSubType_PI_Company)
                        {
                            ItemTitel = Application.Current.Resources["ItemCompany"] as string,
                            CreateItemType=typeof(CompanySecureItemViewModel)
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo,SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email)
                        {
                            ItemTitel = Application.Current.Resources["ItemEmail"] as string,
                            CreateItemType=typeof(EmailSecureItemViewModel)
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo,SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names)
                        {
                            ItemType =  SecurityItemsDefaultProperties.SecurityItemSubType_PI_Names,
                            ItemTitel = Application.Current.Resources["ItemName"] as string,
                            CreateItemType=typeof(NameSecureItemViewModel)
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo,SecurityItemsDefaultProperties.SecurityItemSubType_PI_PhoneNumber)
                        {
                            ItemTitel = Application.Current.Resources["ItemPhone"] as string,
                            CreateItemType=typeof(PhoneSecureItemViewModel)
                        },
                        new AddSecureSubItem()
                        {
                            ItemType = string.Empty,
                            ItemTitel = Application.Current.Resources["AddItemDifferent"] as string
                        }

                };
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnShareCenter_Click(object arg1, System.Windows.RoutedEventArgs arg2)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_ShareCenter_ManageShares))
            {
                Selected = false;
                return;
            }
            btnShareCenter.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (content == null)
                {
                    content = new NewShareCenterContentPanel
                    {
                        DataContext = ViewModel
                    };
                }
                return content;
            }
        }

        public string Header { get { return Application.Current.FindResource("NavShareCenter") as string; } }


        public bool Visible
        {
            get
            {
                return true;
            }
            set
            {
                ;
            }
        }

        public bool Selected
        {
            get
            {
                return btnShareCenter.Selected;
            }
            set
            {
                btnShareCenter.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);

                // TO - DO
                /*
                try
                {
                    string miniTourValue = pbData.GetConfigurationValueByAccountAndKey(pbData.ActiveUser, "ShowShareCenterMiniTour");
                    if (!bool.TryParse(miniTourValue, out showMiniTour)) ShowMiniTour = true;
                    if (value && ShowMiniTour)
                    {
                        if (((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreDialogsOpened", null) == false)
                        {
                            miniTourWindow = new ShareCenterMiniTour(resolver);
                            miniTourWindow.Owner = Application.Current.MainWindow;
                            miniTourWindow.Width = miniTourWindow.Owner.ActualWidth;
                            miniTourWindow.Height = miniTourWindow.Owner.ActualHeight;
                            ShowMiniTour = false;
                            miniTourWindow.ShowDialog();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } */
            }
        }

        private bool showMiniTour;
        public bool ShowMiniTour
        {
            get
            {
                return showMiniTour;
            }
            set
            {
                showMiniTour = value;
            }
        }

        public AddItem ComponentTree
        {
            get
            {
                return new AddItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["ItemSharedItem"] as string,
                    Icon = Application.Current.Resources["addSharedItem"] as ImageSource
                };
            }
        }

        public object ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new NewShareCenterViewModel(resolver, SubItemsComponentTree);
                
                return _viewModel;
            }
        }

        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            switch (command)
            {
                case "ShowShareItemList":
                {
                    if (parameters != null && parameters.ContainsKey("item") && parameters["item"] != null &&
                        parameters["item"].GetType() == typeof (Share))
                    {
                        var m = (ShareCenterViewModel) ContentPanel.DataContext;
                        if (m == null) return false;
                        m.DisplaySharesForItem((Share) parameters["item"]);
                        return true;
                    }
                    return false;
                }
                case "AddItem":
                {
                    var addShareWindow = new AddShareWindow();
                    addShareWindow.DataContext = new AddShareViewModel(resolver, SubItemsComponentTree);
                    addShareWindow.ShowDialog();
                    return true;
                }
                default:
                    return false;
            }
        }
    }
}
