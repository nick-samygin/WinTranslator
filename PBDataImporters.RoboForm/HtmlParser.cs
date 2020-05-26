using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PBDataImporters.RoboForm
{
	internal static class HtmlParser
	{
		internal const string IdentitiesListTitle = "ROBOFORM IDENTITIES LIST";
		internal const string LoginsListTitle = "ROBOFORM LOGINS LIST";
		internal const string SafenotesListTitle = "ROBOFORM SAFENOTES LIST";

		private static bool ClassEqualsTo(this HtmlNode node, string target)
		{
			return node.GetAttributeValue("class", null).Equals(target, StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsCaption(this HtmlNode node)
		{
			bool isCaption = false;

			if (node.Name.Equals("td", StringComparison.OrdinalIgnoreCase))
			{
				isCaption = node.ClassEqualsTo("caption");
			}
			else
			{
				isCaption = node.ClassEqualsTo("idsubcaption");
			}

			return isCaption;
		}

		private static RoboformExportType GetExportType(HtmlDocument html)
		{
			HtmlNodeCollection title = html.DocumentNode.SelectNodes("//title");
			string roboFormTitle = title[0].InnerText.ToUpperInvariant().Trim();

			var currentRBExportType = RoboformExportType.SafeNotes;

			if (roboFormTitle == SafenotesListTitle)
			{
				currentRBExportType = RoboformExportType.SafeNotes;
			}
			else if (roboFormTitle == LoginsListTitle)
			{
				currentRBExportType = RoboformExportType.Logins;
			}
			else if (roboFormTitle == IdentitiesListTitle)
			{
				currentRBExportType = RoboformExportType.Identities;
			}
			else
			{
				currentRBExportType = RoboformExportType.SafeNotes;
			}
			return currentRBExportType;
		}

		private static IEnumerable<HtmlNode> GetBody(HtmlDocument html)
		{
			HtmlNodeCollection nc = html.DocumentNode.SelectNodes("//body");
			return nc.Descendants();
		}

		private static bool NeedToReadInnerText(HtmlNode d)
		{
			bool needReadInnerText = false;
			if (d.Name.Equals("td"))
			{
				needReadInnerText = d.ClassEqualsTo("idsubcaption");
			}
			else
			{
				needReadInnerText = !string.IsNullOrEmpty(d.InnerText);
			}
			return needReadInnerText;
		}

		private static bool IsDictionary(HtmlNode trNode)
		{
			var currentNode = trNode.XPath;

			while ((trNode.Name.ToUpperInvariant() != "TR")
				&& (trNode.Name.ToUpperInvariant() != "TABLE")
				&& (trNode.Name.ToUpperInvariant() != "BODY"))
			{
				trNode = trNode.ParentNode;
			}

			if (trNode.Name.ToUpperInvariant() != "TR")
			{
				currentNode = "Parent <TR> node not found in node: " + currentNode;
				throw new ArgumentException(currentNode);
			}

			int colcnt = trNode.SelectNodes("td[@class='field']").Count();
			return colcnt == 2;
		}

		private static RoboFormData[] ParseFile(HtmlDocument html, List<string> importMessages, out RoboformExportType currentRBExportType)
		{
			if (html == null)
				throw new ArgumentNullException("html");

			if (importMessages == null)
				throw new ArgumentNullException("importMessages");

			currentRBExportType = GetExportType(html);

			int lncnt = 0;
			List<RoboFormData> dataList = new List<RoboFormData>();
			RoboFormData roboData = null;
			KVSwitch currKV = KVSwitch.Key;
			string currKey = "";
			string currValue = "";
			string currentIdentityName = "";			
			foreach (var d in GetBody(html))
			{
				lncnt++;
				try
				{
					if (d.IsCaption())
					{
						currKV = KVSwitch.Key;
						currKey = "";
						currValue = "";
						if (roboData != null)
						{
							roboData.IsAdded = true;
							dataList.Add(roboData);
						}
						roboData = new RoboFormData();

						if (d.ClassEqualsTo("caption"))
							currentIdentityName = d.InnerText;

			
						if (NeedToReadInnerText(d))
							roboData.Caption = d.InnerText;  // <TD> node also can be "caption" class (no text in it, reason unknown)

						roboData.IdentityName = currentIdentityName;
					}
					else if (d.ClassEqualsTo("subcaption"))
					{
						if (!string.IsNullOrEmpty(d.InnerText))
							roboData.SubCaption = d.InnerText;
					}
					else if (d.ClassEqualsTo("field"))
					{
						roboData.DataList.Add(d.InnerHtml.Replace("<br>", " ").Replace("<br/>", " ").Replace("<br />", " "));
						roboData.IsDictionary = IsDictionary(d);

						switch (currKV)
						{
							case KVSwitch.Key:
								currKey = d.InnerText.Replace(":", "");
								currKV = KVSwitch.Value;
								break;

							case KVSwitch.Value:
								currValue = d.InnerHtml.Replace("<br>", " ").Replace("<br/>", " ").Replace("<br />", " ");

								while (roboData.Data.ContainsKey(currKey))
									currKey += "_";

								roboData.Data.Add(currKey, currValue);

								currKV = KVSwitch.Key;
								break;
						}
					}

				}
				catch (Exception ex)
				{
					if (ex is IndexOutOfRangeException || ex is ArgumentException || ex is NullReferenceException)
					{
						importMessages.Add(string.Format(CultureInfo.InvariantCulture, "Error in line:{0}.", lncnt));
					}
					else
					{
						throw;
					}
				}

			}

			if ((roboData != null) && (!roboData.IsAdded))
				dataList.Add(roboData);

			return dataList.ToArray();
		}

		public static RoboFormData[] ParseFile(string filePath, List<string> importMessages, out RoboformExportType currentRBExportType)
		{
			if (filePath == null)
				throw new ArgumentNullException("filePath");

			if (importMessages == null)
				throw new ArgumentNullException("importMessages");

			HtmlDocument html = new HtmlDocument();
			try
			{
				string source = System.IO.File.ReadAllText(filePath, Encoding.ASCII).Replace(@"&shy;", "");

				source = System.Net.WebUtility.HtmlDecode(source);
				html.LoadHtml(source);
			}
			catch
			{
				throw new FileLoadException(string.Format(CultureInfo.InvariantCulture, "Error while loading file '{0}'.", filePath));
			}

			return ParseFile(html, importMessages, out currentRBExportType);
		}
	}
}
