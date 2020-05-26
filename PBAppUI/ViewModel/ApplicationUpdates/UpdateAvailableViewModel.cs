using PasswordBoss.Helpers;
using System;
using System.Windows;

namespace PasswordBoss.ViewModel.ApplicationUpdates
{
	public class UpdateAvailableViewModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(UpdateAvailableViewModel));

        public RelayCommand UpdateLaterCommand { get; set; }
        public RelayCommand UpdateNowCommand { get; set; }

		public event EventHandler UpdateNowTriggered;

        private string _headerText;

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                RaisePropertyChanged("HeaderText");
            }
        }

        private string _boldBodyText;
        public string BoldBodyText
        {
            get { return _boldBodyText; }
            set
            {
                _boldBodyText = value;
                RaisePropertyChanged("BoldBodyText");
            }
        }

        private string _regularBodyText;
        public string RegularBodyText
        {
            get { return _regularBodyText; }
            set
            {
                _regularBodyText = value;
                RaisePropertyChanged("RegularBodyText");
            }
        }

        private bool _laterButtonVisibility = true;

        public bool LaterButtonVisibility
        {
            get { return _laterButtonVisibility; }
            set
            {
                _laterButtonVisibility = value;
                RaisePropertyChanged("LaterButtonVisibility");
            }
        }

        private bool _showIcon = false;

        public bool ShowIcon
        {
            get{ return _showIcon; }
            set
            {
                _showIcon = value;
                RaisePropertyChanged("ShowIcon");
            }
        }

        private readonly IPBData pbData;
		private bool nightScheduleExecuted = false;
		public static object UpdateLocker = new object();

		public UpdateAvailableViewModel(IPBData pbData)
        {
            InitializeCommands();
			this.pbData = pbData;
		}
		
        public void InitializeCommands()
        {
            UpdateLaterCommand = new RelayCommand(UpdateLaterCommandClick);
            UpdateNowCommand = new RelayCommand(UpdateNowClick);
        }

        private void UpdateLaterCommandClick(object obj)
        {
			
			var window = obj as Window;
            if (window != null)
            {
                window.Close();
            }

			if (updateExecuted)
				return;

			if (!nightScheduleExecuted)
			{
                nightScheduleExecuted = pbData.AddScheduleInAppUpdateTaskAction();
			}
		}

		private bool updateExecuted = false;
        private void UpdateNowClick(object obj)
        {
			updateExecuted = true;
			if (UpdateNowTriggered != null)
			{
				UpdateNowTriggered(this, EventArgs.Empty);
			}

            var window = obj as Window;
            if (window != null)
            {
                window.Close();
            }
		}
	}
}
