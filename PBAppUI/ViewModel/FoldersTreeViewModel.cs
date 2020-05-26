using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace PasswordBoss.ViewModel
{

    public class TreeItemFolder
    {
        public string uuid { get; set; }
        public List<TreeItemFolder> ChildList { get; set; }
        public string parentId { get; set; }
        public string Name { get; set; }
        public bool browser { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }
    }

    public class MoveSecureItemEventArgs
    {
        public IEnumerable<object> Items { get; set; }
        public string FolderId { get; set; }
    }

    public class FoldersTreeViewModel : ViewModelBase
    {
        private IPBData pbData = null;
        IResolver resolver = null;
        private bool isPined;

        public event EventHandler SelectedFolder_Changed;
        public event EventHandler FolderList_Changed;

        public delegate void MoveSecureItemCommandHandler(object sender, MoveSecureItemEventArgs e);

        public event MoveSecureItemCommandHandler ItemsToFolder_Moved;

        public bool IsPined
        {
            get { return isPined; }
        }

        private ObservableCollection<TreeItemFolder> foldersTreeCollection;
        public ObservableCollection<TreeItemFolder> FoldersTreeCollection
        {
            get { return foldersTreeCollection; }
            set
            {
                foldersTreeCollection = value;
                RaisePropertyChanged("FoldersTreeCollection");
            }
        }

        private TreeItemFolder _selectedFolder;
        public TreeItemFolder SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                if (_selectedFolder != value)
                {
                    _selectedFolder = value;

                    if (_selectedFolder == null)
                    {
                        SelectedFolderText = Application.Current.FindResource("MenuAllFolders") as string;
                        ServiceLocator.Get<IFolderService>().SelectedFolderId = null;
                    }
                    else
                    {
                        SelectedFolderText = _selectedFolder.Name;
                        ServiceLocator.Get<IFolderService>().SelectedFolderId = _selectedFolder.uuid;
                    }


                    if (SelectedFolder_Changed != null)
                        SelectedFolder_Changed(this, null);

                    if (!IsPined)
                        ShowMenuTreeView = false;

                    RaisePropertyChanged("SelectedFolder");
                }

            }
        }

        private string _selectedFolderText = Application.Current.FindResource("MenuAllFolders") as string;
        public string SelectedFolderText
        {
            get
            {
                return _selectedFolderText;
            }
            set
            {
                _selectedFolderText = value;
                RaisePropertyChanged("SelectedFolderText");
            }
        }

        public bool _showMenuTreeView;

        public bool ShowMenuTreeView
        {
            get { return _showMenuTreeView; }
            set
            {
                _showMenuTreeView = value;

                RaisePropertyChanged("ShowMenuTreeView");
            }
        }



        public RelayCommand ShowHideFolderTreeCommand { get; set; }
        public RelayCommand ShowAllCommand { get; set; }
        public RelayCommand AddNewFolderCommand { get; set; }
        public RelayCommand PinPopupCommand { get; set; }
        public RelayCommand ClosePopupCommand { get; set; }

        public FoldersTreeViewModel(IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();

            ShowAllCommand = new RelayCommand((o) => { SelectedFolder = null; });
            ShowHideFolderTreeCommand = new RelayCommand(ShowHideFolderTreeClick);
            AddNewFolderCommand = new RelayCommand(AddNewFolderClick);
            PinPopupCommand = new RelayCommand((o) => { isPined = !isPined; RaisePropertyChanged("IsPined"); });
            ClosePopupCommand = new RelayCommand((o) => { ShowMenuTreeView = false; });
        }

        private void ShowHideFolderTreeClick(object parameter)
        {
            ShowMenuTreeView = !ShowMenuTreeView;
            UpdateFolderTree();
        }

        public void UpdateFolderTree()
        {
            if (ShowMenuTreeView)
                FoldersTreeCollection = GetFoldersTreeCollection();
        }

        public void AddNewFolderClick(object obj)
        {
            var folder = ServiceLocator.Get<IFolderService>().AddFolder();
            if (!string.IsNullOrEmpty(folder))
            {
                FoldersTreeCollection = GetFoldersTreeCollection();
                if (FolderList_Changed != null)
                    FolderList_Changed(this, null);

            };
        }

        private ObservableCollection<TreeItemFolder> GetFoldersTreeCollection()
        {
            var tempList = new ObservableCollection<TreeItemFolder>();
            foreach (var item in pbData.GetFoldersBySecureItemType())
            {
                tempList.Add(new TreeItemFolder()
                {
                    uuid = item.Id,
                    parentId = item.ParentId,
                    Name = item.Name,
                    ChildList = new List<TreeItemFolder>()
                });

            }

            var itemsToRemove = new List<TreeItemFolder>();

            for (int i = 0; i < tempList.Count; i++)
            {
                if (!string.IsNullOrEmpty(tempList[i].parentId))
                {
                    var parent = tempList.FirstOrDefault(x => x.uuid == tempList[i].parentId);
                    if (parent != null)
                    {
                        parent.ChildList.Add(tempList[i]);
                        itemsToRemove.Add(tempList[i]);
                    }

                }

            }
            foreach (var item in itemsToRemove)
                tempList.Remove(item);

            return tempList;
        }


        public void MoveSecureItemToFolder(IEnumerable<object> items, string folderId)
        {
            if (ItemsToFolder_Moved != null)
                ItemsToFolder_Moved(this, new MoveSecureItemEventArgs() { Items = items, FolderId = folderId });

        }
    }
}
