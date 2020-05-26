using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordBoss.UserControls
{
    /// <summary>
    /// Interaction logic for MenuButtonControl.xaml
    /// </summary>
    public partial class MenuButtonControl : ToggleButton
    {
        public MenuButtonControl()
        {
            
            InitializeComponent();
            this.SizeChanged += MenuButtonControl_SizeChanged;
        }

        private void MenuButtonControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        //    if (e.NewSize != null && e.NewSize.Width != null && e.NewSize.Width <40)
        //        txtName.Visibility = Visibility.Collapsed;
        //    else
        //        txtName.Visibility = Visibility.Visible;
        }

        public static readonly DependencyProperty ImageSourceProperty =
         DependencyProperty.Register(
         "ImageSource",
         typeof(ImageSource),
         typeof(MenuButtonControl),null);

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }


        public static readonly DependencyProperty SelectedImageSourceProperty =
         DependencyProperty.Register(
         "SelectedImageSource",
         typeof(ImageSource),
         typeof(MenuButtonControl), null);

        public ImageSource SelectedImageSource
        {
            get { return (ImageSource)GetValue(SelectedImageSourceProperty); }
            set { SetValue(SelectedImageSourceProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
         DependencyProperty.Register(
         "Header",
         typeof(string),
         typeof(MenuButtonControl), null);

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }


        public static readonly DependencyProperty CheckIconVisibilityProperty =
         DependencyProperty.Register(
         "CheckIconVisibility",
         typeof(Visibility),
         typeof(MenuButtonControl), new PropertyMetadata(Visibility.Visible));

        public Visibility CheckIconVisibility
        {
            get { return (Visibility)GetValue(CheckIconVisibilityProperty); }
            set { SetValue(CheckIconVisibilityProperty, value); }
        }

       
    }
}
