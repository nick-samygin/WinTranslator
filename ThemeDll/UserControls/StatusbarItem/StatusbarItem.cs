using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace QuickZip.UserControls
{
    public class StatusbarItem : ContentControl
    {
        static StatusbarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusbarItem), new FrameworkPropertyMetadata(typeof(StatusbarItem)));
        }


        public DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(StatusbarItem),
            new FrameworkPropertyMetadata("Header"));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }


        public DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(StatusbarItem),
            new FrameworkPropertyMetadata(null));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
