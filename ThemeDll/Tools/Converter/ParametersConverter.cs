using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QuickZip.Converters
{
    public class ParametersConverter : IMultiValueConverter
    {    
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parameters = new FindCommandParameters();
            foreach (var obj in values)
            {
                if (parameters.element == null) parameters.element = obj;
                else if (parameters.element2 == null) parameters.element2 = obj;
            }
            return parameters;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
