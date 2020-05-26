using System;
using System.Diagnostics;
using System.Threading;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public class InternetExplorerSetupProvider : SetupProviderBase
	{
		private static string browserName = GetResourceString("InternetExplorer");

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
				return "/image;component/images/BrowserExtensions/ie-42x42.png";
            }
		}

		public override string BrowserScreenshot
		{
			get
			{
				return "/image;component/images/BrowserExtensions/browser-button-ie-example.png";
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
				return @"http://www.passwordboss.com/internet-explorer-installed/?utm_source=PC&utm_medium=menu&utm_campaign=InstallBrowserExtension&Prompt=true";
            }
		}

		public override void OnBeforeSetup()
		{
			base.OnBeforeSetup();
			base.KillBrowser("iexplorer");
		}
	}

}
