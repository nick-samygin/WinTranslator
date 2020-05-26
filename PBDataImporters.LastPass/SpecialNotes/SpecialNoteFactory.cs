using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	public static class SpecialNoteFactory
	{
		private static readonly ILogger logger = Logger.GetLogger(typeof(SpecialNoteFactory));

		private static List<SpecialNoteBase> specialNotes;
		private static List<SpecialNoteBase> SpecialNotes
		{
			get
			{
				if (specialNotes == null)
				{
					specialNotes = Assembly.GetAssembly(typeof(SpecialNoteFactory))
						.GetTypes()
						.Where(t => t.IsSubclassOf(typeof(SpecialNoteBase)))
						.Select(t => (SpecialNoteBase)t.GetConstructor(new Type[0]).Invoke(new Type[0]))
						.ToList();
				}

				return specialNotes;
			}
		}

		public static SpecialNoteBase GetSpecialNote(Dictionary<string,string> dict)
		{
			var specialNoteType = dict.ElementAt(0).Value.Trim("\n\r\t ".ToCharArray());
			var res = SpecialNotes.SingleOrDefault(sn => sn.NoteTypeName.Equals(specialNoteType));

			if (res == null)
			{
				logger.Error("SpecialNote type not found {0}. Generating plain note", specialNoteType);
				res = (SpecialNoteBase) new PlainSpecialNote();
			}

			return res;
		}

		public static SecureItem GetSecureItem(CsvExportItem exportItem, Dictionary<string, string> dict)
		{
			return GetSpecialNote(dict).GetSecureItem(exportItem, dict);
		}
	}
}
