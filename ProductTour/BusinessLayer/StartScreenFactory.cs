using PasswordBoss;
using ProductTour.BusinessLayer.Stubs;
using ProductTour.Models;
using ProductTour.Views.Scans;
using System;
using System.Linq;
using System.Windows;

namespace ProductTour.BusinessLayer
{
    public static class StartScreenFactory
    {
        private static readonly IRegistryManager registryManager = new RegistryManager();
        public static Window CreateStartupWindow(IResolver resolver)
        {
            if (registryManager.IsStartupScanEnabled())
            {
                var loginsReader = LoginsReaderFactory.CreateLoginsReader(Args, resolver.GetInstanceOf<IPBData>());
                return new StartupScanEnabled(resolver, registryManager, loginsReader, IsAuto());
            }
            else
            {
                return new StartupScanDisabled(resolver);
            }
        }

        private static string[] Args
        {
            get
            {
                return ((IApplication)(Application.Current)).ApplicationArguments.Select(a => a.ToLower()).ToArray();
            }
        }

        private static bool IsAuto()
        {
            return
                Args
                .Any(a => a.Contains("/auto"));
        }
    }
}
