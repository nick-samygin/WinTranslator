using PasswordBoss;
using PasswordBoss.PBAnalytics;
using PasswordBoss.ViewModel;
using System;

namespace ProductTour.ViewModel.Scans
{
    public class ScanResultNoItemsViewModel : StartupScanViewModel
    {
        public ScanResultNoItemsViewModel(IResolver resolver, Action onClose) : base(resolver, onClose)
        {
            
        }
    }
}
