using PasswordBoss;
using ProductTour.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductTour.BusinessLayer
{
	public interface IRiskCalculator
	{
		ScanRiskFlag GetRisk(string plainPassword);
		ScanItem[] MarkDuplicates(IEnumerable<ScanItem> scanList);
    }

	public class RiskCalculator : IRiskCalculator
	{
		private static readonly PasswordScanner scanner = new PasswordScanner();

		public ScanRiskFlag GetRisk(string plainPassword)
		{
			var strength = scanner.scanPassword(plainPassword);
			if (strength == PasswordScanner.Strength.WEAK)
				return ScanRiskFlag.Weak;
			else
				return ScanRiskFlag.Insecure;
		}

		public ScanItem[] MarkDuplicates(IEnumerable<ScanItem> scanList)
		{
			var sorted = scanList
				.OrderBy(s => s.Password)
				.ToArray();

			for (int i = 1; i < sorted.Length; i++)
			{
				var current = sorted[i].Password;
				var previous = sorted[i - 1].Password;

				if (current.Equals(previous))
				{
					sorted[i].Risk = sorted[i].Risk.Add(ScanRiskFlag.Duplicate);
				}
			}

			return sorted
				.OrderBy(s => s.Site)
				.ThenBy(s => s.Username)
				.ToArray();
		}
	}
}
