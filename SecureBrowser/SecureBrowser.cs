using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using PasswordBoss;
using PasswordBoss.Views;
using System.Diagnostics;
using PasswordBoss.ViewModel;
using PasswordBoss.Helpers;
using System.Windows.Controls;

namespace PasswordBoss
{
    [Export(typeof(IUIComponent))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class SecureBrowser : IUIComponent
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecureBrowser));
        
        private SecureBrowserMenuButton btnSecureBrowser = null;
        private SecureBrowserContentPanel content = null;
        private SecureBrowserMiniTour miniTourWindow = null;
        private IResolver resolver;
        private IPBData pbData;

        [ImportingConstructor]
        public SecureBrowser([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/SecureBrowser;component/resources/dictionary/SecureBrowserResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "SecureBrowser"; }
        }

        public float MenuRank
        {
            get { return 7f; }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get
            {
                if (btnSecureBrowser == null)
                {
                    btnSecureBrowser = new SecureBrowserMenuButton();
                    btnSecureBrowser.Click += btnSecureBrowser_Click;
                }
                return btnSecureBrowser;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnSecureBrowser_Click(object arg1, System.Windows.RoutedEventArgs arg2)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_SecureBrowser_AccessSecureBrowser))
            {
                Selected = false;
                return;
            }

            //btnSecureBrowser.Selected = true;
            if (MenuButtonClick != null) MenuButtonClick(this, arg2);
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (content == null)
                {
					try
					{
						content = new SecureBrowserContentPanel(resolver);
					}
					catch (Exception ex)
					{
						logger.Error("Unable to create Secure Browser panel");
						logger.Error(ex.ToString());
					}
                }
                return content;
            }
        }

        public string Header { get { return Application.Current.FindResource("SecureBrowser") as string; } }

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
                return btnSecureBrowser.Selected;
            }
            set
            {
                btnSecureBrowser.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                if(content != null && value == true)
                {
                    content.OnShowSecureBrowser();
                }

                //Mini-Tour section
                /*
                try
                {
                    string miniTourValue = pbData.GetConfigurationValueByAccountAndKey(pbData.ActiveUser, "ShowSecureBrowserMiniTour");
                    if (!bool.TryParse(miniTourValue, out showMiniTour)) ShowMiniTour = true;
                    if(value && ShowMiniTour)
                    {
                        if (((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreDialogsOpened", null) == false)
                        {
                            miniTourWindow = new SecureBrowserMiniTour(resolver);
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
                return null;
            }
        }

        public object ViewModel
        {
            get
            {
                return null;
            }
        }

        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            if(command == "ReloadSecureBrowser")
            {
                var model = content.DataContext as SecureBrowserViewModel;
                
                if(model != null && model.TabItemCollection != null)
                {
                    var tmp = model.TabItemCollection.ToList();
                    foreach (var tab in tmp)
                    {
                        model.TabItemCollection.Single(p => p.TabId == tab.TabId).RemoveSelectedTabClick(null);
                    }

                    model.LoadSecureBrowserFavoriteList();
                }

            }
            else
            {
                IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
                if (!featureChecker.IsEnabled(DefaultProperties.Features_SecureBrowser_AccessSecureBrowser, showUIIfNotEnabled: false))
                {
                    BrowserHelper.OpenInDefaultBrowser(new Uri(parameters["url"].ToString(), UriKind.RelativeOrAbsolute));
                }
                else
                {
                    var model = content.DataContext as SecureBrowserViewModel;
                    if (model.SelectedTabItem != null)
                    {
                        if (parameters.ContainsKey("TabIndex") && parameters["TabIndex"] is int && (int)parameters["TabIndex"] > -1 && (int)parameters["TabIndex"] < model.TabItemCollection.Count)
                        {
                            var currentTab = model.TabItemCollection[(int)parameters["TabIndex"]];
                            model.SelectedTabItem = currentTab;
                            currentTab.SearchBar.Address = parameters["url"].ToString();
                            currentTab.SearchBar.Navigate();
                        }
                        else
                        {
                            model.SelectedTabItem.AddNewTabForUrl(parameters["url"].ToString(), true);
                        }

                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}
