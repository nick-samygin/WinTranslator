using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductTour.Views.Styles
{
    public class ActionRequiredHeader : ContentControl
    {
        static ActionRequiredHeader()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionRequiredHeader), new FrameworkPropertyMetadata(typeof(ActionRequiredHeader)));
		}
    }
}
