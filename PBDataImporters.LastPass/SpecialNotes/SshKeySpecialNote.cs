using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class SshKeySpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "SSH Key"; }
		}

		public static class DictKeys
		{
			public static string BitStrength = "Bit Strength";
			public static string Format = "Format";
			public static string Passphrase = "Passphrase";
			public static string PrivateKey = "Private Key";
			public static string PublicKey = "Public Key";
			public static string Hostname = "Hostname";
			public static string Date = "Date";
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
