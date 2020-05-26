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

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for SecurityScoreContentPanel.xaml
    /// </summary>
    public partial class SecurityScoreContentPanel : UserControl
    {
        public SecurityScoreContentPanel(IResolver resolver)
        {
            InitializeComponent();
            this.DataContext = new SecurityScoreViewModel(resolver);
            //this.SecurityScoreExpander.IsKeyboardFocusWithinChanged += SecurityScoreExpander_IsKeyboardFocusWithinChanged;
        }

        private void PasswordTextUC_Loaded(object sender, RoutedEventArgs e)
        {
                   }

        //void SecurityScoreExpander_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((bool)e.OldValue == true && (bool)e.NewValue == false)
        //    {
        //        SecurityScoreExpander.IsExpanded = false;
        //    }
        //}

    }
}
