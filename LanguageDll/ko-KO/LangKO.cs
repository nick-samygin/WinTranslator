using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PasswordBoss
{
    [Export(typeof(ITextResource))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    class LangKO : ITextResource
    {
        public string Lang
        {
            get { return "ko"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/ko-KO;Component/resources/ko-KO.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
