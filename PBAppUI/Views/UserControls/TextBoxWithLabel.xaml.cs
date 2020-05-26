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
	/// Interaction logic for TextBoxWithLabelVertical.xaml
	/// </summary>
	public partial class TextBoxWithLabel : ContentControl
	{
		public static readonly DependencyProperty LabelTextProperty =
		   DependencyProperty.Register("LabelText", typeof(string), typeof(TextBoxWithLabel), new PropertyMetadata(string.Empty));

		public string LabelText
		{
			get { return (string)GetValue(LabelTextProperty); }
			set { SetValue(LabelTextProperty, value); }
		}

		static TextBoxWithLabel()
		{

			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWithLabel), new FrameworkPropertyMetadata(typeof(TextBoxWithLabel)));
		}

		public TextBoxWithLabel()
		{
			GotFocus += TextBoxWithLabel_GotFocus;
		}

		void TextBoxWithLabel_GotFocus(object sender, RoutedEventArgs e)
		{
			if (Content is ComboBox) // lead us to stack overflow
			{
				return;
			}
			((UIElement)Content).Focus();
		}

	}
}
