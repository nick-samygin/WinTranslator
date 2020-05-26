using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Emergency.Converters
{
    class TrustedContactSplashVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool) values[0]) && ((bool) values[1])
                ? Visibility.Visible
                : Visibility.Collapsed;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
