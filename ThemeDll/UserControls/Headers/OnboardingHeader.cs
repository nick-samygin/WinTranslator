using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickZip.UserControls
{
	public class OnboardingHeader : ContentControl
	{

		public static readonly DependencyProperty CloseButtonVisibilityProperty =
			DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(OnboardingHeader), new UIPropertyMetadata(null));

		public Visibility CloseButtonVisibility
		{
			get { return (Visibility)GetValue(CloseButtonVisibilityProperty); }
			set { SetValue(CloseButtonVisibilityProperty, value); }
		}

		private Button closeButtonElement;
		private Button CloseButtonElement
		{
			get { return closeButtonElement; }
			set	{ closeButtonElement = value; }
		}
		
		static OnboardingHeader()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OnboardingHeader), new FrameworkPropertyMetadata(typeof(OnboardingHeader)));
		}
	}	
}