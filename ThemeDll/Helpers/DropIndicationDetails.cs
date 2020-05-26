using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls;

namespace PasswordBoss.Helpers
{
    public class DropIndicationDetails : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private IList currentDraggedItem;
        private DropPosition currentDropPosition;
        private object currentDraggedOverItem;

        public object CurrentDraggedOverItem
        {
            get
            {
                return currentDraggedOverItem;
            }
            set
            {
                if (this.currentDraggedOverItem != value)
                {
                    currentDraggedOverItem = value;
                    RaisePropertyChanged("CurrentDraggedOverItem");
                }
            }
        }

        public int DropIndex { get; set; }

        public DropPosition CurrentDropPosition
        {
            get
            {
                return this.currentDropPosition;
            }
            set
            {
                if (this.currentDropPosition != value)
                {
                    this.currentDropPosition = value;
                    RaisePropertyChanged("CurrentDropPosition");
                }
            }
        }

        public IList CurrentDraggedItem
        {
            get
            {
                return currentDraggedItem;
            }
            set
            {
                if (currentDraggedItem != value)
                {
                    currentDraggedItem = value;
                    RaisePropertyChanged("CurrentDraggedItem");
                    RaisePropertyChanged("HasOne");
                }
            }
        }


        public bool HasOne
        {
            get
            {
                if (CurrentDraggedItem == null)
                    return false;

                return CurrentDraggedItem.Count == 1;
            }

        }


        private bool _isValidDrop;
        public bool IsValidDrop
        {
            get
            {
                return _isValidDrop;
            }
            set
            {
                if (_isValidDrop != value)
                {
                    _isValidDrop = value;
                    RaisePropertyChanged("IsValidDrop");
                }
            }
        }

    }
}
