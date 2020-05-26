using PasswordBoss;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


namespace PBDataImporters.KeePass
{
	[Export(typeof(AppImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")] // this is correct spelling!
	public class KeePassImportStrategy : PasswordBoss.AppImportStrategyBase
	{
		public override string Name
		{
			get { return "KeePass"; }
		}

		protected override PasswordBoss.ImportFromAppResult ImportInternal(PasswordBoss.FileImportArgs args)
		{
			if (args == null)
				throw new ArgumentNullException("args");
			var res = new ImportFromAppResult();

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
			string[] fline;
			// "Account","Login Name","Password","Web Site","Comments"   KeePass row structure


			foreach (string fileLine in file)
			{
				lncnt++;

				try
				{
					fline = fileLine.Substring(1, fileLine.Length - 2).Replace(@""",""", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);


					if ((fileLine.Trim().Length == 0) || (fileLine.Trim() == @"""Account"",""Login Name"",""Password"",""Web Site"",""Comments"""))
					{
						continue;
					}
					else
					{
						PBSubType.PasswordVault.Login data = new PBSubType.PasswordVault.Login();

						data.SiteName = fline[0].Trim(); // Account
						data.UserName = fline[1].Trim(); //Login Name
						data.Password = fline[2].Trim(); //Password
						data.Url = fline[3].Trim();  //Web Site 
						data.Notes = fline[4].Trim(); //Comments

						res.SecureItems.Add(data.GetSecureItem());
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
