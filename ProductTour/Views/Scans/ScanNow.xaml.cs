using ProductTour.ViewModel.Scans;
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

namespace ProductTour.Views.Scans
{
    /// <summary>
    /// Interaction logic for ScanNow.xaml
    /// </summary>
    public partial class ScanNow : UserControl
    {
        public ScanNow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ScanNowViewModel;
            vm.ScanNow.Execute(sender);
        }
    }
}
