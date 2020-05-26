using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PasswordBoss.Converters
{
    class DateFromNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime) value;
            var offset = DateTime.Now - date;
            if (offset.Days > 0)
                return string.Format("{0} {1}", offset.Days, Application.Current.Resources["DaysAgo"] as string);

            return string.Format("{0} {1}", offset.Hours, Application.Current.Resources["HoursAgo"] as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
