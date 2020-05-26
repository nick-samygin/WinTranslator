using PasswordBoss;
using PasswordBoss.Helpers;
using ProductTour.Models;
using System;

namespace ProductTour.ViewModel.Scans
{
    public class ScanPopupNotificationViewModel : StartupScanViewModel, IScanSummary
    {
        private readonly IScanSummary scanSummary;
        public ScanPopupNotificationViewModel(IResolver resolver, IScanSummary scanSummary, Action onClose)
            : base(resolver, onClose)
        {
            this.scanSummary = scanSummary;

        }

        public int Duplicate
        {
            get { return scanSummary.Duplicate; }
        }

        public int Weak
        {
            get { return scanSummary.Weak; }
        }

        public int Insecure
        {
            get { return scanSummary.Insecure; }
        }

    }
}
