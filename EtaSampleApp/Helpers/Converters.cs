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
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using EtaSDK.ApiModels;

namespace EtaSampleApp.Helpers
{
    public class NegativeNumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var helper = (int)value;
            if (helper < 0)
            {
                return "";
            }
            else
            {
                return helper.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result = -1;

            if (double.TryParse(value.ToString(),out result))
            {
                return (int)result;
            }
            return -1;
        }
    }

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

    public class OfferToPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var offer = value as Offer;
            if (offer == null)
            {
                return "0";
            }
            if (!string.IsNullOrWhiteSpace(offer.Preprice))
            {
                double preprice, price, discount;
                if (double.TryParse(offer.Preprice, out preprice) && double.TryParse(offer.Price, out price))
                {
                    discount = preprice - price;
                    return string.Format("{0},-, før {1},- (spar {2})", offer.Price, offer.Preprice,discount);

                }
                else
                {
                    return string.Format("{0},-, før {1},-", offer.Price, offer.Preprice);
                }
            }
            else
            {
                return string.Format("{0},-.", offer.Price);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
