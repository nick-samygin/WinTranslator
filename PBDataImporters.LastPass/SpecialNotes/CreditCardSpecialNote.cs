using PasswordBoss;
using System.Collections.Generic;

namespace PBDataImporters.LastPass.SpecialNotes
{
	class CreditCardSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Credit Card"; }
		}

		public static class DictKeys
		{
			public static string Number = "Number";
			public static string ExpirationDate = "Expiration Date";
			public static string NameOnCard = "Name on Card";
			public static string SecurityCode = "Security Code";
			public static string Type = "Type";
		}

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			var data = new PBSubType.DigitalWallet.CreditCard();
			
			var expirationDateParts = values[DictKeys.ExpirationDate].Split(',');

			data.CreditCardNickname = exportItem.name;
			data.CardNumber = values[DictKeys.Number];
			data.ExpiresMonth = Helpers.GetMonthValue(expirationDateParts[0]);
			data.ExpiresYear = expirationDateParts[1];
			data.NameOnCard = values[DictKeys.NameOnCard];
			data.Pin = values[DictKeys.SecurityCode];
			data.CreditCardType = values[DictKeys.Type];
		
			var secureItem = data.GetSecureItem();
			secureItem.Color = "0";
			return secureItem;
		}
	}
}
