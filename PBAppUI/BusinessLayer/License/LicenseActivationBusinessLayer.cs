using Microsoft.Win32;
using PasswordBoss.Helpers;
using PasswordBoss.WEBApiJSON;
using System;
using System.Windows;

namespace PasswordBoss.BusinessLayer.License
{
	public enum ActivateStatus
	{
		Success,
		InvalidArgument,
		PromocodeUsed,
		InvalidPromocode
	}

	public interface ILicenseActivationBusinessLayer
	{
		int GetLicenseTermDaysRegistryValue();
		int? GetInstallTypeRegistryValue();
		string GetErrorStringFromActivateStatus(ActivateStatus status);
		ActivateStatus Activate(string code);
		bool GetPremium();
    }

	public class LicenseActivationBusinessLayer : ILicenseActivationBusinessLayer
	{
		private static readonly string installTypeRegistryKey = "installtype";
		private static readonly string licenseTermDaysKey = "licensetermdays";
		private static readonly ILogger logger = Logger.GetLogger(typeof(LicenseActivationBusinessLayer));
		private readonly IPBWebAPI webAPI;
		private readonly IPBData pbData;

		public LicenseActivationBusinessLayer(IResolver resolver)
		{
			if (resolver != null)
			{
				webAPI = resolver.GetInstanceOf<IPBWebAPI>();
				pbData = resolver.GetInstanceOf<IPBData>();
			}
		}

		public int? GetInstallTypeRegistryValue()
		{
			using (var installTypeKey = OpenBaseRegistryToRead())
			{
				try
				{
					var obj = installTypeKey == null ? -1 : installTypeKey.GetValue(installTypeRegistryKey);
					return int.Parse(obj.ToString());
				}
				catch (Exception exc)
				{
					logger.Error(exc.ToString());
					return null;
				}
			}
		}

		public int GetLicenseTermDaysRegistryValue()
		{
			using (var installTypeKey = OpenBaseRegistryToRead())
			{
				try
				{
					var obj = installTypeKey == null ? 0 : installTypeKey.GetValue(licenseTermDaysKey);
					if (obj == null)
					{
						return 0;
					}
					return int.Parse(obj.ToString());
				}
				catch (Exception exc)
				{
					logger.Error(exc.ToString());
					return 0;
				}
			}
		}

		public string GetErrorStringFromActivateStatus(ActivateStatus status)
		{
			switch(status)
			{
				case ActivateStatus.PromocodeUsed:
					return Application.Current.FindResource("PromoCodeUsedError").ToString();
				case ActivateStatus.InvalidPromocode:
				case ActivateStatus.InvalidArgument:
					return Application.Current.FindResource("PromoCodeError").ToString();
			}

			return "";
		}
		
		public ActivateStatus Activate(string code)
		{
			if (string.IsNullOrWhiteSpace(code))
			{
				logger.Error("Activate failed - empty promocode");
				return ActivateStatus.InvalidArgument;
			}

			var request = new SubmitPromoCodeRequest
			{
				promotion = code.Trim()
			};
            var response = webAPI.SubmitPromoCode(request, pbData.ActiveUser + "|" + pbData.DeviceUUID);

			if (response == null || response.error != null)
			{
				
				if (response.error.code == 400)
				{
					return ActivateStatus.PromocodeUsed;
				}
				else
				{
					return ActivateStatus.InvalidPromocode;
				}
			}

			return ActivateStatus.Success;
		}

		public bool GetPremium()
		{
			try
			{
				//Application.Current.Dispatcher.BeginInvoke((Action)(() =>
				//{
				//	try
				//	{
				//		string uri = pbData.GetPrivateSetting(DefaultProperties.Configuration_Purchase_Url_InApp);
				//		Dictionary<string, object> param = new Dictionary<string, object>();
				//		param.Add("url", uri);
				//		((IAppCommand)System.Windows.Application.Current).ExecuteCommand("OpenUrlInSecureBrowser", param);
				//	}
				//	catch (Exception ex)
				//	{
				//		logger.Error(ex.ToString());
				//	}
				//}));
				string uri = pbData.GetPrivateSetting(DefaultProperties.Configuration_Purchase_Url_Discount);
				BrowserHelper.OpenInDefaultBrowser(new Uri(uri));
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
			return false;
		}

		private static RegistryKey OpenBaseRegistryToRead()
		{
			return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\PasswordBoss", false);
		}
	}
}
