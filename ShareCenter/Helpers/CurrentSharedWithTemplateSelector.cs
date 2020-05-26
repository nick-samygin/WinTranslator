using System.Windows;
using System.Windows.Controls;
using PasswordBoss.ViewModel;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.Helpers
{
    class CurrentSharedWithTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate HeaderTemplate { get; set; }
        public HierarchicalDataTemplate SubItemTemplate { get; set; }
        public DataTemplate SecureItemTemplate { get; set; }
        public HierarchicalDataTemplate FolderTemplate { get; set; }
        public HierarchicalDataTemplate FolderCategoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SharedWithMeItemShareViewModel)
                return SubItemTemplate;
            if(item is ISecureItemVM)
                return SecureItemTemplate;
            if (item is ShareFolderViewModel)
                return FolderTemplate;
            if (item is FolderCategoryViewModel)
                return FolderCategoryTemplate;

            return HeaderTemplate;
        }
    }
}
