using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordBoss.UserControls
{
    public class CustomChildWindow:Window
    {
        public static readonly DependencyProperty IsWindowMayCloseProperty = DependencyProperty.Register("IsWindowMayClose",
           typeof(bool), typeof(CustomChildWindow), new PropertyMetadata(true, OnIsWindowMayCloseChanged));

        private static void OnIsWindowMayCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as CustomChildWindow;
            if ((bool) e.NewValue)
                window.Close();
        }

        public bool IsWindowMayClose
        {
            get { return (bool)GetValue(IsWindowMayCloseProperty); }
            set { SetValue(IsWindowMayCloseProperty, value); }
        }
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand",
            typeof(ICommand), typeof(CustomChildWindow));
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DependencyProperty OkButtonCommandProperty = DependencyProperty.Register("OkButtonCommand",
            typeof(ICommand), typeof(CustomChildWindow));
        public ICommand OkButtonCommand
        {
            get { return (ICommand)GetValue(OkButtonCommandProperty); }
            set { SetValue(OkButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty CancelButtonContentProperty = DependencyProperty.Register("CancelButtonContent",
            typeof(string), typeof(CustomChildWindow));
        public string CancelButtonContent
        {
            get { return (string)GetValue(CancelButtonContentProperty); }
            set { SetValue(CancelButtonContentProperty, value); }
        }

        public static readonly DependencyProperty OkButtonContentProperty = DependencyProperty.Register("OkButtonContent",
            typeof(string), typeof(CustomChildWindow));
        public string OkButtonContent
        {
            get { return (string)GetValue(OkButtonContentProperty); }
            set { SetValue(OkButtonContentProperty, value); }
        }

        public static readonly DependencyProperty TitelContentProperty =
      DependencyProperty.Register(
      "TitelContent",
      typeof(object),
      typeof(CustomChildWindow), null);

        public object TitelContent
        {
            get { return (object)GetValue(TitelContentProperty); }
            set { SetValue(TitelContentProperty, value); }
        }

        public static readonly DependencyProperty MaxContentHeightProperty =
    DependencyProperty.Register(
    "MaxContentHeight",
    typeof(double),
    typeof(CustomChildWindow), new PropertyMetadata(double.NaN));

        public double MaxContentHeight
        {
            get { return (double)GetValue(MaxContentHeightProperty); }
            set { SetValue(MaxContentHeightProperty, value); }
        }


        public static readonly DependencyProperty ShowButtonsPanelProperty =
        DependencyProperty.Register(
        "ShowButtonsPanel",
        typeof(Visibility),
        typeof(CustomChildWindow), new PropertyMetadata(Visibility.Visible, onShowButtonsPanelChangedCallback));

        public Visibility ShowButtonsPanel
        {
            get { return (Visibility)GetValue(ShowButtonsPanelProperty); }
            set { SetValue(ShowButtonsPanelProperty, value); }
        }

        static void onShowButtonsPanelChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs args)
        {
            
        }


        public static readonly DependencyProperty ShowTitelBorderProperty =
       DependencyProperty.Register(
       "ShowTitelBorder",
       typeof(Visibility),
       typeof(CustomChildWindow), new PropertyMetadata(Visibility.Visible, onShowTitelBorderChangedCallback));

        public Visibility ShowTitelBorder
        {
            get { return (Visibility)GetValue(ShowTitelBorderProperty); }
            set { SetValue(ShowTitelBorderProperty, value); }
        }

        static void onShowTitelBorderChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs args)
        {
            CustomChildWindow control = dobj as CustomChildWindow;
            if (control != null)
            {
                if (control.titelBorder != null)
                    control.titelBorder.Visibility = control.ShowTitelBorder;
            }

        }


        public static readonly DependencyProperty ContentWidthProperty =
      DependencyProperty.Register(
      "ContentWidth",
      typeof(double),
      typeof(CustomChildWindow), new PropertyMetadata(double.NaN, onContentWidthChangedCallback));

        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public static readonly DependencyProperty ScrollWidthProperty =
    DependencyProperty.Register(
    "ScrollWidth",
    typeof(double),
    typeof(CustomChildWindow), new PropertyMetadata(double.NaN, onContentWidthChangedCallback));

        public double ScrollWidth
        {
            get { return (double)GetValue(ScrollWidthProperty); }
            set { SetValue(ScrollWidthProperty, value); }
        }

        static void onContentWidthChangedCallback(DependencyObject dobj, DependencyPropertyChangedEventArgs args)
        {
            CustomChildWindow control = dobj as CustomChildWindow;
            if (control != null)
            {
                if (control.contentControl != null && !double.IsNaN(control.ContentWidth))
                    control.contentControl.Width = control.ContentWidth;
            }

        }



        static CustomChildWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomChildWindow),
                new FrameworkPropertyMetadata(typeof(CustomChildWindow)));
            
        }

        public CustomChildWindow(): base()
        {
            SetMainWindowAsParent();
            OkButtonContent = Application.Current.Resources["Save"] as string;
            CancelButtonContent = Application.Current.Resources["Cancel"] as string;
        }

        
        private void SetMainWindowAsParent()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Window parentWindow = null;

            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType().Name == "MainWindow")
                {
                    parentWindow = window;
                }
            }

            this.Owner = parentWindow;
        }

        private Border titelBorder;

        private Panel contentControl;

        public override void OnApplyTemplate()
        {

            Button closeButton = GetTemplateChild("btnCloseWindow") as Button;
            if (closeButton != null)
                closeButton.Click += CloseClick;

            Button cancelButton = GetTemplateChild("btnCancel") as Button;
            if (cancelButton != null)
                cancelButton.Click += CancelClick;

            Button okButton = GetTemplateChild("btnOk") as Button;
            if (okButton != null)
                okButton.Click += OkClick;

            titelBorder = GetTemplateChild("titelBorder") as Border;
            if (titelBorder != null)
                titelBorder.Visibility = ShowTitelBorder;

            Panel buttonsPanel = GetTemplateChild("buttonsPanel") as Panel;
            if (buttonsPanel != null)
                buttonsPanel.Visibility = ShowButtonsPanel;

            ScrollViewer contentScrollViewer = GetTemplateChild("svScrollViewer") as ScrollViewer;
            if (contentScrollViewer != null && !double.IsNaN(MaxContentHeight))
                contentScrollViewer.MaxHeight = MaxContentHeight;

            if (contentScrollViewer != null && !double.IsNaN(ScrollWidth))
                contentScrollViewer.Width = ScrollWidth;


            ContentPresenter headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            if (headerContentPresenter != null)
                headerContentPresenter.Content = TitelContent;

            contentControl = GetTemplateChild("AddEditControl") as Panel;
            if (contentControl != null & !double.IsNaN(ContentWidth))
                contentControl.Width = ContentWidth;
            

            base.OnApplyTemplate();
        }

        private void CloseClick(object sender, RoutedEventArgs routedEventArgs)
        {
            DialogResult = false;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if(CancelCommand != null)
                CancelCommand.Execute(null);
            else
            {
                DialogResult = false;
                Close();
            }
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if(OkButtonCommand != null)
                OkButtonCommand.Execute(null);
            else
            {
                DialogResult = true;
                Close();
            }
        }

    }
}
