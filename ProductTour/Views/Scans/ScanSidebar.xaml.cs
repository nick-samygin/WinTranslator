using System;
using System.Reflection;
using System.Windows.Controls;

namespace ProductTour.Views.Scans
{
    public partial class ScanSidebar : UserControl
    {
        public ScanSidebar()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                RadioButton ck = sender as RadioButton;
                if (ck.IsChecked.Value)
                {
                    Type thisType = DataContext.GetType();
                    MethodInfo theMethod = thisType.GetMethod("OnMenuChecked");
                    theMethod.Invoke(DataContext, new object[] { ck.Name });

                }
            }
            catch
            { }
        }
    }
}