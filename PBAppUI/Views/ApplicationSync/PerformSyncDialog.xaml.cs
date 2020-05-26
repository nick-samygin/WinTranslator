using PasswordBoss.Helpers;
using PasswordBoss.Helpers.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views.ApplicationSync
{
	/// <summary>
	/// Interaction logic for PerformSyncDialog.xaml
	/// </summary>
	public partial class PerformSyncDialog : Window
	{
		public static event EventHandler Started;
		public static event EventHandler Finished;
		public static bool IsInSync { get; private set; }

		private static object syncCompletedLocker = new object();
		private Action onSyncCompleted = new Action(() => { });

		private bool isAnimationCompleted = false;
		private bool isSyncCompleted = false;

		private readonly IResolver resolver;
		private ProgressBarAnimation progressBarAnimation;

		public RelayCommand ErrorNotificationPopupCancelCommand { get; set; }

		public RelayCommand ErrorNotificationPopupTryAgainCommand { get; set; }

		public RelayCommand CloseCommand { get; set; }

		private SyncFailedDialog syncFailedDialog = new SyncFailedDialog();

		public PerformSyncDialog(IResolver resolver)
		{
			InitializeComponent();

			syncFailedDialog.Closed += syncFailedDialog_Closed;

			//this.onSyncCompleted = onSyncCompleted;
			//this.storyboardIndicator.Completed += storyboardIndicator_Completed;
			//resolver.GetInstanceOf<IPBSync>().OnSyncFinished += PerformSyncDialog_OnSyncFinished;

			this.resolver = resolver;
			this.Loaded += PerformSyncDialog_Loaded;

			ErrorNotificationPopupCancelCommand = new RelayCommand(ErrorNotificationPopupCancelCommandExecute);
			ErrorNotificationPopupTryAgainCommand = new RelayCommand(ErrorNotificationPopupTryAgainCommandExecute);
			CloseCommand = new RelayCommand(CloseCommandExecute);

			syncFailedDialog.DataContext = this;
			DataContext = this;
		}

		void syncFailedDialog_Closed(object sender, EventArgs e)
		{
			ErrorNotificationPopupCancelCommandExecute(this);
		}

		private void CloseCommandExecute(object obj)
		{			
			this.Close();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (Finished != null)
				Finished(this, EventArgs.Empty);

			this.isClosed = true;
			IsInSync = false;
		}

		public void ErrorNotificationPopupCancelCommandExecute(object obj)
		{
			if (Finished != null)
				Finished(this, EventArgs.Empty);
			this.Close();

			if (obj != this)
				syncFailedDialog.Close();
		}

		public void ErrorNotificationPopupTryAgainCommandExecute(object obj)
		{	
			syncFailedDialog.Hide();
			StartSync();
		}

		private bool isClosed = false;


		private void StartSync()
		{
			IsInSync = true;
			var sync = resolver.GetInstanceOf<IPBSync>();
			progressBarAnimation = new ProgressBarAnimation(sync.StepCount, Indicator);

			Indicator.Width = 0;
			this.Show();

			bool res = false;

			Task.Factory.StartNew(() =>
			{
				res = sync.Sync(progressBarAnimation.OnProgressChanged);

				var action = (Action)(() =>
				{

					if (!res)
					{
						if (!isClosed)
						{
							this.Hide();
							syncFailedDialog.Show();
						}
					}
					else
					{
						CloseCommandExecute(this);
					}
				});

				Application.Current.Dispatcher.Invoke(action);
			});

		}

		void PerformSyncDialog_Loaded(object sender, RoutedEventArgs e)
		{
			if (Started != null)
				Started(this, EventArgs.Empty);
			StartSync();
		}

								

		void PerformSyncDialog_OnSyncFinished(bool obj)
		{
			isSyncCompleted = true;
			RaiseSyncCompleted();
		}

		private void RaiseSyncCompleted()
		{

			lock (syncCompletedLocker)
			{
				if (isAnimationCompleted && isSyncCompleted)
				{
					Application.Current.Dispatcher.Invoke((Action)(() =>
				   {
					   if (onSyncCompleted != null)
					   {
						   onSyncCompleted();
					   }
				   }));
				}
			}

		}

		void storyboardIndicator_Completed(object sender, EventArgs e)
		{
			isAnimationCompleted = true;
			RaiseSyncCompleted();
		}
	}
}
