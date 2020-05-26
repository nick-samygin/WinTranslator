using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickZip.UserControls
{
    [TemplatePart(Name = ConfirmButtonPart, Type = typeof(ConfirmationPopupWindow))]
    public class ConfirmationPopupWindow : PopupWindow, ICommandSource
    {
        private const string ConfirmButtonPart = "PART_ConfirmButton";
        private const string AdditionalButtonPart = "PART_AdditionalButton";
        private Button confirmButton;
        private Button additionalButton;

        private EventHandler canExecuteChangedHandler;

        protected Button ConfirmButton
        {
            get { return confirmButton; }
            set
            {
                if(confirmButton != null)
                {
                    confirmButton.Click -= ConfirmButtonClick;
                }

                confirmButton = value;

                if(confirmButton != null)
                {
                    confirmButton.Click += ConfirmButtonClick;
                }
            }
        }

        protected Button AdditionalButton
        {
            get { return additionalButton; }
            set
            {
                if (additionalButton != null)
                {
                    additionalButton.Click -= AdditionalButtonClick;
                }

                additionalButton = value;

                if (additionalButton != null)
                {
                    additionalButton.Click += AdditionalButtonClick;
                }
            }
        }

        private void AdditionalButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseAdditionalCommand();
        }

        private void RaiseAdditionalCommand()
        {
            if (AdditionalCommand != null)
            {
                RoutedCommand rc = AdditionalCommand as RoutedCommand;

                if (rc != null)
                {
                    rc.Execute(AdditionalCommandParameter, null);
                }
                else
                {
                    AdditionalCommand.Execute(AdditionalCommandParameter);
                }
            }
        }

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseCommand();
        }

        private void RaiseCommand()
        {
            if(Command != null)
            {
                RoutedCommand rc = Command as RoutedCommand;

                if(rc != null)
                {
                    rc.Execute(CommandParameter, CommandTarget);
                }
                else
                {
                    Command.Execute(CommandParameter);
                }
            }
        }

        static ConfirmationPopupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConfirmationPopupWindow), new FrameworkPropertyMetadata(typeof(ConfirmationPopupWindow)));
        }

		//public static readonly DependencyProperty ConfirmButtonStyleProperty =
		//	DependencyProperty.Register("ConfirmButtonStyle", typeof(Style), typeof(PopupWindow), new PropertyMetadata(null));

		//public Style ConfirmButtonStyle
		//{
		//	get { return (Style)GetValue(ConfirmButtonStyleProperty); }
		//	set { SetValue(ConfirmButtonStyleProperty, value); }
		//}

		public static readonly DependencyProperty ConfirmButtonStyleProperty =
			DependencyProperty.Register("ConfirmButtonStyle", typeof(Style), typeof(ConfirmationPopupWindow), new PropertyMetadata(null));

		


        public static readonly DependencyProperty ConfirmActionProperty =
            DependencyProperty.Register("ConfirmAction", typeof(string), typeof(ConfirmationPopupWindow), new PropertyMetadata("Action"));

        public string ConfirmAction
        {
            get { return (string)GetValue(ConfirmActionProperty); }
            set { SetValue(ConfirmActionProperty, value); }
        }

        public static readonly DependencyProperty AdditionalActionProperty =
            DependencyProperty.Register("AdditionalAction", typeof(string), typeof(ConfirmationPopupWindow), new PropertyMetadata("Action"));

        public string AdditionalAction
        {
            get { return (string)GetValue(AdditionalActionProperty); }
            set { SetValue(AdditionalActionProperty, value); }
        }

        public static readonly DependencyProperty AdditionalActionVisibilityProperty =
            DependencyProperty.Register("AdditionalActionVisibility", typeof(Visibility), typeof(ConfirmationPopupWindow), new PropertyMetadata(Visibility.Collapsed));

        public Visibility AdditionalActionVisibility
        {
            get { return (Visibility)GetValue(AdditionalActionVisibilityProperty); }
            set { SetValue(AdditionalActionVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ConfirmationButtonVisibilityProperty =
            DependencyProperty.Register("ConfirmationButtonVisibility", typeof(Visibility), typeof(ConfirmationPopupWindow), new PropertyMetadata(Visibility.Visible));

        public Visibility ConfirmationButtonVisibility
        {
            get { return (Visibility)GetValue(ConfirmationButtonVisibilityProperty); }
            set { SetValue(ConfirmationButtonVisibilityProperty, value); }
        }
        
        public static readonly DependencyProperty ContentHeightSizeProperty =
            DependencyProperty.Register("ContentHeightSize", typeof(double), typeof(ConfirmationPopupWindow), new PropertyMetadata(180.0));

        public double ContentHeightSize
        {
            get { return (double)GetValue(ContentHeightSizeProperty); }
            set { SetValue(ContentHeightSizeProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", 
                typeof(ICommand), 
                typeof(ConfirmationPopupWindow),
                new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        public static readonly DependencyProperty AdditionalCommandProperty =
            DependencyProperty.Register("AdditionalCommand",
                typeof(ICommand),
                typeof(ConfirmationPopupWindow),
                new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConfirmationPopupWindow control = d as ConfirmationPopupWindow;
            if(control != null)
            {
                control.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
            }
        }

        protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue)
        {
            if(oldValue != null)
            {
                UnhookCommand(oldValue);
            }

            HookupCommand(newValue);

            CanExecuteChanged(null, null);
        }

        private void UnhookCommand(ICommand oldCommand)
        {
            oldCommand.CanExecuteChanged -= CanExecuteChanged;
        }

        private void HookupCommand(ICommand newCommand)
        {
            EventHandler handler = new EventHandler(CanExecuteChanged);
            canExecuteChangedHandler = handler;
            if(newCommand != null)
            {
                newCommand.CanExecuteChanged += canExecuteChangedHandler;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if(Command != null && ConfirmButton != null)
            {
                RoutedCommand rc = Command as RoutedCommand;

                if(rc != null)
                {
                    ConfirmButton.IsEnabled = rc.CanExecute(CommandParameter, CommandTarget);
                }
                else
                {
                    ConfirmButton.IsEnabled = Command.CanExecute(CommandParameter);
                }
            }
        }

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        [TypeConverter(typeof(CommandConverter))]
        public ICommand AdditionalCommand
        {
            get { return (ICommand)GetValue(AdditionalCommandProperty); }
            set { SetValue(AdditionalCommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter",
                typeof(object),
                typeof(ConfirmationPopupWindow),
                new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty AdditionalCommandParameterProperty =
            DependencyProperty.Register("AdditionalCommandParameter",
                typeof(object),
                typeof(ConfirmationPopupWindow),
                new PropertyMetadata(null));

        public object AdditionalCommandParameter
        {
            get { return GetValue(AdditionalCommandParameterProperty); }
            set { SetValue(AdditionalCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget",
                typeof(IInputElement),
                typeof(ConfirmationPopupWindow),
                new PropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ConfirmButton = GetTemplateChild(ConfirmButtonPart) as Button;
            AdditionalButton = GetTemplateChild(AdditionalButtonPart) as Button;
        }

    }
}
