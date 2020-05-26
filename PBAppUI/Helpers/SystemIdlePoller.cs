using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace PasswordBoss.Helpers
{
	public class SystemIdlePoller : IDisposable
	{
		private readonly ILogger logger = Logger.GetLogger(typeof(SystemIdlePoller));

		[StructLayout(LayoutKind.Sequential)]
		struct LASTINPUTINFO
		{
			public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

			[MarshalAs(UnmanagedType.U4)]
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.U4)]
			public UInt32 dwTime;
		}
		
		private const int SPI_GETSCREENSAVERRUNNING = 0x0072;
				
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool SystemParametersInfo(int action, int param, ref int retval, int updini);

		private readonly Timer poller;

		public SystemIdlePoller()
		{
			poller = new Timer(PollerCallback, this, 1000, 1000);
		}
		
		private void PollerCallback(object state)
		{
			int active = 0;
			SystemParametersInfo(SPI_GETSCREENSAVERRUNNING, 0, ref active, 0);
			WriteIdleRegistry(active);
		}

		private void WriteIdleRegistry(int active)
		{
			var keyName = @"SOFTWARE\PasswordBoss";
			var valueName = "LastScreensaverTime";

			RegistryKey hkcuPasswordBoss = null;
			try
			{
				hkcuPasswordBoss = Registry.CurrentUser.OpenSubKey(keyName, true);

				if (hkcuPasswordBoss == null)
				{
					hkcuPasswordBoss = Registry.CurrentUser.CreateSubKey(keyName);
				}
				if (active == 1)
				{
					hkcuPasswordBoss.SetValue(valueName, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					if (hkcuPasswordBoss.GetValue(valueName) != null)
					{
						hkcuPasswordBoss.DeleteValue(valueName);
					}
                }
			}
			catch(Exception ex)
			{
				logger.Error(ex.ToString());
			}
			finally
			{
				if (hkcuPasswordBoss != null)
				{
					hkcuPasswordBoss.Close();
				}
			}
		}

		public void Dispose()
		{
			poller.Dispose();
		}
	}
}
