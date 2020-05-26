using PasswordBoss;
using PasswordBoss.PBAnalytics;

namespace ProductTour.Analytics
{
    public class OnboardingItemAnalyticsLogger<TEvent> : AnalyticsLoggerBase<OnboardingItem>
        where TEvent : Event, IEventProperty<OnboardingItem>, new()
    {
        private readonly OnboardingSteps onboardingStep;

        public OnboardingItemAnalyticsLogger(IInAppAnalytics analytics, OnboardingSteps onboardingStep)
            : base(analytics)
        {
            this.onboardingStep = onboardingStep;
        }
        protected override IAnalytics<OnboardingItem> CreateAnalytics(IInAppAnalytics analytics)
        {
            return analytics.Get<TEvent, OnboardingItem>();
        }

        protected override OnboardingItem CreateItemForLog(MarketingActionType action)
        {
            return new OnboardingItem(onboardingStep, action);
        }
    }
}
