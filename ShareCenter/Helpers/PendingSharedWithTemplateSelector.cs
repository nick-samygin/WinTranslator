using System.Windows;
using System.Windows.Controls;
using PasswordBoss.ViewModel;

namespace PasswordBoss.Helpers
{
    class PendingSharedWithTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate HeaderTemplate { get; set; }
        public DataTemplate SubItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is SharedWithMeItemShareViewModel)
                return SubItemTemplate;
            
            return HeaderTemplate;
        }
    }
}
