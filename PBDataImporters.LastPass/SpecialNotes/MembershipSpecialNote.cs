
using PasswordBoss;
namespace PBDataImporters.LastPass.SpecialNotes
{
	class MembershipSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Membership"; }
		}

		public static class DictKeys
		{
			public static string MembershipNumber = "Membership Number";
			public static string MembershipName = "Member Name";
			public static string Organization = "Organization";
			public static string StartDate = "Start Date";
			public static string ExpirationDate = "Expiration Date";
			public static string Website = "Website";
			public static string Telephone = "Telephone";
			public static string Password = "Password";
			public static string Notes = "Notes";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, System.Collections.Generic.Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.MemberIDs();
			data.MemberId = values[DictKeys.MembershipNumber];
			data.MemberIdNickname = values[DictKeys.MembershipName];

			data.MemberIdNotes = Helpers.GetValueFormated(values, DictKeys.Organization) +
				Helpers.GetValueFormated(values, DictKeys.StartDate) +
				Helpers.GetValueFormated(values, DictKeys.ExpirationDate) +
				Helpers.GetValueFormated(values, DictKeys.Website) +
				Helpers.GetValueFormated(values, DictKeys.Telephone) +
				Helpers.GetValueFormated(values, DictKeys.Password) +
				Helpers.GetValueFormated(values, DictKeys.Notes);

			return data.GetSecureItem();
		}
	}
}
