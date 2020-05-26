using PasswordBoss;
using PasswordBoss.DTO;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class PassportSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Passport"; }
		}

		public static class DictKeys
		{
			public static string Name = "Name";
			public static string Number = "Number";
			public static string Nationality = "Nationality";
			public static string IssuingAuthority = "Issuing Authority";
			public static string DateOfBirth = "Date of Birth";
			public static string IssuedDate = "Issued Date";
			public static string ExpirationDate = "Expiration Date";
		}

		protected override SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.Passport();

			data.PassportNickName = exportItem.name;
			data.PassportFirstName = values[DictKeys.Name];
			data.PassportNumber = values[DictKeys.Number];
			data.PassportNationality = values[DictKeys.Nationality];
			data.PassportPlaceOfIssue = values[DictKeys.IssuingAuthority];
			data.PassportDateOfBirth = values[DictKeys.DateOfBirth];
			data.PassportIssueDate = values[DictKeys.IssuedDate];
			data.PassportExpiers = values[DictKeys.ExpirationDate];

			return data.GetSecureItem();
		}
	}
}
