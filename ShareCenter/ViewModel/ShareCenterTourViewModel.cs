using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.ViewModel
{
    class ShareCenterTourViewModel : ViewModelBase
    {
        #region Commands

        public RelayCommand TabsNextCommand { get; set; }
        public RelayCommand SharedByMeCommand { get; set; }

        #endregion

        #region Properties

        private bool _shareTabsVisibility = false;
        public bool ShareTabsVisibility
        {
            get { return _shareTabsVisibility; }
            set
            {
                _shareTabsVisibility = value;
                RaisePropertyChanged("ShareTabsVisibility");
            }
        }

        private bool _sharedByMeVisibility = false;
        public bool SharedByMeVisibility
        {
            get { return _sharedByMeVisibility; }
            set
            {
                _sharedByMeVisibility = value;
                RaisePropertyChanged("SharedByMeVisibility");
            }
        }

        private bool _sharedWithMeVisibility = false;
        public bool SharedWithMeVisibility
        {
            get { return _sharedWithMeVisibility; }
            set
            {
                _sharedWithMeVisibility = value;
                RaisePropertyChanged("SharedWithMeVisibility");
            }
        }


        #endregion

        public ShareCenterTourViewModel()
        {
            TabsNextCommand = new RelayCommand(TabsNextClick);
            SharedByMeCommand = new RelayCommand(SharedByMeNextClick);
            ShareTabsVisibility = true;
        }

        public void TabsNextClick(object obj)
        {
            ShareTabsVisibility = false;
            SharedByMeVisibility = true;
        }

        public void SharedByMeNextClick(object obj)
        {
            SharedByMeVisibility = false;
            SharedWithMeVisibility = true;
        }
    }
}
