using PasswordBoss;
using PasswordBoss.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PBDataImporters.LastPass
{
	[Export(typeof(AppImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public class LastPassImportStrategy : PasswordBoss.AppImportStrategyBase
	{
		public override string Name
		{
			get { return "LastPass"; }
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

			using (TextReader reader = File.OpenText(args.FilePath))
			{
				var csvReader = new CsvHelper.CsvReader(reader);
				var records = csvReader.GetRecords<CsvExportItem>()
					.ToArray() // store records in memory
					.Select(i => i.ToSecureItem())
					.ToArray();
				res.SecureItems.AddRange(records);
			}

			return res;
		}

		
	}
}
