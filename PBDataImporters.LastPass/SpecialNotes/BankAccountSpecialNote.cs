using PasswordBoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.LastPass.SpecialNotes
{
	public class BankAccountSpecialNote : SpecialNoteBase
	{
		public override string NoteTypeName
		{
			get { return "Bank Account"; }
		}

		public static class DictKeys
		{
			public static string BankName = "Bank Name";
			public static string AccountNumber = "Account Number";
			public static string RoutingNumber = "Routing Number";
			public static string SwiftCode = "SWIFT Code";
			public static string IbanNumber = "IBAN Number";
		}	

		protected override PasswordBoss.DTO.SecureItem GetSecureItemInternal(CsvExportItem exportItem, Dictionary<string, string> values)
		{
			PBSubType.DigitalWallet.Bank data = new PBSubType.DigitalWallet.Bank();
			data.AccountNickname = exportItem.name;
			data.AccountBankName = values[DictKeys.BankName];
			data.AccountNumber = values[DictKeys.AccountNumber];
			data.AccountRoutingNumber = values[DictKeys.RoutingNumber];
			data.Swift = values[DictKeys.SwiftCode];
			data.Iban = values[DictKeys.IbanNumber];

			return data.GetSecureItem();
		}
	}
}
