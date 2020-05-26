using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProductTour.Converters
{
    public class GridRowIndexToColorConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                                 System.Globalization.CultureInfo culture)
        {
            return (value as DataGridRow).GetIndex();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                   System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
