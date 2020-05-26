using ProductTour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductTour.BusinessLayer.Stubs
{
    public class LoginsReaderFake : ILoginsReader
    {

        public ScanResult ScanBrowsers()
        {
            var items = new ScanItem[] 
            {
                new ScanItem("google.com", "steve.wise@google.com", "duplicate"),
                new ScanItem("Site2.com", "User2", "duplicate"),
                new ScanItem("Site3.com", "User3", "Password3"),
                new ScanItem("Site4.com", "User1", "VERY_Str0ngPassword_And_long_as_fantasy_lets_to_type_as_much_letters_as_we_can!!!!"),
                new ScanItem("Site5.com", "User2", "wek"),
                new ScanItem("Site6.com", "User3", "wek"),
                new ScanItem("Site7.com", "User1", "wek"),
                new ScanItem("Site8.com", "User2", "weak4duplicate"),
                new ScanItem("Site9.com", "User3", "weak4duplicate"),
            };
            items = new RiskCalculator().MarkDuplicates(items);
			var res = new ScanResult(items);
			new RegistryManager().PutScanSummaryToRegistry(res);

			return res;
            
        }

        public bool IsScanCompleted
        {
            get { return true; }
        }
    }
}
