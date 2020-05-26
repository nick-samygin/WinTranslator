using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : UserControl, INotifyPropertyChanged
    {
        public enum CustomMessageBoxTypeEnum { Info, Confirmation }
        
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public CustomMessageBoxTypeEnum CustomMessageBoxType { get; set; }

        public bool OkButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Info; } }
        public bool CancelButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Confirmation; } }
        public bool ConfirmButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Confirmation; } }

       
        public static DependencyProperty MessageBoxCancelCommandProperty
        = DependencyProperty.Register(
            "MessageBoxCancelCommand",
            typeof(ICommand),
            typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxConfirmCommandProperty
        = DependencyProperty.Register(
            "MessageBoxConfirmCommand",
            typeof(ICommand),
            typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxOkCommandProperty
        = DependencyProperty.Register(
            "MessageBoxOkCommand",
            typeof(ICommand),
            typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxVisibilityProperty
        = DependencyProperty.Register(
            "MessageBoxVisibility",
            typeof(bool),
            typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxOkButtonTextProperty
        = DependencyProperty.Register(
            "MessageBoxOkButtonText",
            typeof(string),
            typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxConfirmButtonTextProperty
       = DependencyProperty.Register(
           "MessageBoxConfirmButtonText",
           typeof(string),
           typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxCancelButtonTextProperty
       = DependencyProperty.Register(
           "MessageBoxCancelButtonText",
           typeof(string),
           typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxHeaderTextProperty
       = DependencyProperty.Register(
           "MessageBoxHeaderText",
           typeof(string),
           typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxTextProperty
       = DependencyProperty.Register(
           "MessageBoxText",
           typeof(string),
           typeof(CustomMessageBox));


        public ICommand MessageBoxCancelCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxCancelCommandProperty);
            }

            set
            {
                SetValue(MessageBoxCancelCommandProperty, value);
                RaisePropertyChanged("MessageBoxCancelCommand");
            }
        }

        public ICommand MessageBoxConfirmCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxConfirmCommandProperty);
            }

            set
            {
                SetValue(MessageBoxConfirmCommandProperty, value);
                RaisePropertyChanged("MessageBoxConfirmCommand");
            }
        }

        public ICommand MessageBoxOkCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxOkCommandProperty);
            }

            set
            {
                SetValue(MessageBoxOkCommandProperty, value);
                RaisePropertyChanged("MessageBoxOkCommand");
            }
        }

        public bool MessageBoxVisibility
        {
            get
            {
                return (bool)GetValue(MessageBoxVisibilityProperty);
            }

            set
            {
                SetValue(MessageBoxVisibilityProperty, value);
                RaisePropertyChanged("MessageBoxVisibility");
            }
        }

        public string MessageBoxOkButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxOkButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxOkButtonTextProperty, value);
                RaisePropertyChanged("MessageBoxOkButtonText");
            }
        }

        public string MessageBoxConfirmButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxConfirmButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxConfirmButtonTextProperty, value);
                RaisePropertyChanged("MessageBoxConfirmButtonText");
            }
        }

        public string MessageBoxCancelButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxCancelButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxCancelButtonTextProperty, value);
                RaisePropertyChanged("MessageBoxCancelButtonText");
            }
        }

        public string MessageBoxHeaderText
        {
            get
            {
                return (string)GetValue(MessageBoxHeaderTextProperty);
            }
            set
            {
                SetValue(MessageBoxHeaderTextProperty, value);
                RaisePropertyChanged("MessageBoxHeaderText");
            }
        }

        public string MessageBoxText
        {
            get
            {
                return (string)GetValue(MessageBoxTextProperty);
            }
            set
            {
                SetValue(MessageBoxTextProperty, value);
                RaisePropertyChanged("MessageBoxText");
            }
        }

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
