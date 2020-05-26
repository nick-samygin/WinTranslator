using PasswordBoss;
using PasswordBoss.Browsers;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PBDataImporters.Firefox
{
	[Export(typeof(BrowserImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public class FirefoxImportStrategy : BrowserImportStrategyBase
	{
		public override BrowsersFlag BrowserType
		{
			get { return BrowsersFlag.Firefox; }
		}

		 [ImportingConstructor]
		public FirefoxImportStrategy([Import(typeof(IResolver))] IResolver resolver)
			: base(resolver)
		{

		}

		protected override List<LoginInfo> GenerateLoginInfoList()
		{
			if (PasswordBoss.Browsers.BrowserVersionGetter.GetFFVersion() != null)
			{
				return pbData.GetFFAccounts(() => { return null; });
			}
			return new List<LoginInfo>();
		}
	}
}
