using System.Windows;
using PasswordBoss.ViewModel.NotificationViewModels;

namespace PasswordBoss.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ShareNotification.xaml
    /// </summary>
    public partial class ShareNotificationView
    {
        private NotificationWindow _window;
        public static readonly DependencyProperty MessageBoxDialogVisibilityProperty =
           DependencyProperty.Register("MessageBoxDialogVisibility", typeof(bool), typeof(ShareNotificationView), new FrameworkPropertyMetadata
           {
               BindsTwoWayByDefault = true,
               PropertyChangedCallback = OnMessageBoxDialogVisibilityChanged,
           });
        public bool MessageBoxDialogVisibility
        {
            get { return (bool)GetValue(MessageBoxDialogVisibilityProperty); }
            set { SetValue(MessageBoxDialogVisibilityProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxDataContextProperty =
           DependencyProperty.Register("MessageBoxDataContext", typeof(BaseNototificationViewModel), typeof(ShareNotificationView), new FrameworkPropertyMetadata
           {
               BindsTwoWayByDefault = true,
           });
        public BaseNototificationViewModel MessageBoxDataContext
        {
            get { return (BaseNototificationViewModel)GetValue(MessageBoxDataContextProperty); }
            set { SetValue(MessageBoxDataContextProperty, value); }
        }

        private static void OnMessageBoxDialogVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ShareNotificationView;
            var isShowed = (bool) e.NewValue;
            if (isShowed)
            {
                var ownerWindow = Window.GetWindow(d);
                view._window = new NotificationWindow
                {
                    Owner = ownerWindow,
                    DataContext = view.MessageBoxDataContext.GetCopy(),
                    Top = ownerWindow.Top,
                    Left = ownerWindow.Left,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                view._window.ShowDialog();
                if (view._window.DialogResult.HasValue && view._window.DialogResult.Value)
                    view.MessageBoxDataContext = (BaseNototificationViewModel)view._window.DataContext;
                view._window = null;
                view.MessageBoxDialogVisibility = false;
            }
            else if (view._window != null)
            {
                view._window.Close();
                view._window = null;
            }
        }

        public ShareNotificationView()
        {
            InitializeComponent();
        }
    }
}
