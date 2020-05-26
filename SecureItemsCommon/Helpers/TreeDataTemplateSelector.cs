using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SecureItemsCommon.Helpers
{
    public class TreeDataTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate FolderTemplate
        {
            get; set;
        }

        public DataTemplate SecureItemsListTemplate
        {
            get; set;

        }


        public TreeDataTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is SecureItemsView)
                return this.SecureItemsListTemplate;

            return this.FolderTemplate;

        }
    }
}
