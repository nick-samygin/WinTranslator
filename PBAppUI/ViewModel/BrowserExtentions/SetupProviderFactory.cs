
using PasswordBoss.Browsers;
using System;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public static class SetupProviderFactory
	{
		public static SetupProviderBase GetDefaultBrowser()
		{
			var installedBrowsers = BrowserGetter.GetInstalledBrowsers();
			if (installedBrowsers.Contains(BrowsersFlag.Chrome))
			{
				return new ChromeSetupProvider();
			}

			if (installedBrowsers.Contains(BrowsersFlag.InternetExplorer))
			{
				return new InternetExplorerSetupProvider();
			}

			if (installedBrowsers.Contains(BrowsersFlag.Firefox))
			{
				return new FirefoxSetupProvider();
			}

			if (installedBrowsers.Contains(BrowsersFlag.Opera))
			{
				return new OperaSetupProvider();
			}


			return null;
        }	
	}
}
