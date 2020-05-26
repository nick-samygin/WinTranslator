using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace PasswordBoss.Helpers.Animation
{
	public class ProgressBarAnimation
	{
		private readonly int stepCount;
		private readonly FrameworkElement target;
		private readonly double targetWidth;
		private double targetCurrentWidth;
		private double TargetWidthStep { get { return targetWidth / stepCount; } }
		private readonly TimeSpan stepAnimationDuration = TimeSpan.FromSeconds(0.25);

		public ProgressBarAnimation(int stepCount, FrameworkElement target)
		{
			this.stepCount = stepCount;
			this.target = target;
			this.targetWidth = target.Width;
			this.targetCurrentWidth = 0;
			target.Width = 0;
		}

		public void OnProgressChanged(int step, int currentStep)
		{

			Action action = () =>
			{
				var from = targetCurrentWidth;
				var to = targetCurrentWidth + TargetWidthStep;
				targetCurrentWidth = to;

				if (to > targetWidth)
				{
					targetCurrentWidth = targetWidth;
					to = targetWidth;
				}

				var progress = new DoubleAnimation()
				{
					From = from,
					To = to,
					Duration = stepAnimationDuration,
				};

				Storyboard.SetTarget(progress, target);
				Storyboard.SetTargetProperty(progress, new PropertyPath(FrameworkElement.WidthProperty));

				var sb = new Storyboard();
				sb.Children.Add(progress);
				sb.Begin();
			};
			Application.Current.Dispatcher.Invoke(action);
		}
	}
}
