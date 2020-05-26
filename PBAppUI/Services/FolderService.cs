using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.DTO;
using PasswordBoss.Views;

namespace PasswordBoss.Services
{
    public class FolderService : IFolderService
    {
        private IPBData pbData;

        private IEnumerable<Folder> GetFoldersCollection()
        {
            var result = new List<Folder>() { new Folder() { Name = string.Empty } };
            result.AddRange(pbData.GetFoldersBySecureItemType());          

            return result;
        }
        public string SelectedFolderId { get; set; }

        public FolderService(IPBData _pbData)
        {
            pbData = _pbData;
        }

    

        public string AddFolder()
        {
            AddFolder cb = new AddFolder(GetFoldersCollection());

            bool? dialogResult = cb.ShowDialog();
            if (dialogResult.Value)
            {
                //save category
                if (cb.ParentFolder != null)
                    return pbData.AddFolder(cb.FolderName, false, cb.ParentFolder.Id);
                else
                    return pbData.AddFolder(cb.FolderName, false); 

            }

             return null;
        }

        public bool UpdateFolder(Folder folder)
        {
            if (folder == null)
                return false;
            AddFolder cb = new AddFolder(GetFoldersCollection(), folder);

            bool? dialogResult = cb.ShowDialog();
            if (dialogResult.HasValue)
                if (dialogResult.Value)
                {
                    folder.Name = cb.FolderName;

                    folder.ParentId = cb.ParentFolder == null ? string.Empty : cb.ParentFolder.Id;
                    //folder.UseSecureBrowser = cb.UseSecureBrowser;

                    //save category
                    pbData.UpdateFolder(folder);
                    return true;
                }

               return false;
        }
    }
}
