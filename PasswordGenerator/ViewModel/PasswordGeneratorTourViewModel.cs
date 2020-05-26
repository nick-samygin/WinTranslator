using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBoss.ViewModel
{
    class PasswordGeneratorTourViewModel : ViewModelBase
    {
        #region Commands

        public RelayCommand NextCommand { get; set; }

        #endregion

        #region Properties

        private bool _copyGridVisibility = false;
        public bool CopyGridVisibility
        {
            get { return _copyGridVisibility; }
            set
            {
                _copyGridVisibility = value;
                RaisePropertyChanged("CopyGridVisibility");
            }
        }

        private bool _generatePasswordVisibility = false;
        public bool GeneratePasswordVisibility
        {
            get { return _generatePasswordVisibility; }
            set
            {
                _generatePasswordVisibility = value;
                RaisePropertyChanged("GeneratePasswordVisibility");
            }
        }

        #endregion

        public PasswordGeneratorTourViewModel()
        {
            NextCommand = new RelayCommand(NextClick);
            GeneratePasswordVisibility = true;
        }

        public void NextClick(object obj)
        {
            GeneratePasswordVisibility = false;
            CopyGridVisibility = true;
        }
    }
}
