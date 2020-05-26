using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProductTour.Models
{
	public enum ScanRiskFlag
	{
        Insecure = 0,   // all password insecure because we can read them in plain string, no matter how strong is it
		Weak = 1,		// if exposed password weak
		Duplicate = 2,  // if exposed password duplicate, no matter how strong is it
	}

	public static class ScanRiskFlagHelper
	{
        // RiskFlag - text, shield image, color
        private static List<RiskHelperTuple> riskDefinitions = new List<RiskHelperTuple>();

        static ScanRiskFlagHelper()
        {
            Action<ScanRiskFlag, string, string, string> addDefinition = (flg, def, img, colr) =>
            {
                riskDefinitions.Add(new RiskHelperTuple()
                {
                    Flag = flg,
                    Definition = def,
                    Image = img,
                    Color = colr
                });
            };

            addDefinition(ScanRiskFlag.Weak, 
                "Weak",
                @"/image;component/images/shield-yellow-16x18.png",
                "onboardWeakColor");

            addDefinition(ScanRiskFlag.Insecure,
                "Onboardv4StatusInsecure",
                @"/image;component/images/shield-red-16x18.png",                 
                "onboardInsecureColor");

            addDefinition(ScanRiskFlag.Duplicate,
                "Onboardv4StatusDuplicate",
                @"/image;component/images/shield-orange-16x18.png",                
                "onboardDuplicateColor");
        }

        public static ScanRiskFlag GetHighestRisk(this ScanRiskFlag flag)
        {
            if ((flag & ScanRiskFlag.Duplicate) != 0)
                return ScanRiskFlag.Duplicate;
            else if ((flag & ScanRiskFlag.Weak) != 0)
                return ScanRiskFlag.Weak;
            else
                return ScanRiskFlag.Insecure;
        }

        public static ScanRiskFlag Add(this ScanRiskFlag flag, ScanRiskFlag value)
        {
            flag = flag | value;
            return flag;
        }

        public class RiskHelperTuple
        {
            public ScanRiskFlag Flag { get; set; }
            public string Definition { get; set; }
            public string Image { get; set; }
            public string Color { get; set; }
        }

        private static RiskHelperTuple Find(ScanRiskFlag flag)
        {
            if (flag == null)
                throw new NullReferenceException("flag");

            var f = flag.GetHighestRisk();
            return riskDefinitions.Single(rd => rd.Flag.Equals(f));
        }

        public static string GetDefinition(this ScanRiskFlag flag)
        {
            return Find(flag).Definition;
        }

        public static string GetImage(this ScanRiskFlag flag)
        {
            return Find(flag).Image;
        }

        public static string GetColor(this ScanRiskFlag flag)
        {
            return Find(flag).Color;
        }
	}
}
