using PasswordBoss;
using PasswordBoss.DTO;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class InstantMessengerSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Instant Messenger"; }
		}

		public static class DictKeys
		{
			public static string Username = "Username";
			public static string Password = "Password";
			public static string Server = "Server";
			public static string Port = "Port";
			public static string Notes = "Notes";
		}

		protected override SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.SecureNotes();
			data.Title = exportItem.name;
			data.Notes = exportItem.extra;
			return data.GetSecureItem();
		}
	}
}
