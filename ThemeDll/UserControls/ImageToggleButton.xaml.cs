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

namespace PasswordBoss.UserControls
{
    /// <summary>
    /// Interaction logic for ImageToggleButton.xaml
    /// </summary>
    public partial class ImageToggleButton : UserControl
    {
        public ImageToggleButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UncheckedProperty =
         DependencyProperty.Register(
         "Unchecked",
         typeof(ImageSource),
         typeof(ImageToggleButton),
         new PropertyMetadata(onUncheckedChangedCallback));

        public ImageSource Unchecked
        {
            get { return (ImageSource)GetValue(UncheckedProperty); }
            set { SetValue(UncheckedProperty, value); }
        }

        static void onUncheckedChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }

        public static readonly DependencyProperty MouseOverUncheckedProperty =
            DependencyProperty.Register(
            "MouseOverUnchecked",
            typeof(ImageSource),
            typeof(ImageToggleButton),
            new PropertyMetadata(onMouseOverUncheckedChangedCallback));

        public ImageSource MouseOverUnchecked
        {
            get { return (ImageSource)GetValue(MouseOverUncheckedProperty); }
            set { SetValue(MouseOverUncheckedProperty, value); }
        }

        static void onMouseOverUncheckedChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }


        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register(
            "Checked",
            typeof(ImageSource),
            typeof(ImageToggleButton),
            new PropertyMetadata(onCheckedPropertyChangedCallback));

        public ImageSource Checked
        {
            get { return (ImageSource)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        static void onCheckedPropertyChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }


        public static readonly DependencyProperty MouseOverCheckedProperty =
           DependencyProperty.Register(
           "MouseOverChecked",
           typeof(ImageSource),
           typeof(ImageToggleButton),
           new PropertyMetadata(onMouseOverCheckedCallback));

        public ImageSource MouseOverChecked
        {
            get { return (ImageSource)GetValue(MouseOverCheckedProperty); }
            set { SetValue(MouseOverCheckedProperty, value); }
        }

        static void onMouseOverCheckedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something if needed
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
            "IsChecked",
            typeof(Boolean),
            typeof(ImageToggleButton),
            new PropertyMetadata(onCheckedChangedCallback));

        public Boolean IsChecked
        {
            get { return (Boolean)GetValue(IsCheckedProperty); }
            set { if (value != IsChecked) SetValue(IsCheckedProperty, value); }
        }

        static void onCheckedChangedCallback(
            DependencyObject dobj,
            DependencyPropertyChangedEventArgs args)
        {
            //do something, if needed
        }
    }
}
