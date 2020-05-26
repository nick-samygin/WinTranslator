using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PasswordBoss.Converters
{
    class SharedWithMeItemStateToVisibilityConverter : IValueConverter
    {
        public SharedWithMeItemState StateForVisibility { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (SharedWithMeItemState) value;
            return state == StateForVisibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
