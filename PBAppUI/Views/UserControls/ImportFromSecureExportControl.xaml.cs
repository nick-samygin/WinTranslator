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
    public partial class ImportFromSecureExportControl
    {
        private IResolver resolver;
        public ImportFromSecureExportControl()
        {
            this.resolver = ((PBApp)Application.Current).GetResolver();
            this.DataContext = new MenuViewModel();
            InitializeComponent();
        }

        public ImportFromSecureExportControl(MenuViewModel viewModel)
        {
            viewModel.FilePathTextSecureExport = String.Empty;
            viewModel.EmailSecureExport = String.Empty;
            viewModel.PasswordSecureExport = String.Empty;
            viewModel.GridTwoEnabledSecureExport = false;
            viewModel.GridThreeEnabledSecureExport = false;
            viewModel.GridOneBackgroundSecureExport = viewModel.ReturnBackgroundColor("ImportPasswordsBackgroundColor");
            viewModel.GridTwoBackgroundSecureExport = viewModel.ReturnBackgroundColor("WhiteColor");
            viewModel.GridThreeBackgroundSecureExport = viewModel.ReturnBackgroundColor("WhiteColor");
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

        private void txtEmailSecureExportWatermark_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmailSecureExportWatermark.Visibility = System.Windows.Visibility.Collapsed;
            txtEmailSecureExport.Visibility = System.Windows.Visibility.Visible;
            txtEmailSecureExport.Focus();
        }

        private void txtEmailSecureExport_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEmailSecureExport.Text))
            {
                txtEmailSecureExport.Visibility = System.Windows.Visibility.Collapsed;
                txtEmailSecureExportWatermark.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void txtPasswordSecureExportWatermark_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPasswordSecureExportWatermark.Visibility = System.Windows.Visibility.Collapsed;
            pbPasswordSecureExport.Visibility = System.Windows.Visibility.Visible;
            pbPasswordSecureExport.Focus();
        }

        private void pbPasswordSecureExport_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.pbPasswordSecureExport.Password))
            {
                pbPasswordSecureExport.Visibility = System.Windows.Visibility.Collapsed;
                txtPasswordSecureExportWatermark.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
