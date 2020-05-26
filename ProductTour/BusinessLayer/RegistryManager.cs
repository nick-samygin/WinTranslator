using Microsoft.Win32;
using PasswordBoss;
using ProductTour.Models;
using System;

namespace ProductTour.BusinessLayer
{
	public interface IRegistryManager
	{
		bool IsStartupScanEnabled();

        IScanSummary GetScanSummary();

        void PutScanSummaryToRegistry(IScanSummary summary);
	}

	public class RegistryManager : IRegistryManager
	{
        private class RegistryScanSummary : IScanSummary
        {
            public int Duplicate { get; set; }
            public int Weak { get; set; }
            public int Insecure { get; set; }
        }
		private static class RegistryKeys
		{
			public static string StartupScan = "startupscan"; // 1 / 0
			public static string ScanInsecure = "scan1";
			public static string ScanWeak = "scan2";
			public static string ScanDuplicate = "scan3";
		}

		private readonly ILogger logger = Logger.GetLogger(typeof(RegistryManager));


        public IScanSummary GetScanSummary()
		{
            return new RegistryScanSummary()
			{
                Insecure = ReadInt(RegistryKeys.ScanInsecure),
				Duplicate = ReadInt(RegistryKeys.ScanDuplicate),
				Weak = ReadInt(RegistryKeys.ScanWeak)
			};
		}

		public bool IsStartupScanEnabled()
		{
			return ReadInt(RegistryKeys.StartupScan) == 1;
		}


		private int ReadInt(string valueName)
		{
			int result = 0;
			try
			{
				RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\PasswordBoss");

				if (rk != null)
				{
                    object value = rk.GetValue(valueName);
                    
                    if (value != null)
                    {
                        result = Convert.ToInt32(value);
                    }
					rk.Close();
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("Unable to get {0} key, assume {0} = 0", valueName));
				logger.Error(ex.ToString());
			}

			return result;
		}

		private void WriteInt(string valueName, int value)
		{
			try
			{
				RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\PasswordBoss", true);
				if (rk != null)
				{
					rk.SetValue(valueName, value);
					rk.Close();
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("Unable to set {0} key, assume {0} = 0", valueName));
				logger.Error(ex.ToString());
			}
		}

        public void PutScanSummaryToRegistry(IScanSummary summary)
		{
            WriteInt(RegistryKeys.ScanInsecure, summary.Insecure);
			WriteInt(RegistryKeys.ScanDuplicate, summary.Duplicate);
			WriteInt(RegistryKeys.ScanWeak, summary.Weak);
		}
	}
}