using System;
using System.Diagnostics;
using System.Threading;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public class FirefoxSetupProvider : SetupProviderBase
	{
		private readonly string browserName = GetResourceString("Firefox");
		public override string BrowserFullName
		{
			get
			{
				return browserName;
			}
		}

		public override string BrowserIcon
		{
			get
			{
				return "/image;component/images/BrowserExtensions/firefox-42x42.png";
            }
		}

		public override string BrowserScreenshot
		{
			get
			{
				return "/image;component/images/BrowserExtensions/browser-button-firefox-example.png";
            }
		}

		public override string BrowserShortName
		{
			get
			{
				return browserName;
			}
		}

		public override string ExtentionGetLinkUrl
		{
			get
			{
				return @"https://www.passwordboss.com/firefox-installed/?utm_source=PC&utm_medium=menu&utm_campaign=InstallBrowserExtension&Prompt=true";
            }
		}

		public override void OnBeforeSetup()
		{
			base.OnBeforeSetup();
			base.KillBrowser("firefox");
		}
	}
}