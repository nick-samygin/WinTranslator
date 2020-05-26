using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PasswordBoss.Views.UserControls
{
    public class SetupProgressPanel : StackPanel
    {
        public static readonly DependencyProperty FillProperty =
         DependencyProperty.Register(
         "Fill",
         typeof(Brush),
         typeof(SetupProgressPanel), new PropertyMetadata(new SolidColorBrush(Colors.White), OnProgressChanged));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty ProgressProperty =
         DependencyProperty.Register(
         "Progress",
         typeof(int),
         typeof(SetupProgressPanel), new PropertyMetadata(OnProgressChanged));

        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }


        private static void OnProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((SetupProgressPanel)sender).DrawProgress();


        }

        public SetupProgressPanel()
        {
            Orientation = Orientation.Horizontal;
            DrawProgress();
        }

        private void DrawProgress()
        {
            this.Children.Clear();
            for (int i = 0; i < 5; i++)
            {
                DrawEllipse(i < Progress);
                if (i != 4)
                    DrawRectangle();
            }
        }

        private void DrawEllipse(bool fill)
        {
            var circle = new Ellipse() { Width = 17, Height = 17, Stroke = Fill, StrokeThickness = 2 };
            if (fill)
                circle.Fill = Fill;
            Children.Add(circle);
        }

        private void DrawRectangle()
        {
            var rect = new Rectangle() { Height = 5, Margin = new Thickness(-1, 0, -1, 0), Width = 8, Fill = Fill, VerticalAlignment = System.Windows.VerticalAlignment.Center };

            Children.Add(rect);
        }
    }
}
