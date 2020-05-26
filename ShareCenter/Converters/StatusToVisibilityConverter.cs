using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace PasswordBoss.Converters
{
    class StatusToVisibilityConverter : IValueConverter
    {
        public ShareWithStatus[] AcceptedStatuses { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ShareWithStatus)value;
            return AcceptedStatuses.Contains(status)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
