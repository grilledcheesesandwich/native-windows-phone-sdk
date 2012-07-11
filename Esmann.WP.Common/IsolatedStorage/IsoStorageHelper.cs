using System.IO;
using System.IO.IsolatedStorage;

namespace Esmann.WP.Common.IsolatedStorage
{
    public class IsoStorageHelper
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

        public void CreateDirectory(string foldername)
        {
            if (foldername.Contains(Path.DirectorySeparatorChar.ToString()))
            {
                foldername = foldername.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            if (foldername.Contains(Path.AltDirectorySeparatorChar.ToString()))
            {
                var path = foldername;// Path.GetFullPath(foldername).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string partialPath = "";
                var paths = path.Split(Path.AltDirectorySeparatorChar);
                foreach (var directory in paths)
                {
                    if (string.IsNullOrEmpty(directory))
                    {
                        continue;
                    }
                    if (directory.Contains("."))
                    {
                        break;
                    }
                    string nextPath = string.IsNullOrWhiteSpace(partialPath) ? directory : partialPath + Path.AltDirectorySeparatorChar + directory;

                    if (isoStore.DirectoryExists(nextPath))
                    {
                        partialPath = nextPath;
                        continue;
                    }
                    isoStore.CreateDirectory(nextPath);
                    partialPath = nextPath;
                }
            }
        }

        private void DeleteFile(string filename)
        {
            if (!isoStore.FileExists(filename))
            {
                return;
            }
            isoStore.DeleteFile(filename);
        }

        private void DeleteFiles(string foldername)
        {
            if (!isoStore.DirectoryExists(foldername))
            {
                return;
            }
            foreach (var file in isoStore.GetFileNames(foldername + Path.AltDirectorySeparatorChar + "*"))
            {
                isoStore.DeleteFile(foldername + Path.AltDirectorySeparatorChar + file);
            }
        }

        public void DeleteDiretory(string foldername)
        {
            if (!isoStore.DirectoryExists(foldername))
            {
                return;
            }
            DeleteFiles(foldername);

            foreach (var diretory in isoStore.GetDirectoryNames(foldername + Path.AltDirectorySeparatorChar + "*"))
            {
                var childFolder = foldername + Path.AltDirectorySeparatorChar + diretory;
                DeleteFiles(childFolder);
                DeleteDiretory(childFolder);
            }
            isoStore.DeleteDirectory(foldername);
        }

        public bool FileExists(string filename)
        {
            return isoStore.FileExists(filename);
        }

        public IsolatedStorageFileStream CreateFile(string filename)
        {
            if (!FileExists(filename))
            {
                var directory = Path.GetDirectoryName(filename);
                CreateDirectory(directory);
                return isoStore.CreateFile(filename);
            }
            return null;
        }

        public IsolatedStorageFileStream OpenFile(string filename, FileMode mode)
        {
            if (isoStore.FileExists(filename))
            {
                return isoStore.OpenFile(filename, mode);
            }
            else
            {
                return null;
            }
        }
    }
}
