using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductTour.Views.Styles
{
    public class ScanSummaryBlock : ContentControl
    {
        static ScanSummaryBlock()
		{
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScanSummaryBlock), new FrameworkPropertyMetadata(typeof(ScanSummaryBlock)));
		}
    }
}
