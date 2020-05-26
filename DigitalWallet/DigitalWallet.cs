using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using PasswordBoss.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using SecureItemsCommon;

namespace PasswordBoss
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class DigitalWallet : IUIComponent
    {
        private DigitalWalletMenuButton btnDigitalWallet = null;
        private NewDigitalWalletContentPanel content = null;
        private DigitalWalletMiniTour miniTourWindow = null;

        private SecureItemsHolderViewModel viewModel = null;


        private IResolver resolver;
        private IPBData pbData;

        [ImportingConstructor]
        public DigitalWallet([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/DigitalWallet;component/resources/dictionary/DigitalWalletResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "DigitalWallet"; }
        }

        public float MenuRank
        {
            get { return 2f; }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get {
                if (btnDigitalWallet == null)
                {
                    btnDigitalWallet = new DigitalWalletMenuButton();
                    btnDigitalWallet.Click += btnDigitalWallet_Click;
                }
                return btnDigitalWallet;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnDigitalWallet_Click(object arg1, System.Windows.RoutedEventArgs arg2)
        {
            btnDigitalWallet.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }


        public object ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new SecureItemsHolderViewModel(resolver, SubItemsComponentTree, SecurityItemsDefaultProperties.SecurityItemType_DigitalWallet); // new DigitalWalletViewModel(resolver);
                    viewModel.CurrentDataTemplateSelector = Application.Current.Resources["DWSecureItemTemplateSelector"] as DataTemplateSelector;
                }
                return viewModel;
            }
        }



        public Control ContentPanel
        {
            get
            {
                if (content == null)
                {
                    content = new NewDigitalWalletContentPanel();
                    content.DataContext = ViewModel;
                }
                //if (itemsPanel == null)
                //{
                //    itemsPanel = new DigitalWalletItems();

                //}
                return content;
               
            }
        }

        public string Header { get { return Application.Current.FindResource("DigitalWallet") as string; } }

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

        //void content_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    var mv = (DigitalWalletViewModel)ContentPanel.DataContext;
        //    if (mv == null || mv.AddControlViewModel == null) return;
        //    if (!content.DigitalWalletAddEditControl.IsKeyboardFocusWithin)
        //    {
        //        if (mv.AddControlViewModel.CreditCardVisibility && mv.AddControlViewModel.HasModelChanged())
        //        {
        //            if (mv.AddControlViewModel.IsValid)
        //                mv.AddControlViewModel.CreditCardSettingsChangeDialogVisibility = true;
        //            else
        //            {
        //                mv.AddControlViewModel.IsValidErrorMessageVisible = true;
        //                mv.AddControlViewModel.SettingsChangeInvalidDialogVisibility = true;
                        
        //            }
        //        }

        //        if (mv.AddControlViewModel.BankAccountVisibility && mv.AddControlViewModel.HasModelChanged())
        //        {
        //            if (mv.AddControlViewModel.IsValid)
        //                mv.AddControlViewModel.BankAccountSettingsChangeDialogVisibility = true;
        //            else
        //            {
        //                mv.AddControlViewModel.IsValidErrorMessageVisible = true;
        //                mv.AddControlViewModel.SettingsChangeInvalidDialogVisibility = true;
                        
        //            }
        //        }
        //    }
        //}

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
                return btnDigitalWallet.Selected;
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
                btnDigitalWallet.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);


                //Mini-tour section
                /*
                string miniTourValue = pbData.GetConfigurationValueByAccountAndKey(pbData.ActiveUser, "ShowDigitalWalletMiniTour");
                if (!bool.TryParse(miniTourValue, out showMiniTour)) ShowMiniTour = true;
                if (ContentPanel.Visibility == Visibility.Visible && value && ShowMiniTour)
                {
                    try
                    {

                        if (((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreDialogsOpened", null) == false)
                        {
                            miniTourWindow = new DigitalWalletMiniTour(resolver);
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

        public AddItem ComponentTree
        {
            get
            {
                return new AddSecureItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["DigitalWallet"] as string,
                    Icon = Application.Current.Resources["addDigitalWallet"] as ImageSource,
                    AddTitel = Application.Current.Resources["AddDigitalWallet"] as string,
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
