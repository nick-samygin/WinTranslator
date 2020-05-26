using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class DriversLicenseSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Driver's License"; }
		}

		public static class DictKeys
		{
			public static string Country = "Country";
			public static string Number = "Number";
			public static string ExpirationDate = "Expiration Date";
			public static string Name = "Name";
			public static string State = "State";
		}

		protected override SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.DriverLicense();
			data.LicenseNickname = exportItem.name;
			data.LicenseCountry = values[DictKeys.Country];
			data.LicenseNumber = values[DictKeys.Number];
			data.LicenseExpires = values[DictKeys.ExpirationDate];
			data.LicenseFirstName = values[DictKeys.Name];
			data.LicenseState = values[DictKeys.State];

			return data.GetSecureItem();
		}
	}
}
