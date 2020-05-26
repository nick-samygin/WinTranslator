using PasswordBoss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SecureItemsCommon.Helpers
{
    public class ContextAction : INotifyPropertyChanged, IContextAction
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IContextAction> SubItems { get; set; }

        public bool IsFolderList { get; set; }
      
        public Visibility Visibility { get; set; }
        private ImageSource icon;
        public ImageSource Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        private ImageSource iconHover;
        public ImageSource IconHover
        {
            get { return iconHover; }
            set
            {
                iconHover = value;
                RaisePropertyChanged("IconHover");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private ICommand action;
        public ICommand Action
        {
            get { return action; }
            set
            {
                action = value;
                RaisePropertyChanged("Action");
            }
        }



    }

    public class SubContextAction:ContextAction, ISubContextAction
    {  
        public string ActionParameter { get; set; }

        public SubContextAction()
        {
            IsFolderList = true;
        }

    }
}
