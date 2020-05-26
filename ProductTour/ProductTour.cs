using PasswordBoss;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using ProductTour.BusinessLayer;
using ProductTour.ViewModel.Scans;
using ProductTour.Views.Scans;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProductTour
{
    [Export(typeof(IDialog))]
    [Export(typeof(IProductTour))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProductTour : IProductTour
    {
        #region Events

        public event Action<object, RoutedEventArgs> DialogClosed;

        #endregion

        #region Fields

        private IResolver _resolver = null;
        private IPBData _pbData = null;

        #endregion

        [ImportingConstructor]
        public ProductTour([Import(typeof(IResolver))] IResolver resolver)
        {
            _resolver = resolver;
            _pbData = resolver.GetInstanceOf<IPBData>();

            ContentPanel.Visibility = Visibility.Hidden;
        }

        #region Public methods

        public void Show(Window parent)
        {
            IsShown = true;

            Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (_content == null)
                        _content = StartScreenFactory.CreateStartupWindow(_resolver);

                    _content.Closing += onContentClosing;

                    if (parent != null)
                    {
                        _content.Owner = parent;
                        _content.Left = parent.Left;
                        _content.Top = parent.Top;
                        _content.Width = parent.ActualWidth;
                        _content.Height = parent.ActualHeight;
                        _content.WindowStartupLocation = parent.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
                    }

                    _content.Show();

                    Configuration configDontShowOnStartup = new Configuration()
                    {
                        AccountEmail = _pbData.ActiveUser,
                        Key = DefaultProperties.Configuration_Key_ProductTourOnStartup,
                        Value = true.ToString()
                    };

                    _pbData.AddOrUpdateConfiguration(configDontShowOnStartup);
                }));
            });
        }

        #endregion

        #region Event handlers

        private void onContentClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsShown = false;

            if (DialogClosed != null)
                DialogClosed(this, null);
        }

        #endregion

        #region Properties

        public string ID
        {
            get
            {
                return "ProductTour";
            }
        }

        public int OrderNumber
        {
            get
            {
                return 1;
            }
        }

        private Window _content = null;
        public Control ContentPanel
        {
            get
            {
                if (_content == null)
                {
                    _content = StartScreenFactory.CreateStartupWindow(_resolver);
                    _content.Closing += onContentClosing;
                }

                return _content;
            }
        }

        public bool ShowOnStartup
        {
            get
            {
                return false;
            }
        }

        private bool _isShown = false;
        public bool IsShown
        {
            get
            {
                return _isShown;
            }
            set
            {
                _isShown = value;
            }
        }

		public static ClosedType ClosedTypeStatic { get; set; }

		public ClosedType ClosedType { get { return ClosedTypeStatic; } }

        #endregion
    }
}