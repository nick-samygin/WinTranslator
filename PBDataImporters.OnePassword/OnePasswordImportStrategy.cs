using Newtonsoft.Json.Linq;
using PasswordBoss;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PBDataImporters.OnePassword
{
	[Export(typeof(AppImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")] // this is correct spelling!
	public class OnePasswordImportStrategy : PasswordBoss.AppImportStrategyBase
	{
		public override string Name
		{
			get { return "OnePassword"; }
		}

		protected override PasswordBoss.ImportFromAppResult ImportInternal(PasswordBoss.FileImportArgs args)
		{
			if (args == null)
				throw new ArgumentNullException("args");
			var res = new PasswordBoss.ImportFromAppResult();
			string[] file;
			try
			{
				file = System.IO.File.ReadAllLines(args.FilePath);
			}
			catch
			{
				throw new System.IO.FileLoadException(string.Format(CultureInfo.InvariantCulture, "Error while loading file '{0}'.", args.FilePath));
			}

			int lncnt = 0;

			var locator = new Common.TypeParsers.TypeParserLocator<JObject>();

			foreach (string fileLine in file)
			{
				lncnt++;

				try
				{
					if (fileLine.StartsWith("***", StringComparison.Ordinal))
					{
						continue;
					}

					JObject jsonData = JObject.Parse(fileLine);
					var processor = locator.Locate((string)jsonData["typeName"]);
					
					if (processor != null)
					{
						processor.AddParsedItem(res.SecureItems, res.ImportMessages, jsonData);
					}
				}
				catch (Exception ex)
				{
					if (ex is IndexOutOfRangeException || ex is ArgumentException || ex is NullReferenceException)
					{
						res.ImportMessages.Add(string.Format(CultureInfo.InvariantCulture, "Input file:{0}. Error in line:{1}.", args.FilePath, lncnt));
					}
					else
					{
						throw;
					}
				}

			}

			return res;
		}
	}
}
