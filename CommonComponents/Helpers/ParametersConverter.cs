using System;
using System.Globalization;
using System.Windows.Data;
using PasswordBoss.Helpers;
using System.Windows;

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

    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }

    public class ActionToVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var action = parameter as string;
            var state = value as string;

            switch (state)
            {
                case ShareStatus.Rejected:
                case ShareStatus.Expired:
                    if (action == ShareAction.Remove)
                        return System.Windows.Visibility.Visible;
                    break;
                case ShareStatus.Waiting:
                    if (action == ShareAction.AcceptRequest || action == ShareAction.Cancel)
                        return System.Windows.Visibility.Visible;
                    break;
                case ShareStatus.Pending:
                    if (action == ShareAction.Accept || action == ShareAction.Reject || action == ShareAction.Cancel|| action == ShareAction.Resend) //
                        return System.Windows.Visibility.Visible;
                    break;
                case ShareStatus.Waiting4Data:
                    if (action == ShareAction.ShareData || action == ShareAction.Cancel)
                        return System.Windows.Visibility.Visible;
                    break;
                case ShareStatus.Shared:
                    if (action == ShareAction.Revoke)
                        return System.Windows.Visibility.Visible;
                    break;
                default:
                    return System.Windows.Visibility.Hidden;
            }
            return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(GridLength))]
    public class BoolToGridRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int height = 0;
            if(parameter != null)
                int.TryParse(parameter.ToString(), out height);
            return ((bool)value == true) ? new GridLength(height > 0 ? height : 1, height > 0 ? GridUnitType.Pixel : GridUnitType.Star) : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {    // Don't need any convert back
            return null;
        }
    }
}
