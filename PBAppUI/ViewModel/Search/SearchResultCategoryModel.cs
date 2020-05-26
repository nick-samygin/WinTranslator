using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PasswordBoss.ViewModel.Search
{
    public class SearchResultCategoryModel : ViewModelBase
    {
        public ObservableCollection<SearchResultItemModel> SearchResultItemList { get; set; }
        private string _TypeName;
        public string TypeName
        {
            get { return _TypeName; }
            set
            {
                _TypeName = value;
                RaisePropertyChanged("TypeName");
            }
        }

        public SearchResultCategoryModel()
        {
            SearchResultItemList = new ObservableCollection<SearchResultItemModel>();
        }
    }
}
