using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PasswordBoss;
using System.Windows;
using Identities.Views;
using System.Windows.Controls;
using System.Windows.Media;

namespace Identities
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Identities : IUIComponent
    {
        private UserControl contentPanel = null;
        private IdentitiesMenuButton btnIdentities = null;

        private IResolver resolver;

        [ImportingConstructor]
        public Identities([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (contentPanel == null)
                {
                    contentPanel = new UserControl();
                }

                return contentPanel;
            }
        }

        public string ID
        {
            get
            {
                return "Identities";
            }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnIdentities == null)
                {
                    btnIdentities = new IdentitiesMenuButton();
                    btnIdentities.Click += btnIdentities_Click;
                }
                return btnIdentities;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnIdentities_Click(object arg1, RoutedEventArgs arg2)
        {
            btnIdentities.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }


        public float MenuRank
        {
            get
            {

                return 5f;
            }
        }


        public string Header { get { return Application.Current.FindResource("NavIdentities") as string; } }

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
                return btnIdentities.Selected;
            }
            set
            {
                btnIdentities.Selected = value;

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
                    ItemTitel = Application.Current.Resources["ItemIdentity"] as string,
                    Icon = Application.Current.Resources["addIdentity"] as ImageSource
                };
            }
        }

        public object ViewModel
        {
            get
            {
                return null;
               // throw new NotImplementedException();
            }
        }

        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
