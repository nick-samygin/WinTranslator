using PasswordBoss;
using System;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class SocialSecuritySpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Social Security" ; }
		}

		public static class DictKeys
		{
			public static string Name = "Name";
			public static string Number = "Number";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.SocialSecurity();

			data.SocialSecurityFirstName = values[DictKeys.Name];
			data.SocialSecurityNumber = values[DictKeys.Number];

			return data.GetSecureItem();
		}
	}
}
