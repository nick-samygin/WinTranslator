using System;
using System.Globalization;
using System.Windows.Data;

namespace PasswordBoss.Converters
{
    class ShareItemTypeToBoolConverter : IValueConverter
    {
        public AddShareItemType CurrentType { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((AddShareItemType) value) == CurrentType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CurrentType;
        }
    }
}
