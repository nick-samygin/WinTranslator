using System.Windows;
using System.Windows.Controls;
using Emergency.ViewModel;
using Telerik.Windows.Controls;

namespace Emergency
{
    internal class RadTreeViewItemStyleSelector : StyleSelector
    {
        public Style BaseStyle { get; set; }
        public Style GroupStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is EmergencyContactViewModel)
                return BaseStyle;

            return GroupStyle;
        }
    }
}
