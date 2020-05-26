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
	class CreditCardTypeParser : TypeParserBase<Newtonsoft.Json.Linq.JObject>
	{
		public override string[] GetSupportedTypes()
		{
			return new string[] {"wallet.financial.CreditCard" }; 
		}
		
		protected override void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, JObject jsonData)
		{
			if (jsonData == null)
				throw new System.ArgumentNullException("jsonData");

			if (secureItems == null)
				throw new System.ArgumentNullException("secureItems");

			if (messages == null)
				throw new System.ArgumentNullException("messages");

			PBSubType.DigitalWallet.CreditCard data = new PBSubType.DigitalWallet.CreditCard();

			data.CreditCardNickname = (string)jsonData["title"];
			data.CardNumber = (string)jsonData["secureContents"]["ccnum"];
			data.Cvv = (string)jsonData["secureContents"]["cvv"];
			data.ExpiresMonth = (string)jsonData["secureContents"]["expiry_mm"];
			data.ExpiresYear = (string)jsonData["secureContents"]["expiry_yy"];
			data.IssueBank = (string)jsonData["secureContents"]["bank"];
			data.NameOnCard = (string)jsonData["secureContents"]["cardholder"];
			data.Pin = (string)jsonData["secureContents"]["pin"];
			data.CreditCardType = (string)jsonData["secureContents"]["type"];
			/*data.SelectedAddress = "";
			data.SelectedCountry = "";
			data.SelectedCreditCard = "";*/

			secureItems.Add(data.GetSecureItem());
		}
	}
}
