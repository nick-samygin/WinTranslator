using System.Collections.Generic;

namespace PBDataImporters.RoboForm
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")] // this is correct spelling!
	internal class RoboFormData
	{
		internal string Caption { get; set; }
		internal string IdentityName { get; set; }
		internal string SubCaption { get; set; }

		internal List<string> DataList { get; private set; }
		internal Dictionary<string, string> Data { get; private set; }
		internal bool IsDictionary { get; set; }
		internal bool IsAdded { get; set; }

		internal RoboFormData()
		{
			Caption = "";
			IdentityName = "";
			SubCaption = "";
			DataList = new List<string>();
			Data = new Dictionary<string, string>();
			IsDictionary = false;
			IsAdded = false;
		}
	}

}
