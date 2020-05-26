using PasswordBoss;
using PBDataImporters.Common.TypeParsers;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PBDataImporters.RoboForm
{
	[Export(typeof(AppImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")] // this is correct spelling!
	public class RoboFormImportStrategy : PasswordBoss.AppImportStrategyBase
	{
		
		public override string Name
		{
			get { return "RoboForm"; }
		}

		protected override PasswordBoss.ImportFromAppResult ImportInternal(PasswordBoss.FileImportArgs args)
		{
			if (args == null)
				throw new ArgumentNullException("args");

			RoboformExportType currentRBExportType;

			var importResult = new PasswordBoss.ImportFromAppResult();
			var dataList = HtmlParser.ParseFile(args.FilePath, importResult.ImportMessages, out currentRBExportType);
			
			var parser =  new TypeParserLocator<RoboFormData>().Locate(currentRBExportType.ToString());

			foreach (RoboFormData rdata in dataList)
			{
				try
				{
					parser.AddParsedItem(importResult.SecureItems, importResult.ImportMessages, rdata);
				}
				catch(Exception ex)
				{
					if (ex is ArgumentException || ex is InvalidOperationException)
						importResult.ImportMessages.Add(string.Format(CultureInfo.InvariantCulture, "Input file:{0}. Error in parsed data object:{1}.", args.FilePath, rdata.Caption));
					else
						throw;
				}

			}


			return importResult;
		}
	}
}
