using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.ViewModel
{
    public class SecureBrowserTourViewModel : ViewModelBase
    {
        #region Commands

        public RelayCommand NextCommand { get; set; }

        #endregion

        #region Properties

        private bool _searchBarFavoritesVisibility = false;
        public bool SearchBarFavoritesVisibility
        {
            get { return _searchBarFavoritesVisibility; }
            set
            {
                _searchBarFavoritesVisibility = value;
                RaisePropertyChanged("SearchBarFavoritesVisibility");
            }
        }

        private bool _favoriteSitesVisibility = false;
        public bool FavoriteSitesVisibility
        {
            get { return _favoriteSitesVisibility; }
            set
            {
                _favoriteSitesVisibility = value;
                RaisePropertyChanged("FavoriteSitesVisibility");
            }
        }

        #endregion

        public SecureBrowserTourViewModel()
        {
            NextCommand = new RelayCommand(NextClick);
            SearchBarFavoritesVisibility = true;
        }

        public void NextClick(object obj)
        {
            SearchBarFavoritesVisibility = false;
            FavoriteSitesVisibility = true;
        }
    }
}
