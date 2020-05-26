using PasswordBoss;
using PasswordBoss.Browsers;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PBDataImporters.Chrome
{
	[Export(typeof(BrowserImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public class ChromeImportStrategy : BrowserImportStrategyBase
	{
		public override BrowsersFlag BrowserType
		{
			get { return BrowsersFlag.Chrome; }
		}

		 [ImportingConstructor]
		public ChromeImportStrategy([Import(typeof(IResolver))] IResolver resolver)
			: base(resolver)
		{

		}

		protected override List<LoginInfo> GenerateLoginInfoList()
		{
			if (PasswordBoss.Browsers.BrowserVersionGetter.GetChromeVersion() != null)
			{
				return pbData.GetChromeAccounts();
			}
			return new List<LoginInfo>();
		}
	}
}
