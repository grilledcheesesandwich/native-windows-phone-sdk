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
using esmann.WP.Common.ViewModels;
using Esmann.WP.Common.AppSettings;
using System.ComponentModel;
using BingServices;
using Esmann.WP.Common.Location;

namespace EtaSampleApp.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        const string SettingsKeyName = "UserViewModel";

        [Obsolete("Use GetUserViewModel", true)]
        public UserViewModel()
        {

        }

        private UserViewModel(object dummy)
        {

        }

        private static UserViewModel instance;
        public static UserViewModel GetUserViewModel
        {
            get
            {
                if (Application.Current.RootVisual != null && DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                {
                    return new UserViewModel(null);
                }

                if (instance == null)
                {
                    instance = Load();
                }
                if (instance.AllowGPS)
                {
                    instance.UpdateLocationInformationGPS();
                }
                return instance;
            }
        }

        private static UserViewModel Load()
        {
            var settings = new AppSettingsHelper();
            return settings.GetValueOrNew<UserViewModel>(SettingsKeyName);
        }

        public static bool Save()
        {
            
            if (instance != null)
            {
                if (instance.firstTimeApplicationRuns)
                {
                    instance.firstTimeApplicationRuns = false;
                }
                var settings = new AppSettingsHelper();
                return settings.SetValue(SettingsKeyName, instance);
            }
            return false;
        }

        bool isUpdateingLocation = false;
        public bool IsUpdateingLocation
        {
            get { return isUpdateingLocation; }
            set
            {
                if (value != isUpdateingLocation)
                {
                    isUpdateingLocation = value;
                    NotifyPropertyChanged(() => IsUpdateingLocation);
                }
            }
        }
        public void UpdateLocationInformationGPS(){
            if (!IsUpdateingLocation)
            {
                IsUpdateingLocation = true;
                UpdateLocationInformationGPSInternal();
            }
        }
        private async void UpdateLocationInformationGPSInternal()
        {
            var gps = new GPSHelper();
            var result = await gps.GetPositionAsync();
            if (result != null)
            {
                Location =
                    new Location()
                    {
                        IsGeoCoded = true,
                        IsValid = true,
                        Latitude = result.Latitude,
                        Longitude = result.Longitude,
                        Timestamp = DateTime.Now.Ticks
                    };
                var locationAPI = new LocationsAPIHelper();
                var address = await locationAPI.GeoCoordinateToZipCodeAsync(result.Latitude, result.Longitude);
                int zipCode = 0;

                if (!string.IsNullOrWhiteSpace(address) && int.TryParse(address, out zipCode))
                {
                    ZipCode = zipCode;
                }
                else
                {
                    ZipCode = 0;
                    MessageBox.Show("Kunne ikke matche din lokation med et postnummer.", "Postnummer", MessageBoxButton.OK);
                }
                IsUpdateingLocation = false;
            }
        }

        public void UpdateLocationInformationZipCode(string zip)
        {
            if (!IsUpdateingLocation)
            {
                IsUpdateingLocation = true;
                UpdateLocationZipCodeInternal(zip);
            }
        }

        private async void UpdateLocationZipCodeInternal(string zip)
        {
            var location = new BingServices.LocationsAPIHelper();
            var result = await location.ZipCodeToGeoCoordinateAsync(zip);
            if (result != null)
            {
                Location = new ViewModels.Location
                {
                    IsGeoCoded = false,
                    IsValid = true,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    Timestamp = DateTime.Now.Ticks
                };
            }
            else
            {
                Location = new ViewModels.Location
                {
                    IsGeoCoded = false,
                    IsValid = false,
                    Latitude = 0,
                    Longitude = 0,
                    Timestamp = DateTime.Now.Ticks
                };
                MessageBox.Show("Kunne ikke genkende postnummeret.", "Postnummer", MessageBoxButton.OK);
            }
            IsUpdateingLocation = false;
        }

        bool firstTimeApplicationRuns = true;
        public bool FirstTimeApplicationRuns
        {
            get { return firstTimeApplicationRuns; }
            set
            {
                if (value != firstTimeApplicationRuns)
                {
                    firstTimeApplicationRuns = value;
                    NotifyPropertyChanged(() => FirstTimeApplicationRuns);
                }
            }
        }

        bool allowGPS = true;
        public bool AllowGPS
        {
            get { return allowGPS; }
            set
            {
                if (value != allowGPS)
                {
                    allowGPS = value;
                    NotifyPropertyChanged(() => AllowGPS);
                }
            }
        }

        int zipCode = -1;
        public int ZipCode
        {
            get { return zipCode; }
            set
            {
                if (value != zipCode)
                {
                    zipCode = value;
                    NotifyPropertyChanged(() => ZipCode);
                }
            }
        }

        int distance = 10000;
        public int Distance
        {
            get { return distance; }
            set
            {
                if (value != distance)
                {
                    distance = value;
                    NotifyPropertyChanged(() => Distance);
                }
            }
        }

        Location location = null;
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged(() => Location);
                }
            }
        }
    }

    public class Location : ViewModelBase
    {
        long timestamp = 0;
        public long Timestamp
        {
            get { return timestamp; }
            set
            {
                if (value != timestamp)
                {
                    timestamp = value;
                    NotifyPropertyChanged(() => Timestamp);
                }
            }
        }

        double latitude = 0;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value != latitude)
                {
                    latitude = value;
                    NotifyPropertyChanged(() => Latitude);
                }
            }
        }

        int accuracy = int.MaxValue ;
        public int Accuracy
        {
            get { return accuracy; }
            set
            {
                if (value != accuracy)
                {
                    accuracy = value;
                    NotifyPropertyChanged(() => Accuracy);
                }
            }
        }

        double longitude = 0;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value != longitude)
                {
                    longitude = value;
                    NotifyPropertyChanged(() => Longitude);
                }
            }
        }

        bool isGeoCoded = true;
        public bool IsGeoCoded
        {
            get { return isGeoCoded; }
            set
            {
                if (value != isGeoCoded)
                {
                    isGeoCoded = value;
                    NotifyPropertyChanged(() => IsGeoCoded);
                }
            }
        }

        bool isValid = false;
        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    NotifyPropertyChanged(() => IsValid);
                }
            }
        }
    }
}
