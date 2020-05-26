using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class PassportTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] { "wallet.government.Passport" };
		}


		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");

			PBSubType.PersonalInfo.Passport data = new PBSubType.PersonalInfo.Passport();

			data.PassportNickName = (string)jsonData["title"];
			data.PassportDateOfBirth = new DateTime(int.Parse((string)jsonData["secureContents"]["birthdate_yy"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["birthdate_mm"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["birthdate_dd"], CultureInfo.InvariantCulture)).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			data.PassportExpiers = new DateTime(int.Parse((string)jsonData["secureContents"]["expiry_date_yy"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["expiry_date_mm"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["expiry_date_dd"], CultureInfo.InvariantCulture)).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			data.PassportFirstName = (string)jsonData["secureContents"]["fullname"];
			data.PassportIssueDate = new DateTime(int.Parse((string)jsonData["secureContents"]["issue_date_yy"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["issue_date_mm"], CultureInfo.InvariantCulture), int.Parse((string)jsonData["secureContents"]["issue_date_dd"], CultureInfo.InvariantCulture)).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			data.PassportNationality = (string)jsonData["secureContents"]["nationality"];
			data.PassportNumber = (string)jsonData["secureContents"]["number"];
			data.PassportPlaceOfIssue = (string)jsonData["secureContents"]["issuing_authority"] + ", " + (string)jsonData["secureContents"]["issuing_country"];

			secureItems.Add(data.GetSecureItem());
		}
	}
}
