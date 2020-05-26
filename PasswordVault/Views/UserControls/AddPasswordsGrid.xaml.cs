using PasswordBoss.DTO;
using PasswordBoss.Helpers;
using PasswordBoss.ViewModel;
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
    /// Interaction logic for AddPasswordsGrid.xaml
    /// </summary>
    public partial class AddPasswordsGrid
    {
        private IResolver resolver;
        public AddPasswordsGrid(IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
     
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NeverShowCheckBox.IsChecked.Value)
            {
                IPBData pbData = resolver.GetInstanceOf<IPBData>();
                Configuration configDontShowInfoDialog = new Configuration()
                {
                    AccountEmail = pbData.ActiveUser,
                    Key = DefaultProperties.Configuration_Key_PasswordVaultInfo,
                    Value = true.ToString()
                };
                pbData.AddOrUpdateConfiguration(configDontShowInfoDialog);
            }
            this.Close();
        }
    }
}
