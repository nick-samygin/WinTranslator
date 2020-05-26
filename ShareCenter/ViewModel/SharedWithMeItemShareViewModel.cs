using System;
using System.Collections.Generic;

namespace PasswordBoss.ViewModel
{
    class SharedWithMeItemShareViewModel : ShareBaseViewModal
    {
        private DateTime _sentDate;
        public DateTime SentDate
        {
            get { return _sentDate; }
            set
            {
                _sentDate = value;
                RaisePropertyChanged("SentDate");
            }
        }

        private SharedWithMeItemState _state;
        public SharedWithMeItemState State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }

        public SharedWithMeItemShareViewModel(string id, string name, bool isPending, IEnumerable<object> items, bool isExpanded,
            DateTime sentDate, SharedWithMeItemState state)
            : base(id, name, isPending, items, isExpanded)
        {
            SentDate = sentDate;
            State = state;
        }
    }
}
