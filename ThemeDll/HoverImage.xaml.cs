using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace QuickZip.UserControls
{
    /// <summary>
    /// Interaction logic for HoverImage.xaml
    /// </summary>
    public partial class HoverImage : Image
    {
		public static readonly DependencyProperty InitialImageSourceProperty =
			DependencyProperty.Register("InitialImageSource", typeof(ImageSource), typeof(HoverImage), new UIPropertyMetadata(null));

        public static readonly DependencyProperty HoverImageSourceProperty =
            DependencyProperty.Register("HoverImageSource", typeof(ImageSource), typeof(HoverImage), new UIPropertyMetadata(null));

		public ImageSource InitialImageSource
		{
			get { return (ImageSource)GetValue(InitialImageSourceProperty); }
			set 
			{ 
				SetValue(InitialImageSourceProperty, value);
				SetValue(SourceProperty, value);
			}
		}
        
        public ImageSource HoverImageSource
        {
            get { return (ImageSource)GetValue(HoverImageSourceProperty); }
            set { SetValue(HoverImageSourceProperty, value); }
        }

        public HoverImage()
        {
			InitializeComponent();
        }

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == InitialImageSourceProperty)
			{
				Source = null;
				Source = InitialImageSource;
			}
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
            
            Source = HoverImageSource;
        }

        private void RevertSwapImages()
        {
			Source = InitialImageSource;
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
