using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PasswordBoss;
using System.Windows;
using Settings.Views;
using System.Windows.Controls;
using Settings.ViewModel;
using Settings.ViewModel.AccountSettings;
using PasswordBoss.UserControls;
using System.Windows.Media;

namespace Settings
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Settings : IUIComponent
    {
        private SettingsContentPanel contentPanel = null;

        private SettingsMenuButton btnSettings = null;
        private AccountSettingsViewModel viewModel = null;

        private IResolver resolver;

        [ImportingConstructor]
        public Settings([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
        }

        public object ViewModel
        {
           
            get
            {
                if (viewModel == null)
                    viewModel = new AccountSettingsViewModel(resolver);
                return viewModel;
            }
        }

      


        public string Header { get { return Application.Current.FindResource("Settings") as string; } }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (contentPanel == null)
                {
                    contentPanel = new SettingsContentPanel(resolver);
                    this.contentPanel.DataContext = ViewModel;
                }

                return contentPanel;
            }
        }

        public string ID
        {
            get
            {
                return "Settings";
            }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnSettings == null)
                {
                    btnSettings = new SettingsMenuButton();
                    btnSettings.Click += btnSettings_Click;
                }
                return btnSettings;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnSettings_Click(object arg1, RoutedEventArgs arg2)
        {
            btnSettings.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }


        public float MenuRank
        {
            get
            {

                return 7f;
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
                return btnSettings.Selected;
            }
            set
            {
                btnSettings.Selected = value;

                //if (ContentPanel.Visibility == Visibility.Visible && !value)
                //{
                //    var m = (PasswordVaultViewModel)ContentPanel.DataContext;
                //    if (m != null)
                //    {
                //        m.AddNewItemVisibility = false;
                //    }

                //}
                //if (ContentPanel.Visibility == Visibility.Collapsed && value)
                //{
                //    var m = (PasswordVaultViewModel)ContentPanel.DataContext;
                //    if (m != null)
                //    {
                //        m.RefreshData();
                //    }

                //}

                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);

            }
        }


        public AddItem ComponentTree
        {
            get
            {
                return new AddItem()
                {
                    ComponentId = ID,
                    ItemTitel = Application.Current.Resources["ItemFolder"] as string,
                    Icon = Application.Current.Resources["addFolder"] as ImageSource
                };
            }
        }

        

        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            switch (command)
            {          
                case "AddItem":
                        var m = (AccountSettingsViewModel)ContentPanel.DataContext;
                        if (m == null) return false;
                        m.AddNewFolderClick(null);
                        return true;
                default:
                    return false;
            }
        }
    }
}
