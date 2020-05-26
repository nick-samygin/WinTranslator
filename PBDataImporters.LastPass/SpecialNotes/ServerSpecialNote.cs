using System;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class ServerSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Server"; }
		}

		public static class DictKeys
		{
			public static string Hostname = "Hostname";
			public static string Username = "Username";
			public static string Password = "Password";
			public static string Notes = "Notes";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PasswordBoss.PBSubType.PersonalInfo.SecureNotes();
			data.Title = exportItem.name;
			data.Notes = exportItem.extra;
			return data.GetSecureItem();
		}
	}
}
