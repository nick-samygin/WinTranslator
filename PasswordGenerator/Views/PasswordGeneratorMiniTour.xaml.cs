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
using PasswordBoss.ViewModel;
using PasswordBoss.DTO;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for PasswordGeneratorMiniTour.xaml
    /// </summary>
    public partial class PasswordGeneratorMiniTour
    {
        private IResolver resolver;
        public PasswordGeneratorMiniTour(IResolver resolver)
        {
            InitializeComponent();
            this.DataContext = new PasswordGeneratorTourViewModel();
            this.resolver = resolver;
        }

        private void btnSafelyStorInfoPopupNext_Click(object sender, RoutedEventArgs e)
        {
            if (resolver != null)
            {
                IPBData pbData = resolver.GetInstanceOf<IPBData>();
                pbData.AddOrUpdateConfiguration(new Configuration() { AccountEmail = pbData.ActiveUser, Key = "ShowPasswordGeneratorMiniTour", Value = false.ToString() });
            }
            this.Close();
        }
    }
}
