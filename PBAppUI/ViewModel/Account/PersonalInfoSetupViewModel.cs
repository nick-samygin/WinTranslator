using Newtonsoft.Json;
using PasswordBoss.Analytics;
using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.PBAnalytics;
using PasswordBoss.Views;
using System;
using System.Windows;

namespace PasswordBoss.ViewModel.Account
{

	public class PersonalInfoSetupViewModel : ViewModelBase, IAnalyticsLogger
	{
		private readonly ILogger logger = Logger.GetLogger(typeof(PersonalInfoSetupViewModel));
		private const string PI_NAME_KEY = "setup_wizard_PI_name";
		private const string PI_ADDRESS_KEY = "setup_wizard_PI_address";

		private readonly PersonalInfoSetupAnalyticsLogger mixpanelLogger;

		private readonly IResolver resolver;
		private readonly IPBData pbData;
		private readonly IPBWebAPI api;
		public PersonalInfoSetupViewModel(IResolver resolver)
		{
			this.resolver = resolver;
			this.pbData = resolver.GetInstanceOf<IPBData>();
			this.api = resolver.GetInstanceOf<IPBWebAPI>();
			this.mixpanelLogger = new PersonalInfoSetupAnalyticsLogger(resolver.GetInstanceOf<IInAppAnalytics>());

			this.CloseCommand = new RelayCommand(CloseCommandExecute);
			this.ContinueCommand = new RelayCommand(ContinueCommandExecute);

			email = pbData.ActiveUser;
		}

		#region Properties
		private string email;
		public string Email { get { return email; } set { email = value; RaisePropertyChanged("Email"); } }

		private string firstName;
		public string FirstName { get { return firstName; } set { firstName = value; RaisePropertyChanged("FirstName"); } }

		private string lastName;
		public string LastName { get { return lastName; } set { lastName = value; RaisePropertyChanged("LastName"); } }

		private string address;
		public string Address { get { return address; } set { address = value; RaisePropertyChanged("Address"); } }

		private string city;
		public string City { get { return city; } set { city = value; RaisePropertyChanged("City"); } }

		private string state;
		public string State { get { return state; } set { state = value; RaisePropertyChanged("State"); } }


		public string[] States
		{
			get
			{
				return new string[]
				{
					"Alabama" ,
					"Alaska" ,
					"American Samoa" ,
					"Arizona" ,
					"Arkansas" ,
					"California" ,
					"Colorado" ,
					"Connecticut" ,
					"Delaware" ,
					"District Of Columbia" ,
					"Federated States Of Micronesia" ,
					"Florida" ,
					"Georgia" ,
					"Guam" ,
					"Hawaii" ,
					"Idaho" ,
					"Illinois" ,
					"Indiana" ,
					"Iowa" ,
					"Kansas" ,
					"Kentucky" ,
					"Louisiana" ,
					"Maine" ,
					"Marshall Islands" ,
					"Maryland" ,
					"Massachusetts" ,
					"Michigan" ,
					"Minnesota" ,
					"Mississippi" ,
					"Missouri" ,
					"Montana" ,
					"Nebraska" ,
					"Nevada" ,
					"New Hampshire" ,
					"New Jersey" ,
					"New Mexico" ,
					"New York" ,
					"North Carolina" ,
					"North Dakota" ,
					"Northern Mariana Islands" ,
					"Ohio" ,
					"Oklahoma" ,
					"Oregon" ,
					"Palau" ,
					"Pennsylvania" ,
					"Puerto Rico" ,
					"Rhode Island" ,
					"South Carolina" ,
					"South Dakota" ,
					"Tennessee" ,
					"Texas" ,
					"Utah" ,
					"Vermont" ,
					"Virgin Islands" ,
					"Virginia" ,
					"Washington" ,
					"West Virginia" ,
					"Wisconsin" ,
					"Wyoming"
				};
			}
		}

		private string zip;
		public string Zip { get { return zip; } set { zip = value; RaisePropertyChanged("Zip"); } }

		#endregion

		#region commands
		public RelayCommand CloseCommand { get; set; }

		public RelayCommand ContinueCommand { get; set; }

		private void CloseCommandExecute(object obj)
		{
			LogStep(MarketingActionType.Close);
			ShowNextScreen();
		}

		private void ContinueCommandExecute(object obj)
		{
			LogStep(MarketingActionType.Continue);
			StoreInput();
			ShowNextScreen();
		}

		#endregion

		#region private methods

		private void ShowNextScreen()
		{
			var loginWindow = ((PBApp)Application.Current).FindWindow<LoginWindow>();
			loginWindow.NavigateloginScreens(LoginWindow.ScreenNames.BrowserExtention);
		}


		private void StoreInput()
		{
			if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
			{

				var siName = new SecureItem() {
                    SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo,
                    Type = DefaultProperties.SecurityItemSubType_PI_Names };

				try
				{
					if (siName.Data == null)
						siName.Data = new SecureItemData();
					siName.Data.firstName = firstName;
					siName.Data.lastName = lastName;
					siName.Name = String.Format("{0} {1}", firstName, lastName);
					//siName.Category = new Category() { Id = DefaultCategories.CategoryNames };

					siName = pbData.AddOrUpdateSecureItem(siName);
					pbData.ChangePrivateSetting(PI_NAME_KEY, JsonConvert.SerializeObject(siName, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

					UserInfo ui = pbData.GetUserInfo(pbData.ActiveUser);
					ui.FirstName = firstName;
					ui.LastName = lastName;
					pbData.UpdateUserInfo(ui);

					api.UpdateAccount(pbData.ActiveUser + "|" + pbData.DeviceUUID, new PasswordBoss.WEBApiJSON.AccountRequest { first_name = firstName, last_name = lastName });

				}
				catch (Exception ex)
				{
					logger.Error(ex.ToString());
				}
			}

			if (!string.IsNullOrEmpty(address))
			{

				var siAddress = new SecureItem() { SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo, Type = DefaultProperties.SecurityItemSubType_PI_Address };
				try
				{
					siAddress.SecureItemTypeName = DefaultProperties.SecurityItemType_PersonalInfo;
					siAddress.Type = DefaultProperties.SecurityItemSubType_PI_Address;
					siAddress.Name = address;
					if (siAddress.Data == null)
						siAddress.Data = new SecureItemData();

					siAddress.Data.address1 = address;
					siAddress.Data.city = city;
					siAddress.Data.zipCode = zip;
					siAddress.Data.state = state;					

					// TODO: are we need it?
					//siAddress.Data.country = PIAddressCountry.Value.Key;
					siAddress = pbData.AddOrUpdateSecureItem(siAddress);
					pbData.ChangePrivateSetting(PI_ADDRESS_KEY, JsonConvert.SerializeObject(siAddress, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
				}
				catch (Exception ex)
				{
					logger.Error(ex.ToString());
				}
			}
		}


		#endregion

		public void LogStep(MarketingActionType action)
		{
			mixpanelLogger.LogStep(action);
		}
	}	
}
