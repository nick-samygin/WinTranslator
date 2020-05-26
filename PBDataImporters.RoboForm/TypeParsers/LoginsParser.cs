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
	class LoginsParser : TypeParserBase<RoboFormData>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] 
			{
				RoboformExportType.Logins.ToString()
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

			if ((rdata.IsDictionary) && (rdata.Data.ContainsKey("username")) /*&& (rdata.data.ContainsKey("password"))*/)
			{
				PBSubType.PasswordVault.Login loginData = new PBSubType.PasswordVault.Login();

				loginData.Url = rdata.SubCaption;
				loginData.UserName = Helpers.GetValue(rdata.Data, "username");
				loginData.Password = Helpers.GetValue(rdata.Data, "password");
				//loginData.Notes = "";
				loginData.SiteName = rdata.Caption;

				secureItems.Add(loginData.GetSecureItem());
			}
			else
			{
				PBSubType.PersonalInfo.SecureNotes loginNoteData = new PBSubType.PersonalInfo.SecureNotes();

				loginNoteData.Title = rdata.Caption;

				var notetext = string.IsNullOrEmpty(rdata.SubCaption) ? "" : rdata.SubCaption + "; ";

				if (rdata.IsDictionary)
				{
					foreach (KeyValuePair<string, string> data in rdata.Data)
					{
						notetext += data.Key.ToUpperInvariant() + ": " + data.Value + "; ";
					}
				}
				else
				{
					foreach (string note in rdata.DataList)
					{
						notetext += note + " ";
					}
				}

				loginNoteData.Notes = notetext.Trim();

				secureItems.Add(loginNoteData.GetSecureItem());
			}
		}
	}
}
