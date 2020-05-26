using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using PasswordBoss.Views;
using PasswordBoss.ViewModel;

namespace PasswordBoss
{
    [Export(typeof(IUISubComponent))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
    public class SecurityScore : IUISubComponent
    {
        private SecurityScoreContentPanel content = null;
        private static readonly ILogger logger = Logger.GetLogger(typeof(SecurityScore));
        private IResolver resolver;

        [ImportingConstructor]
        public SecurityScore([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/SecurityScore;component/resources/dictionary/SecurityScoreResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (content == null)
                {
                    content = new SecurityScoreContentPanel(resolver);
                    //content.btnUpdateSecutityScore.Click += btnUpdateSecutityScore_Click;
                    content.IsVisibleChanged += SecurityScoreExpander_IsVisibleChanged;
                }
                return content;
            }
        }

        void SecurityScoreExpander_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                content.SecurityScoreExpander.Visibility = Visibility.Visible;
            }
            else
            {
                content.SecurityScoreExpander.Visibility = Visibility.Collapsed;
            }
            
        }

        //void btnUpdateSecutityScore_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SubComponentAction != null) SubComponentAction(sender, e, "OpenAccountSecuritySettings");
        //    content.SecurityScoreExpander.IsExpanded = false;
        //}

        public IList<string> UiComponentIDs
        {
            get { return new List<string> { "PersonalInfo", "DigitalWallet","PasswordVault" }; }
        }


        public event Action<object, RoutedEventArgs, string> SubComponentAction;

        public void NotifySubComponent(string action)
        {
            if(action == "HideSubComponent")
            {
                content.SecurityScoreExpander.IsExpanded = false;
            }

            if(action == "RefreshStats")
            {
                try
                {
                    if (content != null)
                    {
                        var _viewModel = content.DataContext as SecurityScoreViewModel;
                        _viewModel.RefreshStats();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
            }
        }

        public void RefreshData()
        {
            try
            {
                if (content != null)
                {
                    var _viewModel = content.DataContext as SecurityScoreViewModel;
                    _viewModel.SecurityScoreDataCount();
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
            
        }
    }
}
