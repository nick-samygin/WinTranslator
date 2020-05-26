using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Emergency.ViewModel;

namespace Emergency.Converters
{
    class EmergencyContactsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var contacts = value as List<EmergencyContactViewModel>;
            if (contacts != null)
                return contacts.Where(c => c.AccessPeriodType != AccessPeriodType.FullAccess && !c.IsPending).ToList();
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
