using PasswordBoss;
using PasswordBoss.PBAnalytics;
using System;

namespace ProductTour.ViewModel.Scans
{
    class StartupScanDisabledViewModel : StartupScanViewModel
    {
        public StartupScanDisabledViewModel(IResolver resolver, Action onClose)
            : base(resolver, onClose)
        {
        }
    }
}