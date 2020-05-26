using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordBoss.BusinessLayer
{
	public class CreateAccountLayer
	{
		public EventHandler<BussinessLayerMessageEventArgs> MessageRaised;
		public EventHandler VerificationRequired;
		public EventHandler BackToLoginScreenRequired;
		public EventHandler SetupCompleteRequired;

		private static readonly ILogger logger = Logger.GetLogger(typeof(CreateAccountLayer));
		private IResolver resolver = null;
		private IPBData pbData = null;
		private IInAppAnalytics inAppAnalyitics = null;
		private IPBWebAPI webApi = null;

		private static readonly string accountCreatedResponse_UnknownDevice = "400";
		private static readonly string deviceRegistrationResponse_UnverifiedDevice = "403";
		private readonly GenerateKeysStepResult generateKeysStep;

		private readonly Action<MarketingActionType> logStep;

		public CreateAccountLayer(IResolver resolver, Action<MarketingActionType> logStep)
		{
			this.resolver = resolver;

			this.logStep = logStep;

			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.webApi = resolver.GetInstanceOf<IPBWebAPI>();
			inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();

			generateKeysStep = new GenerateKeysStep().Execute();
		}
		//public void StartBackgroundTasks()
		//{
		//	generateKeysStep.ExecuteAsync();
		//}

		//public void CancelBackgroundTasks()
		//{
		//	try
		//	{
		//		generateKeysStep.CancellationTokenSource.Cancel();
		//	}
		//	catch (Exception ex)
		//	{
		//		logger.Debug(ex.ToString());
		//	}
		//}

		public Tuple<bool, bool> AuthenticateUser(string userEmailId, string password)
		{
			var profileExists = false;
			var isAuthenticated = pbData.AuthenticateUser(userEmailId, password, out profileExists);			

			if (!profileExists)
			{
				profileExists = CheckAccount(userEmailId);
			}

			if (profileExists)
			{
				//CancelBackgroundTasks();
			}

			return new Tuple<bool, bool>(isAuthenticated, profileExists);
		}

		private bool CheckAccount(string email)
		{
			dynamic twoStepResp = webApi.CheckAccount(email);
			return twoStepResp != null && twoStepResp.error == null;
		}

		public void CreateAccount(string email, string password)
		{
			try
			{
				logger.Debug("Started account request and creating rsaKeys");
				if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
				{
					OnMessageRaised("Error retreiving account information");
					return;
				}


				// thread 1
				byte[] publicKeyPem = null;
				ProtectedDataBlock privateKeyPem = null;
				dynamic accountResponse = null;

				{
					//var r = generateKeysStep.Result;
					var r = generateKeysStep;
					if (r.IsError)
						return;

					publicKeyPem = r.PublicKey;
					privateKeyPem = r.PrivateKey;
				}

				IPBWebAPI webAPI = resolver.GetInstanceOf<IPBWebAPI>();
				logger.Debug("perform request");

				accountResponse = webAPI.RequestAccount(new WEBApiJSON.AccountRequest()
				{
					email = email,
					language = "English",
					installation = pbData.InstallationUUID,
					public_key = Convert.ToBase64String(publicKeyPem)
				});

				if (accountResponse == null)
				{
					OnMessageRaised("Error in account registration");
					return;
				}

				// if create account web request returned UnknownDevice error
				if (IsError(accountResponse, accountCreatedResponse_UnknownDevice))
				{
					string uuid = "";
					if (TryToRegisterDevice(email, out uuid))
					{
						if (BackToLoginScreenRequired != null)
						{
							BackToLoginScreenRequired(this, EventArgs.Empty);
						}
					}
					logger.Debug("Try to register device called");
				}

				logger.Debug("request account task done");

				CreateProfile(email, password, privateKeyPem, publicKeyPem);

				logger.Debug("Profile created");

				var performInitialSyncTask = new Task(() =>
			   {
				   logger.Debug("Performing initial sync");
				   PerformInitialSync();
				   logger.Debug("Initial sync performed");
			   });

				var setDefaultSettingsTask = new Task(() =>
				{
					SetDefaultSettings(pbData);
					logger.Debug("Default settings set");
				});

				performInitialSyncTask.Start();
				setDefaultSettingsTask.Start();

				Task.WaitAll(performInitialSyncTask, setDefaultSettingsTask);

				logStep(MarketingActionType.Continue);

				// Added dispatcher because background worker can't create new UI elements
				//Application.Current.Dispatcher.Invoke((Action)delegate
				//{
				//    var nextScreen = new SetupComplete(resolver);// resolver.GetInstanceOf<SetupComplete>();

				//    Navigator.NavigationService.Navigate(nextScreen);
				//});

				if (SetupCompleteRequired != null)
					SetupCompleteRequired(this, EventArgs.Empty);

			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		private void CreateProfile(string email, string password, ProtectedDataBlock privateKeyPem, byte[] publicKeyPem)
		{
			logger.Debug("Creating profile");
			if (!pbData.CreateProfile(email, password))
			{
				OnMessageRaised("Error while creating secure database");
			}

			pbData.AddUserInfo(new DTO.UserInfo()
			{
				Email = email,
				RSAPrivateKey = privateKeyPem,
				PublicKey = publicKeyPem
			});
		}

		#region Sync

		private void PerformInitialSync()
		{
			try
			{
				//if not default device goto verification
				string uuid = "";
				if (!TryToRegisterDevice(pbData.ActiveUser, out uuid))
				{
					return;
				}

				pbData.DeviceUUID = uuid;
				Guid g;
				if (!Guid.TryParse(pbData.DeviceUUID, out g))
				{
					OnMessageRaised("Invalid device ID");
					return;
				}
				logger.Debug("Adding device");
				if (pbData.AddDevice(
					new DTO.Device() { InstallationId = pbData.InstallationUUID, UUID = pbData.DeviceUUID, Nickname = System.Windows.Forms.SystemInformation.ComputerName }) == null)
				{
					OnMessageRaised("Failed to save device data");
					return;
				}

				evDone.Reset();
				IPBSync sync = resolver.GetInstanceOf<IPBSync>();
				//sync.OnGetMergePassword(GetMergePassword);
				sync.OnSyncFinished += sync_OnSyncFinished;
				Task.Factory.StartNew(() =>
				{
					var sw = Stopwatch.StartNew();
					try
					{
						if (!sync.Sync(3, ProgressInfo))
						{
							logger.Error("Initial sync failed");
						}
						else
						{

						}
					}
					catch (Exception ex)
					{
						logger.Error(ex.Message);
					}
					finally
					{
						var str = sw.Elapsed.ToString();
						logger.Info("Sync elasped {0}", str);
						evSyncDone.Set();
					}

				});

				evSyncDone.WaitOne();

				evDone.WaitOne();
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		private AutoResetEvent evSyncDone = new AutoResetEvent(false);

		private ManualResetEvent evDone = new ManualResetEvent(false);

		void ProgressInfo(int currentStep, int totalNumberOfSteps)
		{
			//Treba dodati wait dijalog sa progresom
		}

		void sync_OnSyncFinished(bool status)
		{
			try
			{
				IPBSync sync = resolver.GetInstanceOf<IPBSync>();
				sync.OnSyncFinished -= sync_OnSyncFinished;
				List<LoginInfo> chromeLoginInfo = new List<LoginInfo>();
				List<LoginInfo> ieLoginInfo = new List<LoginInfo>();
				List<LoginInfo> ffLoginInfo = new List<LoginInfo>();

				Parallel.Invoke(() =>
				{
					//loading chrome items parallel
					if (Browsers.BrowserVersionGetter.GetChromeVersion() != null)
					{
						//if (!BrowserHelper.IsChromeOpened)
						//{
						var tmploginInfo = pbData.GetChromeAccounts();
						chromeLoginInfo.AddRange(tmploginInfo);
						//}
					}
				}
				, () =>
				{
					if (Browsers.BrowserVersionGetter.GetIEVersion() != null)
					{
						//if (!BrowserHelper.IsIEOpened)
						//{
						var tmploginInfo = pbData.GetIEAccounts();
						ieLoginInfo.AddRange(tmploginInfo);
						//}
					}

				}
				, () =>
				{
					if (Browsers.BrowserVersionGetter.GetFFVersion() != null)
					{
						//if (!BrowserHelper.IsFFOpened)
						//{
						var tmploginInfo = pbData.GetFFAccounts(() => { return null; });
						ffLoginInfo.AddRange(tmploginInfo);
						//}
					}

				});

				SecureItemHelper siHelper = new SecureItemHelper(pbData, sync);
				int ieImportedSitesNum = 0;
				int ffImportedSitesNum = 0;
				int chromeImportedSitesNum = 0;
				int ieAlredyImportedSitesNum = 0;
				int ffAlredyImportedSitesNum = 0;
				int chromeAlredyImportedSitesNum = 0;
				int alredyImportedSitesNum = 0;

				List<SecureItem> userSites = pbData.GetSecureItemsByItemType(PasswordBoss.Helpers.DefaultProperties.SecurityItemType_PasswordVault).Where(x => x.Site.IsRecommendedSite == false).ToList();
				logger.Debug("Importing sites from browsers - Start");

				siHelper.ImportLoginInfoList(chromeLoginInfo.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Distinct().ToList(), ref chromeImportedSitesNum, ref chromeAlredyImportedSitesNum);
				siHelper.ImportLoginInfoList(ieLoginInfo.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Distinct().ToList(), ref ieImportedSitesNum, ref ieAlredyImportedSitesNum);
				siHelper.ImportLoginInfoList(ffLoginInfo.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Distinct().ToList(), ref ffImportedSitesNum, ref ffAlredyImportedSitesNum);

                logger.Debug("Importing sites from browsers - Finish");

                var importedSitesNum = ieImportedSitesNum + ffImportedSitesNum + chromeImportedSitesNum;
				
				if (userSites.Count == 0)
				{
					alredyImportedSitesNum = 0; //we didn't have anything previous. alredyImportedSitesNum can also mean overlapping between browsers
				}
				else
				{
					alredyImportedSitesNum = ieAlredyImportedSitesNum + ffAlredyImportedSitesNum + chromeAlredyImportedSitesNum;
				}

				var inAppAnalyitics = resolver.GetInstanceOf<IInAppAnalytics>();
				if (inAppAnalyitics != null)
				{
					ImportPasswordsItem item = new ImportPasswordsItem(importedSitesNum, ImportPasswordsTrigger.Installer, null);
					var analytics = inAppAnalyitics.Get<Events.ImportPasswords, ImportPasswordsItem>();
					analytics.Log(item);
				}

				pbData.ChangePrivateSetting("setup_wizard_imported_passwords_number", importedSitesNum.ToString());
				pbData.ChangePrivateSetting("setup_wizard_already_passwords_number", alredyImportedSitesNum.ToString());

				logger.Debug("private settings modified");
				AddEmailToPI();
				logger.Debug("added email to PI");

				SyncImagesHelper syncImages = new SyncImagesHelper(pbData, webApi);
				syncImages.SyncImages();				

				logger.Debug("Finished");
			}
			finally
			{
				evDone.Set();
			}
		}

		private void OnMessageRaised(string message)
		{
			if (MessageRaised != null)
			{
				MessageRaised(this, new BussinessLayerMessageEventArgs(message));
			}
		}

		private bool IsError(dynamic response, string errorCode)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			Func<Func<dynamic>, bool> compare = (dErrorFieldSelector) =>
			{
				try
				{
					dynamic d = dErrorFieldSelector();
					return d == errorCode;
				}
				catch
				{
					return false;
				}
			};

			return compare(() => response.error) || compare(() => response.error.code);
		}

		private bool TryToRegisterDevice(string email, out string uuid)
		{
			IPBWebAPI webAPI = resolver.GetInstanceOf<IPBWebAPI>();
			uuid = "";

			//try to register device
			logger.Debug("register device request");
			dynamic deviceRegistrationResponse = webAPI.RegisterDevice(new WEBApiJSON.DeviceRegistrationRequest()
			{
				installation = pbData.InstallationUUID,
				nickname = Environment.MachineName,
				software_version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
			}, email);

			if (deviceRegistrationResponse == null)
			{
				OnMessageRaised("Error in device registration");
				return false;
			}
			else
			{
				try
				{
					uuid = deviceRegistrationResponse.devices[0].uuid.ToString();
				}
				catch
				{
					uuid = "";
				}
				// if api returned verification request for this device - 
				if (IsError(deviceRegistrationResponse, deviceRegistrationResponse_UnverifiedDevice))
				{
					//send verification code for new device
					dynamic verificationRequestResponse = webAPI.RequestVerificationCode(email);
					if (VerificationRequired != null)
						VerificationRequired(this, EventArgs.Empty);
					return false;
				}
			}

			return true;
		}

		private void AddEmailToPI()
		{
			
			SecureItem secureItem = new SecureItem()
			{
				SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
				Name = pbData.ActiveUser,
				Type = DefaultProperties.SecurityItemSubType_PI_Email,
                Order = 0,
				Data = new SecureItemData()
				{
					email = pbData.ActiveUser,
					notes = ""
				}
			};

			bool isExisting = pbData.IsSecureItemExistingBySimpleRule(pbData.ActiveUser, SecurityItemsDefaultProperties.SecurityItemSubType_PI_Email);

			if (!isExisting)
			{
				pbData.AddOrUpdateSecureItem(secureItem);
			}
		}

		#endregion


		private static void SetDefaultSettings(IPBData pbData)
		{
			Parallel.Invoke(() =>
				{
					string tmpCloudEnabled = pbData.GetPrivateSetting(DefaultProperties.Settings_CloudStorage);
					if (tmpCloudEnabled == null)
					{
						pbData.ChangePrivateSetting(DefaultProperties.Settings_CloudStorage, bool.TrueString);
					}
				}, () =>
				{
					var rememberLoginEmail = pbData.GetConfigurationByAccountAndKey(pbData.ActiveUser, DefaultProperties.Configuration_Key_RememberEmail);
					if (rememberLoginEmail == null)
					{
						rememberLoginEmail = new Configuration()
						{
							AccountEmail = pbData.ActiveUser,
							Key = DefaultProperties.Configuration_Key_RememberEmail,
							Value = true.ToString()
						};
						pbData.AddOrUpdateConfiguration(rememberLoginEmail);
					}

					bool isEnabledRememberLoginEmail = false;
					if (bool.TryParse(rememberLoginEmail.Value, out isEnabledRememberLoginEmail) && isEnabledRememberLoginEmail)
					{

						var lastLogin = new Configuration()
						{
							AccountEmail = DefaultProperties.Configuration_DefaultAccount,
							Key = DefaultProperties.Configuration_Key_LastLoginEmail,
							Value = pbData.ActiveUser
						};
						pbData.AddOrUpdateConfiguration(lastLogin);
					}
				}, () =>
				{
					string autoLogin = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin);
					if (autoLogin == null)
					{
						pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoLogin, bool.TrueString);
					}
				}, () =>
				{
					string passwordSavingInBrowser = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving);
					if (passwordSavingInBrowser == null)
					{
						pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_TurnOffPassSaving, bool.TrueString);
					}
				}, () =>
				{
					string autoStoreData = pbData.GetPrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData);
					if (autoStoreData == null)
					{
						pbData.ChangePrivateSetting(DefaultProperties.Settings_Advanced_AutoStoreData, bool.TrueString);
					}
				}, () =>
				{
                    string currentSetupProgressStep = pbData.GetPrivateSetting(DefaultProperties.Settings_SetupProgress_CurrentStep);
					if (currentSetupProgressStep != null)
					{
                        pbData.ChangePrivateSetting(DefaultProperties.Settings_SetupProgress_CurrentStep, DefaultProperties.Settings_SetupProgress_DefaultStep);
					}
				},() =>
                {
                var userInfo = pbData.GetUserInfo(pbData.ActiveUser);
                if (userInfo != null && userInfo.StorageRegionUUID == null)
                {
                    var region = pbData.GetStorageRegions().FirstOrDefault();

                    if (region != null)
                    {
                        pbData.UpdateCurrentStorageRegion(region.UUID);
                    }
                }
            });
		}

	}
}
