using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProgressBarPopup.xaml
    /// </summary>
    public partial class ProgressBarPopup : UserControl
    {
        public ProgressBarPopup()
        {
            InitializeComponent();
            this.Loaded += ProgressBarPopup_Loaded;
        }

        void ProgressBarPopup_Loaded(object sender, RoutedEventArgs e)
        {
            mainGrid.IsVisibleChanged += Grid_IsVisibleChanged;
        }

        ProgressBarPopupWindow window = null;

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                window = new ProgressBarPopupWindow();
                window.Owner = Window.GetWindow(this);
                window.Height = window.Owner.ActualHeight;
                window.Width = window.Owner.ActualWidth;
                window.Top = window.Owner.Top;
                window.Left = window.Owner.Left;
                window.DataContext = this.DataContext;

                if(window.Owner.WindowState == WindowState.Maximized)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.Manual;
                }

                window.ShowDialog();
            }
            else
            {
                if(window != null)
                {
                    window.Close();
                    window = null;
                }
            }
        }
    }
}
