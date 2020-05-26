using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PasswordBoss;
using System.Windows;
using SecureNotes.Views;
using System.Windows.Controls;
using System.Windows.Media;
using SecureNotes.ViewModel;
using SecureItemsCommon;
using PasswordBoss.Helpers;

namespace SecureNotes
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SecureNotes : IUIComponent
    {
        private UserControl contentPanel = null;
        private SecureNotesMenuButton btnSecureNotes = null;

        private IResolver resolver;

        private SecureItemsHolderViewModel viewModel = null;

        [ImportingConstructor]
        public SecureNotes([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/SecureNotes;component/resources/dictionary/SecureNotesResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public object ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new SecureItemsHolderViewModel(resolver, SubItemsComponentTree, SecurityItemsDefaultProperties.SecurityItemType_SecureNotes);
                    viewModel.CurrentDataTemplateSelector = Application.Current.Resources["SNSecureItemTemplateSelector"] as DataTemplateSelector;

                }
                return viewModel;
            }

        }

        public Control ContentPanel
        {
            get
            {
                if (contentPanel == null)
                {
                    contentPanel = new SecureNoteContentPanel();
                    contentPanel.DataContext = ViewModel;
                }

                return contentPanel;
            }
        }

        public string ID
        {
            get
            {
               return "SecureNotes";
            }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnSecureNotes == null)
                {
                    btnSecureNotes = new SecureNotesMenuButton();
                    btnSecureNotes.Click += btnSecureNotes_Click;
                }
                return btnSecureNotes;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnSecureNotes_Click(object arg1, RoutedEventArgs arg2)
        {
            btnSecureNotes.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }


        public float MenuRank
        {
            get
            {

               return 4f;
            }
        }



        public string Header { get { return Application.Current.FindResource("NavSecureNotes") as string; } }

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
                return btnSecureNotes.Selected;
            }
            set
            {
                if (ContentPanel.Visibility == Visibility.Collapsed && value)
                {
                    if (viewModel != null)
                    {
                        viewModel.ChangeValuesForDatabase();
                        viewModel.RefreshData();
                    }

                }
                btnSecureNotes.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);


            }
        }


        public AddItem ComponentTree
        {
            get
            {
                return new AddSecureItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["SecureNotes"] as string,
                    AddTitel = Application.Current.Resources["AddSecureNote"] as string,
                    AddSubTitel = Application.Current.Resources["AddDialogueOtherBody"] as string,
                    Icon = Application.Current.Resources["addSecureNote"] as ImageSource,
                    SubTypesList = SubItemsComponentTree

                };
            }
        }


        public List<AddSecureSubItem> SubItemsComponentTree
        {
            get
            {
                return new List<AddSecureSubItem>()
                    {
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_AlarmCode)
                        {
                            ItemTitel = Application.Current.Resources["ItemFieldAlarmCode"] as string,
                            CreateItemType=typeof(AlarmCodeSecureItemViewModel),
                        },
                          new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_DriverLicense)
                        {
                            ItemTitel = Application.Current.Resources["ItemDriversLicense"] as string,
                            CreateItemType=typeof(DriversLicenseSecureItemViewModel),
                        },
                          new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_EstatePlan)
                        {
                            ItemTitel = Application.Current.Resources["ItemEstatePlan"] as string,
                            CreateItemType=typeof(EstatePlanSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_FrequentFlyer)
                        {
                            ItemTitel = Application.Current.Resources["ItemFrequentFlyer"] as string,
                            CreateItemType=typeof(FrequentFlyerSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_GenericNote)
                        {
                            ItemTitel = Application.Current.Resources["ItemNote"] as string,
                            CreateItemType=typeof(NoteSecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_HealthInsurance)
                        {
                            ItemTitel = Application.Current.Resources["ItemHealthInsurance"] as string,
                            CreateItemType=typeof(HealthInsuranceSecureItemViewModel),
                        },
                          new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_HotelRewards)
                        {
                            ItemTitel = Application.Current.Resources["ItemHotelRewards"] as string,
                            CreateItemType=typeof(HotelRewardsSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_HotelRewards)
                        {
                            ItemTitel = Application.Current.Resources["ItemInsurance"] as string,
                            CreateItemType=typeof(InsuranceSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_MemberIDs)
                        {
                            ItemTitel = Application.Current.Resources["ItemMemberID"] as string,
                            CreateItemType=typeof(MemberIDSecureItemViewModel),
                        },
                        new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_Passport)
                        {
                            ItemTitel = Application.Current.Resources["ItemPassport"] as string,
                            CreateItemType=typeof(PassportCodeSecureItemViewModel),
                        },
                       new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_Prescription)
                        {
                            ItemTitel = Application.Current.Resources["ItemPrescription"] as string,
                            CreateItemType=typeof(PrescriptionSecureItemViewModel),
                        },
                         new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_SocialSecurity)
                        {
                            ItemTitel = Application.Current.Resources["ItemSocialSecurity"] as string,
                            CreateItemType=typeof(SocialSecuritySecureItemViewModel),
                        },
                       new AddSecureSubItem(SecurityItemsDefaultProperties.SecurityItemType_SecureNotes,SecurityItemsDefaultProperties.SecurityItemSubType_SN_SoftwareLicense)
                        {
                            ItemTitel = Application.Current.Resources["ItemSoftwareLicense"] as string,
                            CreateItemType=typeof(SoftwareLicenseSecureItemViewModel),
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
            switch (command)
            {
                case "AddItem":
                    if (parameters != null && parameters.ContainsKey("item") && parameters["item"] != null && parameters["item"] is ISecureItemVM)
                    {

                        if (viewModel == null) return false;
                        viewModel.AddNewItem((ISecureItemVM)parameters["item"]);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}
