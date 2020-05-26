using PasswordBoss;
using PasswordBoss.DTO;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	public abstract class SpecialNoteBase
	{
		public static class DictKeys
		{
			public static string NoteType = "NoteType";
		}

		public abstract string NoteTypeName { get; }

		public SpecialNoteBase()
		{
		}

		public SecureItem GetSecureItem(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			return GetSecureItemInternal(exportItem, values);
		}

		protected abstract SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values);
	}
}
