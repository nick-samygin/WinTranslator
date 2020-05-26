using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.LastPass.SpecialNotes;
using System;
using System.Collections.Generic;

namespace PBDataImporters.LastPass
{
	public class CsvExportItem
	{
		// keep exact column names. This is a contract.
		public string url { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string extra { get; set; }
		public string name { get; set; }
		public string grouping { get; set; }
		public string fav { get; set; }

		public SecureItem ToSecureItem()
		{
			if (url.Equals(@"http://sn"))
			{
				var dict = GetExtraAsDict();
				var note = IsPlainNote() ? 	new PlainSpecialNote()
					: SpecialNoteFactory.GetSpecialNote(dict);

				return note.GetSecureItem(this, dict);
			}
			else
			{
				// login item
				PBSubType.PasswordVault.Login data = new PBSubType.PasswordVault.Login();

				data.Url = url;
				data.UserName = username;
				data.Password = password;
				data.Notes = extra;
				data.SiteName = name;

				return data.GetSecureItem();
			}

			return null;
		}

		private bool IsPlainNote()
		{
			return !extra.Contains("NoteType");
		}

		private Dictionary<string, string> GetExtraAsDict()
		{
			return GetExtraAsDict(':');
		}

		private Dictionary<string, string> GetExtraAsDict(char separator)
		{
			var dict = new Dictionary<string, string>();

			if (!extra.Contains("\n"))
			{
				dict = new Dictionary<string, string>();
				dict.Add("NoteType", "");
				dict.Add("Value", extra);
				return dict;				
			}

			var lines = extra.Split('\n');
			foreach (var line in lines)
			{
				if (line.Contains(separator.ToString()))
				{
					var tokens = line.Split(separator);

					if (tokens.Length == 2)
					{
						if (dict.ContainsKey(tokens[0]))
						{
							throw new ArgumentException(string.Format("Key {0} already present", tokens[0]));
						}
						dict.Add(tokens[0], tokens[1]);
					}
					else
					{
						throw new FormatException(string.Format("line {0} is in incorrect format", line));
					}
				}
			}

			return dict;

		}

	}
}
