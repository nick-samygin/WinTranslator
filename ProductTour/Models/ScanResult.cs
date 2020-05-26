using ProductTour.BusinessLayer;
using System.Linq;

namespace ProductTour.Models
{
    public class ScanResult : IScanSummary
    {
        public int Duplicate { get { return Risks.Count(r => r.GetHighestRisk() == ScanRiskFlag.Duplicate); } }
        public int Weak { get { return Risks.Count(r => r.GetHighestRisk() == ScanRiskFlag.Weak); } }
        public int Insecure { get { return Risks.Count(); } }

        private ScanItem[] scanList;
        public ScanItem[] ScanList
        {
            get { return scanList != null ? scanList : new ScanItem[0]; }
            set { scanList = value; }
        }

        private ScanRiskFlag[] Risks
        {
            get
            {
                return scanList.Select(r => r.Risk).ToArray();
            }
        }

        public ScanResult()
        {
            scanList = new ScanItem[0];
        }

        public ScanResult(ScanItem[] scanResult)
        {
            this.scanList = scanResult;
        }
    }
}
