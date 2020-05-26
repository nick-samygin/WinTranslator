using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class MembershipTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] { "wallet.membership.Membership" };
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");


			PBSubType.PersonalInfo.MemberIDs data = new PBSubType.PersonalInfo.MemberIDs();

			data.MemberIdNickname = (string)jsonData["title"];
			data.MemberIdFirstName = (string)jsonData["secureContents"]["member_name"];
			data.MemberId = (string)jsonData["secureContents"]["membership_no"];
			data.MemberIdNotes = (string)jsonData["secureContents"]["notesPlain"] + "; PIN:" + (string)jsonData["secureContents"]["pin"];

			secureItems.Add(data.GetSecureItem());
		}
	}
}
