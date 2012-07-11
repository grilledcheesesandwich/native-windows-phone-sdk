using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;
using Esmann.WP.Common.Location;
using EtaSampleApp.ViewModels;

namespace EtaSampleApp.Views
{
    public partial class LocationUserControl : UserControl
    {
        BingServices.LocationsAPIHelper bingHelper = new BingServices.LocationsAPIHelper();

        public LocationUserControl()
        {
            InitializeComponent();
            InitializeGPS();
        }

        private async void InitializeGPS()
        {
            if (App.ViewModel.UserViewModel.AllowGPS)
            {
                var gps = new GPSHelper();
                var result = await gps.GetPositionAsync();
                if (result != null)
                {
                    App.ViewModel.UserViewModel.Location = 
                        new Location() { 
                            IsGeoCoded = true, 
                            IsValid = true,
                            Latitude = result.Latitude,
                            Longitude = result.Longitude
                        };
                }
            }
            
        }

        public string ZipCode { get { return inputFiled.Text; } }
        public event EventHandler<RoutedEventArgs> Click;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                Click(this, e);
            }
        }

        async private void inputFiled_ActionIconTapped(object sender, System.EventArgs e)
        {
            btnNext.Visibility = System.Windows.Visibility.Collapsed;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            App.ViewModel.UserViewModel.AllowGPS = true;
            var gps = new GPSHelper();
            var result = await gps.GetPositionAsync();
            if (result != null)
            {
                int zipHelper = 0;
                var zip = await bingHelper.GeoCoordinateToZipCodeAsync(result.Latitude, result.Longitude);
                inputFiled.Text = zip;

                App.ViewModel.UserViewModel.Location =
                    new Location()
                    {
                        IsGeoCoded = true,
                        IsValid = true,
                        Latitude = result.Latitude,
                        Longitude = result.Longitude,
                        Accuracy = 0,
                        ZipCode = int.TryParse(zip,out zipHelper) ? zipHelper : 0
                    };

                inputFiled.SelectionStart = inputFiled.Text.Length;

            }
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            btnNext.Visibility = System.Windows.Visibility.Visible;
        }

        char[] validDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private void inputFiled_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = (sender as TextBox);
            var text = textBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var ch = text.Last();
            if (!validDigits.Contains(ch))
            {
                text = text.Remove(text.Length - 1, 1);
                textBox.Text = text;
                textBox.SelectionStart = text.Length;
            }            
        }

        async private void inputFiled_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            int test = 0;
            if (textBox.Text.Length == 4 && int.TryParse(textBox.Text, out test))
            {
                progressBar.Visibility = System.Windows.Visibility.Visible;
                var result = await bingHelper.ZipCodeToGeoCoordinateAsync(test.ToString());
                // ok
                 App.ViewModel.UserViewModel.Location =
                    new Location()
                    {
                        IsGeoCoded = true,
                        IsValid = true,
                        Latitude = result.Latitude,
                        Longitude = result.Longitude,
                        Accuracy = 0,
                        ZipCode = test
                    };

                if (btnNext.Visibility != System.Windows.Visibility.Visible)
                {
                    btnNext.Visibility = System.Windows.Visibility.Visible;
                }
                progressBar.Visibility = System.Windows.Visibility.Collapsed;

            }
            else
            {
                progressBar.Visibility = System.Windows.Visibility.Collapsed;

                if (btnNext.Visibility != System.Windows.Visibility.Collapsed)
                {
                    btnNext.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

    }
}
