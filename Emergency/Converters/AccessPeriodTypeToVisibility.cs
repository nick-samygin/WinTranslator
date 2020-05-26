using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Emergency.Converters
{
    class AccessPeriodTypeToVisibility : IValueConverter
    {
        public AccessPeriodType TypeForVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((AccessPeriodType) value) == TypeForVisibility
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
