using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordBoss.ViewModel;
using Telerik.Windows.Controls;

namespace PasswordBoss.Helpers
{
    class RadTreeViewItemStyleSelector : StyleSelector
    {
        public Style BaseStyle { get; set; }
        public Style FolderStyle { get; set; }
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var style = new Style(typeof(RadTreeViewItem), BaseStyle);
            if (item is SharedByMeViewModel)
            {
                var marginSetter = new Setter(FrameworkElement.MarginProperty, new Thickness(0, 0, 0, 10));
                style.Setters.Add(marginSetter);

                var borderBrushSetter = new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Gray));
                style.Setters.Add(borderBrushSetter);
                
                var tagSetter = new Setter(FrameworkElement.TagProperty, true);
                style.Setters.Add(tagSetter);
            }

            if (item is ShareFolderViewModel)
                return FolderStyle;

            return style;
        }
    }
}
