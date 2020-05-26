using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class HealthInsuranceSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Health Insurance"; }
		}

		public static class DictKeys
		{
			public static string Company = "Company";
			public static string CompanyPhone = "Company Phone";
			public static string PolicyType = "Policy Type";
			public static string PolicyNumber = "Policy Number";
			public static string GroupID = "Group ID";
			public static string MemberName = "Member Name";
			public static string MemberID = "Member ID";
			public static string PhysicianName = "Physician Name";
			public static string PhysicianPhone = "Physician Phone";
			public static string PhysicianAddress = "Physician Address";
			public static string CoPay = "Co-pay";
			public static string Notes = "Notes";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.PersonalInfo.SecureNotes();
			data.Title = exportItem.name;
			data.Notes = exportItem.extra;
			return data.GetSecureItem();
		}
	}
}
