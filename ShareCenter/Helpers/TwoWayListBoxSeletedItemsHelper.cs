using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace PasswordBoss.Helpers
{
    class TwoWayListBoxSeletedItemsHelper
    {
        private static ListBox _listBox;
        private static bool _isDuringCollectionChaged;
        #region SelectedItems
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TwoWayListBoxSeletedItemsHelper),
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
            _listBox = (ListBox)d;
            var selectedItems = GetSelectedItems(_listBox) as ObservableCollection<object>;
            if(selectedItems != null)
                selectedItems.CollectionChanged += OnCollectionChanged;
            ReSetSelectedItems();
            _listBox.SelectionChanged += delegate
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
                    if(_listBox.SelectedItems.Contains(oldItem))
                        _listBox.SelectedItems.Remove(oldItem);
                }
            }
            _isDuringCollectionChaged = false;
        }

        #endregion

        private static void ReSetSelectedItems()
        {
            var selectedItems = GetSelectedItems(_listBox);
            if (_isDuringCollectionChaged)
                return;
            selectedItems.Clear();
            if (_listBox.SelectedItems != null)
            {
                foreach (var item in _listBox.SelectedItems)
                    selectedItems.Add(item);
            }
        }
    }
}
