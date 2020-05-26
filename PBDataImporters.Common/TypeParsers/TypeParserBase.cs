using PasswordBoss.DTO;
using System;
using System.Collections.Generic;

namespace PBDataImporters.Common.TypeParsers
{
	public abstract class TypeParserBase<TArg>
	{
		public abstract string[] GetSupportedTypes();

		public void AddParsedItem(List<SecureItem> secureItems, List<string> messages, TArg arg)
		{
			AddParsedItemInternal(secureItems, messages, arg);
		}

		protected abstract void AddParsedItemInternal(List<SecureItem> secureItems, List<string> messages, TArg arg);
	}
}
