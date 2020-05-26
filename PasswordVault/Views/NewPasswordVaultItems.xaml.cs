using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using SecureItemsCommon;
using SecureItemsCommon.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for NewPasswordVaultItems.xaml
    /// </summary>
    public partial class NewPasswordVaultItems : UserControl
    {
        public NewPasswordVaultItems()
        {
            InitializeComponent();

            DragDropManager.AddDragOverHandler(treeView, OnItemDragOver);
            DragDropManager.AddDropHandler(treeView, OnDrop);

            this.AddHandler(RadTreeViewItem.DragEnterEvent, new System.Windows.DragEventHandler(OnTreeItemDragEnter));
            this.AddHandler(RadTreeViewItem.DragLeaveEvent, new System.Windows.DragEventHandler(OnTreeItemDragLeave));

        }


        void OnTreeItemDragLeave(object sender, System.Windows.DragEventArgs e)
        {
            RadTreeViewItem treeItem = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
            if (treeItem == null) return;
            VisualStateManager.GoToState(treeItem, "Normal", true);

        }

        void OnTreeItemDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            RadTreeViewItem treeItem = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
            if (treeItem == null) return;
            VisualStateManager.GoToState(treeItem, "MouseOver", true);
            treeItem.IsExpanded = true;

        }

        private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var data = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData");
            if (data == null || (data as IList)==null || (data as IList).Count==0) return;
            if (e.Effects != DragDropEffects.None)
            {
                var destinationItem = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
                var dropDetails = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

                if (destinationItem != null)
                {
                    if (destinationItem.DataContext is SecureItemsCommon.Helpers.FolderView && DataContext is SecureItemsHolderViewModel)
                        ((SecureItemsHolderViewModel)DataContext).MoveSecureItemToFolder(data as IEnumerable<object>, (destinationItem.DataContext as SecureItemsCommon.Helpers.FolderView).uuid);
                }
            }
        }

        IList destinationItems = null;
        private void OnItemDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var item = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
            if (item == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            var position = GetPosition(item, e.GetPosition(item));
            if (item.Level == 0 && position != DropPosition.Inside)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            RadTreeView tree = sender as RadTreeView;
            var draggedData = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData");
            var dropDetails = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

            if ((draggedData == null && dropDetails == null))
            {
                return;
            }
            if (position != DropPosition.Inside)
            {
                e.Effects = DragDropEffects.All;
                dropDetails.IsValidDrop = true;
                destinationItems = item.Level > 0 ? (IList)item.ParentItem.ItemsSource : (IList)tree.ItemsSource;
                int index = destinationItems.IndexOf(item.Item);
                dropDetails.DropIndex = position == DropPosition.Before ? index : index + 1;
            }
            else
            {
                destinationItems = (IList)item.ItemsSource;
                int index = 0;

                if (destinationItems == null)
                {
                    e.Effects = DragDropEffects.None;
                    dropDetails.IsValidDrop = false;
                }
                else
                {
                    e.Effects = DragDropEffects.All;
                    dropDetails.DropIndex = index;
                    dropDetails.IsValidDrop = true;
                }
            }

            dropDetails.CurrentDraggedOverItem = item.Item;
            dropDetails.CurrentDropPosition = position;

            e.Handled = true;
        }



        private DropPosition GetPosition(RadTreeViewItem item, Point point)
        {
            double treeViewItemHeight = 24;
            if (point.Y < treeViewItemHeight / 4)
            {
                return DropPosition.Before;
            }
            else if (point.Y > treeViewItemHeight * 3 / 4)
            {
                return DropPosition.After;
            }

            return DropPosition.Inside;
        }


    }
}
