﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace PasswordBoss
{
    [Export(typeof(ITextResource))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class LangES : ITextResource
    {
        public string Lang
        {
            get { return "es"; }
        }

        public string ResourceKey
        {
            get { return "Localization"; }
        }

        public Uri ResourcePath
        {
            get { return new Uri("pack://application:,,,/es-SP;Component/resources/es-SP.xaml", UriKind.RelativeOrAbsolute); }
        }
    }
}
