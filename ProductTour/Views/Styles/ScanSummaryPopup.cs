using ProductTour.Models;
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

namespace ProductTour.Views.Styles
{
    /// <summary>
    /// Interaction logic for ScanSummaryPopup.xaml
    /// </summary>
    public partial class ScanSummaryPopup : ContentControl
    {
        public static readonly DependencyProperty CloseButtonVisibilityProperty =
            DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(ScanSummaryPopup), new UIPropertyMetadata(null));

        public Visibility CloseButtonVisibility
        {
            get { return (Visibility)GetValue(CloseButtonVisibilityProperty); }
            set { SetValue(CloseButtonVisibilityProperty, value); }
        }

        static ScanSummaryPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScanSummaryPopup), new FrameworkPropertyMetadata(typeof(ScanSummaryPopup)));
        }
    }
}
