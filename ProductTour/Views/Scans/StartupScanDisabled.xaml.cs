using PasswordBoss;
using ProductTour.ViewModel.Scans;
using System.Windows;
using System.Windows.Input;

namespace ProductTour.Views.Scans
{
    public partial class StartupScanDisabled
    {
        public StartupScanDisabled(IResolver resolver)
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            var vm = new StartupScanDisabledViewModel(resolver, () => this.Close());
            this.DataContext = vm;

            this.TitleGrid.MouseLeftButtonDown += OnTitleGridMouseLeftButtonDown;
        }

        #region Event handlers

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Activate();
        }

        private void OnTitleGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion
    }
}