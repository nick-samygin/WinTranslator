using PasswordBoss.PBAnalytics;

namespace PasswordBoss.Analytics
{
	public class PersonalInfoSetupAnalyticsLogger : AnalyticsLoggerBase<PersonalizeAccountItem>
	{
		public PersonalInfoSetupAnalyticsLogger(IInAppAnalytics inAppAnalytics)
			: base(inAppAnalytics)
		{

		}

		protected override PersonalizeAccountItem CreateItemForLog(MarketingActionType action)
		{
			return new PersonalizeAccountItem(action);
		}

		protected override IAnalytics<PersonalizeAccountItem> CreateAnalytics(IInAppAnalytics analytics)
		{
			return analytics.Get<Events.PersonalizeAccountEvent, PersonalizeAccountItem>();
		}
	}
}
