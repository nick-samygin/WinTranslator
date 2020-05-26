using PasswordBoss;
using PasswordBoss.Helpers;
using PasswordBoss.UserControls;
using PasswordBoss.ViewModel;
using SecureItemsCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SecureNotes.ViewModel
{
    public class SecureNotesViewModel : ViewModelBase
    {
        private IResolver resolver = null;
        private IPBData pbData = null;

        public SecureNotesViewModel(IResolver rs)
        {
            resolver = rs;
            pbData = resolver.GetInstanceOf<IPBData>();
        }

        public void AddNewItem(ISecureItemVM secureItem)
        {
            
            //if (secureItem is SecureItemWithCountryViewModel)
            //{
            //    ((SecureItemWithCountryViewModel)secureItem).Countries = new ObservableCollection<KeyValuePair<string, string>>(pbData.GetCountries());

            //    string country = pbData.GetPrivateSetting(DefaultProperties.Settings_Country);
            //    if (String.IsNullOrEmpty(country))
            //    {
            //        pbData.ChangePrivateSetting(DefaultProperties.Settings_Country, DefaultProperties.Settings_DefaultCountryCode);
            //        country = DefaultProperties.Settings_DefaultCountryCode;
            //    }
            //     ((SecureItemWithCountryViewModel)secureItem).SelectedCountry = ((SecureItemWithCountryViewModel)secureItem).Countries.FirstOrDefault(x => x.Key == country);
            //}



            //AddSecureItemWindow addWindow = new AddSecureItemWindow(Application.Current.Resources["SNSecureItemTemplateSelector"] as DataTemplateSelector) { Title = secureItem.ItemTitel };
            //addWindow.DataContext = secureItem;
            //bool? dialogResult = addWindow.ShowDialog();
            //if (dialogResult.Value)
            //{

            //}

        }

    }
}
