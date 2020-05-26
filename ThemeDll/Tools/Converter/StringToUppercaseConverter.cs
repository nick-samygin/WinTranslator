using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace QuickZip.Converters
{
    /// <summary>
    /// This class simply converts a String to Uppercase
    /// with an optional invert
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToUppercaseConverter: IValueConverter
    {
        public static StringToUppercaseConverter Instance = new StringToUppercaseConverter();
        #region IValueConverter implementation
        /// <summary>
        /// Converts String to Uppercase
        /// </summary>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

                return value.ToString().ToUpper();           

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
