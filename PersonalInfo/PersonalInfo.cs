using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Windows;
using PasswordBoss.Views;
using PasswordBoss.ViewModel;
using PasswordBoss.DTO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Helpers;
using SecureItemsCommon;

namespace PasswordBoss
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class PersonalInfo : IUIComponent
    {
        private PersonalInfoMenuButton btnPersonalInfo = null;
        private NewPersonalInfoContentPanel content = null;
        private PersonalInfoMiniTour miniTourWindow = null;

        private SecureItemsHolderViewModel viewModel = null;
        private IResolver resolver;
        private IPBData pbData;

        [ImportingConstructor]
        public PersonalInfo([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/PersonalInfo;component/resources/dictionary/PersonalInfoResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "PersonalInfo"; }
        }

        public float MenuRank
        {
            get { return 3f; }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnPersonalInfo == null)
                {
                    btnPersonalInfo = new PersonalInfoMenuButton();
                    btnPersonalInfo.Click += btnPersonalInfo_Click;
                }
                return btnPersonalInfo;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnPersonalInfo_Click(object arg1, System.Windows.RoutedEventArgs arg2)
        {
            btnPersonalInfo.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (content == null)
                {
                    content = new NewPersonalInfoContentPanel();
                    content.DataContext = ViewModel;
                    //content.PersonalInfoAddControl.LostFocus += PersonalInfoAddControl_LostFocus;
                    //content.PersonalInfoAddControl.tabSettings.IsVisibleChanged += tabSettings_IsVisibleChanged;
                }
               
                return content;
            }
        }


        public string Header { get { return Application.Current.FindResource("PersonalInfo") as string; } }




        void tabSettings_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    ((FrameworkElement)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                });

            }
        }

        void PersonalInfoAddControl_LostFocus(object sender, RoutedEventArgs e)
        {
            //var mv = (PersonalInfoViewModel)ContentPanel.DataContext;
            //if (mv == null || mv.PersonalInfoAddNewItemViewModel == null) return;
            //if (!content.PersonalInfoAddControl.IsKeyboardFocusWithin && mv.PersonalInfoAddNewItemViewModel.HasModelChanged())
            //{
            //    if (mv.PersonalInfoAddNewItemViewModel.IsValid)
            //        mv.PersonalInfoAddNewItemViewModel.SettingsChangeDialogVisibility = true;
            //    else
            //    {
            //        mv.PersonalInfoAddNewItemViewModel.IsValidErrorMessageVisible = true;
            //        mv.PersonalInfoAddNewItemViewModel.SettingsChangeInvalidDialogVisibility = true;
                   
            //    }
            //}
        }

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
                return btnPersonalInfo.Selected;
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
                btnPersonalInfo.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);

                //Mini-Tour section
                /*
                string miniTourValue = pbData.GetConfigurationValueByAccountAndKey(pbData.ActiveUser, "ShowPersonalInfoMiniTour");
                if (!bool.TryParse(miniTourValue, out showMiniTour)) ShowMiniTour = true;
                if (ContentPanel.Visibility == Visibility.Visible && value && ShowMiniTour)
                {
                    try
                    {

                        if (((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreDialogsOpened", null) == false)
                        {
                            miniTourWindow = new PersonalInfoMiniTour(resolver);
                            miniTourWindow.Owner = Application.Current.MainWindow;
                            miniTourWindow.Width = miniTourWindow.Owner.ActualWidth;
                            miniTourWindow.Height = miniTourWindow.Owner.ActualHeight;
                            miniTourWindow.ShowDialog();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                */
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

        public object ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new SecureItemsHolderViewModel(resolver, SubItemsComponentTree, SecurityItemsDefaultProperties.SecurityItemType_PersonalInfo);
                    viewModel.CurrentDataTemplateSelector = Application.Current.Resources["PISecureItemTemplateSelector"] as DataTemplateSelector;

                }
                //viewModel = new PasswordVaultViewModel(resolver);
                return viewModel;
            }
        
        }

        public AddItem ComponentTree
        {
            get
            {
                return new AddSecureItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["PersonalInfo"] as string,
                    Icon = Application.Current.Resources["addPersonalInfo"] as ImageSource,
                    AddTitel = Application.Current.Resources["AddPersonalInfo"] as string,
                    AddSubTitel = Application.Current.Resources["AddDialogueOtherBody"] as string,
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


        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            return true;
        }
    }
}
