namespace ProductTour.Models
{
    public interface IScanSummary
    {
        int Duplicate { get; }
        int Weak { get; }
        int Insecure { get; } // total passwords

    }

    public static class ScanSummaryHelper
    {
		//public static int Total(this IScanSummary target)
		//{
		//	return target.Duplicate + target.Insecure + target.Weak;
		//}

        public static bool IsEmpty(this IScanSummary target)
        {
            return target.Duplicate == 0 && target.Insecure == 0 && target.Weak == 0;
        }
    }
}
