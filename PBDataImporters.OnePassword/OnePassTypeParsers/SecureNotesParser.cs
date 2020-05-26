using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System.Collections.Generic;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class SecureNotesParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] {
				"wallet.onlineservices.InstantMessenger",
				"wallet.membership.RewardProgram",
				"wallet.onlineservices.FTP",
				"wallet.onlineservices.AmazonS3",
				"wallet.onlineservices.GenericAccount",
				"wallet.computer.UnixServer",
				"wallet.computer.Database",
				"securenotes.SecureNote",
				"wallet.government.SsnUS",
				"wallet.computer.Router",
				"wallet.computer.License",
				"wallet.government.HuntingLicense",
				"wallet.onlineservices.DotMac",
				"system.folder.Regular",
				"wallet.onlineservices.ISP",
				"wallet.onlineservices.iTunes",
				"wallet.onlineservices.Email.v2"
			}; 
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");

			PBSubType.PersonalInfo.SecureNotes data = new PBSubType.PersonalInfo.SecureNotes();

			data.Title = (string)jsonData["title"];
			data.Notes = FileParser.PackSecureContents(jsonData);

			secureItems.Add(data.GetSecureItem());
		}
	}
}
