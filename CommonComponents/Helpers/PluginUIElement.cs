using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using PasswordBoss.DTO;
using System.Windows.Input;
using PasswordBoss.ViewModel;
using SecureItemsCommon.Helpers;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.Helpers
{
    public class Expandable : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
    }

    public class SecureItemRoutedEventArgs : RoutedEventArgs
    {
        public DefaultView Data { get; set; }
        private string itemId;
        public SecureItemRoutedEventArgs(string itemId)
        {
            this.itemId = itemId;
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
            }
        }
    }        

    public class PluginUIElement
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(PluginUIElement));
        //const int ItemButtonHeight = 121;
        const int ItemButtonHeight = 95;
        const int ItemButtonWidth = 142;


        const double ItemButtonMargin = 2;

        const int ColumnRowOneLength = 18;
        const int ColumnRowTwoLength = 1;
        const int ColumnRowThreeLength = 17;

        const int IconButtonHeightWidth = 61;
        
        const int IconButtonHeight = 65;
        const int IconButtonWidth = 130;
        
        const int ShareImageHeightWidth = 20;
        const int FavouriteSettingImageHeightWidth = 15;
        const int CategoryNameWidth = 122;

        //Handlers for Grid
        public delegate void FavoriteImageHandler(object sender, SecureItemRoutedEventArgs e);
        public event FavoriteImageHandler FavoriteImage_Clicked;

        public delegate void ShareImageHandler(object sender, SecureItemRoutedEventArgs e);
        public event ShareImageHandler ShareImage_Clicked;

        public delegate void SettingImageHandler(object sender, SecureItemRoutedEventArgs e);
        public event SettingImageHandler SettingImage_Clicked;

        public delegate void OpenBrowserHandler(object sender, SecureItemRoutedEventArgs e);
        public event OpenBrowserHandler OpenBrowser_Clicked;

        public delegate void DeleteImageHandler(object sender, SecureItemRoutedEventArgs e);
        public event DeleteImageHandler DeleteImage_Clicked;

        Common _common = new Common();

        public PluginUIElement()
        {}

        #region Grid Items

        public StackPanel makeNewDigitalWalletItemsForGrid(List<DefaultView> _items, bool passwordVaultFlag,  int viewByIndex, bool recommendedSiteFlag)
        {
            StackPanel sPanel = GetStackpanelProperties();


            if (recommendedSiteFlag)
            {
                string _message = string.Empty;
                if(passwordVaultFlag)
                {
                    _message = System.Windows.Application.Current.FindResource("RecommendedItemsHeader") as string;
                }
                else
                {
                    _message = System.Windows.Application.Current.FindResource("SelectAnItemToAdd") as string;
                }
                TextBlock recommendedSiteHeading = GetCategoryName(_message);
                sPanel.Children.Add(recommendedSiteHeading);
            }
          
                List<string> kategorije = new List<string>();
                if(viewByIndex == 0)
                {
                    foreach (DefaultView r in _items)
                    {
                        if (!kategorije.Contains(r.Category))
                            kategorije.Add(r.Category);
                    }
                }
                else
                {
                    kategorije.Add("");
                }

                foreach(string category_name in kategorije)
                {
                    if(category_name != "")
                    {
                        TextBlock CategoryHeading = GetCategoryName(category_name);
                        sPanel.Children.Add(CategoryHeading);
                    }

                    WrapPanel categoryWrapPanel = new WrapPanel { Orientation = Orientation.Horizontal };

                    foreach (DefaultView r in _items)
                    {
                        if(category_name == "" || r.Category.Equals(category_name))
                        {
                            StackPanel sp = makeNewDigitalWalletItem(r, true, r.Favorite, recommendedSiteFlag, passwordVaultFlag);
                            if(!recommendedSiteFlag)
                            {
                                //Context menu for deleting items
                                ContextMenu cm = new ContextMenu();
                                MenuItem mi = new MenuItem();
                                mi.CommandParameter = r.Id;
                                mi.Header = System.Windows.Application.Current.FindResource("Delete");
                                Image IconImage = new Image();
                                IconImage.Height = 15;
                                IconImage.Width = 15;
                                IconImage.Source = System.Windows.Application.Current.FindResource("imgTrash") as ImageSource;
                                mi.Icon = IconImage;
                                BindingOperations.SetBinding(mi, MenuItem.CommandProperty, new Binding("DeleteItemCommand"));
                                cm.Items.Add(mi);
                                sp.ContextMenu = cm;
                            }
                            categoryWrapPanel.Children.Add(sp);
                        }
                        
                    }
                    sPanel.Children.Add(categoryWrapPanel);
                }

            return sPanel;

            //sPanel.Children.Add(categoryWrapPanel);
        }

        private static Button getTileButtonProperties()
        {
            Button itemButton = new Button
            {
                Background = new SolidColorBrush(Colors.White),
                Style = Application.Current.Resources["NormalbuttonStyle"] as Style,
                BorderThickness = new Thickness(ItemButtonMargin),
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                Height = ItemButtonHeight,
                Width = ItemButtonWidth,
                Margin = new Thickness(5, 8, 14.5, 0),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
                
            };
            return itemButton;
        }

        private static Grid getTileGridDesign()
        {
            Grid itemGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            ColumnDefinition itemGridColumnOne = new ColumnDefinition();
            ColumnDefinition itemGridColumnTwo = new ColumnDefinition();
            ColumnDefinition itemGridColumnThree = new ColumnDefinition();
            itemGrid.ColumnDefinitions.Add(itemGridColumnOne);
            itemGridColumnOne.Width = new GridLength(ColumnRowOneLength);
            itemGrid.ColumnDefinitions.Add(itemGridColumnTwo);
            itemGridColumnTwo.Width = new GridLength(ColumnRowTwoLength, GridUnitType.Star);
            itemGrid.ColumnDefinitions.Add(itemGridColumnThree);
            itemGridColumnThree.Width = new GridLength(ColumnRowThreeLength);

            RowDefinition itemGridRowOne = new RowDefinition { Height = new GridLength(ColumnRowOneLength) };
            RowDefinition itemGridRowTwo = new RowDefinition { Height = new GridLength(ColumnRowTwoLength, GridUnitType.Star) };
            //RowDefinition itemGridRowThree = new RowDefinition { Height = new GridLength(ColumnRowThreeLength) };
            itemGrid.RowDefinitions.Add(itemGridRowOne);
            itemGrid.RowDefinitions.Add(itemGridRowTwo);
            //itemGrid.RowDefinitions.Add(itemGridRowThree);

            return itemGrid;
        }

        private static Button getTileHoverButtonProperties(bool recommendedsiteFlag, bool passwordVaultFlag)
        {
            Button hoverItemButton = new Button();
            if(passwordVaultFlag)
            {
                if(recommendedsiteFlag)
                {
                    hoverItemButton.Style = Application.Current.Resources["btnStyleNoHover"] as Style;
                }
                else
                {
                    hoverItemButton.Style = Application.Current.Resources["ItemBtnStyle"] as Style;
                }
            }
            else
            {
                hoverItemButton.Style = Application.Current.Resources["DigitalWalletPersonalInfoButtonStyle"] as Style;
            }

            hoverItemButton.Background = new SolidColorBrush(Colors.White);
            Grid.SetRow(hoverItemButton, 0);
            Grid.SetRowSpan(hoverItemButton, 2);
            Grid.SetColumn(hoverItemButton, 0);
            Grid.SetColumnSpan(hoverItemButton, 3);
            return hoverItemButton;
        }

        private static Grid GetTileGridHoverDesign()
        {
            Grid hoverItemButtonGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            ColumnDefinition hoverButtonGridColumnOne = new ColumnDefinition();
            ColumnDefinition hoverButtonGridColumnTwo = new ColumnDefinition();
            ColumnDefinition hoverButtonGridColumnThree = new ColumnDefinition();
            hoverItemButtonGrid.ColumnDefinitions.Add(hoverButtonGridColumnOne);
            hoverButtonGridColumnOne.Width = new GridLength(ColumnRowOneLength);
            

            hoverItemButtonGrid.ColumnDefinitions.Add(hoverButtonGridColumnTwo);
            hoverButtonGridColumnTwo.Width = new GridLength(ColumnRowTwoLength, GridUnitType.Star);
            hoverItemButtonGrid.ColumnDefinitions.Add(hoverButtonGridColumnThree);
            hoverButtonGridColumnThree.Width = new GridLength(ColumnRowThreeLength);

            RowDefinition hoverButtonGridRowOne = new RowDefinition { Height = new GridLength(ColumnRowOneLength) };
            RowDefinition hoverButtonGridRowTwo = new RowDefinition
            {
                Height = new GridLength(ColumnRowTwoLength, GridUnitType.Star)
            };
            hoverItemButtonGrid.RowDefinitions.Add(hoverButtonGridRowOne);
            hoverItemButtonGrid.RowDefinitions.Add(hoverButtonGridRowTwo);

            return hoverItemButtonGrid;
        }

        //private static Image GetShareImage(bool imageFlag)
        //{
        //    Image shareImage = new Image
        //    {
        //        Height = ShareImageHeightWidth,
        //        Width = ShareImageHeightWidth,
        //        Margin = new Thickness(2, 2, 0, 0),
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Top,
        //        Style = Application.Current.Resources["ImageStyleSharp"] as Style
        //    };
        //    if (imageFlag)
        //    {
        //        var shareImagePath = Application.Current.FindResource("imgShared") as string;
        //        Uri uriShareImage = new Uri(shareImagePath, UriKind.RelativeOrAbsolute);
        //        shareImage.Source = new BitmapImage(uriShareImage);
        //    }
        //    Grid.SetRow(shareImage, 0);
        //    Grid.SetColumn(shareImage, 0);
        //    return shareImage;
        //}

        private static StackPanel GetItemStackpanelProperties()
        {
            StackPanel itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                //0,2,0,0
                Margin = new Thickness(0,0,0,0)
            };
            return itemStackPanel;
        }

        private static Button IconButton(string _imagePath)
        {
            Common c = new Common();
            Button iconButton = new Button
            {
                Height = IconButtonHeight,// IconButtonHeightWidth,
                Width = IconButtonWidth,// IconButtonHeightWidth,
                Style = Application.Current.Resources["btnStyleNoHover"] as Style,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(0,0,0,0)
               
            };

            Image iconImage = new Image();
            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.UriSource = new Uri(_imagePath, UriKind.RelativeOrAbsolute);
            //bi.EndInit();

            //iconImage.Height = 61;
            //iconImage.Width = 61;
            iconImage.Source = c.GetImageForPath(_imagePath);
            iconImage.Style = Application.Current.Resources["ImageStyleSharp"] as Style;
            iconButton.Content = iconImage;
            return iconButton;
        }

        private Button CreateImageButton()
        {
            Button btn = new Button() { Background = Brushes.White, Margin = new Thickness(0, 0, 0, 0), Padding = new Thickness(0, 0, 0, 0) };
            return btn;
        }

        public Button GetFavoriteImageButton(bool imageFlag)
        {
            var img = GetFavoriteImage(imageFlag);
            Button btn = CreateImageButton();
            btn.Content = img;
            return btn;
        }

        public static Image GetFavoriteImage(bool imageFlag)
        {
            Image favouriteImage = new Image
            {
                Margin = new Thickness(2, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Uniform,
                Height = FavouriteSettingImageHeightWidth,
                Width = FavouriteSettingImageHeightWidth
            };
            if(imageFlag)
            {
                favouriteImage.Source = System.Windows.Application.Current.FindResource("imgStarHover") as ImageSource;
            }
            else
            {
                favouriteImage.Source = System.Windows.Application.Current.FindResource("imgStar") as ImageSource;
            }
            favouriteImage.Style = Application.Current.Resources["ImageStyleSharp"] as Style;
            return favouriteImage;

            //favouriteImage.Height = FavouriteSettingImageHeightWidth;
            //favouriteImage.Width = FavouriteSettingImageHeightWidth;

            //string favouriteImagePath = Application.Current.FindResource(imageFlag ? "imgStarHover" : "imgStarTest") as string; 
            
            //Uri urifavouriteImage = new Uri(favouriteImagePath, UriKind.RelativeOrAbsolute);
            //favouriteImage.Source = new BitmapImage(urifavouriteImage);

        }

        

        public Button GetSettingImageButton()
        {
            var img = GetSettingImage();
            Button btn = CreateImageButton();
            btn.Content = img;
            return btn;
        }

        private static Image GetSettingImage()
        {
            Image settingImage = new Image
            {
                Height = FavouriteSettingImageHeightWidth,
                Width = FavouriteSettingImageHeightWidth,
                Margin = new Thickness(0,0,2,0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            //var settingImagePath = Application.Current.FindResource("imgIconSetting") as string;
            //Uri uriSettingImage = new Uri(settingImagePath, UriKind.RelativeOrAbsolute);
            //settingImage.Source = new BitmapImage(uriSettingImage);
            settingImage.Source = System.Windows.Application.Current.FindResource("imgIconSetting") as ImageSource;
            settingImage.Style = Application.Current.Resources["ImageStyleSharp"] as Style;
            //settingImage.Margin = new Thickness(0, 0, 2, 0);
            //settingImage.HorizontalAlignment = HorizontalAlignment.Right;
            //settingImage.VerticalAlignment = VerticalAlignment.Center;
            return settingImage;
        }


        public StackPanel makeNewDigitalWalletItem(DefaultView r, bool shareFlag, bool favoriteFlag, bool recommendedSiteFlag, bool passwordVaultFlag)
        {
            StackPanel rootItemStackPanel = getRootStackPanel();
            try
            {
                Button itemButton = getTileButtonProperties();
                itemButton.DataContext = r;
                Grid itemGrid = getTileGridDesign();

                Button hoverItemButton = getTileHoverButtonProperties(recommendedSiteFlag, passwordVaultFlag);

                if(!recommendedSiteFlag)
                {
                    hoverItemButton.MouseDoubleClick += hoverItemButton_MouseDoubleClick;
                }
                else
                {
                    hoverItemButton.Click += hoverItemButton_Click;
                }
                
                hoverItemButton.Tag = r.Id;

                itemGrid.Children.Add(hoverItemButton);
                
                Grid hoverItemButtonGrid = GetTileGridHoverDesign();

                /* Old design with share icon
                if (!recommendedSiteFlag)
                {
                    var shareImage = GetShareImage(shareFlag);
                    hoverItemButtonGrid.Children.Add(shareImage);
                }
                 * 
                else
                {
                    itemButton.Style = Application.Current.Resources["ItemBtnRecommendedSiteStyle"] as Style;
                }*/
                if (recommendedSiteFlag)
                {
                    if(passwordVaultFlag)
                    {
                        itemButton.Style = Application.Current.Resources["ItemBtnRecommendedSiteStyle"] as Style;
                    }
                    else
                    {
                        itemButton.Style = Application.Current.Resources["DigitalWalletRecommendedGridItemStyle"] as Style;
                    }
                }

                StackPanel itemStackPanel = GetItemStackpanelProperties();
                Grid.SetRow(itemStackPanel, 1);
                //Grid.SetRowSpan(itemStackPanel, 2);
                Grid.SetColumn(itemStackPanel, 0);
                Grid.SetColumnSpan(itemStackPanel, 3);

                
                Button iconButton = IconButton(r.Image);

                itemStackPanel.Children.Add(iconButton);
                hoverItemButtonGrid.Children.Add(itemStackPanel);
                hoverItemButton.Content = hoverItemButtonGrid;

                if (!recommendedSiteFlag)
                {
                    Button btnFavoriteImage = GetFavoriteImageButton(favoriteFlag);
                    btnFavoriteImage.Tag = r.Id;
                    btnFavoriteImage.Style = Application.Current.Resources["NoHoverEffectButtonStyle"] as Style;
                    btnFavoriteImage.Click += btnFavoriteImage_Click;
                    btnFavoriteImage.ToolTip = Application.Current.FindResource("Favorite");


                    //btnFavoriteImage.MouseEnter += btnFavoriteImage_MouseEnter;
                    //btnFavoriteImage.MouseLeave += btnFavoriteImage_MouseLeave;

                    Grid.SetRow(btnFavoriteImage, 0);
                    Grid.SetColumn(btnFavoriteImage, 0);
                    itemGrid.Children.Add(btnFavoriteImage);

                    Button btnSettingImage = GetSettingImageButton();

                    btnSettingImage.Tag = r.Id;
                    btnSettingImage.MouseEnter += btnSettingImage_MouseEnter;
                    btnSettingImage.MouseLeave += btnSettingImage_MouseLeave;
                    btnSettingImage.Click += btnSettingImage_Click;
                    btnSettingImage.Style = Application.Current.Resources["NoHoverEffectButtonStyle"] as Style;
                    btnSettingImage.ToolTip = Application.Current.FindResource("Settings");

                    Grid.SetRow(btnSettingImage, 0);
                    Grid.SetColumn(btnSettingImage, 2);
                    itemGrid.Children.Add(btnSettingImage);
                }

                itemButton.Content = itemGrid;
                rootItemStackPanel.Children.Add(itemButton);
                TextBlock categoryName = getTextBoxWithItemName(r.Name);
                rootItemStackPanel.Children.Add(categoryName);

            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            return rootItemStackPanel;
        }

        void hoverItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenBrowser_Clicked != null)
            {
                var btn = sender as Button;
                var args = new SecureItemRoutedEventArgs(btn.Tag.ToString());
                args.ItemId = btn.Tag.ToString();

                OpenBrowser_Clicked(sender, args);
            }
        }
        
        void hoverItemButton_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (OpenBrowser_Clicked != null)
            {
                var btn = sender as Button;
                var args = new SecureItemRoutedEventArgs(btn.Tag.ToString());
                args.ItemId = btn.Tag.ToString();

                OpenBrowser_Clicked(sender, args);
            }
        }


        void btnSettingImage_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
            var settingsButton = sender as Button;
            Image buttonImage = (Image)settingsButton.Content;
            buttonImage.Source = (ImageSource)Application.Current.Resources["imgSettingIcon"];
        }

        void btnSettingImage_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var settingsButton = sender as Button;
            Image buttonImage = (Image)settingsButton.Content;
            buttonImage.Source = (ImageSource)Application.Current.Resources["imgGearHover"];

        }

        void btnFavoriteImage_Click(object sender, RoutedEventArgs e)
        {
            if(FavoriteImage_Clicked != null)
            {
                var btn = sender as Button;
               
                var args = new SecureItemRoutedEventArgs(btn.Tag.ToString());
                args.ItemId = btn.Tag.ToString();
                FavoriteImage_Clicked(sender, args);
                ((DefaultView)btn.DataContext).Favorite = !((DefaultView)btn.DataContext).Favorite;
            }
        }

        void btnSettingImage_Click(object sender, RoutedEventArgs e)
        {

            if (SettingImage_Clicked != null)
            {
                var btn = sender as Button;
                var args = new SecureItemRoutedEventArgs(btn.Tag.ToString());
                args.ItemId = btn.Tag.ToString();
                
                SettingImage_Clicked(sender, args);
            }
        }

        #endregion

        #region Recommended Items For Grid

        //Main method for Populating grid with all recommended items.  
        public StackPanel makeNewRecommendedItemsForGrid(List<DefaultView> _items)
        {
            StackPanel sPanel = GetStackpanelProperties();
            string _message = System.Windows.Application.Current.FindResource("SelectAnItemToAdd") as string;
            TextBlock recommendedSiteHeading = GetCategoryName(_message);
            sPanel.Children.Add(recommendedSiteHeading);
            WrapPanel categoryWrapPanel = new WrapPanel { Orientation = Orientation.Horizontal };
            foreach (DefaultView r in _items)
            {
                categoryWrapPanel.Children.Add(makeNewItem(r));
            }
            sPanel.Children.Add(categoryWrapPanel);
            return sPanel;
        }

        //Method for creating single item.
        public StackPanel makeNewItem(DefaultView r)
        {
            StackPanel rootItemStackPanel = getRootStackPanel();
            try
            {
                Button itemButton = getItemButton();
                Grid itemGrid = getDefaultGridDesign();

                Button hoverItemButton = new Button()
                {
                    Background = new SolidColorBrush(Colors.White),
                    Style = Application.Current.Resources["ItemBtnRecommendedSiteStyle"] as Style
                };

                itemGrid.Children.Add(hoverItemButton);

                Grid hoverItemButtonGrid = getDefaultGridDesign();

                StackPanel itemStackPanel = getItemStackPanel();
                Grid.SetRow(itemStackPanel, 0);

                Button iconButton = getButtonWithIcon(r.Image);
                itemStackPanel.Children.Add(iconButton);
                hoverItemButtonGrid.Children.Add(itemStackPanel);
                hoverItemButton.Content = hoverItemButtonGrid;

                itemButton.Content = itemGrid;
                rootItemStackPanel.Children.Add(itemButton);
                var categoryName = getTextBoxWithItemName(r.Name);
                rootItemStackPanel.Children.Add(categoryName);

            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            return rootItemStackPanel;
        }

        //Method for creating root stack Panel.
        public StackPanel getRootStackPanel()
        {
            StackPanel s = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Vertical,
                //Margin = new Thickness(0, 0, 0, 12)
            };
            return s;
        }

        //Method for item stack panel.
        private static StackPanel getItemStackPanel()
        {
            StackPanel itemStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            return itemStackPanel;
        }

        //Method for getting item button.
        public Button getItemButton()
        {
            Button b = new Button
            {
                Background = new SolidColorBrush(Colors.White),
                Height = 103,
                Width = 122,
                Margin = new Thickness(0, 5, 20, 0),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                BorderBrush = new SolidColorBrush(Colors.Transparent),
            };
            return b;
        }

        //Method for button with icon.
        public Button getButtonWithIcon(string _imageURI)
        {
            var iconButton = new Button
            {
                Height = 61,
                Width = 61,
                Style = Application.Current.Resources["btnStyleNoHover"] as Style,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            try
            {
                iconButton.Content = getDefaultImage(_imageURI);
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)System.Windows.Application.Current.FindResource("GeneralErrorText"));
                logger.Error(ex.Message);
            }
            return iconButton;
        }


        //Method for getting default grid.
        public Grid getDefaultGridDesign()
        {
            var itemGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var itemGridRowOne = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            var itemGridRowTwo = new RowDefinition { Height = new GridLength(0) };
            itemGrid.RowDefinitions.Add(itemGridRowOne);
            itemGrid.RowDefinitions.Add(itemGridRowTwo);

            return itemGrid;
        }

        //TextBox for showing item name.
        public TextBlock getTextBoxWithItemName(string _name)
        {
            var resultName = new TextBlock
            {
                Text = StringTrim(StringTrim(_name)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(-2, 5, 5, 11),
                Width = 122,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = (FontFamily)Application.Current.FindResource("ProximaRegular"),
                Foreground = (Brush)Application.Current.FindResource("LightGrayTextForegroundColor")
            };
            return resultName;
        }

        //Method for limiting item length.
        public string StringTrim(string _itemName)
        {
            return _itemName != null && _itemName.Length > 25 ? _itemName.Substring(0, 25) + "..." : _itemName;
            //const int itemLength = 25;
            //string itemName = _itemName;
            //if (itemName.Length > itemLength)
            //{
            //    itemName = itemName.Substring(0, itemLength);
            //    itemName = itemName + "...";
            //}
            //return itemName;
        }

        public Image getDefaultImage(string image)
        {
            Uri resourceUri = new Uri(image, UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            Image iconImage = new Image();
            iconImage.Source = temp;
            iconImage.Style = Application.Current.Resources["PasswordVaultIconsStyle"] as Style;
            return iconImage;
        }

        #endregion

        #region ListBox Items


        /* Main method for Populating listview with all recommended items.   
         * _items - List of Items that will be populated in ListView
         * PasswordVaultFlag - Flag that indicates whether it is PasswordVault screen or DigitalWallet/PersonalInfo
         * */
        public StackPanel makeNewRecommendedItemsForList(List<DefaultView> _items, bool PasswordVaultFlag, int viewByIndex, bool recommendedSiteFlag)
        {

            StackPanel returnListviewTile = GetStackpanelProperties();

            if (recommendedSiteFlag)
            {
                string _headerMessage = string.Empty;
                if(PasswordVaultFlag)
                {
                    _headerMessage = System.Windows.Application.Current.FindResource("RecommendedItemsHeader") as string;
                }
                else
                {
                    _headerMessage = System.Windows.Application.Current.FindResource("SelectAnItemToAdd") as string;
                }
                    TextBlock recommendedSiteHeading = GetCategoryName(_headerMessage);
                    returnListviewTile.Children.Add(recommendedSiteHeading);
                
            }

                List<string> kategorije = new List<string>();
                foreach (DefaultView r in _items)
                {
                    if (r.Favorite)
                    {
                        r.FavoriteImage = (ImageSource)Application.Current.FindResource("imgStarHoverList");
                    }
                    else
                    {
                        r.FavoriteImage = (ImageSource)Application.Current.FindResource("imgStarList");
                    }
                    if (viewByIndex == 0)
                    {
                        
                        if (!kategorije.Contains(r.Category))
                            kategorije.Add(r.Category);
                        
                    }
                    else
                    {
                        if (!kategorije.Contains(""))
                        {
                            kategorije.Add("");
                        }
                    }
                }

                foreach (string category_name in kategorije)
                {
                    if (category_name != "")
                    {
                        TextBlock CategoryHeading = GetCategoryName(category_name);
                        returnListviewTile.Children.Add(CategoryHeading);
                    }
                   
                    ListBox recommendedSiteListBox = GetrecommendeSiteListboxProperties();
                    if(PasswordVaultFlag)
                    {
                        if(recommendedSiteFlag)
                        {
                            recommendedSiteListBox.ItemTemplate = Application.Current.Resources["PasswordVaultRecommendedListItemTemplate"] as DataTemplate;
                        }
                        else
                        {
                            recommendedSiteListBox.ItemTemplate = Application.Current.Resources["listdatatemplate"] as DataTemplate;
                        }
                        
                    }
                    else
                    {
                        if (recommendedSiteFlag)
                        {
                            recommendedSiteListBox.ItemTemplate = Application.Current.Resources["DigitalWalletRecommendedItemTemplate"] as DataTemplate;
                        }
                        else
                        {
                            recommendedSiteListBox.ItemTemplate = Application.Current.Resources["NoSiteHoverDataTemplate"] as DataTemplate;
                        }
                    }

                    foreach (DefaultView r in _items)
                    {
                        if (r.Category!= null && (r.Category.Equals(category_name) || category_name == ""))
                        {
                            //if(r.Image == null)
                            //{
                            //    Application.Current.Resources["NoSiteHoverDataTemplate"] as ImageSource
                            //}

                            r.RecommendedSiteFlag = recommendedSiteFlag;
                            recommendedSiteListBox.Items.Add(r);
                          
                            r.GearButton_Clicked += r_GearButton_Clicked;
                            r.FavoritesIcon_Clicked += r_FavoritesIcon_Clicked;
                            r.OpenInBrowser_Clicked += r_OpenInBrowser_Clicked;
                            r.SharingIcon_Clicked += r_SharingIcon_Clicked;
                            r.DeletingIcon_Clicked += r_DeletingIcon_Clicked;
                        }
                    }

                   recommendedSiteListBox.ItemContainerGenerator.StatusChanged += (sender, e) =>
                    {
                        recommendedSiteListBox.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            foreach (DefaultView item in recommendedSiteListBox.Items)
                            {
                                ListBoxItem lbItem = recommendedSiteListBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                                if(!item.RecommendedSiteFlag && item != null && lbItem != null)
                                {
                                    ContextMenu cm = new ContextMenu();
                                    MenuItem mi = new MenuItem();
                                    mi.CommandParameter = item.Id;
                                    mi.Header = System.Windows.Application.Current.FindResource("Delete");
                                    Image IconImage = new Image();
                                    IconImage.Height = 15;
                                    IconImage.Width = 15;
                                    IconImage.Source = System.Windows.Application.Current.FindResource("imgTrash") as ImageSource;
                                    mi.Icon = IconImage;
                                    mi.DataContext = recommendedSiteListBox.DataContext;
                                    BindingOperations.SetBinding(mi, MenuItem.CommandProperty, new Binding("DeleteItemCommand"));
                                    cm.Items.Add(mi);
                                    lbItem.ContextMenu = cm;
                                }
                            }
                        }));
                    };

                    returnListviewTile.Children.Add(recommendedSiteListBox);
                }

            return returnListviewTile;
        }

        void r_OpenInBrowser_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            OpenBrowser_Clicked(sender, e);
        }

        void r_GearButton_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            SettingImage_Clicked(sender, e);
        }

        void r_FavoritesIcon_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            FavoriteImage_Clicked(sender, e);
        }
        void r_SharingIcon_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            ShareImage_Clicked(sender, e);
        }
        void r_DeletingIcon_Clicked(object sender, SecureItemRoutedEventArgs e)
        {
            DeleteImage_Clicked(sender, e);
        }
        /*
        private childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        */

        private static StackPanel GetStackpanelProperties()
        {
            StackPanel returnStackpanel = new StackPanel();
            returnStackpanel.Margin = new Thickness(0, 0, 0, 0);
            returnStackpanel.Orientation = Orientation.Vertical; 
            return returnStackpanel;
        }

        //Method for getting TextBlock with category name.
        private static TextBlock GetCategoryName(string name)
        {
            var categoryTextblock = new TextBlock();
            categoryTextblock.Style = (Style)Application.Current.FindResource("TextBlockCategoryHead");
            categoryTextblock.Text = name;
            return categoryTextblock;
        }

        private static ListBox GetrecommendeSiteListboxProperties()
        {
            var recommendeSiteListBox = new ListBox
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(0, 3, 0, 20),
                BorderThickness = new Thickness(0, 0, 0, 0),
                AlternationCount = 2,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                ItemContainerStyle = Application.Current.Resources["ListBoxCustomItemContainerStyle"] as Style,
                Style = Application.Current.Resources["CustomListBoxTemplate"] as Style
            };
            return recommendeSiteListBox;
        }


        #endregion




    }

    public class CategoryDefaultView: Expandable
    {
        public string Name { get; set; }
        public ObservableCollection<DefaultView> Views { get; private set; }

       

        public CategoryDefaultView()
        {
            Views = new ObservableCollection<DefaultView>();
        }
    }


    public class SecureItemsView : Expandable
    {
        public SecureItemsView()
        {

            SecureList = new ObservableCollection<ISecureItemVM>();
        }
              
        
        private ObservableCollection<ISecureItemVM> _secureList;
        public ObservableCollection<ISecureItemVM> SecureList
        {
            get { return _secureList; }
            set
            {
                _secureList = value;
                RaisePropertyChanged("SecureList");
            }
        }

        private ObservableCollection<ISecureItemVM> _selectedItems=new ObservableCollection<ISecureItemVM>();
        public ObservableCollection<ISecureItemVM> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                RaisePropertyChanged("SelectedItems");
                RaisePropertyChanged("Actions");
            }
        }

        private List<IContextAction> actions = new List<IContextAction>();
        public List<IContextAction> Actions
        {
            get
            {
                if (_selectedItems != null)
                {
                    if (_selectedItems.Count == 1)
                        return (_selectedItems.FirstOrDefault() as SecureItemViewModel).Actions;
                }
                return actions;
            }
            set
            {
                actions = value;
                RaisePropertyChanged("Actions");
            }
        }

    }

    public class FolderView : Expandable
    {
        public FolderView()
        {

            //SecureList = new ObservableCollection<ISecureItemVM>();
            ChildList = new ObservableCollection<object>();// { SecureList };
        }

        public FolderView(string id,string prntId,string folderName,bool expandState)
        {
            uuid = id;
            parentId = prntId;
            FolderName = folderName;
            _IsExpanded = expandState;


           // SecureList = new ObservableCollection<ISecureItemVM>();
            ChildList = new ObservableCollection<object>();// { SecureList };
        }


        public void AddSecureItem(ISecureItemVM newItem)
        {
            if (!ChildList.Any() || !(ChildList.First() is SecureItemsView) )
                ChildList.Insert(0, SecureItemsView);           

            SecureItemsView.SecureList.Add(newItem);
            RaisePropertyChanged("Count");
        }

        public string uuid { get; set; }

        private ObservableCollection<object> childList;
        public ObservableCollection<object> ChildList
        {
            get { return childList; }
            set
            {
                childList = value;
                RaisePropertyChanged("ChildList");
                RaisePropertyChanged("Count");
            }
        }

        //private ObservableCollection<ISecureItemVM> _secureList;
        //public ObservableCollection<ISecureItemVM> SecureList
        //{
        //    get { return _secureList; }
        //    set
        //    {
        //        _secureList = value;
        //        RaisePropertyChanged("SecureList");
        //        RaisePropertyChanged("ChildList");
        //    }
        //}

        private SecureItemsView _secureItemsView=new SecureItemsView();
        public SecureItemsView SecureItemsView
        {

            get { return _secureItemsView; }
            set
            {
                _secureItemsView = value;
                RaisePropertyChanged("SecureItemsView");
                RaisePropertyChanged("Count");
            }
        }
        

        public string parentId { get; set; }
        public string FolderName { get; set; }

        public List<ISecureItemVM> GetAllSecureItems()
        {
            var result=new List<ISecureItemVM>(SecureItemsView.SecureList);
            foreach(var item in childList)
            {
                var folder = item as FolderView;
                if (folder!=null)
                    result.AddRange(folder.GetAllSecureItems());                
            }
           return result;
        }

        public int Count
        {
            get
            {
                if(SecureItemsView!=null && SecureItemsView.SecureList!=null)
                    return SecureItemsView.SecureList.Count();
                return 0;
            }

        }
    }



    public class DefaultView : FolderView
    {
        private string id;
        private string category;
        private Folder folder;
        private string image;
        private string name;
        private string username;
        private bool favorite;
        public bool shared;
        private bool hasNote;
        private string recommendedHoverText;

        
        private DateTime? lastAccess;

        private ImageSource favoriteImage;

        public RelayCommand GearButtonCommand { get; set; }
        public RelayCommand FavoritesCommand { get; set; }
        public RelayCommand OpenInBrowserCommand { get; set; }
        public RelayCommand ShareItemCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand EditItemCommand { get; set; }

        //Handlers for List
        public delegate void GearButtonCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event GearButtonCommandHandler GearButton_Clicked;

        public delegate void FavoritesIconCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event FavoritesIconCommandHandler FavoritesIcon_Clicked;

        public delegate void SharingIconCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event SharingIconCommandHandler SharingIcon_Clicked;

        public delegate void OpenInBrowserCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event OpenInBrowserCommandHandler OpenInBrowser_Clicked;

        public delegate void DeletingIconCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event DeletingIconCommandHandler DeletingIcon_Clicked;

        public event EventHandler Edit_Clicked;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public Folder Folder
        {
            get { return folder; }
            set { folder = value; }
        }
                
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public string RecommendedHoverText
        {
            get { return recommendedHoverText; }
            set 
            { 
                recommendedHoverText = value;
                RaisePropertyChanged("RecommendedHoverText");
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value;
            RaisePropertyChanged("Name");
            }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                favorite = value;
                RaisePropertyChanged("Favorite");
            }
        }

        public bool Shared
        {
            get
            {
                return shared;
            }
            set
            {
                shared = value;
                RaisePropertyChanged("Shared");
            }
        }

        public bool HasNote
        {
            get
            {
                return hasNote;
            }
            set
            {
                hasNote = value;
                RaisePropertyChanged("HasNote");
            }
        }

        private ImageSource shareImage;
        public ImageSource ShareImage
        {
            get
            {
                if (shared)
                {
                    return (ImageSource)Application.Current.FindResource("imgSharePeopleHover");
                }
                else
                {
                    return (ImageSource)Application.Current.FindResource("imgSharePeople");
                }
            }
            set
            {
                shareImage = value;
                RaisePropertyChanged("ShareImage");
            }
        }

        public ImageSource FavoriteImage
        {
            get { return favoriteImage; }
            set { favoriteImage = value;
            RaisePropertyChanged("FavoriteImage");
            }
        }

        public DateTime? LastAccess
        {
            get
            {
                return lastAccess; 
            }
            set
            {
                lastAccess = value;
                if(lastAccess == null)
                {
                    LastAcceessTime = "Never";
                }
                RaisePropertyChanged("LastAccess");
            }
        }

        private String lastAccessTime;
        public String LastAcceessTime
        {
            get
            {
                return lastAccessTime;
            }
            set
            {
                lastAccessTime = value;                
                RaisePropertyChanged("LastAccessTime");
            }
        }

        public bool RecommendedSiteFlag { get; set; }

        public ObservableCollection<ContextAction> Actions { get; set; }

        public DefaultView(string id, string cat, string im, string n, bool fav, string username)
        {
            this.id = id;
            category = cat;
            image = im;
            name = n;
            favorite = fav;
            this.username = username;
        }

        public DefaultView() 
        {


            GearButtonCommand = new RelayCommand(GearButtonClicked);
            FavoritesCommand = new RelayCommand(FavoritesIconClicked);
            ShareItemCommand = new RelayCommand(SharingButtonClicked);
            OpenInBrowserCommand = new RelayCommand(OpenInBrowserClick);
            DeleteItemCommand = new RelayCommand(DeletingItemClicked);
            EditItemCommand = new RelayCommand(EditItemClicked);

            Actions = new ObservableCollection<ContextAction>()
            { new ContextAction() { Action=EditItemCommand,Name="Edit Item"} };
        }

       

        private void OpenInBrowserClick(object obj)
        {
            if (OpenInBrowser_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.ItemId = this.id;
                OpenInBrowser_Clicked(null, args);
            }
        }

        private void GearButtonClicked(object obj)
        {
            if(GearButton_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.ItemId = this.id;
                GearButton_Clicked(null, args);
            }
        }

        private void SharingButtonClicked(object obj)
        {
            if (SharingIcon_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.ItemId = this.id;
                SharingIcon_Clicked(null, args);
            }
        }

        private void DeletingItemClicked(object obj)
        {
            if (DeletingIcon_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.ItemId = this.id;
                DeletingIcon_Clicked(null, args);
            }
        }

        private void EditItemClicked(object obj)
        {
            if (Edit_Clicked != null)
                Edit_Clicked(this, null);
           
        }
            

        private void FavoritesIconClicked(object obj)
        {
            if (Favorite)
            {
                FavoriteImage = (ImageSource)Application.Current.FindResource("imgStarList");
            }
            else
            {
                FavoriteImage = (ImageSource)Application.Current.FindResource("imgStarHoverList");
            }
            if (FavoritesIcon_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.Data = this;
                args.ItemId = this.id;
                FavoritesIcon_Clicked(null, args);
                Favorite = !Favorite;
            }          

            
        }
    }

}
