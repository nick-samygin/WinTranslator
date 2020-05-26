using System.Collections.Generic;

namespace PBDataImporters.LastPass
{
	internal static class Helpers
	{
		internal static string GetValue(Dictionary<string, string> dic, string key)
		{
			string result = "";

			if (!dic.TryGetValue(key, out result)) result = "";

			return result;
		}

		internal static string GetValueFormated(Dictionary<string, string> dic, string key)
		{
			string result = "";

			if (dic.TryGetValue(key, out result))
				result = key + ":" + result + "; ";
			else result = "";

			return result;
		}

		internal static string GetMonthValue(string monthName)
		{
			if (monthName == "January") return "01";
			else if (monthName == "February") return "02";
			else if (monthName == "March") return "03";
			else if (monthName == "April") return "04";
			else if (monthName == "May") return "05";
			else if (monthName == "June") return "06";
			else if (monthName == "July") return "07";
			else if (monthName == "August") return "08";
			else if (monthName == "September") return "09";
			else if (monthName == "October") return "10";
			else if (monthName == "November") return "11";
			else if (monthName == "December") return "12";
			else return "00";
		}
	}
}
