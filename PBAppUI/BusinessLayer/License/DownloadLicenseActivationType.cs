namespace PasswordBoss.BusinessLayer.License
{
	public class DownloadLicenseActivationType : LicenseActivationTypeBase
	{
		public override string LicenseMessage
		{
			get
			{
				return GetResourceString("OnboardLicenseEntryType012Body");
            }
		}

		public override string LicenseType
		{
			get
			{
				return string.Format("{0} / {1}",
					GetResourceString("SubscriptionTypeFree"),
					GetResourceString("SubscriptionTypeTrial"));
			}
		}
	}
}