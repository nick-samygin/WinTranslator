using PasswordBoss;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.Analytics;
using ProductTour.Models;
using ProductTour.ViewModel.Scans;
using System;
using System.Collections.Generic;
using CreateAnalyticsFunc = System.Func<PasswordBoss.IInAppAnalytics, PasswordBoss.PBAnalytics.IAnalyticsLogger>;

namespace ProductTour.ViewModel
{
	public class AnalyticsFactory : AnalyticsLoggerFactoryBase
    {
        protected override IAnalyticsLogger CreateSpecialAnalyticsItem(IResolver resolver, IAnalyticsLogger analyticsTarget)
		{
			var analytics = resolver.GetInstanceOf<IInAppAnalytics>();

			if (analyticsTarget is ScanResultWithItemsViewModel)
			{
				return new OnboardingItemScanResultAnalyticsLogger<Events.ScanResultsEvent>(analytics, OnboardingSteps.ScanResultWithItems, (IScanSummary)analyticsTarget);
			}
			else if (analyticsTarget is ProgressScanViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.StartupScanInProgress>(analytics, OnboardingSteps.ScanInProgress);
			}
			else if (analyticsTarget is ScanNowViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.StartupScanManualStart>(analytics, OnboardingSteps.ManualStart);
			}
			else if (analyticsTarget is ScanResultNoItemsViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.ScanResultsNoItems>(analytics, OnboardingSteps.ScanResultNoItems);
			}
			else if (analyticsTarget is ScanPopupNotificationViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.ScanResultsPopUpNotification>(analytics, OnboardingSteps.ScanResultPopupNotification);
			}
			else if (analyticsTarget is StartupScanDisabledViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.ScanEventDisabled>(analytics, OnboardingSteps.StartupScanDisabled);
			}
			else if (analyticsTarget is StartupScanEnabledViewModel)
			{
				return new OnboardingItemAnalyticsLogger<Events.ScanEventEnabled>(analytics, OnboardingSteps.StartupScanEnabled);
			}
			
			return null;
		}
	}
}
