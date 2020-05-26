using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickZip.UserControls
{
    [TemplatePart(Name = CloseButtonImagePart, Type = typeof(PopupWindow))]
    [TemplatePart(Name = HeaderGridPart, Type = typeof(PopupWindow))]
    public class PopupWindow : ContentControl
    {
        private const string CloseButtonImagePart = "PART_CloseButtonImage";
        private const string HeaderGridPart = "PART_HeaderGrid";

        private Grid headerGrid;

        protected Grid HeaderGrid
        {
            get { return headerGrid; }
            set
            {
                if (headerGrid != null)
                {
                    headerGrid.MouseLeftButtonDown -= HeaderGridMouseLeftButtonDown;
                }

                headerGrid = value;

                if (headerGrid != null)
                {
                    headerGrid.MouseLeftButtonDown += HeaderGridMouseLeftButtonDown;
                }
            }
        }        

        private Image closeButtonImage;

        protected Image CloseButtonImage
        {
            get { return closeButtonImage; }
            set
            {
                if(closeButtonImage != null)
                {
                    closeButtonImage.MouseLeftButtonDown -= ClosePopup;
                }

                closeButtonImage = value;

                if(closeButtonImage != null)
                {
                    closeButtonImage.MouseLeftButtonDown += ClosePopup;
                }
            }
        } 

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PopupWindow), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty LogoVisibilityProperty =
            DependencyProperty.Register("LogoVisibility", typeof(Visibility), typeof(PopupWindow), new PropertyMetadata(Visibility.Visible));

        public Visibility LogoVisibility
        {
            get { return (Visibility)GetValue(LogoVisibilityProperty); }
            set { SetValue(LogoVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LogoWithTextVisibilityProperty =
           DependencyProperty.Register("LogoWithTextVisibility", typeof(Visibility), typeof(PopupWindow), new PropertyMetadata(Visibility.Collapsed));

        public Visibility LogoWithTextVisibility
        {
            get { return (Visibility)GetValue(LogoWithTextVisibilityProperty); }
            set { SetValue(LogoWithTextVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ContentGridHeightSizeProperty =
            DependencyProperty.Register("ContentGridHeightSize", typeof(double), typeof(PopupWindow), new PropertyMetadata(240.0));

        public double ContentGridHeightSize
        {
            get { return (double)GetValue(ContentGridHeightSizeProperty); }
            set { SetValue(ContentGridHeightSizeProperty, value); }
        }

        public static readonly DependencyProperty ControlHeightSizeProperty =
            DependencyProperty.Register("ControlHeightSize", typeof(double), typeof(PopupWindow), new PropertyMetadata(300.0));

        public double ControlHeightSize
        {
            get { return (double)GetValue(ControlHeightSizeProperty); }
            set { SetValue(ControlHeightSizeProperty, value); }
        }

        static PopupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupWindow), new FrameworkPropertyMetadata(typeof(PopupWindow)));
        }        

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CloseButtonImage = GetTemplateChild(CloseButtonImagePart) as Image;
            HeaderGrid = GetTemplateChild(HeaderGridPart) as Grid;
        }

        private void ClosePopup(object sender, MouseButtonEventArgs e)
        {
            Window holder = Window.GetWindow(this);
            if(holder != null)
            {
                holder.Close();
            }			
        }

        private void HeaderGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window holder = Window.GetWindow(this);
            if (holder != null)
            {
                holder.DragMove();
            }
        }
    }
}
