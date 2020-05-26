using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PasswordBoss.ViewModel;
using SecureItemsCommon.ViewModels;

namespace PasswordBoss.Converters
{
    class ItemShareTypeToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0] as SecureItemViewModel;
            var currentType = (AddShareItemType) values[1];
            var searchString = (string) values[2];
            if (!string.IsNullOrEmpty(searchString))
                return item.Name.IndexOf(searchString, 0, StringComparison.CurrentCultureIgnoreCase) != -1 ? Visibility.Visible : Visibility.Collapsed;
            
            switch (currentType)
            {
                case AddShareItemType.Passwords:
                {
                    if (item is SecureItemWithPasswordViewModel || item is SSHKeySecureItemViewModel)
                        return Visibility.Visible;
                }
                break;
                case AddShareItemType.DigitalWallet:
                {
                    if (item is BankAccountItemViewModel || item is CreditCardItemViewModel)
                        return Visibility.Visible;
                }
                break;
                case AddShareItemType.PersonalInfo:
                {
                    if (item is SecureItemWithCountryViewModel || item is CompanySecureItemViewModel ||
                        item is EmailSecureItemViewModel || item is NameSecureItemViewModel)
                        return Visibility.Visible;
                }
                break;

                case AddShareItemType.SecureNotes:
                case AddShareItemType.None:
                default:
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
