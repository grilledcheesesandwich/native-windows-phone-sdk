﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace EtaSampleApp.Helpers
{
    public class IsoFileToBitmapImageConverter : IValueConverter
    {
        private object syncContext = new object();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                

                string isoFilename = value.ToString();
                BitmapImage image = new BitmapImage();

                lock (syncContext)
                {
                    if (!isoStore.FileExists(isoFilename))
                    {
                        return value;
                    }
                    using (var stream = isoStore.OpenFile(isoFilename, System.IO.FileMode.Open))
                    {
                        image.SetSource(stream);
                    }
                }
                
                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Image ex: " + ex.Message);
                MessageBox.Show("UPS! image Load ex: " + ex.Message + "path: "+ value);
                return value.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool helper = (bool)value;
            var result = helper ? Visibility.Visible : Visibility.Collapsed;
            //Debug.Assert(result != (Visibility)value);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityN : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool helper = !(bool)value;
            var result = helper ? Visibility.Visible : Visibility.Collapsed;
            //Debug.Assert(result != (Visibility)value);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }

    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool helper = (bool)value;
            return !helper;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CatalogUriTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string template = value.ToString();
            template = template.Replace("%d", "{0}");
            return string.Format(template,1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
