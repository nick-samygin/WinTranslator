using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordBoss
{
    /// <summary>
    /// Interaction logic for MasterPwdBox.xaml
    /// </summary>
    public partial class FirefoxMasterPasswordConfirm : Window, INotifyPropertyChanged
    {

        public string Password
        {
            get
            {
                return PwdBox.Password;
            }
        }
        public FirefoxMasterPasswordConfirm()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public void btnOk_Click(object sender, RoutedEventArgs e)
        {
            
                this.DialogResult = true;
                this.Hide();
            
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }

    

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;


    }
}
