using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System.Collections.Generic;
using System.Linq;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class WebFormTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] { "webforms.WebForm" };
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");


			PBSubType.PasswordVault.Login data = new PBSubType.PasswordVault.Login();

			data.SiteName = (string)jsonData["title"];
			data.Url = (string)jsonData["location"];

			data.UserName = (string)jsonData["secureContents"]["fields"][0]["value"];
			data.Password = (string)jsonData["secureContents"]["fields"][1]["value"];

			data.Notes = ""; //tags

			if (jsonData["openContents"]["tags"] != null)
			{
				int tagsCnt = jsonData["openContents"]["tags"].Count();
				var tags = jsonData["openContents"]["tags"];

				for (int i = 0; i < tagsCnt; i++)
					data.Notes += (string)tags[i] + " ";
			}

			secureItems.Add(data.GetSecureItem());
		}
	}
}
