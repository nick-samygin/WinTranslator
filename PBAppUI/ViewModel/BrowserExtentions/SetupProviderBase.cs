using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace PasswordBoss.ViewModel.BrowserExtentions
{
	public abstract class SetupProviderBase
	{
		protected readonly ILogger logger;
		public abstract string BrowserShortName { get; }
		public abstract string BrowserFullName { get; }
		public abstract string ExtentionGetLinkUrl { get; }
		public abstract string BrowserIcon { get; }
		public abstract string BrowserScreenshot { get; }

		public SetupProviderBase()
		{
			logger = Logger.GetLogger(this.GetType());
		}

		protected static string GetResourceString(string name)
		{
			return (string)System.Windows.Application.Current.FindResource(name);
		}

		public virtual void OnBeforeSetup()
		{

		}

		public virtual void OnAfterSetup()
		{

		}

		protected void KillBrowser(string browserName)
		{
			foreach (var p in Process.GetProcessesByName(browserName))
			{
				try
				{
					p.CloseMainWindow();
				}
				catch(Exception ex)
				{
					logger.Error(ex.ToString());
				}
			}
			Thread.Sleep(500);
			foreach (var p in Process.GetProcessesByName(browserName))
			{
				try
				{
					p.Kill();
				}
				catch (Exception ex)
				{
					logger.Error(ex.ToString());
				}
			}
		}
	}
}
