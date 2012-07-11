using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace Esmann.WP.Common.IsolatedStorage
{
    public class ImageHelper
    {
        public BitmapImage ConvertIsoFileStreamToBitmapImage(string filename)
        {
            if (filename == null || string.IsNullOrWhiteSpace(filename))
            {
                return null;
            }
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (!isoStore.FileExists(filename))
                {
                    return null;
                }
                BitmapImage image;

                using (var stream = isoStore.OpenFile(filename, System.IO.FileMode.Open))
                {
                    image = ConvertIsoFileStreamToBitmapImage(stream);
                }

                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Image ex: " + ex.Message);
                return null;
            }
        }

        public BitmapImage ConvertIsoFileStreamToBitmapImage(IsolatedStorageFileStream isoFileStream)
        {
            if (isoFileStream == null || isoFileStream.Length == 0)
            {
                return null;
            }
            try
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(isoFileStream);
                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Image ex: " + ex.Message);
                return null;
            }
        }
    }
}
