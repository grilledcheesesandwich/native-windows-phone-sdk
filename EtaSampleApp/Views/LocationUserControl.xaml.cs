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


    }
}
