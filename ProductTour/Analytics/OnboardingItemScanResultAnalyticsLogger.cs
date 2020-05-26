using PasswordBoss;
using PasswordBoss.PBAnalytics;
using ProductTour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductTour.Analytics
{
    public class OnboardingItemScanResultAnalyticsLogger<TEvent> : AnalyticsLoggerBase<OnboardingItemScanResult>
        where TEvent : Event, IEventProperty<OnboardingItemScanResult>, new()
    {
        private readonly OnboardingSteps onboardingStep;

        private readonly IScanSummary scanSummary;

        public OnboardingItemScanResultAnalyticsLogger(IInAppAnalytics analytics, OnboardingSteps onboardingStep, IScanSummary scanSummary)
            : base(analytics)
        {
            this.onboardingStep = onboardingStep;
            this.scanSummary = scanSummary;
        }

        protected override IAnalytics<OnboardingItemScanResult> CreateAnalytics(IInAppAnalytics analytics)
        {
            return analytics.Get<TEvent, OnboardingItemScanResult>();
        }

        protected override OnboardingItemScanResult CreateItemForLog(MarketingActionType action)
        {
            return new OnboardingItemScanResult(onboardingStep, action, scanSummary.Insecure, scanSummary.Duplicate, scanSummary.Weak);
        }
    }
}
