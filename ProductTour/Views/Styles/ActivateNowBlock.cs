using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductTour.Views.Styles
{
    public class ActivateNowBlock : ContentControl
    {
        static ActivateNowBlock()
		{
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActivateNowBlock), new FrameworkPropertyMetadata(typeof(ActivateNowBlock)));
		}
    }
}
