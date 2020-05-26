using System;
using System.Globalization;
using System.Windows.Data;

namespace PasswordBoss.Helpers
{
    /// <summary>
    /// this converter class is used to pass multiple command parameter with command
    /// </summary>
    public class ParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var parameters = new FindCommandParameters();
            foreach (var obj in values)
            {
                if (parameters.element==null) parameters.element = obj;
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
