using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordBoss.ViewModel;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuExtensionControl.xaml
    /// </summary>
    public partial class ImportFromApplicationControl
    {
        private IResolver resolver;
        public ImportFromApplicationControl()
        {
            this.resolver = ((PBApp)Application.Current).GetResolver();
            this.DataContext = new MenuViewModel();
            InitializeComponent();
        }

        public ImportFromApplicationControl(MenuViewModel viewModel)
        {
            this.resolver = ((PBApp)Application.Current).GetResolver();
            this.DataContext = viewModel ?? new MenuViewModel();
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
