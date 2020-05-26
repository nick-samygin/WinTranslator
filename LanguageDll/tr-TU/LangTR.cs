using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PasswordBoss
{
    [Export(typeof(ITextResource))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class LangTR : ITextResource
    {
        public string Lang
        {
            get { return "tr"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/tr-TU;Component/resources/tr-TU.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
