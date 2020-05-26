using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss;
using System.ComponentModel.Composition;

namespace PasswordBoss
{
    [Export(typeof(ITextResource))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class LangFR : ITextResource
    {
        public string Lang
        {
            get { return "fr"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/fr-FR;Component/resources/fr-FR.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
