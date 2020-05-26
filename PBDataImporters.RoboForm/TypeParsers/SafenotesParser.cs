using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.RoboForm.TypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class SafenotesParser : TypeParserBase<RoboFormData>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] 
			{
				RoboformExportType.SafeNotes.ToString()
			};
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, RoboFormData rdata)
		{
			if (rdata == null)
				throw new ArgumentNullException("rdata");

			if (secureItems == null)
				throw new ArgumentNullException("secureItems");

			if (messages == null)
				throw new ArgumentNullException("messages");


			PBSubType.PersonalInfo.SecureNotes noteData = new PBSubType.PersonalInfo.SecureNotes();

			noteData.Title = rdata.Caption;

			var notetext = string.IsNullOrEmpty(rdata.SubCaption) ? "" : rdata.SubCaption + "; ";

			foreach (string note in rdata.DataList)
			{
				notetext += note + " ";
			}

			noteData.Notes = notetext.Trim();

			secureItems.Add(noteData.GetSecureItem());
		}
	}
}
