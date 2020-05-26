using System;
using System.Collections.Generic;
using PasswordBoss.Helpers;

namespace PasswordBoss.ViewModel
{
    class SharedWithMeViewModel : ShareBaseViewModal
    {
        private RelayCommand _declineAllCommand;

        public RelayCommand DeclineAllCommand
        {
            get { return _declineAllCommand; }
            set
            {
                _declineAllCommand = value;
                RaisePropertyChanged("DeclineAllCommand");
            }
        }

        private RelayCommand _acceptAllCommand;

        public RelayCommand AcceptAllCommand
        {
            get { return _acceptAllCommand; }
            set
            {
                _acceptAllCommand = value;
                RaisePropertyChanged("AcceptAllCommand");
            }
        }

        public SharedWithMeViewModel(string id, string name, bool isPending, IEnumerable<object> items, bool isExpanded)
            : base(id, name, isPending, items, isExpanded)
        {
            DeclineAllCommand = new RelayCommand(OnDeclineAllCommandHandler);
            AcceptAllCommand = new RelayCommand(OnAcceptAllCommandHandler);
        }

        private void OnAcceptAllCommandHandler(object o)
        {
        }

        private void OnDeclineAllCommandHandler(object o)
        {
        }
    }
}
