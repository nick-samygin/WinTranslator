using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using PasswordBoss.Helpers;

namespace PasswordBoss.ViewModel
{
    internal class SetupProgressViewModel : ViewModelBase
    {
        private const int On = 1;
        private const int Off = 0;

        #region Relay commands
        public RelayCommand SetupGridHelpImageCommand { get; set; }
        #endregion

        public SetupProgressViewModel()
        {
            InitializeCommands();
            HelpIcon = ReturnHelpImage(Off);
        }

        public void InitializeCommands()
        {
            SetupGridHelpImageCommand = new RelayCommand(SetupProgressClick);
        }

        #region properties

        /// SetUp Progress Grid visibility property
        private bool _setUpProgressGridVisibility;

        public bool SetUpProgressGridVisibility
        {
            get { return _setUpProgressGridVisibility; }
            set
            {
                _setUpProgressGridVisibility = value;
                if(SetUpProgressGridVisibility)
                {
                    HelpIcon = ReturnHelpImage(On);
                }
                else
                {
                    HelpIcon = ReturnHelpImage(Off);
                }
                RaisePropertyChanged("SetUpProgressGridVisibility");
            }
        }

        private ImageSource _helpIcon;

        public ImageSource HelpIcon
        {
            get { return _helpIcon; }
            set
            {
                if (Equals(_helpIcon, value)) return;
                _helpIcon = value;
                RaisePropertyChanged("HelpIcon");
            }
        }

        #endregion

        /// <summary>
        /// return non selected and selected icon 
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        internal ImageSource ReturnHelpImage(int viewType)
        {
            ImageSource returnIcon = null;
            switch (viewType)
            {
                case 0:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgQuestionMark");
                    break;
                case 1:
                    returnIcon = (ImageSource)Application.Current.FindResource("imgHelpHover");
                    break;
            }
            return returnIcon;
        }

        /// <summary>
        /// used to enable the setup progress popup with selected help icon and vice versa
        /// </summary>
        /// <param name="parameter"></param>
        private void SetupProgressClick(object obj)
        {
            string parameter = string.Empty;
            if(obj != null)
            {
                parameter = obj as string;
            }

            if (parameter != null && parameter.Equals("close"))
            {
                SetUpProgressGridVisibility = false;
                return;
            }
            SetUpProgressGridVisibility = true;

        }
    }
}
