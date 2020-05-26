using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;

namespace PasswordBoss.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        #region Properties

        IPBData pbData = null;
        IResolver resolver = null;
        Common _common = new Common();
        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                //Debugger.Launch();
                _search = value;
                RaisePropertyChanged("Search");
                SearchSecureItems(_search);
            }
        }

        private bool _showWattermark = true;
        public bool ShowWattermark
        {
            get { return _showWattermark; }
            set
            {
                if (_showWattermark != value)
                {
                    _showWattermark = value;
                    RaisePropertyChanged("ShowWattermark");
                }
            }
        }

        private bool _showNoMatching;
        public bool ShowNoMatching
        {
            get { return _showNoMatching; }
            set
            {
                if (_showNoMatching != value)
                {
                    _showNoMatching = value;
                    RaisePropertyChanged("ShowNoMatching");
                }
            }
        }


        private bool _IsOpen;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set
            {
                if (_IsOpen != value)
                {
                    _IsOpen = value;
                    if (!_IsOpen)
                        ShowWattermark = true;

                    RaisePropertyChanged("IsOpen");
                    if (ChangeVisibility != null)
                        ChangeVisibility(null, null);
                }
            }
        }

        public event EventHandler ChangeVisibility;



        private ObservableCollection<ISecureItemVM> _searchResultItemList = new ObservableCollection<ISecureItemVM>();
        public ObservableCollection<ISecureItemVM> SearchResultItemList
        {
            get { return _searchResultItemList; }
            set
            {
                _searchResultItemList = value;
                RaisePropertyChanged("SearchResultItemList");
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
            }
        }


        #endregion

        public event EventHandler Edit_Clicked;


        public RelayCommand ClearSearchCommand { get; set; }
        public RelayCommand WatermarkGotFocusCommand { get; set; }
        public RelayCommand MovetoFolderCommand { get; set; }
        public RelayCommand DeleteItemsCommand { get; set; }
        #region Relay commands
        #endregion
        private IEnumerable<AddSecureSubItem> _addSecureSubItemsList;
        private IEnumerable<ISecureHolder> _secureHolderCollection;



        public SearchViewModel(IResolver resolver, IEnumerable<ISecureHolder> secureHolderCollection)
        {
            this.resolver = resolver;
            this.pbData = resolver.GetInstanceOf<IPBData>();
            SearchResultItemList = new ObservableCollection<ISecureItemVM>();
            _secureHolderCollection = secureHolderCollection;
            ClearSearchCommand = new RelayCommand(ClearClick);
            WatermarkGotFocusCommand = new RelayCommand(WatermarkGotFocus);
            MovetoFolderCommand = new RelayCommand(MovetoFolder);
            DeleteItemsCommand = new RelayCommand(DeleteItems);
        }


        #region Custom methods

        public void SearchSecureItems(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                IsOpen = false;
                return;
            }

            IsOpen = true;
            if (_secureHolderCollection == null)
                return;

            SearchResultItemList.Clear();
            var searchResult = new List<SecureItemSearchResult>();

            if (!string.IsNullOrWhiteSpace(search))
            {

                searchResult = pbData.GetSecureItemSearchResult(search);
            }

            //convert search result to view model suitable for showing
            foreach (var item in searchResult)
            {

                var holder = _secureHolderCollection.FirstOrDefault(x => x.SecureItemType == item.SecureItemTypeName);
                if (holder != null)
                {
                    var secureItemVM = holder.CreateItemForSearch(item);
                    if (secureItemVM != null)
                        SearchResultItemList.Add(secureItemVM);
                }

            }
            ShowNoMatching = SearchResultItemList.Any() ? false : true;


        }

        public void Update()
        {
            if (IsOpen)
                SearchSecureItems(Search);
        }

        private void SecureItemVM_Edit_Clicked(object sender, EventArgs e)
        {
            if (Edit_Clicked != null)
                Edit_Clicked(sender, e);
        }

        private void ClearClick(object param)
        {
            Search = string.Empty;
        }

        private void WatermarkGotFocus(object param)
        {
            ShowWattermark = false;
            if (param != null && param is TextBox)
                ((TextBox)param).Focus();
        }

        private void MovetoFolder(object param)
        {
            var ob = param as Tuple<object, string>;
            if (ob != null)
            {
                var parentFolderId = ob.Item2;
                foreach (var item in SelectedItems.GroupBy(x => (x as ISecureItemVM).Type))
                {
                    var holder = _secureHolderCollection.FirstOrDefault(x => x.SecureItemType == item.Key);
                    if (holder != null)
                    {
                        holder.MoveSecureItemToFolder(item, parentFolderId);
                    }
                }

            }

        }

        private void DeleteItems(object param)
        {

            foreach (var item in SelectedItems.GroupBy(x => (x as ISecureItemVM).Type))
            {
                var holder = _secureHolderCollection.FirstOrDefault(x => x.SecureItemType == item.Key);
                if (holder != null)
                {
                    holder.DeleteSelectedItemsClick(item);
                }
            }



        }

        public IEnumerable<IContextAction> GetActions()
        {
            if (_selectedItems.Count == 1)
                return _selectedItems.FirstOrDefault().Actions;
            var actions = _secureHolderCollection.FirstOrDefault().GetActions(_selectedItems);
            actions.FirstOrDefault(x => x.IsFolderList).SubItems.ForEach(x => x.Action = MovetoFolderCommand);
            actions.LastOrDefault().Action = DeleteItemsCommand;
            return actions;
        }



        #endregion

    }
}
