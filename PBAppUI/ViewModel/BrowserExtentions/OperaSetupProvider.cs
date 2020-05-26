using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public class OperaSetupProvider : SetupProviderBase
	{
		private static string browserName = GetResourceString("Opera");

		public override string BrowserShortName
		{
			get { return browserName; }
		}

		public override string BrowserFullName
		{
			get { return browserName; }
		}

		public override string ExtentionGetLinkUrl
		{
			get
			{
				return @"https://www.passwordboss.com/getting-started/opera/";
			}
		}

		public override string BrowserIcon
		{
			get
			{
				return "/image;component/images/opera-small.png";
			}
		}

		public override string BrowserScreenshot
		{
			get
			{
				return "/image;component/images/opera-screenshot.png";
			}
		}

		public override void OnBeforeSetup()
		{
			base.OnBeforeSetup();
			base.KillBrowser("opera");
		}	
	}
}