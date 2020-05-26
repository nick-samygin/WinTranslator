using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Telerik.Windows.Controls;

namespace PasswordBoss.Helpers
{
    class RadTreeViewSelectionHelper
    {
        private static RadTreeView _treeView;
        private static bool _isDuringCollectionChaged;
        #region SelectedItems
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(RadTreeViewSelectionHelper),
                new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _treeView = (RadTreeView)d;
            var selectedItems = GetSelectedItems(_treeView) as ObservableCollection<object>;
            if (selectedItems != null)
                selectedItems.CollectionChanged += OnCollectionChanged;
            ReSetSelectedItems();
            _treeView.SelectionChanged += delegate
            {
                ReSetSelectedItems();
            };
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                _isDuringCollectionChaged = true;
                foreach (var oldItem in e.OldItems)
                {
                    if (_treeView.SelectedItems.Contains(oldItem))
                        _treeView.SelectedItems.Remove(oldItem);
                }
            }
            _isDuringCollectionChaged = false;
        }

        #endregion

        private static void ReSetSelectedItems()
        {
            var selectedItems = GetSelectedItems(_treeView);
            if (_isDuringCollectionChaged)
                return;
            selectedItems.Clear();
            if (_treeView.SelectedItems != null)
            {
                foreach (var item in _treeView.SelectedItems)
                    selectedItems.Add(item);
            }
        }
    }
}
