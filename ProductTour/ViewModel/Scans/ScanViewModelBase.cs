using PasswordBoss;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using ProductTour.Analytics;

namespace ProductTour.ViewModel.Scans
{
    public class ScanViewModelBase : ViewModelBase, IAnalyticsLogger
    {
        private readonly IAnalyticsLogger analyticsLogger;
        public ScanViewModelBase(IResolver resolver)
        {
            this.analyticsLogger = new AnalyticsFactory().CreateAnalyticsItem(resolver, this);
        }

        public void LogStep(MarketingActionType action)
        {
            if (analyticsLogger != null)
            {
                analyticsLogger.LogStep(action);
            }
        }
    }
}
