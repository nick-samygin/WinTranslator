using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PasswordBoss.Helpers
{
    public class IsExpandedToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;

            return val ? 180 : 45;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}