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
    /// Interaction logic for CustomMessageBoxUserControl.xaml
    /// </summary>
    public partial class CustomMessageBoxUserControl : UserControl, INotifyPropertyChanged
    {
        public enum CustomMessageBoxTypeEnum { Info, Confirmation }

        public CustomMessageBoxUserControl()
        {
            InitializeComponent();
        }

        public CustomMessageBoxDialog WindowObj = null;

        public CustomMessageBoxTypeEnum CustomMessageBoxType { get; set; }

        public bool OkButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Info; } }
        public bool CancelButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Confirmation; } }
        public bool ConfirmButtonVisible { get { return this.CustomMessageBoxType == CustomMessageBoxTypeEnum.Confirmation; } }

       
        public static DependencyProperty MessageBoxDialogCancelCommandProperty
        = DependencyProperty.Register(
            "MessageBoxDialogCancelCommand",
            typeof(ICommand),
            typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogConfirmCommandProperty
        = DependencyProperty.Register(
            "MessageBoxDialogConfirmCommand",
            typeof(ICommand),
            typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogOkCommandProperty
        = DependencyProperty.Register(
            "MessageBoxDialogOkCommand",
            typeof(ICommand),
            typeof(CustomMessageBoxUserControl));

        public static readonly DependencyProperty MessageBoxDialogVisibilityProperty =
        DependencyProperty.Register("MessageBoxDialogVisibility", typeof(bool), typeof(CustomMessageBoxUserControl), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
            PropertyChangedCallback = MessageBoxDialogVisibilityPropertyChanged,
        });

        //public static DependencyProperty MessageBoxDialogVisibilityProperty
        //= DependencyProperty.Register(
        //    "MessageBoxDialogVisibility",
        //    typeof(bool),
        //    typeof(CustomMessageBox));

        public static DependencyProperty MessageBoxDialogOkButtonTextProperty
        = DependencyProperty.Register(
            "MessageBoxDialogOkButtonText",
            typeof(string),
            typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogConfirmButtonTextProperty
       = DependencyProperty.Register(
           "MessageBoxDialogConfirmButtonText",
           typeof(string),
           typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogCancelButtonTextProperty
       = DependencyProperty.Register(
           "MessageBoxDialogCancelButtonText",
           typeof(string),
           typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogHeaderTextProperty
       = DependencyProperty.Register(
           "MessageBoxDialogHeaderText",
           typeof(string),
           typeof(CustomMessageBoxUserControl));

        public static DependencyProperty MessageBoxDialogTextProperty
       = DependencyProperty.Register(
           "MessageBoxDialogText",
           typeof(string),
           typeof(CustomMessageBoxUserControl));


        public ICommand MessageBoxDialogCancelCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxDialogCancelCommandProperty);
            }

            set
            {
                SetValue(MessageBoxDialogCancelCommandProperty, value);
            }
        }

        public ICommand MessageBoxDialogConfirmCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxDialogConfirmCommandProperty);
            }

            set
            {
                SetValue(MessageBoxDialogConfirmCommandProperty, value);
            }
        }

        public ICommand MessageBoxDialogOkCommand
        {
            get
            {
                return (ICommand)GetValue(MessageBoxDialogOkCommandProperty);
            }

            set
            {
                SetValue(MessageBoxDialogOkCommandProperty, value);
            }
        }

        public bool MessageBoxDialogVisibility
        {
            get
            {
                return (bool)GetValue(MessageBoxDialogVisibilityProperty);
            }

            set
            {
                SetValue(MessageBoxDialogVisibilityProperty, value);
            }
        }

        public string MessageBoxDialogOkButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxDialogOkButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxDialogOkButtonTextProperty, value);
            }
        }

        public string MessageBoxDialogConfirmButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxDialogConfirmButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxDialogConfirmButtonTextProperty, value);
            }
        }

        public string MessageBoxDialogCancelButtonText
        {
            get
            {
                return (string)GetValue(MessageBoxDialogCancelButtonTextProperty);
            }
            set
            {
                SetValue(MessageBoxDialogCancelButtonTextProperty, value);
            }
        }

        public string MessageBoxDialogHeaderText
        {
            get
            {
                return (string)GetValue(MessageBoxDialogHeaderTextProperty);
            }
            set
            {
                SetValue(MessageBoxDialogHeaderTextProperty, value);
            }
        }

        public string MessageBoxDialogText
        {
            get
            {
                return (string)GetValue(MessageBoxDialogTextProperty);
            }
            set
            {
                SetValue(MessageBoxDialogTextProperty, value);
            }
        }

        private static void MessageBoxDialogVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var val = (bool)e.NewValue;

            var window = ((CustomMessageBoxUserControl)d).WindowObj;

            if ((bool)e.NewValue == true)
            {
                window = new CustomMessageBoxDialog();
                ((CustomMessageBoxUserControl)d).WindowObj = window;
                window.Owner =  Window.GetWindow(d);
                window.Height = window.Owner.ActualHeight;
                window.Width = window.Owner.ActualWidth;
                window.Top = window.Owner.Top;
                window.Left = window.Owner.Left;
                window.DataContext = d;
                window.WindowStartupLocation = window.Owner.WindowState == WindowState.Maximized ? WindowStartupLocation.CenterOwner : WindowStartupLocation.Manual;
                window.ShowDialog();
            }
            else
            {
                if (window != null)
                {
                    window.Close();
                    window = null;
                }
            }
        }

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
