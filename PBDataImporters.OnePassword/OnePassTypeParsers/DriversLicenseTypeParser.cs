using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class DriversLicenseTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] {"wallet.government.DriversLicense" };
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");

			PBSubType.PersonalInfo.DriverLicense data = new PBSubType.PersonalInfo.DriverLicense();
			data.LicenseNickname = (string)jsonData["title"];
			data.LicenseCountry = (string)jsonData["secureContents"]["country"];
			data.LicenseExpires = (string)jsonData["secureContents"]["expiry_date"];
			data.LicenseFirstName = (string)jsonData["secureContents"]["fullname"];
			data.LicenseNumber = (string)jsonData["secureContents"]["number"];
			data.LicenseState = (string)jsonData["secureContents"]["state"];

			secureItems.Add(data.GetSecureItem());
		}
	}
}
