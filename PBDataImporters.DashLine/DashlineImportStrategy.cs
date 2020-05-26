using PasswordBoss;
using PasswordBoss.Helpers;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;



namespace PBDataImporters.DashLine
{
	[Export(typeof(AppImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public class DashLineImportStrategy : AppImportStrategyBase
	{

		public override string Name
		{
			get { return "Dashline"; }
		}

		private static bool IsPidLike(string column)
		{
			if ((column.Length > 35) 
				&& (column.StartsWith("{", StringComparison.Ordinal))
				&& (column.EndsWith("}", StringComparison.Ordinal)) 
				&& (column.Contains('-'))) 
				return true;
			else return false;
		}

		private static bool IsPhoneLikeLineId(string column)
		{
			if ((column == "Cell") || (column == "Home") || (column == "Fax") || (column == "Work") || (column == "Work cell") || (column == "Work fax")) return true;
			else return false;
		}

		protected override ImportFromAppResult ImportInternal(FileImportArgs args)
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
				throw new System.IO.FileLoadException (string.Format(CultureInfo.InvariantCulture, "Error while loading file '{0}'.", args.FilePath));
			}


			int lncnt = 0;
			string[] fline;
			// dashlane export format rules: no rule


			foreach (string fileLine in file)
			{
				lncnt++;

				try
				{
					/*lineId = fileLine.Substring(0, fileLine.IndexOf(',') - 1).Trim();
					csvLine = fileLine.Substring(fileLine.IndexOf(',') + 1);*/

					fline = fileLine.Substring(1, fileLine.Length - 2).Replace(@""",""", Environment.NewLine).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);


					if ((fileLine.Trim().Length == 0))
					{
						continue;
					}
					else if ((fline.Count() == 5) && (!IsPidLike(fline[0].Trim())) /*&& (fline[1].Trim().IndexOf('.') >= 0)*/) // may be url login
					{
						// eg. "rsg2","rsg.ba","ruspasbos","rusPasbos.7","vijesti"

						if (fline[0].Trim().ToUpper(CultureInfo.InvariantCulture).IndexOf("PAYPAL", StringComparison.Ordinal) >= 0)
						{
							var data = new PBSubType.DigitalWallet.PayPal();

							data.PaypalNickname = fline[0].Trim();
							data.Username = fline[2].Trim();
							data.Password = fline[3].Trim();
							data.PaypalNotes = fline[4].Trim();

							res.SecureItems.Add(data.GetSecureItem());
						}
						else
						{
							var data = new PBSubType.PasswordVault.Login();

							data.SiteName = fline[0].Trim();
							data.UserName = fline[2].Trim();
							data.Password = fline[3].Trim();
							data.Url = fline[1].Trim();
							data.Notes = fline[4].Trim();
							if (string.IsNullOrWhiteSpace(data.SiteName))
							{
								data.SiteName = data.Url;
							}
							res.SecureItems.Add(data.GetSecureItem());
						}

					}
					else if ((fline.Count() == 6) && (!IsPidLike(fline[0].Trim())) /*&& (fline[1].Trim().IndexOf('.') >= 0)*/) // may be url login
					{
						var data = new PBSubType.PasswordVault.Login();

						data.SiteName = fline[0].Trim();
						data.UserName = fline[2].Trim();
						data.Password = fline[4].Trim();
						data.Url = fline[1].Trim();
						data.Notes = fline[4].Trim();
						if (string.IsNullOrWhiteSpace(data.SiteName))
						{
							data.SiteName = data.Url;
						}
						res.SecureItems.Add(data.GetSecureItem());
					}
					else if ((fline.Count() == 1) && (new Common().IsEmailValid(fline[0].Trim()))) // email
					{
						var data = new PBSubType.PersonalInfo.Email();

						data.NickName = fline[0].Trim();
						data.Address = fline[0].Trim();

						res.SecureItems.Add(data.GetSecureItem());
					}
					else if ((fline.Count() >= 3) && (IsPhoneLikeLineId(fline[0].Trim())))
					{
						var data = new PBSubType.PersonalInfo.PhoneNumber();

						data.Nickname = fline[0].Trim();
						data.Number = fline[1].Trim();
						data.Notes = "";

						for (int i = 2; i < fline.Count(); i++)
							if (fline[i].Trim().Length != 0) 
								data.Notes += fline[i].Trim() + " ";

						res.SecureItems.Add(data.GetSecureItem());
					}
					else if (!IsPidLike(fline[0].Trim()))
					{
							res.ImportMessages.Add(string.Format(CultureInfo.InvariantCulture, "Input file:{0}. Error in line:{1}.", args.FilePath, lncnt));
					}
					else
					{
						res.ImportMessages.Add(string.Format(CultureInfo.InvariantCulture, "Input file:{0}. Error in line:{1}.", args.FilePath, lncnt));
					}

				}
				catch(Exception ex)
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
