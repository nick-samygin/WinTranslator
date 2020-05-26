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
    public class LangZH : ITextResource
    {
        public string Lang
        {
            get { return "zh"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/zh-CH;Component/resources/zh-CH.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
