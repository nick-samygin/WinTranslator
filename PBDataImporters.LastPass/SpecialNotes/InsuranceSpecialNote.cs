using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class InsuranceSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Insurance"; }
		}

		public static class DictKeys
		{
			public static string Company = "Company";
			public static string PolicyType = "Policy Type";
			public static string PolicyNumber = "PolicyNumber";
			public static string Expiration = "Expiration";
			public static string AgentName = "Agent Name";
			public static string AgentPhone = "Agent Phone";
			public static string URL = "URL";
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
