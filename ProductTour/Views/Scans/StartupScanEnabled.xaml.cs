using PasswordBoss;
using ProductTour.BusinessLayer;
using ProductTour.ViewModel.Scans;
using System.Windows;
using System.Windows.Input;

namespace ProductTour.Views.Scans
{
    public partial class StartupScanEnabled
    {
        #region Fields

        private readonly WindowResizer _resizer;
        private readonly StartupScanEnabledViewModel viewModel;

        #endregion

        public StartupScanEnabled(IResolver resolver, IRegistryManager registryManager, ILoginsReader loginsReader, bool isAutoShowPopup)
        {
            _resizer = new WindowResizer(this);
            this.Loaded += OnLoaded;
            viewModel = new StartupScanEnabledViewModel(resolver,
                registryManager,
                loginsReader,
                () => this.Close(), isAutoShowPopup);
            this.DataContext = viewModel;

            InitializeComponent();
        }

        #region Event handlers

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Activate();
        }

        private void Resize(object sender, MouseEventArgs e)
        {
            //_resizer.ResizeWindow(sender);
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            _resizer.DisplayResizeCursor(sender);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            _resizer.ResetCursor();
        }

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    PART_MAXIMIZE_RESTORE_Click(sender, e);
                }
                else
                {
                    DragMove();
                }
            }
            catch
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
            }
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            //if (this.WindowState == WindowState.Maximized)
            //{
            //    this.WindowState = WindowState.Normal;
            //}
            //else
            //{
            //    this.WindowState = WindowState.Maximized;
            //}
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
			viewModel.CloseWizardCommand.Execute("AccountCreation_Close");
			//this.Close();
        }

        #endregion
    }
}