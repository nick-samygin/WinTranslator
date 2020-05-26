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
    /// Interaction logic for PinUserControl.xaml
    /// </summary>
    public partial class PinUserControl : UserControl
    {
        public bool LocalChange = false;
        public static readonly DependencyProperty PinProperty =
        DependencyProperty.Register("Pin", typeof(string), typeof(PinUserControl), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
            //PropertyChangedCallback = PinChanged,
            CoerceValueCallback = PinChanged
        });

        public static DependencyProperty LostFocusCommandProperty
        = DependencyProperty.Register(
            "LostFocusCommand",
            typeof(ICommand),
            typeof(PinUserControl));

        public static DependencyProperty PinChangedCommandProperty
        = DependencyProperty.Register(
            "PinChangedCommand",
            typeof(ICommand),
            typeof(PinUserControl));

        public string Pin
        {
            get
            {
                return (string)GetValue(PinProperty);
            }
            set
            {
                LocalChange = true;
                SetValue(PinProperty, value);
            }
        }

        public ICommand LostFocusCommand
        {
            get
            {
                return (ICommand)GetValue(LostFocusCommandProperty);
            }

            set
            {
                SetValue(LostFocusCommandProperty, value);
            }
        }

        public ICommand PinChangedCommand
        {
            get
            {
                return (ICommand)GetValue(PinChangedCommandProperty);
            }

            set
            {
                SetValue(PinChangedCommandProperty, value);
            }
        }

        public PinUserControl()
        {
            InitializeComponent();
        }

        bool moveNext = false;
        bool moveBack = false;

        Dictionary<string, TextBox> dic = new Dictionary<string, TextBox>();

        private void KeyDownHandle(object sender, KeyEventArgs e)
        {
            TextBox pb = sender as TextBox;
            e.Handled = e.Key == Key.Space;

            if((e.Key == Key.Delete || e.Key == Key.Back) && pb.Text.Count() == 0)
            {
                moveBack = true;
            }
        }

        private void KeyUpHandle(object sender, KeyEventArgs e)
        {
            var pb = sender as TextBox;
            if (pb != null)
            {
                if (moveNext)
                {
                    switch (pb.Name)
                    {
                        case "pbPinBox1": pbPinBox2.Focus();
                            break;
                        case "pbPinBox2": pbPinBox3.Focus();
                            break;
                        case "pbPinBox3": pbPinBox4.Focus();
                            break;
                    }
                }

                if (moveBack)
                {
                    switch (pb.Name)
                    {
                        case "pbPinBox2": pbPinBox1.Focus(); pbPinBox1.Text = string.Empty;
                            break;
                        case "pbPinBox3": pbPinBox2.Focus(); pbPinBox2.Text = string.Empty;
                            break;
                        case "pbPinBox4": pbPinBox3.Focus(); pbPinBox3.Text = string.Empty;
                            break;
                    }
                }

            }

            moveNext = false;
            moveBack = false;

            SetPinInternal();

        }

        private void SetPinInternal()
        {
            Pin = pbPinBox1.Text + pbPinBox2.Text + pbPinBox3.Text + pbPinBox4.Text;
        }

        public void SetPinValue(string pin)
        {
            pbPinBox1.Text = pbPinBox2.Text = pbPinBox3.Text = pbPinBox4.Text = string.Empty;
            if(pin != null)
            {
                if(pin.Length > 0)
                {
                    pbPinBox1.Text = pin[0].ToString();
                }
                if (pin.Length > 1)
                {
                    pbPinBox2.Text = pin[1].ToString();
                }
                if (pin.Length > 2)
                {
                    pbPinBox3.Text = pin[2].ToString();
                }
                if (pin.Length > 3)
                {
                    pbPinBox4.Text = pin[3].ToString();
                }
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            TextBox pb = sender as TextBox;
            e.Handled = IsTextNumeric(e.Text);
            if(!e.Handled)
            {
                moveNext = true;
            }
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            if(this.IsEnabled)
            {
                SetDefaultStyle();
            }
            else
            {
                SetDisabledStyle();
            }
            
        }

        private void SetDefaultStyle()
        {
            pbPinBox1.Background = pbPinBox2.Background = pbPinBox3.Background = pbPinBox4.Background = Brushes.White;
            pbPinBox1.BorderBrush = pbPinBox2.BorderBrush = pbPinBox3.BorderBrush = pbPinBox4.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3"));
        }

        private void SetDisabledStyle()
        {
            pbPinBox1.Background = pbPinBox2.Background = pbPinBox3.Background = pbPinBox4.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#eff3f3"));
            pbPinBox1.BorderBrush = pbPinBox2.BorderBrush = pbPinBox3.BorderBrush = pbPinBox4.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFABADB3")); // (SolidColorBrush)(new BrushConverter().ConvertFrom("#e4e9e9"));
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            SetDefaultStyle();
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!this.IsKeyboardFocusWithin)
            {
                var cmd = (ICommand)this.GetValue(LostFocusCommandProperty);
                if (cmd != null) cmd.Execute(null);
            }
        }

        private void pbPinBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cmd = (ICommand)this.GetValue(PinChangedCommandProperty);
            SetPinInternal();
            if (cmd != null) cmd.Execute(null);
        }


        private static object PinChanged(DependencyObject d, Object baseValue)
        {
            PinUserControl control = (PinUserControl)d;

            if(!control.LocalChange)
            {
                control.SetPinValue((string)baseValue);
            }

            control.LocalChange = false;

            return baseValue;
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.IsEnabled)
            {
                SetDefaultStyle();
            }
            else
            {
                SetDisabledStyle();
            }
        }
    }
}
