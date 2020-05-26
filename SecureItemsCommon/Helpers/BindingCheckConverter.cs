using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SecureItemsCommon.Helpers
{
    [ValueConversion(typeof(string), typeof(string))]
    public class BindingCheckConverter : IValueConverter
    {
        #region IValueConverter implementation
        /// <summary>
        /// Converts String to Uppercase
        /// </summary>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            return value;

        }

        /// <summary>
        /// Convert back, but its not implemented
        /// </summary>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Not implemented");
        }
        #endregion
    }
}
