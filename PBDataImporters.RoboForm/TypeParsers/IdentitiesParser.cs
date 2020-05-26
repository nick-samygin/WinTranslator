using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.RoboForm.TypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class IdentitiesParser : TypeParserBase<RoboFormData>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] 
			{
				RoboformExportType.Identities.ToString()
			};
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, RoboFormData rdata)
		{
			if (rdata == null)
				throw new ArgumentNullException("rdata");

			if (secureItems == null)
				throw new ArgumentNullException("secureItems");

			if (messages == null)
				throw new ArgumentNullException("messages");

			if (rdata.IsDictionary)
			{
				if (rdata.Caption == "Person")
				{
					PBSubType.PersonalInfo.Name identityNameData = new PBSubType.PersonalInfo.Name();

					identityNameData.FirstName = Helpers.GetValue(rdata.Data, "Name");

					secureItems.Add(identityNameData.GetSecureItem());

					TryToProcessPhoneNumber("Phone", secureItems, rdata);
					TryToProcessPhoneNumber("Home Tel", secureItems, rdata);
					TryToProcessPhoneNumber("Work Tel", secureItems, rdata);

					TryToProcessPhoneNumber("Cell Tel", secureItems, rdata);
					TryToProcessPhoneNumber("Fax", secureItems, rdata);
					TryToProcessPhoneNumber("Work Tel", secureItems, rdata);
					
					if (rdata.Data.ContainsKey("Email"))
					{
						PBSubType.PersonalInfo.Email identityEmail = new PBSubType.PersonalInfo.Email();
						identityEmail.NickName = Helpers.GetValue(rdata.Data, "Name");
						identityEmail.Address = Helpers.GetValue(rdata.Data, "Email");
						secureItems.Add(identityEmail.GetSecureItem());
					}
					if (rdata.Data.ContainsKey("Driver Lic"))
					{
						PBSubType.PersonalInfo.DriverLicense identityDriverLic = new PBSubType.PersonalInfo.DriverLicense();
						identityDriverLic.LicenseNickname = rdata.IdentityName;
						identityDriverLic.LicenseFirstName = Helpers.GetValue(rdata.Data, "Name");
						identityDriverLic.LicenseNumber = Helpers.GetValue(rdata.Data, "Driver Lic");
						secureItems.Add(identityDriverLic.GetSecureItem());
					}
					if (rdata.Data.ContainsKey("Note"))
					{
						PBSubType.PersonalInfo.SecureNotes identityData = new PBSubType.PersonalInfo.SecureNotes();
						identityData.Title = Helpers.GetValue(rdata.Data, "Name");
						identityData.Notes = Helpers.GetValue(rdata.Data, "Note");
						secureItems.Add(identityData.GetSecureItem());
					}
				} // Person
				else if (rdata.Caption == "Business")
				{
					PBSubType.PersonalInfo.Company identData = new PBSubType.PersonalInfo.Company();
					identData.Name = Helpers.GetValue(rdata.Data, "Company Name");

					string compnote = "";
					foreach (KeyValuePair<string, string> data in rdata.Data)
					{
						compnote += data.Key == "Company Name" ? "" : data.Key.ToUpperInvariant() + ": " + data.Value + "; ";
					}
					identData.Notes = compnote;
					secureItems.Add(identData.GetSecureItem());
				} // Business
				else if (rdata.Caption == "Passport")
				{
					PBSubType.PersonalInfo.Passport identData = new PBSubType.PersonalInfo.Passport();
					identData.PassportNickName = rdata.IdentityName;
					identData.PassportExpiers = Helpers.GetValue(rdata.Data, "Passport Exp­iration Date");
					identData.PassportIssueDate = Helpers.GetValue(rdata.Data, "Passport Issue Date");
					identData.PassportPlaceOfIssue = Helpers.GetValue(rdata.Data, "Passport Issue Place");
					identData.PassportNationality = Helpers.GetValue(rdata.Data, "Passport­ Type");
					identData.PassportNumber = Helpers.GetValue(rdata.Data, "Passport Number");
					secureItems.Add(identData.GetSecureItem());
				} // Passport
				else if (rdata.Caption == "Address")
				{
					PBSubType.PersonalInfo.Address identData = new PBSubType.PersonalInfo.Address();
					identData.Nickname = rdata.IdentityName;
					identData.Address1 = Helpers.GetValue(rdata.Data, "Address Line 1");
					identData.Address2 = Helpers.GetValue(rdata.Data, "Address Line 2");
					identData.City = Helpers.GetValue(rdata.Data, "PostCode City");
					identData.State = Helpers.GetValue(rdata.Data, "Country");
					identData.Notes = Helpers.GetValue(rdata.Data, "Note");
					secureItems.Add(identData.GetSecureItem());
				} // Address
				else if (rdata.Caption == "Credit Card")
				{
					PBSubType.DigitalWallet.CreditCard identData = new PBSubType.DigitalWallet.CreditCard();
					identData.CreditCardNickname = rdata.IdentityName;
					identData.CardNumber = Helpers.GetValue(rdata.Data, "Card Number");
					identData.Cvv = Helpers.GetValue(rdata.Data, "Validation Code");
					identData.ExpiresMonth = Helpers.GetValue(rdata.Data, "Card Expires").Split('/')[0];
					identData.ExpiresYear = Helpers.GetValue(rdata.Data, "Card Expires").Split('/')[1];
					identData.IssueBank = Helpers.GetValue(rdata.Data, "Issuing Bank");
					identData.NameOnCard = Helpers.GetValue(rdata.Data, "Card User Name");
					identData.Pin = Helpers.GetValue(rdata.Data, "PIN Number");
					secureItems.Add(identData.GetSecureItem());
				} // Credit Card
				else if (rdata.Caption == "Bank Account")
				{
					PBSubType.DigitalWallet.Bank identData = new PBSubType.DigitalWallet.Bank();
					identData.AccountNickname = rdata.IdentityName;
					identData.AccountBankName = Helpers.GetValue(rdata.Data, "Bank Name");
					identData.AccountName = Helpers.GetValue(rdata.Data, "Account Type");
					identData.AccountNumber = Helpers.GetValue(rdata.Data, "Account Number");
					identData.AccountRoutingNumber = Helpers.GetValue(rdata.Data, "Routing Number");
					secureItems.Add(identData.GetSecureItem());
				} // Bank Account
			}
			else
			{
				PBSubType.PersonalInfo.SecureNotes identityNoteData = new PBSubType.PersonalInfo.SecureNotes();

				identityNoteData.Title = rdata.Caption;

				var notetext = string.IsNullOrEmpty(rdata.SubCaption) ? "" : rdata.SubCaption + "; ";

				if (rdata.IsDictionary)
				{
					foreach (KeyValuePair<string, string> data in rdata.Data)
					{
						notetext += data.Key.ToUpperInvariant() + ": " + data.Value + "; ";
					}
				}
				else
				{
					foreach (string note in rdata.DataList)
					{
						notetext += note + " ";
					}
				}

				identityNoteData.Notes = notetext.Trim();

				secureItems.Add(identityNoteData.GetSecureItem());
			}
		}

		private static void TryToProcessPhoneNumber(string type, List<SecureItem> secureItems, RoboFormData rdata)
		{
			if (rdata.Data.ContainsKey(type))
			{
				PBSubType.PersonalInfo.PhoneNumber identityPhoneData = new PBSubType.PersonalInfo.PhoneNumber();
				identityPhoneData.Nickname = Helpers.GetValue(rdata.Data, "Name");
				identityPhoneData.Number = Helpers.GetValue(rdata.Data, type);
				identityPhoneData.Notes = type;
				secureItems.Add(identityPhoneData.GetSecureItem());
			}
		}
	}
}
