using Microsoft.Win32;

namespace PasswordBoss.BusinessLayer.License
{
	public abstract class LicenseActivationTypeBase
	{		
		public abstract string LicenseType { get; }
		public int LicenseDurationDays
		{
			get; set;
		}

		public abstract string LicenseMessage { get; }

		public string LicenseDurationStr { get { return string.Format("{0} {1}", LicenseDurationDays, GetResourceString("Day")); } }

		public int InstallType { get; set; }

		protected static string GetResourceString(string name)
		{
			return (string)System.Windows.Application.Current.FindResource(name);
		}
	}

    public class DontShowLicenseType : LicenseActivationTypeBase
    {
        public override string LicenseType
        {
            get { return ""; }
        }

        public override string LicenseMessage
        {
            get { return ""; }
        }
    }
}
