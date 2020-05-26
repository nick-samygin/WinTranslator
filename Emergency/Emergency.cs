using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PasswordBoss;
using System.Windows;
using Emergency.Views;
using System.Windows.Controls;
using System.Windows.Media;
using Emergency.ViewModel;

namespace Emergency
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Emergency : IUIComponent
    {
        private UserControl contentPanel = null;
        private EmergencyMenuButton btnEmergency = null;
        private EmergencyViewModel _viewModel;
        private IResolver resolver;

        [ImportingConstructor]
        public Emergency([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (contentPanel == null)
                {
                    contentPanel = new EmergencyView
                    {
                        DataContext = ViewModel
                    };
                }

                return contentPanel;
            }
        }

        public string ID
        {
            get
            {
                return "Emergency";
            }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnEmergency == null)
                {
                    btnEmergency = new EmergencyMenuButton();
                    btnEmergency.Click += btnEmergency_Click;
                }
                return btnEmergency;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnEmergency_Click(object arg1, RoutedEventArgs arg2)
        {
            btnEmergency.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }


        public float MenuRank
        {
            get
            {

                return 9f;
            }
        }


        public string Header { get { return Application.Current.FindResource("NavEmergency") as string; } }

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
                return btnEmergency.Selected;
            }
            set
            {
                btnEmergency.Selected = value;

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
                    ItemTitel = Application.Current.Resources["ItemEmergencyContact"] as string,
                    Icon = Application.Current.Resources["addEmergencyContact"] as ImageSource
                };
            }
        }

        public object ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new EmergencyViewModel(resolver);
                }
                return _viewModel;
            }
        }

        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
