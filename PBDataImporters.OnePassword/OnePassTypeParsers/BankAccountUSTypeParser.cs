using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System.Collections.Generic;

namespace PBDataImporters.OnePassword.OnePassTypeParsers
{
	// instantinated via reflection
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	class BankAccountUSTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		
		public override string[] GetSupportedTypes()
		{
			return new string[] { "wallet.financial.BankAccountUS" }; 
		}

		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");


			PBSubType.DigitalWallet.Bank data = new PBSubType.DigitalWallet.Bank();
			data.AccountNickname = (string)jsonData["title"];
			data.AccountBankName = (string)jsonData["secureContents"]["bankName"];
			data.AccountName = (string)jsonData["secureContents"]["owner"];
			data.AccountNumber = (string)jsonData["secureContents"]["accountNo"];
			data.AccountRoutingNumber = (string)jsonData["secureContents"]["routingNo"];
			data.Swift = (string)jsonData["secureContents"]["swift"];
			data.Iban = (string)jsonData["secureContents"]["iban"];


			secureItems.Add(data.GetSecureItem());
		}
	}
}
