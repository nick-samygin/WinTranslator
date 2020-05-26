using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PasswordBoss.ViewModel.Search
{
    public class SearchResultItemModel : ViewModelBase
    {
        private static readonly ILogger logger = Logger.GetLogger(typeof(SearchResultItemModel));
        
        IPBData pbData = null;
        IResolver resolver = null;
        public SecureItemSearchResult SecureItem { get; set; }
        public RelayCommand OpenOptionsCommand { get; set; }
        public RelayCommand OpenCommand { get; set; }

       //TODO: Add image here
        private string m_Header;
        public string Header
        {
            get { return m_Header; }
            set
            {
                m_Header = value;
                RaisePropertyChanged("Header");
            }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string m_Value;
        public string Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                RaisePropertyChanged("Value");
            }
        }

        private string m_Image;
        public string Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                RaisePropertyChanged("Image");
            }
        }

        public SearchResultItemModel(IPBData pbData, IResolver resolver)
        {
            this.resolver = resolver;
            this.pbData = pbData;
            OpenOptionsCommand = new RelayCommand(OpenOptionsClick);
            OpenCommand = new RelayCommand(OpenClick);
        }


        public void OpenOptionsClick(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var dictionary = new Dictionary<string, object> { { "id", SecureItem.Id } };
                    ((IAppCommand)System.Windows.Application.Current).ExecuteCommand("ShowSecureItemEditor", dictionary);
                }
            }

            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void OpenClick(object obj)
        {
            try
            {
                var secureItem = pbData.GetSecureItemById(SecureItem.Id);
                if (SecureItem.Type == SecurityItemsDefaultProperties.SecurityItemSubType_PV_Login)
                {
                    if(secureItem!= null && secureItem.Site != null
                        && secureItem.Site.Uri != null)
                    {
                        BrowserHelper.OpenInDefaultBrowser(new Uri(secureItem.Site.Uri, UriKind.RelativeOrAbsolute));
                    }
                    
                }
                else
                {
                    OpenOptionsClick(obj);
                }
           
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

    }
}
