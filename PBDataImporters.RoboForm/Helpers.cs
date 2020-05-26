using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBDataImporters.RoboForm
{
	public static class Helpers
	{
		internal static string GetValue(Dictionary<string, string> dic, string key)
		{
			string result = "";

			if (!dic.TryGetValue(key, out result)) result = "";

			return result;
		}
	}
}
