using System;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public class ChromeSetupProvider : SetupProviderBase
	{
		public override string BrowserFullName
		{
			get
			{
				return GetResourceString("Chrome");
            }
		}

		public override string BrowserIcon
		{
			get
			{
				return "/image;component/images/BrowserExtensions/chrome-42x42.png";
			}
		}

		public override string BrowserScreenshot
		{
			get
			{
				return "/image;component/images/BrowserExtensions/browser-button-chrome-example.png";
            }
		}

		public override string BrowserShortName
		{
			get
			{
				return GetResourceString("Chrome");
			}
		}

		public override string ExtentionGetLinkUrl
		{
			get
			{
				//different links, regarding is Chrome Extension installed or not
				if (BrowserHelper.IsChromeExtInstalled)
				{
					return @"https://www.passwordboss.com/getting-started/manage-chrome-extension";
				}
				else
				{
					return @"https://www.passwordboss.com/install/ch/?utm_source=Menu&utm_medium=Chrome&utm_campaign=InstallBHO";
				}
			}
		}
	}
}
