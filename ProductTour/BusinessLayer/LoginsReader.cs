using PasswordBoss;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ProductTour.Models;
using System;

namespace ProductTour.BusinessLayer
{
	public interface ILoginsReader
	{
		ScanResult ScanBrowsers();
		bool IsScanCompleted { get; }
	}

	public class LoginsReader : ILoginsReader
	{
		private readonly ILogger logger = Logger.GetLogger(typeof(LoginsReader));
		private readonly IPBData data;
		private static readonly IRiskCalculator riskCalculator = new RiskCalculator();
		protected static readonly IRegistryManager registryManager = new RegistryManager();

		private static object scanLocker = new object();
		public bool IsScanCompleted { get; private set; }

		public LoginsReader(IPBData pbData)
		{
			this.data = pbData;
		}

		public ScanResult ScanBrowsers()
		{
			var res = ScanBrowsersInternal();
			registryManager.PutScanSummaryToRegistry(res);
			return res;
		}

		private ScanResult ScanBrowsersInternal()
		{
			lock (scanLocker)
			{
				IsScanCompleted = false;
				try
				{
					var scanItems = ReadLoginsFromBrowsers()
						.Select(login => ScanItem.FromLogin(login))
						.ToArray();
					scanItems = riskCalculator.MarkDuplicates(scanItems);

                    return new ScanResult(scanItems);
				}
				catch (Exception ex)
				{
					logger.Error(ex.ToString());
					return new ScanResult();
				}
				finally
				{
					IsScanCompleted = true;
				}
			}
		}

		private List<LoginInfo> ReadLoginsFromBrowsers()
		{
			List<LoginInfo> chromeLoginInfo = new List<LoginInfo>();
			List<LoginInfo> ieLoginInfo = new List<LoginInfo>();
			List<LoginInfo> ffLoginInfo = new List<LoginInfo>();

			Parallel.Invoke(() =>
			{
				//loading chrome items parallel
				if (PasswordBoss.Browsers.BrowserVersionGetter.GetChromeVersion() != null)
				{
					//if (!BrowserHelper.IsChromeOpened)
					//{
					var tmploginInfo = data.GetChromeAccounts();
					chromeLoginInfo.AddRange(tmploginInfo);
					//}
				}
			}
			, () =>
			{
				if (PasswordBoss.Browsers.BrowserVersionGetter.GetIEVersion() != null)
				{
					//if (!BrowserHelper.IsIEOpened)
					//{
					var tmploginInfo = data.GetIEAccounts();
					ieLoginInfo.AddRange(tmploginInfo);
					//}
				}

			}
			, () =>
			{
				if (PasswordBoss.Browsers.BrowserVersionGetter.GetFFVersion() != null)
				{
					//if (!BrowserHelper.IsFFOpened)
					//{
					var tmploginInfo = data.GetFFAccounts(() => { return null; });
					ffLoginInfo.AddRange(tmploginInfo);
					//}
				}
			});

			var res = new List<LoginInfo>();
			res.AddRange(chromeLoginInfo);
			res.AddRange(ffLoginInfo);
			res.AddRange(ieLoginInfo);

			return res;
		}
	}
}