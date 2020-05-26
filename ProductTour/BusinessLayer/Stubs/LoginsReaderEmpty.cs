using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductTour.BusinessLayer.Stubs
{
    public class LoginsReaderEmpty : ILoginsReader
    {
        public Models.ScanResult ScanBrowsers()
        {
			var res = new Models.ScanResult();
			new RegistryManager().PutScanSummaryToRegistry(res);

            return res;
        }

        public bool IsScanCompleted
        {
            get { return true;}
        }
    }
}
