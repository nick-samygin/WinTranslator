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
    /// Interaction logic for ShareCenterMiniTour.xaml
    /// </summary>
    public partial class ShareCenterMiniTour
    {
        private IResolver resolver;
        public ShareCenterMiniTour(IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
            this.DataContext = new ShareCenterTourViewModel();
        }

        private void btnFavoriteSitesNext_Click(object sender, RoutedEventArgs e)
        {
            if (resolver != null)
            {
                IPBData pbData = resolver.GetInstanceOf<IPBData>();
                pbData.AddOrUpdateConfiguration(new Configuration() { AccountEmail = pbData.ActiveUser, Key = "ShowShareCenterMiniTour", Value = false.ToString() });
            }
            this.Close();
        }
    }
}
