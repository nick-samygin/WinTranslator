using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class WiFiPasswordSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Wi-Fi Password"; }
		}

		public static class DictKey
		{
			public static string SSID = "SSID";
			public static string Password = "Password";
			public static string ConnectionType = "Connection Type";
			public static string ConnectionMode = "Connection Mode";
			public static string Authentication = "Authentication";
			public static string Encryption = "Encryption";
			public static string Use802_1X = "Use 802.1X";
			public static string FipsMode = "FIPS Mode";
			public static string KeyType = "Key Type";
			public static string Protected = "Protected";
			public static string KeyIndex = "Key Index";
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
