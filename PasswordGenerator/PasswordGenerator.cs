using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;
using System.Threading.Tasks;
using PasswordBoss.Views;
using PasswordBoss.ViewModel;
using PasswordBoss.Helpers;
using System.Windows.Controls;

namespace PasswordBoss
{
   // [Export(typeof(IUIComponent))]
    //[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class PasswordGenerator //: IUIComponent
    {
        private PasswordGeneratorMenuButton btnPasswordGenerator = null;
        private PasswordGeneratorContentPanel content = null;
        private PasswordGeneratorMiniTour miniTourWindow = null;
        private IResolver resolver;
        private IPBData pbData;
        private UserControl topPanel = null;

        [ImportingConstructor]
        public PasswordGenerator([Import(typeof(IResolver))] IResolver resolver)
        {
            this.resolver = resolver;
            LoadResources();
            ShowMiniTour = true;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        private void LoadResources()
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/PasswordGenerator;component/resources/dictionary/PasswordGeneratorResources.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string ID
        {
            get { return "PasswordGenerator"; }
        }

        public float MenuRank
        {
            get { return 10f; }
        }

        public System.Windows.Controls.Control MenuButton
        {
            get {
                if(btnPasswordGenerator == null)
                {
                    btnPasswordGenerator = new PasswordGeneratorMenuButton();
                    btnPasswordGenerator.Click += btnPasswordGenerator_Click;
                }
                return btnPasswordGenerator;
            }
        }

        public event Action<object, RoutedEventArgs> MenuButtonClick;

        void btnPasswordGenerator_Click(object arg1, System.Windows.RoutedEventArgs arg2)
        {
            IFeatureChecker featureChecker = resolver.GetInstanceOf<IFeatureChecker>();
            if (!featureChecker.IsEnabled(DefaultProperties.Features_PasswordGenerator_AccessPasswordGenerator))
            {
                Selected = false;
                return;
            }

            btnPasswordGenerator.Selected = true;
            if (MenuButtonClick != null)
            {
                MenuButtonClick(this, arg2);
                if(content != null)
                {
                    var _passwordGeneratorViewModel = content.DataContext as PasswordGeneratorViewModel;
                    _passwordGeneratorViewModel.DefaultView();
                }
            }
        }

        public System.Windows.Controls.Control ContentPanel
        {
            get
            {
                if (content == null)
                {
                    content = new PasswordGeneratorContentPanel(resolver);
                }
                return content;
            }
        }

       

        public System.Windows.Controls.Control TopPanel
        {
            get
            {
                if (topPanel == null)
                {
                    topPanel = new System.Windows.Controls.UserControl();
                }

                return topPanel;
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
                ;
            }
        }

        public bool Selected
        {
            get
            {
                return btnPasswordGenerator.Selected;
            }
            set
            {
                btnPasswordGenerator.Selected = value;
                ContentPanel.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);

                // Mini-tour section

                /*
                try
                {
                    string miniTourValue = pbData.GetConfigurationValueByAccountAndKey(pbData.ActiveUser, "ShowPasswordGeneratorMiniTour");
                    if (!bool.TryParse(miniTourValue, out showMiniTour)) ShowMiniTour = true;
                    if (value && ShowMiniTour)
                    {
                        if (((IAppCommand)System.Windows.Application.Current).ExecuteCommand("AreDialogsOpened", null) == false)
                        {
                            miniTourWindow = new PasswordGeneratorMiniTour(resolver);
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
                 * */
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


        public bool ExecuteCommand(string command, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
