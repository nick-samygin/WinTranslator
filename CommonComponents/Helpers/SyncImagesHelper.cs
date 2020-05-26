using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordBoss.Helpers
{
	public class SyncImagesHelper
	{
		private static readonly ILogger logger = Logger.GetLogger(typeof(SyncImagesHelper));
		private IPBWebAPI webApi;
		private IPBData pbData;
		private static object lck = new object();

		public SyncImagesHelper(IPBData pbData, IPBWebAPI webApi)
		{
			this.pbData = pbData;
			this.webApi = webApi;
		}
		private bool IsImageForSiteExisting(string siteId)
		{
			bool isExisting = false;

			if (siteId != null)
			{
				string imageDirectory = AppHelper.ImageFolderLocation; //DefaultProperties.CurrentImageDirectory + "\\";
				string imageFileName = null;
				if (siteId != null)
				{
					imageFileName = imageDirectory + siteId + ".png";
				}

				isExisting = File.Exists(imageFileName);
			}

			return isExisting;
		}

		public void InsertImageIfNeeded(string site_id, string site_uuid, string imageUrl, bool forceImageDownload = false)
		{

			if (site_uuid != null && (forceImageDownload || !IsImageForSiteExisting(site_uuid)))
			{
				string imageDirectory = AppHelper.ImageFolderLocation;
				webApi.DownloaLogo(imageUrl, imageDirectory + site_uuid + ".png");
			}

		}
		public void SyncImagesAsync()
		{
			System.Threading.Tasks.Task.Factory.StartNew(() =>
			{
				SyncImages();
			});
		}
		public void SyncImages()
		{
			logger.Debug("Sync Images start");
			try
			{
				lock (lck)
				{
					if (pbData.Locked)
					{
						logger.Debug("Sync Images quits - database locked");
						return; //don't do anything
					}
					//Debugger.Launch();
					DateTime lastImageSyncDate = DateTime.MinValue;
					string lastImageSyncDateStr = pbData.GetPrivateSetting(DefaultProperties.LastImageSyncDate);
					DateTime.TryParse(lastImageSyncDateStr, out lastImageSyncDate);

					var recommendedSites = pbData.GetRecommendedSites();
					Parallel.ForEach(recommendedSites, (recommendedSite) =>
					{
						try
						{
							bool forceDownload = false;
							if (recommendedSite.Image != null && recommendedSite.Image.LastModifiedDate > lastImageSyncDate)
							{
								forceDownload = true;
							}
							if (recommendedSite.Image != null)
							{
								InsertImageIfNeeded(recommendedSite.Id, recommendedSite.UUID, recommendedSite.Image.Url, forceDownload);
							}
						}
						catch (Exception ex)
						{
							logger.Error("Insert recommendedSites item failed {0}", ex.ToString());
						}
					});
					logger.Debug("Got {0} Recommented sites", recommendedSites.Count);

					var secureItems = pbData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
					if (secureItems == null)
						return;

					logger.Debug("Got {0} secure items", secureItems.Count);

					Parallel.ForEach(secureItems, (secureItem) =>
					{
						try
						{
							if (secureItem.Site != null && secureItem.Site.Image != null)
							{
								bool forceDownload = false;
								if (secureItem.ImageLastModifiedDate.GetValueOrDefault(secureItem.LastModifiedDate) >= lastImageSyncDate)
								{
									forceDownload = true;
								}
								InsertImageIfNeeded(secureItem.Site.Id, secureItem.Site.UUID, secureItem.Site.Image.Url, forceDownload);
							}
						}
						catch (Exception ex)
						{
							logger.Error("Insert secure item failed {0}", ex.ToString());
						}
					});

					logger.Debug("Images inserted");

					pbData.ChangePrivateSetting(DefaultProperties.LastImageSyncDate, DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
				}

			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
			finally
			{
				logger.Debug("Sync images done");
			}
		}
	}
}
