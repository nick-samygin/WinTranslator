using PasswordBoss.DTO;
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

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for PersonalInfoMiniTour.xaml
    /// </summary>
    public partial class PersonalInfoMiniTour
        {
        private IResolver resolver;
        public PersonalInfoMiniTour(IResolver resolver)
        {
            InitializeComponent();
            this.resolver = resolver;
        }

        private void btnNewItemPopupNext_Click(object sender, RoutedEventArgs e)
        {
            if (resolver != null)
            {
                IPBData pbData = resolver.GetInstanceOf<IPBData>();
                pbData.AddOrUpdateConfiguration(new Configuration() { AccountEmail = pbData.ActiveUser, Key = "ShowPersonalInfoMiniTour", Value = false.ToString() });
            }
            this.Close();
        }
    }
}
