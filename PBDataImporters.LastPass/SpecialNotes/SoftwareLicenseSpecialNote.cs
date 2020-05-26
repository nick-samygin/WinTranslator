using PasswordBoss;
using System;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class SoftwareLicenseSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Software License"; }
		}

		public static class DictKeys
		{
			 public static string LicenseKey = "License Key";
			 public static string Licensee = "Licensee";
			 public static string Version = "Version";
			 public static string Publisher = "Publisher";
			 public static string Support = "Support";
			 public static string Website = "Website";
			 public static string Price = "Price";
			 public static string PurchaseDate = "PurchaseDate";
			 public static string OrderNumber = "Order Number";
			 public static string NumberOfLicenses = "Number of Licenses";
			 public static string OrderTotal = "Order Total";
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
