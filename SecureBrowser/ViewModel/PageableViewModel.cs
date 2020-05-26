using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PasswordBoss.ViewModel
{
    public class PageableViewModel<T> : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PageableViewModel<T>));

        private int _SelectedIndex;
        
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                ViewList.View.Refresh();
                DetermineButtonVisibility();
                RaisePropertyChanged("SelectedIndex");
            }
        }

        private int _ItemsOnPageCount;

        public int ItemsOnPageCount
        {
            get { return _ItemsOnPageCount; }
            set
            {
                _ItemsOnPageCount = value;
                RaisePropertyChanged("ItemsOnPageCount");
            }
        }

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get { return _isNextButtonVisible; }
            set
            {
                _isNextButtonVisible = value;
                RaisePropertyChanged("IsNextButtonVisible");
            }
        }

        private bool _isPreviousButtonVisible;
        public bool IsPreviousButtonVisible
        {
            get { return _isPreviousButtonVisible; }
            set
            {
                _isPreviousButtonVisible = value;
                RaisePropertyChanged("IsPreviousButtonVisible");
            }
        }
        /// <summary>
        /// Property to be shown on GUI
        /// </summary>
        public CollectionViewSource ViewList { get; set; }
        public ObservableCollection<int> PageList { get; set; }
        public ObservableCollection<T> SourceList { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; } 

        public PageableViewModel(ObservableCollection<T> sourceList)
        {
            NextCommand = new RelayCommand(NextClick);
            PreviousCommand = new RelayCommand(PreviousClick);
            ItemsOnPageCount = 8; //default number of items on one page
            SourceList = sourceList;
            PageList = new ObservableCollection<int>();
            ViewList = new CollectionViewSource();
            ViewList.Source = SourceList;
            ViewList.Filter += ViewList_Filter;
            CreatePaging();
            ViewList.View.Refresh();
        }

        public void CreatePaging()
        {
            if(PageList != null) PageList.Clear();
            if (SourceList != null && SourceList.Count > 0)
            {
                int pageCount = 0;
                if (SourceList.Count % ItemsOnPageCount == 0)
                {
                    pageCount = (SourceList.Count / ItemsOnPageCount);
                }
                else
                {
                    pageCount = (SourceList.Count / ItemsOnPageCount) + 1;
                }

                for (int i = 0; i < pageCount; i++)
                {
                    PageList.Add(i);
                }
            }
            
            SelectedIndex = 0;      
        }

        void ViewList_Filter(object sender, FilterEventArgs e)
        {
            int index = SourceList.IndexOf((T)e.Item);

            if (index >= ItemsOnPageCount * SelectedIndex && index < ItemsOnPageCount * (SelectedIndex + 1))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
            
        }

        private void NextClick(object obj)
        {
            try
            {
                SelectedIndex += 1;
                DetermineButtonVisibility();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

        }

        private void PreviousClick(object obj)
        {
            try
            {
                SelectedIndex -= 1;
                DetermineButtonVisibility();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

        }

        public void DetermineButtonVisibility()
        {
            IsPreviousButtonVisible = true;
            IsNextButtonVisible = true;
            if (PageList.Count == 1 || SourceList.Count == 0)
            {
                IsPreviousButtonVisible = false;
                IsNextButtonVisible = false;
                return;
            }
            if (SelectedIndex == 0)
            {
                IsPreviousButtonVisible = false;
            }
            if (SelectedIndex == PageList.Count - 1)
            {
                IsNextButtonVisible = false;
            }
        }
    }
}
