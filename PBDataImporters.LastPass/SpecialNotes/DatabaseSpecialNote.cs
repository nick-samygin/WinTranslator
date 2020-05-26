using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class DatabaseSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Database"; }
		}

		public static class DictKeys
		{
			public static string Hostname = "Hostname";
			public static string Port = "Port";
			public static string Database = "Database";
			public static string Username = "Username";
			public static string Password = "Password";
			public static string SID = "SID";
			public static string Alias = "Alias";
			public static string Notes = "Notes";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			// TODO:
			var data = new PBSubType.PersonalInfo.SecureNotes();
			data.Title = exportItem.name;
			data.Notes = exportItem.extra;
			return data.GetSecureItem();
		}
	}
}
