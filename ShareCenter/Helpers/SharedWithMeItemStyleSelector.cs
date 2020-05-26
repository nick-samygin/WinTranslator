using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.ViewModel;
using Telerik.Windows.Controls;

namespace PasswordBoss.Helpers
{
    class SharedWithMeItemStyleSelector : StyleSelector
    {
        public Style BaseStyle { get; set; }
        public Style FolderStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var style = new Style(typeof (RadTreeViewItem), BaseStyle);
            if (item is SharedWithMeViewModel)
            {
                var borderBrushSetter = new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Gray));
                style.Setters.Add(borderBrushSetter);

                var marginSetter = new Setter(FrameworkElement.MarginProperty, new Thickness(0,0,0,10));
                style.Setters.Add(marginSetter);
            }
            if (item is ShareFolderViewModel)
                return FolderStyle;
            if (item is SharedWithMeItemShareViewModel)
            {
                var tagSetter = new Setter(FrameworkElement.TagProperty, Visibility.Visible);
                style.Setters.Add(tagSetter);
            }

            return style;
        }
    }
}
