using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PasswordBoss.Converters
{
    class ValidStateToBrushConverter : IValueConverter
    {
        public Brush ValidBrush {get; set;}
        public Brush ErrorBrush {get; set;}
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ValidBrush : ErrorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
