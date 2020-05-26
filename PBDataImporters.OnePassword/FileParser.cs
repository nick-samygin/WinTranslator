using Newtonsoft.Json.Linq;
using PasswordBoss;
using PasswordBoss.DTO;
using PBDataImporters.Common.TypeParsers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PBDataImporters.OnePassword
{
	internal static class FileParser
	{
		public static string PackSecureContents(JObject JsonData)
		{
			string result = "";

			foreach (JProperty prop in JsonData["secureContents"].Value<JObject>().Children())
			{
				if ((prop.HasValues) && (prop.Name.ToUpperInvariant() != "SECTIONS") && (prop.Name.ToUpperInvariant() != "FIELDS") && (!string.IsNullOrEmpty((string)prop.Value)))
					result += prop.Name.ToUpperInvariant() + ":" + prop.Value + "; ";
			}

			return result;
		}
	}
}
