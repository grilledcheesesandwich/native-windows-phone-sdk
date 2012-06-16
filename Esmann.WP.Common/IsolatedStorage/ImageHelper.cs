using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Diagnostics;

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
