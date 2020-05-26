using System.Windows;
using System.Windows.Controls;
using PasswordBoss.ViewModel;

namespace PasswordBoss.Helpers
{
    public class TreeDataTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate HeaderTemplate
        {
            get; set;
        }

        public DataTemplate SecureItemsListTemplate
        {
            get; set;
        }

        public HierarchicalDataTemplate FolderTemplate
        {
            get; set;
        }

        public HierarchicalDataTemplate FolderCategoryTemplate
        {
            get; set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is ISecureItemVM)
                return SecureItemsListTemplate;
            if (item is ShareFolderViewModel)
                return FolderTemplate;
            if (item is FolderCategoryViewModel)
                return FolderCategoryTemplate;

            return HeaderTemplate;

        }
    }
}
