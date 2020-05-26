using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
using SecureItemsCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SecureItemsCommon.Helpers
{
    public class CommonView : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        

    }
    public class SecureItemsView : CommonView
    {
        public SecureItemsView()
        {

            SecureList = new ObservableCollection<ISecureItemVM>();
        }

       

        private ObservableCollection<ISecureItemVM> _secureList;
        public ObservableCollection<ISecureItemVM> SecureList
        {
            get { return _secureList; }
            set
            {
                _secureList = value;
                RaisePropertyChanged("SecureList");
            }
        }

        private ObservableCollection<ISecureItemVM> _selectedItems = new ObservableCollection<ISecureItemVM>();
        public ObservableCollection<ISecureItemVM> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                RaisePropertyChanged("SelectedItems");
                RaisePropertyChanged("Actions");
            }
        }

        private IEnumerable<object> actions = new ObservableCollection<ContextAction>();
        public IEnumerable<object> Actions
        {
            get
            {
                if (_selectedItems != null)
                {
                    if (_selectedItems.Count == 1)
                        return (_selectedItems.FirstOrDefault() as SecureItemViewModel).Actions;
                }
                return actions;
            }
            set
            {
                actions = value;
                RaisePropertyChanged("Actions");
            }
        }

    }

    public class FolderView : CommonView
    {          
        public FolderView()
        {
            ChildList = new ObservableCollection<CommonView>();            
        }

        public FolderView(string id, string prntId, string folderName, bool expandState)
        {
            uuid = id;
            parentId = prntId;
            FolderName = folderName;
            _IsExpanded = expandState;

            
            ChildList = new ObservableCollection<CommonView>();
        }



        public void AddSecureItem(ISecureItemVM newItem)
        {
            if (!ChildList.Any() || !(ChildList.First() is SecureItemsView))
                ChildList.Insert(0, SecureItemsView);

            SecureItemsView.SecureList.Add(newItem);
            RaisePropertyChanged("Count");
        }

        public void RemoveSecureItem(ISecureItemVM item)
        {
            if (ChildList.Any() && ChildList.First() is SecureItemsView)
            {
                if (SecureItemsView.SecureList.FirstOrDefault(x=>x.Id==item.Id)!=null)
                {
                    SecureItemsView.SecureList.Remove(SecureItemsView.SecureList.FirstOrDefault(x => x.Id == item.Id));
                    if (!SecureItemsView.SecureList.Any())
                    {
                        ChildList.Remove(SecureItemsView);
                    }
                }
            }

            RaisePropertyChanged("ChildList");
            RaisePropertyChanged("Count");
        }

        public string uuid { get; set; }

        private ObservableCollection<CommonView> childList;
        public ObservableCollection<CommonView> ChildList
        {
            get { return childList; }
            set
            {
                childList = value;
                RaisePropertyChanged("ChildList");
                RaisePropertyChanged("Count");
            }
        }

      
        private SecureItemsView _secureItemsView = new SecureItemsView();
        public SecureItemsView SecureItemsView
        {

            get { return _secureItemsView; }
            set
            {
                _secureItemsView = value;
                RaisePropertyChanged("SecureItemsView");
                RaisePropertyChanged("Count");
            }
        }


        public string parentId { get; set; }
        public string FolderName { get; set; }

        public List<ISecureItemVM> GetAllSecureItems()
        {
            var result = new List<ISecureItemVM>(SecureItemsView.SecureList);
            foreach (var item in childList)
            {
                var folder = item as FolderView;
                if (folder != null)
                    result.AddRange(folder.GetAllSecureItems());
            }
            return result;
        }

        public int Count
        {
            get
            {
                if (SecureItemsView != null && SecureItemsView.SecureList != null)
                    return SecureItemsView.SecureList.Count();
                return 0;
            }

        }

        public bool HasSecureItems
        {
            get
            {                
                return Count>0;
            }

        }
    }

    public class CompareCommonViews : IComparer<CommonView>
    {
        // Because the class implements IComparer, it must define a 
        // Compare method. This Compare method compares integers.
        public int Compare(CommonView view1, CommonView view2)
        {
            if(view1 is SecureItemsView && view2 is SecureItemsView)
                return 0;
            if (view1 is SecureItemsView && view2 is FolderView)
                return -1;
            if (view1 is FolderView && view2 is SecureItemsView)
                return 1;
            if (view1 is FolderView && view2 is FolderView)
            {
                var folder1 = view1 as FolderView;
                var folder2 = view2 as FolderView;
                if (folder1.uuid == folder2.uuid)
                    return 0;
                if (folder1.HasSecureItems == folder2.HasSecureItems)
                {
                    if (string.IsNullOrEmpty(folder1.uuid))
                    {
                        return folder1.HasSecureItems ? -1 : 1;
                    }
                    if (string.IsNullOrEmpty(folder2.uuid))
                        return folder2.HasSecureItems ? 1 : -1;

                    return string.Compare(folder1.FolderName, folder2.FolderName);
                }

               return  folder2.Count- folder1.Count ;
            }
               
            return -1;
        }
    }

}
