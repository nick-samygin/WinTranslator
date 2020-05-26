using PasswordBoss.DTO;
using PasswordBoss.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PasswordBoss.Views
{
    /// <summary>
    /// Interaction logic for AddFolder.xaml
    /// </summary>
    public partial class AddFolder : CustomChildWindow
    {
        public string FolderName { get; set; }

        public Folder ParentFolder { get; set; }

        public AddFolder()
        {
           
            InitializeComponent();
            this.Closing += AddFolder_Closing;
        }

        private void AddFolder_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FolderName = txtFolderName.Text.Trim();
            this.Closing -= AddFolder_Closing;
        }

        public AddFolder(IEnumerable<Folder> foldersList, Folder currentFolder =null)
        {

            InitializeComponent();
            FoldersComboBox.ItemsSource = foldersList;
            this.Closing += AddFolder_Closing;
            if (currentFolder != null)
            {
                this.txtTitel.Text = Application.Current.FindResource("FolderEditFolder") as string;

                txtFolderName.Text = currentFolder.Name;
                if (currentFolder.ParentId != null)
                    FoldersComboBox.SelectedItem = foldersList.FirstOrDefault(x => x.Id == currentFolder.ParentId);
            }

        }

        private void FoldersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FoldersComboBox.SelectedItem != null)
            {
                var parentFolder = FoldersComboBox.SelectedItem as Folder;
                if (parentFolder != null)
                    ParentFolder = parentFolder;
            }

        }
    }
}
