using PasswordBoss;
using PasswordBoss.DTO;

namespace PBDataImporters.LastPass.SpecialNotes
{
	public class PlainSpecialNote : SpecialNoteBase
	{

		public override string NoteTypeName
		{
			get { return ""; }
		}

		protected override SecureItem GetSecureItemInternal(CsvExportItem exportItem, System.Collections.Generic.Dictionary<string, string> values)
		{
			var note = new PBSubType.PersonalInfo.SecureNotes();
			note.Notes = exportItem.extra;
			return note.GetSecureItem();
		}
	}
}
