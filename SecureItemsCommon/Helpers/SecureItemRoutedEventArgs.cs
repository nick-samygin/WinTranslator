using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SecureItemsCommon.Helpers
{
    public class SecureItemRoutedEventArgs : RoutedEventArgs
    {
        public FolderView Folder { get; set; }
        private string itemId;
        public SecureItemRoutedEventArgs(string itemId)
        {
            this.itemId = itemId;
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
            }
        }
    }
}
