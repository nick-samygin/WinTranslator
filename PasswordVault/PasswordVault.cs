using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using PasswordBoss.DTO;
using PasswordBoss.ViewModel;
using PasswordBoss.Views;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using PasswordBoss.Helpers;
using SecureItemsCommon;

namespace PasswordBoss
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class PasswordVault : IUIComponent
    {
        private PasswordVaultMenuButton btnPasswordVault = null;
        private NewPasswordVaultContentPanel content = null;

        private SecureItemsHolderViewModel viewModel = null;

        private IResolver resolver;

        [ImportingConstructor]
        public PasswordVault([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
        }
               
        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/PasswordVault;component/resources/dictionary/PasswordVaultResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "PasswordVault"; }
        }

        public float MenuRank
        {
            get { return 1f; }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get {
                if(btnPasswordVault == null)
                {
                    btnPasswordVault = new PasswordVaultMenuButton();
                    btnPasswordVault.DataContext = ViewModel;
                    btnPasswordVault.Click += btnPasswordVault_Click;
                }
                return btnPasswordVault;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnPasswordVault_Click(object arg1, RoutedEventArgs arg2)
        {
            btnPasswordVault.Selected = true;
            if(MenuButtonClick != null) MenuButtonClick(this, arg2);
        }

        public object ViewModel
        {
            get {
                if (viewModel == null)
                {
                    viewModel = new SecureItemsHolderViewModel(resolver, SubItemsComponentTree, SecurityItemsDefaultProperties.SecurityItemType_PasswordVault); 
                    viewModel.CurrentDataTemplateSelector = Application.Current.Resources["PVSecureItemTemplateSelector"] as DataTemplateSelector;

                }
                //viewModel = new PasswordVaultViewModel(resolver);
                    return viewModel;
                }
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get {
                if(content == null)
                {
                    content = new NewPasswordVaultContentPanel();
                    content.DataContext = ViewModel;
                 }               
                return content;
            }
        }

        public string Header { get { return Application.Current.FindResource("Passwords") as string; } }


        void AddEditControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    ((FrameworkElement)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                });

            }
            
        }

        public bool Visible
        {
            get
            {
                return true;
            }
            set
            {
                // Cannot change visibility of Password Vault.
            }
        }

        public bool Selected
        {
            get
            {
                return btnPasswordVault.Selected;
            }
            set
            {
                btnPasswordVault.Selected = value;


                if (ContentPanel.Visibility == Visibility.Collapsed && value)
                {
                    if (viewModel != null)
                    {
                        viewModel.ChangeValuesForDatabase();
                        viewModel.RefreshData();
                    }

                }

                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                
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
                return new AddSecureItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["Password"] as string,
                    Icon = Application.Current.Resources["addPassword"] as ImageSource,
                    AddTitel = Application.Current.Resources["AddPassword"] as string,
                    AddSubTitel = Application.Current.Resources["AddDialoguePasswordBody"] as string,
                    SubTypesList = viewModel.SubItemsComponentTree
                };
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
                        }

                };
            }
        }



        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            var item = (SecureItem)parameters["item"];
            var secureItemVM = viewModel.secureItemList.Find(x => x.Id == item.Id);
            if (secureItemVM != null)
            {
                viewModel.EditItemCommand.Execute(secureItemVM);
            }
            return true;
        }

    }
}
