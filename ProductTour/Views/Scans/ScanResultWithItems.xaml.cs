using PasswordBoss;
using ProductTour.ViewModel.Scans;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductTour.Views.Scans
{
    public partial class ScanResultWithItems : UserControl
    {
        public ScanResultWithItems()
        {
            InitializeComponent();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
			//if (e.Row.GetIndex() % 2 == 0)
			//{
			//	e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(242, 246, 249));
			//}
        }
    }
}