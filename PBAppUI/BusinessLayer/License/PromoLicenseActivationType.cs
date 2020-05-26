namespace PasswordBoss.BusinessLayer.License
{
	public class PromoLicenseActivationType : LicenseActivationTypeBase
	{
		public override string LicenseMessage
		{
			get
			{
				return GetResourceString("OnboardLicenseEntryType3Body");
            }
		}

		public override string LicenseType
		{
			get
			{
				return GetResourceString("SubscriptionTypePaid");
			}
		}
	}
}
