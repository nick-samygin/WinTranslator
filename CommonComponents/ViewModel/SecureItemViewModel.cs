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
using PasswordBoss.Helpers;
using System.Windows.Input;
using PasswordBoss;
using SecureItemsCommon.Helpers;

namespace PasswordBoss
{ 

  
    public class SecureItemViewModel : INotifyPropertyChanged, ISecureItemVM
    {
        internal Common _common = new Common();

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        private Folder folder;
        private string image;
        private string name;
        private bool favorite;
        public bool shared;
        private bool hasNote;
        private string recommendedHoverText;

        public virtual bool IsWebSite
        {
            get
            {
                return false;

            }

        }

        public string ItemTitel { get; set; }

        protected string type;
        public string Type
        {
            get { return type; }
        }

        protected string subType;
        public string SubType
        {
            get { return subType; }
        }

        private bool showItemMenu;
        public bool ShowItemMenu
        {
            get { return showItemMenu; }
            set
            {
                showItemMenu = value;
                RaisePropertyChanged("ShowItemMenu");
            }
        }


        private DateTime? lastAccess;


        public RelayCommand GearButtonCommand { get; set; }
        public RelayCommand FavoritesCommand { get; set; }
        public RelayCommand OpenInBrowserCommand { get; set; }
        public RelayCommand ShareItemCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand AddNoteCommand { get; set; }
        public RelayCommand NoteLostFocusCommand { get; set; }
        public RelayCommand AddNewFolderCommand { get; set; }

        public RelayCommand EditItemCommand { get; set; }

        //Handlers for List
        public delegate void GearButtonCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event GearButtonCommandHandler GearButton_Clicked;

        public event EventHandler FavoritesIcon_Clicked;

        public delegate void SharingIconCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event SharingIconCommandHandler SharingIcon_Clicked;

        public delegate void OpenInBrowserCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event OpenInBrowserCommandHandler OpenInBrowser_Clicked;

        public delegate void DeletingIconCommandHandler(object sender, SecureItemRoutedEventArgs e);
        public event DeletingIconCommandHandler DeletingIcon_Clicked;

        public delegate void SecureItemCommandHandler(object sender, SecureItemRoutedEventArgs e);

        public event EventHandler AddNewFolder_Clicked;

        public event EventHandler Edit_Clicked;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<Folder> foldersList;
        public List<Folder> FoldersList
        {
            get { return foldersList; }
            set
            {
                foldersList = value;
                RaisePropertyChanged("FoldersList");
            }
        }


        public Folder Folder
        {
            get { return folder; }
            set
            {
                if (folder != value)
                {
                    folder = value;
                    IsOpenFoldersDropDown = false;
                    RaisePropertyChanged("Folder");
                }
            }
        }

        private bool isOpenFoldersDropDown;
        public bool IsOpenFoldersDropDown
        {
            get { return isOpenFoldersDropDown; }
            set
            {
                isOpenFoldersDropDown = value;
                RaisePropertyChanged("IsOpenFoldersDropDown");
            }
        }

        
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                RaisePropertyChanged("Image");
            }
        }

        private bool _isDefaultImage = true;
        public bool IsDefaultImage
        {
            get { return _isDefaultImage; }
            set
            {
                _isDefaultImage = value;
                RaisePropertyChanged("IsDefaultImage");
            }
        }

        private bool _canPickColor = true;
        public bool CanPickColor
        {
            get { return _canPickColor; }
            set
            {
                _canPickColor = value;
                RaisePropertyChanged("CanPickColor");
            }
        }


        public string Name
        {
            get { return name; }
            set
            {

                name = value;
                IsValidName =!string.IsNullOrEmpty(Name);
                RaisePropertyChanged("Name");
            }
        }

        private bool isValidName=true;
        public bool IsValidName
        {
            get { return isValidName; }
            set
            {
                isValidName = value;
                RaisePropertyChanged("IsValidName");
            }
        }

        
        public string ListViewSecondName
        {
            get; set;
        }


        public bool Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                if (favorite != value)
                {
                    favorite = value;                   
                    RaisePropertyChanged("Favorite");
                }
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

        private bool hasNoteHistory = true;
        public bool HasNoteHistory
        {
            get
            {
                return hasNoteHistory;
            }
            set
            {
                hasNoteHistory = value;
                RaisePropertyChanged("HasNoteHistory");
            }
        }


        protected bool hasAddvancedSettings;
        public bool HasAddvancedSettings
        {
            get
            {
                return hasAddvancedSettings;
            }           
        }

        private string note;
        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
                RaisePropertyChanged("Note");
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
                if (lastAccess == null)
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

        private DateTime? createdDate;
        public DateTime CreatedDate
        {
            get
            {
                if (createdDate == null)
                    createdDate = DateTime.UtcNow;
                return createdDate.Value;
            }
            set
            {
                createdDate = value;
                RaisePropertyChanged("CreatedDate");
            }
        }


        private DateTime? lastModifiedDate;
        public DateTime LastModifiedDate
        {
            get
            {
                if (lastModifiedDate == null)
                    lastModifiedDate = DateTime.UtcNow;
                return lastModifiedDate.Value;
            }
            set
            {
                lastModifiedDate = value;
                RaisePropertyChanged("LastModifiedDate");
            }
        }

        private Brush _background;
        public Brush Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                RaisePropertyChanged("Background");
            }
        }


        public bool RecommendedSiteFlag { get; set; }

        public  List<object> Actions { get; set; }

       

        public SecureItemViewModel()
        {
            InitCommand();
            HasNoteHistory = false;
        }


        public SecureItemViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon)
        {
            InitCommand();
            Id = item.Id;

            Background = defaultColor;

            //var img = defaultIcon as BitmapImage;
            //if (img != null)
            //    Image= img.ToString();

            var img = defaultIcon as BitmapImage; //Application.Current.Resources["imgPbOwlGrey"] as BitmapImage;
            Image = img.ToString();


            name = item.Name;
            Folder = item.Folder==null?new Folder() { UUID=string.Empty}:item.Folder;
            favorite = item.Favorite;
            LastAccess = item.LastAccess;
            CreatedDate = item.CreatedDate;
            LastModifiedDate = item.LastModifiedDate;
            shared = item.Share;
            Note = item.Data.notes;
            HasNote = !string.IsNullOrEmpty(Note);

            if (item.Color != null)
            { 
                try
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.Color));
                }
                catch (FormatException ex)
                {

                }
            }


        }

        private void InitCommand()
        {
            GearButtonCommand = new RelayCommand(GearButtonClicked);
            FavoritesCommand = new RelayCommand(FavoritesIconClicked);
            ShareItemCommand = new RelayCommand(SharingButtonClicked);
            OpenInBrowserCommand = new RelayCommand(OpenInBrowserClick);
            DeleteItemCommand = new RelayCommand(DeletingItemClicked);
            AddNoteCommand = new RelayCommand(AddNoteExcute);
            NoteLostFocusCommand = new RelayCommand(param => NoteLostTextBoxFocus(), null);
            AddNewFolderCommand = new RelayCommand(AddNewFolderClicked);
            EditItemCommand = new RelayCommand(EditItemClicked);

            Actions = new List<object>()
            {   new ContextAction()
                {
                    Action =EditItemCommand,
                    Name =Application.Current.FindResource("Edit") as string,
                    Icon =Application.Current.Resources["menuGearGrey"] as ImageSource,
                    IconHover =Application.Current.Resources["menuGearGreen"] as ImageSource
                },
                
                    new ContextAction()
                    {
                        Action =ShareItemCommand,
                        Name ="Share",
                        Icon =Application.Current.Resources["menuPeopleGrey"] as ImageSource,
                        IconHover =Application.Current.Resources["menuPeopleGreen"] as ImageSource
                    },
                    new ContextAction()
                    {
                        Action =DeleteItemCommand,
                        Name =Application.Current.FindResource("Delete") as string,
                        Icon =Application.Current.Resources["menuTrashGrey"] as ImageSource,
                        IconHover =Application.Current.Resources["menuTrashGreen"] as ImageSource
                    }

            };

        }

        public virtual SecureItem CreateSecureItem()
        {
            SecureItem secureItem = new SecureItem()
            {
                Id = Id,
                SecureItemType = Type,
                Type = SubType,
                Folder = Folder,
                Name = Name,
                Color = Background.ToString(),
                Data = new SecureItemData()
                {
                    notes =  Note
                }
                
            };

            return secureItem;
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
            if (GearButton_Clicked != null)
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
            ShowItemMenu = false;
            if (DeletingIcon_Clicked != null)
                DeletingIcon_Clicked(this, null);            
        }

        private void AddNewFolderClicked(object obj)
        {
            if (AddNewFolder_Clicked != null)
            {
                var args = new SecureItemRoutedEventArgs(this.id);
                args.ItemId = this.id;
                AddNewFolder_Clicked(this, args);
            }
        }

        private void EditItemClicked(object obj)
        {
            ShowItemMenu = false;
            if (Edit_Clicked != null)
                Edit_Clicked(this, null);

        }


        private void FavoritesIconClicked(object obj)
        {           
            if (FavoritesIcon_Clicked != null)
                FavoritesIcon_Clicked(this, null); 
        }

        private void AddNoteExcute(object obj)
        {
            HasNote = true;
        }

        private void NoteLostTextBoxFocus()
        {
            if (string.IsNullOrEmpty(note))
                HasNote = false;
        }

        #region Validation
      
        

        public virtual bool Validate()
        {

            return IsValidName = !string.IsNullOrEmpty(Name);
        }


        #endregion

    }

    public class SecureItemWithPasswordViewModel : SecureItemViewModel
    {
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                RaisePropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        private bool hasPasswordHistory = true;
        public bool HasPasswordHistory
        {
            get
            {
                return hasPasswordHistory;
            }
            set
            {
                hasPasswordHistory = value;
                RaisePropertyChanged("HasPasswordHistory");
            }
        }

        private bool showCopyPassword;
        public bool ShowCopyPassword
        {
            get
            {
                return showCopyPassword;
            }
            set
            {
                showCopyPassword = value;
                RaisePropertyChanged("ShowCopyPassword");
            }
        }


        public SecureItemWithPasswordViewModel() : base()
        {
            InitCommand();
            HasPasswordHistory = false;

        }
        public SecureItemWithPasswordViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item,defaultColor,defaultIcon)
        {

            ListViewSecondName=Username = item.Data.username;
            Password = item.Data.password;
            ShowCopyPassword = true;
            InitCommand();
        }

        private void InitCommand()
        {
            if (Actions != null && Actions.Count>1)
            {
                var copyPassword = new ContextAction()
                {
                    Action = EditItemCommand,
                    Name = Application.Current.FindResource("CopyPassword") as string,
                    Icon = Application.Current.Resources["menuPasswordGrey"] as ImageSource,
                    IconHover = Application.Current.Resources["menuPasswordGreen"] as ImageSource
                };

                var copyUserName = new ContextAction()
                {
                    Action = EditItemCommand,
                    Name = Application.Current.FindResource("MenuCopyUsername") as string,
                    Icon = Application.Current.Resources["menuPersonGrey"] as ImageSource,
                    IconHover = Application.Current.Resources["menuPersonGreen"] as ImageSource
                };
                Actions.Insert(1, copyUserName);
                Actions.Insert(1, copyPassword);
            }
              
                   

        }

    }

    public class SecureItemWithCountryViewModel : SecureItemViewModel
    {
        private ObservableCollection<KeyValuePair<string, string>> _countries;
        public ObservableCollection<KeyValuePair<string, string>> Countries
        {
            get
            {
                return _countries;
            }
            set
            {
                _countries = value;
                RaisePropertyChanged("Countries");
            }
        }

        private KeyValuePair<string, string>? selectedCountry;
        public KeyValuePair<string, string>? SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                if (value.HasValue && selectedCountry.HasValue)
                {
                    if (selectedCountry.Value.Key != value.Value.Key)
                    {
                        selectedCountry = value;
                        RaisePropertyChanged("SelectedCountry");
                    }
                }
                else
                {
                    selectedCountry = value;
                    RaisePropertyChanged("SelectedCountry");
                }
            }
        }

        public SecureItemWithCountryViewModel() : base()
        {
        }

        public SecureItemWithCountryViewModel(SecureItem item, Brush defaultColor, ImageSource defaultIcon) : base(item, defaultColor, defaultIcon)
        {

        }
      
    }


}
