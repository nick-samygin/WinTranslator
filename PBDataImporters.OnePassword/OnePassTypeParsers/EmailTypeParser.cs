using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System.Collections.Generic;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class EmailTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] { "wallet.onlineservices.Email.v2" };
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");

			// SMTP server data -> PI_SecureNotes , // NOT PI_Email 

			PBSubType.PersonalInfo.SecureNotes data = new PBSubType.PersonalInfo.SecureNotes();

			data.Title = "Server " + (string)jsonData["title"];
			data.Notes = FileParser.PackSecureContents(jsonData);

			secureItems.Add(data.GetSecureItem());
		}
	}
}
