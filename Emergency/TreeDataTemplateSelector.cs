using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Controls;
using Emergency.ViewModel;

namespace Emergency
{
    class TreeDataTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate HeaderTemplate
        {
            get; set;
        }
        public HierarchicalDataTemplate GroupTemplate
        {
            get; set;
        }
        public DataTemplate ItemStyle
        {
            get; set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is EmergencyContactViewModel)
                return HeaderTemplate;
            if(item is EmergencyGroupViewModel)
                return GroupTemplate;

            return ItemStyle;
        }
    }
}
