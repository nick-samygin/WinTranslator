namespace PasswordBoss.BusinessLayer.License
{
	public interface ILicenseFactory
	{
		LicenseActivationTypeBase CreateLicense(int? licenseType, int licenseTermDays);
	}

	public class LicenseFactory : ILicenseFactory
	{
		public LicenseActivationTypeBase CreateLicense(int? licenseType, int licenseTermDays)
		{
			LicenseActivationTypeBase res = null;
			switch (licenseType)
			{
				case 3:
					res = new PromoLicenseActivationType();
					break;
                case 5:
                    res = new DontShowLicenseType();
					break;
				default:
					res = new DownloadLicenseActivationType();
					break;
			}

			res.LicenseDurationDays = licenseTermDays;
			res.InstallType = licenseType ?? 0;
			return res;
		}
	}
}