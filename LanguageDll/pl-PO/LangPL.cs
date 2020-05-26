using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;


namespace PasswordBoss
{
    [Export(typeof(ITextResource))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class LangPL : ITextResource
    {
        public string Lang
        {
            get { return "pl"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/pl-PO;Component/resources/pl-PO.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
