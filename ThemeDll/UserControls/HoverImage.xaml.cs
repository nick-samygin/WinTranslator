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

namespace QuickZip.UserControls
{
    /// <summary>
    /// Interaction logic for HoverImage.xaml
    /// </summary>
    public partial class HoverImage : Image
    {

        private bool initialImageSwapped = false;
        private object locker = new object();

        public static readonly DependencyProperty HoverImageSourceProperty =
            DependencyProperty.Register("HoverImageSource", typeof(ImageSource), typeof(HoverImage), new UIPropertyMetadata(null));


        private ImageSource initialImage;
        public ImageSource HoverImageSource
        {
            get { return (ImageSource)GetValue(HoverImageSourceProperty); }
            set { SetValue(HoverImageSourceProperty, value); }
        }

        public HoverImage()
        {

            InitializeComponent();

        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var ansestor = this.FindAncestor<RadioButton>();
            if (ansestor != null)
            {
                ansestor.MouseEnter += HoverImage_MouseEnter;
                ansestor.MouseLeave += HoverImage_MouseLeave;
                ansestor.Unchecked += ansestor_Unchecked;
                return;
            }

            var panel = this.FindAncestor<Panel>();
            if (panel != null)
            {
                panel.MouseEnter += HoverImage_MouseEnter;
                panel.MouseLeave += HoverImage_MouseLeave;
                return;
            }
            this.MouseEnter += HoverImage_MouseEnter;
            this.MouseLeave += HoverImage_MouseLeave;
        }

        void ansestor_Unchecked(object sender, RoutedEventArgs e)
        {
            RevertSwapImages();
        }


        private void SwapImages()
        {
            if (!initialImageSwapped)
            {
                lock (locker)
                {
                    initialImage = Source;
                    initialImageSwapped = true;
                }
            }
            Source = HoverImageSource;
        }

        private void RevertSwapImages()
        {
            Source = initialImage;
            lock (locker)
            {
                initialImageSwapped = false;
            }
        }

        void HoverImage_MouseLeave(object sender, MouseEventArgs e)
        {

            if (!IsAnsestorChecked())
            {
                RevertSwapImages();
            }
        }

        void HoverImage_MouseEnter(object sender, MouseEventArgs e)
        {
            SwapImages();
        }

        private bool IsAnsestorChecked()
        {
            var ansestor = this.FindAncestor<RadioButton>();
            if (ansestor != null)
            {
                return ansestor.IsChecked ?? false;
            }

            return false;

        }
    }
}
