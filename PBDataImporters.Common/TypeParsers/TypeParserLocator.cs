using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PBDataImporters.Common.TypeParsers
{
	public class TypeParserLocator<TArg>
	{
		private TypeParserBase<TArg>[] parsers;
		private TypeParserBase<TArg>[] Parsers
		{
			get
			{
				if (parsers == null)
				{
					var types = Assembly.GetCallingAssembly()
										.GetTypes()
										.Where(t => t.BaseType == typeof(TypeParserBase<TArg>));

					var parserList = new List<TypeParserBase<TArg>>();
					foreach (var type in types)
					{
						parserList.Add((TypeParserBase<TArg>)Activator.CreateInstance(type));
					}

					parsers = parserList.ToArray();
				}

				return parsers;
			}
		}
		public TypeParserBase<TArg> Locate(string name)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			return Parsers.FirstOrDefault(p => p.GetSupportedTypes().Any(st => st.Equals(name, StringComparison.OrdinalIgnoreCase)));
		}
	}
}
