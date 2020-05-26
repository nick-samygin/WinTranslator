using PasswordBoss;
using PasswordBoss.Browsers;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PBDataImporters.InternetExplorer
{
	[Export(typeof(BrowserImportStrategyBase))]
	[PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
	public class ExplorerImportStrategy : BrowserImportStrategyBase
	{
		public override BrowsersFlag BrowserType
		{
			get { return BrowsersFlag.InternetExplorer; }
		}

		 [ImportingConstructor]
		public ExplorerImportStrategy([Import(typeof(IResolver))] IResolver resolver)
			: base(resolver)
		{

		}

		protected override List<LoginInfo> GenerateLoginInfoList()
		{
			if (PasswordBoss.Browsers.BrowserVersionGetter.GetIEVersion() != null)
			{
				return pbData.GetIEAccounts();
			}
			return new List<LoginInfo>();
		}
	}
}
