using ProductTour.ViewModel.Scans;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ProductTour.Views.Scans
{
    public partial class ProgressScan : UserControl
    {
        public ProgressScan()
        {
            InitializeComponent();
            storyboardIndicator.CurrentTimeInvalidated += storyboardIndicator_CurrentTimeInvalidated;
            storyboardIndicator.Completed += storyboardIndicator_Completed;

			bool dataContextInitialized = false;
			object locker = new object();
			this.DataContextChanged += (o, e) => 
			{
				lock(locker)
				{
					if (!dataContextInitialized)
					{
						if (ViewModel != null)
						{
							dataContextInitialized = true;
							ViewModel.OnCloseEvent += () =>
							{
								FadeOutAnimation.Completed -= OnFadeOutAnimationCompleted;
								FadeOutAnimation.Stop();

								storyboardIndicator.CurrentTimeInvalidated -= storyboardIndicator_CurrentTimeInvalidated;
								storyboardIndicator.Completed -= storyboardIndicator_Completed;
								storyboardIndicator.Stop();
							};
						}
					}
				}
			};
        }

		protected override void OnInitialized(System.EventArgs e)
		{
			base.OnInitialized(e);
			
		}

		private Storyboard FadeOutAnimation
		{
			get
			{
				Storyboard sb = this.FindResource("fadeOutAnimation") as Storyboard;
				if (sb == null)
				{
					throw new System.NullReferenceException("unable to find storyboard fadeOutAnimation");
				}
				return sb;
			}
		}

        void storyboardIndicator_Completed(object sender, System.EventArgs e)
        {
            ViewModel.ScannedValue = 100;
            ViewModel.RemainingValue = 0;			
			Storyboard.SetTarget(FadeOutAnimation, this.contentGrid);
			FadeOutAnimation.Completed += OnFadeOutAnimationCompleted;
			FadeOutAnimation.Begin();
        }

		private void OnFadeOutAnimationCompleted(object sender, System.EventArgs e)
		{
			ViewModel.OnAnimationScanCompleted();
			FadeOutAnimation.Completed -= OnFadeOutAnimationCompleted;
		}

        void storyboardIndicator_CurrentTimeInvalidated(object sender, System.EventArgs e)
        {
            var clockGroup = ((ClockGroup)sender);
            var currentProgress = clockGroup.CurrentProgress.Value * 5; // HACK:
            var total = clockGroup.NaturalDuration.TimeSpan.TotalSeconds;
            ViewModel.ScannedValue = (int) ((currentProgress / total) * 100);
            ViewModel.RemainingValue = (int) (total - currentProgress) + 1;
        }

        private ProgressScanViewModel ViewModel
        {
			get { return this.DataContext as ProgressScanViewModel; }
        }
    }
}