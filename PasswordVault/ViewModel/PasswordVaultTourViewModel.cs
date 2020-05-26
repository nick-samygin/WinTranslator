using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace PasswordBoss.ViewModel
{
    class PasswordVaultTourViewModel : ViewModelBase
    {
        #region Commands

        public RelayCommand NextCommand { get; set; }

        #endregion

        #region Properties



        private double _securityScorePosition;
        public double SecurityScorePosition
        {
            get { return _securityScorePosition; }
            set
            {
                _securityScorePosition = value;
                RaisePropertyChanged("SecurityScorePosition");
            }
        }

        private double _mainContentHeight;
        public double MainContentHeight
        {
            get { return _mainContentHeight; }
            set
            {
                _mainContentHeight = value;
                RaisePropertyChanged("MainContentHeight");
            }
        }

        private bool _personalAccountsVisibility = false;
        public bool PersonalAccountsVisibility
        {
            get { return _personalAccountsVisibility; }
            set
            {
                _personalAccountsVisibility = value;
                RaisePropertyChanged("PersonalAccountsVisibility");
            }
        }

        private bool _logosVisibility = false;
        public bool LogosVisibility
        {
            get { return _logosVisibility; }
            set
            {
                _logosVisibility = value;
                RaisePropertyChanged("LogosVisibility");
            }
        }

        private bool _addNewItemVisibility = false;
        public bool AddNewItemVisibility
        {
            get { return _addNewItemVisibility; }
            set
            {
                _addNewItemVisibility = value;
                RaisePropertyChanged("AddNewItemVisibility");
            }
        }

        private bool _alertsVisibility = false;
        public bool AlertsVisibility
        {
            get { return _alertsVisibility; }
            set
            {
                _alertsVisibility = value;
                RaisePropertyChanged("AlertsVisibility");
            }
        }

        private bool _viewsVisibility = false;
        public bool ViewsVisibility
        {
            get { return _viewsVisibility; }
            set
            {
                _viewsVisibility = value;
                RaisePropertyChanged("ViewsVisibility");
            }
        }

        private bool _setupProgressVisibility = false;
        public bool SetupProgressVisibility
        {
            get { return _setupProgressVisibility; }
            set
            {
                _setupProgressVisibility = value;
                RaisePropertyChanged("SetupProgressVisibility");
            }
        }

        private bool _dataStorageVisibility = false;
        public bool DataStorageVisibility
        {
            get { return _dataStorageVisibility; }
            set
            {
                _dataStorageVisibility = value;
                RaisePropertyChanged("DataStorageVisibility");
            }
        }

        private bool _securityScoreVisibility = false;
        public bool SecurityScoreVisibility
        {
            get { return _securityScoreVisibility; }
            set
            {
                _securityScoreVisibility = value;
                RaisePropertyChanged("SecurityScoreVisibility");
            }
        }

        private UIElement _securityScoreElement;
        public UIElement SecurityScoreElement
        {
            get { return _securityScoreElement; }
            set
            {
                _securityScoreElement = value;
                RaisePropertyChanged("SecurityScoreElement");
            }
        }

        private Grid _mainPanelElement;
        public Grid MainPanelElement
        {
            get { return _mainPanelElement; }
            set
            {
                _mainPanelElement = value;
                RaisePropertyChanged("MainPanelElement");
            }
        }

        #endregion



        public PasswordVaultTourViewModel(IResolver resolver)
        {
            InitializeCommands();
            PersonalAccountsVisibility = true;
        }

        public void InitializeCommands()
        {
            NextCommand = new RelayCommand(NextClick);
            

        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj, string elemName) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        var frameworkElement = child as FrameworkElement;
                        if (child is T && frameworkElement != null && frameworkElement.Name == elemName)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child, elemName))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public void DetermineSecurityScorePosition()
        {
            Window main = System.Windows.Application.Current.MainWindow;
            //UiElement = main.FindName("alertMessagesButton") as UIElement;
            var elems = FindLogicalChildren<Expander>(main, "SecurityScoreExpander");
            Expander expander = elems.First();
            SecurityScoreElement = expander.Template.FindName("PopupRootGrid", expander) as Grid;
            //SecurityScoreElement = elems.First();
            //var location = UiElement.PointToScreen(new Point(0, 0));

            if (SecurityScoreElement != null)
            {
                Point relativeLocation = SecurityScoreElement.TranslatePoint(new Point(0, 0), main);
                SecurityScorePosition = relativeLocation.Y;
            }
        }

        public void DetermineMainPanelHeight()
        {
            Window main = System.Windows.Application.Current.MainWindow;
            var elems = FindLogicalChildren<Grid>(main, "ItemsGrid");
            MainPanelElement = elems.First();
            
            if(MainPanelElement != null)
            {
                MainContentHeight = MainPanelElement.ActualHeight;
            }
            

        }

        private void NextClick(object obj)
        {
            if(obj != null)
            {
                int _number = Convert.ToInt32(obj); 
                    switch(_number)
                    {
                        case 1:
                            
                            PersonalAccountsVisibility = false;
                            DetermineMainPanelHeight();
                            LogosVisibility = true;
                            break;
                        case 2:
                            LogosVisibility = false;
                            AddNewItemVisibility = true;
                            break;
                        case 3:
                            AddNewItemVisibility = false;
                            AlertsVisibility = true;
                            break;
                        case 4:
                            AlertsVisibility = false;
                            ViewsVisibility = true;
                            break;
                        case 5:
                            ViewsVisibility = false;
                            SetupProgressVisibility = true;
                            break;
                        case 6:
                            SetupProgressVisibility = false;
                            DataStorageVisibility = true;
                            break;
                        case 7:
                            DetermineSecurityScorePosition();
                            DataStorageVisibility = false;
                            SecurityScoreVisibility = true;
                            break;
                        default:
                            break;
                    }
            }

        }

    }
}
